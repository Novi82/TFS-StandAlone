using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFS_SA.Utils
{
    class Utils
    {
        static public void StartFile(String _path)
        {
            var path = Path.Combine(_path);
                if (File.Exists(path))
                {
                    Process.Start(path);
                }
        }
    }
}
