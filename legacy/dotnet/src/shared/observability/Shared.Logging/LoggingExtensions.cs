using Microsoft.Extensions.Hosting;
using Serilog;

namespace Shared.Logging
{
    public static class LoggingExtensions
    {
        public static IHostBuilder AddSerilogLogger(this IHostBuilder builder)
        {
            builder.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId()
                    .Enrich.WithProperty("ApplicationName", context.HostingEnvironment.ApplicationName)
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);

                configuration.ReadFrom.Configuration(context.Configuration);
            });
            
            return builder;
        }
    }
}