using KSozluk.WebAPI.Entities;

namespace KSozluk.WebAPI.DTOs
{
    public class DescriptionDto
    {
        public Guid Id { get; set; }
        public Guid? PreviousDescId { get; set; }
        public string DescriptionContent { get; set; }
        public Guid WordId { get; set; }
        public int Order { get; set; }
        public ContentStatus Status { get; set; }
        public long UserId { get; set; }
        public DateTime LastEditedDate { get; set; }
        public string PreviousDescriptionContent { get; set; }
        public bool IsActive { get; set; }
        public int? RejectionReasons { get; set; }
        public long? AcceptorId { get; set; }
        public string CustomRejectionReason { get; set; }
    }

    public class DescriptionWithIsLikeDto
    {
        public Guid Id { get; set; }
        public string DescriptionContent { get; set; }       
        public bool isLike { get; set; }
    }

    public class DescriptionHeaderNameDto
    {
        public Guid Id { get; set; }
        public string DescriptionContent { get; set; }

    }

    public class DescriptionReccomendDto
    {
        public string DescriptionContent { get; set; }
        public DateTime LastEditedDate { get; set; }
    }

    public class AcceptorIdDto
    {
        public long? AcceptorId { get; set; }
    }

    public class DescriptionTimelineDto 
    {
        public Guid Id { get; set; }
        public ContentStatus Status { get; set; }
        public Guid WordId {get; set; }
        public string DescriptionContent { get; set; }
        public string WordContent { get; set; }
        public int? RejectionReasons { get; set; }
        public string CustomRejectionReason { get; set; }
        public bool IsActive{ get; set; }
    }
}
