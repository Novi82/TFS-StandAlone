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
    public enum InputMode
    {
        Enable,
        Disable
    }
    public partial class frmMain : Form
    {
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

        public frmMain()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            try
            {
                tfs = new TfsWrapper();
                treeSupport = new FolderTreeSupport(this.trvServerFolder);
                lvSupport = new ListViewSupport(this.lstFile);
                //tfs = TfsCommand.getIntance;
                //profile = Profile.Load(Global.PROFILE_PATH);
                if (tfs.IsConnected)
                {
                    ServerItem = this.tfs.GetRoot();
                    this.ServerRootDir = ServerItem.ServerItem;
                    if (this.tfs.IsLocalMapped(ServerRootDir))
                    {
                        this.localPath = this.tfs.getLocalItem(ServerRootDir);
                    }
                    else
                    {
                        this.localPath = String.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }

        private Boolean loadConnection()
        {
            try
            {
                String fileName = Properties.Settings.Default["ConnectionFile"].ToString();
                //this.tfs.LoadConnection(TfsConnectionSerialize.LoadConnection());
                switch (this.tfs.LoadConnectionFromFile(fileName))
                {
                    case ConnectResult.Success:
                        return true;
                    case ConnectResult.Cancel:
                    case ConnectResult.Failure:
                        return false;
                }
                return false;
                //return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void btnLocalPath_Click(object sender, EventArgs e)
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

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void btnGet_Click(object sender, EventArgs e)
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

        private void GetItemSet(String _Path)
        {
            var folders = tfs.getFolders(_Path);
        }

        private void btnConnect_Click(object sender, EventArgs e)
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

        private void frmMain_Shown(object sender, EventArgs e)
        {
            this.ChangeInputArea(InputMode.Disable);
            this.ttrMessage.Text = @"Loading Setting...";
            this.StartProgress();
            var bw = new BackgroundWorker();
            // this allows our worker to report progress during work
            bw.WorkerReportsProgress = true;

            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate(object o, DoWorkEventArgs args)
            {
                BackgroundWorker b = o as BackgroundWorker;
                this.isConnected = this.loadConnection();
            });
            // what to do when worker completes its task (notify the user)
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate(object o, RunWorkerCompletedEventArgs args)
            {
                if (!isConnected)
                {
                    var rs = tfs.Connect();
                    switch (rs)
                    {
                        case ConnectResult.Success:
                            this.isConnected = tfs.IsConnected;
                            break;
                        case ConnectResult.Cancel:
                            Environment.Exit(0);
                            break;
                    }
                }
                var root = this.tfs.GetRoot();
                treeSupport.AddRootNode(root);
                this.StopProgress();
                this.ttrMessage.Text = @"Load Completed"; ;
                this.ChangeInputArea(InputMode.Enable);
            });

            bw.RunWorkerAsync();
        }

        public void ChangeInputArea(InputMode _Mode)
        {
            switch (_Mode)
            {
                case InputMode.Enable:
                    pnlContent.Enabled = true;
                    pnlMenu.Enabled = true;
                    break;
                case InputMode.Disable:
                    pnlContent.Enabled = false;
                    pnlMenu.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //   ttProgress.PerformStep();
        }

        private void StartProgress()
        {
            ttProgress.Style = ProgressBarStyle.Marquee;
            ttProgress.MarqueeAnimationSpeed = 50;
        }

        private void StopProgress()
        {
            ttProgress.MarqueeAnimationSpeed = 0;
            ttProgress.Style = ProgressBarStyle.Blocks;
            ttProgress.Value = ttProgress.Maximum;
        }

        private void trvServerFolder_AfterSelect(object sender, TreeViewEventArgs e)
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

        private void lblLocalPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!tfs.IsMapped)
            {
                this.setMapLocal();
            }
            else
            {
                var path = Path.Combine(lblLocalPath.Text);
                if (Directory.Exists(path))
                {
                    Process.Start(path);
                }

            }
        }

        private void setMapLocal()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tfs.SetMapPath(fbd.SelectedPath);
                initLblLocalPath();
            }
        }

        private void initLblLocalPath()
        {
            if (!String.IsNullOrWhiteSpace(LocalItem))
            {
                if (tfs.IsServerMapped(LocalItem))
                {
                    lblLocalPath.Text = tfs.tfsConnection.LocalPath;
                }
                else
                {
                    lblLocalPath.Text = @"Map Local Now!";
                }
            }
            else
            {
                lblLocalPath.Text = @"Map Local Now!";
            }
        }

        private void lstFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //var msg = tfs.tfsConnection.WorkSpace.MappingsAvailable.ToString();
                //MessageBox.Show(msg);
                var workspace = tfs.tfsConnection.WorkSpace;
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

        private void lstFile_MouseDoubleClick(object sender, MouseEventArgs e)
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

        private void btnGetFolder_Click(object sender, EventArgs e)
        {

        }

        private void btnGetFile_Click(object sender, EventArgs e)
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

        private void DownLoadFile(String fileName, Item serverItem)
        {
            this.StartProgress();
            var bw = new BackgroundWorker();
            // this allows our worker to report progress during work
            bw.WorkerReportsProgress = true;

            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate(object o, DoWorkEventArgs args)
            {
                BackgroundWorker b = o as BackgroundWorker;
                this.ttrMessage.Text = String.Format(@"Downloading: {0}", fileName);
                //var stream = serverItem.DownloadFile();
                String serverPath = serverItem.ServerItem;
                String localPath = tfs.getLocalItem(serverPath);
                String Msg = String.Empty;
                tfs.dowloadFile(serverPath,localPath,ref Msg);
            });
            // what to do when worker completes its task (notify the user)
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate(object o, RunWorkerCompletedEventArgs args)
            {
                this.ttrMessage.Text = String.Format(@"Downloaded: {0}", fileName);
            });
            bw.RunWorkerAsync();
        }

    }
}
