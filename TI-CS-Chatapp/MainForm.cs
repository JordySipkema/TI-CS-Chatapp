using ChatShared.Entity;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TI_CS_Chatapp.Subforms;

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
            Global.LoginResultEvent += HandleLoginStatus;
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

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RegisterForm().Show();
            
        }  
        
        // ** end of events ** //

        public void LoadContactsIntoListBox(List<User> nicknames)
        {
            ucContacts.LoadContacts(nicknames);
        }

        public void Login(string username, string password, bool rememberPassword)
        {
            Global.LoginToServer(username, password);
            Global.SetRememberPassword(rememberPassword);
        }

        public void HandleLoginStatus(string status)
        {
            if (status == "200") // OK
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((new Action(() => HandleLoginStatus(status))));
                    return;
                }

                Global.InitializeContacts();
                Global.GetAllMessagesFromServer();

                loginscreenUC1.Visible = false;
                ucChatSession.Visible = true;
                ucContacts.Visible = true;
            }
            else if (status == "430") // Invalid Username or Password
            {
                MessageBox.Show("Invalid Username or Password", "Error", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Unhandled error occured", "Error", MessageBoxButtons.OK);
            }
        }

        public void ClearChatHistory()
        {
            ucChatSession.ClearHistory();
        }

        public void Logout()
        {
            ucContacts.RemoveContactsFromListBox();
            ucChatSession.Visible = false;
            ucContacts.Visible = false;
            loginscreenUC1.Visible = true;
            Global.LogoutFromServer();
        }

        public void ExitApp()
        {
            Logout();
            Global.Exiting();
            Application.Exit();
        }

              
        
    }
}
