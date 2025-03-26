/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public class TabControl : ContainerControl
{
    public readonly TabControlBase self = new();
    public override object GtkControl => self;
    private readonly ControlCollection _controls;
    private readonly TabPageCollection _tabPageControls;
    public TabControl()
    {
        _controls = new ControlCollection(this);
        _tabPageControls = new TabPageCollection(this);
        self.SwitchPage += Self_SwitchPage;
    }
    private void Self_SwitchPage(object? o, SwitchPageArgs args)
    {
        if (SelectedIndexChanged != null && self.IsMapped)
            OnSelectedIndexChanged(EventArgs.Empty);
    }

    protected virtual void OnSelectedIndexChanged(EventArgs e)
    {
        SelectedIndexChanged?.Invoke(this, e);
    }

    /// <summary>
    /// gtk Unique menu function for use when needed
    /// </summary>
    [Browsable(false)]
    public bool EnablePopup
    {
        set { self.EnablePopup = true; self.PopupEnable(); }
    }
    public TabAlignment Alignment
    {
        get
        {
            if (self.TabPos == PositionType.Left)
                return TabAlignment.Left;
            if (self.TabPos == PositionType.Top)
                return TabAlignment.Top;
            if (self.TabPos == PositionType.Right)
                return TabAlignment.Right;
            if (self.TabPos == PositionType.Bottom)
                return TabAlignment.Bottom;
            return TabAlignment.Top;
        }
        set
        {
            if (value == TabAlignment.Left)
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
    public int SelectedIndex
    {
        get => self.CurrentPage;
        set => self.CurrentPage = value;
    }

    public TabPage SelectedTab
    {
        get => _controls[self.CurrentPage];
        set { }
    }

    public TabSizeMode SizeMode { get; set; }
    public TabDrawMode DrawMode { get; set; }
    public bool ShowToolTips { get; set; }
    public bool ShowTabs { get => self.ShowTabs; set => self.ShowTabs = value; }
    public Size ItemSize { get; set; }
    public int TabCount => self.NPages;
    public TabPageCollection TabPages => _tabPageControls;

    public Rectangle GetTabRect(int index)
    {
        if (index < 0 || index >= TabCount)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        var page = TabPages[index];
        var tab = page.TabLabel;
        var rect = tab.Allocation;
        return new Rectangle(0, 0, rect.Width, rect.Height);
    }

    public new ControlCollection Controls => _controls;
    public event EventHandler? SelectedIndexChanged;

    public event DrawItemEventHandler? DrawItem;

    public new class ControlCollection : List<TabPage>
    {
        readonly TabControl _owner;
        public ControlCollection(TabControl owner)
        {
            _owner = owner;
            _owner.self.Shown += Self_Shown;
        }

        private void Self_Shown(object? sender, EventArgs e)
        {
            foreach (var item in this)
            {
                if (_owner != null)
                {
                    _owner.self.SetMenuLabelText(item.self, _owner.self.GetTabLabelText(item.self));
                }
            }
        }

        public new int Add(TabPage item)
        {
            try
            {
                item.Parent = _owner;
                item.TabLabel.Name = Count.ToString();
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
                item.TabLabel.Drawn += (sender, args) =>
                {
                    if (_owner.DrawMode == TabDrawMode.OwnerDrawFixed && _owner.DrawItem != null)
                    {
                        var tab = (Gtk.Label)sender;
                        tab.GetAllocatedSize(out var allocation, out _);
                        args.Cr.ResetClip();
                        var width = allocation.Width + 24;
                        var height = allocation.Height + 2;
                        OnDrawItem(new DrawItemEventArgs(new Graphics(tab, args.Cr, new Gdk.Rectangle(0, 0, width, height)) { DiffLeft = -12, DiffTop = -2 }, _owner.Font, new Rectangle(0, 0, width, height), Convert.ToInt32(tab.Name), DrawItemState.Default));

                    }
                };
                return _owner.self.AppendPage(item.self, item.TabLabel);
            }
            finally
            {
                _owner.self.SetTabReorderable(item.self, true);
                if (_owner.Widget.IsRealized)
                    item.Widget.ShowAll();
            }
        }

        protected virtual void OnDrawItem(DrawItemEventArgs e)
        {
            _owner.DrawItem?.Invoke(this, e);
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            _owner.self.RemovePage(index);
        }
        public new void Remove(TabPage value)
        {
            base.Remove(value);
            if (value.Widget is Widget widget) _owner.self.Remove(widget);
        }
    }


    public class TabPageCollection : IList
    {
        private readonly TabControl _owner;
        public TabPageCollection(TabControl owner)
        {
            _owner = owner;
        }

        public virtual TabPage this[string key] { get { return _owner.Controls.Find(p => p.Name == key); } }
        public virtual TabPage this[int index]
        {
            get => _owner.Controls[index];
            set => Add(value);
        }

        object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //[Browsable(false)]
        public int Count => _owner.Controls.Count;

        public bool IsReadOnly => false;

        bool IList.IsFixedSize => throw new NotImplementedException();

        bool ICollection.IsSynchronized => throw new NotImplementedException();

        object ICollection.SyncRoot => throw new NotImplementedException();

        public void Add(string? key, string? text, string? imageKey)
        {
            var tp = new TabPage();
            tp.Name = key;
            tp.Text = text??string.Empty;
            Add(tp);
        }

        public void Add(TabPage value)
        {
            value.Parent = _owner;
            _owner.Controls.Add(value);
        }
        public void Add(string? key, string? text)
        {
            Add(key, text, null);
        }

        public void Add(string? text)
        {
            Add($"tabPage{Count}", text, null);
        }

        public void Add(string? key, string? text, int imageIndex)
        {
            Add(key, text, null);
        }

        int IList.Add(object? value)
        {
            throw new NotImplementedException();
        }

        public void AddRange(TabPage[] pages)
        {
            foreach (var page in pages)
                Add(page);
        }

        bool IList.Contains(object? value)
        {
            throw new NotImplementedException();
        }

        void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object? value)
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
            _owner.Controls.Insert(index, new TabPage { Name = key, Text = text }); 
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

        void IList.Remove(object? value)
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
    }
}