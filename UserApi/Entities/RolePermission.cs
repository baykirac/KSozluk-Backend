using Ozcorps.Generic.Entity;

namespace UserApi.Entities;

public sealed class RolePermission : EntityBase, IModifiedEntity
{
    public long RoleId { get; set; }
    public long PermissionId { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    public DateTime? InsertedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public long? InsertedUserId { get; set; }
    public long? ModifiedUserId { get; set; }
}