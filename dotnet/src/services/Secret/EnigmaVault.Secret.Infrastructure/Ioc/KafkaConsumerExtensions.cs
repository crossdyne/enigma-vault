using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Contracts.Messaging.Abstractions;
using Shared.Messaging;

namespace EnigmaVault.Secret.Infrastructure.Ioc
{
    public static class KafkaConsumerExtensions
    {
        public static IServiceCollection AddKafkaConsumer<TEvent>(this IServiceCollection services, string topic) 
            where TEvent : class, IIntegrationEvent
        {
            services.AddSingleton<IHostedService>(sp =>
            {
                var config = sp.GetRequiredService<IOptions<ConsumerConfig>>();
                var logger = sp.GetRequiredService<ILogger<KafkaConsumer<TEvent>>>();
                
                return new KafkaConsumer<TEvent>(config, topic, sp, logger);
            });
            
            return services;
        }
    }
}