using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using GLib;

namespace System.Windows.Forms
{
    public class ToolStripItemCollection : List<ToolStripItem>, ICollection, IEnumerable
    {
        private ToolStripItem owner;
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
        public ToolStripItemCollection(ToolStripItem owner)
        {
            this.owner = owner;
            isContextMenu = false;
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

        public int AddMemu(ToolStripItem value)
        {
            if (isToolStrip == true)
            {
                toolStrip.Control.Add(value.Widget);
            }
            else if (isContextMenu == true)
            {
                menu.Add(value.Widget);
            }
            else
            {
                if (owner.MenuItem.Submenu == null)
                {
                    this.menu = new Gtk.Menu();
                    owner.MenuItem.Submenu = this.menu;
                }
                menu.Add(value.Widget);
            }
            base.Add(value);
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
