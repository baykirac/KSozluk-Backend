using System;

namespace KSozluk.WebAPI.DTOs;

public class RequestIsActiveDto
{
   public Guid DescriptionId { get; set; }
   public bool IsActive { get; set; }
}
