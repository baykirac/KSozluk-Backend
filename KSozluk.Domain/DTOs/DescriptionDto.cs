namespace KSozluk.Domain.DTOs
{
    public class DescriptionDto
    {
        public Guid Id { get; set; }
        public Guid? PreviousDescId { get; set; }
        public string DescriptionContent { get; set; }
        public Guid WordId { get; set; }
        public int Order { get; set; }
        public ContentStatus Status { get; set; }
        public Guid? AcceptorId { get; set; }
        public Guid? RecommenderId { get; set; }
        public DateTime LastEditedDate { get; set; }
        public string PreviousDescriptionContent { get; set; }
    }

    public class DescriptionWithIsLikeDto
    {
        public Guid Id { get; set; }
        public string DescriptionContent { get; set; }       
        public bool isLike { get; set; }
    }
}
