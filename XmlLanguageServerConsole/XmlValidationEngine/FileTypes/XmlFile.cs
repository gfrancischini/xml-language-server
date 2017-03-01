using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XmlValidation.FileTypes
{
    class XmlFile
    {
        public String FilePath
        {
            get;
            private set;
        }
        public String Content
        {
            get;
            private set;
        }
        public String Uri
        {
            get;
            private set;
        }
        public String XsdFilePath
        {
            get;
            private set;
        }
        public Boolean HasDeclaredXsd
        {
            get;
            private set;
        }

        public XmlFile(string filePath, string content)
        {
            this.FilePath = filePath;
            this.Content = content; // File.ReadAllText(this.FilePath);
            this.XsdFilePath = this.GetNoNamespaceSchemaFromXml();
        }

        private string GetNoNamespaceSchemaFromXml()
        {
            string xsdFilePath = GetXsdFileNameFromXml();
            if (xsdFilePath != null)
            {
                xsdFilePath = Path.Combine(Path.GetDirectoryName(this.FilePath), xsdFilePath);
                xsdFilePath = Path.GetFullPath(xsdFilePath);
                this.HasDeclaredXsd = true;
            }
            return xsdFilePath;
        }

        private string GetXsdFileNameFromXml()
        {
            var match = Regex.Match(this.Content, @"xsi:noNamespaceSchemaLocation=""([^""]*)""", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        public string GetXsdFileNameFromXmlTemplate()
        {
            var match = Regex.Match(this.Content, @"xsi:noNamespaceSchemaLocation=""\$xsdpath\$(.*)""", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }
    }
}
