using Npgsql;

namespace Discount.gRPC.Extensions;

public static class HostExtensions
{
    public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
    {
        int retryForAvailability = retry.Value;

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var configuration = services
                .GetRequiredService<IConfiguration>();

            var logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Migrating postgresql database");
                using var connection = new NpgsqlConnection
                (
                     configuration.GetValue<string>("DatabaseSettings:ConnectionString")
                );

                connection.Open();
                using var command = new NpgsqlCommand
                {
                    Connection = connection,
                    CommandText = "DROP TABLE IF EXISTS Coupon"
                };

                command.ExecuteNonQuery();

                command.CommandText =
                    "CREATE TABLE Coupon(" +
                    "Id SERIAL PRIMARY KEY," +
                    "ProductName VARCHAR(200)," +
                    "Description TEXT," +
                    "Amount INT" +
                    ")";

                command.ExecuteNonQuery();

                command.CommandText =
                    "INSERT INTO Coupon" +
                    "(ProductName,Description,Amount)" +
                    "VALUES" +
                    "('IPhone X','IPhone Discount',100)";

                command.ExecuteNonQuery();

                command.CommandText =
                    "INSERT INTO Coupon" +
                    "(ProductName,Description,Amount)" +
                    "VALUES" +
                    "('Samsung A15','Samsung A15 Discount',200)";

                command.ExecuteNonQuery();

                logger.LogInformation("Migration has been completed !!!");
            }
            catch (NpgsqlException ex)
            {
                logger.LogError("Error postgresql database" + Environment.NewLine + ex.Message);

                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    MigrateDatabase<TContext>(host, retryForAvailability);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error postgresql database");
            }

            return host;
        }
    }
}
