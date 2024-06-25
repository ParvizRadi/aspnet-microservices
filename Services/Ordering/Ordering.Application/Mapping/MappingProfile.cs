using AutoMapper;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList.ViewModel;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderViewModel>()
                .ReverseMap();

            CreateMap<Order, CheckoutOrderCommand>()
                .ReverseMap();

            CreateMap<Order, UpdateOrderCommand>()
                .ReverseMap();
        }
    }
}
