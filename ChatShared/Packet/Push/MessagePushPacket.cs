using System;
using ChatShared.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Push
{
    public class MessagePushPacket : PushPacket
    {
        // Inherited fields: CMD
        // Introduced fields: Message (type = chatmessage)

        public const string DefCmd = "PUSH-MSG";

        public ChatMessage Message { get; private set; }

        public MessagePushPacket(ChatMessage message) : base(DefCmd)
        {
            Initialize(message);
        }

        public MessagePushPacket(JObject json) : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "MessagePushPacket ctor: json is null!");

            JToken messageToken;

            if (!(json.TryGetValue("Message", StringComparison.CurrentCultureIgnoreCase, out messageToken)))
                throw new ArgumentException("Message is not found in json: \n" + json);

            var message = JsonConvert.DeserializeObject<ChatMessage>(messageToken.ToString());

            Initialize(message);
        }

        private void Initialize(ChatMessage message)
        {
            Message = message;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("Message", JsonConvert.SerializeObject(Message));
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
