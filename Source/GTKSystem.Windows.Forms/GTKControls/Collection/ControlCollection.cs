/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
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
            }
            public ControlCollection(Control owner, Gtk.Container ownerContainer)
            {
                __ownerControl = ownerContainer;
                __owner = owner;
                __ownerControl.Mapped += OwnerContainer_Mapped;
                __ownerControl.Removed += __ownerControl_Removed;
            }

            private void __ownerControl_Removed(object o, RemovedArgs args)
            {
                if (o is Gtk.Overlay lay && lay.HeightRequest > 0)
                {
                    lay.WidthRequest = 1;
                    lay.HeightRequest = 1;
                    foreach (Gtk.Widget widget in lay.Children)
                    {
                        lay.WidthRequest = Math.Max(lay.WidthRequest, widget.MarginStart + widget.WidthRequest);
                        lay.HeightRequest = Math.Max(lay.HeightRequest, widget.MarginTop + widget.HeightRequest);
                    }
                }
            }

            private void OwnerContainer_Mapped(object? sender, EventArgs e)
            {
                if (sender is Gtk.Overlay lay)
                {
                    if (lay.HeightRequest > 0)
                    {
                        foreach (Gtk.Widget widget in lay.Children)
                        {
                            lay.WidthRequest = Math.Max(lay.WidthRequest, widget.MarginStart + widget.WidthRequest);
                            lay.HeightRequest = Math.Max(lay.HeightRequest, widget.MarginTop + widget.HeightRequest);
                        }
                    }
                    List<Control> _tabs = InnerList.ConvertAll<Control>(o => (Control)o);
                    _tabs.Sort(new Comparison<Control>((a, b) => { return a.TabIndex.CompareTo(b.TabIndex); }));
                    lay.ChildFocus(Gtk.DirectionType.TabForward);
                    lay.FocusChain = _tabs.Select(o => o.Widget).ToArray();
                }
            }
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
                            if (control.Widget is Gtk.Label || control.Widget is Gtk.Button || control.Widget is Gtk.Entry || control.Widget is Gtk.TextView || control.Widget is Gtk.ScrolledWindow)
                                lay.SetOverlayPassThrough(control.Widget, true);
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
                            lay2.Put(con.Widget, 0, 0);
                        }
                        else if (item is Gtk.Widget widget)
                        {
                            lay2.Put(widget, 0, 0);
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
            public virtual void Add(Gtk.Widget value)
            {
                NativeAdd(value);
            }
            public void AddWidget(Gtk.Widget item, Control control)
            {
                control.Parent = __owner;
                InnerList.Add(new ArrangedElementWidget(control));
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
                if (InnerList.Remove(value))
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
            Control _control;
            internal ArrangedElementWidget(Control control)
            {
                _widget = control.Widget;
                _control = control;
            }
            public Gtk.Widget GetWidget { get => _widget; }
            public Drawing.Rectangle Bounds => _control.Bounds;

            public Drawing.Rectangle DisplayRectangle => _control.DisplayRectangle;

            public bool ParticipatesInLayout => _control.ParticipatesInLayout;

            public PropertyStore Properties { get { PropertyStore property = new PropertyStore(); property.SetObject(_control.Handle.ToInt32(), this); return property; } }

            public IArrangedElement Container => _control.Parent;

            public ArrangedElementCollection Children => _control.Children;

            public ISite Site { get => _control.Site; set => _control.Site = value; }

            public event EventHandler Disposed;

            public void Dispose()
            {
                if (_control != null)
                {
                    _control.Dispose();
                }
            }

            public Size GetPreferredSize(Size proposedSize)
            {
                return _control.GetPreferredSize(proposedSize);
            }

            public void PerformLayout(IArrangedElement affectedElement, string propertyName)
            {
                
            }

            public void SetBounds(Drawing.Rectangle bounds, BoundsSpecified specified)
            {
                
            }
        }
    }
}