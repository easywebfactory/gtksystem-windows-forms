using System.Collections.Generic;
using System.Drawing;
using System.Collections;

namespace System.Windows.Forms
{
    public class ToolStripItemCollection : List<ToolStripItem>, ICollection, IEnumerable
    {
        private ToolStripItem owner;
        private ToolStrip toolStrip;
        private StatusStrip statusStrip;
        private Gtk.Menu menu;
        private bool isToolStrip;
        private bool isStatusStrip;
        private bool isMenuStrip;
        private bool isToolStripDropDown;

        public ToolStripItemCollection(ToolStrip owner) 
        {
            this.toolStrip = owner;
            isToolStrip = true;
        }
        public ToolStripItemCollection(ToolStrip toolStrip, string owner)
        {
            this.toolStrip = toolStrip;
            isMenuStrip = owner == "MenuStrip";
        }
        public ToolStripItemCollection(StatusStrip owner)
        {
            this.statusStrip = owner;
            isStatusStrip = true;
        }
        public ToolStripItemCollection(ToolStripDropDown owner)
        {
             this.menu = owner.Widget as Gtk.Menu;
            isToolStripDropDown = true;
        }
        public ToolStripItemCollection(ToolStripItem owner)
        {
            this.owner = owner;
            if(owner.Widget is Gtk.Menu gmenu)
            {
                this.menu = gmenu;
                isToolStripDropDown = true;
            }
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
            AddMemu(toolStripItem);
            return toolStripItem;
        }

        public int AddMemu(ToolStripItem item)
        {
            item.Parent = owner;
            if (isToolStrip == true)
            {
                toolStrip.self.Add(item.Widget);
            }
            else if (isStatusStrip == true)
            {
                statusStrip.self.Add(item.Widget);
            }
            else if (isMenuStrip == true)
            {
                toolStrip.self.Add(item.Widget);
            }
            else if (isToolStripDropDown == true)
            {
                menu.Add(item.Widget);
            }
            else
            {
                if (owner.MenuItem.Submenu == null)
                {
                    this.menu = new Gtk.Menu();
                    owner.MenuItem.Submenu = this.menu;
                }
                menu.Add(item.Widget);
            }

            base.Add(item);
            return Count;
        }

        public void AddRange(ToolStripItem[] toolStripItems)
        {
            for (int i = 0; i < toolStripItems.Length; i++)
            {
                AddMemu(toolStripItems[i]);
            }
        }

        public void AddRange(ToolStripItemCollection toolStripItems)
        {
            int count = toolStripItems.Count;
            for (int i = 0; i < count; i++)
            {
                AddMemu(toolStripItems[i]);
            }
        }
        //-------------------
        public new ToolStripItem this[int index]
        {
            get { return base[index]; }
            set { menu.Insert(value.Widget, index); base[index] = value; }
        }
    }
}
