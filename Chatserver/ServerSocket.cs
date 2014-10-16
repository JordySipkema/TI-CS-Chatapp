using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatserver
{

    class ServerSocket
    {
        public readonly int ServerPort;

        public ServerSocket()
        {
            ServerPort = 8000;

        }

        public ServerSocket(int serverPort) 
        {
            this.ServerPort = serverPort;

        }

        public void initializeSocket()
        {


        }

    }
}
