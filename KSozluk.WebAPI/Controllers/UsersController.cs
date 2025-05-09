using Ozcorps.Ozt;
using Ozcorps.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KSozluk.WebAPI.Business;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.SharedKernel;
using KSozluk.WebAPI.Entities;
using Microsoft.AspNetCore.RateLimiting;
using KSozluk.WebAPI.DTOs.Request;

namespace KSozluk.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [OzLoggerActionFilter]
    [EnableRateLimiting("interact-limit")]
    public class UsersController : ControllerBase
    {
        private readonly OztTool _OztTool;
        private readonly IOzLogger _Logger;
        private readonly IUserService _userService;

        public UsersController(IOzLogger logger, OztTool oztTool, IUserService userService)
        {
            _Logger = logger;
            _OztTool = oztTool;
            _userService = userService;
        }
        [HttpGet("[action]")]
        [OztActionFilter(Permissions = "superadmin")]
        public async Task<ServiceResponse> GetAllUsers([FromQuery] RequestPagenationDto _dto)
        {
            try
            {
                var _response = await _userService.GetUsersPagedAsync(_dto.PageNumber, _dto.PageSize);

                return new ServiceResponse
                {
                    Data = _response,
                    Success = true,
                    Message = "Kullanıcılar getirildi"
                };
            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex,
                    _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    _username: HttpContext.GetOztUser()?.Username,
                    _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse
                {
                    Data = null,
                    Success = false,
                    Message = _ex.Message
                };
            }
        }


        [HttpPost("[action]")]
        [OztActionFilter(Permissions = "superadmin")]
        public async Task<ServiceResponse> UpdateUserRole(RequestUpdateUserRole _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                await _userService.UpdateRoleUserAsync(_dto.userId, _dto.newRoleAndPermissionId);

                return new ServiceResponse
                {
                    Data = null,
                    Success = true,
                    Message = "Kullanıcı Role bilgisi güncellendi"
                };

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

                    _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

                    _username: HttpContext.GetOztUser()?.Username,

                    _userId: (long)(HttpContext.GetOztUser()?.UserId));


                return new ServiceResponse
                {

                    Data = null,

                    Success = false,

                    Message = _ex.Message

                };

            }
        }
    }
}