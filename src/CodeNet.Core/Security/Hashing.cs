using System.Text;

namespace CodeNet.Core.Security
{
    public static class Hashing
    {
        public static string MD5(string input) => MD5(Encoding.UTF8.GetBytes(input));

        public static string MD5(byte[] inputBytes) => BitConverter.ToString(System.Security.Cryptography.MD5.HashData(inputBytes)).Replace("-", "");

        public static string Shake128(string input) => Shake128(Encoding.UTF8.GetBytes(input));

        public static string Shake128(byte[] inputBytes) => BitConverter.ToString(System.Security.Cryptography.Shake128.HashData(inputBytes, inputBytes.Length)).Replace("-", "");

        public static string SHA1(string input) => SHA1(Encoding.UTF8.GetBytes(input));

        public static string SHA1(byte[] inputBytes) => BitConverter.ToString(System.Security.Cryptography.SHA1.HashData(inputBytes)).Replace("-", "");

        public static string SHA256(string input) => SHA256(Encoding.UTF8.GetBytes(input));

        public static string SHA256(byte[] inputBytes) => BitConverter.ToString(System.Security.Cryptography.SHA256.HashData(inputBytes)).Replace("-", "");

        public static string SHA512(string input) => SHA512(Encoding.UTF8.GetBytes(input));

        public static string SHA512(byte[] inputBytes) => BitConverter.ToString(System.Security.Cryptography.SHA512.HashData(inputBytes)).Replace("-", "");
    }
}
