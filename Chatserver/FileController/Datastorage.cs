using System.Collections.Generic;
using System.Linq;
using ChatShared.Entity;
using ChatShared.Utilities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.IO;
using System;
using Newtonsoft.Json;
using Chatserver.Properties;

namespace Chatserver.FileController
{
    internal class Datastorage
    {
        private static Datastorage _instance;
        public static Datastorage Instance
        {
            get { return _instance ?? (_instance = new Datastorage()); }
        }

        private readonly List<User> _users = new List<User>();
        private readonly List<ChatMessage> _messages = new List<ChatMessage>(); 

        private Datastorage()
        {
            //_users.Add(new User("Henk", "testuser01", Crypto.CreateSHA256("1234")));
            //_users.Add(new User("Bart", "bart", Crypto.CreateSHA256("hoi")));
            //_users.Add(new User("Jordy", "jordy", Crypto.CreateSHA256("hoi")));

            // TODO: Create initializer code (fetching data from FileIO?)
            
            OpenFromFile();
            
            
        } 

        public User GetUser(string username)
        {
            return _users.FirstOrDefault(user => user.Username == username);
        }

        public IEnumerable<User> GetUsers()
        {
            return _users;
        } 

        public IEnumerable<ChatMessage> GetMessages(string username)
        {
            var x =
                from message in _messages
                where message.Recipient == username || message.Sender == username
                orderby message.Timestamp ascending
                select message;

            return x;
        }
        public IEnumerable<ChatMessage> GetMessagesSentTo(string username)
        {
            var x =
                from message in _messages
                where message.Recipient == username
                orderby message.Timestamp ascending
                select message;

            return x;
        }
        public IEnumerable<ChatMessage> GetMessagesFrom(string username)
        {
            var x =
                from message in _messages
                where message.Sender == username
                orderby message.Timestamp ascending
                select message;

            return x;
        }

        public bool AddUser(User user)
        {
            _users.Add(user);
            return true;
        }

        public void AddMessage(ChatMessage message)
        {
            _messages.Add(message);
        }

        private void OpenFromFile()
        {
            string location = Settings.Default.UsersFileLocation;
            if (File.Exists(@location))
            {
                var users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@location));
                foreach (User u in users)
                    _users.Add(u);
            }

            location = Settings.Default.MessagesFileLocation;
            if (File.Exists(@location))
            {
                var messages = JsonConvert.DeserializeObject<List<ChatMessage>>(File.ReadAllText(@location));
                foreach (ChatMessage m in messages)
                    _messages.Add(m);
            }
        }

        public void SaveToFile()
        {
            SaveToFile((object)_users, Settings.Default.UsersFileLocation);
            SaveToFile((object)_messages, Settings.Default.MessagesFileLocation);
        }
        public void SaveToFile(object list, string location)
        {
            if (File.Exists(@location))
                File.Delete(@location);
            FileStream fs = File.Open(@location, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            JsonWriter jw = new JsonTextWriter(sw);
            {
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, list);
            }
            jw.Close();
            sw.Close();
            fs.Close();
        }


        private void FfOpen(string location)
        {
            //this gives problems...

            var json = JObject.Parse(File.ReadAllText(@location));
            var users = JArray.Load(json.Property("Users").CreateReader()).ToObject<List<User>>();
            var chatmessages = JArray.Load(json.Property("Chatmessages").CreateReader()).ToObject<List<ChatMessage>>();
            foreach (User u in users)
            {
                AddUser(u);
            }
            foreach (ChatMessage cm in chatmessages)
            {
                AddMessage(cm);
            }
        }
        private void FfSave(string location) 
        {
            
            JObject json = new JObject();
            json.Add(new JProperty("Users", new JArray(_users)));
            json.Add(new JProperty("Chatmessages", new JArray(_messages)));
            
            File.WriteAllText(@location, json.ToString());
            
            //write json to file, call json.toString first;

        }
    }
}
