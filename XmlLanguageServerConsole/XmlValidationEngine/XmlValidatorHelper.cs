using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XmlValidation
{
    public class XmlValidatorHelper
    {
        public static int fixLineOrColumn(int value)
        {
            if (value > 0)
            {
                return --value;
            }
            return value;
        }

        public static int FindTagAtLine(string content, int line, int column, string tag)
        {
            int offset = FindOffset(content, line, column);
            string afterOffset = content.Substring(offset);
            int index = afterOffset.IndexOf(tag);
            if (index < 0)
            {
                return index;
            }
            return column + index + 1;
        }

        public static int FindTagEndAtLine(string content, int line, int column, string tag)
        {
            int offset = FindOffset(content, line, column);
            string afterOffset = content.Substring(offset);
            int index = afterOffset.IndexOf(tag);
            if (index < 0)
            {
                return index;
            }
            return column + index + 1 + tag.Length;
        }

        public static int FindFirstTagBefore(string content, int line, int column, string tag)
        {
            int offset = FindOffset(content, line, column);
            string afterOffset = content.Substring(0, offset);
            int index = afterOffset.LastIndexOf(tag);
            if (index < 0)
            {
                return index;
            }
            return column + index + 1 + tag.Length;
        }

        public static int FindOffset(string content, int line, int column)
        {
            // we count our current line and column position
            int currentCol = 0;
            int currentLine = 0;


            for (int i = 0; i < content.Length; i++)
            {
                if (currentLine == line && currentCol == column)
                {
                    return i;
                }


                // line break - increment the line counter and reset the column
                if (content[i] == '\n')
                {
                    currentLine++;
                    currentCol = 0;
                }
                else
                {
                    currentCol++;
                }
            }

            return -1;
        }

    }
}
