using Ozcorps.Generic.Entity;

namespace UserApi.Entities;

public partial class UserSimple : EntityBase
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }

}