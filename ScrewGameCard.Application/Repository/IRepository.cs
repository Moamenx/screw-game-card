    using System.Linq.Expressions;
    using ScrewGameCard.Application.DTO.Common;

    namespace ScrewGameCard.Application.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = true);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true);
        Task<T> GetAsync(object id, bool asNoTracking = true);
        Task DeleteAsync(T entity);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true);
        Task UpdateAsync(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true);
        Task<PaginatedResult<T>> GetPaginatedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? predicate = null, bool asNoTracking = true);
    }
}
