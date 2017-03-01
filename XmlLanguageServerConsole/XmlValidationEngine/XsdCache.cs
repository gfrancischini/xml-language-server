using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XmlValidation.FileTypes;

namespace XmlValidation
{
    public class XsdCache
    {
        /// <summary>
        /// Cache that Holds all the loaded XSDs
        /// </summary>
        private Dictionary<string, XsdFile> xsds = new Dictionary<string, XsdFile>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public XsdFile Load(string path)
        {
            // load the new xsd
            if (File.Exists(path) == false)
            {
                //TODO : create a specific extesion
                throw new Exception("XSD File doest not exist");
            }

            XsdFile xsdFileInfo = new XsdFile()
            {
                Name = Path.GetFileName(path),
                Path = path
            };

            this.xsds.Add(path, xsdFileInfo);

            return xsdFileInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public XsdFile Get(string path)
        {
            //check if the file is loaded
            if (xsds.ContainsKey(path))
            {
                //return from cache
                return xsds[path];
            }

            return this.Load(path);
        }
    }
}
