using AutoMapper;
using Basket.Api.Entities;
using Basket.Api.gRPCServices;
using Basket.Api.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        #region Constructor's

        private readonly IBasketRepository _basketRepository;
        private readonly DiscountgRPCService _discountgRPCService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;


        public BasketController(IBasketRepository basketRepository, DiscountgRPCService discountgRPCService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _basketRepository = basketRepository;
            _discountgRPCService = discountgRPCService;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        #endregion

        #region Method's

        [HttpGet("{username}")]
        [ProducesResponseType(type: typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
        {
            var basket = await _basketRepository
                .GetUserBasketAsync(username);

            return Ok(basket ?? new ShoppingCart(username: username));
        }

        [HttpPost]
        [ProducesResponseType(type: typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await _discountgRPCService
                    .GetDiscount(item.ProductName);

                item.Price = item.Price - coupon.Amount;
            }

            return Ok(await _basketRepository.UpdateBasketAsync(basket: basket));
        }

        [HttpDelete("{username}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await _basketRepository.DeleteBasketAsync(username);

            return Ok();
        }

        #endregion

        #region Checkout

        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // get existing basket with total price

            var basket = await _basketRepository
                .GetUserBasketAsync(username: basketCheckout.UserName);

            if (basketCheckout == null)
            {
                return BadRequest();
            }

            // create basket checkout event -- total price on basketcheckout event message

            var eventMessage = _mapper
                .Map<BasketCheckoutEvent>(basketCheckout);

            eventMessage.TotalPrice = basket.TotalPrice;

            // send checkout event to rabbitmq
            await _publishEndpoint.Publish(eventMessage);

            //remove basket
            await _basketRepository
                .DeleteBasketAsync(username: basketCheckout.UserName);

            return Accepted();
        }

        #endregion
    }
}
