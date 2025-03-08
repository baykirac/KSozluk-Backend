using Ozcorps.Generic.Entity;

namespace KSozluk.WebAPI.Entities;

public partial class User 
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Retries { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsLdap { get; set; }
    public long? InsertedUserId { get; set; }
    public long? ModifiedUserId { get; set; }
    public DateTime? InsertedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public DateTime? RetriesDate { get; set; }
}