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
        Disable,
        Wait
    }
    public partial class frmMain
    {
        #region Form Controller
        /// <summary>
        /// Init Setting
        /// </summary>
        protected void init()
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
        #endregion
    
        #region Commbobox
        protected void AddWorkSapces()
        {
            Workspace[] wss = tfs.GetWorkspaces();
            var sorted = wss.OrderBy(x => x.Name);
            foreach (Workspace ws in sorted)
            {
                tfsWorkspaceItem item = new tfsWorkspaceItem(ws.Name, ws);
                cbxWorkspace.Items.Add(item);
            }
        }
        #endregion

        #region Item Controller
        protected void ChangeInputArea(InputMode _Mode)
        {
            switch (_Mode)
            {
                case InputMode.Enable:
                    pnlContent.Enabled = true;
                    pnlMenu.Enabled = true;
                    this.Cursor = Cursors.Default;
                    break;
                case InputMode.Disable:
                    pnlContent.Enabled = false;
                    pnlMenu.Enabled = false;
                    this.Cursor = Cursors.Default;
                    break;
                case InputMode.Wait:
                    pnlContent.Enabled = true;
                    pnlMenu.Enabled = true;
                    this.Cursor = Cursors.WaitCursor;
                    break;
                default:
                    break;
            }
        }
        protected void setMapLocal()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lblLocalPath.Text = fbd.SelectedPath;
                var local = Path.Combine(lblLocalPath.Text);
                var server = Path.Combine(ServerItem.ServerItem);
                tfs.SetMapPath(local, server);
            }
        }
        protected void initLblLocalPath(String _Server)
        {
            if (tfs.IsServerMapped(_Server))
                {
                    var local = tfs.getLocalItem(_Server);
                    lblLocalPath.Text = local;
                }
                else
                {
                    lblLocalPath.Text = @"Map Local Now!";
                }
           
        }
        #endregion

        #region TFS
        protected Boolean loadConnection()
        {
            try
            {
                String fileName = Properties.Settings.Default["ConnectionFile"].ToString();
                //this.tfs.LoadConnection(TfsConnectionSerialize.LoadConnection());
                switch (this.tfs.LoadConnectionFromFile(fileName))
                {
                    case TFS_ConnectResult.Success:
                        return true;
                    case TFS_ConnectResult.Cancel:
                    case TFS_ConnectResult.Failure:
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
        protected void GetItemSet(String _Path)
        {
            var folders = tfs.getFolders(_Path);
        }

        #endregion

        #region Directory Manager
        protected void DownLoadFile(String fileName, String _Server)
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
                String localPath = tfs.getLocalItem(_Server);
                String Msg = String.Empty;
                tfs.dowloadFile(_Server, localPath, ref Msg);
            });
            // what to do when worker completes its task (notify the user)
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate(object o, RunWorkerCompletedEventArgs args)
            {
                this.ttrMessage.Text = String.Format(@"Downloaded: {0}", fileName);
            });
            bw.RunWorkerAsync();
        }
        protected void DownLoadFolder(String _Local, String _Server)
        {
            if (!Directory.Exists(_Local)){
                Directory.CreateDirectory(_Local);
            }

            var folders = Directory.GetDirectories(localPath);
            foreach (var folder in folders)
            {
                DownLoadFolder(folder, tfs.getServerItem(folder));
            }

            var files = Directory.GetFiles(_Local);
            foreach(var file in files)
            {
                DownLoadFile(file, tfs.getServerItem(_Local));
            }


        }
        
        #endregion

        #region Progress bar controller
        /// <summary>
        /// Begin start Progress
        /// </summary>
        protected void StartProgress()
        {
            ttProgress.Style = ProgressBarStyle.Marquee;
            ttProgress.MarqueeAnimationSpeed = 50;
        }

        /// <summary>
        /// Stop Progress
        /// </summary>
        protected void StopProgress()
        {
            ttProgress.MarqueeAnimationSpeed = 0;
            ttProgress.Style = ProgressBarStyle.Blocks;
            ttProgress.Value = ttProgress.Maximum;
        }
        #endregion

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    // 
        //    // frmMain
        //    // 
        //    this.ClientSize = new System.Drawing.Size(410, 303);
        //    this.Name = "frmMain";
        //    this.ResumeLayout(false);

        //}

    }
}