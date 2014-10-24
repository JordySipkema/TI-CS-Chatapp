using System;
using ChatShared.Entity;
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Push
{
    public class UserChangedPacket : PushPacket
    {
        //Inherited Fields: CMD
        //Introduced Fields: Username, Nickname, Status

        public String Username { get; set; }
        public String Nickname { get; set; }
        public bool Status { get; set; }

        public const string DefCmd = "PUSH-UC";

        public UserChangedPacket(User user) : base(DefCmd)
        {
            Initialize(user.Username, user.Nickname, user.OnlineStatus);
        }

        public UserChangedPacket(String username, string nickname, bool status) : base(DefCmd)
        {
            Initialize(username, nickname, status);
        }


        public UserChangedPacket(JObject json) : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "UserChangedpacket ctor: json is null!");

            JToken usernameToken;
            JToken nicknameToken;
            JToken statusToken;

            if (!(json.TryGetValue("username", StringComparison.CurrentCultureIgnoreCase, out usernameToken)
                && json.TryGetValue("nickname", StringComparison.CurrentCultureIgnoreCase, out nicknameToken)
                && json.TryGetValue("status", StringComparison.CurrentCultureIgnoreCase, out statusToken)))
                throw new ArgumentException("Username, nickname and/or status is not found in json: \n" + json);

            Initialize((string)usernameToken, (string)nicknameToken, (bool)statusToken);
        }

        private void Initialize(String username, string nickname, bool status)
        {
            Username = username;
            Nickname = nickname;
            Status = status;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("Username", Username);
            json.Add("Nickname", Nickname);
            json.Add("Status", Status);
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
