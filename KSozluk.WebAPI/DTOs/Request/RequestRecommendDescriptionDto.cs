using System;

namespace KSozluk.WebAPI.DTOs;

public class RequestRecommendDescriptionDto
{

    public Guid WordId { get; set; }
    public Guid? PreviousDescriptionId { get; set; }
    public string Content { get; set; }
}
