using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPMessenger.Packets.Types
{
    [Flags]
    public enum QosType : byte
    {
        None,
        Ack = 1, 
        Resend = 2,
        Order = 4,
        Channel = 8,
        Chunk = 16
    }
}
