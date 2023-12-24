
using Gtk;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class Button : WidgetControl<Gtk.Button>
    {
        public Button() : base(new Gtk.Label())
        {
            Widget.StyleContext.AddClass("Button");
            Widget.StyleContext.AddClass("BorderRadiusStyle");
        }

        public override string Text { get => base.Control.Label; set => base.Control.Label = value; }
    }
}
