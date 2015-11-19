using System.Security.Cryptography;
using System.Text;

namespace AUPPRB.Common.Utils
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Хэширует текст
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public static string Sha1EncryptPassword(string phrase)
        {
            var encoder = new UTF8Encoding();
            var sha1Hasher = new SHA1CryptoServiceProvider();
            var hashedDataBytes = sha1Hasher.ComputeHash(encoder.GetBytes(phrase));
            return ByteArrayToString(hashedDataBytes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputArray"></param>
        /// <returns></returns>
        private static string ByteArrayToString(byte[] inputArray)
        {
            var output = new StringBuilder("");
            for (int i = 0; i < inputArray.Length; i++)
            {
                output.Append(inputArray[i].ToString("X2"));
            }
            return output.ToString();
        }
    }
}
