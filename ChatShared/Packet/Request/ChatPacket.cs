using System;
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Request
{
    public class ChatPacket : AuthenticatedPacket
    {
        //Inherited fields: CMD, AUTHTOKEN
        //Introduced fields: Message, UsernameDestination

        public const string DefCmd = "CHAT";

        public String Message { get; private set; }
        public String UsernameDestination { get; private set; }

        public ChatPacket(JObject json) : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "Chatpacket ctor: json is null!");

            JToken message;
            JToken usernameDestination;

            if (!(json.TryGetValue("message", StringComparison.CurrentCultureIgnoreCase, out message)
                && json.TryGetValue("UsernameDestination", StringComparison.CurrentCultureIgnoreCase, out usernameDestination)))
                throw new ArgumentException("Message or UsernameDestination is not found in json: \n" + json);

            Initialize((string)message, (string)usernameDestination);
        }

        public ChatPacket(string message, string usernameDestination, string authtoken)
            : base(DefCmd, authtoken)
        {
            Initialize(message, usernameDestination);
        }

        private void Initialize(string message, string usernameDestination)
        {
            Message = message;
            UsernameDestination = usernameDestination;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("Message", Message);
            json.Add("UsernameDestination", UsernameDestination);
            return base.ToJsonObject();
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
