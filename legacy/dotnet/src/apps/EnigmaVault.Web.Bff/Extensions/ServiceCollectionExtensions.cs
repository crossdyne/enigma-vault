using Crossdyne.Security.Cryptography;
using EnigmaVault.Web.Bff.Services;
using Shared.Redis;

namespace EnigmaVault.Web.Bff.Extensions
{
 public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJwtReadService, JwtReadService>();
            services.AddCashService(configuration);
            services.AddCrossdyneCryptography();

            return services;
        }
    }
}