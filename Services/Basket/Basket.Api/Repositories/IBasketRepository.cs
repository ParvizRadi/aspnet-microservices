using Basket.Api.Entities;

namespace Basket.Api.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetUserBasketAsync(string username);

        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket);

        Task DeleteBasketAsync(string username);
    }
}
