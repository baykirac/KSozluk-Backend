using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Domain.Enums
{
 
    public enum RejectionReasons
    {
        [Description("Uygunsuz")]
        V1 = 1,
        [Description("Zaten mevcut")]
        V2 = 2,
        [Description("Eksik Açıklama")]
        V3 = 3,
        [Description("Yanlış Tanım")]
        V4 = 4,
        [Description("Daha Önceden Reddedilen Tanım")]
        V5 = 5,
        [Description("Karmaşık veya Anlaşılmaz İfade")]
        V6 = 6,
        [Description("Aşırı Teknik veya Özel Terminoloji")]
        V7 = 7,
        [Description("Kelimeyle Alakasız")]
        V8 = 8,
        [Description("Çok Yönlü Tanım")]
        V9 = 9,
        [Description("Yanlış Bilgi Veriyor")]
        V10 = 10,
        [Description("Farklı Anlamlar Karışmış")]
        V11 = 11,
        [Description("Diğer")]
        V12 = 12
    }
}
