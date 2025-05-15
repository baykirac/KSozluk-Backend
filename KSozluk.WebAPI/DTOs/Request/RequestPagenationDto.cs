using System;

namespace KSozluk.WebAPI.DTOs.Request;

public class RequestPagenationDto
{
  public int PageNumber { get; set; }
  public int PageSize { get; set; }

  // Filtre alanları
  public string? Username { get; set; }
  public string? Name { get; set; }
  public string? Surname { get; set; }
  public string? Email { get; set; }
}
