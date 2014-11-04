using ChatShared.Packet;
using System;

namespace Chatserver.Server
{
    interface IClientHandler
    {
        void Send(String s);

        void Send(Packet s);
    }
}
