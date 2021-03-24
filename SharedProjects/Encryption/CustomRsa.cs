using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Encryption
{
    public class CustomRsa
    {
        RSACryptoServiceProvider rsa;
        public RSAParameters Private { get => rsa.ExportParameters(true); set => rsa.ImportParameters(value); }
        public RSAParameters Public { get => rsa.ExportParameters(false); set => rsa.ImportParameters(value); }

        public CustomRsa()
        {
            rsa = new RSACryptoServiceProvider();
        }

        public byte[] EncryptString(string data)
        {
            return rsa.Encrypt(Encoding.Unicode.GetBytes(data), false);
        }

        public byte[] Encrypt(byte[] data)
        {
            return rsa.Encrypt(data, false);
        }
        public byte[] Decrypt(byte[] data)
        {
            return rsa.Decrypt(data, false);
        }

        public string DecryptString(byte[] data)
        {
            return Encoding.Unicode.GetString(rsa.Decrypt(data, false));
        }
    }
}
