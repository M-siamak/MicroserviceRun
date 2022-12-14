using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContexts>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContexts>>();
                try
                {
                    logger.LogInformation("Migarting postresql database. ");

                    using var connection = new NpgsqlConnection
                        (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand
                    {

                        Connection = connection,
                    };
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();
                    command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,ProductName VARCHAR(24) NOT NULL,Description TEXT, Amount INT)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO coupon(productname, description, amount) VALUES('IPhone X', 'IPhone discount', 150)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO coupon(productname, description, amount) VALUES('Samsung 10', 'Samsung discount', 100)";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Postgresql database migrated");

                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database");
                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContexts>(host, retryForAvailability);
                    }
                }

            }

            return host;
            
            

        }
    }
}
