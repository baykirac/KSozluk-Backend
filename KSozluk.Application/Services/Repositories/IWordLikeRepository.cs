using KSozluk.Domain;
using KSozluk.Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Application.Services.Repositories
{
    public interface IWordLikeRepository : IDomainRepository<DescriptionLike>
    {
        Task<DescriptionLike> GetByDescriptionAndUserAsync(Guid _descriptionId, Guid _userId);

        void Delete(DescriptionLike entity);
    }
}
