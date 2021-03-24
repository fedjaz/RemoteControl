using System;
using System.IO;
using System.Security.Cryptography;

namespace Encryption
{
    public class CustomAes
    {
        Aes aes;
        public byte[] Key { get => (byte[])aes.Key.Clone(); set => aes.Key = value; }
        public byte[] IV { get => (byte[])aes.IV.Clone(); set => aes.IV = value; }
        public CustomAes()
        {
            aes = Aes.Create();
            aes.GenerateKey();
        }

        public byte[] Encrypt(string data)
        {
            aes.GenerateIV();
            using(MemoryStream memoryStream = new MemoryStream())
            {
                ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);
                using(CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                {
                    using(StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(data);
                    }
                    return memoryStream.ToArray();
                }
            }
        }

        public string Decrypt(byte[] data, byte[] IV)
        {
            aes.IV = IV;
            using(MemoryStream memoryStream = new MemoryStream(data))
            {
                ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
                using(CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
                {
                    using(StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}
