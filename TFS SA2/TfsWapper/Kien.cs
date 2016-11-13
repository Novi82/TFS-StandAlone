//using Microsoft.TeamFoundation.Client;
//using Microsoft.TeamFoundation.VersionControl.Client;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace TFS_SA.TfsWapper
//{
//    public static class TfsHelper
//    {
//        static bool baseIsConnected = false;
//        static string baseLogPath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "TFS_LOG");
//        static StreamWriter baseLogWriter = null;
//        static TfsTeamProjectCollection baseTfsProjectCollection;
//        static VersionControlServer baseVersionControlServer;
//        static Workspace baseWorkspace;
//        static String baseTfsUrl = "https://tfs-dev3.its-process.net:8081/tfs/DefaultCollection";
//        public static String baseWorkspacePath = string.Empty;
//        public static String baseNewPendingChangeLog = string.Empty;

//        public static bool ConnectToTfs()
//        {
//            try
//            {
//                if (baseIsConnected)
//                {
//                    return true;
//                }
//                //
//                if (!Directory.Exists(baseLogPath))
//                {
//                    Directory.CreateDirectory(baseLogPath);
//                }
//                //
//                baseTfsProjectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(baseTfsUrl));
//                baseTfsProjectCollection.EnsureAuthenticated();

//                baseVersionControlServer = baseTfsProjectCollection.GetService<VersionControlServer>();

//                baseWorkspace = baseVersionControlServer.GetWorkspace(baseWorkspacePath);
//                if (baseWorkspace != null)
//                {
//                    if (!baseWorkspace.HasWorkspacePermission(WorkspacePermissions.CheckIn))
//                    {
//                        MessageBox.Show("Error Check_WorkspacePermission [ CheckIn ] not availabel .!");
//                        return false;
//                    }
//                }
//                baseIsConnected = true;
//                //
//                return true;
//            }
//            catch (Exception ex)
//            {
//                ModernDialog.ShowMessage(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButton.OK);
//                return false;
//            }
//        }
//        public static bool PendingChangeToProject(string sPath, bool isRecursive = true)
//        {
//            if (!PendingAdd(sPath, isRecursive))
//            {
//                return PendingEdit(sPath, isRecursive);
//            }
//            //
//            return true;
//        }
//        public static bool PendingAdd(string sPath, bool isRecursive = true)
//        {
//            try
//            {
//                if (baseWorkspace != null)
//                {
//                    int penCount = baseWorkspace.PendAdd(sPath, isRecursive);
//                    string msg = "PendingAdd ( " + penCount + " ) :" + sPath;
//                    if (penCount > 0)
//                    {
//                        AddLog(msg);
//                    }

//                    return penCount > 0;
//                }
//                //
//                return false;
//            }
//            catch (Exception ex)
//            {
//                ModernDialog.ShowMessage(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButton.OK);
//                return false;
//            }
//        }

//        public static bool PendingEdit(string sPath, bool isRecursive = true)
//        {
//            try
//            {
//                if (baseWorkspace != null)
//                {
//                    int penCount = baseWorkspace.PendEdit(sPath, isRecursive ? RecursionType.Full : RecursionType.None);
//                    string msg = "PendingEdit ( " + penCount + " ) :" + sPath;
//                    if (penCount > 0)
//                    {
//                        AddLog(msg);
//                    }
//                    return penCount > 0;
//                }
//                //
//                return false;
//            }
//            catch (Exception ex)
//            {
//                ModernDialog.ShowMessage(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, MessageBoxButton.OK);
//                return false;
//            }
//        }

//        private static void AddLog(string sLog)
//        {
//            if (baseLogWriter != null)
//            {
//                baseLogWriter.WriteLine(sLog);
//            }
//        }

//        public static void StartLog(string sKey)
//        {
//            string sFile = Path.Combine(baseLogPath, sKey + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");

//            StopLog();
//            baseLogWriter = new StreamWriter(sFile, false, Encoding.UTF8);
//        }
//        //public static void StartLog(ProgramInfo.ProgramSettingInfo programInfo)
//        //{
//        //    string sKey = programInfo.ProgramId + "_" + programInfo.プログラム名 + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
//        //    string sFile = Path.Combine(baseLogPath, sKey);

//        //    StopLog();
//        //    baseLogWriter = new StreamWriter(sFile, false, Encoding.UTF8);
//        //}
//        //public static void StopLog()
//        //{
//        //    if (baseLogWriter != null)
//        //    {
//        //        GetPendingChange();
//        //        //
//        //        baseLogWriter.Close();
//        //        baseLogWriter = null;
//        //    }
//        //}
//        public static void GetPendingChange()
//        {
//            PendingChange[] pendingChanges = baseWorkspace.GetPendingChanges();
//            if (pendingChanges != null && pendingChanges.Length > 0)
//            {
//                foreach (PendingChange pendingChange in pendingChanges)
//                {
//                    AddLog("Path: " + pendingChange.LocalItem + ", PendingChange: " + PendingChange.GetLocalizedStringForChangeType(pendingChange.ChangeType));
//                }
//            }
//        }
//    }
//}
