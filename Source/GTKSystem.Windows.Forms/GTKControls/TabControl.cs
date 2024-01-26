/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using GLib;
using Gtk;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class TabControl : WidgetControl<Gtk.Notebook>
    {
        private TabControl.ControlCollection _controls;
        private TabControl.TabPageCollection _tabPageControls;
        public TabControl() : base()
        {
            Widget.StyleContext.AddClass("TabControl");
            _controls = new ControlCollection(this);
            _tabPageControls = new TabPageCollection(this);
            base.Control.Realized += Control_Realized;
        }

        private void Control_Realized(object sender, EventArgs e)
        {
             if(SizeMode == TabSizeMode.Fixed)
            {
                foreach(TabPage page in _tabPageControls)
                {
                    page._tabLabel.WidthRequest = this.ItemSize.Width;
                    page._tabLabel.HeightRequest = this.ItemSize.Height;
                }
            }
        }

        public int SelectedIndex { get { return base.Control.CurrentPage; } set { base.Control.CurrentPage = value; } }

        public TabPage SelectedTab { get { return _controls[base.Control.CurrentPage]; } set { } }

        public TabSizeMode SizeMode { get; set; }
        public TabDrawMode DrawMode { get; set; }
        public bool ShowToolTips { get; set; }
        public Size ItemSize { get; set; }
        public int TabCount { get => base.Control.NPages; }
        public TabPageCollection TabPages { get { return _tabPageControls; } }
        public Rectangle GetTabRect(int index)
        {
            if (index < 0 || (index >= TabCount))
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, "SR.InvalidArgument");
            }
            TabPage page = TabPages[index];
            Gtk.Label tab = page.TabLabel;
            Gdk.Rectangle rect = tab.Allocation;
            return new Rectangle(0, 0, rect.Width, rect.Height);
        }

        public new TabControl.ControlCollection Controls => _controls;
        public event EventHandler SelectedIndexChanged
        {
            add { base.Control.SwitchPage += (object sender, Gtk.SwitchPageArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
            remove { base.Control.SwitchPage -= (object sender, Gtk.SwitchPageArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
        }

        public event DrawItemEventHandler DrawItem;

        public class ControlCollection : List<TabPage>
        {
            TabControl _owner;
            public ControlCollection(TabControl owner)
            {
                _owner = owner;
            }
            public new int Add(TabPage item)
            {
                item.Parent = _owner;
                item.TabLabel.Name = base.Count.ToString();
                base.Add(item);
                item.TabLabel.Drawn += (object sender, DrawnArgs args) =>
                {
                    if (_owner.DrawMode == TabDrawMode.OwnerDrawFixed && _owner.DrawItem != null)
                    {
                        Gtk.Label tab = (Gtk.Label)sender;
                        tab.GetAllocatedSize(out Gdk.Rectangle allocation, out int baseline);
                        args.Cr.ResetClip();
                        int width = allocation.Width + 24;
                        int height = allocation.Height + 2;
                       // _owner.DrawItem(this, new DrawItemEventArgs(new Graphics(tab, args.Cr, new Gdk.Rectangle(0, 0, width, height)), _owner.Font, new Rectangle(-12, -2, width, height), Convert.ToInt32(tab.Name), DrawItemState.Default));
                        _owner.DrawItem(this, new DrawItemEventArgs(new Graphics(tab, args.Cr, new Gdk.Rectangle(0, 0, width, height)) { diff_left=-12, diff_top=-2 }, _owner.Font, new Rectangle(0, 0, width, height), Convert.ToInt32(tab.Name), DrawItemState.Default));

                    }
                };
                return _owner.Control.AppendPage(item.Control, item.TabLabel);
            }
            public new void RemoveAt(int index)
            {
                base.RemoveAt(index);
                _owner.Control.RemovePage(index);
            }
            public new void Remove(TabPage value)
            {
                base.Remove(value);
                _owner.Control.Remove(((TabPage)value).Widget);
            }
        }


        public class TabPageCollection : IList, ICollection, IEnumerable
        {
            private TabControl _owner;
            public TabPageCollection(TabControl owner)
            {
                _owner = owner;
            }

            public virtual TabPage this[string key] { get { return _owner.Controls.Find(p => p.Name == key); } }
            public virtual TabPage this[int index] { get { return _owner.Controls[index]; } set { Add(value); } }

            object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            //[Browsable(false)]
            public int Count { get { return _owner.Controls.Count; } }

            public bool IsReadOnly { get { return false; } }

            bool IList.IsFixedSize => throw new NotImplementedException();

            bool ICollection.IsSynchronized => throw new NotImplementedException();

            object ICollection.SyncRoot => throw new NotImplementedException();

            public void Add(string key, string text, string imageKey)
            {
                TabPage tp = new TabPage();
                tp.Name = key;
                tp.Text = text;
                this.Add(tp);
            }

            public void Add(TabPage value)
            {
                value.Parent = _owner;
                _owner.Controls.Add(value);
            }
            public void Add(string key, string text)
            {
                this.Add(key, text, null);
            }

            public void Add(string text)
            {
                this.Add($"tabPage{Count}", text, null);
            }

            public void Add(string key, string text, int imageIndex)
            {
                this.Add(key, text, null);
            }

            int IList.Add(object value)
            {
                throw new NotImplementedException();
            }

            public void AddRange(TabPage[] pages)
            {
                foreach (TabPage page in pages)
                    this.Add(page);
            }

            bool IList.Contains(object value)
            {
                return _owner.Controls.Contains(value);
            }

            void CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            public int IndexOf(object value)
            {
                throw new NotImplementedException();
            }

            public virtual void Clear()
            {
                _owner.Controls.Clear();
            }

            public bool Contains(TabPage page)
            {
                return _owner.Controls.Contains(page);
            }

            public virtual bool ContainsKey(string key)
            {
                return _owner.Controls.FindIndex(p => p.Name == key) > -1;

            }

            public IEnumerator GetEnumerator()
            {
                return _owner.Controls.GetEnumerator();
            }

            public int IndexOf(TabPage page)
            {
                return _owner.Controls.IndexOf(page);
            }

            public virtual int IndexOfKey(string key)
            {
                return _owner.Controls.FindIndex(p => p.Name == key);
            }

            public void Insert(int index, string key, string text, int imageIndex)
            {
                _owner.Controls.Insert(index, new TabPage() { Name = key, Text = text }); ;
            }

            public void Insert(int index, string key, string text)
            {
                Insert(index, key, text, -1);
            }

            public void Insert(int index, TabPage tabPage)
            {
                _owner.Controls.Insert(index, tabPage);
            }

            public void Insert(int index, string key, string text, string imageKey)
            {
                Insert(index, key, text, -1);
            }

            public void Insert(int index, string text)
            {
                Insert(index, $"tabPage{Count}", text, -1);
            }

            public void Insert(int index, object value)
            {
                throw new NotImplementedException();
            }

            public void Remove(TabPage value)
            {
                _owner.Controls.Remove(value);
            }

            public void RemoveAt(int index)
            {
                _owner.Controls.RemoveAt(index);
            }

            public virtual void RemoveByKey(string key)
            {
                _owner.Controls.RemoveAt(_owner.Controls.FindIndex(p => p.Name == key));
            }

            void IList.Remove(object value)
            {
                throw new NotImplementedException();
            }

            void ICollection.CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }
        }
    }
}
