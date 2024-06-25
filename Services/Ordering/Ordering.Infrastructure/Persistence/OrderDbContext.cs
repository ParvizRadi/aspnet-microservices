using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreateDateTime = DateTime.Now;
                        entry.Entity.CreatedBy = "MyUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedDateTime = DateTime.Now;
                        entry.Entity.LastModifiedBy = "MyUser";
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
