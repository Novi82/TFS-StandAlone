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
    /// <summary>
    /// Manage TFS
    /// </summary>
    public class TfsWrapper
    {
        #region PROPERTY

        private TfsTeamProjectCollection _Tfs;
        public TfsTeamProjectCollection Tfs
        {
            get { return _Tfs; }
            set { _Tfs = value; }
        }

        private VersionControlServer _VCS;
        public VersionControlServer VCS
        {
            get { return _VCS; }
            set { _VCS = value; }
        }

        private Workspace _WorkSpace;
        public Workspace WorkSpace
        {
            get { return _WorkSpace; }
            set { _WorkSpace = value; }
        }

        public Boolean IsConnected;

        #endregion

        #region PRE_CONNECT
        ///// <summary>
        ///// Load Connection 
        ///// </summary>
        ///// <param name="_tfsConnection">TFS Connection</param>
        //public void LoadConnection(TfsConnection _tfsConnection)
        //{
        //    this.tfsConnection = _tfsConnection;
        //}

        /// <summary>
        ///  Connect to TFS
        /// </summary>
        public TFS_ConnectResult Connect()
        {
            //todo del var notes = new StringBuilder();
            // Connect to TFS Using Team Project Picker, the  user is allowed to select only one project
            using (TeamProjectPicker tfsPp = new TeamProjectPicker(TeamProjectPickerMode.MultiProject, false))
            {
                var rs = tfsPp.ShowDialog();
                switch (rs)
                {
                    case System.Windows.Forms.DialogResult.OK:

                        // The TFS project collection
                        try
                        {
                            this.Tfs = tfsPp.SelectedTeamProjectCollection;
                            this.Tfs.EnsureAuthenticated();
                        }
                        catch (NullReferenceException)
                        {
                            throw new NullReferenceException("No Team Project have been selected!");
                        }

                        if (tfsPp.SelectedProjects.Any())
                        {
                            //  The selected Team Project
                            var fileName = Properties.Settings.Default["ConnectionFile"].ToString();
                            this.SaveConnection(_Tfs.Uri, fileName);
                            this.VCS = this.Tfs.GetService<VersionControlServer>();
                            this.WorkSpace = this.GetkWorkSpace(Environment.MachineName);
                            this.CheckWorkSpacePermission(WorkSpace);
                            IsConnected = true;
                        }
                        return TFS_ConnectResult.Success;
                    case System.Windows.Forms.DialogResult.Cancel:
                        return TFS_ConnectResult.Cancel;
                    default:
                        return TFS_ConnectResult.Failure;
                }
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
            IsConnected = true;
        }

        /// <summary>
        /// Save Uri to file
        /// </summary>
        /// <param name="_Connection"></param>
        /// <returns></returns>
        public Boolean SaveConnection(Uri _Connection, String _fileName)
        {
            try
            {
                var uri = _Connection.ToString();
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
        public TFS_ConnectResult LoadConnectionFromFile(String _fileName)
        {
            try
            {
                var uri = Properties.Settings.Default["TfsServerUri"].ToString();
                this.Tfs = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(uri));
                this.Tfs.EnsureAuthenticated();
                this.VCS = this.Tfs.GetService<VersionControlServer>();
                this.WorkSpace = this.GetkWorkSpace(Environment.MachineName);
                this.CheckWorkSpacePermission(WorkSpace);
                //IsConnected = true;
                return TFS_ConnectResult.Success;
            }
            catch (WorkspaceExistsException)
            {
                return TFS_ConnectResult.Failure;
            }
            catch (Exception ex)
            {
                return TFS_ConnectResult.Failure;
            }
        }

        /// <summary>
        /// get lasted User have changed file.
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        public String GetLastedCommiter(String _path)
        {
            IEnumerable results = this.VCS.QueryHistory(_path,
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
            return this.VCS.GetItem("$/", VersionSpec.Latest, DeletedState.NonDeleted, false);
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
                return this.VCS.GetItems(_path,
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
                return this.VCS.GetItems(_FolderPath,
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

       
        //public String LoadMapPath()
        //{
        //    try
        //    {
        //        var localMap = Properties.Settings.Default["TfsLocalMap"].ToString();
        //        LocalPath = localMap;
        //        return localMap;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public Boolean SaveMapPath(TfsConnection _Connection)
        //{
        //    try
        //    {
        //        var localMap = _Connection.LocalPath.ToString();
        //        Properties.Settings.Default["TfsLocalMap"] = localMap;
        //        Properties.Settings.Default.Save();
        //        return true;

        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        public void SetMapPath(String _Local, String _Server)
        {
            //LocalPath = _Path;
            WorkSpace.Map(_Server, _Local);
        }
        /// <summary>
        /// get server Path
        /// </summary>
        /// <param name="_localPath">Local Path</param>
        /// <returns>Server Path</returns>
        public String getServerItem(String _localPath)
        {
            return WorkSpace.GetServerItemForLocalItem(_localPath);
        }
        /// <summary>
        /// get Local Path
        /// </summary>
        /// <param name="_serverPath">Server Path</param>
        /// <returns>Local Path</returns>
        public String getLocalItem(String _serverPath)
        {
            return WorkSpace.GetLocalItemForServerItem(_serverPath);
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
                WorkSpace.GetServerItemForLocalItem(_localPath);
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
                WorkSpace.GetLocalItemForServerItem(_serverPath);
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
        public Result dowloadFile(String _serverFile, String _localFile, ref String _msg)
        {
            try
            {
                VCS.DownloadFile(_serverFile, _localFile);
                _msg = String.Empty;
                return Result.Success;
            }
            catch (Exception)
            {
                _msg = @"Can't Download File";
                return Result.Failure;
            }
        }

        ///// <summary>
        ///// Download a File
        ///// </summary>
        ///// <param name="_serverFile">Server Path</param>
        ///// <param name="_localFile">Local Path</param>
        ///// <param name="_msg">Message Return</param>
        ///// <returns></returns>
        //public Result dowloadFile(String _serverFolder, String _localFolder, ref String _msg)
        //{
        //    //// TODO
        //    //throw new NotImplementedException();
        //    try
        //    {
        //        VCS.DownloadFile(_serverFolder, _localFolder);
        //        _msg = String.Empty;
        //        return Result.Success;
        //    }
        //    catch (Exception)
        //    {
        //        _msg = @"Can't Download File";
        //        return Result.Failure;
        //    }
        //}

        ///// <summary>
        ///// Download Folder
        ///// </summary>
        ///// <param name="_serverFolder">Server Folder Path</param>
        ///// <param name="_localFolder">Local Folder Path</param>
        ///// <param name="_msg">Message Return</param>
        ///// <returns></returns>
        //public Result dowloadFolder(String _serverFolder, String _localFolder, ref String _msg)
        //{
        //    //// TODO
        //    //throw new NotImplementedException();
        //    try
        //    {
        //        VCS.DownloadFile(_serverFolder, _localFolder);
        //        _msg = String.Empty;
        //        return Result.Success;
        //    }
        //    catch (Exception)
        //    {
        //        _msg = @"Can't Download Folder";
        //        return Result.Failure;
        //    }
        //}

        public Result dowload(String _serverFolder)
        {
            try
            {
               var status = this.WorkSpace.Get(new GetRequest(_serverFolder, RecursionType.Full, VersionSpec.Latest), GetOptions.GetAll | GetOptions.Overwrite);
               return Result.Success;
            }
            catch (Exception)
            {
                return Result.Failure;
            }
        }

        #endregion

        #region Workspace
        public Workspace[] GetWorkspaces()
        {
            return VCS.QueryWorkspaces(null, VCS.AuthenticatedUser, System.Net.Dns.GetHostName().ToString());
                //return VCS.QueryWorkspaces(null, "", System.Net.Dns.GetHostName().ToString());
                //You also can get remote workspaces
                //QueryWorkspaces(worspaceName,worksapceOwner,computer)
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_wsName"></param>
        /// <returns></returns>
        public Workspace GetkWorkSpace(String _wsName)
        {
            var rs = VCS.QueryWorkspaces(string.IsNullOrEmpty(_wsName) ? null : _wsName,
                                        null,
                                        null);
            if (rs.Length > 0)
            {
                return rs[0];
            }
            else
            {
                return this.VCS.CreateWorkspace(Environment.MachineName);
            }
        }
        #endregion

    }
}
