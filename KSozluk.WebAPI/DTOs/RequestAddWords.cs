using System;

namespace KSozluk.WebAPI.DTOs;

public class RequestAddWords
{
    public string WordContent { get; set; }
    public List<string> Description { get; set; }
}