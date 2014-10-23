using System;
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Request
{
    public class RegisterPacket : RequestPacket
    {
        // Inherited fields: CMD
        // Introduced fields: Nickname, Username, Passhash
        public const string DefCmd = "REGISTER";

        public String Nickname { get; private set; }
        public String Username { get; private set; }
        public String Passhash { get; private set; }

        public RegisterPacket(string nickname, string username, string passhash) : base(DefCmd)
        {
            Initialize(nickname,username,passhash);
        }

        public RegisterPacket(JObject json) : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "Loginpacket ctor: json is null!");

            JToken nickname;
            JToken username;
            JToken password;

            if (!(json.TryGetValue("username", StringComparison.CurrentCultureIgnoreCase, out username)
                && json.TryGetValue("password", StringComparison.CurrentCultureIgnoreCase, out password)
                && json.TryGetValue("nickname", StringComparison.CurrentCultureIgnoreCase, out nickname)))
                throw new ArgumentException("Nickname, username or password is not found in json: \n" + json);

            Initialize((string)nickname, (string)username, (string)password);
        }

        private void Initialize(string nickname, string username, string passhash)
        {
            Nickname = nickname;
            Username = username;
            Passhash = passhash;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("nickname", Nickname);
            json.Add("username", Username);
            json.Add("password", Passhash);
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
