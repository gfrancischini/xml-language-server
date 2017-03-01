using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XmlValidation
{
    public enum XmlErrorType
    {
        SYNTAX,
        SEMANTIC
    }

    public enum XmlErrorGravity
    {
        WARNING,
        ERROR
    }

    public class XmlError
    {
        public XmlErrorType ErrorType
        {
            get;
            set;
        }
        public int Line
        {
            get;
            set;
        }
        public int Column
        {
            get;
            set;
        }
        public string Message
        {
            get;
            set;
        }
        public int ErrorCode
        {
            get;
            set;
        }
        public string FilePath
        {
            get;
            set;
        }
        public XmlErrorGravity ErrorGravity
        {
            get;
            set;
        }

    }
}
