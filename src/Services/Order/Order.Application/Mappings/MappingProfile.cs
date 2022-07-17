using AutoMapper;
using Order.Application.Features.Orders.Commands.CheckoutOrder;
using Order.Application.Features.Orders.Commands.UpdateOrder;
using Order.Application.Features.Orders.Queries.GetOrdersList;

namespace Order.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order.Domain.Entities.Order, OrderDTO>().ReverseMap();
            CreateMap<Order.Domain.Entities.Order, CheckoutOrderCommand>().ReverseMap();
            CreateMap<Order.Domain.Entities.Order, UpdateOrderCommand>().ReverseMap();
        }
    }
}
