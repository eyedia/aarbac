using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public class Encryption
    {
        public Encryption() { }
        
        public byte[] Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return null;

            AesManagedEncryption enc = new AesManagedEncryption();
            return enc.Encrypt(plainText);
        }
        public string Decrypt(byte[] encryptedText)
        {
            if (encryptedText == null)
                return null;

            AesManagedEncryption dec = new AesManagedEncryption();
            return dec.Decrypt(encryptedText);
        }
    }
}
