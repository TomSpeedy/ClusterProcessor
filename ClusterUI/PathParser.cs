using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterUI
{
    class PathParser
    {
        public static string GetPrefixPath(string iniPath)
        {
            if (!iniPath.Contains('/') && !iniPath.Contains('\\'))
                return "";
            int lastIndex = 0;
            for (int i = 0; i < iniPath.Length; i++)
            {
                if (iniPath[i] == '/' || iniPath[i] == '\\')
                    lastIndex = i;
            }
            return iniPath.Substring(0, lastIndex + 1);
        }
    }
}
