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
        public DateTime OperationDate { get; private set; }

        private List<Description> _descriptions = new List<Description>();
        public Word()
        {
            Descriptions = _descriptions;
 
        }

        private Word(Guid id, string word, ContentStatus status, Guid acceptorId, DateTime lastEditedDate, DateTime operationDate)
        {
            Id = id;
            WordContent = word;
            Status = status;
            AcceptorId = acceptorId;
            RecommenderId = acceptorId;
            LastEditedDate = lastEditedDate;
            OperationDate = operationDate;
        }

        private Word(Guid id, string word, ContentStatus status, Guid? acceptorId, Guid recommenderId, DateTime lastEditedDate, DateTime operationDate)
        {
            Id = id;
            WordContent = word;
            Status = status;
            AcceptorId = acceptorId;
            RecommenderId = recommenderId;
            LastEditedDate = lastEditedDate;
            OperationDate = operationDate;
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
            DateTime operationDate = DateTime.Now;
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

            return new Word(id, word, ContentStatus.Onaylı, acceptorId, lastEditedDate, operationDate);
        }

        public static Word Create(string word, Guid recommenderId, DateTime lastEditedDate, DateTime operationDate) //yeni bir kelime ve anlam önerme işlemi
        {
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

            return new Word(id, word, ContentStatus.Bekliyor, null, recommenderId, lastEditedDate, operationDate);
        }

        public static Word UpdateOperationDate(Word _dto, DateTime _date)
        {
            _dto.OperationDate = _date;

            return _dto;
        }

        public void AddDescription(Description description)
        {
            _descriptions.Add(description);
            Descriptions = _descriptions;
           
        }

        public static void ClearResponse(Word word)
        {
            word.Acceptor = null;
           
        }

        public void ChangeContent(string content)
        {
            WordContent = content;
        
        }

        public void UpdateStatus(ContentStatus status)
        {
            Status = status;
          
        }

        public void RemoveDescription(Description description)
        {
            _descriptions.Remove(description);
            Descriptions = _descriptions;
           
        }

        
    }
}
