using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace XmlValidation
{
    public static class XmlHelper
    {




        /*public static XDocument CreateXDocument(XMLFile xmlFile)
        {
            XDocument document = null;
            if (File.Exists(xmlFile.Path) == false)
            {
                // throw file not exist
                return null;
            }

            //using (Stream stream = GenerateStreamFromString(xmlFile.Content))
            //{
            //document = XDocument.Load(stream, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo | LoadOptions.SetBaseUri);
            //}

            document = XDocument.Parse(xmlFile.Content, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo | LoadOptions.SetBaseUri);
            return document;
        }*/

        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }





        public static void ValidateCode(XDocument doc)
        {
            //validate if code is equal to file name
            {
                if (doc.Root != null)
                {
                    var codeAttr = doc.Root.Attribute(XName.Get("code", ""));
                    if (codeAttr != null)
                    {
                        //if (codeAttr.Value != Path.GetFileNameWithoutExtension(fileInfo.Name))
                        //{
                        //fileInfo.Errors.Add(new XMLError(Messages.DeployErrorXmlCodeDifferentThanFileName(fileInfo.Name), fileInfo.RefPath, 0, 0));
                        //   return false;
                        //}
                    }
                }
            }
        }


        public static void ValidateDuplicatedValue(List<string> errorAccumulator, XmlElement xmlNode, List<string> valueAccumulator, string elementName, string attrName)
        {
            if (xmlNode.HasAttribute(attrName))
            {
                var codeVal = xmlNode.Attributes[attrName].Value;
                if (valueAccumulator.Contains(codeVal))
                {
                    //errorAccumulator.Add(Messages.XmlAttributeDuplicated(elementName, codeVal));
                }
                else
                {
                    valueAccumulator.Add(codeVal);
                }
            }
        }

        public static void ValidateAttributeLength(List<string> errorAccumulator, XmlElement xmlNode, string attrName, int maxLength)
        {
            if (xmlNode.HasAttribute(attrName))
            {
                if (xmlNode.Attributes[attrName].Value.Length > maxLength)
                {
                    //errorAccumulator.Add(Messages.XmlAttributeLengthExceeded(xmlNode.Attributes[attrName].Value, attrName, maxLength));
                }
            }
        }

        public static void ValidateAttributeRange(List<string> errorAccumulator, XmlElement xmlNode, string attrName, int minValue, int maxValue)
        {
            if (xmlNode.HasAttribute(attrName))
            {
                int intval;
                try
                {
                    intval = Int32.Parse(xmlNode.Attributes[attrName].Value);
                }
                catch
                {
                    //errorAccumulator.Add(Messages.XmlAttributeMustBeInteger(attrName));
                    return;
                }

                if (intval < minValue || intval > maxValue)
                {
                    // errorAccumulator.Add(Messages.XmlAttributeRangeOutside(xmlNode.Attributes[attrName].Value, attrName, minValue, maxValue));
                }
            }
        }

        public static string GetNameOfFirstTag(string fullPath)
        {
            using (var reader = XmlReader.Create(fullPath))
            {
                var readResult = false;
                do
                {
                    try
                    {
                        readResult = reader.Read();
                    }
                    catch (XmlException)
                    {
                        //Put here so the name of the first tag will be read
                        //even if the file is mal-formed
                    }

                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        return reader.Name;
                    }
                } while (readResult);
            }

            return null;
        }
    }
}
