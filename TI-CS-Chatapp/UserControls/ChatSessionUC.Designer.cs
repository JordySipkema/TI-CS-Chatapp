namespace TI_CS_Chatapp.UserControls
{
    partial class ChatSessionUC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbMsgHistory = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbMessage
            // 
            this.tbMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbMessage.Location = new System.Drawing.Point(12, 594);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(278, 21);
            this.tbMessage.TabIndex = 0;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(296, 591);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 28);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // tbMsgHistory
            // 
            this.tbMsgHistory.Location = new System.Drawing.Point(12, 14);
            this.tbMsgHistory.Multiline = true;
            this.tbMsgHistory.Name = "tbMsgHistory";
            this.tbMsgHistory.Size = new System.Drawing.Size(359, 564);
            this.tbMsgHistory.TabIndex = 2;
            // 
            // ChatSessionUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbMsgHistory);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbMessage);
            this.Name = "ChatSessionUC";
            this.Size = new System.Drawing.Size(384, 629);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox tbMsgHistory;
    }
}
