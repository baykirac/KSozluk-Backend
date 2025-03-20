using KSozluk.WebAPI.Entities;

namespace KSozluk.WebAPI.DTOs
{
    public class WordDto
    {
        public Guid Id { get; set; }
        public string WordContent { get; set; }
        public ContentStatus Status { get; set; }
        public long UserId { get; set; }
        public DateTime LastEditedDate { get; set; }
        public List<DescriptionDto> Descriptions { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class AddWordResultDto
    {
        public Guid Id { get; set; }
        public string WordContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public int DescriptionCount { get; set; }
    }

    public class GetLastEditDto
    {
        public string WordContent { get; set; }
        public DateTime LastEditedDate { get; set; }
        public ContentStatus Status { get; set; }
    }

    public class WordSearchResultDto
    {
        public Guid Id { get; set; }
        public string WordContent { get; set; }
    }


    public class WordPagedResultDto
    {
        public List<WordListDto> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class WordListDto
    {
        public Guid Id { get; set; }
        public string WordContent { get; set; }
        public ContentStatus Status { get; set; }
    }

    public class UpdateWordByIdResultDto
    {
        public Guid Id { get; set; }
        public string WordContent { get; set; }
    }

    // UpdateWordResultDto eksikse ekleyelim
    public class UpdateWordResultDto
    {
        public Guid Id { get; set; }
        public string WordContent { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid DescriptionId { get; set; }
        public string DescriptionContent { get; set; }
    }


    public class PagedWordDto
    {
        public char Letter { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<WordSearchResultDto> WordResults { get; set; }
    }

    public class LikeResultDto
    {
        public bool IsLiked { get; set; }
        public int TotalLikes { get; set; }
    }

    public class RecommendWordResultDto
    {
        public Guid Id { get; set; }
        public string WordContent { get; set; }
        public DateTime SubmitDate { get; set; }
    }

    public class RecentWordDto
    {
        public string WordContent { get; set; }
        public DateTime LastEditedDate { get; set; }
    }
}
