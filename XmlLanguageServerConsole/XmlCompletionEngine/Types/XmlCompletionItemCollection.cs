using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlCompletionEngine.Types
{
    [Serializable()]
    public class XmlCompletionItemCollection : Collection<XmlCompletionItem>
    {
        public XmlCompletionItemCollection()
        {
        }

        public XmlCompletionItemCollection(XmlCompletionItemCollection items)
            : this()
        {
            AddRange(items);
        }

        public XmlCompletionItemCollection(XmlCompletionItem[] items)
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
            List<XmlCompletionItem> items = base.Items as List<XmlCompletionItem>;
            items.Sort();
        }

        public void AddRange(XmlCompletionItem[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (!Contains(items[i].Text))
                {
                    Add(items[i]);
                }
            }
        }

        public void AddRange(XmlCompletionItemCollection item)
        {
            for (int i = 0; i < item.Count; i++)
            {
                if (!Contains(item[i].Text))
                {
                    Add(item[i]);
                }
            }
        }

        public bool Contains(string name)
        {
            foreach (XmlCompletionItem data in this)
            {
                if (data.Text != null)
                {
                    if (data.Text.Length > 0)
                    {
                        if (data.Text == name)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a count of the number of occurrences of a particular name
        /// in the completion data.
        /// </summary>
        public int GetOccurrences(string name)
        {
            int count = 0;

            foreach (XmlCompletionItem item in this)
            {
                if (item.Text == name)
                {
                    ++count;
                }
            }

            return count;
        }

        public XmlCompletionItem SuggestedItem
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
