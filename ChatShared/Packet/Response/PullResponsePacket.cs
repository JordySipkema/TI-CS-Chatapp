using System;
using System.Collections.Generic;
using ChatShared.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Response
{
    public class PullResponsePacket<T> : ResponsePacket
    {
        public const string DefCmd = "RESP-PULL";

        public enum DataType
        {
            User,
            ChatMessage
        }

        public DataType PDataType { get; private set; }
        public IEnumerable<T> Data { get; private set; }

        public PullResponsePacket(Statuscode.Status status, DataType dataType, IEnumerable<T> dataEnumerable)
            : base(status, DefCmd)
        {
            Initialize(dataType, dataEnumerable);
        }

        public PullResponsePacket(string status, string description,
            DataType dataType, IEnumerable<T> dataEnumerable)
            : base(status, description, DefCmd)
        {
            Initialize(dataType, dataEnumerable);
        }

        public PullResponsePacket(JObject json)
            : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "PullResponsePacket ctor: json is null!");

            if (Status != "200") return;

            JToken dataTypeToken;
            JToken dataToken;
            if (!(json.TryGetValue("DATATYPE", StringComparison.CurrentCultureIgnoreCase, out dataTypeToken)
                && json.TryGetValue("DATA", StringComparison.CurrentCultureIgnoreCase, out dataToken)))
                throw new ArgumentException("Datatype or Data is not found in json \n" + json);

            var dataType = (DataType)Enum.Parse(typeof(DataType), (string)dataTypeToken);
            if (!Enum.IsDefined(typeof(DataType), dataType))
                throw new ArgumentException("DataType is found, but is invalid in json: \n" + json);

            if (!(dataType == DataType.User && (typeof(T) == typeof(User)) ||
               dataType == DataType.ChatMessage && (typeof(T) == typeof(ChatMessage))))
                throw new ArgumentException(String.Format("DataType ({0}) and TypeParameter ({1}) are not in sync", 
                    dataType, typeof(T)));

            var data = dataToken.Children().Values<T>();
            Initialize(dataType, data);
        }

        private void Initialize(DataType dataType, IEnumerable<T> dataEnumerable)
        {
            PDataType = dataType;
            Data = dataEnumerable;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("DATATYPE", PDataType.ToString());
            json.Add("DATA", JArray.FromObject(Data));
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
