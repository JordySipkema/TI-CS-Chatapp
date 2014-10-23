using System;
using System.Windows.Forms;

namespace TI_CS_Chatapp
{
    public partial class MainForm : Form
    {

        // hoogte zonder menustrip en menubalk (-/+/x) is 700 - (24 + 39) = 637
        // user control width/height voor login screen: 484 en 629
        private AppGlobal Global; 
        public MainForm(AppGlobal global)
        {
            InitializeComponent();
            Global = global;
        }

        // ** begin of events ** //
        private void MainForm_Load(object sender, EventArgs e)
        {
            //debug
            loginscreenUC1.Visible = true;
            //end debug
        }

        private void viewLoginScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ucChatSession.Visible = false;
            ucContacts.Visible = false;
            loginscreenUC1.Visible = true;
        }

        private void viewContactScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ucContacts.Visible = true;
            loginscreenUC1.Visible = false;
        }

        private void viewChatScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ucChatSession.Visible = true;
            loginscreenUC1.Visible = false;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SettingsForm().Show();

        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitApp();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExitApp();
        }

        private void toolsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        
        // ** end of events ** //

        private void LoadContactsIntoListBox()
        {
            ucContacts.LoadContacts(Global.InitializeContacts());
        }

        public void Login(string username, string password, bool rememberPassword)
        {
            loginscreenUC1.Visible = false;
            ucChatSession.Visible = true;
            ucContacts.Visible = true;
            bool successful = Global.LoginToServer(username, password);
            Global.SetRememberPassword(rememberPassword);
            if (successful)
            {
                LoadContactsIntoListBox();
            }
        }

        public void Logout()
        {
            ucContacts.RemoveContactsFromListBox();
            ucChatSession.Visible = false;
            ucContacts.Visible = false;
            loginscreenUC1.Visible = true;
        }

        public void ExitApp()
        {
            Logout();
            Global.Exiting();
            Application.Exit();
        }        
        
    }
}
