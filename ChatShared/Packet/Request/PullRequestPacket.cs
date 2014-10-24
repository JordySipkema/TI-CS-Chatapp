using System;
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Request
{
    public class PullRequestPacket : AuthenticatedPacket
    {
        //Inherited field: CMD, AuthToken
        //Introduced fields: Datatype (enum)

        public const string DefCmd = "PULL";
        public enum RequestType
        {
            UsersByStatus,
            MessagesByUser,
            ReceivedMessages,
        }

        public RequestType Request { get; private set; }
        public String SearchKey { get; private set; }


        public PullRequestPacket(RequestType requestType, string authtoken) : base(DefCmd, authtoken)
        {
            Initialize(requestType);
        }

        public PullRequestPacket(RequestType requestType, string searchKey, string authtoken)
            : base(DefCmd, authtoken)
        {
            Initialize(requestType, searchKey);
        }

        public PullRequestPacket(JObject json) : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "Loginpacket ctor: json is null!");

            JToken requestTypeToken;
            JToken searchKeyToken;

            if (!(json.TryGetValue("REQUESTTYPE", StringComparison.CurrentCultureIgnoreCase, out requestTypeToken)))
                throw new ArgumentException("RequestType is not found in json: \n" + json);


            var requestType = (RequestType) Enum.Parse(typeof (RequestType), (string) requestTypeToken);
            if (!Enum.IsDefined(typeof (RequestType), requestType))
                throw new ArgumentException("RequestType is found, but is invalid in json: \n" + json);

            //Check if searchkey is present. If so: Initialize with the key, else: init without it. (the key is not neccesary)
            if (json.TryGetValue("SEARCHKEY", StringComparison.CurrentCultureIgnoreCase, out searchKeyToken))
                Initialize(requestType, (string) searchKeyToken);
            else
                Initialize(requestType);
        }

        private void Initialize(RequestType requestType, string searchKey = null)
        {
            Request = requestType;
            SearchKey = searchKey;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("REQUESTTYPE", Request.ToString());
            if (SearchKey != null)
                json.Add("SEARCHKEY", SearchKey);
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
