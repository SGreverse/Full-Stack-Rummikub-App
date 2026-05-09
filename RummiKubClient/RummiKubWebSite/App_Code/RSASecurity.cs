using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using ServiceReference2;

/// <summary>
/// Summary description for RSASecurity
/// </summary>
public class RSASecurity
{
    public static string Encrypt(string text)
    {
        RummiKubServiceClient service = new RummiKubServiceClient();
        string publickey = service.GetPublicKey().ReturnedObject;

        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publickey);
            byte[] encrypted = rsa.Encrypt(Encoding.UTF8.GetBytes(text), true);
            return Convert.ToBase64String(encrypted);

        }
    }
}