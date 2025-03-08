using System.Linq.Expressions;

namespace KSozluk.WebAPI.SharedKernel
{
    public interface IDomainRepository<T>
    {
        Task CreateAsync(T entity);
        Task<T> FindAsync(Guid? id);
        Task DeleteAsync(Guid id);
        Task<List<T>> GetAll(Expression<Func<T, bool>> predicate);
        Task<T> GetById(Expression<Func<T, bool>> predicate);

    }
}
