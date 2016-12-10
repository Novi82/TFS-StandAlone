namespace TFS_SA.GUI
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pnlContent = new System.Windows.Forms.SplitContainer();
            this.btnWorkspace = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxWorkspace = new System.Windows.Forms.ComboBox();
            this.trvServerFolder = new System.Windows.Forms.TreeView();
            this.lblLocalPath = new System.Windows.Forms.LinkLabel();
            this.lstFile = new System.Windows.Forms.ListView();
            this.ItemName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.User = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Lastest = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LastCheckin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imlIcon = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ttProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.ttrMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.btnGetFile = new System.Windows.Forms.Button();
            this.ButtonIcon = new System.Windows.Forms.ImageList(this.components);
            this.btnGetFolder = new System.Windows.Forms.Button();
            this.btnRefesh = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pnlContent)).BeginInit();
            this.pnlContent.Panel1.SuspendLayout();
            this.pnlContent.Panel2.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pnlMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContent.Location = new System.Drawing.Point(12, 65);
            this.pnlContent.Name = "pnlContent";
            // 
            // pnlContent.Panel1
            // 
            this.pnlContent.Panel1.Controls.Add(this.btnWorkspace);
            this.pnlContent.Panel1.Controls.Add(this.label1);
            this.pnlContent.Panel1.Controls.Add(this.cbxWorkspace);
            this.pnlContent.Panel1.Controls.Add(this.trvServerFolder);
            // 
            // pnlContent.Panel2
            // 
            this.pnlContent.Panel2.Controls.Add(this.lblLocalPath);
            this.pnlContent.Panel2.Controls.Add(this.lstFile);
            this.pnlContent.Size = new System.Drawing.Size(922, 456);
            this.pnlContent.SplitterDistance = 337;
            this.pnlContent.TabIndex = 6;
            // 
            // btnWorkspace
            // 
            this.btnWorkspace.Location = new System.Drawing.Point(299, 5);
            this.btnWorkspace.Name = "btnWorkspace";
            this.btnWorkspace.Size = new System.Drawing.Size(35, 21);
            this.btnWorkspace.TabIndex = 13;
            this.btnWorkspace.Text = "...";
            this.btnWorkspace.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Workspace";
            // 
            // cbxWorkspace
            // 
            this.cbxWorkspace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxWorkspace.FormattingEnabled = true;
            this.cbxWorkspace.Location = new System.Drawing.Point(71, 5);
            this.cbxWorkspace.Name = "cbxWorkspace";
            this.cbxWorkspace.Size = new System.Drawing.Size(222, 21);
            this.cbxWorkspace.TabIndex = 12;
            this.cbxWorkspace.SelectedIndexChanged += new System.EventHandler(this.cbxWorkspace_SelectedIndexChanged);
            // 
            // trvServerFolder
            // 
            this.trvServerFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.trvServerFolder.HideSelection = false;
            this.trvServerFolder.Location = new System.Drawing.Point(0, 28);
            this.trvServerFolder.Name = "trvServerFolder";
            this.trvServerFolder.Size = new System.Drawing.Size(337, 428);
            this.trvServerFolder.TabIndex = 1;
            this.trvServerFolder.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvServerFolder_AfterSelect);
            // 
            // lblLocalPath
            // 
            this.lblLocalPath.AutoSize = true;
            this.lblLocalPath.Location = new System.Drawing.Point(3, 8);
            this.lblLocalPath.Name = "lblLocalPath";
            this.lblLocalPath.Size = new System.Drawing.Size(85, 13);
            this.lblLocalPath.TabIndex = 11;
            this.lblLocalPath.TabStop = true;
            this.lblLocalPath.Text = "Map Local Now!";
            this.lblLocalPath.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblLocalPath_LinkClicked);
            // 
            // lstFile
            // 
            this.lstFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ItemName,
            this.User,
            this.Lastest,
            this.LastCheckin});
            this.lstFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstFile.FullRowSelect = true;
            this.lstFile.GridLines = true;
            this.lstFile.HideSelection = false;
            this.lstFile.Location = new System.Drawing.Point(2, 28);
            this.lstFile.Name = "lstFile";
            this.lstFile.Size = new System.Drawing.Size(576, 428);
            this.lstFile.SmallImageList = this.imlIcon;
            this.lstFile.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstFile.TabIndex = 0;
            this.lstFile.UseCompatibleStateImageBehavior = false;
            this.lstFile.View = System.Windows.Forms.View.Details;
            this.lstFile.SelectedIndexChanged += new System.EventHandler(this.lstFile_SelectedIndexChanged);
            this.lstFile.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstFile_MouseDoubleClick);
            // 
            // ItemName
            // 
            this.ItemName.Text = "Name";
            this.ItemName.Width = 238;
            // 
            // User
            // 
            this.User.Text = "User";
            this.User.Width = 95;
            // 
            // Lastest
            // 
            this.Lastest.Text = "Lastest";
            this.Lastest.Width = 74;
            // 
            // LastCheckin
            // 
            this.LastCheckin.Text = "Last Checkin";
            this.LastCheckin.Width = 388;
            // 
            // imlIcon
            // 
            this.imlIcon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlIcon.ImageStream")));
            this.imlIcon.TransparentColor = System.Drawing.Color.Transparent;
            this.imlIcon.Images.SetKeyName(0, "Folder");
            this.imlIcon.Images.SetKeyName(1, "File");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ttProgress,
            this.ttrMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 534);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(946, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ttProgress
            // 
            this.ttProgress.Name = "ttProgress";
            this.ttProgress.Size = new System.Drawing.Size(100, 16);
            // 
            // ttrMessage
            // 
            this.ttrMessage.Name = "ttrMessage";
            this.ttrMessage.Size = new System.Drawing.Size(53, 17);
            this.ttrMessage.Text = "Message";
            // 
            // pnlMenu
            // 
            this.pnlMenu.Controls.Add(this.btnGetFile);
            this.pnlMenu.Controls.Add(this.btnGetFolder);
            this.pnlMenu.Controls.Add(this.btnRefesh);
            this.pnlMenu.Controls.Add(this.btnConnect);
            this.pnlMenu.Location = new System.Drawing.Point(12, 12);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(922, 47);
            this.pnlMenu.TabIndex = 11;
            // 
            // btnGetFile
            // 
            this.btnGetFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGetFile.ImageKey = "Get";
            this.btnGetFile.ImageList = this.ButtonIcon;
            this.btnGetFile.Location = new System.Drawing.Point(279, 3);
            this.btnGetFile.Name = "btnGetFile";
            this.btnGetFile.Padding = new System.Windows.Forms.Padding(10, 0, 25, 0);
            this.btnGetFile.Size = new System.Drawing.Size(131, 41);
            this.btnGetFile.TabIndex = 1;
            this.btnGetFile.Text = "Get File";
            this.btnGetFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGetFile.UseVisualStyleBackColor = true;
            this.btnGetFile.Click += new System.EventHandler(this.btnGetFile_Click);
            // 
            // ButtonIcon
            // 
            this.ButtonIcon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ButtonIcon.ImageStream")));
            this.ButtonIcon.TransparentColor = System.Drawing.Color.Transparent;
            this.ButtonIcon.Images.SetKeyName(0, "Get");
            this.ButtonIcon.Images.SetKeyName(1, "Refresh");
            // 
            // btnGetFolder
            // 
            this.btnGetFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGetFolder.ImageKey = "Get";
            this.btnGetFolder.ImageList = this.ButtonIcon;
            this.btnGetFolder.Location = new System.Drawing.Point(142, 3);
            this.btnGetFolder.Name = "btnGetFolder";
            this.btnGetFolder.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.btnGetFolder.Size = new System.Drawing.Size(131, 41);
            this.btnGetFolder.TabIndex = 0;
            this.btnGetFolder.Text = "Get Folder";
            this.btnGetFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGetFolder.UseVisualStyleBackColor = true;
            this.btnGetFolder.Click += new System.EventHandler(this.btnGetFolder_Click);
            // 
            // btnRefesh
            // 
            this.btnRefesh.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefesh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefesh.ImageKey = "Refresh";
            this.btnRefesh.ImageList = this.ButtonIcon;
            this.btnRefesh.Location = new System.Drawing.Point(3, 3);
            this.btnRefesh.Name = "btnRefesh";
            this.btnRefesh.Padding = new System.Windows.Forms.Padding(8, 0, 25, 0);
            this.btnRefesh.Size = new System.Drawing.Size(131, 41);
            this.btnRefesh.TabIndex = 0;
            this.btnRefesh.Text = "Refesh";
            this.btnRefesh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRefesh.UseVisualStyleBackColor = true;
            this.btnRefesh.Click += new System.EventHandler(this.btnRefesh_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.Location = new System.Drawing.Point(788, 4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(131, 41);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 556);
            this.Controls.Add(this.pnlMenu);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlContent);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TFS StandAlone";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.pnlContent.Panel1.ResumeLayout(false);
            this.pnlContent.Panel1.PerformLayout();
            this.pnlContent.Panel2.ResumeLayout(false);
            this.pnlContent.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlContent)).EndInit();
            this.pnlContent.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer pnlContent;
        private System.Windows.Forms.TreeView trvServerFolder;
        private System.Windows.Forms.ListView lstFile;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.LinkLabel lblLocalPath;
        private System.Windows.Forms.ToolStripProgressBar ttProgress;
        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ToolStripStatusLabel ttrMessage;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ColumnHeader ItemName;
        private System.Windows.Forms.ColumnHeader User;
        private System.Windows.Forms.ColumnHeader Lastest;
        private System.Windows.Forms.ColumnHeader LastCheckin;
        private System.Windows.Forms.ImageList imlIcon;
        private System.Windows.Forms.Button btnGetFolder;
        private System.Windows.Forms.Button btnRefesh;
        private System.Windows.Forms.ImageList ButtonIcon;
        private System.Windows.Forms.Button btnGetFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxWorkspace;
        private System.Windows.Forms.Button btnWorkspace;
    }
}

