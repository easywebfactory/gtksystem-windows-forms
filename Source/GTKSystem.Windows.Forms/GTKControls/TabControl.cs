/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class TabControl : ContainerControl
    {
        public readonly TabControlBase self = new TabControlBase();
        public override object GtkControl => self;
        private TabControl.ControlCollection _controls;
        private TabControl.TabPageCollection _tabPageControls;
        public TabControl() : base()
        {
            _controls = new ControlCollection(this);
            _tabPageControls = new TabPageCollection(this);
            self.SwitchPage += Self_SwitchPage;
        }
        private void Self_SwitchPage(object o, SwitchPageArgs args)
        {
            if (SelectedIndexChanged != null && self.IsMapped)
                SelectedIndexChanged(this, new EventArgs());
        }
        public TabAlignment Alignment {
            get
            {
                if (self.TabPos == PositionType.Left)
                    return TabAlignment.Left;
                else if (self.TabPos == PositionType.Top)
                    return TabAlignment.Top;
                else if (self.TabPos == PositionType.Right)
                    return TabAlignment.Right;
                else if (self.TabPos == PositionType.Bottom)
                    return TabAlignment.Bottom;
                else
                    return TabAlignment.Top;
            }
            set {
                if(value == TabAlignment.Left)
                    self.TabPos = PositionType.Left;
                else if (value == TabAlignment.Top)
                    self.TabPos = PositionType.Top;
                else if (value == TabAlignment.Right)
                    self.TabPos = PositionType.Right;
                else if (value == TabAlignment.Bottom)
                    self.TabPos = PositionType.Bottom;
            }
        }
        public bool Multiline { get; set; }
        public int SelectedIndex { get { return self.CurrentPage; } set { self.CurrentPage = value; } }

        public TabPage SelectedTab { get { return _controls[self.CurrentPage]; } set { } }

        public TabSizeMode SizeMode { get; set; }
        public TabDrawMode DrawMode { get; set; }
        public bool ShowToolTips { get; set; }
        public bool ShowTabs { get => self.ShowTabs; set => self.ShowTabs = value; }
        public Size ItemSize { get; set; }
        public int TabCount { get => self.NPages; }
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
        public event EventHandler SelectedIndexChanged;

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
                try
                {
                    item.Parent = _owner;
                    item.TabLabel.Name = base.Count.ToString();
                    item._tabLabel.WidthRequest = _owner.ItemSize.Width;
                    item._tabLabel.HeightRequest = _owner.ItemSize.Height;
                    if (_owner.SizeMode == TabSizeMode.Fixed)
                    {
                        item._tabLabel.Ellipsize = Pango.EllipsizeMode.End;
                    }
                    else if (_owner.SizeMode == TabSizeMode.FillToRight)
                    {
                        item._tabLabel.Halign = Align.End;
                    }
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
                            _owner.DrawItem(this, new DrawItemEventArgs(new Graphics(tab, args.Cr, new Gdk.Rectangle(0, 0, width, height)) { diff_left = -12, diff_top = -2 }, _owner.Font, new Rectangle(0, 0, width, height), Convert.ToInt32(tab.Name), DrawItemState.Default));

                        }
                    };
                    return _owner.self.AppendPage(item.self, item.TabLabel);
                }
                finally
                {
                    _owner.self.SetTabReorderable(item.self, true);
                }
            }
            public new void RemoveAt(int index)
            {
                base.RemoveAt(index);
                _owner.self.RemovePage(index);
            }
            public new void Remove(TabPage value)
            {
                base.Remove(value);
                _owner.self.Remove(((TabPage)value).Widget);
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
                throw new NotImplementedException();
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