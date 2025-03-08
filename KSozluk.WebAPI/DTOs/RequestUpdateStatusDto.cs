using System;
using KSozluk.WebAPI.Entities;

namespace KSozluk.WebAPI.DTOs;

public class RequestUpdateStatusDto
{
    public Guid DescriptionId { get; set; }
    public ContentStatus Status { get; set; }
    public int RejectionReasons { get; set; }
    public string CustomRejectionReason { get; set; }

}