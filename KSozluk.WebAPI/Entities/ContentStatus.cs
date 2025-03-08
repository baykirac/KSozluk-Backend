using System.ComponentModel;
namespace KSozluk.WebAPI.Entities
{
    public enum ContentStatus
    {
        [Description("Onaylı")]
        Onaylı = 1,
        [Description("Bekliyor")]
        Bekliyor = 2,
        [Description("Reddedildi")]
        Reddedildi = 3,
        [Description("Önerildi")]
        Önerildi = 4
    }
}
