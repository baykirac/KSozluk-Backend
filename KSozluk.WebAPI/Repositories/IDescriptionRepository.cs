using KSozluk.WebAPI.Entities;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.SharedKernel;

namespace KSozluk.WebAPI.Repositories
{
    public interface IDescriptionRepository : IDomainRepository<Description>
    {

        public Task<List<DescriptionHeaderNameDto>> FindHeaderByWordAsync(Guid id);
        public Task<List<DescriptionWithIsLikeDto>> FindByWordAsync(Guid id, long? userId);
        public Task<int> FindGreatestOrder(Guid wordId);
        public Task<Word> FindByDescription(Guid id);
        public Task<Description> FindParentDescription(Guid id);
        public Task<List<DescriptionTimelineDto>> GetDescriptionForTimelineAsync(long? userId);
        public Task<Description> FindByContentAsync(string descriptionContent);
    }
}
