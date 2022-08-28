using Microsoft.EntityFrameworkCore;
using Order.Application.Contracts.Persistence;
using Order.Domain.Common;
using Order.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Order.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly OrderContext _dbContext;

        public RepositoryBase(OrderContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync() => await _dbContext.Set<T>().ToListAsync().ConfigureAwait(false);

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate) => await _dbContext.Set<T>().Where(predicate).ToListAsync().ConfigureAwait(false);

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking) query = query.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

            if (predicate is not null) query = query.Where(predicate);

            if (orderBy is not null) return await orderBy(query).ToListAsync().ConfigureAwait(false);

            return await query.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking) query = query.AsNoTracking();

            if (includes is not null) query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (predicate is not null) query = query.Where(predicate);

            if (orderBy is not null) return await orderBy(query).ToListAsync().ConfigureAwait(false);

            return await query.ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<T> GetByIdAsync(int id) => await _dbContext.Set<T>().FindAsync(id).ConfigureAwait(false);

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
