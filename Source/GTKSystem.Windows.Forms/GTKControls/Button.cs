using Gdk;
using Gtk;
using GTKSystem.Resources;
using Pango;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;

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
