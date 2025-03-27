using Ozcorps.Core.Models;
 using Microsoft.AspNetCore.Mvc;
 using UserApi.Dtos;
 using UserApi.Business;
 using Ozcorps.Logger;
 using Ozcorps.Ozt;
 using System.Text;
 using UserApi.Entities;
 using Ozcorps.Localization;
 
 namespace UserApi.Controllers;
 
 [ApiController]
 [Route("api/[controller]")]
 public class TokenController : ControllerBase
 {
     private readonly IUserApiService _UserApiService;
 
     private readonly IOzLogger _Logger;
 
     private readonly OztTool _OztTool;
 
     public TokenController(IUserApiService _userApiService, IOzLogger _logger, OztTool _oztTool)
     {
         _UserApiService = _userApiService;
 
         _Logger = _logger;
 
         _OztTool = _oztTool;
     }
 
     [HttpPost("get-token")]
     public Response GetToken(RequestGetTokenDto _dto)
     {
         var _result = new Response();
 
         try
         {
             if (_dto.RetriesControl && _dto.RetriesCount < 1)
             {
                 _result.Message = "RetriesControl parametresi true olduğunda RetriesCount parametresi 1'den küçük olamaz!";
 
                 return _result;
             }
 
             var _responseValidateUser = _UserApiService.Validate(
                 _dto, true, HttpContext.GetOzl());
 
             if (!_responseValidateUser.Success)
             {
                 _Logger.User(new UserLog
                 {
                     Username = _dto.Username,
                     UserLogType = UserLogType.WrongLogIn,
                     UserId = _responseValidateUser.User?.Id ?? 0,
                     Date = DateTime.Now,
                     UserIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                 });
                 _result.Data = new
                 {
                     retries = _responseValidateUser.Retries
                 };
                 _result.Message = _responseValidateUser.Message;
 
                 return _result;
             }
 
             var _tokenUser = new OztUser
             {
                 CompanyIds = _UserApiService.GetCompanyIds(_responseValidateUser.User.Id),
                 Email = _responseValidateUser.User.Email,
                 Username = _responseValidateUser.User.Username,
                 Permissions = _responseValidateUser.Permissions.
                     Select(x => x.Name).
                     ToList(),
                 Roles = _responseValidateUser.Roles.
                     Select(x => x.Name).
                     ToList(),
                 UserId = _responseValidateUser.User.Id,
                 RoleIds = _responseValidateUser.Roles.
                     Select(x => x.Id).ToList()
             };
 
             var _token = _OztTool.GenerateToken(_tokenUser);
 
             if (string.IsNullOrEmpty(_token))
             {
                 _result.Message = "Token oluşturulamadı.";
 
                 return _result;
             }
 
             _Logger.User(new UserLog
             {
                 UserLogType = UserLogType.LogIn,
                 UserId = _responseValidateUser.User.Id,
                 Username = _responseValidateUser.User.Username,
                 UserRoles = _tokenUser.Roles.Any() ? _tokenUser.Roles.Aggregate((i, j) => i + "," + j) : "",
                 Date = DateTime.Now,
                 UserIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
             });
 
             _result.Data = new
             {
                 Token = _token,
                 User = new
                 {
                     UserId = _responseValidateUser.User.Id,
                     _responseValidateUser.User.Username,
                     Permissions = _tokenUser.Permissions.Select(x =>
                         Convert.ToBase64String(Encoding.UTF8.GetBytes(x)))
                 }
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

    [HttpPost("reset-password")]
    public Response ResetPassword(ResetPasswordDto _dto)
    {
        var _result = new Response();

        try
        {
            var _user = _UserApiService.GetFirst(x => x.Email == _dto.Email &&
                !x.IsDeleted &&
                x.IsActive);

            if (_user == null)
            {

                _user = _UserApiService.GetFirst(x => x.Username == _dto.Email &&
                    !x.IsDeleted &&
                    x.IsActive);

                if (_user == null)
                {
                    _result.Message = "Kullanıcı adı ya da email adresi bulunamadı!";

                    return _result;
                }
            }

            if (_user.IsLdap)
            {
                _result.Message = "Ldap kullanıcılarını şifresi uygulama üzerinden değiştirilemez!";

                return _result;
            }

            _UserApiService.ResetPassword(_user.Email, _user.Id);

            _result.Data = true;

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

    [HttpPost("change-password")]
    public Response ChangePassword(ChangePasswordDto _dto)
    {
        var _result = new Response();

        try
        {
            var _user = _OztTool.ValidateToken(_dto.Token);

            if (!_user.IsValidated || _user.OztUser.Roles[0] != "reset-pass")
            {
                _result.Message = "Kullanıcı bulunamadı ya da şifre sıfırlama bağlantısı zaman aşımına uğramış olabilir. Lütfen yeni bir şifre sıfırlama isteğinde bulunun.";

                return _result;
            }

            _UserApiService.ChangePassword(_dto.Password, _user.OztUser.UserId);

            _result.Data = true;

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

    [HttpPost("get-user")]
    // [OztActionFilter(Permissions = "pm_user_profile")]
    [OztActionFilter()]
    public Response GetUser(string username)
    {
        var _result = new Response();
        try
        {

            var _user = _UserApiService.Get(username);
            if (_user != null)
            {
                var _userSimplified = new UserSimple
                {
                    Username = _user.Username,
                    Email = _user.Email,
                    Name = _user.Name,
                    Surname = _user.Surname,
                    Id = _user.Id,
                };
                var _userRoles = _UserApiService.GetUserRoles(_user.Id).ToList();
                var _userRolesSimple = _userRoles.Select(x => new UserRoleDescriptionSimple { RoleName = x.RoleName }).ToList();


                var userWithRoles = new UserWithRolesSimple
                {
                    User = _userSimplified,
                    Roles = _userRolesSimple
                };
                _result.Data = userWithRoles;
                _result.Success = true;
            }
            else
            {
                _result.Success = false;
                _result.Message = "Kullanıcı bulunamadı";
            }
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
 [HttpGet("get-all-users")]
[OztActionFilter()]
public Response GetAllUsers()
{
    var _result = new Response();

    try
    {
        var users = _UserApiService.GetAll();
        _result.Data = users;
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


    [HttpGet("test")]
    public string TestMe() => "ok";
}

