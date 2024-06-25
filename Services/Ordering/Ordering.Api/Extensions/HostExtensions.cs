using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>
        (
            this IHost host,
            Action<TContext,
            IServiceProvider> seeder,
            int? retry = 0
        ) where TContext : DbContext
        {
            int retryForAvailability = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("SQL Server Migrating Started ");
                    InvokeSeeder(seeder, context, services);
                    logger.LogInformation("SQL Server Migrating Done !! ");

                }
                catch (SqlException ex)
                {
                    logger.LogError(" 1- SQL Server Migrating Error !! " + Environment.NewLine + ex.Message);
                    if (retryForAvailability < 10)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);

                        MigrateDatabase<TContext>(host, seeder, retryForAvailability);
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    logger.LogError("SQL Server Migrating Error !! " + Environment.NewLine + ex.Message);
                    if (retryForAvailability < 10)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);

                        MigrateDatabase<TContext>(host, seeder, retryForAvailability);
                    }
                    throw;
                }
            }

            return host;
        }

        private static void InvokeSeeder<TContext>
        (
            Action<TContext, IServiceProvider> seeder,
            TContext context, IServiceProvider services
        ) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
