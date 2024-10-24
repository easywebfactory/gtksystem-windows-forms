using Gtk;
using System.Collections;
using System.Reflection;

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
            __ownerControl.Realized += __ownerControl_Realized;
        }
        public ControlCollection(Control owner, Gtk.Container ownerContainer)
        {
            __ownerControl = ownerContainer;
            __owner = owner;
            __ownerControl.Realized += __ownerControl_Realized;
        }
        bool __ownerControlRealized = false;
        private void __ownerControl_Realized(object sender, EventArgs e)
        {
            if (__ownerControlRealized == false)
            {
                __ownerControlRealized = true;
                Gtk.Widget parent = (Gtk.Widget)sender;
                if (__ownerControl is Gtk.Overlay lay)
                {
                    foreach (object item in this)
                    {
                        if (item is Control control)
                        {
                            if (control.Anchor != AnchorStyles.None)
                            {
                                SetMarginEnd(lay, control.Widget);
                            }
                        }
                        else if (item is Gtk.Widget widget)
                        {
                            SetMarginEnd(lay, widget);
                        }
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

        internal Drawing.Point Offset = new Drawing.Point(0,0);
        public override int Add(object item)
        {
            if (item is Control icontrol)
            {
                icontrol.Parent = __owner;
            }
            if (__ownerControl is Gtk.Overlay lay)
            {
                Gtk.Fixed fixedContainer = lay.Child as Gtk.Fixed;
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
                else if (item is Control control)
                {
                    lay.AddOverlay(control.Widget);
                    fixedContainer.WidthRequest = Math.Max(0, Math.Max(fixedContainer.WidthRequest, control.Widget.MarginStart + control.Widget.WidthRequest + control.Widget.MarginEnd));
                    fixedContainer.HeightRequest = Math.Max(0, Math.Max(fixedContainer.HeightRequest, control.Widget.MarginTop + control.Widget.HeightRequest + control.Widget.MarginBottom));
                    if (control.Anchor != AnchorStyles.None)
                    {
                        SetMarginEnd(lay, control.Widget);
                    }
                }
                else if (item is Gtk.Widget widget)
                {
                    lay.AddOverlay(widget);
                    fixedContainer.WidthRequest = Math.Max(fixedContainer.WidthRequest, widget.MarginStart + widget.WidthRequest + widget.MarginEnd);
                    fixedContainer.HeightRequest = Math.Max(fixedContainer.HeightRequest, widget.MarginTop + widget.HeightRequest + widget.MarginBottom);
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
        private void SetMarginEnd(Gtk.Overlay lay, Gtk.Widget widget)
        {
            if (widget.Halign == Align.End || widget.Halign == Align.Fill || widget.Valign == Align.End || widget.Valign == Align.Fill)
            {
                widget.MarginEnd = Math.Max(0, lay.AllocatedWidth - widget.MarginStart - widget.WidthRequest);
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

        public override void Remove(object value) {
            if(value is Control control)
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
            if(base.Contains(value))
                base.Remove(value); ;
        }
        public override void RemoveAt(int index) {
            base.RemoveAt(index);
            if (__ownerControl.Children.Length > index)
                __ownerControl.Remove(__ownerControl.Children[index]);
        }
    }
}