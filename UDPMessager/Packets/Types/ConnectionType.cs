﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPMessenger.Packets.Types
{
    public enum ConnectionType : byte
    {
        Connecting,
        ConnectingResponse,
        Connected,
        ConnectedResponse
    }
}
