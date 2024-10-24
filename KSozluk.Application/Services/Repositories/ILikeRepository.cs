using KSozluk.Domain;
using KSozluk.Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Application.Services.Repositories
{
    public interface ILikeRepository : IDomainRepository<DescriptionLike>
    {
        Task<DescriptionLike> GetByDescriptionAndUserAsync(Guid _descriptionId, Guid _userId);
        Task<WordLike> GetByWordAndUserAsync(Guid wordId, Guid userId);

        void DeleteDescriptionLike(DescriptionLike descriptionLike);
        void DeleteWordLike(WordLike wordLike);
        Task CreateDescriptionLike(DescriptionLike descriptionLike);
        Task CreateWordLike(WordLike wordLike);
        Task<WordLike> FindLikedWordAsync(Guid wordId);
        Task<DescriptionLike> FindLikedDescriptionAsync(Guid descriptionId);

    }
}
