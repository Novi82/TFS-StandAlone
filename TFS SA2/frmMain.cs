using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TFS_SA.Gui_Support;
using TFS_SA.TfsWapper;

namespace TFS_SA.GUI
{

    public partial class frmMain : Form
    {
        #region Variable

        //private String MappedPath = @"D:/test";
        protected TfsWrapper tfs;
        protected String localPath;
        protected String ServerRootDir;
        protected FolderTreeSupport treeSupport;
        protected ListViewSupport lvSupport;
        protected Boolean isConnected;
        protected Item ServerItem;

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
            init();
        }

        #endregion

        #region Event

        #region Form
        protected void frmMain_Shown(object sender, EventArgs e)
        {
            try
            {
                this.ChangeInputArea(InputMode.Disable);
                this.ttrMessage.Text = @"Loading Setting...";
                this.StartProgress();
                this.isConnected = this.loadConnection();

                if (!isConnected)
                {
                    var rs = tfs.Connect();
                    switch (rs)
                    {
                        case TFS_ConnectResult.Success:
                            this.isConnected = tfs.IsConnected;
                            break;
                        case TFS_ConnectResult.Cancel:
                            Environment.Exit(0);
                            break;
                    }
                }
                this.AddWorkSapces();
                if (cbxWorkspace.Items.Count > 0)
                {
                    cbxWorkspace.SelectedIndex = 0;
                }

                var root = this.tfs.GetRoot();
                treeSupport.AddRootNode(root);
                this.StopProgress();
                this.ttrMessage.Text = @"Load Completed";
                this.ChangeInputArea(InputMode.Enable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Button

        protected void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (tfs.IsConnected)
                {
                    var root = this.tfs.GetRoot();
                    treeSupport.AddRootNode(root);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        protected void lblLocalPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var localPath = Path.Combine(lblLocalPath.Text);

                if (!tfs.IsLocalMapped(localPath))
                {
                    this.setMapLocal();
                }

                if (Directory.Exists(localPath))
                {
                    Process.Start(localPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        protected void btnGetFolder_Click(object sender, EventArgs e)
        {
            try
            {
                this.ChangeInputArea(InputMode.Wait);
                this.StartProgress();
                if (!tfs.IsServerMapped(ServerItem.ServerItem))
                {
                    MessageBox.Show("Folder have not been mapped!");
                    return;
                }
                var name = tfs.getLocalItem(ServerItem.ServerItem);
                this.ttrMessage.Text = "Downloading " + name;
                var rs = tfs.dowload(ServerItem.ServerItem);
                var msg = rs == Result.Success ? "Downloaded " + name : "Download failure";
                this.ttrMessage.Text = msg;
            }
            catch (Exception ex)
            {
                this.ttrMessage.Text = "Download failure";
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.ChangeInputArea(InputMode.Enable);
                this.StopProgress();
            }
        }

        protected void btnGetFile_Click(object sender, EventArgs e)
        {
            try
            {
                this.ChangeInputArea(InputMode.Wait);
                this.StartProgress();
                String fileName = String.Empty;
                var serverItem = lvSupport.getSelectedItem(ref fileName);
                this.ttrMessage.Text = "Downloading " + fileName;
                var rs = tfs.dowload(serverItem.ServerItem);
                var msg = rs == Result.Success ? "Downloaded " + fileName : "Download failure";
                this.ttrMessage.Text = msg;
            }
            catch (Exception ex)
            {
                this.ttrMessage.Text = "Download failure";
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.ChangeInputArea(InputMode.Enable);
                this.StopProgress();
            }
        }
        #endregion

        #region listview

        protected void lstFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var workspace = tfs.WorkSpace;
                if (workspace.MappingsAvailable)
                {

                }
                else
                {
                    MessageBox.Show("Not Mapped!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected void lstFile_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstFile.SelectedItems.Count > 0)
            {
                var selectedItem = (Item)lstFile.SelectedItems[0].Tag;
                switch (selectedItem.ItemType)
                {
                    case ItemType.Folder:
                        // folder
                        treeSupport.SelectNode(selectedItem);
                        break;
                    case ItemType.File:
                        //file
                        //open file
                        var path = tfs.getLocalItem(selectedItem.ServerItem);
                        Utils.Utils.StartFile(path);
                        break;

                    default:
                        break;
                }
            }

        }
        #endregion

        #region Treeview

        protected void trvServerFolder_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
                var currentNode = (FolderTreeNode)(e.Node);
                ServerItem = currentNode.Item;
                var path = ServerItem.ServerItem;
                var rs = tfs.getFolders(path).Items;
                var DetailItem = tfs.getItems(path);
                trvServerFolder.SuspendLayout();
                //var folders = rs.Cast<Item>().ToList<>().OrderBy(o => o.ServerItem).ToList<Item>;
                List<FolderTreeNode> folders = rs.Skip(1).Select(array => new FolderTreeNode(array)).ToList().OrderBy(o => o.Item.ServerItem)
                                                .ToList<FolderTreeNode>();
                if (folders.Count > 0)
                {
                    // UPDATE TREE
                    treeSupport.addNode(ref currentNode, folders, BuildMode.AddNew);
                    e.Node.Expand();
                    // UPDATE LIST VIEW
                    lvSupport.Load(DetailItem, tfs.GetLastedCommiter);
                }
                else
                {
                    // CHANGE COLOR
                    e.Node.ForeColor = Color.Blue;
                    lvSupport.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                initLblLocalPath(ServerItem.ServerItem);
                trvServerFolder.ResumeLayout();
                this.Cursor = Cursors.Default;
            }
        }
        #endregion

        protected void timer1_Tick(object sender, EventArgs e)
        {
            //   ttProgress.PerformStep();
        }


        #endregion

        private void cbxWorkspace_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var item = (tfsWorkspaceItem)cbxWorkspace.SelectedItem;
                tfs.WorkSpace = item.WorkSpace;
                var server = (ServerItem == null) ? "" : ServerItem.ServerItem;
                initLblLocalPath(server);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            try
            {
                var oldNode = trvServerFolder.SelectedNode;
                trvServerFolder.SelectedNode = null;

                trvServerFolder.SelectedNode = oldNode;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnWorkspace_Click(object sender, EventArgs e)
        {
            try
            {
                var frmWs = new frmWorkspace(tfs);
                frmWs.ShowDialog();
                AddWorkSapces();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void menuGetLastest_Click(object sender, EventArgs e)
        {
            try
            {
                var parent = ((ContextMenuStrip)(((ToolStripMenuItem)sender).Owner)).SourceControl;
                
                if (parent == trvServerFolder){
                    btnGetFolder_Click(sender, e);
                }
                else if (parent == lstFile)
                {
                    btnGetFile_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void menuContext_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
             try
            {
                var parent = ((ContextMenuStrip)(sender)).SourceControl;

                if (parent == lstFile)
                {
                    if (lstFile.SelectedItems.Count == 0)
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void trvServerFolder_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    trvServerFolder.SelectedNode = e.Node;
                    menuContext.Show(trvServerFolder,e.Location);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
            
    }
}
