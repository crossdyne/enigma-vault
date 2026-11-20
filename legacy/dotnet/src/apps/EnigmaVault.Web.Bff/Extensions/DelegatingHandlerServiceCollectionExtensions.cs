using EnigmaVault.Web.Bff.Handlers;

namespace EnigmaVault.Web.Bff.Extensions
{
    public static class DelegatingHandlerServiceCollectionExtensions
    {
        public static IServiceCollection AddDelegationsHandlers(this IServiceCollection services)
        {
            services.AddTransient<AccessTokenHandler>();

            return services;
        }
    }
}