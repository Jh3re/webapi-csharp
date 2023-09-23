using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace Tools
{
    public class PasswordEncryptor
    {
        public static string EncryptPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashString;
            }
        }

        public static bool VerifyPassword(string passwordToCheck, string storedHashedPassword)
        {
            string hashedPasswordToCheck = EncryptPassword(passwordToCheck);
            return string.Equals(hashedPasswordToCheck, storedHashedPassword, StringComparison.OrdinalIgnoreCase);
        }
    }
}