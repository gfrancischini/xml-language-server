using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlCompletionEngine.Parser;
using XmlCompletionEngine.Types;

namespace XmlCompletionEngine
{
    public class XmlBaseProvider
    {
        protected bool HasOpenToken
        {
            get;
            set;
        }

        protected Text Text
        {
            get;
            set;
        }

        protected TextPosition Position
        {
            get;
            set;
        }

        /// <summary>
        /// Auto create a new Schema
        /// </summary>
        public XmlBaseProvider(Text text, TextPosition position)
        {
            this.Text = text;
            this.Position = position;
        }


        /// <summary>
        /// Automatically retrieve the xsds referenced on the xml
        /// </summary>
        /// <param name="xmlContent"></param>
        /// <returns></returns>
        protected XmlSchemaCompletionCollection AutoFindSchemaCollection()
        {
            XmlSchemaCompletionCollection schemas = new XmlSchemaCompletionCollection();
            List<String> xsds = XmlParser.GetAllXsdFileNamesFromXml(this.Text.Content);
            foreach (string xsd in xsds)
            {
                Uri xsdUri = new Uri(this.Text.Uri, xsd);

                if (File.Exists(xsdUri.LocalPath) == false)
                {
                    continue;
                }

                //string xsdPath = FileUtility.GetAbsolutePath(text.FileName.GetParentDirectory(), xsd);
                schemas.Add(new XmlSchemaCompletion(xsdUri));
            }
            return schemas;
        }

        protected int FixOffset(Text text, int currentOffset)
        {
            int elementStartIndex = XmlParser.GetActiveElementStartIndex(text.Content, currentOffset);
            HasOpenToken = true;
            if (elementStartIndex <= -1)
            {
                currentOffset--;
                while (currentOffset > 0)
                {
                    elementStartIndex = XmlParser.GetActiveElementStartIndex(text.Content, currentOffset);
                    currentOffset--;
                    if (elementStartIndex > 0)
                    {
                        HasOpenToken = false;
                        break;
                    }
                }
                if (elementStartIndex <= -1)
                {
                    return -1;
                }
            }
            if (XmlParser.ElementStartsWith("<!", elementStartIndex, text))
                return -1;
            if (XmlParser.ElementStartsWith("<?", elementStartIndex, text))
                return -1;

            return currentOffset;
        }

        protected XmlSchemaCompletion GetDefaultSchema(XmlElementPath elementPath, XmlSchemaCompletionCollection schemas)
        {
            XmlSchemaCompletion defaultSchema = schemas[""];
            if (elementPath.Elements.Count > 0)
            {
                defaultSchema = schemas[elementPath.GetRootNamespace()]; //schemaFileAssociations.GetSchemaCompletion(editor.FileName);
            }
            return defaultSchema;
        }

    }
}
