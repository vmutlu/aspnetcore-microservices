using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Order.Domain.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Infrastructure.Persistence
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<Order.Domain.Entities.Order> Orders { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (EntityEntry<BaseEntity> entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now.Date;
                        entry.Entity.CreatedBy = "vmutlu";
                        break; 
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now.Date;
                        entry.Entity.LastModifiedBy = "vmutlu";
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
