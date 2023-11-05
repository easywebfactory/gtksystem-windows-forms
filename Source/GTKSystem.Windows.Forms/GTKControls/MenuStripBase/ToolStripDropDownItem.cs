using System;
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public delegate void ToolStripItemClickedEventHandler(object sender, ToolStripItemClickedEventArgs e);
    public abstract class ToolStripDropDownItem : ToolStripItem
    {
        protected ToolStripDropDownItem() : this("", null, null, "") { }

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
            base.Shown += ToolStripButton_Shown;
        }

        private void ToolStripButton_Shown(object sender, EventArgs e)
        {
            if (DisplayStyle == ToolStripItemDisplayStyle.Text)
            {
                base.AlwaysShowImage = false;
            }
            else if (DisplayStyle == ToolStripItemDisplayStyle.Image)
            {
                base.AlwaysShowImage = true;
                base.Text = "";

            }
            else if (DisplayStyle == ToolStripItemDisplayStyle.ImageAndText)
            {
                base.AlwaysShowImage = true;
            }
            else if (DisplayStyle == ToolStripItemDisplayStyle.None)
            {
                base.AlwaysShowImage = false;
                base.Text = "";
            }
            base.Text += " ▼";
        }

        private Image image;
        public new Image Image
        {
            get { return image; }
            set
            {
                image = value;
                if (value != null && value.PixbufData != null)
                    base.Image = new Gtk.Image(new Gdk.Pixbuf(value.PixbufData));

            }
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Pressed { get; }
      
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

        public event EventHandler DropDownOpening;

        public event EventHandler DropDownClosed;

        public event EventHandler DropDownOpened;


        public event ToolStripItemClickedEventHandler DropDownItemClicked;

        public void HideDropDown() { }

        public void ShowDropDown() { }

    }
}

