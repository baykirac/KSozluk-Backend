using System;

namespace KSozluk.WebAPI.DTOs.Request;
public class UserDto
{
    public long Id { get; set; }
    public bool IsActive { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public long CompanyId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public long UserId { get; set; } 
    public int Retires { get; set; }
    public DateTime? RetriesDate { get; set; }
    public DateTime? InsertedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string CompanyName { get; set; }
}
