
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
        control.lockLocation = true;
        control.Parent = Container;
        control.Widget.Margin = 4;
        control.Widget.Valign = Align.Start;
        control.Widget.Halign = Align.Start;
        control.Widget.Hexpand = false;
        control.Widget.WidthRequest = 100;
        if (Container?.self.GetChildAt(column, row) is Viewport view)
        {
            var widget = control.Widget as Widget;
            if (widget != null) view.Child = widget;
        }
        else
        {
            var viewport = new Viewport { Vexpand = false, Hexpand = false };
            viewport.Valign = Align.Fill;
            viewport.Halign = Align.Fill;
            var widget = control.Widget as Widget;
            if (widget != null) viewport.Add(widget);
            Container?.self.Attach(viewport, column, row, 1, 1);
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