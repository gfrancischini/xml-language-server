using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RxpInterface.Utility
{
    public class ResourceUtils
    {
        /// <summary>
        /// Extracts [resource] into the the file specified by [path]
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="path"></param>
        public static void ExtractResource(string resource, string path)
        {
            //Assembly assembly = Assembly.GetExecutingAssembly();
            var assembly = typeof(RxpInterface.Utility.ResourceUtils).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(resource);
            byte[] bytes = new byte[(int)stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            File.WriteAllBytes(path, bytes);
        }
    }
}
