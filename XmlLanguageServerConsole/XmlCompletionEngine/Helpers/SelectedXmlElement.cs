using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlCompletionEngine.Parser;

namespace XmlCompletionEngine.Types
{
    public class SelectedXmlElement
    {
        XmlElementPath path = new XmlElementPath();
        string selectedAttribute = String.Empty;
        string selectedAttributeValue = String.Empty;

        public SelectedXmlElement(string xml, int index)
        {
            FindSelectedElement(xml, index);
            FindSelectedAttribute(xml, index);
            FindSelectedAttributeValue(xml, index);
        }

        void FindSelectedElement(string xml, int index)
        {
            path = XmlParser.GetActiveElementStartPathAtIndex(xml, index);
        }

        void FindSelectedAttribute(string xml, int index)
        {
            selectedAttribute = XmlParser.GetAttributeNameAtIndex(xml, index);
        }

        void FindSelectedAttributeValue(string xml, int index)
        {
            selectedAttributeValue = XmlParser.GetAttributeValueAtIndex(xml, index);
        }

        public XmlElementPath Path
        {
            get { return path; }
        }

        public string SelectedAttribute
        {
            get { return selectedAttribute; }
        }

        public bool HasSelectedAttribute
        {
            get { return selectedAttribute.Length > 0; }
        }

        public string SelectedAttributeValue
        {
            get { return selectedAttributeValue; }
        }

        public bool HasSelectedAttributeValue
        {
            get { return selectedAttributeValue.Length > 0; }
        }
    }
}
