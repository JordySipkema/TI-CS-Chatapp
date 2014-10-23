using System;
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Response
{
    public class RegisterResponsePacket : ResponsePacket
    {
        public const string DefCmd = "RESP-REGISTER";

        #region Constructors
        public RegisterResponsePacket(Statuscode.Status status)
            : base(status, DefCmd)
        {
        }

        public RegisterResponsePacket(String status, String description) 
            : base(status, description, DefCmd)
        {
        }

        public RegisterResponsePacket(JObject json) : base(json)
        {
        }
        #endregion
    }
}
