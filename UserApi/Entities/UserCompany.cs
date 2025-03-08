using Ozcorps.Generic.Entity;

namespace UserApi.Entities;

public partial class UserCompany : EntityBase, IModifiedEntity
{
    public long UserId { get; set; }
    public long CompanyId { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    public DateTime? InsertedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public long? InsertedUserId { get; set; }
    public long? ModifiedUserId { get; set; }
}