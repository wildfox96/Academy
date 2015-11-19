using System;

namespace AUPPRB.Common.Attributes
{
    public class EnumText : Attribute
    {
        public readonly String value;

        public EnumText(String val)
        {
            value = val;
        }

        public String Value
        {
            get { return value; }
        }

    }
}
