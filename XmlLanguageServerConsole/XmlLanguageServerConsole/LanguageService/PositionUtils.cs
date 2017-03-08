using LanguageServerProtocol.Contracts.Document.Types;
using XmlCompletionEngine.Types;

namespace XmlLanguageServerConsole.LanguageService
{
    public class PositionUtils
    {
        public static int FindOffset(string fileText, int line, int column)
        {
            // we count our current line and column position
            int currentCol = 0;
            int currentLine = 0;


            for (int i = 0; i < fileText.Length; i++)
            {
                if (currentLine == line && currentCol == column)
                {
                    return i;
                }


                // line break - increment the line counter and reset the column
                if (fileText[i] == '\n')
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

        public static TextPosition CreateTextPosition(string text, Position position)
        {
            TextPosition textPosition = new TextPosition();
            textPosition.Column = position.Character;
            textPosition.Line = position.Line;
            textPosition.Offset = PositionUtils.FindOffset(text, textPosition.Line, textPosition.Column);
            return textPosition;
        }
    }
}
