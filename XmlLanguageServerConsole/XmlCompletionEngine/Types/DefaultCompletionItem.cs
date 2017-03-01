using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlCompletionEngine.Types
{
    public class DefaultCompletionItem
    {
        public string Text { get; set; }
        public virtual string Documentation { get; set; }
        public virtual double Priority { get; protected set; }

        public DefaultCompletionItem(string text)
        {
            this.Text = text;
        }
    }
}
