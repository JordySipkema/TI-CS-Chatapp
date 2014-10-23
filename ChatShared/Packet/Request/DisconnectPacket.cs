
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Request
{
    public class DisconnectPacket : AuthenticatedPacket
    {
        public const string DefCmd = "DC";

        public DisconnectPacket(string authtoken) : base(DefCmd, authtoken)
        {
        }

        public DisconnectPacket(JObject json) : base(json)
        {
        }
    }
}
