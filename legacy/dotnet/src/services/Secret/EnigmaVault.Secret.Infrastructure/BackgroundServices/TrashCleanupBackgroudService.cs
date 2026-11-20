using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EnigmaVault.Secret.Infrastructure.BackgroundServices
{
    public sealed class TrashCleanupBackgroundService(
        IServiceScopeFactory scopeFactory, 
        ILogger<TrashCleanupBackgroundService> logger) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Сервис очистки корзины запущен");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<EnigmaContext>();

                    int deletedCount = await context.Set<VaultItem>().Where(vi => vi.DeletedAt <= DateTime.UtcNow).ExecuteDeleteAsync(stoppingToken);
               
                    logger.LogInformation("Удалено записей из корзины: {deletedCount}", deletedCount);

                    var currentTime = DateTime.UtcNow;
                    var nextRun = currentTime.Date.AddDays(1).AddHours(3);
                    var delay = nextRun - currentTime; 

                    logger.LogInformation("Следующая очистка в {nextRun} (через {delay})", nextRun, delay);
                    
                    await Task.Delay(delay, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    logger.LogInformation("Сервис очистки корзины остановлен");
                    throw;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Ошибка при очистке корзины");

                    try 
                    { 
                        await Task.Delay(TimeSpan.FromHours(1), stoppingToken); 
                    }
                    catch (OperationCanceledException) 
                    { 
                        throw; 
                    }
                }
            }
        }
    }
}