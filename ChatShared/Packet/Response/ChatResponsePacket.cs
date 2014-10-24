using System;
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Response
{
    public class ChatResponsePacket : ResponsePacket
    {
        public const string DefCmd = "RESP-CHAT";

        #region Constructors
        public ChatResponsePacket(Statuscode.Status status)
            : base(status, DefCmd)
        {
        }

        public ChatResponsePacket(String status, String description) 
            : base(status, description, DefCmd)
        {
        }

        public ChatResponsePacket(JObject json) : base(json)
        {
        }
        #endregion
    }
}
