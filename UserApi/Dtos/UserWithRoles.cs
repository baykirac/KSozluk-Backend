using Ozcorps.Generic.Entity;
using UserApi.Entities;

namespace UserApi.Dtos;

public class UserWithRoles : EntityBase
{
    public User User { get; set; }
    public List<UserRoleDescription> Roles { get; set; }
}