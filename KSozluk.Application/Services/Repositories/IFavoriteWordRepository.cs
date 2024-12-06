using KSozluk.Domain;
using KSozluk.Domain.DTOs;
using KSozluk.Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Application.Services.Repositories
{
    public interface IFavoriteWordRepository : IDomainRepository<FavoriteWord>
    {
        Task<FavoriteWord> GetByFavoriteWordAndUserAsync(Guid _wordId, Guid _userId);
        Task<List<ResponseFavouriteWordContentDto>> GetFavouriteWordsByUserIdAsync(Guid _userId);

        void Delete(FavoriteWord entity);
    }
}
