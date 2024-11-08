using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
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
            __ownerControl.Mapped += __ownerControl_Mapped;
        }

        public ControlCollection(Control owner, Gtk.Container ownerContainer)
        {
            __ownerControl = ownerContainer;
            __owner = owner;
            __ownerControl.Mapped += __ownerControl_Mapped;
        }

        private void __ownerControl_Mapped(object sender, EventArgs e)
        {
            Gtk.Widget parent = (Gtk.Widget)sender;
            if (__ownerControl is Gtk.Overlay lay)
            {
                foreach (object item in this)
                {
                    if (item is Control control)
                    {
                        control.Widget.MarginStart = Math.Max(0, control.Widget.MarginStart + Offset.X);
                        control.Widget.MarginTop = Math.Max(0, control.Widget.MarginTop + Offset.Y);
                        SetMarginEnd(lay, control.Widget);
                    }
                    else if (item is Gtk.Widget widget)
                    {
                        widget.MarginStart = Math.Max(0, widget.MarginStart + Offset.X);
                        widget.MarginTop = Math.Max(0, widget.MarginTop + Offset.Y);
                        SetMarginEnd(lay, widget);
                    }
                }
            }
        }

        public ControlCollection(CheckedListBox owner, Type itemType)
        {
            __ownerControl = owner.self;
            checkedListBox = owner;
            __owner = owner;
            __itemType = itemType;
        }

        internal Drawing.Point Offset = new Drawing.Point(0, 0);
        public override int Add(object item)
        {
            if (item is Control icontrol)
            {
                icontrol.Parent = __owner;
            }
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
                        Gtk.Overlay overlay = new Overlay();
                        overlay.HeightRequest = statusbar.Height;
                        overlay.AddOverlay(statusbar.self);
                        form.self.ContentArea.PackEnd(overlay, false, false, 0);
                    }
                }
                else if (item is Control control)
                {
                    lay.AddOverlay(control.Widget);
                    lay.WidthRequest = Math.Max(0, Math.Max(lay.AllocatedWidth, control.Widget.MarginStart + control.Widget.WidthRequest + control.Widget.MarginEnd));
                    lay.HeightRequest = Math.Max(0, Math.Max(lay.AllocatedHeight, control.Widget.MarginTop + control.Widget.HeightRequest + control.Widget.MarginBottom));
                    SetMarginEnd(lay, control.Widget);
                    control.DockChanged += Control_DockChanged;
                    control.AnchorChanged += Control_AnchorChanged;
                }
                else if (item is Gtk.Widget widget)
                {
                    lay.AddOverlay(widget);
                    lay.WidthRequest = Math.Max(lay.WidthRequest, widget.MarginStart + widget.WidthRequest + widget.MarginEnd);
                    lay.HeightRequest = Math.Max(lay.HeightRequest, widget.MarginTop + widget.HeightRequest + widget.MarginBottom);
                    SetMarginEnd(lay, widget);
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

            return base.Add(item);
        }

        private void Control_AnchorChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control.Widget.Parent is Gtk.Overlay lay)
            {
                 SetMarginEnd(lay, control.Widget);
            }
        }

        private void Control_DockChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control.Widget.Parent is Gtk.Overlay lay)
            {
                SetMarginEnd(lay, control.Widget);
            }
        }
        private void SetMarginEnd(Gtk.Overlay lay, Gtk.Widget widget)
        {
            if (widget.Halign == Align.End || widget.Halign == Align.Fill)
            {
                if (widget.WidthRequest > 0)
                    widget.MarginEnd = Math.Max(0, lay.AllocatedWidth - widget.MarginStart - widget.WidthRequest);
            }
            if (widget.Valign == Align.End || widget.Valign == Align.Fill)
            {
                if (widget.HeightRequest > 0)
                    widget.MarginBottom = Math.Max(0, lay.AllocatedHeight - widget.MarginTop - widget.HeightRequest);
            }
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

        public override void Remove(object value)
        {
            if (value is Control control)
            {
                control.Widget.Unparent();
                control.Widget.Destroy();
                control.Widget.Dispose();
                control.Dispose();
            }
            if (value is Gtk.Widget widget)
            {
                widget.Unparent();
                widget.Destroy();
                widget.Dispose();
            }
            if (base.Contains(value))
                base.Remove(value); ;
        }
        public override void RemoveAt(int index)
        {
            base.RemoveAt(index);
            if (__ownerControl.Children.Length > index)
                __ownerControl.Remove(__ownerControl.Children[index]);
        }
    }
}