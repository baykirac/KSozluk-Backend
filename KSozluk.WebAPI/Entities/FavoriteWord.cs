
using Ozcorps.Generic.Entity;

namespace KSozluk.WebAPI.Entities 
{
    public class FavoriteWord: EntityBase
    {
        public Guid Id { get; set; }
        public long? UserId { get; set; }     
        public Guid WordId { get; set; }

    }
}
