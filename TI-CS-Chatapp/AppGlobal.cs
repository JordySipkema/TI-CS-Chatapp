using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatShared;
using ChatShared.Entity;

namespace TI_CS_Chatapp
{
    public class AppGlobal
    {
        private List<User> Users;

        public AppGlobal () {
            
            //debug
            Users = new List<User>();
            Users.Add(new User("Jordy", "jordy", "123"));
            Users.Add(new User("Bart", "bart", "456"));
            Users.Add(new User("Klaas", "klaas", "789"));

        }
        //return true if succeed
        public bool LoginToServer(string username, string password)
        {
            return true;
        }

        public void SetRememberPassword(bool remember)
        {
            Properties.Settings.Default.RememberPassword = remember;
        }

        public List<string> InitializeContacts()
        {
            /* ***WIP*** 
             * ik haal hieronder de informatie over het netwerk op van de server OF vanuit de cache
             * // Users = ChatServer.Networking.GetUsers();
            */
            
            return Users.Select(user => user.Nickname).ToList();
            
            /* Zo kan het ook:
            var x = from user in Users
                    select user.Nickname;
            return x.ToList();
             */
        }

        // hij zou zn chat in cache moeten opslaan, WIP
        public void Exiting() 
        {
            Properties.Settings.Default.Save();
        }

    }
}
