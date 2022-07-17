using MediatR;
using System.Collections.Generic;

namespace Order.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrderListQuery : IRequest<List<OrderDTO>>
    {
        public string UserName { get; set; }
        public GetOrderListQuery(string userName)
        {
            UserName = userName;
        }
    }
}
