using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync
        (
            OrderDbContext orderDbContext,
            ILogger<OrderContextSeed> logger
        )
        {
            if (!await orderDbContext.Orders.AnyAsync())
            {
                await orderDbContext.Orders.AddRangeAsync(GetPreData());
                await orderDbContext.SaveChangesAsync();

                logger.LogInformation("data seed section configured");
            }
        }

        private static IEnumerable<Order> GetPreData()
        {
            return new List<Order>()
            {
                new Order{
                    FirstName ="Parviz",
                    LastName="Radi",
                    UserName="parviz_radi",
                    EmailAddress = "test@yahoo.com",
                    City="tabriz",
                    Country="iran",
                    TotalPrice=1000,
                    BankName=string.Empty,
                    RefCode=string.Empty,
                    PaymentMethod=0,
                    CreatedBy="parviz",
                    LastModifiedBy="parviz",
                },
                new Order{
                    FirstName ="Farzin",
                    LastName="Radi",
                    UserName="farzin_radi",
                    EmailAddress = "test@yahoo.com",
                    City="tabriz",
                    Country="iran",
                    TotalPrice=2000,
                    BankName=string.Empty,
                    RefCode=string.Empty,
                    PaymentMethod=0,
                    CreatedBy="parviz",
                    LastModifiedBy="parviz",
                }
            };
        }
    }
}
