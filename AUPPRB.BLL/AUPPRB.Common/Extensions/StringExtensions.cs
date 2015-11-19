using System;
using System.Text;

namespace AUPPRB.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToStringWithValue(this object o)
        {
            if (o == null)
                return String.Empty;

            return o.ToString();
        }

        public static int ToInt(this string o)
        {
            int i = 0;

            int.TryParse(o, out i);

            return i;
        }

        public static string EncodeTo64(this string toEncode)
        {
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);
            string returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string DecodeFrom64(this string encodedData)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
            string returnValue = Encoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public static string RemoveTrailingSlash(this string source)
        {
            return source.TrimEnd('/');
        }
    }
}
