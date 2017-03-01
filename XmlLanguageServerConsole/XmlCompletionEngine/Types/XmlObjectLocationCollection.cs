using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlCompletionEngine.Types
{
    [Serializable()]
    public class XmlObjectLocationCollection : Collection<XmlObjectLocation>
    {
        public XmlObjectLocationCollection()
        {
        }

        /*public XmlObjectLocationCollection(XmlObjectLocationCollection items)
            : this()
        {
            AddRange(items);
        }*/

        public XmlObjectLocationCollection(XmlObjectLocation[] items)
            : this()
        {
            AddRange(items);
        }

        public bool HasItems
        {
            get { return Count > 0; }
        }

        public bool ContainsAllAvailableItems
        {
            get
            {
                return true;
            }
        }

        public void Sort()
        {
            List<XmlObjectLocation> items = base.Items as List<XmlObjectLocation>;
            items.Sort();
        }

        public void AddRange(XmlObjectLocation[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                //if (!Contains(items[i].Text))
                //{
                Add(items[i]);
                //}
            }
        }


        public XmlObjectLocation SuggestedItem
        {
            get
            {
                if (HasItems && PreselectionLength == 0)
                {
                    return this[0];
                }
                return null;
            }
        }

        public int PreselectionLength { get; set; }
        public int PostselectionLength { get; set; }
    }
}
