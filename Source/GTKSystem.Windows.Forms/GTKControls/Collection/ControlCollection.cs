using Gtk;
using Pango;
using System.Collections;

namespace System.Windows.Forms
{
    public class ControlCollection : ArrayList
    {
        Gtk.Container __ownerControl;
        Control __owner;
        Type __itemType;
        CheckedListBox checkedListBox;
        public ControlCollection(Control owner)
        {
            __ownerControl = owner.GtkControl as Gtk.Container;
            __owner = owner;
        }
        public ControlCollection(Control owner, Gtk.Container ownerContainer)
        {
            __ownerControl = ownerContainer;
            __owner = owner;
        }

        public ControlCollection(CheckedListBox owner, Type itemType)
        {
            __ownerControl = owner.self;
            checkedListBox = owner;
            __owner = owner;
            __itemType = itemType;
        }
        internal Drawing.Point Offset = new Drawing.Point(0,0);
        public override int Add(object item)
        {
            if (item is Control control)
            {
                control.Parent = __owner;
            }
            GLib.Timeout.Add(100, new GLib.TimeoutHandler(() => {
            if (__ownerControl is Gtk.Overlay lay)
            {
                if (item is StatusStrip statusbar)
                {
                    if (__owner is Form form)
                    {
                        statusbar.self.Halign = Gtk.Align.Fill;
                        statusbar.self.Valign = Gtk.Align.Start;
                        statusbar.self.Expand = false;
                        statusbar.self.MarginStart = 0;
                        statusbar.self.MarginTop = 0;
                        statusbar.self.MarginEnd = 0;
                        statusbar.self.MarginBottom = 0;
                        form.self.ContentArea.PackEnd(statusbar.self, false, true, 0);
                    }
                }
                else if (item is Control con)
                {
                    if (lay.Child is Fixed fix)
                    {
                        if (con.Widget.Halign == Gtk.Align.Fill || con.Widget.Halign == Gtk.Align.Fill)
                            lay.AddOverlay(con.Widget);
                        else
                            fix.Put(con.Widget, 0, 0);
                    }
                    else
                        lay.AddOverlay(con.Widget);
                }
                else if (item is Gtk.Widget widget)
                {
                    if (lay.Child is Fixed fix)
                    {
                        if (widget.Halign == Gtk.Align.Fill || widget.Halign == Gtk.Align.Fill)
                            lay.AddOverlay(widget);
                        else
                            fix.Put(widget, 0, 0);
                    }
                    else
                        lay.AddOverlay(widget);
                }
            }
            else if (__ownerControl is Gtk.Fixed lay2)
            {
                if (item is Control con)
                {
                    lay2.Put(con.Widget, Offset.X, Offset.Y);
                }
                else if (item is Gtk.Widget widget)
                {
                    lay2.Put(widget, Offset.X, Offset.Y);
                }
            }
            else if (__ownerControl is Gtk.Layout lay3)
            {
                if (item is Control con)
                {
                    lay3.Put(con.Widget, Offset.X, Offset.Y);
                }
                else if (item is Gtk.Widget widget)
                {
                    lay3.Put(widget, Offset.X, Offset.Y);
                }
            }

                __ownerControl.ShowAll();

                return false;
            }));
            return base.Add(item);
        }

        public int AddWidget(Gtk.Widget item, Control control)
        {
            control.Parent = __owner;
            return base.Add(item);
        }
        public virtual void Add(Type itemType, object item)
        {
            //重载处理
            base.Add(item);
            if (__ownerControl.IsRealized)
            {
                __ownerControl.ShowAll();
            }
        }

        public virtual void AddRange(object[] items)
        {
            foreach (object item in items)
            {
                if (item is String)
                    Add(__itemType, item);
                else
                    Add(item);
            }
        }

        public override void Clear()
        {
            foreach (Widget wid in __ownerControl.Children)
                __ownerControl.Remove(wid);

            base.Clear();
        }

        public override void Insert(int index, object item) { __ownerControl.Add((Widget)item); base.Insert(index, item); }

        public override void Remove(object value) { base.Remove(((Control)value).Widget); __ownerControl.Remove(((Control)value).Widget); }
        public override void RemoveAt(int index) { base.RemoveAt(index); __ownerControl.Remove(__ownerControl.Children[index]); }
    }
}