using Lykke.AzureStorage;
using Lykke.AzureStorage.Tables;
using Lykke.Common.Log;
using LykkeMarketMakers.AzureRepositories.Entities;
using LykkeMarketMakers.AzureRepositories.Repositories;
using LykkeMarketMakers.Core.Interfaces.Repositories;
using LykkeMarketMakers.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LykkeMarketMakers.Web
{
    public static class ServiceRegistrator
    {
        public static void RegisterLyykeServices(this IServiceCollection services)
        {
            // Add application services.
            //services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddSingleton<IUserCacheService, UserCacheService>();
        }

        public static void RegisterRepositories(this IServiceCollection services, string connectionString, ILog log)
        {
            services.AddSingleton<INoSQLTableStorage<BackOfficeUserEntity>>(new AzureTableStorage<BackOfficeUserEntity>(connectionString, "MarketMakerUiUsers", log));
            services.AddSingleton<INoSQLTableStorage<BackOfficeUserRoleEntity>>(new AzureTableStorage<BackOfficeUserRoleEntity>(connectionString, "MarketMakerUiUserRoles", log));
            services.AddSingleton<IBrowserSessionsRepository>(new BrowserSessionsRepository(new AzureTableStorage<BrowserSessionEntity>(connectionString, "MaretMakerUiBrowserSessions", log)));
            services.AddSingleton<IMenuBadgesRepository>(new MenuBadgesRepository(new AzureTableStorage<MenuBadgeEntity>(connectionString, "MarketMakerUiMenuBadges", log)));

            services.AddTransient<IBackOfficeUsersRepository, BackOfficeUsersRepository>();
            services.AddTransient<IBackofficeUserRolesRepository, BackOfficeUserRolesRepository>();
        }
    }
}
