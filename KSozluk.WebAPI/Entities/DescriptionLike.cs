using Ozcorps.Generic.Entity;

namespace KSozluk.WebAPI.Entities
{
    public class DescriptionLike: EntityBase
    {
        public long? UserId { get; set; }
        public Guid Id { get; set; }
        public Guid DescriptionId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
