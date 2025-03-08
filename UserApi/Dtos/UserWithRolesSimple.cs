using Ozcorps.Generic.Entity;
using UserApi.Entities;

namespace UserApi.Dtos;

public class UserWithRolesSimple : EntityBase
{
    public UserSimple User { get; set; }
    public List<UserRoleDescriptionSimple> Roles { get; set; }
}