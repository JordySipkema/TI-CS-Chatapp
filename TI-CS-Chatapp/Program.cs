using System;
using System.Windows.Forms;
using ChatShared;

namespace TI_CS_Chatapp
{
    static class Program
    {
        public static ClientSocket clientSocket;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            Settings.loadSettingsFromFile();
        }

        static void startClientSocket()
        {
            clientSocket = new ClientSocket(Settings.ServerIP, AppProperties.PortNumber);
        }
    }
}