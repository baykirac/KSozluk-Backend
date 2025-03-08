using KSozluk.Domain.SharedKernel;
using KSozluk.Persistence.Contexts;
using System.Threading;
using System.Threading.Tasks;

namespace KSozluk.Persistence.Repositories
{
    public class Unit: IUnit
    {
        private readonly SozlukContext _context;

        public Unit(SozlukContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}