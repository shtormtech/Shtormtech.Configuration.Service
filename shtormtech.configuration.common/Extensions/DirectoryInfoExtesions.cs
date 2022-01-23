using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shtormtech.configuration.common.Extensions
{
    public static class DirectoryInfoExtesions
    {   
        public static void RecursiveDelete(this DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.EnumerateDirectories())
            {
                dir.RecursiveDelete();
            }
            baseDir.Delete(true);
        }

    }
}
