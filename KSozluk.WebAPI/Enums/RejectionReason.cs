using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.WebAPI.Enums
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
        [Description("Karmaşık veya Anlaşılmaz İfade")]
        V5 = 5,
        [Description("Birden Fazla Anlam")]
        V6 = 6,
        [Description("Diğer")]
        V7 = 7
    }
}
