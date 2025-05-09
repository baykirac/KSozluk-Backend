using System;
using Ozcorps.Generic.Entity;

namespace KSozluk.WebAPI.Entities;

public class UserRole : EntityBase
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    public DateTime? InsertedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public long? InsertedUserId { get; set; }
    public long? ModifiedUserId { get; set; }
}
