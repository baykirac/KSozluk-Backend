using KSozluk.WebAPI.SharedKernel;

namespace KSozluk.WebAPI.Entities
{
    public sealed class Word
    {
        public Guid Id { get; private set; }
        public string WordContent { get; private set; }
        public ContentStatus Status { get; private set; }
        public List<Description> Descriptions { get; private set; }
        public long? UserId { get; private set; }
        public User User { get; private set; }
        public DateTime LastEditedDate { get; private set; }
        public DateTime OperationDate { get; private set; }
        public List<Description> _descriptions = new List<Description>();
        public Word()
        {
            Descriptions = _descriptions;
 
        }

        private Word(Guid id, string word, ContentStatus status, long? userId, DateTime lastEditedDate, DateTime operationDate)
        {
            Id = id;
            WordContent = word;
            Status = status;
            UserId = userId;
            LastEditedDate = lastEditedDate;
            OperationDate = operationDate;
        }

        private Word(Guid id, string word, ContentStatus status, long userId, DateTime lastEditedDate, DateTime operationDate)
        {
            Id = id;
            WordContent = word;
            Status = status;
            UserId = userId;
            LastEditedDate = lastEditedDate;
            OperationDate = operationDate;
        }
        private Word(string word, ContentStatus status, long userId)
        {
            Id = Guid.NewGuid();
            WordContent = word;
            Status = status;
            UserId = userId;          
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

        public static Word Create(long? userId, string word)
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

        public static Word Create(string word, long? userId) // admin için 
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

            return new Word(id, word, ContentStatus.Onaylı, userId, lastEditedDate, operationDate);
        }

        public static Word Create(string word, long? userId, DateTime lastEditedDate, DateTime operationDate) //yeni bir kelime ve anlam önerme işlemi
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

            return new Word(id, word, ContentStatus.Bekliyor, userId, lastEditedDate, operationDate);
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
            word.User = null;
           
        }

        public void ChangeContent(string Content)
        {
            WordContent = Content;
        
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
