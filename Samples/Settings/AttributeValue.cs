using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings
{
    public class AttributeValue : Attribute
    {
        public string Value { get; private set; }

        public AttributeValue(string value)
        {
            this.Value = value;
        }
    }
}