using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.Application.Contracts.Persistence
{
    public interface IOrderRepository: IRepository<Order.Domain.Entities.Order>
    {
        Task<IEnumerable<Order.Domain.Entities.Order>> GetOrdersByUserName(string userName);
    }
}
