using System;
using KSozluk.WebAPI.Entities;

namespace KSozluk.WebAPI.DTOs;

public class ResponseGetLastEditDto
{
        public string WordContent { get; set; }
        public DateTime LastEditedDate { get; set; }
        public ContentStatus Status { get; set; }
}
