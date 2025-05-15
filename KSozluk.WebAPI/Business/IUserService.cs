using System;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.DTOs.Request;
using KSozluk.WebAPI.Entities;

namespace KSozluk.WebAPI.Business
{
    public interface IUserService
    {
        Task<PagedResult<UserDto>> GetUsersPagedAsync(RequestPagenationDto dto);
 
         Task<UserDto> UpdateRoleUserAsync(long? userId, long newRoleAndPermissionId);

    }
}
 
