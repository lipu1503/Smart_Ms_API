using System.Text;
using System.Security.Cryptography;
namespace SmartManagement.Domain.Utilities
{
    public class SmartEncryptionSuit
    {
        private static readonly byte[] key;
        private static readonly byte[] iv;
        static SmartEncryptionSuit()
        {
            key = Encoding.ASCII.GetBytes("Smart_Smart@BLR#0708SMART_SMART#070800").Take(32).ToArray();
            iv = Encoding.ASCII.GetBytes("8070#SGE@SMART_SMART").Take(16).ToArray();
        }  
        public static string Encrypt(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Null or Empty string is not allowed.");
            }
            var newProvider = new AesManaged();
            var newEncryptor = newProvider.CreateEncryptor(key, iv);
            return Convert.ToBase64String(Trasform(Encoding.UTF8.GetBytes(value), newEncryptor));
        }
        public static string Decrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Null or Empty string is not allowed.");
            }
            var newProvider = new AesManaged();
            var newDecryptor = newProvider.CreateDecryptor(key, iv);
            return Encoding.UTF8.GetString(Trasform(Convert.FromBase64String(value), newDecryptor));
        }
        private static byte[] Trasform(byte[] input, ICryptoTransform transform)
        {
            using (var memory = new MemoryStream())
                using (var crypto = new CryptoStream(memory, transform, CryptoStreamMode.Write))
            {
                crypto.Write(input, 0, input.Length);
                crypto.FlushFinalBlock();
                return memory.ToArray();
            }
        }
    }
}
