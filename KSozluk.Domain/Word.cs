using KSozluk.Domain.SharedKernel;

namespace KSozluk.Domain
{
    public sealed class Word
    {
        public Guid Id { get; private set; }
        public string WordContent { get; private set; }
        public bool Status { get; private set; }
        public ICollection<Description> Descriptions { get; private set; }

        public Word() { }
        private Word(Guid id, string word, bool status)
        {
            Id = id;
            WordContent = word;
            Status = status;
        }

        public Word(Guid id, string word)
        {
            Id = id;
            WordContent = word;
        }

        public static Word Create(Guid id, string word)
        {
            if (String.IsNullOrEmpty(word))
            {
                throw new DomainException("WordNullOrEmptyException", "Kelime null veya boşluktan oluşamaz.");
            }

            if (String.IsNullOrWhiteSpace(word))
            {
                throw new DomainException("WordNullOrSpaceException", "Kelime null veya boşluk karakterlerinden oluşamaz.");
            }

            if (word.Length > 255)
            {
                throw new DomainException("WordNotInRange", "Kelime 255 karakterden fazla olamaz.");
            }

            return new Word(id, word);
        }

        public static Word Create(Guid id, string word, bool status)
        {
            if (String.IsNullOrEmpty(word))
            {
                throw new DomainException("WordNullOrEmptyException", "Kelime null veya boşluktan oluşamaz.");
            }

            if (String.IsNullOrWhiteSpace(word))
            {
                throw new DomainException("WordNullOrSpaceException", "Kelime null veya boşluk karakterlerinden oluşamaz.");
            }

            if (word.Length > 255)
            {
                throw new DomainException("WordNotInRange", "Kelime 255 karakterden fazla olamaz.");
            }

            return new Word(id, word, status);
        }
    }
}
