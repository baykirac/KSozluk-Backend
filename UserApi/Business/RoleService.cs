using Ozcorps.Core.Extensions;
using Ozcorps.Core.Models;
using Ozcorps.Generic.Dal;
using UserApi.Dtos;
using UserApi.Entities;

namespace UserApi.Business;

public class RoleService : IRoleService
{
    private readonly IRepository<Role> _RoleRepository;

    public RoleService(IUnitOfWork _unitOfWork)
    {
        _RoleRepository = _unitOfWork.GetRepository<Role>();
    }

    public PaginatorResponseDto<RoleDto> Paginate(PaginatorDto _dto, List<long> _companyIds)
    {

        var _rows = _RoleRepository.GetQueryable()
            .Where(x => (_companyIds != null ? _companyIds.Contains((long)x.CompanyId) : true) && x.IsDeleted == false)
            .OrderByDescending(x => x.Id)
            .Paginate(_dto, out int _count)
            .ToListDynamic<Role>()
            .Select(x => new RoleDto
            {

                Id = x.Id,

                Name = x.Name

            }).ToList();

        return new PaginatorResponseDto<RoleDto>
        {

            Count = _count,

            Rows = _rows
            
        };
    }
}