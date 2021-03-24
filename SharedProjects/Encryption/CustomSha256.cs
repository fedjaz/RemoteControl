using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Encryption
{
    public class CustomSha256
    {
        SHA256 SHA256;
        public CustomSha256()
        {
            SHA256 = SHA256.Create();
        }

        public string ComputeHah(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            bytes = SHA256.ComputeHash(bytes);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
