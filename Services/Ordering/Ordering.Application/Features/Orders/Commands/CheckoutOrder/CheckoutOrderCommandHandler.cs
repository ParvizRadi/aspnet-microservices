using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler
        (
            IOrderRepository orderRepository,
            IMapper mapper,
            IEmailService emailService,
            ILogger<CheckoutOrderCommandHandler> logger
        )
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper
                .Map<Order>(request);

            var newOrder = await _orderRepository
                .AddAsync(entity: orderEntity);

            _logger.LogInformation($"Order with orderId : {newOrder.Id} created successfully");

            await SendMail(newOrder);

            return newOrder.Id;
        }

        private async Task SendMail(Order order)
        {
            try
            {
                var email = new Email
                {
                    To = order.EmailAddress,
                    Subject = "Create Order",
                    Body = "order created successfully ",
                };

                await _emailService.SendEmailAsync(email: email);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in send mail : " +
                    Environment.NewLine +
                    ex.Message
                );
            }
        }

    }
}
