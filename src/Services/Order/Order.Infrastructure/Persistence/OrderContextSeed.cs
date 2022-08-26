using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync().ConfigureAwait(false);
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order.Domain.Entities.Order> GetPreconfiguredOrders()
        {
            return new List<Order.Domain.Entities.Order>
            {
                new Order.Domain.Entities.Order() {UserName = "vmutlu", FirstName = "Veysel", LastName = "MUTLU", EmailAddress = "veysel_mutlu42@hotmail.com", AddressLine = "Darıca", Country = "Turkey", TotalPrice = 350 }
            };
        }
    }
}
