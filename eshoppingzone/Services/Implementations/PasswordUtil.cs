using System.Security.Cryptography;
using System.Text;

namespace EshoppingZone.Services.Implementations
{
    internal static class PasswordUtil
    {
        public static string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }
    }
}