using System;
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Request
{
    public class LoginPacket : RequestPacket
    {
        //Inherited fields: CMD
        //Introduced fields: Username, Password (hash)

        public const string DefCmd = "LOGIN";

        public String Username { get; private set; }
        public String Passhash { get; private set; }

        public LoginPacket(JObject json) 
            : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "Loginpacket ctor: json is null!");

            JToken username;
            JToken password;

            if (!(json.TryGetValue("username", StringComparison.CurrentCultureIgnoreCase, out username)
                && json.TryGetValue("password", StringComparison.CurrentCultureIgnoreCase, out password)))
                throw new ArgumentException("Username or password is not found in json: \n" + json);

            Initialize((string)username, (string)password);
        }

        public LoginPacket(string username, string passhash)
            : base(DefCmd)
        {
            Initialize(username, passhash);
        }

        private void Initialize(string user, string passhash)
        {
            Username = user;
            Passhash = passhash;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
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
