using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartManagement.Common;
using SmartManagement.Domain.Utilities;
using SmartManagement.Infrastructure.Data;

namespace SmartMangement.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<SmartDbContext>(options =>
            {
                var connectionString = SmartEncryptionSuit.Decrypt(AppConfigSetting.Suit.GetConnectionString("SmartConnection"));

                options.UseSqlServer(connectionString);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.LogTo(Console.WriteLine);
            });

            return services;
        }

    }
}
