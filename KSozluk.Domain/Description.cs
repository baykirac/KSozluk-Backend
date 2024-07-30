using KSozluk.Domain.SharedKernel;

namespace KSozluk.Domain
{
    public sealed class Description
    {
        public Guid Id { get; private set; }
        public string DescriptionContent { get; private set; }
        public Guid WordId { get; private set; }
        public double Order { get; private set; }
        public ContentStatus Status { get; private set; }
        public Guid? AcceptorId { get; private set; }
        public Guid? RecommenderId { get; private set; }
        public User Acceptor { get; private set; }
        public User Recommender { get; private set; }
        public DateTime LastEditedDate { get; private set; }
        public Word Word { get; private set; }  
        public Description() { }
        public Description(Guid id, string description, double order, ContentStatus status, DateTime lastEditedDate, Guid acceptorId)
        {
            Id = id;
            DescriptionContent = description;
            Order = order;
            Status = status;
            LastEditedDate = lastEditedDate;
            AcceptorId = acceptorId;
            RecommenderId = acceptorId;
        }

        public static Description Create(string description, double order, Guid acceptorId) // admin için
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

            if(description.Length > 550)
            {
                throw new DomainException("DescriptionNotInRange", "Açıklama 550 karakterden fazla olamaz.");
            }

            return new Description(id, description, order, ContentStatus.Onaylı, lastEditedDate, acceptorId);
        }

        public void ChangeContent(string content)
        {
            DescriptionContent = content;
        }

        public void ChangeRecommender(Guid id)
        {
            RecommenderId = id;
        }
    }
}
