using Gtk;
using System.ComponentModel;
using Container = Gtk.Container;

namespace System.Windows.Forms;

[ListBindable(false)]
public class TableLayoutControlCollection : Control.ControlCollection
{
    public TableLayoutControlCollection(Control? owner, Container? ownerContainer) : base(owner, ownerContainer)
    {
        Container = owner as TableLayoutPanel;
    }
    public TableLayoutControlCollection(TableLayoutPanel? container) : base(container)
    {
        Container = container;
    }

    public virtual void Add(Control control, int column, int row)
    {
        control.Location = new Drawing.Point(0, 0);
        control.LockLocation = true;
        control.Parent = Container;
        control.Widget.Margin = 4;
        control.Widget.Valign = Align.Start;
        control.Widget.Halign = Align.Start;
        control.Widget.Hexpand = false;
        if (Container?.grid.GetChildAt(column, row) is Viewport view)
        {
            view.Child = control.Widget as Widget;
        }
        else
        {
            var viewport = new Viewport() { Vexpand = false, Hexpand = false };
            viewport.Valign = Align.Fill;
            viewport.Halign = Align.Fill;
            viewport.BorderWidth = 0;
            viewport.Child = control.Widget as Widget;
            Container?.grid.Attach(viewport, column, row, 1, 1);
        }
        base.Add(control);
        if (Container != null)
        {
            Container.SetColumn(control, column);
            Container.SetRow(control, row);
        }
    }

    public TableLayoutPanel? Container { get; }
}