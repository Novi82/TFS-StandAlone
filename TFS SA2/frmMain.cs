using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TFS_SA.TfsWapper;
using TFS_SA.Gui_Support;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Diagnostics;

namespace TFS_SA.GUI
{

    public partial class frmMain : Form
    {
        #region Variable

        //private String MappedPath = @"D:/test";
        protected TfsWrapper tfs;
        protected String localPath;
        protected String currentPath;
        protected String ServerRootDir;
        protected FolderTreeSupport treeSupport;
        protected ListViewSupport lvSupport;
        protected Boolean isConnected;
        protected Item ServerItem;
        protected String LocalItem;

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
            this.ChangeInputArea(InputMode.Disable);
            this.ttrMessage.Text = @"Loading Setting...";
            this.StartProgress();
            //this.isConnected = this.loadConnection();
            //this.AddWorkSapces();
            //var bw = new BackgroundWorker();
            //// this allows our worker to report progress during work
            //bw.WorkerReportsProgress = true;

            //// what to do in the background thread
            //bw.DoWork += new DoWorkEventHandler(
            //delegate(object o, DoWorkEventArgs args)
            //{
            //    BackgroundWorker b = o as BackgroundWorker;
            //});
            //// what to do when worker completes its task (notify the user)
            //bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            //delegate(object o, RunWorkerCompletedEventArgs args)
            //{
            //    if (!isConnected)
            //    {
            //        var rs = tfs.Connect();
            //        switch (rs)
            //        {
            //            case TFS_ConnectResult.Success:
            //                this.isConnected = tfs.IsConnected;
            //                break;
            //            case TFS_ConnectResult.Cancel:
            //                Environment.Exit(0);
            //                break;
            //        }
            //    }
            //    var root = this.tfs.GetRoot();
            //    treeSupport.AddRootNode(root);
            //    this.StopProgress();
            //    this.ttrMessage.Text = @"Load Completed"; ;
            //    this.ChangeInputArea(InputMode.Enable);
            //});

            //bw.RunWorkerAsync();
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
            if (cbxWorkspace.Items.Count > 0) {
                cbxWorkspace.SelectedIndex = 0;
            }
           
            var root = this.tfs.GetRoot();
            treeSupport.AddRootNode(root);
            this.StopProgress();
            this.ttrMessage.Text = @"Load Completed"; ;
            this.ChangeInputArea(InputMode.Enable);
        }

        #endregion

        #region Button

        protected void btnLocalPath_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    FolderBrowserDialog fbd = new FolderBrowserDialog();
            //    if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    {
            //        txtServerPath.Text = fbd.SelectedPath;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
        protected void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                //String Target;
                //String Source = trvServerFolder.SelectedNode.Name;
                //FolderBrowserDialog fbd = new FolderBrowserDialog();
                //if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //    Target = fbd.SelectedPath;
                //    tfs.getFolder(Source, Target, workSpace);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
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
            //if (!tfs.IsMapped)
            //{
            this.setMapLocal();
            //}
            //else
            {
                var path = Path.Combine(lblLocalPath.Text);
                if (Directory.Exists(path))
                {
                    Process.Start(path);
                }

            }
        }
        protected void btnGetFolder_Click(object sender, EventArgs e)
        {

        }

        protected void btnGetFile_Click(object sender, EventArgs e)
        {
            try
            {
                String fileName = String.Empty;
                var serverItem = lvSupport.getSelectedItem(ref fileName);
                DownLoadFile(fileName, serverItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.ttrMessage.Text = String.Empty;
            }
            finally
            {
                this.StopProgress();
            }
        }
        #endregion

        #region listview

        protected void lstFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //var msg = tfs.tfsConnection.WorkSpace.MappingsAvailable.ToString();
                //MessageBox.Show(msg);
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
            this.Cursor = Cursors.WaitCursor;
            var currentNode = (FolderTreeNode)(e.Node);
            ServerItem = currentNode.Item;
            var path = ServerItem.ServerItem;
            var rs = tfs.getFolders(path).Items;
            var DetailItem = tfs.getItems(path);
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
            this.Cursor = Cursors.Default;
        }
        #endregion

        protected void timer1_Tick(object sender, EventArgs e)
        {
            //   ttProgress.PerformStep();
        }


        #endregion

    }
}
