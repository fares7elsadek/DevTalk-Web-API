using System.Linq.Expressions;

namespace DevTalk.Domain.Repositories;

public interface IRepositories<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(string? IncludeProperties = null);
    Task<IEnumerable<T>> GetAllWithConditionAsync(Expression<Func<T, bool>> filter, string? IncludeProperties = null);
    Task<T?> GetOrDefalutAsync(Expression<Func<T, bool>> filter, string? IncludeProperties = null);
    Task AddAsync(T entity);
    Task<IEnumerable<T>> GetAllWithPagination(Expression<Func<T, bool>> filter, int page, int size, string? IncludeProperties = null);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    void Update(T entity);
}
