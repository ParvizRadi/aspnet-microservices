using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Queries.GetOrdersList.ViewModel;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrderViewModel>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderViewModel>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var ordersList = await _orderRepository.GetOrdersByUserNameAsync(request.UserName);

            return _mapper.Map<List<OrderViewModel>>(ordersList);
        }
    }
}
