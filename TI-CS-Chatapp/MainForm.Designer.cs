namespace TI_CS_Chatapp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginLogoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLoginScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewContactScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewChatScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ucChatSession = new TI_CS_Chatapp.UserControls.ChatSessionUC();
            this.ucContacts = new TI_CS_Chatapp.ContactsUserControl();
            this.loginscreenUC1 = new TI_CS_Chatapp.UserControls.LoginscreenUC();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logoutToolStripMenuItem,
            this.loginLogoutToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.fileToolStripMenuItem.Text = "Chatapp";
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.logoutToolStripMenuItem.Text = "Logout";
            // 
            // loginLogoutToolStripMenuItem
            // 
            this.loginLogoutToolStripMenuItem.Name = "loginLogoutToolStripMenuItem";
            this.loginLogoutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loginLogoutToolStripMenuItem.Text = "Exit";
            this.loginLogoutToolStripMenuItem.Click += new System.EventHandler(this.loginLogoutToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.debugToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(584, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewLoginScreenToolStripMenuItem,
            this.viewContactScreenToolStripMenuItem,
            this.viewChatScreenToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.debugToolStripMenuItem.Text = "Debug";
            // 
            // viewLoginScreenToolStripMenuItem
            // 
            this.viewLoginScreenToolStripMenuItem.Name = "viewLoginScreenToolStripMenuItem";
            this.viewLoginScreenToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.viewLoginScreenToolStripMenuItem.Text = "View login screen";
            this.viewLoginScreenToolStripMenuItem.Click += new System.EventHandler(this.viewLoginScreenToolStripMenuItem_Click);
            // 
            // viewContactScreenToolStripMenuItem
            // 
            this.viewContactScreenToolStripMenuItem.Name = "viewContactScreenToolStripMenuItem";
            this.viewContactScreenToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.viewContactScreenToolStripMenuItem.Text = "View contact screen";
            this.viewContactScreenToolStripMenuItem.Click += new System.EventHandler(this.viewContactScreenToolStripMenuItem_Click);
            // 
            // viewChatScreenToolStripMenuItem
            // 
            this.viewChatScreenToolStripMenuItem.Name = "viewChatScreenToolStripMenuItem";
            this.viewChatScreenToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.viewChatScreenToolStripMenuItem.Text = "View chat screen";
            this.viewChatScreenToolStripMenuItem.Click += new System.EventHandler(this.viewChatScreenToolStripMenuItem_Click);
            // 
            // ucChatSession
            // 
            this.ucChatSession.Location = new System.Drawing.Point(200, 27);
            this.ucChatSession.Name = "ucChatSession";
            this.ucChatSession.Size = new System.Drawing.Size(384, 629);
            this.ucChatSession.TabIndex = 3;
            this.ucChatSession.Visible = false;
            // 
            // ucContacts
            // 
            this.ucContacts.Location = new System.Drawing.Point(0, 27);
            this.ucContacts.Name = "ucContacts";
            this.ucContacts.Size = new System.Drawing.Size(200, 629);
            this.ucContacts.TabIndex = 2;
            this.ucContacts.Visible = false;
            // 
            // loginscreenUC1
            // 
            this.loginscreenUC1.Location = new System.Drawing.Point(0, 27);
            this.loginscreenUC1.Name = "loginscreenUC1";
            this.loginscreenUC1.Size = new System.Drawing.Size(584, 629);
            this.loginscreenUC1.TabIndex = 1;
            this.loginscreenUC1.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 661);
            this.Controls.Add(this.ucChatSession);
            this.Controls.Add(this.ucContacts);
            this.Controls.Add(this.loginscreenUC1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Chatapp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginLogoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private UserControls.LoginscreenUC loginscreenUC1;
        private ContactsUserControl ucContacts;
        private UserControls.ChatSessionUC ucChatSession;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLoginScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewContactScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewChatScreenToolStripMenuItem;

    }
}

