using System;
using System.Reflection;
using AUPPRB.Common.Attributes;

namespace AUPPRB.Common.Extensions
{
    public static class EnumExtensions
    {
        public static String GetEnumText(this Enum e)
        {
            Type type = e.GetType();

            MemberInfo[] memberInfos = type.GetMember(e.ToString());

            if (memberInfos != null && memberInfos.Length > 0)
            {
                object[] attributes = memberInfos[0].GetCustomAttributes(typeof(EnumText),
                    false);
                if (attributes != null && attributes.Length > 0)
                    return ((EnumText)attributes[0]).Value;
            }
            throw new ArgumentException("Enum " + e + " has no EnumText defined!");
        }

        public static int GetEnumValue(this Enum e)
        {
            Type type = e.GetType();

            MemberInfo[] memberInfos = type.GetMember(e.ToString());

            if (memberInfos != null && memberInfos.Length > 0)
            {
                object[] attributes = memberInfos[0].GetCustomAttributes(typeof(EnumValue),
                    false);
                if (attributes != null && attributes.Length > 0)
                    return ((EnumValue)attributes[0]).Value;
            }
            throw new ArgumentException("Enum " + e + " has no EnumText defined!");
        }
    }
}
