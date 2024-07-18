using KSozluk.Domain.SharedKernel;

namespace KSozluk.Domain
{
    public sealed class Descriptions
    {
        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public Words Word { get; private set; }
        public double Order { get; private set; }
        public bool Status { get; private set; }

        public Descriptions(Guid id, string description, double order, bool status)
        {
            Id = id;
            Description = description;
            Order = order;
            Status = status;
        }

        public static Descriptions Create(Guid id, string description, double order, bool status)
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

            return new Descriptions(id, description, order, status);
        }
    }
}
