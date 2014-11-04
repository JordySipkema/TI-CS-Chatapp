using System;
using Newtonsoft.Json.Linq;
using ChatShared.Entity;

namespace ChatShared.Packet.Request
{
    public class ChatPacket : AuthenticatedPacket
    {
        //Inherited fields: CMD, AUTHTOKEN
        //Introduced fields: Message, UsernameDestination, Sent (datetime)

        public const string DefCmd = "CHAT";

        public String Message { get; private set; }
        public String UsernameDestination { get; private set; }
        public DateTime Sent { get; private set; }

        public ChatPacket(JObject json) : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "Chatpacket ctor: json is null!");

            JToken message;
            JToken usernameDestination;
            JToken sent;

            if (!(json.TryGetValue("message", StringComparison.CurrentCultureIgnoreCase, out message)
                && json.TryGetValue("UsernameDestination", StringComparison.CurrentCultureIgnoreCase, out usernameDestination)
                && json.TryGetValue("Sent", StringComparison.CurrentCultureIgnoreCase, out sent)))
                throw new ArgumentException("Message or UsernameDestination is not found in json: \n" + json);

            Initialize((string)message, (string)usernameDestination, (DateTime)sent);
        }

        public ChatPacket(string message, string usernameDestination, string authtoken)
            : base(DefCmd, authtoken)
        {
            Initialize(message, usernameDestination);
        }

        public ChatPacket(ChatMessage message, string authtoken)
            : base(DefCmd, authtoken)
        {
            Initialize(message.Message, message.Recipient);
        }

        private void Initialize(string message, string usernameDestination)
        {
            Initialize(message,usernameDestination,DateTime.Now);
        }

        private void Initialize(string message, string usernameDestination, DateTime sent)
        {
            Message = message;
            UsernameDestination = usernameDestination;
            Sent = sent;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("Message", Message);
            json.Add("UsernameDestination", UsernameDestination);
            json.Add("Sent", Sent);
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
