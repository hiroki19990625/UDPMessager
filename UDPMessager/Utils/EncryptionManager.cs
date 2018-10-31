using System.Security.Cryptography;

namespace UDPMessenger.Utils
{
    public class EncryptionManager
    {
        public static RsaKeys GenerateKeys()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            return new RsaKeys(rsa.ToXmlString(false), rsa.ToXmlString(true));
        }

        public static byte[] Encrypt(byte[] buffer, string publicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            return rsa.Encrypt(buffer, false);
        }

        public static byte[] Decrypt(byte[] buffer, string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);
            return rsa.Decrypt(buffer, false);
        }
    }
}
