using Crossdyne.Security.Abstractions;
using EnigmaVault.Web.Bff.Constants;
using EnigmaVault.Web.Bff.Extensions;
using EnigmaVault.Web.Bff.Models;
using EnigmaVault.Web.Bff.Services;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.StackExchangeRedis;
using Shared.Contracts.AuthenticationService.Clients;
using Shared.Contracts.AuthenticationService.Requests;
using Shared.Contracts.Common.Cache.Abstractions;
using StackExchange.Redis;

namespace EnigmaVault.Web.Bff.Extensions
{
    public static class AuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedCryptoKeyForDecryptCookie(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataProtection().SetApplicationName("Crossdyne.SharedBff");

            services.AddOptions<KeyManagementOptions>().Configure<IServiceProvider>((options, sp) =>
            {
                var redis = sp.GetRequiredService<IConnectionMultiplexer>();

                options.XmlRepository = new RedisXmlRepository(() => redis.GetDatabase(), RedisKeyExtensions.DataProtectionKeys());
            });

            return services;
        }

        public static IServiceCollection AddDistributedLock(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
                return new RedisDistributedSynchronizationProvider(multiplexer.GetDatabase());
            });

            return services;
        }

        public static IServiceCollection UseCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowTrustedFrontend", policy =>
                {
                    policy.WithOrigins("http://127.0.0.1:4202", "https://vault.crossdyne.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            return services;
        }

        public static IServiceCollection AddCookie(this IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "Crossdyne";
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.Cookie.HttpOnly = true;
                options.SlidingExpiration = true;

                if (environment.IsDevelopment())
                {
                    options.Cookie.Domain = "127.0.0.1"; 
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; 
                    options.Cookie.SameSite = SameSiteMode.Lax;
                }
                else
                {
                    options.Cookie.Domain = ".crossdyne.com"; 
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                }

                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };

                options.Events.OnValidatePrincipal = async context =>
                {
                    var sessionId = context.Principal?.FindFirst("SessionId")?.Value;

                    if (string.IsNullOrWhiteSpace(sessionId))
                    {
                        context.RejectPrincipal();
                        return;
                    }

                    var cacheSessionKey = RedisKeyExtensions.SessionKey(sessionId!);
                    var cache = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
                    var cryptoService = context.HttpContext.RequestServices.GetRequiredService<ICryptoService>();
                    var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                    var key = Convert.FromBase64String(configuration.GetValue<string>(ConfigurationConstants.RedisDataEncryptionKey) ?? throw new InvalidOperationException($"{ConfigurationConstants.RedisDataEncryptionKey} не настроен"));

                    var session = await cache.GetJsonAsync<UserSession>(cacheSessionKey);

                    if (session == null)
                    {
                        context.RejectPrincipal();
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(session.EncryptedAccessToken) || string.IsNullOrWhiteSpace(session.EncryptedRefreshToken))
                    {
                        await cache.RemoveAsync(cacheSessionKey);
                        await cache.SetRemoveAsync(RedisKeyExtensions.UserSessionsKey(session.UserId), sessionId);
                        context.RejectPrincipal();
                        return;
                    }

                    if (session.AccessTokenExpiresAt <= DateTime.UtcNow.AddMinutes(1))
                    {
                        var lockProvider = context.HttpContext.RequestServices.GetRequiredService<RedisDistributedSynchronizationProvider>();
                        var lockKey = RedisKeyExtensions.DistributedLock(sessionId);

                        await using var handle = await lockProvider.TryAcquireLockAsync(lockKey, timeout: TimeSpan.FromSeconds(5));

                        if (handle == null)
                        {
                            context.RejectPrincipal();
                            return;
                        }

                        session = await cache.GetJsonAsync<UserSession>(cacheSessionKey);

                        if (session == null)
                        {
                            context.RejectPrincipal();
                            return;
                        }

                        if (session.AccessTokenExpiresAt <= DateTime.UtcNow.AddMinutes(1))
                        {
                            var authClient = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();

                            var refreshResult = await authClient.RefreshTokens(new LoginByTokenRequest(cryptoService.DecryptData<string>(session.EncryptedRefreshToken, key)!));
                            
                            if (refreshResult.IsSuccess)
                            {
                                var jwtReader = context.HttpContext.RequestServices.GetRequiredService<IJwtReadService>();
                                var jwtData = jwtReader.ExtractData(refreshResult.Value!.AccessToken);

                                session.EncryptedAccessToken = cryptoService.EncryptData(refreshResult.Value.AccessToken, key, CryptoConstants.CryptoVersion);
                                session.EncryptedRefreshToken = cryptoService.EncryptData(refreshResult.Value.RefreshToken, key, CryptoConstants.CryptoVersion);
                                session.AccessTokenExpiresAt = jwtData.ExpiredTime;

                                await cache.SetJsonAsync(cacheSessionKey, session, TimeSpan.FromDays(30));
                            }
                            else
                            {
                                await cache.RemoveAsync(cacheSessionKey);
                                await cache.SetRemoveAsync(RedisKeyExtensions.UserSessionsKey(session.UserId), sessionId);
                                context.RejectPrincipal();
                                return;
                            }
                        }
                    }
  
                    context.HttpContext.Items["AccessToken"] = cryptoService.DecryptData<string>(session.EncryptedAccessToken, key);
                };
            });

            return services;
        }
    }
}