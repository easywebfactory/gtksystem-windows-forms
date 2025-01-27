using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public delegate void ToolStripItemClickedEventHandler(object sender, ToolStripItemClickedEventArgs e);
    public abstract class ToolStripDropDownItem : WidgetToolStrip<Gtk.MenuItem>
    {
        protected ToolStripDropDownItem() : this("", null, null, "") {
            
        }

        protected ToolStripDropDownItem(string text, Image image, EventHandler onClick) : this(text, image, onClick, "")
        {

        }
   
        protected ToolStripDropDownItem(string text, Image image, params ToolStripItem[] dropDownItems) : this(text, image, null, "")
        {
            DropDownItems.AddRange(dropDownItems);
        }
       
        protected ToolStripDropDownItem(string text, Image image, EventHandler onClick, string name) : base("ToolStripDropDownItem", text, image, onClick, name)
        {
            DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
        }

        public override string Text { get => base.Text; set => base.Text = value; } //+ " ▼"


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


        //public event ToolStripItemClickedEventHandler DropDownItemClicked;

        public void HideDropDown() { }

        public void ShowDropDown() { }

    }
}

