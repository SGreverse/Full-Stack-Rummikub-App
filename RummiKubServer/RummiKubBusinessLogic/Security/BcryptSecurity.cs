using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace RummiKubClasses.Security
{
    public class BcryptSecurity
    {
        public static string GenerateHash(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text);
        }
        public static bool IsSameValue(string password,string Hashedpassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, Hashedpassword);
        }
    }
}
