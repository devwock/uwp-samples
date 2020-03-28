using System;
using System.Linq;
using System.Reflection;

namespace LanguageChange
{
    public class AttributeValue : Attribute
    {
        public string Value { get; private set; }

        public AttributeValue(string value)
        {
            Value = value;
        }
    }

    public class LocalizedAttribute : Attribute
    {
        private string value;

        public string Value
        {
            get => LanguageManager.Instance.GetString(value);
            private set
            {
                this.value = value;
            }
        }

        public LocalizedAttribute(string resourceId)
        {
            Value = resourceId;
        }
    }

    public static class AttributeExtension
    {
        public static string GetAttributeValue<T>(this object value) where T : AttributeValue
        {
            MemberInfo memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();

            if (memberInfo != null)
            {
                var attribute = memberInfo.GetCustomAttributes(typeof(T), false).OfType<T>().Where(x => x.GetType() == typeof(T));
                var returnValue = (T)attribute.FirstOrDefault();
                return returnValue.Value;
            }
            return string.Empty;
        }

        public static string GetLocalizedAttributeValue(this object enumValue)
        {
            MemberInfo memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            if (memberInfo != null)
            {
                var attribute = memberInfo.GetCustomAttributes(typeof(LocalizedAttribute), false).OfType<LocalizedAttribute>().Where(x => x.GetType() == typeof(LocalizedAttribute));
                var returnValue = attribute.FirstOrDefault();
                return returnValue.Value;
            }
            return string.Empty;
        }
    }
}