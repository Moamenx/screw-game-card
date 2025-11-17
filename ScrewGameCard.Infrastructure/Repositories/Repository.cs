using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ScrewGameCard.Application.DTO.Common;
using ScrewGameCard.Application.Repositories;
using ScrewGameCard.Infrastructure.Data;

namespace ScrewGameCard.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ScrewGameCardDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ScrewGameCardDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (asNoTracking)
                query = query.AsNoTracking();
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            if (asNoTracking)
                query = query.AsNoTracking();
            return await query.ToListAsync();
        }
        public async Task<T> GetAsync(object id, bool asNoTracking = true)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null && asNoTracking)
                _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true)
        {
            var query = _dbSet.Where(predicate);
            if (asNoTracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
                return await _dbSet.AsNoTracking().CountAsync();
            return await _dbSet.AsNoTracking().CountAsync(predicate);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            if (asNoTracking)
                query = query.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<PaginatedResult<T>> GetPaginatedAsync(
            int pageNumber, int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null)
                query = query.Where(predicate);
            if (asNoTracking)
                query = query.AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return PaginatedResult<T>.Create(items, totalCount, pageNumber, pageSize);
        }
    }
}
