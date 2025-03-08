using System;

namespace KSozluk.WebAPI.DTOs;

public class RequestUpdateWordById
{
    public Guid WordId { get; set; }
    public string WordContent { get; set; }
}
