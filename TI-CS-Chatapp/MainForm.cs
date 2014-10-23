using System;
using System.Windows.Forms;

namespace TI_CS_Chatapp
{
    public partial class MainForm : Form
    {

        // hoogte zonder menustrip en menubalk (-/+/x) is 700 - (24 + 39) = 637
        // user control width/height voor login screen: 484 en 629
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //loginscreenUC1.Visible = true;
            ucChatSession.Visible = true;
            ucContacts.Visible = true;
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

        private void loginLogoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        
    }
}
