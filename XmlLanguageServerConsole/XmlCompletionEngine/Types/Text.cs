using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlCompletionEngine.Types
{
    public class Text
    {
        /// <summary>
        /// 
        /// </summary>
        public Uri Uri
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ContentLength
        {
            get;
            set;
        }

        public Uri BaseUri
        {
            get;set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="filePath"></param>
        public Text(string text, Uri uri, Uri baseUri)
        {
            this.Uri = uri;
            this.Content = text;
            this.ContentLength = text.Length;
            this.BaseUri = baseUri;
        }


        /// <summary>
        /// Gets a character at the specified position in the document.
        /// </summary>
        /// <paramref name="offset">The index of the character to get.</paramref>
        /// <exception cref="ArgumentOutOfRangeException">Offset is outside the valid range (0 to TextLength-1).</exception>
        /// <returns>The character at the specified position.</returns>
        /// <remarks>This is the same as Text[offset], but is more efficient because
        ///  it doesn't require creating a String object.</remarks>
        public char GetCharAt(int offset)
        {
            return this.Content[offset];
        }

        /// <summary>
        /// Retrieves the text for a portion of the document.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">offset or length is outside the valid range.</exception>
        /// <remarks>This is the same as Text.Substring, but is more efficient because
        ///  it doesn't require creating a String object for the whole document.</remarks>
        public string GetText(int offset, int length)
        {
            return this.Content.Substring(offset, length);
        }
    }


    /// <summary>
    /// Represents the caret in a text editor.
    /// </summary>
    public class TextPosition
    {
        /// <summary>
        /// Gets/Sets the caret offset;
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets/Sets the caret line number.
        /// Line numbers are counted starting from 1.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// Gets/Sets the caret column number.
        /// Column numbers are counted starting from 1.
        /// </summary>
        public int Column { get; set; }
    }


}
