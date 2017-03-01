using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RxpInterface
{
    /// <summary>
    /// 
    /// </summary>
    public class RxpError
    {
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get;
            set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int Line
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Column
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Path
        {
            get;
            set;
        }
    }
}
