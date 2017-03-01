using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlCompletionEngine.Types
{
    /// <summary>
    /// The type of text held in this object.
    /// </summary>
    public enum XmlCompletionItemType
    {
        None = 0,
        XmlElement = 1,
        XmlAttribute = 2,
        NamespaceUri = 3,
        XmlAttributeValue = 4
    }

    /// <summary>
    /// Holds the text for  namespace, child element or attribute
    /// autocomplete (intellisense).
    /// </summary>
    public class XmlCompletionItem : DefaultCompletionItem, IComparable<XmlCompletionItem>
    {
        XmlCompletionItemType dataType = XmlCompletionItemType.XmlElement;
        string documentation = String.Empty;

        public XmlCompletionItem(string text)
            : this(text, String.Empty, XmlCompletionItemType.XmlElement)
        {
        }

        public XmlCompletionItem(string text, string documentation)
            : this(text, documentation, XmlCompletionItemType.XmlElement)
        {
        }

        public XmlCompletionItem(string text, XmlCompletionItemType dataType)
            : this(text, String.Empty, dataType)
        {
        }

        public XmlCompletionItem(string text, string documentation, XmlCompletionItemType dataType)
            : base(text)
        {
            this.documentation = documentation;
            this.dataType = dataType;
        }

        /// <summary>
        /// Returns the xml item's documentation as retrieved from
        /// the xs:annotation/xs:documentation element.
        /// </summary>
        public override string Documentation
        {
            get { return documentation; }
        }

        public XmlCompletionItemType DataType
        {
            get { return dataType; }
        }

        /*public override void Complete(CompletionContext context)
        {
            base.Complete(context);

            switch (dataType)
            {
                case XmlCompletionItemType.XmlAttribute:
                    //context.Editor.Document.Insert(context.EndOffset, "=\"\"");
                    //context.Editor.Caret.Offset--;
                    //					XmlCodeCompletionBinding.Instance.CtrlSpace(context.Editor);
                    break;
            }
        }*/

        public override string ToString()
        {
            return "[" + Text + "]";
        }

        public override int GetHashCode()
        {
            return dataType.GetHashCode() ^ Text.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            XmlCompletionItem item = obj as XmlCompletionItem;
            if (item != null)
            {
                return (dataType == item.dataType) && (Text == item.Text);
            }
            return false;
        }

        public int CompareTo(XmlCompletionItem other)
        {
            return Text.CompareTo(other.Text);
        }
    }
}
