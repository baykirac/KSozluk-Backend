using Ozcorps.Generic.Entity;

namespace UserApi.Entities;

public sealed class UserRoleDescription : EntityBase, IInsertedEntity
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public string RoleName { get; set; }
    public DateTime InsertedDate { get; set; }
    public long InsertedUserId { get; set; }
}