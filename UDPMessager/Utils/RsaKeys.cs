using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPMessenger.Utils
{
    public class RsaKeys
    {
        public string PublicKey { get; }
        public string PrivateKey { get; }

        public RsaKeys(string publicKey, string privateKey)
        {
            this.PublicKey = publicKey;
            this.PrivateKey = privateKey;
        }
    }
}
