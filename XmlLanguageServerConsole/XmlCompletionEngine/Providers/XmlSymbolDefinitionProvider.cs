using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlCompletionEngine.Parser;
using XmlCompletionEngine.Types;

namespace XmlCompletionEngine
{
    public class XmlSymbolDefinitionProvider : XmlBaseProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        public XmlSymbolDefinitionProvider(Text text, TextPosition position)
            : base(text, position)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual XmlObjectLocationCollection RetrieveSymbolLocation()
        {
            //retrieve information about the path that we are working
            int offset = this.FixOffset(this.Text, this.Position.Offset);
            if (offset == -1)
            {
                return null;
            }

            return this.RetrieveSymbolLocationFromXsd();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected XmlObjectLocationCollection RetrieveSymbolLocationFromXsd()
        {
            XmlObjectLocationCollection xmlObjectLocationCollection = new XmlObjectLocationCollection();
            XmlSchemaCompletionCollection schemas = this.AutoFindSchemaCollection();

            // Find schema object for selected xml element or attribute.
            XmlSchemaCompletion currentSchemaCompletion = schemas.FirstOrDefault();
            if (currentSchemaCompletion == null)
            {
                return null;
            }

            XmlSchemaDefinition schemaDefinition = new XmlSchemaDefinition(schemas, currentSchemaCompletion);
            XmlObjectLocation schemaObjectLocation = schemaDefinition.GetSelectedSchemaObjectLocation(this.Text.Content, this.Position.Offset);
            if (schemaObjectLocation != null)
            {
                xmlObjectLocationCollection.Add(schemaObjectLocation);
            }

            return xmlObjectLocationCollection;
        }

    }
}
