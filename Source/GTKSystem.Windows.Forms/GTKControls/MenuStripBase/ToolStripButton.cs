using System;
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{

    public class ToolStripButton : WidgetToolStrip<Gtk.MenuItem>
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
        }

        public bool CheckOnClick { get; set; }
       
      
        protected bool DefaultAutoToolTip { get; }
       
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected AccessibleObject CreateAccessibilityInstance()
        {
            return new AccessibleObject();
        }
     
        protected virtual void OnCheckedChanged(EventArgs e) { }
       
        protected virtual void OnCheckStateChanged(EventArgs e)
        {

        }

    }
}

