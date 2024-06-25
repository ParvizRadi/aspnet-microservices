using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderCommand> _logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<UpdateOrderCommand> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var record = await _orderRepository
                .GetByIdAsync(request.Id);

            if (record == null)
            {
                _logger.LogError("order is not exists");
            }
            else
            {
                await _orderRepository
                    .DeleteAsync(record);

                _logger.LogInformation($"order with id : {request.Id} deleted successfully");
            }

            return Unit.Value;
        }
    }
}
