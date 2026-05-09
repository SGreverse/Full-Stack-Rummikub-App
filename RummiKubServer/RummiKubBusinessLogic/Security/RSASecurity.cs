using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses
{
    public class RSASecurity
    {
        public static string GetPublicKey()
        {
            return File.ReadAllText(@"..\..\..\public.key");
        }
        public static string EncryptText(string text)
        {
            string publickey = File.ReadAllText(@"..\..\..\public.key");

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publickey);
                byte[] encrypted = rsa.Encrypt(Encoding.UTF8.GetBytes(text), true);
                return Convert.ToBase64String(encrypted);

            }
        }
        public static string DecryptText(string encryptedText)
        {
            string privatekey = File.ReadAllText(@"..\..\..\private.key");
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privatekey);
                byte[] decrypted = rsa.Decrypt(Convert.FromBase64String(encryptedText), true);
                return Encoding.UTF8.GetString(decrypted);
            }
        }
    }
}
