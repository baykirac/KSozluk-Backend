using KSozluk.Domain;
using KSozluk.Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Application.Services.Repositories
{
    public interface IFavouriteWordRepository : IDomainRepository<FavouriteWord>
    {
        Task<FavouriteWord> GetByFavouriteWordAndUserAsync(Guid _wordId, Guid _userId);

        void Delete(FavouriteWord entity);
    }
}
