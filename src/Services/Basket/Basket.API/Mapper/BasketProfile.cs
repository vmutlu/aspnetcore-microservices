using AutoMapper;
using Basket.API.Entities;
using Core.Messages.Events;

namespace Basket.API.Mapper
{
	public class BasketProfile : Profile
	{
		public BasketProfile()
		{
			CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
		}
	}
}
