using System;
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{

    public class ToolStripButton : ToolStripItem
    {

        public ToolStripButton() : this("", null, null, "")
        {

        }

        public ToolStripButton(string text) : this(text, null, null, "")
        {

        }
        public ToolStripButton(Image image) : this("", image, null, "") { }

        public ToolStripButton(string text, Image image) : this(text, image, null, "")
        {

        }
        
        public ToolStripButton(string text, Image image, EventHandler onClick) : this(text, image, onClick, "")
        {

        }
       
        public ToolStripButton(string text, Image image, EventHandler onClick, string name) : base(text, image, onClick, name)
        {
            DisplayStyle=System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
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
                base.Text = "  ";
            }
        }

        private Image image;
        public new Image Image { 
            get { return image; }
            set { 
                image = value;
                if (value != null && value.PixbufData != null)
                    base.Image = new Gtk.Image(new Gdk.Pixbuf(value.PixbufData));

            }
        }

        //[DefaultValue(CheckState.Unchecked)]
        //public CheckState CheckState { get; set; }

        //[DefaultValue(false)]
        //public bool Checked { get; set; }

        [DefaultValue(false)]
        public bool CheckOnClick { get; set; }
       
        public bool CanSelect { get; }
      
        //[DefaultValue(true)]
        //public bool AutoToolTip { get; set; }
      
        protected bool DefaultAutoToolTip { get; }

        public event EventHandler CheckStateChanged;
       
        public event EventHandler CheckedChanged;

        public Size GetPreferredSize(Size constrainingSize)
        {
            return constrainingSize;
        }
       
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected AccessibleObject CreateAccessibilityInstance()
        {
            return new AccessibleObject();
        }
     
        protected virtual void OnCheckedChanged(EventArgs e) { }
       
        protected virtual void OnCheckStateChanged(EventArgs e)
        {

        }
       
        protected  void OnClick(EventArgs e) { }

    }
}

