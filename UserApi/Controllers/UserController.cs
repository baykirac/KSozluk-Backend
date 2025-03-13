using Microsoft.AspNetCore.Mvc;
using Ozcorps.Core.Models;
using Ozcorps.Ozt;
using Ozcorps.Logger;
using UserApi.Business;
using System.Text;

namespace UserApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[OzLoggerActionFilter]
public class UserController : ControllerBase
{
    private readonly IUserApiService _UserApiService;
    private readonly IOzLogger _Logger;
    private readonly OztTool _OztTool;

    public UserController(IOzLogger _logger, IUserApiService _userApiService, OztTool _oztTool)
    {
        _UserApiService = _userApiService;

        _Logger = _logger;

        _OztTool = _oztTool;
    }

    [OztActionFilter]
    [HttpPost]
    public Response Paginate(PaginatorDto _dto)
    {
        var _result = new Response();

        try
        {
            _result.Data = _UserApiService.Paginate(_dto, HttpContext.GetCompanyIds(), HttpContext.GetUserId());

            _result.Success = true;
        }
        catch (Exception _ex)
        {
           _Logger.Error(_ex,
               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
               _username: HttpContext.GetOztUser()?.Username,
               _userId: (long)(HttpContext.GetOztUser()?.UserId));
        }

        return _result;
    }

    [HttpGet]
    // [OztActionFilter(Permissions = "web_application_access,map_proxy_access")]
    public Response GetUserByToken()
    {
        var _result = new Response();

        try
        {
            var _user = HttpContext.GetOztUser();

            _result.Data = new
            {
                _user.UserId,
                _user.Username,
                Permissions = _user.Permissions.Select(x =>
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(x)))
            };

            _result.Success = true;
        }
        catch (Exception _ex)
        {
            _Logger.Error(_ex,
               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
               _username: HttpContext.GetOztUser()?.Username,
               _userId: (long)(HttpContext.GetOztUser()?.UserId));
        }

        return _result;
    }
}