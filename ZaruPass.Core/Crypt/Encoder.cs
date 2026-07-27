using Ecng.Common;
using Ecng.Security;

namespace ZaruPass.Core.Crypt
{
    public static class Encoder
    {
        public static byte[] Encode(string text, string key)
        {
            // 1. Преобразуем строку пароля в байты для шифрования.
            byte[] plainTextBytes = text.UTF8();

            // 2. Генерируем случайную соль и вектор инициализации.
            //    Соль и IV должны быть уникальными для каждого шифрования.
            byte[] salt = TypeHelper.GenerateSalt(CryptoHelper.DefaultSaltSize);
            byte[] iv = new byte[16]; // 16 байт для AES

            // 3. Выполняем шифрование.
            //    Метод EncryptAes использует PBKDF2 для генерации ключа из парольной фразы.
            byte[] cipherText = plainTextBytes.EncryptAes(key, salt, iv);

            // 4. Сохраняем соль, IV и зашифрованный текст вместе.
            //    Это необходимо для последующей расшифровки.
            using (var outputStream = new System.IO.MemoryStream())
            {
                outputStream.Write(salt, 0, salt.Length);
                outputStream.Write(iv, 0, iv.Length);
                outputStream.Write(cipherText, 0, cipherText.Length);
                return outputStream.ToArray();
            }
        }
        public static string Decode(byte[] text, string key)
        {
            // 1. Извлекаем соль, IV и зашифрованный текст из входного массива.
            int saltSize = CryptoHelper.DefaultSaltSize;
            int ivSize = 16;

            byte[] salt = new byte[saltSize];
            byte[] iv = new byte[ivSize];
            byte[] cipherText = new byte[text.Length - saltSize - ivSize];

            Array.Copy(text, 0, salt, 0, saltSize);
            Array.Copy(text, saltSize, iv, 0, ivSize);
            Array.Copy(text, saltSize + ivSize, cipherText, 0, cipherText.Length);

            // 2. Выполняем расшифровку.
            //    Метод DecryptAes использует те же соль и IV для восстановления ключа.
            byte[] plainTextBytes = cipherText.DecryptAes(key, salt, iv);

            // 3. Преобразуем байты обратно в строку.
            return plainTextBytes.UTF8();
        }

        public static string GetStringFromBytes(byte[] text)
        {
            return text.UTF8();
        }

        public static byte[] GetBytesFromString(string text)
        {
            return text.UTF8();
        }
    }
}
