using System;

namespace KSozluk.WebAPI.DTOs;

public class RequestDeletedDto
{
    public Guid WordId { get; set; }
}

public class RequestDeleteDescriptionDto
{
    public Guid DescriptionId { get; set; }
}
