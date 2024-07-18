using KSozluk.Domain.SharedKernel;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace KSozluk.Domain
{
    public sealed class Words
    {
        public Guid Id { get; private set; }
        public string Word { get; private set; }
        public bool Status { get; private set; }
        public ICollection<Descriptions> Descriptions { get; private set; }

        public Words() { }
        private Words(Guid id, string word, bool status)
        {
            Id = id;
            Word = word;
            Status = status;
        }

        public Words(Guid id, string word)
        {
            Id = id;
            Word = word;
        }

        public static Words Create(Guid id, string word)
        {
            if(String.IsNullOrEmpty(word))
            {
                throw new DomainException("WordNullOrEmptyException", "Kelime null veya boşluktan oluşamaz.");
            }

            if (String.IsNullOrWhiteSpace(word))
            {
                throw new DomainException("WordNullOrSpaceException", "Kelime null veya boşluk karakterlerinden oluşamaz.");
            }

            if(word.Length > 255)
            {
                throw new DomainException("WordNotInRange", "Kelime 255 karakterden fazla olamaz.");
            }

            return new Words(id, word);
        }

        public static Words Create(Guid id, string word, bool status)
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

            return new Words(id, word, status);
        }
    }
}
