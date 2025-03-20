using System;

namespace KSozluk.WebAPI.DTOs;

public class PaginationMeta
{
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}
