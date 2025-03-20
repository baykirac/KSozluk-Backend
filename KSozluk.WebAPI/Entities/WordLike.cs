using Ozcorps.Generic.Entity;

namespace KSozluk.WebAPI.Entities
{
    public sealed class WordLike : EntityBase
    { //NEW LİKE ENTİTY
        public Guid Id { get; set; }
        public Guid WordId { get; set; }
        public long? UserId { get; set; }
        public DateTime Timestamp { get; set; }

        private WordLike(Guid id, Guid wordId, long? userId, DateTime timestamp)
        {
            Id = id;
            WordId = wordId;
            UserId = userId;
            Timestamp = timestamp;
        }

        public static WordLike Create(Guid id, Guid wordId, long? userId, DateTime timestamp)
        {
            return new WordLike(id, wordId, userId, timestamp);
        }
    }

}
