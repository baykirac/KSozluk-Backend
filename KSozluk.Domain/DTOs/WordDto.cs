namespace KSozluk.Domain.DTOs
{
    public class WordDto
    {
        public Guid Id { get; set; }
        public string WordContent { get; set; }
        public ContentStatus Status { get; set; }
        public Guid? AcceptorId { get; set; }
        public Guid? RecommenderId { get; set; }
        public DateTime LastEditedDate { get; set; }
        public List<DescriptionDto> Descriptions { get; set; }
    }
}
