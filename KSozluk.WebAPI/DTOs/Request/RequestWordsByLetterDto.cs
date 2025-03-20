using System;

namespace KSozluk.WebAPI.DTOs;

public class RequestWordsByLetterDto
{
    public char Letter { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
