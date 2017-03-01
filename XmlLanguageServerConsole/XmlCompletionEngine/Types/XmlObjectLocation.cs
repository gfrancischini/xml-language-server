using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace XmlCompletionEngine.Types
{
    public class XmlObjectLocation
    {
        public Uri Uri
        {
            get;
            protected set;
        }

        public int LineNumber
        {
            get;
            protected set;
        }

        public int LinePosition
        {
            get;
            protected set;
        }

        public XmlObjectLocation()
        {
        }

        public XmlObjectLocation(int lineNumber, int linePosition, Uri uri)
        {
            this.LineNumber = lineNumber;
            this.LinePosition = linePosition;
            this.Uri = uri;
        }

    }
}
