using System.ComponentModel;

namespace KSozluk.WebAPI.Entities
{
    public enum Permission : short
    {
        [Description("Normal Kullanıcı")]
        NormalUser = 1,

        [Description("Admin")]
        Admin = 2,
      
        [Description("Süper Admin")]
        SuperAdmin = 3,
    }
}
