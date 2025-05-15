using System;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.Entities;
using Ozcorps.Generic.Bll;
using Ozcorps.Generic.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KSozluk.WebAPI.DTOs.Request;


namespace KSozluk.WebAPI.Business
{
        public class UserService : DbServiceBase, IUserService
        {
                private readonly IRepository<User> _UserRepository;
                private readonly IRepository<UserRole> _RoleRepository;
                private readonly IRepository<UserPermission> _PermissionRepository;

                public UserService(IUnitOfWork _unitOfWork) : base(_unitOfWork)
                {
                        _UserRepository = _unitOfWork.GetRepository<User>();

                        _RoleRepository = _unitOfWork.GetRepository<UserRole>();

                        _PermissionRepository = _unitOfWork.GetRepository<UserPermission>();
                }
                public async Task<PagedResult<UserDto>> GetUsersPagedAsync(RequestPagenationDto dto)
                {
                        var query = _UserRepository.GetQueryable()
                            .Where(x => !x.IsDeleted);

                        if (!string.IsNullOrEmpty(dto.Username))
                                query = query.Where(x => x.Username.Contains(dto.Username));

                        if (!string.IsNullOrEmpty(dto.Name))
                                query = query.Where(x => x.Name.Contains(dto.Name));

                        if (!string.IsNullOrEmpty(dto.Surname))
                                query = query.Where(x => x.Surname.Contains(dto.Surname));

                        if (!string.IsNullOrEmpty(dto.Email))
                                query = query.Where(x => x.Email.Contains(dto.Email));

                        var totalCount = await query.CountAsync();

                        var users = await query
                            .OrderBy(x => x.Id)
                            .Skip((dto.PageNumber - 1) * dto.PageSize)
                            .Take(dto.PageSize)
                            .Select(x => new UserDto
                            {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Surname = x.Surname,
                                    Username = x.Username,
                                    Email = x.Email,
                                    RoleId = _RoleRepository.GetQueryable()
                                    .Where(r => r.UserId == x.Id)
                                    .Select(r => r.RoleId)
                                    .FirstOrDefault(),
                                    PermissionId = _PermissionRepository.GetQueryable()
                                    .Where(p => p.UserId == x.Id)
                                    .Select(p => p.PermissionId)
                                    .FirstOrDefault()
                            })
                            .ToListAsync();

                        return new PagedResult<UserDto>
                        {
                                TotalCount = totalCount,
                                Items = users
                        };
                }

                public async Task<UserDto> UpdateRoleUserAsync(long? userId, long newRoleAndPermissionId)
                {
                        var user = await _UserRepository.GetQueryable()
                         .Where(x => x.Id == userId && !x.IsDeleted)
                          .FirstOrDefaultAsync();

                        if (user == null)
                        {
                                return null;
                        }

                        var userRole = await _RoleRepository.GetQueryable()
                            .FirstOrDefaultAsync(x => x.UserId == userId);

                        if (userRole != null)
                        {
                                userRole.RoleId = newRoleAndPermissionId;
                        }
                        else
                        {
                                return null;
                        }

                        var userPermission = await _PermissionRepository.GetQueryable()
                            .FirstOrDefaultAsync(x => x.UserId == userId);

                        if (userPermission != null)
                        {
                                userPermission.PermissionId = newRoleAndPermissionId;
                        }
                        else
                        {
                                return null;
                        }

                        _UnitOfWork.Save();

                        var updatedUser = new UserDto
                        {
                                Id = user.Id,
                                Name = user.Name,
                                Surname = user.Surname,
                                Username = user.Username,
                                Email = user.Email,
                                RoleId = userRole != null ? userRole.RoleId : 2,
                                PermissionId = userPermission != null ? userPermission.PermissionId : 2
                        };
                        return updatedUser;

                }
        }
}