using Microsoft.AspNetCore.Mvc;
using Ozcorps.Core.Models;
using Ozcorps.Ozt;
using Ozcorps.Logger;
using UserApi.Business;

namespace UserApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[OztActionFilter]
[OzLoggerActionFilter]
public class RoleController : ControllerBase
{
    private readonly IRoleService _RoleService;
    private readonly IOzLogger _Logger;

    public RoleController(IOzLogger _logger, IRoleService _roleService)
    {

        _RoleService = _roleService;

        _Logger = _logger;
        
    }

    [HttpPost]
    public Response Paginate(PaginatorDto _dto)
    {
        var _result = new Response();

        try
        {

            _result.Data = _RoleService.Paginate(_dto, HttpContext.GetCompanyIds());

            _result.Success = true;

        }
        catch (System.Exception _ex)
        {

            _Logger.Error(_ex,

            _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

            _username: HttpContext.GetOztUser()?.Username,

            _userId: (long)(HttpContext.GetOztUser()?.UserId));

        }

        return _result;
    }
}