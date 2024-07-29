using KSozluk.Domain.SharedKernel;
using System.Collections.ObjectModel;

namespace KSozluk.Domain
{
    public sealed class Word
    {
        public Guid Id { get; private set; }
        public string WordContent { get; private set; }
        public ContentStatus Status { get; private set; }
        public List<Description> Descriptions { get; private set; }
        public Guid? AcceptorId { get; private set; }
        public Guid? RecommenderId { get; private set; }
        public User Acceptor {  get; private set; }
        public User Recommender { get; private set; }
        public DateTime LastEditedDate { get; private set; }
        private List<Description> _descriptions = new List<Description>();
        public Word()
        {
            Descriptions = _descriptions;
        }

        private Word(Guid id, string word, ContentStatus status, Guid acceptorId, DateTime lastEditedDate)
        {
            Id = id;
            WordContent = word;
            Status = status;
            AcceptorId = acceptorId;
            LastEditedDate = lastEditedDate;
        }

        private Word(string word, ContentStatus status, Guid acceptorId, Guid recommenderId)
        {
            Id = Guid.NewGuid();
            WordContent = word;
            Status = status;
            AcceptorId = acceptorId;
            RecommenderId = recommenderId;
        }
        private Word(string word, ContentStatus status, Guid acceptorId)
        {
            Id = Guid.NewGuid();
            WordContent = word;
            Status = status;
            AcceptorId = acceptorId;
        }

        private Word(string word, ContentStatus status)
        {
            Id = Guid.NewGuid();
            WordContent = word;
            Status = status;
        }

        public Word(string word)
        {
            Id = Guid.NewGuid();
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

            return new Word(word);
        }

        public static Word Create(string word, Guid acceptorId) // admin için 
        {
            DateTime lastEditedDate = DateTime.Now;
            Guid id = Guid.NewGuid();

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

            return new Word(id, word, ContentStatus.Onaylı, acceptorId, lastEditedDate);
        }

        public void  AddDescription(Description description)
        {
            _descriptions.Add(description);
            Descriptions = _descriptions;
        }

        public static void ClearResponse(Word word)
        {
            word.Acceptor = null;
        }
    }
}
