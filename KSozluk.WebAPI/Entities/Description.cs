using KSozluk.WebAPI.Enums;
using KSozluk.WebAPI.SharedKernel;

namespace KSozluk.WebAPI.Entities
{
    public sealed class Description
    {
        public Guid Id { get; private set; }
        public Guid? PreviousDescriptionId { get; private set; }
        public string DescriptionContent { get; private set; }
        public Guid WordId { get; private set; }
        public int Order { get; private set; }
        public ContentStatus Status { get; private set; }
        public User User { get; private set; }
        public long? UserId { get; set; }
        public DateTime LastEditedDate { get; private set; }
        public Word Word { get; private set; }
        public Description PreviousDescription { get; private set; }
        public int? RejectionReasons { get; private set; }
        public string CustomRejectionReason { get; private set; }

        public Description() { }
        private Description(Guid id, string description, int order, ContentStatus status, DateTime lastEditedDate, long? userId)
        {
            Id = id;
            DescriptionContent = description;
            Order = order;
            Status = status;
            LastEditedDate = lastEditedDate;
            UserId = userId;
        }
        private Description(Guid id, string description, int order, ContentStatus status, DateTime lastEditedDate, long userId)
        {
            Id = id;
            DescriptionContent = description;
            Order = order;
            Status = status;
            LastEditedDate = lastEditedDate;
            UserId = userId;
        }

        private Description(Guid id, string description, int order, ContentStatus status, DateTime lastEditedDate,long? userId, Guid? previousDescId)
        {
            Id = id;
            DescriptionContent = description;
            Order = order;
            Status = status;
            LastEditedDate = lastEditedDate;
            UserId = userId;
            PreviousDescriptionId = previousDescId;
        }

        public static Description Create(string description, int order, long? userId) // admin için
        {
            var id = Guid.NewGuid();
            var lastEditedDate = DateTime.Now;

            if (String.IsNullOrEmpty(description))
            {
                throw new DomainException("DescriptionNullOrEmptyException", "Açıklama null veya boş olamaz.");
            }

            if (String.IsNullOrWhiteSpace(description))
            {
                throw new DomainException("DescriptionNullOrWhiteException", "Açıklama null veya boşluk karakterinden oluşamaz.");
            }

            if(description.Length > 2000)
            {
                throw new DomainException("DescriptionNotInRange", "Açıklama 2000 karakterden fazla olamaz.");
            }

            return new Description(id, description, order, ContentStatus.Onaylı, lastEditedDate, userId);
        }

        public static Description Create(string description, int order, long? userId, Guid? previousDescId) // öneri yapan kullanıcılar için
        {
            var id = Guid.NewGuid();
            var lastEditedDate = DateTime.Now;

            if (String.IsNullOrEmpty(description))
            {
                throw new DomainException("DescriptionNullOrEmptyException", "Açıklama null veya boş olamaz.");
            }

            if (String.IsNullOrWhiteSpace(description))
            {
                throw new DomainException("DescriptionNullOrWhiteException", "Açıklama null veya boşluk karakterinden oluşamaz.");
            }

            if (description.Length > 2000)
            {
                throw new DomainException("DescriptionNotInRange", "Açıklama 2000 karakterden fazla olamaz.");
            }

            return new Description(id, description, order, ContentStatus.Önerildi, lastEditedDate, userId, previousDescId);         

        }
        public void UpdateContent(string Content)
        {
            DescriptionContent = Content;
        }

        public void UpdateRecommender(long? id)
        {
            UserId = id;
        }

        public void UpdateAcceptor(long? id)
        {
            UserId = id;
        }

        public void UpdateOrder(int order)
        {   
            Order = order;
        }

        public void UpdateStatus(ContentStatus status)
        {
            Status = status;
        }

        public void UpdateRejectionReasons(int? rejectionReasons, string customRejectionReason)
        {
            RejectionReasons = rejectionReasons;
            CustomRejectionReason = customRejectionReason;
        }
    }
}
