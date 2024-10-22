using KSozluk.Domain;
using KSozluk.Domain.DTOs;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Services.Repositories
{
    public interface IDescriptionRepository : IDomainRepository<Description>
    {
        public Task<List<DescriptionWithIsLikeDto>> FindByWordAsync(Guid id, Guid userId);
        public Task<int> FindGreatestOrder(Guid wordId);
        public Task<Word> FindByDescription(Guid id);
        public Task<Description> FindParentDescription(Guid id);
    }
}
