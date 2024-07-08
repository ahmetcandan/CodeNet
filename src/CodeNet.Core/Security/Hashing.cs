using System.Text;

namespace CodeNet.Core.Security
{
    public class Hashing
    {
        public static string MD5(string input)
        {
            return MD5(Encoding.UTF8.GetBytes(input));
        }

        public static string MD5(byte[] inputBytes)
        {
            byte[] hashBytes = System.Security.Cryptography.MD5.HashData(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        public static string Shake128(string input)
        {
            return Shake128(Encoding.UTF8.GetBytes(input));
        }

        public static string Shake128(byte[] inputBytes)
        {
            byte[] hashBytes = System.Security.Cryptography.Shake128.HashData(inputBytes, inputBytes.Length);
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        public static string SHA1(string input)
        {
            return SHA1(Encoding.UTF8.GetBytes(input));
        }

        public static string SHA1(byte[] inputBytes)
        {
            byte[] hashBytes = System.Security.Cryptography.SHA1.HashData(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        public static string SHA256(string input)
        {
            return SHA256(Encoding.UTF8.GetBytes(input));
        }

        public static string SHA256(byte[] inputBytes)
        {
            byte[] hashBytes = System.Security.Cryptography.SHA256.HashData(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        public static string SHA512(string input)
        {
            return SHA512(Encoding.UTF8.GetBytes(input));
        }

        public static string SHA512(byte[] inputBytes)
        {
            byte[] hashBytes = System.Security.Cryptography.SHA512.HashData(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }
    }
}
