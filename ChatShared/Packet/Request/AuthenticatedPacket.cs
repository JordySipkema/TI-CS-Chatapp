using System;
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Request
{
    public class AuthenticatedPacket : RequestPacket
    {
        //Inherited field: CMD
        //Introduced fields: AuthToken

        public String AuthToken { get; private set; }
        protected AuthenticatedPacket(string cmd, string authtoken) : base(cmd)
        {
            Initialize(authtoken);
        }

        protected AuthenticatedPacket(JObject json) : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "Authenticatedpacket ctor: json is null!");

            JToken authToken;

            if (!(json.TryGetValue("AUTHTOKEN", StringComparison.CurrentCultureIgnoreCase, out authToken)))
                throw new ArgumentException("Authtoken is not found in json: \n" + json);

            Initialize((string)authToken);
        }

        private void Initialize(string authtoken)
        {
            AuthToken = authtoken;
        }

        public override JObject ToJsonObject()
        {
            var x = base.ToJsonObject();
            x.Add("AUTHTOKEN", AuthToken);
            return x;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
