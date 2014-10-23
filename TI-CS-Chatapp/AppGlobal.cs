using ChatShared.Entity;
using System.Collections.Generic;
using System.Linq;

namespace TI_CS_Chatapp
{
    public class AppGlobal
    {
        private readonly List<User> Users;

        public AppGlobal () {
            
            //debug
            Users = new List<User>
            {
                new User("Jordy", "jordy", "123"),
                new User("Bart", "bart", "456"),
                new User("Klaas", "klaas", "789")
            };
        }
        //return true if succeed
        public bool LoginToServer(string username, string password)
        {
            /* ***WIP***
             * hier komt de login code voor het verbinding maken met de server enzovoort.
            */

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
             * // Users = Connection.GetUsers();
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
