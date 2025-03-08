using UserApi.Entities;
using Ozcorps.Core.Models;

namespace UserApi.Dtos;

public class ResponseValidateUserDto : Response
{
    public User User { get; set; }

    public List<Permission> Permissions { get; set; }

    public List<Role> Roles { get; set; }

    public bool IsLdapLogin { get; set; }

    public int Retries { get; set; }
}
