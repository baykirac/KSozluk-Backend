using KSozluk.WebAPI;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.Entities;
using KSozluk.WebAPI.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.WebAPI.Repositories
{
    public interface IFavoriteWordRepository : IDomainRepository<FavoriteWord>
    {
        Task<FavoriteWord> GetByFavoriteWordAndUserAsync(Guid _wordId, long? _userId);
        Task<List<ResponseFavouriteWordContentDto>> GetFavouriteWordsByUserIdAsync(long? _userId);

        void Delete(FavoriteWord entity);
    }
}
