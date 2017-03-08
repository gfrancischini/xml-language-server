using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XmlCompletionEngine.Utility
{
    public class PathUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ResolveFilePath(string filePath)
        {

            if (filePath.StartsWith(@"file://"))
            {
                // Client sent the path in URI format, extract the local path and trim
                // any extraneous slashes
                Uri fileUri = new Uri(filePath);
                filePath = fileUri.LocalPath.TrimStart('/');
            }

            // Some clients send paths with UNIX-style slashes, replace those if necessary
            filePath = filePath.Replace('/', '\\');

            // Clients could specify paths with escaped space, [ and ] characters which .NET APIs
            // will not handle.  These paths will get appropriately escaped just before being passed
            // into the SqlTools engine.
            filePath = UnescapePath(filePath);

            // Get the absolute file path
            filePath = Path.GetFullPath(filePath);



            return filePath;
        }

        /// <summary>
        /// Unescapes any escaped [, ] or space characters. Typically use this before calling a
        /// .NET API that doesn't understand PowerShell escaped chars.
        /// </summary>
        /// <param name="path">The path to unescape.</param>
        /// <returns>The path with the ` character before [, ] and spaces removed.</returns>
        public static string UnescapePath(string path)
        {
            if (!path.Contains("`"))
            {
                return path;
            }

            return Regex.Replace(path, @"`(?=[ \[\]])", "");
        }
    }
}
