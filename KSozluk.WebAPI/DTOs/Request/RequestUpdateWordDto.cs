using System;

namespace KSozluk.WebAPI.DTOs;

public class RequestUpdateWordDto
{
    public Guid WordId { get; set; }
    public Guid DescriptionId { get; set; }
    public string WordContent { get; set; }    
    public string DescriptionContent { get; set; }
}
