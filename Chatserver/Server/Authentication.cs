using Chatserver.FileController;
using ChatShared.Entity;
using ChatShared.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Chatserver.Server
{
    static class Authentication
    {
        //ConcurrentDictionary to enhance thread safety.
        private static readonly ConcurrentDictionary<User, IClientHandler> AuthUsers 
            = new ConcurrentDictionary<User, IClientHandler>();

        public static Boolean Authenticate(String username, String passhash, IClientHandler clientHandler)
        {
            //check that user and passhash are valid.  
            var user = Datastorage.Instance.GetUser(username);

            // if the user is null, there is no user found. 
            // => return false (authentication failed).
            if (user == null)  return false;
            // if the password is not equals to the passhash, 
            // the password is incorrect. => return false (auth failed).
            if (user.Password != passhash) return false;

            //Creating the hash (AuthToken)
            //1. Prepare the string for hashing (user-passhash-milliseconds_since_epoch)
            var millis = DateTime.Now.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds;
            var aboutToHash = String.Format("{0}-{1}-{2}", username, passhash, millis);

            //2. Hash the string.
            var hash = Crypto.CreateSHA256(aboutToHash);

            //3. Link the authtoken to the user
            user.AuthToken = hash;

            //4. Remove the user if he was already in the list.
            var linqQuery = AuthUsers.Where(u => u.Key.Username == user.Username).ToList();
            foreach (var keyValuePair in linqQuery)
            {
                IClientHandler cHandler;
                AuthUsers.TryRemove(keyValuePair.Key, out cHandler);

            }

            //4. Add the user to the AuthUsers class.
            AuthUsers.GetOrAdd(user, clientHandler);

            return true;
        }


        public static Boolean Authenticate(String authToken)
        {
            return (AuthUsers.Count(x => x.Key.AuthToken == authToken) == 1);
        }
        public static void ReleaseAuthToken(String authToken)
        {
            var users = AuthUsers.Keys.Where(user => user.AuthToken == authToken);
            foreach (var user in users)
            {
                IClientHandler s;
                AuthUsers.TryRemove(user, out s);
            }
        }

        public static IClientHandler GetStream(String username)
        {
            return AuthUsers.First(x => x.Key.Username == username).Value;
        }

        public static User GetUser(String username)
        {
            return AuthUsers.First(x => x.Key.Username == username).Key;
        }

        public static List<User> GetAllUsers()
        {
            return AuthUsers.Keys.ToList();
        }
    }
}
