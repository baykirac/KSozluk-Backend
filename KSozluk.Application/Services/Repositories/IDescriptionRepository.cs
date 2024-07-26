using KSozluk.Domain;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Services.Repositories
{
    public interface IDescriptionRepository : IDomainRepository<Description>
    {
        public Task<List<Description>> FindByWordAsync(Guid id);
        public Task<double> FindGreatestOrder(Guid wordId);
    }
}
