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
    }
}
