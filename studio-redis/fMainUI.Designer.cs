namespace studio_redis
{
    partial class fMainUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMainUI));
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnWriteFile = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnMin = new System.Windows.Forms.Button();
            this.btnMax = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelList = new System.Windows.Forms.Panel();
            this.listKeys = new System.Windows.Forms.CheckedListBox();
            this.panelListHeader = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.treeKeys = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelEditor = new System.Windows.Forms.Panel();
            this.txtEditor = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnEditorFontBigger = new System.Windows.Forms.Button();
            this.btnEditorFontSmaller = new System.Windows.Forms.Button();
            this.btnEditorSearch = new System.Windows.Forms.Button();
            this.txtEditorSearch = new System.Windows.Forms.TextBox();
            this.btnLogItemSaveFile = new System.Windows.Forms.Button();
            this.lblKeySelected_Time = new System.Windows.Forms.Label();
            this.lblKeySelected_Path = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelList.SuspendLayout();
            this.panelListHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelEditor.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panelHeader.Controls.Add(this.lblMessage);
            this.panelHeader.Controls.Add(this.btnWriteFile);
            this.panelHeader.Controls.Add(this.btnClearAll);
            this.panelHeader.Controls.Add(this.btnLoad);
            this.panelHeader.Controls.Add(this.txtPort);
            this.panelHeader.Controls.Add(this.txtIP);
            this.panelHeader.Controls.Add(this.btnMin);
            this.panelHeader.Controls.Add(this.btnMax);
            this.panelHeader.Controls.Add(this.btnClose);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1024, 38);
            this.panelHeader.TabIndex = 0;
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblMessage.Location = new System.Drawing.Point(384, 4);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(527, 30);
            this.lblMessage.TabIndex = 8;
            this.lblMessage.Text = "LOG Redis";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnWriteFile
            // 
            this.btnWriteFile.Location = new System.Drawing.Point(304, 7);
            this.btnWriteFile.Name = "btnWriteFile";
            this.btnWriteFile.Size = new System.Drawing.Size(75, 23);
            this.btnWriteFile.TabIndex = 7;
            this.btnWriteFile.Text = "Export File";
            this.btnWriteFile.UseVisualStyleBackColor = true;
            this.btnWriteFile.Click += new System.EventHandler(this.btnWriteFile_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.Location = new System.Drawing.Point(223, 7);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(75, 23);
            this.btnClearAll.TabIndex = 6;
            this.btnClearAll.Text = "Clean All";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(142, 7);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(52, 23);
            this.btnLoad.TabIndex = 5;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(95, 8);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(46, 20);
            this.txtPort.TabIndex = 4;
            this.txtPort.Text = "11111";
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(6, 8);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(87, 20);
            this.txtIP.TabIndex = 3;
            this.txtIP.Text = "127.0.0.1";
            this.txtIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnMin
            // 
            this.btnMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMin.Location = new System.Drawing.Point(915, 7);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(29, 23);
            this.btnMin.TabIndex = 2;
            this.btnMin.Text = "-";
            this.btnMin.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMin.UseVisualStyleBackColor = true;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // btnMax
            // 
            this.btnMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMax.Location = new System.Drawing.Point(951, 7);
            this.btnMax.Name = "btnMax";
            this.btnMax.Size = new System.Drawing.Size(30, 23);
            this.btnMax.TabIndex = 1;
            this.btnMax.Text = "[  ]";
            this.btnMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMax.UseVisualStyleBackColor = true;
            this.btnMax.Click += new System.EventHandler(this.btnMax_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(987, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panelLeft.Controls.Add(this.panelList);
            this.panelLeft.Controls.Add(this.splitter2);
            this.panelLeft.Controls.Add(this.treeKeys);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 38);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(200, 552);
            this.panelLeft.TabIndex = 1;
            // 
            // panelList
            // 
            this.panelList.Controls.Add(this.listKeys);
            this.panelList.Controls.Add(this.panelListHeader);
            this.panelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelList.Location = new System.Drawing.Point(0, 276);
            this.panelList.Name = "panelList";
            this.panelList.Size = new System.Drawing.Size(200, 276);
            this.panelList.TabIndex = 2;
            // 
            // listKeys
            // 
            this.listKeys.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.listKeys.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listKeys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listKeys.FormattingEnabled = true;
            this.listKeys.Location = new System.Drawing.Point(0, 29);
            this.listKeys.Name = "listKeys";
            this.listKeys.Size = new System.Drawing.Size(200, 247);
            this.listKeys.TabIndex = 1;
            // 
            // panelListHeader
            // 
            this.panelListHeader.Controls.Add(this.txtSearch);
            this.panelListHeader.Controls.Add(this.btnSearch);
            this.panelListHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelListHeader.Location = new System.Drawing.Point(0, 0);
            this.panelListHeader.Name = "panelListHeader";
            this.panelListHeader.Size = new System.Drawing.Size(200, 29);
            this.panelListHeader.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(3, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(135, 20);
            this.txtSearch.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(139, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(59, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // splitter2
            // 
            this.splitter2.BackColor = System.Drawing.SystemColors.HotTrack;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 271);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(200, 5);
            this.splitter2.TabIndex = 1;
            this.splitter2.TabStop = false;
            // 
            // treeKeys
            // 
            this.treeKeys.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.treeKeys.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeKeys.Dock = System.Windows.Forms.DockStyle.Top;
            this.treeKeys.Location = new System.Drawing.Point(0, 0);
            this.treeKeys.Name = "treeKeys";
            this.treeKeys.Size = new System.Drawing.Size(200, 271);
            this.treeKeys.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.splitter1.Location = new System.Drawing.Point(200, 38);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 552);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelEditor);
            this.panelMain.Controls.Add(this.panel1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(205, 38);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(819, 552);
            this.panelMain.TabIndex = 3;
            // 
            // panelEditor
            // 
            this.panelEditor.Controls.Add(this.txtEditor);
            this.panelEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditor.Location = new System.Drawing.Point(0, 23);
            this.panelEditor.Name = "panelEditor";
            this.panelEditor.Padding = new System.Windows.Forms.Padding(9, 9, 0, 0);
            this.panelEditor.Size = new System.Drawing.Size(819, 529);
            this.panelEditor.TabIndex = 2;
            // 
            // txtEditor
            // 
            this.txtEditor.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEditor.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.txtEditor.Location = new System.Drawing.Point(9, 9);
            this.txtEditor.Name = "txtEditor";
            this.txtEditor.ReadOnly = true;
            this.txtEditor.Size = new System.Drawing.Size(810, 520);
            this.txtEditor.TabIndex = 1;
            this.txtEditor.Text = "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Controls.Add(this.btnEditorFontBigger);
            this.panel1.Controls.Add(this.btnEditorFontSmaller);
            this.panel1.Controls.Add(this.btnEditorSearch);
            this.panel1.Controls.Add(this.txtEditorSearch);
            this.panel1.Controls.Add(this.btnLogItemSaveFile);
            this.panel1.Controls.Add(this.lblKeySelected_Time);
            this.panel1.Controls.Add(this.lblKeySelected_Path);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(819, 23);
            this.panel1.TabIndex = 0;
            // 
            // btnEditorFontBigger
            // 
            this.btnEditorFontBigger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditorFontBigger.BackColor = System.Drawing.SystemColors.Control;
            this.btnEditorFontBigger.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnEditorFontBigger.Location = new System.Drawing.Point(586, 1);
            this.btnEditorFontBigger.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditorFontBigger.Name = "btnEditorFontBigger";
            this.btnEditorFontBigger.Size = new System.Drawing.Size(21, 20);
            this.btnEditorFontBigger.TabIndex = 6;
            this.btnEditorFontBigger.Text = "+";
            this.btnEditorFontBigger.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEditorFontBigger.UseVisualStyleBackColor = false;
            this.btnEditorFontBigger.Click += new System.EventHandler(this.btnEditorFontBigger_Click);
            // 
            // btnEditorFontSmaller
            // 
            this.btnEditorFontSmaller.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditorFontSmaller.BackColor = System.Drawing.SystemColors.Control;
            this.btnEditorFontSmaller.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnEditorFontSmaller.Location = new System.Drawing.Point(567, 1);
            this.btnEditorFontSmaller.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditorFontSmaller.Name = "btnEditorFontSmaller";
            this.btnEditorFontSmaller.Size = new System.Drawing.Size(20, 20);
            this.btnEditorFontSmaller.TabIndex = 5;
            this.btnEditorFontSmaller.Text = "-";
            this.btnEditorFontSmaller.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEditorFontSmaller.UseVisualStyleBackColor = false;
            this.btnEditorFontSmaller.Click += new System.EventHandler(this.btnEditorFontSmaller_Click);
            // 
            // btnEditorSearch
            // 
            this.btnEditorSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditorSearch.BackColor = System.Drawing.SystemColors.Control;
            this.btnEditorSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnEditorSearch.Location = new System.Drawing.Point(735, 1);
            this.btnEditorSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditorSearch.Name = "btnEditorSearch";
            this.btnEditorSearch.Size = new System.Drawing.Size(42, 20);
            this.btnEditorSearch.TabIndex = 4;
            this.btnEditorSearch.Text = "search";
            this.btnEditorSearch.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEditorSearch.UseVisualStyleBackColor = false;
            this.btnEditorSearch.Click += new System.EventHandler(this.btnEditorSearch_Click);
            // 
            // txtEditorSearch
            // 
            this.txtEditorSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEditorSearch.Location = new System.Drawing.Point(607, 1);
            this.txtEditorSearch.Name = "txtEditorSearch";
            this.txtEditorSearch.Size = new System.Drawing.Size(128, 20);
            this.txtEditorSearch.TabIndex = 3;
            // 
            // btnLogItemSaveFile
            // 
            this.btnLogItemSaveFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogItemSaveFile.BackColor = System.Drawing.SystemColors.Control;
            this.btnLogItemSaveFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnLogItemSaveFile.Location = new System.Drawing.Point(777, 1);
            this.btnLogItemSaveFile.Margin = new System.Windows.Forms.Padding(0);
            this.btnLogItemSaveFile.Name = "btnLogItemSaveFile";
            this.btnLogItemSaveFile.Size = new System.Drawing.Size(41, 20);
            this.btnLogItemSaveFile.TabIndex = 2;
            this.btnLogItemSaveFile.Text = "save";
            this.btnLogItemSaveFile.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLogItemSaveFile.UseVisualStyleBackColor = false;
            this.btnLogItemSaveFile.Click += new System.EventHandler(this.btnLogItemSaveFile_Click);
            // 
            // lblKeySelected_Time
            // 
            this.lblKeySelected_Time.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKeySelected_Time.AutoSize = true;
            this.lblKeySelected_Time.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeySelected_Time.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblKeySelected_Time.Location = new System.Drawing.Point(391, 5);
            this.lblKeySelected_Time.Name = "lblKeySelected_Time";
            this.lblKeySelected_Time.Size = new System.Drawing.Size(174, 13);
            this.lblKeySelected_Time.TabIndex = 1;
            this.lblKeySelected_Time.Text = "19 - 03 - 2020 08:09:35 - 169";
            this.lblKeySelected_Time.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblKeySelected_Path
            // 
            this.lblKeySelected_Path.AutoSize = true;
            this.lblKeySelected_Path.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeySelected_Path.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblKeySelected_Path.Location = new System.Drawing.Point(3, 4);
            this.lblKeySelected_Path.Name = "lblKeySelected_Path";
            this.lblKeySelected_Path.Size = new System.Drawing.Size(270, 13);
            this.lblKeySelected_Path.TabIndex = 0;
            this.lblKeySelected_Path.Text = "/ MAIN / USER_ON_OFF / 200319131441132";
            // 
            // fMainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1024, 590);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fMainUI";
            this.Text = "LOG";
            this.Load += new System.EventHandler(this.fMainUI_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelList.ResumeLayout(false);
            this.panelListHeader.ResumeLayout(false);
            this.panelListHeader.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelEditor.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnMin;
        private System.Windows.Forms.Button btnMax;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnWriteFile;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelEditor;
        private System.Windows.Forms.RichTextBox txtEditor;
        private System.Windows.Forms.TreeView treeKeys;
        private System.Windows.Forms.Label lblKeySelected_Time;
        private System.Windows.Forms.Label lblKeySelected_Path;
        private System.Windows.Forms.Button btnLogItemSaveFile;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panelList;
        private System.Windows.Forms.CheckedListBox listKeys;
        private System.Windows.Forms.Panel panelListHeader;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnEditorFontBigger;
        private System.Windows.Forms.Button btnEditorFontSmaller;
        private System.Windows.Forms.Button btnEditorSearch;
        private System.Windows.Forms.TextBox txtEditorSearch;
    }
}