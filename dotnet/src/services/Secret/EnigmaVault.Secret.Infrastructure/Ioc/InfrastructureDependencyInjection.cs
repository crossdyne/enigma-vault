using Confluent.Kafka;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Application.Common.Repositories;
using EnigmaVault.Secret.Application.Features.Account.EventHandlers;
using EnigmaVault.Secret.Application.Features.VaultItems.EventHandlers;
using EnigmaVault.Secret.Infrastructure.BackgroundServices;
using EnigmaVault.Secret.Infrastructure.Persistence;
using EnigmaVault.Secret.Infrastructure.Persistence.Contexts;
using EnigmaVault.Secret.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.Messaging.Abstractions;
using Shared.Contracts.Messaging.Events;

namespace EnigmaVault.Secret.Infrastructure.Ioc
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<EnigmaContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<EnigmaContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IFolderRepository, FolderRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IVaultItemRepository, VaultItemRepository>();

            services.Configure<ConsumerConfig>(configuration.GetSection("Kafka:Consumer"));
            services.AddScoped<IIntegrationEventHandler<UserPasswordResetIntegrationEvent>, UserPasswordResetIntegrationEventHandler>();
            services.AddScoped<IIntegrationEventHandler<UserAccountDeletedIntegrationEvent>, UserAccountDeletedIntegrationEventHandler>();
            services.AddKafkaConsumer<UserPasswordResetIntegrationEvent>("user-management.user.password-reset");
            services.AddKafkaConsumer<UserAccountDeletedIntegrationEvent>("user-management.user.account-delete");

            services.AddHostedService<TrashCleanupBackgroundService>();

            return services;
        }
    }
}