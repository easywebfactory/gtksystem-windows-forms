
using Gtk;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [ListBindable(false)]
	public class TableLayoutControlCollection : Control.ControlCollection
	{
        public TableLayoutControlCollection(Control owner, Gtk.Container ownerContainer) : base(owner, ownerContainer)
        {
            Container = owner as TableLayoutPanel;
        }
        public TableLayoutControlCollection(TableLayoutPanel container) : base(container)
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
            if (Container.grid.GetChildAt(column, row) is Gtk.Viewport view)
            {
                view.Child = control.Widget;
            }
            else
            {
                Gtk.Viewport viewport = new Gtk.Viewport() { Vexpand = false, Hexpand = false };
                viewport.Valign = Align.Fill;
                viewport.Halign = Align.Fill;
                viewport.BorderWidth = 0;
                viewport.Child = control.Widget;
                Container.grid.Attach(viewport, column, row, 1, 1);
            }
            base.Add(control);
            Container.SetColumn(control, column);
            Container.SetRow(control, row);
        }

        public TableLayoutPanel Container { get; }
    }
}
