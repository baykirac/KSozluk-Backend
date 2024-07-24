using KSozluk.Domain.SharedKernel;

namespace KSozluk.Domain
{
    public sealed class Description
    {
        public Guid Id { get; private set; }
        public string DescriptionContent { get; private set; }
        public Word Word { get; private set; }
        public double Order { get; private set; }
        public ContentStatus Status { get; private set; }

        public Description() { }
        public Description(Guid id, string description, double order, ContentStatus status)
        {
            Id = id;
            DescriptionContent = description;
            Order = order;
            Status = status;
        }

        public static Description Create(Guid id, string description, double order, ContentStatus status)
        {
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

            return new Description(id, description, order, status);
        }
    }
}
