/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using Gtk;
using Container = Gtk.Container;

namespace System.Windows.Forms;

public partial class Control
{
    public class ControlCollection : ArrangedElementCollection, IList, ICloneable
    {
        readonly Container? ownerControl;
        readonly Control? owner;
        public ControlCollection(Control? owner)
        {
            ownerControl = owner?.GtkControl as Container;
            this.owner = owner;
            if (ownerControl != null)
            {
                ownerControl.Mapped += __ownerControl_Mapped;
                ownerControl.ResizeChecked += __ownerControl_ResizeChecked;
            }
        }

        public ControlCollection(Control? owner, Container? ownerContainer)
        {
            ownerControl = ownerContainer;
            this.owner = owner;
            if (ownerControl != null)
            {
                ownerControl.Mapped += __ownerControl_Mapped;
                ownerControl.ResizeChecked += __ownerControl_ResizeChecked;
            }
        }
        //ResizeChecked可重置布局
        private void __ownerControl_ResizeChecked(object? sender, EventArgs e)
        {
            if (sender is Overlay lay)
            {
                ResizeMapped(lay);
            }
        }

        private bool isOwnerControlMapped;
        private void __ownerControl_Mapped(object? sender, EventArgs e)
        {
            if (isOwnerControlMapped == false)
            {
                isOwnerControlMapped = true;
                if (sender is Overlay lay)
                {
                    ResizeMapped(lay);
                }
            }
        }
        private void ResizeMapped(Overlay lay)
        {
            foreach (var item in this)
            {
                if (item is Control { Widget: not null } control)
                {
                    control.Widget.MarginStart = Math.Max(0, control.Widget.MarginStart + offset.X);
                    control.Widget.MarginTop = Math.Max(0, control.Widget.MarginTop + offset.Y);
                }
                else if (item is Widget widget)
                {
                    widget.MarginStart = Math.Max(0, widget.MarginStart + offset.X);
                    widget.MarginTop = Math.Max(0, widget.MarginTop + offset.Y);
                }
            }
            foreach (var item in this)
            {
                if (item is Control control)
                {
                    SetMarginEnd(lay, control);
                }
            }
        }

        internal Point offset = new(0, 0);

        private void NativeAdd(object? item)
        {
            try
            {
                if (item is Control icontrol)
                {
                    icontrol.Parent = owner;
                }
                if (ownerControl is Overlay lay)
                {
                    if (item is StatusStrip statusbar)
                    {
                        if (owner is Form form)
                        {
                            statusbar.self.Halign = Align.Fill;
                            statusbar.self.Valign = Align.Start;
                            statusbar.self.Expand = false;
                            statusbar.self.MarginStart = 0;
                            statusbar.self.MarginTop = 0;
                            statusbar.self.MarginEnd = 0;
                            statusbar.self.MarginBottom = 0;
                            var overlay = new Overlay();
                            overlay.HeightRequest = statusbar.Height;
                            overlay.AddOverlay(statusbar.self);
                            form.self.ContentArea.PackEnd(overlay, false, false, 0);
                        }
                    }
                    else if (item is Control control)
                    {
                        if (control.Widget is Widget widget) lay.AddOverlay(widget);
                        control.DockChanged += Control_DockChanged;
                        control.AnchorChanged += Control_AnchorChanged;
                        SetMarginEnd(lay, control);
                    }
                    else if (item is Widget widget)
                    {
                        lay.AddOverlay(widget);
                    }
                }
                else if (ownerControl is Fixed lay2)
                {
                    if (item is Control con)
                    {
                        if (con.Widget is Widget widget) lay2.Put(widget, offset.X, offset.Y);
                    }
                    else if (item is Widget widget)
                    {
                        lay2.Put(widget, offset.X, offset.Y);
                    }
                }
                else if (ownerControl is Layout lay3)
                {
                    if (item is Control con)
                    {
                        if (con.Widget is Widget widget) lay3.Put(widget, offset.X, offset.Y);
                    }
                    else if (item is Widget widget)
                    {
                        lay3.Put(widget, offset.X, offset.Y);
                    }
                }
            }
            finally
            {
                if (ownerControl?.IsRealized??false)
                {
                    if (item is Control con)
                        con.Widget.ShowAll();
                    else if (item is Widget widget)
                        widget.ShowAll();
                }
            }
        }

        private void Control_AnchorChanged(object? sender, EventArgs e)
        {
            var control = sender as Control;
            if (control?.Widget.Parent is Overlay lay)
            {
                SetMarginEnd(lay, control);
            }
        }
        private void Control_DockChanged(object? sender, EventArgs e)
        {
            var control = sender as Control;
            if (control?.Widget.Parent is Overlay lay)
            {
                SetMarginEnd(lay, control);
            }
        }
        private void SetMarginEnd(Overlay lay, Control control)
        {
            if (lay.IsMapped)
            {
                var widget = control.Widget as Widget;
                if (widget is { Halign: Align.End })
                {
                    if (widget.WidthRequest > 0)
                        widget.MarginEnd = Math.Max(0, lay.AllocatedWidth - widget.MarginStart - widget.WidthRequest);
                    else
                        widget.MarginEnd = 0;
                }
                else if (widget is { Halign: Align.Fill })
                {
                    if (control.Dock == DockStyle.Fill)
                        widget.MarginEnd = 0;
                    else if (widget.WidthRequest > 0)
                        widget.MarginEnd = Math.Max(control.Padding.Right, lay.AllocatedWidth - widget.MarginStart - widget.WidthRequest);
                    else
                        widget.MarginEnd = 0;
                }
                if (widget is { Valign: Align.End })
                {
                    if (widget.HeightRequest > 0)
                        widget.MarginBottom = Math.Max(0, lay.AllocatedHeight - widget.MarginTop - widget.HeightRequest);
                    else
                        widget.MarginBottom = 0;
                }
                else if (widget is { Valign: Align.Fill })
                {
                    if (control.Dock == DockStyle.Fill)
                        widget.MarginBottom = 0;
                    else if (widget.HeightRequest > 0)
                        widget.MarginBottom = Math.Max(control.Padding.Bottom, lay.AllocatedHeight - widget.MarginTop - widget.HeightRequest);
                    else
                        widget.MarginBottom = 0;
                }
            }
        }

        public virtual void Add(Widget value)
        {
            NativeAdd(value);
        }
        public void AddWidget(Widget? item, Control control)
        {
            control.Parent = owner;
            InnerList.Add(new ArrangedElementWidget(item));
        }
        public virtual void Add(Type itemType, Control item)
        {
            //重载处理
            Add(item);
        }


        public object Clone()
        {
            var ccOther = new ControlCollection(owner, ownerControl);
            ccOther.InnerList.AddRange(InnerList);
            return ccOther;
        }

        public virtual bool ContainsKey(string? key)
        {
            return IsValidIndex(IndexOfKey(key));
        }

        public virtual void Add(Control value)
        {
            if (value is null)
            {
                return;
            }
            NativeAdd(value);
            InnerList.Add(value);
        }

        int IList.Add(object? control)
        {
            if (control is Control c)
            {
                Add(c);
                return IndexOf(c);
            }

            throw new ArgumentException(@"ControlBadControl {0}", nameof(control));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual void AddRange(params Control[] controls)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (controls.Length == 0)
            {
                return;
            }
            foreach (var item in controls)
            {
                Add(item);
            }
        }

        object ICloneable.Clone()
        {
            var ccOther = new ControlCollection(owner, ownerControl);
            ccOther.InnerList.AddRange(InnerList);
            return ccOther;
        }

        public bool Contains(Control? control) => ((IList)InnerList).Contains(control);

        public Control?[]? Find(string key, bool searchAllChildren)
        {
            if(string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            var foundControls = InnerList.FindAll(o =>
            {
                if (o is Control con)
                {
                    return string.Equals(con.Name, key, StringComparison.InvariantCultureIgnoreCase);
                }

                if (o is ArrangedElementWidget widget)
                {
                    return string.Equals(widget.GetWidget?.Name, key, StringComparison.InvariantCultureIgnoreCase);
                }
                return false;
            });
            List<Control?> controls= [];
            if (foundControls != null)
            {
                controls = foundControls.ConvertAll(o => o as Control).ToList();
            }

            if (searchAllChildren)
            {
                foreach (var arrangedElement in InnerList)
                {
                    var element = arrangedElement as Control;
                    if (element != null)
                    {
                        var collection = element.Controls?.Find(key, true);
                        if (collection != null)
                        {
                            controls.AddRange(collection);
                        }
                    }
                }
            }
            return controls?.ToArray();
        }

        public override IEnumerator GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        public int IndexOf(Control? control) => ((IList)InnerList).IndexOf(control);

        public virtual int IndexOfKey(string? key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return -1;
            }
            return InnerList.FindIndex(o =>
            {
                if (o is Control con)
                {
                    return string.Equals(con.Name, key, StringComparison.InvariantCultureIgnoreCase);
                }

                if (o is ArrangedElementWidget widget)
                {
                    return string.Equals(widget.GetWidget?.Name, key, StringComparison.InvariantCultureIgnoreCase);
                }
                return false;
            });
        }

        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < Count;
        }

        public Control? Owner => owner;

        public virtual void Remove(Control? value)
        {
            if (value is null)
            {
                return;
            }
            InnerList.Remove(value);
            if (value.Widget is Widget widget)
                ownerControl?.Remove(widget);
        }

        void IList.Remove(object? element)
        {
            if (element is Control control)
                Remove(control);
            else if (element is Widget widget)
            {
                ownerControl?.Remove(widget);
                var index = IndexOfKey(widget.Name);
                if (index >= 0)
                    InnerList.RemoveAt(index);
            }
        }

        public void RemoveAt(int index)
        {
            var element = InnerList[index];
            if (element is Control control)
                Remove(control);
            else if (element is ArrangedElementWidget widget)
            {
                InnerList.RemoveAt(index);
                ownerControl?.Remove(widget.GetWidget);
            }
        }

        public virtual void RemoveByKey(string? key)
        {
            var index = IndexOfKey(key);
            if (IsValidIndex(index))
            {
                RemoveAt(index);
            }
        }

        public new virtual Control this[int index]
        {
            get
            {
                var control = (Control)InnerList[index]!;
                return control;
            }
        }

        public virtual Control? this[string? key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                {
                    return null;
                }

                var index = IndexOfKey(key);
                if (IsValidIndex(index))
                {
                    return this[index];
                }

                return null;
            }
        }

        public virtual void Clear()
        {
            if (ownerControl != null)
            {
                foreach (var wid in ownerControl.Children)
                    ownerControl.Remove(wid);
            }

            InnerList.Clear();
        }

        public int GetChildIndex(Control child) => GetChildIndex(child, true);

        public virtual int GetChildIndex(Control child, bool throwException)
        {
            var index = IndexOf(child);
            if (index == -1 && throwException)
            {
                throw new ArgumentException("ControlNotChild");
            }

            return index;
        }

        internal virtual void SetChildIndexInternal(Control child, int newIndex)
        {

        }

        public virtual void SetChildIndex(Control child, int newIndex) => SetChildIndexInternal(child, newIndex);
    }

    internal class ArrangedElementWidget : IArrangedElement
    {
        Widget? _widget;
        internal ArrangedElementWidget(Widget? widget)
        {
            _widget = widget;
        }
        public Widget? GetWidget => _widget;
        public Rectangle Bounds => throw new NotImplementedException();

        public Rectangle DisplayRectangle => throw new NotImplementedException();

        public bool ParticipatesInLayout => throw new NotImplementedException();

        public PropertyStore Properties => throw new NotImplementedException();

        public IArrangedElement Container => throw new NotImplementedException();

        public ArrangedElementCollection Children => throw new NotImplementedException();

        public ISite Site { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler? Disposed;

        public void Dispose()
        {
            if (_widget != null)
            {
                _widget.Dispose();
                _widget = null;
            }
        }

        public Size GetPreferredSize(Size proposedSize)
        {
            return proposedSize;
        }

        public void PerformLayout(IArrangedElement affectedElement, string? propertyName)
        {
        }

        public void SetBounds(Rectangle bounds, BoundsSpecified specified)
        {
        }
    }
}