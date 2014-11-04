using System;
using System.Windows.Forms;

namespace TI_CS_Chatapp.UserControls
{
    public partial class LoginscreenUC : UserControl
    {
        public LoginscreenUC()
        {
            InitializeComponent();

            if (!Properties.Settings.Default.RememberPassword) return;

            chkPassword.Checked = true;
            tbUsername.Text = Properties.Settings.Default.Username;
            tbPassword.Text = "******";
        }

        private void lblSignin_Click(object sender, EventArgs e)
        {
            var MyForm = (MainForm)Parent;
            tbPassword.Text = tbPassword.Text.Trim();
            MyForm.Login(tbUsername.Text, tbPassword.Text, chkPassword.CheckState.HasFlag(CheckState.Checked));
            tbPassword.Clear();
        }

        private void tbPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                lblSignin_Click(sender, new EventArgs());

            }
        }


    }
}
