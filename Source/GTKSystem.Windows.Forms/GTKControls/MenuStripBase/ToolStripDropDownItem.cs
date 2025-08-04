using System;
using System.Drawing;
using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Forms
{
    public delegate void ToolStripItemClickedEventHandler(object sender, ToolStripItemClickedEventArgs e);
    public abstract class ToolStripDropDownItem : ToolStripItem
    {

        public StripMenuToolButton self = new StripMenuToolButton();
        public override IToolMenuItem Widget { get => self; }

        protected ToolStripDropDownItem() : this("", null, null, "") {
            
        }

        protected ToolStripDropDownItem(string text, Image image, EventHandler onClick) : this(text, image, onClick, "")
        {

        }
   
        protected ToolStripDropDownItem(string text, Image image, params ToolStripItem[] dropDownItems) : this(text, image, null, "")
        {
            DropDownItems.AddRange(dropDownItems);
        }
       
        protected ToolStripDropDownItem(string text, Image image, EventHandler onClick, string name) : base(text, image, onClick, name)
        {
            DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            self.Realized += Self_Realized;
        }

        private void Self_Realized(object? sender, EventArgs e)
        {
            SetIcon(DisplayStyle);
        }
        public override string Name { get => self.Name; set => self.Name = value; }
        public override string Text { get => self.Label; set => self.Label = value; }
        public override ToolStripItemDisplayStyle DisplayStyle
        {
            get => base.DisplayStyle;
            set
            {
                base.DisplayStyle = value;
                SetIcon(value);
            }
        }
        private void SetIcon(ToolStripItemDisplayStyle displayStyle)
        {
            if (displayStyle == ToolStripItemDisplayStyle.Text)
            {
                self.IsImportant = false;
            }
            else if (displayStyle == ToolStripItemDisplayStyle.Image)
            {
                self.IsImportant = false;
                if (this.Image?.Pixbuf != null)
                {
                    if (self.IconSize == Gtk.IconSize.LargeToolbar)
                    {
                        self.IconWidget = new Gtk.Image(new Gdk.Pixbuf(this.Image.PixbufData, 30, 30)) { Visible = true };
                    }
                    else if (self.IconSize == Gtk.IconSize.Dialog)
                    {
                        self.IconWidget = new Gtk.Image(new Gdk.Pixbuf(this.Image.PixbufData, 50, 50)) { Visible = true };
                    }
                    else
                    {
                        self.IconWidget = new Gtk.Image(new Gdk.Pixbuf(this.Image.PixbufData, 20, 20)) { Visible = true };
                    }
                }
            }
            else if (displayStyle == ToolStripItemDisplayStyle.ImageAndText)
            {
                self.IsImportant = true;
                if (this.Image?.Pixbuf != null)
                {
                    if (self.IconSize == Gtk.IconSize.LargeToolbar)
                    {
                        self.IconWidget = new Gtk.Image(new Gdk.Pixbuf(this.Image.PixbufData, 30, 30)) { Visible = true };
                    }
                    else if (self.IconSize == Gtk.IconSize.Dialog)
                    {
                        self.IconWidget = new Gtk.Image(new Gdk.Pixbuf(this.Image.PixbufData, 50, 50)) { Visible = true };
                    }
                    else
                    {
                        self.IconWidget = new Gtk.Image(new Gdk.Pixbuf(this.Image.PixbufData, 20, 20)) { Visible = true };
                    }
                }

            }
            else
            {
                self.IsImportant = true;
                if (this.Image?.Pixbuf != null)
                {
                    if (self.IconSize == Gtk.IconSize.LargeToolbar)
                    {
                        self.IconWidget = new Gtk.Image(new Gdk.Pixbuf(this.Image.PixbufData, 30, 30)) { Visible = true };
                    }
                    else if (self.IconSize == Gtk.IconSize.Dialog)
                    {
                        self.IconWidget = new Gtk.Image(new Gdk.Pixbuf(this.Image.PixbufData, 50, 50)) { Visible = true };
                    }
                    else
                    {
                        self.IconWidget = new Gtk.Image(new Gdk.Pixbuf(this.Image.PixbufData, 20, 20)) { Visible = true };
                    }
                }
            }
        }
        //public override string Text { get => base.Text; set => base.Text = value; } //+ " ▼"


        [Browsable(false)]
        public bool HasDropDown { get; }
       
        [Browsable(false)]
        public virtual bool HasDropDownItems { get; }

        //public ToolStripItemCollection DropDownItems { get; }
       
        [Browsable(false)]
        public ToolStripDropDownDirection DropDownDirection { get; set; }
      
        [TypeConverter(typeof(ReferenceConverter))]
        public ToolStripDropDown DropDown { get; set; }
       
        protected internal virtual Point DropDownLocation { get; }

        public void HideDropDown() { }

        public void ShowDropDown() { }

    }
}

