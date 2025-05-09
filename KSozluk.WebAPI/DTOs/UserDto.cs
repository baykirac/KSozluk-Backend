using System;

namespace KSozluk.WebAPI.DTOs;
public class UserDto
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public long Id { get; set; } 
    public long RoleId { get; set; }
    public long PermissionId { get; set; }
}

public class UserPermissionDto
{
    public long UserId { get; set; }
    public long PermissionId { get; set; }
}

public class PagedResult<T>
{
    public int TotalCount { get; set; }
    public List<T> Items { get; set; }
}


public class UserRoleDto
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
}