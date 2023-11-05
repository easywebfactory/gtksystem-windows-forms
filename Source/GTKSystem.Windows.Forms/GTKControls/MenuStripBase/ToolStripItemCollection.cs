using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using GLib;

namespace System.Windows.Forms
{
    public class ToolStripItemCollection : IList<ToolStripItem>, ICollection, IEnumerable
    {
        private Gtk.MenuItem owner;
        private ToolStrip toolStrip;
        private Gtk.Menu menu;
        private bool isToolStrip;
        private bool isContextMenu;

        public ToolStripItemCollection(ToolStrip owner) 
        {
            this.toolStrip = owner;
            isToolStrip = true;
        }
        public ToolStripItemCollection(Gtk.Menu owner)
        {
             this.menu = owner;
            isContextMenu = true;
            this.menu.ShowAll();
        }
        public ToolStripItemCollection(Gtk.MenuItem owner)
        {
            this.owner = owner;
            isToolStrip = false;
        }

        internal ToolStripItemCollection(ToolStripItem owner, bool itemsCollection)
            : this(owner, itemsCollection, isReadOnly: false)
        {
        }

        internal ToolStripItemCollection(ToolStripItem owner, bool itemsCollection, bool isReadOnly)
        {
            this.owner = owner;

            isToolStrip = false;
        }
        public ToolStripItemCollection(ToolStripItem owner, ToolStripItem[] value)
        {
            this.owner = owner;
            isToolStrip = false;
            AddRange(value);
        }
        public ToolStripItem Add(string text)
        {
            return Add(text, null, null);
        }

        public ToolStripItem Add(Image image)
        {
            return Add(null, image, null);
        }


        public ToolStripItem Add(string text, Image image)
        {
            return Add(text, image, null);
        }

        public ToolStripItem Add(string text, Image image, EventHandler onClick)
        {
            ToolStripItem toolStripItem = new ToolStripLabel();
            Add(toolStripItem);
            return toolStripItem;
        }

        public int Add(ToolStripItem value)
        {
            if (isToolStrip == true)
            {
                toolStrip.Control.Add(value);
            }
            else if (isContextMenu == true)
            {
                menu.Add(value);
            }
            else
            {
                if (owner.Submenu == null)
                {
                    this.menu = new Gtk.Menu();
                    owner.Submenu = this.menu;
                }
                menu.Add(value);
            }
            return Count;
        }

        public void AddRange(ToolStripItem[] toolStripItems)
        {
            for (int i = 0; i < toolStripItems.Length; i++)
            {
                Add(toolStripItems[i]);
            }
        }

        public void AddRange(ToolStripItemCollection toolStripItems)
        {
                int count = toolStripItems.Count;
                for (int i = 0; i < count; i++)
                {
                    Add(toolStripItems[i]);
                }
            
        }
        //-------------------

        public ToolStripItem this[int index]
        {
            get { ArrayList all = menu.AllChildren as ArrayList; return all[index] as ToolStripItem; }
            set { menu.Insert(value, index); }
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(ToolStripItem item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, ToolStripItem item)
        {
            throw new NotImplementedException();
        }

        void ICollection<ToolStripItem>.Add(ToolStripItem item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(ToolStripItem item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(ToolStripItem[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ToolStripItem item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<ToolStripItem> IEnumerable<ToolStripItem>.GetEnumerator()
        {
            foreach (var item in menu.AllChildren)
            {
                yield return item as ToolStripItem;
            }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {

                if (isToolStrip == true)
                {
                    ArrayList all = toolStrip.Control.AllChildren as ArrayList;
                    return all.Count;
                }
                else
                {
                    ArrayList all = menu.AllChildren as ArrayList;
                    return all.Count;
                }
            }
        }

        public bool IsFixedSize => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();

    }
}
