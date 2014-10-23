using System;
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Response
{
    public class LoginResponsePacket : ResponsePacket
    {
        public const string DefCmd = "RESP-LOGIN";

        public string AuthToken { get; private set; }

        #region Constructors
        public LoginResponsePacket(Statuscode.Status status, String authtoken)
            : base(status, DefCmd)
        {
            Initialize(authtoken);
        }

        public LoginResponsePacket(String status, String description, String authtoken) 
            : base(status, description, DefCmd)
        {
            Initialize(authtoken);
        }

        public LoginResponsePacket(JObject json) : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "LoginResponsepacket ctor: json is null!");

            if (Status != "200") return;

            JToken authToken;
            if (!(json.TryGetValue("AUTHTOKEN", StringComparison.CurrentCultureIgnoreCase, out authToken)))
                throw new ArgumentException("AUTHTOKEN is not found in json \n" + json);
            Initialize(authToken.ToString());
        }
        #endregion

        #region Initializers
        private void Initialize(String authtoken)
        {
            AuthToken = authtoken;
        }
        #endregion

        #region Override Methods
        public override JObject ToJsonObject()
        {
            var returnJson = base.ToJsonObject();
            returnJson.Add("AUTHTOKEN", AuthToken);

            return returnJson;

        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }

        #endregion

    }
}
