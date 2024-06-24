using Basket.Api.Entities;
using Basket.Api.gRPCServices;
using Basket.Api.Repositories;
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

        public BasketController(IBasketRepository basketRepository, DiscountgRPCService discountgRPCService)
        {
            _basketRepository = basketRepository;
            _discountgRPCService = discountgRPCService;
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
    }
}
