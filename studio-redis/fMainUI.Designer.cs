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
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblKeyID_Counter = new System.Windows.Forms.Label();
            this.lblKeyID_Selected = new System.Windows.Forms.Label();
            this.panelListHeader = new System.Windows.Forms.Panel();
            this.button_key_del_all = new System.Windows.Forms.Button();
            this.btnLogItem_DEL = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panelTreeView = new System.Windows.Forms.Panel();
            this.btnKeyDEL = new System.Windows.Forms.Button();
            this.treeKeys = new System.Windows.Forms.TreeView();
            this.labelKey_Selected = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelEditor = new System.Windows.Forms.Panel();
            this.txtEditor = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblKeySelected_Path = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnEditorFontBigger = new System.Windows.Forms.Button();
            this.btnEditorFontSmaller = new System.Windows.Forms.Button();
            this.btnEditorSearch = new System.Windows.Forms.Button();
            this.txtEditorSearch = new System.Windows.Forms.TextBox();
            this.btnLogItemSaveFile = new System.Windows.Forms.Button();
            this.lblKeySelected_Time = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelList.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelListHeader.SuspendLayout();
            this.panelTreeView.SuspendLayout();
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
            this.lblMessage.Text = "View LOG Redis";
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
            this.panelLeft.Controls.Add(this.panelTreeView);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 38);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(243, 552);
            this.panelLeft.TabIndex = 1;
            // 
            // panelList
            // 
            this.panelList.Controls.Add(this.listKeys);
            this.panelList.Controls.Add(this.panel2);
            this.panelList.Controls.Add(this.panelListHeader);
            this.panelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelList.Location = new System.Drawing.Point(0, 197);
            this.panelList.Name = "panelList";
            this.panelList.Size = new System.Drawing.Size(243, 355);
            this.panelList.TabIndex = 2;
            // 
            // listKeys
            // 
            this.listKeys.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.listKeys.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listKeys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listKeys.FormattingEnabled = true;
            this.listKeys.Location = new System.Drawing.Point(0, 51);
            this.listKeys.Name = "listKeys";
            this.listKeys.Size = new System.Drawing.Size(243, 304);
            this.listKeys.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel2.Controls.Add(this.lblKeyID_Counter);
            this.panel2.Controls.Add(this.lblKeyID_Selected);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(243, 27);
            this.panel2.TabIndex = 2;
            // 
            // lblKeyID_Counter
            // 
            this.lblKeyID_Counter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKeyID_Counter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeyID_Counter.ForeColor = System.Drawing.Color.Crimson;
            this.lblKeyID_Counter.Location = new System.Drawing.Point(116, -1);
            this.lblKeyID_Counter.Name = "lblKeyID_Counter";
            this.lblKeyID_Counter.Size = new System.Drawing.Size(125, 12);
            this.lblKeyID_Counter.TabIndex = 1;
            this.lblKeyID_Counter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblKeyID_Selected
            // 
            this.lblKeyID_Selected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKeyID_Selected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeyID_Selected.ForeColor = System.Drawing.Color.MediumBlue;
            this.lblKeyID_Selected.Location = new System.Drawing.Point(3, 10);
            this.lblKeyID_Selected.Name = "lblKeyID_Selected";
            this.lblKeyID_Selected.Size = new System.Drawing.Size(238, 15);
            this.lblKeyID_Selected.TabIndex = 0;
            // 
            // panelListHeader
            // 
            this.panelListHeader.Controls.Add(this.button_key_del_all);
            this.panelListHeader.Controls.Add(this.btnLogItem_DEL);
            this.panelListHeader.Controls.Add(this.txtSearch);
            this.panelListHeader.Controls.Add(this.btnSearch);
            this.panelListHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelListHeader.Location = new System.Drawing.Point(0, 0);
            this.panelListHeader.Name = "panelListHeader";
            this.panelListHeader.Size = new System.Drawing.Size(243, 24);
            this.panelListHeader.TabIndex = 0;
            // 
            // button_key_del_all
            // 
            this.button_key_del_all.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_key_del_all.BackColor = System.Drawing.SystemColors.Control;
            this.button_key_del_all.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_key_del_all.Location = new System.Drawing.Point(200, 2);
            this.button_key_del_all.Margin = new System.Windows.Forms.Padding(0);
            this.button_key_del_all.Name = "button_key_del_all";
            this.button_key_del_all.Size = new System.Drawing.Size(42, 20);
            this.button_key_del_all.TabIndex = 10;
            this.button_key_del_all.Text = "del all";
            this.button_key_del_all.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_key_del_all.UseVisualStyleBackColor = false;
            this.button_key_del_all.Click += new System.EventHandler(this.button_key_del_all_Click);
            // 
            // btnLogItem_DEL
            // 
            this.btnLogItem_DEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogItem_DEL.BackColor = System.Drawing.SystemColors.Control;
            this.btnLogItem_DEL.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnLogItem_DEL.Location = new System.Drawing.Point(169, 2);
            this.btnLogItem_DEL.Margin = new System.Windows.Forms.Padding(0);
            this.btnLogItem_DEL.Name = "btnLogItem_DEL";
            this.btnLogItem_DEL.Size = new System.Drawing.Size(33, 20);
            this.btnLogItem_DEL.TabIndex = 9;
            this.btnLogItem_DEL.Text = "del";
            this.btnLogItem_DEL.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLogItem_DEL.UseVisualStyleBackColor = false;
            this.btnLogItem_DEL.Click += new System.EventHandler(this.btnLogItem_DEL_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(3, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(118, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            this.txtSearch.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtSearch_MouseDoubleClick);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(123, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(48, 21);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // splitter2
            // 
            this.splitter2.BackColor = System.Drawing.SystemColors.HotTrack;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 192);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(243, 5);
            this.splitter2.TabIndex = 1;
            this.splitter2.TabStop = false;
            // 
            // panelTreeView
            // 
            this.panelTreeView.Controls.Add(this.btnKeyDEL);
            this.panelTreeView.Controls.Add(this.treeKeys);
            this.panelTreeView.Controls.Add(this.labelKey_Selected);
            this.panelTreeView.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTreeView.Location = new System.Drawing.Point(0, 0);
            this.panelTreeView.Name = "panelTreeView";
            this.panelTreeView.Size = new System.Drawing.Size(243, 192);
            this.panelTreeView.TabIndex = 3;
            // 
            // btnKeyDEL
            // 
            this.btnKeyDEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKeyDEL.Location = new System.Drawing.Point(200, 0);
            this.btnKeyDEL.Name = "btnKeyDEL";
            this.btnKeyDEL.Size = new System.Drawing.Size(45, 20);
            this.btnKeyDEL.TabIndex = 2;
            this.btnKeyDEL.Text = "DEL";
            this.btnKeyDEL.UseVisualStyleBackColor = true;
            this.btnKeyDEL.Click += new System.EventHandler(this.btnKeyDEL_Click);
            // 
            // treeKeys
            // 
            this.treeKeys.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.treeKeys.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeKeys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeKeys.Location = new System.Drawing.Point(0, 19);
            this.treeKeys.Name = "treeKeys";
            this.treeKeys.Size = new System.Drawing.Size(243, 173);
            this.treeKeys.TabIndex = 0;
            // 
            // labelKey_Selected
            // 
            this.labelKey_Selected.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelKey_Selected.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelKey_Selected.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKey_Selected.ForeColor = System.Drawing.Color.Blue;
            this.labelKey_Selected.Location = new System.Drawing.Point(0, 0);
            this.labelKey_Selected.Name = "labelKey_Selected";
            this.labelKey_Selected.Size = new System.Drawing.Size(243, 19);
            this.labelKey_Selected.TabIndex = 1;
            this.labelKey_Selected.Click += new System.EventHandler(this.labelKey_Selected_Click);
            this.labelKey_Selected.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.labelKey_Selected_MouseDoubleClick);
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.splitter1.Location = new System.Drawing.Point(243, 38);
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
            this.panelMain.Location = new System.Drawing.Point(248, 38);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(776, 552);
            this.panelMain.TabIndex = 3;
            // 
            // panelEditor
            // 
            this.panelEditor.Controls.Add(this.txtEditor);
            this.panelEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditor.Location = new System.Drawing.Point(0, 37);
            this.panelEditor.Name = "panelEditor";
            this.panelEditor.Padding = new System.Windows.Forms.Padding(9, 9, 0, 0);
            this.panelEditor.Size = new System.Drawing.Size(776, 515);
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
            this.txtEditor.Size = new System.Drawing.Size(767, 506);
            this.txtEditor.TabIndex = 1;
            this.txtEditor.Text = "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.lblKeySelected_Path);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnEditorFontBigger);
            this.panel1.Controls.Add(this.btnEditorFontSmaller);
            this.panel1.Controls.Add(this.btnEditorSearch);
            this.panel1.Controls.Add(this.txtEditorSearch);
            this.panel1.Controls.Add(this.btnLogItemSaveFile);
            this.panel1.Controls.Add(this.lblKeySelected_Time);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 37);
            this.panel1.TabIndex = 0;
            // 
            // lblKeySelected_Path
            // 
            this.lblKeySelected_Path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKeySelected_Path.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeySelected_Path.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblKeySelected_Path.Location = new System.Drawing.Point(5, 2);
            this.lblKeySelected_Path.Name = "lblKeySelected_Path";
            this.lblKeySelected_Path.Size = new System.Drawing.Size(510, 13);
            this.lblKeySelected_Path.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(0, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(776, 1);
            this.label2.TabIndex = 8;
            // 
            // btnEditorFontBigger
            // 
            this.btnEditorFontBigger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditorFontBigger.BackColor = System.Drawing.SystemColors.Control;
            this.btnEditorFontBigger.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnEditorFontBigger.Location = new System.Drawing.Point(535, 7);
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
            this.btnEditorFontSmaller.Location = new System.Drawing.Point(516, 7);
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
            this.btnEditorSearch.Location = new System.Drawing.Point(684, 7);
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
            this.txtEditorSearch.Location = new System.Drawing.Point(556, 7);
            this.txtEditorSearch.Name = "txtEditorSearch";
            this.txtEditorSearch.Size = new System.Drawing.Size(128, 20);
            this.txtEditorSearch.TabIndex = 3;
            // 
            // btnLogItemSaveFile
            // 
            this.btnLogItemSaveFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogItemSaveFile.BackColor = System.Drawing.SystemColors.Control;
            this.btnLogItemSaveFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnLogItemSaveFile.Location = new System.Drawing.Point(726, 7);
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
            this.lblKeySelected_Time.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeySelected_Time.ForeColor = System.Drawing.Color.Blue;
            this.lblKeySelected_Time.Location = new System.Drawing.Point(5, 15);
            this.lblKeySelected_Time.Name = "lblKeySelected_Time";
            this.lblKeySelected_Time.Size = new System.Drawing.Size(488, 19);
            this.lblKeySelected_Time.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(3, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "//";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.Text = "View LOG Redis";
            this.Load += new System.EventHandler(this.fMainUI_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelList.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panelListHeader.ResumeLayout(false);
            this.panelListHeader.PerformLayout();
            this.panelTreeView.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnKeyDEL;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblKeyID_Counter;
        private System.Windows.Forms.Label lblKeyID_Selected;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelTreeView;
        private System.Windows.Forms.Label labelKey_Selected;
        private System.Windows.Forms.Button btnLogItem_DEL;
        private System.Windows.Forms.Button button_key_del_all;
    }
}