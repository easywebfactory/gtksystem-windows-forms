/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
    public partial class Control
    {
        public partial class ControlCollection : ArrangedElementCollection, IList, ICloneable
        {
            Gtk.Container __ownerControl;
            Control __owner;
            Type __itemType;
            public ControlCollection(Control owner)
            {
                __ownerControl = owner.GtkControl as Gtk.Container;
                __owner = owner;
                __ownerControl.Mapped += __ownerControl_Mapped;
                __ownerControl.ResizeChecked += __ownerControl_ResizeChecked;
            }

            public ControlCollection(Control owner, Gtk.Container ownerContainer)
            {
                __ownerControl = ownerContainer;
                __owner = owner;
                __ownerControl.Mapped += __ownerControl_Mapped;
                __ownerControl.ResizeChecked += __ownerControl_ResizeChecked;
            }
            //ResizeChecked可重置布局
            private void __ownerControl_ResizeChecked(object sender, EventArgs e)
            {
                if (sender is Gtk.Overlay lay)
                {
                    ResizeMapped(lay);
                }
            }

            private bool is__ownerControl_Mapped = false;
            private void __ownerControl_Mapped(object sender, EventArgs e)
            {
                if (is__ownerControl_Mapped == false)
                {
                    is__ownerControl_Mapped = true;
                    if (sender is Gtk.Overlay lay)
                    {
                        ResizeMapped(lay);
                    }
                }
            }
            private void ResizeMapped(Gtk.Overlay lay)
            {
                foreach (object item in this)
                {
                    if (item is Control control)
                    {
                        control.Widget.MarginStart = Math.Max(0, control.Widget.MarginStart + Offset.X);
                        control.Widget.MarginTop = Math.Max(0, control.Widget.MarginTop + Offset.Y);
                    }
                    else if (item is Gtk.Widget widget)
                    {
                        widget.MarginStart = Math.Max(0, widget.MarginStart + Offset.X);
                        widget.MarginTop = Math.Max(0, widget.MarginTop + Offset.Y);
                    }
                }
                foreach (object item in this)
                {
                    if (item is Control control)
                    {
                        SetMarginEnd(lay, control);
                    }
                }
            }

            internal Drawing.Point Offset = new Drawing.Point(0, 0);

            private void NativeAdd(object item)
            {
                try
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
                                Gtk.Overlay overlay = new Gtk.Overlay();
                                overlay.HeightRequest = statusbar.Height;
                                overlay.AddOverlay(statusbar.self);
                                form.self.ContentArea.PackEnd(overlay, false, false, 0);
                            }
                        }
                        else if (item is Control control)
                        {
                            lay.AddOverlay(control.Widget);
                            control.DockChanged += Control_DockChanged;
                            control.AnchorChanged += Control_AnchorChanged;
                            SetMarginEnd(lay, control);
                        }
                        else if (item is Gtk.Widget widget)
                        {
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
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (__ownerControl.IsRealized)
                    {
                        if (item is Control con)
                            con.Widget.ShowAll();
                        else if (item is Gtk.Widget widget)
                            widget.ShowAll();
                    }
                }
            }
            private void NativeAdd()
            {
                try
                {
                    foreach (object item in this)
                    {
                        NativeAdd(item);
                    }
                }
                catch
                {
                    throw;
                }
            }

            private void Control_AnchorChanged(object sender, EventArgs e)
            {
                Control control = sender as Control;
                if (control.Widget.Parent is Gtk.Overlay lay)
                {
                    SetMarginEnd(lay, control);
                }
            }
            private void Control_DockChanged(object sender, EventArgs e)
            {
                Control control = sender as Control;
                if (control.Widget.Parent is Gtk.Overlay lay)
                {
                    SetMarginEnd(lay, control);
                }
            }
            private void SetMarginEnd(Gtk.Overlay lay, Control control)
            {
                if (__owner is Form)
                {
                    lay.WidthRequest = Math.Max(-1, Math.Max(lay.Parent.Parent.AllocatedWidth, control.Location.X + control.Width));
                    lay.HeightRequest = Math.Max(-1, Math.Max(lay.Parent.Parent.AllocatedHeight, control.Location.Y + control.Height));
                }
                else
                {
                    lay.WidthRequest = Math.Max(-1, Math.Max(__owner.Width - 4, control.Location.X + control.Width));
                    lay.HeightRequest = Math.Max(-1, Math.Max(__owner.Height - 4, control.Location.Y + control.Height));
                }
                if (lay.IsMapped == true)
                {
                    Gtk.Widget widget = control.Widget;
                    if (widget.Halign == Gtk.Align.End)
                    {
                        if (widget.WidthRequest > 0)
                            widget.MarginEnd = Math.Max(0, lay.AllocatedWidth - widget.MarginStart - widget.WidthRequest);
                        else
                            widget.MarginEnd = 0;
                    }
                    else if (widget.Halign == Gtk.Align.Fill)
                    {
                        if (control.Dock == DockStyle.Fill)
                            widget.MarginEnd = 0;
                        else if (widget.WidthRequest > 0)
                            widget.MarginEnd = Math.Max(control.Padding.Right, lay.AllocatedWidth - widget.MarginStart - widget.WidthRequest);
                        else
                            widget.MarginEnd = 0;
                    }
                    if (widget.Valign == Gtk.Align.End)
                    {
                        if (widget.HeightRequest > 0)
                            widget.MarginBottom = Math.Max(0, lay.AllocatedHeight - widget.MarginTop - widget.HeightRequest);
                        else
                            widget.MarginBottom = 0;
                    }
                    else if (widget.Valign == Gtk.Align.Fill)
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
            private Gtk.Widget GetFrame(Gtk.Widget widget)
            {
                Gtk.Widget parent = widget.Parent;
                while (parent != null)
                {
                    if (parent is GTKSystem.Windows.Forms.GTKControls.ControlBase.IControlGtk)
                    {
                        return parent;
                    }
                    else
                        parent = parent.Parent;
                }
                return null;
            }
            public virtual void Add(Gtk.Widget value)
            {
                NativeAdd(value);
            }
            public void AddWidget(Gtk.Widget item, Control control)
            {
                control.Parent = __owner;
                InnerList.Add(new ArrangedElementWidget(item));
            }
            public virtual void Add(Type itemType, Control item)
            {
                //重载处理
                this.Add(item);
            }


            public object Clone()
            {
                ControlCollection ccOther = new ControlCollection(__owner, __ownerControl);
                ccOther.InnerList.AddRange(InnerList);
                return ccOther;
            }

            public virtual bool ContainsKey(string? key)
            {
                return IsValidIndex(IndexOfKey(key));
            }

            public virtual void Add(Control? value)
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
                else
                {
                    throw new ArgumentException("ControlBadControl {0}", nameof(control));
                }
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
                foreach (Control item in controls)
                {
                    Add(item);
                }
            }

            object ICloneable.Clone()
            {
                ControlCollection ccOther = new ControlCollection(__owner, __ownerControl);
                ccOther.InnerList.AddRange(InnerList);
                return ccOther;
            }

            public bool Contains(Control? control) => ((IList)InnerList).Contains(control);

            public Control[] Find(string key, bool searchAllChildren)
            {
                List<IArrangedElement> foundControls = InnerList.FindAll(o =>
                {
                    if (o is Control con)
                    {
                        return con.Name == key;
                    }
                    else if (o is ArrangedElementWidget widget)
                    {
                        return widget.GetWidget?.Name == key;
                    }
                    else { return false; }
                });
                if (foundControls == null)
                    return new Control[0];
                else
                    return foundControls.ConvertAll(o => o as Control).ToArray();
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
                        return con.Name == key;
                    }
                    else if (o is ArrangedElementWidget widget)
                    {
                        return widget.GetWidget?.Name == key;
                    }
                    else { return false; }
                });
            }

            private bool IsValidIndex(int index)
            {
                return ((index >= 0) && (index < Count));
            }

            public Control Owner { get => __owner; }

            public virtual void Remove(Control? value)
            {
                if (value is null)
                {
                    return;
                }
                InnerList.Remove(value);
                __ownerControl.Remove(value.Widget);
            }

            void IList.Remove(object? element)
            {
                if (element is Control control)
                    Remove(control);
                else if (element is Gtk.Widget widget)
                {
                    __ownerControl.Remove(widget);
                    int index = this.IndexOfKey(widget.Name);
                    if (index >= 0)
                        InnerList.RemoveAt(index);
                }
            }

            public void RemoveAt(int index)
            {
                IArrangedElement element = InnerList[index];
                if (element is Control control)
                    Remove(control);
                else if (element is ArrangedElementWidget widget)
                {
                    InnerList.RemoveAt(index);
                    __ownerControl.Remove(widget.GetWidget);
                }
            }

            public virtual void RemoveByKey(string? key)
            {
                int index = IndexOfKey(key);
                if (IsValidIndex(index))
                {
                    RemoveAt(index);
                }
            }

            public new virtual Control this[int index]
            {
                get
                {
                    Control control = (Control)InnerList[index]!;
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

                    int index = IndexOfKey(key);
                    if (IsValidIndex(index))
                    {
                        return this[index];
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            public virtual void Clear()
            {
                foreach (Gtk.Widget wid in __ownerControl.Children)
                    __ownerControl.Remove(wid);

                InnerList.Clear();
            }

            public int GetChildIndex(Control child) => GetChildIndex(child, true);

            public virtual int GetChildIndex(Control child, bool throwException)
            {
                int index = IndexOf(child);
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
            Gtk.Widget _widget;
            internal ArrangedElementWidget(Gtk.Widget widget)
            {
                _widget = widget;
            }
            public Gtk.Widget GetWidget { get => _widget; }
            public Rectangle Bounds => throw new NotImplementedException();

            public Rectangle DisplayRectangle => throw new NotImplementedException();

            public bool ParticipatesInLayout => throw new NotImplementedException();

            public PropertyStore Properties => throw new NotImplementedException();

            public IArrangedElement Container => throw new NotImplementedException();

            public ArrangedElementCollection Children => throw new NotImplementedException();

            public ISite Site { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public event EventHandler Disposed;

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
                throw new NotImplementedException();
            }

            public void PerformLayout(IArrangedElement affectedElement, string propertyName)
            {
                throw new NotImplementedException();
            }

            public void SetBounds(Rectangle bounds, BoundsSpecified specified)
            {
                throw new NotImplementedException();
            }
        }
    }
}