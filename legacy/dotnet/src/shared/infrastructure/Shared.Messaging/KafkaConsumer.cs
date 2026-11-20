using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Contracts.Messaging.Abstractions;

namespace Shared.Messaging
{
    public sealed class KafkaConsumer<TEvent>(
        IOptions<ConsumerConfig> options, 
        string topic, 
        IServiceProvider serviceProvider,
        ILogger<KafkaConsumer<TEvent>> logger) : BackgroundService where TEvent : class, IIntegrationEvent
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => Task.Run(() => RunConsumerLoop(stoppingToken), stoppingToken);

        private async Task RunConsumerLoop(CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<string, string>(options.Value).Build();
            consumer.Subscribe(topic);

            logger.LogInformation("Consumer запущен. Подписка на топик: {Topic}", topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);

                        if (consumeResult?.Message?.Value == null)
                            continue;

                        TEvent? integrationEvent;
                        try
                        {
                            integrationEvent = JsonSerializer.Deserialize<TEvent>(consumeResult.Message.Value, JsonOptions);
                        }
                        catch (JsonException ex)
                        {
                            logger.LogError(ex, "Ошибка десериализации сообщения из топика {Topic}. Сообщение будет пропущено.", topic);
                            consumer.Commit(consumeResult); 
                            continue;
                        }

                        if (integrationEvent == null)
                        {
                            consumer.Commit(consumeResult);
                            continue;
                        }

                        await ProcessEventAsync(integrationEvent, stoppingToken);
                        consumer.Commit(consumeResult);
                    }
                    catch (OperationCanceledException)
                    {
                        logger.LogInformation("Consumer {Topic} корректно остановлен.", topic);
                        break;
                    }
                    catch (ConsumeException ex)
                    {
                        logger.LogError(ex, "Ошибка чтения из Kafka. Топик: {Topic}", topic);
                    }
                }
            }
            finally
            {
                consumer.Close();
            }
        }

        private async Task ProcessEventAsync(TEvent @event, CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();

            var handler = scope.ServiceProvider.GetRequiredService<IIntegrationEventHandler<TEvent>>();

            await handler.HandleAsync(@event, cancellationToken);
        }
    }
}