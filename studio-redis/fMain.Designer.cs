namespace studio_redis
{
    partial class fMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.btnCleanLog = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.panelIP_PORT = new System.Windows.Forms.Panel();
            this.labelMessage = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panelIP_PORT.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCleanLog
            // 
            this.btnCleanLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCleanLog.Location = new System.Drawing.Point(0, 15);
            this.btnCleanLog.Name = "btnCleanLog";
            this.btnCleanLog.Size = new System.Drawing.Size(98, 46);
            this.btnCleanLog.TabIndex = 2;
            this.btnCleanLog.Text = "CLEAN ALL LOG";
            this.btnCleanLog.UseVisualStyleBackColor = true;
            this.btnCleanLog.Click += new System.EventHandler(this.btnCleanLog_Click);
            // 
            // txtPort
            // 
            this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPort.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtPort.Location = new System.Drawing.Point(59, 0);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(39, 20);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "11111";
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtIP
            // 
            this.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIP.Location = new System.Drawing.Point(0, 0);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(59, 20);
            this.txtIP.TabIndex = 0;
            this.txtIP.Text = "127.0.0.1";
            this.txtIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panelIP_PORT
            // 
            this.panelIP_PORT.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelIP_PORT.Controls.Add(this.txtIP);
            this.panelIP_PORT.Controls.Add(this.txtPort);
            this.panelIP_PORT.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelIP_PORT.Location = new System.Drawing.Point(0, 61);
            this.panelIP_PORT.Name = "panelIP_PORT";
            this.panelIP_PORT.Size = new System.Drawing.Size(98, 22);
            this.panelIP_PORT.TabIndex = 3;
            // 
            // labelMessage
            // 
            this.labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMessage.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelMessage.Location = new System.Drawing.Point(28, -1);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(53, 15);
            this.labelMessage.TabIndex = 4;
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(82, -5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(20, 20);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "x";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(98, 83);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.btnCleanLog);
            this.Controls.Add(this.panelIP_PORT);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "LOG Redis";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.fMain_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.fMain_MouseClick);
            this.panelIP_PORT.ResumeLayout(false);
            this.panelIP_PORT.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCleanLog;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Panel panelIP_PORT;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Button btnExit;
    }
}