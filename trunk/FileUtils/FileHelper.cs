
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
namespace FileUtils
{
    public class FileHelper
    {
        public FileInfo[] GetFiles(string path, string extenstion)
        {
            List<FileInfo> result = new List<FileInfo>();

            string extensionMask = String.Empty;

            Match m = extensionMaskExpr.Match(extenstion);
            if (!m.Success) // we don't use *.ext notation for extension parameter
            {
                //check against "ext" notation without "*."
                m = defaultExtensionMaskExpr.Match(extenstion);

                if (!m.Success)
                    return new FileInfo[0];
                else
                {
                    extensionMask = "*." + extenstion;
                }
            }
            else
            {
                extensionMask = extenstion;
            }

            try
            {
                if ( !Directory.Exists(path))
                    return new FileInfo[0];

                DirectoryInfo root = new DirectoryInfo(path);

                Stack<DirectoryInfo> childDirectories = new Stack<DirectoryInfo>(root.GetDirectories());

                result.AddRange(root.GetFiles(extensionMask));

                while (childDirectories.Count > 0)
                {
                    DirectoryInfo top = childDirectories.Pop();

                    DirectoryInfo[] topSubDirs = top.GetDirectories();
                    foreach (DirectoryInfo di in topSubDirs)
                    {
                        childDirectories.Push(di);
                    }

                    result.AddRange(top.GetFiles(extensionMask));
                }
            }
            catch { }

            return result.ToArray();
        }


        private static Regex extensionMaskExpr = new Regex(@"^(\*\.)(\w+)$");
        private static Regex defaultExtensionMaskExpr = new Regex(@"^([a-z]+)$");
    }
}
