using System;
using System.Collections.Generic;
using System.Text;

namespace XmlLanguageServerConsole.Workspace.Contracts
{
    /// <summary>
    /// Contains details relating to a content change in an open file.
    /// </summary>
    public class FileChange
    {
        /// <summary>
        /// The string which is to be inserted in the file.
        /// </summary>
        public string InsertString { get; set; }

        /// <summary>
        /// The 1-based line number where the change starts.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// The 1-based column offset where the change starts.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// The 1-based line number where the change ends.
        /// </summary>
        public int EndLine { get; set; }

        /// <summary>
        /// The 1-based column offset where the change ends.
        /// </summary>
        public int EndOffset { get; set; }
    }
}
