using System.Collections;
using System.Drawing;

namespace System.Windows.Forms
{
    public class ToolStripItemCollection : List<ToolStripItem>, ICollection, IEnumerable
    {
        private ToolStripItem owner;
        private ToolStrip toolStrip;
        private MenuStrip menuStrip;
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
 
        public ToolStripItemCollection(MenuStrip owner)
        {
            this.menuStrip = owner;
            isMenuStrip =true;
        }
        public ToolStripItemCollection(StatusStrip owner)
        {
            this.statusStrip = owner;
            isStatusStrip = true;
        }
        public ToolStripItemCollection(ToolStripItem owner)
        {
            this.owner = owner;
            if(owner.Widget.MenuItem is Gtk.Menu gmenu)
            {
                this.menu = gmenu;
                isToolStripDropDown = true;
            }
            else if (owner.Widget.ToolItem is Gtk.MenuToolButton tmenu)
            {
                this.menu = new Gtk.Menu();
                tmenu.Menu = this.menu;
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
            ToolStripMenuItem toolStripItem = new ToolStripMenuItem();
            toolStripItem.Text = text;
            toolStripItem.Image = image;
            if (onClick != null)
                toolStripItem.Click += onClick;
            AddMemu(toolStripItem);
            return toolStripItem;
        }
        public new void Add(ToolStripItem item)
        {
            AddMemu(item);
        }
        public int AddMemu(ToolStripItem item)
        {
            item.Parent = owner;
            if (isToolStrip == true)
            {
                toolStrip.self.Add(item.Widget.ToolItem);
            }
            else if (isStatusStrip == true)
            {
                statusStrip.self.Add(item.Widget.ToolItem);
            }
            else if (isMenuStrip == true)
            {
                menuStrip.self.Add(item.Widget.MenuItem);
            }
            else if (isToolStripDropDown == true)
            {
                menu.Add(item.Widget.MenuItem);
            }
            else
            {
                if (owner.MenuItem.Submenu == null)
                {
                    this.menu = new Gtk.Menu();
                    owner.MenuItem.Submenu = this.menu;
                }
                menu.Add(item.Widget.MenuItem);
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
            set { menu.Insert(value.Widget.MenuItem, index); base[index] = value; }
        }
    }
}
