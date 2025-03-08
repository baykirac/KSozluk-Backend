using KSozluk.WebAPI;
using KSozluk.WebAPI.Entities;
using KSozluk.WebAPI.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.WebAPI.Repositories
{
    public interface ILikeRepository : IDomainRepository<DescriptionLike>
    {
        Task<DescriptionLike> GetByDescriptionAndUserAsync(Guid _descriptionId, long? _userId);
        Task<WordLike> GetByWordAndUserAsync(Guid wordId, long? userId);
        void DeleteDescriptionLike(DescriptionLike descriptionLike);
        void DeleteWordLike(WordLike wordLike);
        Task CreateDescriptionLike(DescriptionLike descriptionLike);
        Task CreateWordLike(WordLike wordLike);
        Task<WordLike> FindLikedWordAsync(Guid wordId, long? userId);
        Task<DescriptionLike> FindLikedDescriptionAsync(Guid descriptionId);
        Task DeleteWordLikesByWordIdAsync(Guid wordId);

    }
}
