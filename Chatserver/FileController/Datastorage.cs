using System.Collections.Generic;
using System.Linq;
using ChatShared.Entity;
using ChatShared.Utilities;

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
            _users.Add(new User("Henk", "testuser01", Crypto.CreateSHA256("1234")));
            _users.Add(new User("Bart", "bart", Crypto.CreateSHA256("hoi")));

            // TODO: Create initializer code (fetching data from FileIO?)
        } 

        public User GetUser(string username)
        {
            return _users.FirstOrDefault(user => user.Username == username);
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

        public bool AddUser(User user)
        {
            _users.Add(user);
            return true;
        }
    }
}
