using System.Text.Json;

namespace EnigmaVault.Web.Bff.Extensions
{
    public static class OptionServiceCollectionExceptions
    {
        public static IServiceCollection AddCustomOptions(this IServiceCollection services)
        {
            services.Configure<JsonSerializerOptions>(options => 
            {
                options.PropertyNameCaseInsensitive = true;
            });

            return services;
        }
    }
}