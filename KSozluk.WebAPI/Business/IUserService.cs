using System;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.Entities;

namespace KSozluk.WebAPI.Business
{
    public interface IUserService
    {
        Task<PagedResult<UserDto>> GetUsersPagedAsync(int pageNumber, int pageSize);
 
         Task<UserDto> UpdateRoleUserAsync(long? userId, long newRoleAndPermissionId);

    }
}
 
