using Microsoft.EntityFrameworkCore;
using Order.Application.Contracts.Persistence;
using Order.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Domain.Entities.Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Domain.Entities.Order>> GetOrdersByUserName(string userName)
        {
            var orderList = await _dbContext.Orders
                                .Where(o => o.UserName == userName)
                                .ToListAsync().ConfigureAwait(false);
            return orderList;
        }
    }
}
