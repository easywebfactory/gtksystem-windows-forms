
using Gtk;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [ListBindable(false)]
	public class TableLayoutControlCollection : Control.ControlCollection
	{
        public TableLayoutControlCollection(TableLayoutPanel container) : base(container)
        {
            Container = container;
        }
        public override void Add(Control? value)
        {
            throw new NotSupportedException();
        }
        public virtual void Add(Control control, int column, int row)
        {
            control.Location = new Drawing.Point(0, 0);
            control.LockLocation = true;
            control.Parent = Container;
            if (Container.layoutEngine.GetChildAt(column, row) is Gtk.Viewport view)
            {
                view.Add(control.Widget);
            }
            else
            {
                Gtk.Viewport viewport = new Gtk.Viewport();
                viewport.Valign = Align.Fill;
                viewport.Halign = Align.Fill;
                viewport.BorderWidth = 0;
                viewport.ShadowType = ShadowType.In;
                viewport.Add(control.Widget);
                Container.layoutEngine.Attach(viewport, column, row, 1, 1);
            }
            base.Add(control);
            Container.SetColumn(control, column);
            Container.SetRow(control, row);
        }
        public override void Remove(Control? value)
        {
            if (value is null)
            {
                return;
            }
            if (value.Widget.Parent is Gtk.Viewport view)
            {
                view.Remove(view.Child);
                InnerList.Remove(value);
            }
        }
        public TableLayoutPanel Container { get; }
    }
}
