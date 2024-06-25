using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {

        public OrderRepository(OrderDbContext context)
             : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserNameAsync(string username)
        {
            return await _dbContext.Orders
                .Where(x => x.UserName == username)
                .ToListAsync();
        }
    }
}
