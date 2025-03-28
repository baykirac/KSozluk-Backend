using KSozluk.WebAPI.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.WebAPI.Extensions
{
    public static class EnumHelper
    {
        public static string GetEnumDescription(Enum value)
        {

            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
            
            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}
