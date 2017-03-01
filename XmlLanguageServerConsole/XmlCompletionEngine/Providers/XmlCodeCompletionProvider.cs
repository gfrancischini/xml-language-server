using System.Linq;
using XmlCompletionEngine.Parser;
using XmlCompletionEngine.Types;

namespace XmlCompletionEngine
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlCodeCompletionProvider : XmlBaseProvider
    {
        /// <summary>
        /// Auto create a new Schema
        /// </summary>
        public XmlCodeCompletionProvider(Text text, TextPosition position)
            : base(text, position)
        {
        }

        protected virtual XmlCompletionItemCollection GetCompletionItemsInsideAttribute(XmlSchemaCompletion defaultSchema, XmlSchemaCompletionCollection schemas)
        {
            XmlCompletionItemCollection completionItems = new XmlCompletionItemCollection();
            int offset = this.Position.Offset;
            string textUpToCursor = this.Text.GetText(0, offset);

            completionItems = schemas.GetNamespaceCompletion(textUpToCursor);
            if (completionItems.Count == 0)
            {
                completionItems = schemas.GetAttributeValueCompletion(textUpToCursor, this.Position.Offset, defaultSchema);
            }

            return completionItems;
        }

        protected XmlCompletionItemCollection GetCompletionItemsForAttribute(XmlSchemaCompletion defaultSchema, XmlSchemaCompletionCollection schemas)
        {
            int offset = this.Position.Offset;
            string textUpToCursor = this.Text.GetText(0, offset);
            XmlCompletionItemCollection completionItems = new XmlCompletionItemCollection();

            completionItems = schemas.GetAttributeCompletion(textUpToCursor, defaultSchema);
            if (completionItems.Count == 0)
            {
                completionItems = schemas.GetElementCompletion(textUpToCursor, defaultSchema);
                if (this.HasOpenToken == false)
                {
                    completionItems.Select(i => { i.Text = "<" + i.Text; return i; }).ToList();
                }
                completionItems.Add(new XmlCompletionItem("<?"));
                completionItems.Add(new XmlCompletionItem("<!--"));
            }

            return completionItems;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="defaultSchema"></param>
        /// <param name="hasOpenToken"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        protected XmlCompletionItemCollection GetCompletionItems(XmlSchemaCompletion defaultSchema, XmlSchemaCompletionCollection schemas)
        {
            XmlCompletionItemCollection completionItems = new XmlCompletionItemCollection();

            int offset = this.Position.Offset;
            string textUpToCursor = this.Text.GetText(0, offset);

            if (XmlParser.IsInsideAttributeValue(textUpToCursor, offset))
            {
                completionItems.AddRange(this.GetCompletionItemsInsideAttribute(defaultSchema, schemas));
            }
            else
            {
                completionItems.AddRange(this.GetCompletionItemsForAttribute(defaultSchema, schemas));
            }
            return completionItems;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="editor"></param>
        /// <returns></returns>
        public XmlCompletionItemCollection RetrieveCompletionItemCollection()
        {
            XmlSchemaCompletionCollection schemas = this.AutoFindSchemaCollection();

            //retrieve information about the path that we are working
            int offset = this.FixOffset(this.Text, this.Position.Offset);
            if (offset == -1)
            {
                return null;
            }

            string textUpToCursor = this.Text.GetText(0, offset);
            XmlElementPath path = XmlParser.GetActiveElementStartPath(textUpToCursor, textUpToCursor.Length);
            XmlSchemaCompletion defaultSchema = GetDefaultSchema(path, schemas);

            XmlCompletionItemCollection completionItems = GetCompletionItems(defaultSchema, schemas);
            completionItems.Sort();

            return completionItems;
        }


    }
}
