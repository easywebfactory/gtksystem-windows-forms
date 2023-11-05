
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class MenuStrip : ToolStrip
    {
        public MenuStrip() : base()
        {
            Widget.StyleContext.AddClass("MenuStrip");
            base.Control.PackDirection = Gtk.PackDirection.Ltr;
            this.Dock = DockStyle.Top;
        }

    }
}
