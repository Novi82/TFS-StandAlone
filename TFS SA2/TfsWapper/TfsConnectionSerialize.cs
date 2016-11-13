//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Serialization;

//namespace TFS_SA.TfsWapper
//{
//    class TfsConnectionSerialize
//    {
//        public static Boolean SaveConnection(TfsConnection _Connection)
//        {
//            try
//            {
//                //String fileName = Properties.Settings.Default["ConnectionFile"].ToString();

//                //using (Stream stream = File.Open(fileName, false ? FileMode.Append : FileMode.Create))
//                //{
//                //    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
//                //    binaryFormatter.Serialize(stream, _Connection);
//                //}
//                Properties.Settings.Default["TfsServerUri"] = _Connection.Tfs.Uri;
//                return true;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }

//        }
//        public static TfsConnection LoadConnection()
//        {
//            String fileName = Properties.Settings.Default["ConnectionFile"].ToString();
//            using (Stream stream = File.Open(fileName, FileMode.Open))
//            {
//                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
//                return (TfsConnection)binaryFormatter.Deserialize(stream);
//            }
//        }


       

//    }
//}
