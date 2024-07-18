namespace KSozluk.Domain.SharedKernel
{
    public interface IDomainRepository<T>
    {
        Task CreateAsync(T entity);
        Task<T> FindAsync(Guid id);
    }
}
