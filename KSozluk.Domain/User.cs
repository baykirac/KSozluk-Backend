using KSozluk.Domain.SharedKernel;
using System.Text.RegularExpressions;

namespace KSozluk.Domain
{
    public sealed class User
    {
        public Guid Id { get; set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public Permission Permissions { get; private set; }
        public string RefreshToken { get; private set; }
        public DateTime? TokenExpireDate { get; private set; }

        public User() { }

        public User(Guid id, string fullName, string email, string password, Permission permissions)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            Password = password;
            Permissions = permissions;
        }

        public static User Creaate(Guid id, string fullName, string email, string password, Permission permissions)
        {
            if (String.IsNullOrEmpty(fullName))
            {
                throw new DomainException("FullNameNullOrEmptyException", "İsim null veya boş olamaz.");
            }

            if (String.IsNullOrWhiteSpace(fullName))
            {
                throw new DomainException("FullNameNullOrWhiteSpaceException", "İsim null veya boşluk karakterlerinden oluşamaz.");
            }

            if(fullName.Length > 255)
            {
                throw new DomainException("FullNameNotInRange", "İsim 255 karakterden fazla olamaz");
            }

            string emailRegex = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";

            if (!Regex.IsMatch(email, emailRegex))
            {
                throw new DomainException("EmailNotValid", "Email standartlara uymuyor.");
            }

            if (password.Length > 12)
            {
                throw new DomainException("PasswordNotInRange", "Şifre minimum 6 maksimum 12 karakterden oluşmalıdır.");
            }

            if (password.Length < 6)
            {
                throw new DomainException("PasswordNotInRange", "Şifre minimum 6 maksimum 12 karakterden oluşmalıdır.");
            }

            string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).*$";

            if (!Regex.IsMatch(password, passwordRegex))
            {
                throw new DomainException("PasswordNotValid", "Şifre en az bir küçük harf," +
                    " en az bir büyük harf," +
                    " en az bir rakam ve" +
                    " en az bir özel karakter içermelidir.");
            }

            return new User(id, fullName, email, password, permissions);
        }

        public void SignIn(string refreshToken, DateTime tokenExpireDate)
        {
            RefreshToken = refreshToken;
            TokenExpireDate = tokenExpireDate;
        }
    }
}
