using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Framework.Common;
using System.Configuration;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using Microsoft.TeamFoundation.Client;
namespace TFS_SA.TfsWapper
{
    public enum ConnectResult
    {
        Success,
        Cancel,
        Failure
    }

    public enum Result
    {
        Success,
        Cancel,
        Failure
    }
    /// <summary>
    /// Manage TFS
    /// </summary>
    public class TfsWrapper
    {
        #region PROPERTY
        // protected readonly string MyUri = ConfigurationSettings.AppSettings["TfsUri"];
        public String workspacePath = string.Empty;
        public Boolean IsConnected;
        //public TfsConnection tfsConnection;
        public TfsConnection tfsConnection { get; set; }
        public TfsWrapper()
        {
            this.tfsConnection = new TfsConnection();
        }
        public Boolean IsMapped
        {
            get { return tfsConnection.IsMapped; }
        }
        #endregion

        #region PRE_CONNECT
        /// <summary>
        /// Load Connection 
        /// </summary>
        /// <param name="_tfsConnection">TFS Connection</param>
        public void LoadConnection(TfsConnection _tfsConnection)
        {
            this.tfsConnection = _tfsConnection;
        }

        /// <summary>
        ///  Connect to TFS
        /// </summary>
        public ConnectResult Connect()
        {
            var notes = new StringBuilder();
            // Connect to TFS Using Team Project Picker, the  user is allowed to select only one project
            var tfsPp = new TeamProjectPicker(TeamProjectPickerMode.MultiProject, false);
            var rs = tfsPp.ShowDialog();
            switch (rs)
            {
                case System.Windows.Forms.DialogResult.OK:

                    // The TFS project collection
                    try
                    {
                        this.tfsConnection.Tfs = tfsPp.SelectedTeamProjectCollection;
                        this.tfsConnection.Tfs.EnsureAuthenticated();
                    }
                    catch (NullReferenceException ex)
                    {
                        throw new NullReferenceException("No Team Project have been selected!");
                    }

                    if (tfsPp.SelectedProjects.Any())
                    {
                        //  The selected Team Project
                        this.tfsConnection.TeamProject = tfsPp.SelectedProjects[0];
                        var fileName = Properties.Settings.Default["ConnectionFile"].ToString();
                        this.SaveConnection(tfsConnection, fileName);
                        this.tfsConnection.VersionControlServer = this.tfsConnection.Tfs.GetService<VersionControlServer>();
                        //getWorkSpace(this.LoadMapPath());
                        this.tfsConnection.WorkSpace = this.GetkWorkSpace(Environment.MachineName);
                        this.CheckWorkSpacePermission(tfsConnection.WorkSpace);
                        IsConnected = true;
                    }
                    return ConnectResult.Success;
                case System.Windows.Forms.DialogResult.Cancel:
                    return ConnectResult.Cancel;
                default:
                    return ConnectResult.Failure;
            }
        }

        /// <summary>
        /// checl permitssion of workspace
        /// </summary>
        /// <param name="_workspacePath">Local Path mapped</param>
        public void CheckWorkSpacePermission(Workspace _workspace)
        {

            if (_workspace != null)
            {
                if (!_workspace.HasWorkspacePermission(WorkspacePermissions.Read))
                {
                    IsConnected = false;
                    throw new Exception("Error Check_WorkspacePermission [ CheckIn ] not availabel!");
                }
            }
            else
            {
                IsConnected = false;
                throw new Exception("Error Cannot Get WorkSpace!");
            }
        }
        /// <summary>
        /// TODO - co le se xoa
        /// </summary>
        /// <param name="_WorkSpaceName"></param>
        public void SetWorkSpaces(String _WorkSpaceName)
        {

        }

        /// <summary>
        /// Save Uri to file
        /// </summary>
        /// <param name="_Connection"></param>
        /// <returns></returns>
        public Boolean SaveConnection(TfsConnection _Connection, String _fileName)
        {
            try
            {
                var uri = _Connection.Tfs.Uri.ToString();
                Properties.Settings.Default["TfsServerUri"] = uri;
                Properties.Settings.Default.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// load connection from FIle
        /// </summary>
        /// <returns></returns>
        public ConnectResult LoadConnectionFromFile(String _fileName)
        {
            try
            {
                var uri = Properties.Settings.Default["TfsServerUri"].ToString();
                this.tfsConnection.Tfs = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(uri));
                this.tfsConnection.Tfs.EnsureAuthenticated();
                this.tfsConnection.VersionControlServer = this.tfsConnection.Tfs.GetService<VersionControlServer>();
                this.tfsConnection.WorkSpace = this.GetkWorkSpace(Environment.MachineName);
                this.CheckWorkSpacePermission(tfsConnection.WorkSpace);
                IsConnected = true;
                return ConnectResult.Success;
            }
            catch (WorkspaceExistsException)
            {
                return ConnectResult.Failure;
            }
            catch (Exception ex)
            {
                return ConnectResult.Failure;
            }
        }

        /// <summary>
        /// get lasted User have changed file.
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        public String GetLastedCommiter(String _path)
        {
            IEnumerable results = this.tfsConnection.VersionControlServer.QueryHistory(_path,
                                                    VersionSpec.Latest, 0, RecursionType.Full,
                                                    null, null, null, int.MaxValue, true, true);
            List<Changeset> changesets = results.Cast<Changeset>().ToList();
            Changeset latestChangeset = changesets.ElementAt(0);
            return latestChangeset.Committer;
        }

        #endregion

        #region GET INFOR

        /// <summary>
        /// GetRoot Path
        /// </summary>
        /// <returns></returns>
        public Item GetRoot()
        {
            return this.tfsConnection.VersionControlServer.GetItem("$/", VersionSpec.Latest, DeletedState.NonDeleted, false);
        }

        /// <summary>
        /// get Folder in Path
        /// </summary>
        /// <param name="_path">Server Path</param>
        /// <returns>don't get file . don't get sub Object</returns>
        public ItemSet getFolders(String _path)
        {
            if (IsConnected)
            {
                return this.tfsConnection.VersionControlServer.GetItems(_path,
                                                                        VersionSpec.Latest,
                                                                        RecursionType.OneLevel,
                                                                        DeletedState.NonDeleted,
                                                                        ItemType.Folder,
                                                                        false);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get all object in path
        /// </summary>
        /// <param name="_FolderPath">Server Path</param>
        /// <returns></returns>
        public ItemSet getItems(String _FolderPath)
        {
            if (IsConnected)
            {
                return this.tfsConnection.VersionControlServer.GetItems(_FolderPath,
                                                                        VersionSpec.Latest,
                                                                        RecursionType.OneLevel,
                                                                        DeletedState.NonDeleted,
                                                                        ItemType.Any,
                                                                        false);
            }
            else
            {
                return null;
            }
        }

        public Workspace GetkWorkSpace(String _wsName)
        {
            var rs = tfsConnection.VersionControlServer.QueryWorkspaces(string.IsNullOrEmpty(_wsName) ? null : _wsName,
                                        null,
                                        null);
            if (rs.Length > 0)
            {
                return rs[0];
            }
            else
            {
                return this.tfsConnection.VersionControlServer.CreateWorkspace(Environment.MachineName);
            }
        }
        public String LoadMapPath()
        {
            try
            {
                var localMap = Properties.Settings.Default["TfsLocalMap"].ToString();
                tfsConnection.LocalPath = localMap;
                return localMap;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Boolean SaveMapPath(TfsConnection _Connection)
        {
            try
            {
                var localMap = _Connection.LocalPath.ToString();
                Properties.Settings.Default["TfsLocalMap"] = localMap;
                Properties.Settings.Default.Save();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        public void SetMapPath(String _Path)
        {
            tfsConnection.LocalPath = _Path;
            this.SaveMapPath(tfsConnection);
        }
        /// <summary>
        /// get server Path
        /// </summary>
        /// <param name="_localPath">Local Path</param>
        /// <returns>Server Path</returns>
        public String getServerItem(String _localPath)
        {
            return tfsConnection.WorkSpace.GetServerItemForLocalItem(_localPath);
        }
        /// <summary>
        /// get Local Path
        /// </summary>
        /// <param name="_serverPath">Server Path</param>
        /// <returns>Local Path</returns>
        public String getLocalItem(String _serverPath)
        {
            return tfsConnection.WorkSpace.GetLocalItemForServerItem(_serverPath);
        }

        /// <summary>
        /// Check local path is mapped to server item
        /// </summary>
        /// <param name="_localPath">local path</param>
        /// <returns>true : Mapped
        ///          false: not map</returns>
        public Boolean IsLocalMapped(String _localPath)
        {
            try
            {
                tfsConnection.WorkSpace.GetServerItemForLocalItem(_localPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// check server path is mapped to local 
        /// </summary>
        /// <param name="_serverPath"></param>
        /// <returns></returns>
        public Boolean IsServerMapped(String _serverPath)
        {
            try
            {
                tfsConnection.WorkSpace.GetLocalItemForServerItem(_serverPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region DOWNLOAD
        /// <summary>
        /// Download a File
        /// </summary>
        /// <param name="_serverFile">Server Path</param>
        /// <param name="_localFile">Local Path</param>
        /// <param name="_msg">Message Return</param>
        /// <returns></returns>
        public Result dowloadFile(String _serverFile, String _localFile,ref String _msg)
        {
            try
            {
                tfsConnection.VersionControlServer.DownloadFile(_serverFile, _localFile);
                _msg = String.Empty;
                return Result.Success;
            }
            catch (Exception)
            {
                _msg = @"Can't Download File";
                return Result.Failure;
            }
        }

        /// <summary>
        /// Download a File
        /// </summary>
        /// <param name="_serverFile">Server Path</param>
        /// <param name="_localFile">Local Path</param>
        /// <param name="_msg">Message Return</param>
        /// <returns></returns>
        public Result dowloadFile(Item _serverItem)
        {
            throw new NotImplementedException();
           if(_serverItem != null)
           {
               
           }
        }

        /// <summary>
        /// Download Folder
        /// </summary>
        /// <param name="_serverFolder">Server Folder Path</param>
        /// <param name="_localFolder">Local Folder Path</param>
        /// <param name="_msg">Message Return</param>
        /// <returns></returns>
        public Result dowloadFolder(String _serverFolder, String _localFolder, ref String _msg)
        {
            // TODO
            throw new NotImplementedException();

            try
            {
                tfsConnection.VersionControlServer.DownloadFile(_serverFolder, _localFolder);
                _msg = String.Empty;
                return Result.Success;
            }
            catch (Exception)
            {
                _msg = @"Can't Download File";
                return Result.Failure;
            }
        }

        #endregion

    }
}
