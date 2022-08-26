using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Contracts.Infrastructure;
using Order.Application.Contracts.Persistence;
using Order.Application.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var mappedEntity = _mapper.Map<Order.Domain.Entities.Order>(request);
            var result = await _orderRepository.AddAsync(mappedEntity).ConfigureAwait(false);

            _logger.LogInformation($"Order {result.Id} is successfuly created");

            await SendMail(result).ConfigureAwait(false);

            return result.Id;
        }

        private async Task SendMail(Order.Domain.Entities.Order order)
        {
            var email = new Email() { To = "mutluveysel@gmail.com", Body = $"Order was created.", Subject = "Order was created" };

            try
            {
                await _emailService.SendEmail(email).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Order {order.Id} failed due to an error with the mail service: {ex.Message}");
            }
        }
    }
}
