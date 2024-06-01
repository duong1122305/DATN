using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Extentions
{
    public class PasswordExtensitons
    {
        public string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password with PBKDF2
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Combine the salt and hashed password
            return $"{Convert.ToBase64String(salt)}:{hashedPassword}";
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Extract the salt and hashed password from the stored hash
            string[] parts = hashedPassword.Split(':', 2);
            byte[] salt = Convert.FromBase64String(parts[0]);
            string storedHash = parts[1];

            // Compute the hash of the provided password and salt
            string computedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Compare the computed hash with the stored hash
            return storedHash == computedHash;
        }
    }
}
