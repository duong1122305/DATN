using System.Linq.Expressions;

namespace DATN.Aplication.Common
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> GetAsync(Guid id);
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<int> SaveChangesAsync();
        Task AddRangeAsync(List<T> entities);
        Task UpdateRangeAsync(List<T> entities);
    }
}
