using System;
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

        private Datastorage()
        {
            // TODO: Create initializer code (fetching data from FileIO?)
        } 

        public User GetUser(string username)
        {
            return new User("Henk", "testuser01", Crypto.CreateSHA256("1234"));
        }
    }
}
