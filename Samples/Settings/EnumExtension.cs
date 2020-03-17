using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Settings
{
    public static class EnumExtension
    {
        public static string GetAttributeValue<T>(this Enum enumValue) where T : AttributeValue
        {
            MemberInfo memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();

            if (memberInfo != null)
            {
                var attribute = memberInfo.GetCustomAttributes(typeof(T), false).OfType<T>().Where(x => x.GetType() == typeof(T));
                var returnValue = (T)attribute.FirstOrDefault();
                return returnValue.Value;
            }
            return string.Empty;
        }

        public static T GetEnumFromAttribute<T>(string value)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(AttributeValue)) as AttributeValue;
                if (attribute != null)
                {
                    if (attribute.Value == value)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == value)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }
            throw new ArgumentException("Not found.", nameof(value));
        }
    }
}