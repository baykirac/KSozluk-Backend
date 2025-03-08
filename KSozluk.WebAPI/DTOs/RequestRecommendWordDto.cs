using System;

namespace KSozluk.WebAPI.DTOs;

public class RequestRecommendWordDto
{
    public string WordContent { get; set; }
    public List<string> DescriptionContent { get; set; }
}
