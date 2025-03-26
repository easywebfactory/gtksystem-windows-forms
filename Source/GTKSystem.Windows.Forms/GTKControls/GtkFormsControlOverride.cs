using Gtk;

namespace System.Windows.Forms;

internal class GtkFormsControlOverride: GtkControlOverride
{
    public GtkFormsControlOverride(Widget? container) : base(container)
    {
    }
}