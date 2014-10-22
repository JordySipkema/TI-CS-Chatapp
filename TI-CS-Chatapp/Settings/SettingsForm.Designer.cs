namespace TI_CS_Chatapp
{
    partial class SettingsForm
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
            this.tbServerIP = new System.Windows.Forms.TextBox();
            this.tbServerPortNumber = new System.Windows.Forms.TextBox();
            this.tbNickname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblClientIP = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbServerIP
            // 
            this.tbServerIP.Location = new System.Drawing.Point(116, 13);
            this.tbServerIP.Name = "tbServerIP";
            this.tbServerIP.Size = new System.Drawing.Size(90, 20);
            this.tbServerIP.TabIndex = 1;
            this.tbServerIP.TextChanged += new System.EventHandler(this.tbServerIP_TextChanged);
            // 
            // tbServerPortNumber
            // 
            this.tbServerPortNumber.Location = new System.Drawing.Point(116, 39);
            this.tbServerPortNumber.Name = "tbServerPortNumber";
            this.tbServerPortNumber.ReadOnly = true;
            this.tbServerPortNumber.Size = new System.Drawing.Size(90, 20);
            this.tbServerPortNumber.TabIndex = 3;
            this.tbServerPortNumber.TextChanged += new System.EventHandler(this.tbServerPortNumber_TextChanged);
            // 
            // tbNickname
            // 
            this.tbNickname.Location = new System.Drawing.Point(116, 68);
            this.tbNickname.Name = "tbNickname";
            this.tbNickname.Size = new System.Drawing.Size(90, 20);
            this.tbNickname.TabIndex = 5;
            this.tbNickname.TextChanged += new System.EventHandler(this.tbNickname_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "IP address of server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Port number";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Nickname";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "My IP:";
            // 
            // lblClientIP
            // 
            this.lblClientIP.AutoSize = true;
            this.lblClientIP.Location = new System.Drawing.Point(113, 100);
            this.lblClientIP.Name = "lblClientIP";
            this.lblClientIP.Size = new System.Drawing.Size(40, 13);
            this.lblClientIP.TabIndex = 10;
            this.lblClientIP.Text = "0.0.0.0";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(45, 132);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(92, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save and close";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(143, 132);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(63, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 171);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblClientIP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbNickname);
            this.Controls.Add(this.tbServerPortNumber);
            this.Controls.Add(this.tbServerIP);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_FormClosing);
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbServerIP;
        private System.Windows.Forms.TextBox tbServerPortNumber;
        private System.Windows.Forms.TextBox tbNickname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblClientIP;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}