using EnigmaVault.AssetsService.Client;
using EnigmaVault.Authentication.Client;
using EnigmaVault.Desktop.Handlers;
using EnigmaVault.FileService.Client;
using EnigmaVault.SecretService.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.AssetsService.Clients;
using Shared.Contracts.AuthenticationService.Clients;
using Shared.Contracts.FileService.Clients;
using Shared.Contracts.SecretService.Clients;
using Shared.Contracts.UserManagementService.Clients;

namespace EnigmaVault.Desktop.Ioc
{
    public static class RegisterHttpClients
    {
        public static IServiceCollection AddHttpServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<RefreshTokenHandler>();

            string? authServiceApiUrl = configuration.GetValue<string>("BaseAuthServiceUrl");
            const string authServiceApiClientName = "AuthApiClient";
            services.AddHttpClient(authServiceApiClientName, client => client.BaseAddress = new Uri(authServiceApiUrl!));
            services.AddHttpClient<IAuthService, AuthService>(authServiceApiClientName);

            string? userManagementServiceApiUrl = configuration.GetValue<string>("BaseUserManagementServiceUrl");
            const string userManagementServiceApiClientName = "UserManagementApiClient";
            services.AddHttpClient(userManagementServiceApiClientName, client => client.BaseAddress = new Uri(userManagementServiceApiUrl!));
            services.AddHttpClient<IUserManagementService, UserManagementService>(userManagementServiceApiClientName);

            string? passwordServiceApiUrl = configuration.GetValue<string>("BasePasswordServiceUrl");
            const string passwordServiceApiClientName = "PasswordApiClient";
            services.AddHttpClient(passwordServiceApiClientName, client => client.BaseAddress = new Uri(passwordServiceApiUrl!)).AddHttpMessageHandler<RefreshTokenHandler>();
            services.AddHttpClient<ITagService, TagService>(passwordServiceApiClientName);
            services.AddHttpClient<IVaultService, VaultService>(passwordServiceApiClientName);

            string? assetsServiceApiUrl = configuration.GetValue<string>("BaseAssetsServiceUrl");
            const string assetsServiceApiClientName = "AssetsApiClient";
            services.AddHttpClient(assetsServiceApiClientName, client => client.BaseAddress = new Uri(assetsServiceApiUrl!)).AddHttpMessageHandler<RefreshTokenHandler>();
            services.AddHttpClient<IAssetService, AssetService>(assetsServiceApiClientName);
            services.AddHttpClient<IAssetCategoryService, AssetCategoryService>(assetsServiceApiClientName);

            string? fileServiceApiUrl = configuration.GetValue<string>("BaseFileServiceUrl");
            services.AddHttpClient<IFileService, FileStorageService>(client => client.BaseAddress = new Uri(fileServiceApiUrl!));

            return services;
        }
    }
}