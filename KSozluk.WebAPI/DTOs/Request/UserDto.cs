using System;

namespace KSozluk.WebAPI.DTOs;
public class UserDto
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public long Id { get; set; } 
}
