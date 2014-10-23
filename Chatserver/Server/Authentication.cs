using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChatShared;

namespace Chatserver.Server
{
    public static class Authentication
    {
        //ConcurrentDictionary to enhance thread safety.
        private static readonly ConcurrentDictionary<User, Stream> AuthUsers = new ConcurrentDictionary<User, Stream>();

        public static Boolean Authenticate(String username, String passhash, Stream socketStream)
        {
            //check that user and passhash are valid.
            //TODO: Create connection with database and check the the user is valid.    

            //Creating the hash (AuthToken)
            //1. Prepare the string for hashing (user-passhash-milliseconds_since_epoch)
            var millis = DateTime.Now.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds;
            var aboutToHash = String.Format("{0}-{1}-{2}", username, passhash, millis);

            //2. Hash the string.
            var hash = AppProperties.CreateSHA256(aboutToHash);

            // TODO: 3. Create the user (to store in the AuthUsers-dictonairy
            // TODO: Link the authtoken to the user

            // Example: 
            var user = new User();
            user.AuthToken = hash;

            //4. Add the user to the AuthUsers class.
            AuthUsers.GetOrAdd(user, socketStream);

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
                Stream s;
                AuthUsers.TryRemove(user, out s);
            }
        }

        public static Stream GetStream(String username)
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


    // Tijdelijk internal-class om errors te voorkomen
    // Zodra er een echte data-model is, wordt deze klasse verwijderd.
    // TODO: Remove the internal User class
    public class User
    {
        public String AuthToken = "ERROR: PLACEHOLDER FOR AUTHTOKEN";
        public String Username = "ERROR: PLACEHOLDER FOR USERNAME";
    }
}
