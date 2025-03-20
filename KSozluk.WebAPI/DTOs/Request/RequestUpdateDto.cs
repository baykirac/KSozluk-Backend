using System;

namespace KSozluk.WebAPI.DTOs;

public class RequestUpdateDto
{
    public Guid DescriptionId { get; set; }
    public int Order { get; set; }
}
