using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class ToolStripDropDownMenu : ToolStripDropDown
    {
        public ToolStripDropDownMenu()
        {
        }
        //public Rectangle DisplayRectangle { get; }

        //public LayoutEngine LayoutEngine { get; }

        public ToolStripLayoutStyle LayoutStyle { get; set; }

        [DefaultValue(true)] public bool ShowImageMargin { get; set; } = true;

        [DefaultValue(false)] public bool ShowCheckMargin { get; set; }
        protected Padding DefaultPadding { get; }

        protected internal Size MaxItemSize { get; }

        protected void OnFontChanged(EventArgs e)
        {
        }

        protected void OnLayout(LayoutEventArgs e)
        {
        }

        protected void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected void SetDisplayedItems()
        {
        }

        protected internal ToolStripItem CreateDefaultItem(string text, Image image, EventHandler onClick)
        {
            return null;
        }
    }
}