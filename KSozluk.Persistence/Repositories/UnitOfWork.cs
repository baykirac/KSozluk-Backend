using KSozluk.Domain.SharedKernel;
using KSozluk.Persistence.Contexts;

namespace KSozluk.Persistence.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly SozlukContext _context;

        public UnitOfWork(SozlukContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
