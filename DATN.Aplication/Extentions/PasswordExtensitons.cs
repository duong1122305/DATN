using DATN.ViewModels.Common;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DATN.Aplication.Extentions
{
    public class PasswordExtensitons
    {

        private static readonly byte[] Key = Encoding.UTF8.GetBytes("A3F256789B4E9D8F6C7B1E3C5A6D9E7F"); // 32 bytes for AES-256
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("A1B2C3D4E5F60718"); // 16 bytes for AES
        private static readonly byte[] KeyMail = Encoding.UTF8.GetBytes("A3F256789B4E9D8F6C7B1E3C5A6D9E7F"); // 32 bytes for AES-256
        private static readonly byte[] IVMail = Encoding.UTF8.GetBytes("A1B2C3D4E5F60718"); // 16 bytes for AES

        public string HashPassword(string password)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(password);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }


        public string Decrypt(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        public bool VerifyPassword(string password, string hashedPassword)
        {


            return password == Decrypt(hashedPassword);
        }


        public string HashCode(string verifyString)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = KeyMail;
                aesAlg.IV = IVMail;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(verifyString);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }
        public string DeCode(string veryfyCode)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = KeyMail;
                aesAlg.IV = IVMail;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(veryfyCode)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public string GeneratePassword()
        {
            int passLength = 8;
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()_+<>?";

            string allChars = upperCase + lowerCase + digits + specialChars;
            Random random = new Random();

            StringBuilder password = new StringBuilder();

            // Add one random character from each category to ensure the password contains at least one of each
            password.Append(upperCase[random.Next(upperCase.Length)]);
            password.Append(lowerCase[random.Next(lowerCase.Length)]);
            password.Append(digits[random.Next(digits.Length)]);
            password.Append(specialChars[random.Next(specialChars.Length)]);

            // Fill the rest of the password length with random characters
            for (int i = 4; i < passLength; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            // Shuffle the password to remove any predictable pattern
            return ShuffleString(password.ToString());
        }
        private string ShuffleString(string input)
        {
            char[] array = input.ToCharArray();
            Random random = new Random();
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                char temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
            return new string(array);
        }

        public ResponseData<string> ValidatePassword(string password)
        {
            if (password.Length <= 6)
            {
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = "Mật khẩu phải dài hơn 6 ký tự."
                };
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = "Mật khẩu phải chứa ít nhất một chữ hoa."
                };
            }

            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = "Mật khẩu phải chứa ít nhất một chữ thường."
                };
            }

            if (!Regex.IsMatch(password, @"\d"))
            {
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = "Mật khẩu phải chứa ít nhất một chữ số."
                };
            }

            if (!Regex.IsMatch(password, @"[\W_]"))
            {
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = "Mật khẩu phải chứa ít nhất một ký tự đặc biệt."
                };
            }

            if (password.Contains(" "))
            {
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = "Mật khẩu không được chứa dấu cách."
                };
            }

            return new ResponseData<string>
            {
                IsSuccess = true,
                Error = "Thành công"
            };
        }
    }
}
