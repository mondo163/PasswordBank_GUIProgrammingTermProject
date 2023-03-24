using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptDecryptLib
{
    public interface IStringEncryptDecrypt
    {
        void Encrypt(string Entry);

        void Decrypt(string Entry);
    }
}
