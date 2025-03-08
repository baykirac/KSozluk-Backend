using Ozcorps.Core.Models;
using UserApi.Dtos;

namespace UserApi.Business;

public interface IRoleService
{
    PaginatorResponseDto<RoleDto> Paginate(PaginatorDto _dto, List<long> _companyIds);
}