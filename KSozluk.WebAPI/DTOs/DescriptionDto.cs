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

    public class DescriptionWithDetailsDto
    {
        public List<DescriptionWithIsLikeDto> Body { get; set; }
        public bool IsFavourited { get; set; }
        public bool IsLikedWord { get; set; }
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

    public class FavouriteWordDto
    {
        public Guid WordId { get; set; }
        public long? UserId { get; set; }
    }

    public class FavouriteWordContentDto
    {
        public string WordContent { get; set; }
        public Guid Id { get; set; }
    }

    public class DescriptionTimelineDto
    {
        public Guid Id { get; set; }
        public ContentStatus Status { get; set; }
        public Guid WordId { get; set; }
        public string DescriptionContent { get; set; }
        public string WordContent { get; set; }
        public int? RejectionReasons { get; set; }
        public string CustomRejectionReason { get; set; }
        public bool IsActive { get; set; }
    }

    public class LikeDescriptionDto
    {
        public Guid Id { get; set; }
        public long? UserId { get; set; }
    }

    public class TopWordListDto
    {
        public Guid WordId { get; set; }
        public string Word { get; set; }
        public int Count { get; set; }
    }

    public class ReccomendDescriptionDto
    {
        public Guid Id { get; set; }
        public string DescriptionContent { get; set; }
        public DateTime LastEditedDate { get; set; }

    }

    public class UpdateOrderDto
    {
        public Guid DescriptionId { get; set; }
        public int Order { get; set; }
    }
    public class UpdateStatusDto
    {
        public Guid DescriptionId { get; set; }
        public ContentStatus Status { get; set; }
        public int RejectionReasons { get; set; }
        public string CustomRejectionReason { get; set; }
    }

    public class UpdateIsActiveDto
    {
        public Guid DescriptionId { get; set; }
        public bool IsActive { get; set; }
    }
}
