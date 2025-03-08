using Ozcorps.Generic.Entity;

namespace UserApi.Models;

public class Settings : EntityBase, IModifiedEntity
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public long? InsertedUserId { get; set; }
    public long? ModifiedUserId { get; set; }
    public long? DeletedUserId { get; set; }
    public bool? IsEncryption { get; set; }
    public long? CompanyId { get; set; }
    public DateTime? InsertedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public DateTime? DeleteDate { get; set; }

}