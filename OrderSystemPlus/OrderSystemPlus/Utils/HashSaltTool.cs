using System.Security.Cryptography;
using System.Text;

namespace OrderSystemPlus.Utils.HashSaltTool
{
    public class HashSaltResponse
    {
        public int Size { get; set; }
        public string? Salt { get; set; }
        public string? Hash { get; set; }
    }

    public static class HashSaltTool
    {
        /// <summary>
        /// string convert to SHA512 string
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        private static string SHA512(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

#pragma warning disable SYSLIB0021 // Type or member is obsolete
            System.Security.Cryptography.SHA512 sha512 = new SHA512CryptoServiceProvider();
#pragma warning restore SYSLIB0021 // Type or member is obsolete
            var source = Encoding.Default.GetBytes(plainText);
            var crypto = sha512.ComputeHash(source);
            return Convert.ToBase64String(crypto);
        }
        /// <summary>
        /// generate ASCII string
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static string ASCII(int number)
        {
            return (char)number + "";
        }
        /// <summary>
        /// generate salt
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string generateSalt(int length)
        {
            string salt = "";
            Random rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                salt += ASCII(rnd.Next(33, 127)); // ASCII 33~126
            }
            return salt;
        }
        /// <summary>
        /// generate hash&salt result
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="saltSize"></param>
        public static HashSaltResponse Generate(string plainText, int saltSize = 15)
        {
            string salt = generateSalt(saltSize);
            string hash = SHA512(plainText + salt); // plainText + salt

            HashSaltResponse response = new HashSaltResponse();
            response.Hash = hash;
            response.Salt = salt;
            response.Size = saltSize;

            return response;
        }
        /// <summary>
        /// Validate Hash result is equal to SHA512(plainText+salt)
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="salt"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static Boolean Validate(string plainText, string salt, string hash)
        {
            if (hash == SHA512(plainText + salt))
            {
                return true;
            }
            return false;
        }

    }
}
