using System.Windows.Forms;
using Gtk;

namespace GTKSystem.Windows.Forms.GTKControls;

internal class GtkFormsControlOverride: GtkControlOverride, IGtkControlOverride
{
    public GtkFormsControlOverride(Widget? container) : base(container)
    {
    }
}