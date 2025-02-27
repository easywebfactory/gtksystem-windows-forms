/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using GLib;
using Gtk;
using System.Collections;
using System.ComponentModel;
using System.Data;
using Timeout = GLib.Timeout;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
[DefaultEvent("SelectedIndexChanged")]
[DefaultProperty("Items")]
[DefaultBindingProperty("SelectedValue")]
public partial class ListBox : ListControl
{
    public readonly ListBoxBase self = new();
    public override object GtkControl => self;
    public override IGtkControl Self => self;

    protected override void SetStyle(Widget widget)
    {
        base.SetStyle(self.listBox);
    }
    private readonly ControlBindingsCollection? _collect;
    private readonly ObjectCollection _items;

    public ListBox()
    {
        self.listBox.Halign = Align.Fill;
        self.listBox.Valign = Align.Fill;
        self.listBox.Hexpand = true;
        self.listBox.Vexpand = true;
        _collect = new ControlBindingsCollection(this);
        _items = new ObjectCollection(this);
        self.listBox.Realized += Self_Realized;
        self.listBox.SelectedRowsChanged += ListBox_SelectedRowsChanged;
        BorderStyle = BorderStyle.Fixed3D;
    }
    private void ListBox_SelectedRowsChanged(object? sender, EventArgs e)
    {
        if (self.listBox.IsVisible)
        {
            ((EventHandler)events["SelectedIndexChanged"])?.Invoke(this, e);
            ((EventHandler)events["SelectedValueChanged"])?.Invoke(this, e);
            ((EventHandler)events["SelectedItemChanged"])?.Invoke(this, e);
        }
    }

    private void Self_Realized(object? sender, EventArgs e)
    {
        OnSetDataSource();
        if (DataBindings != null)
        {
            foreach (Binding binding in DataBindings)
                self.listBox.AddNotification(binding.PropertyName, PropertyNotity);
        }
    }
    private void PropertyNotity(object? o, NotifyArgs args)
    {
        if (DataBindings != null)
        {
            var binding = DataBindings[args.Property];
            binding?.WriteValue();
        }
    }
    #region listcontrol
    private object? dataSource;
    public override object? DataSource
    {
        get => dataSource;
        set {
            dataSource = value;
            if (self.listBox.IsVisible)
            {
                OnSetDataSource();
            }
        }
    }
    private void OnSetDataSource()
    {
        if (dataSource != null)
        {
            if (dataSource is IListSource listSource)
            {
                var list = listSource.GetList().GetEnumerator();
                using var disposable = list as IDisposable;
                SetDataSource(list);
            }
            else if (dataSource is IEnumerable list1)
            {
                var enumerator = list1.GetEnumerator();
                using var disposable = enumerator as IDisposable;
                SetDataSource(enumerator);
            }
        }
    }
    private void SetDataSource(IEnumerator enumerator)
    {
        _items.Clear();
        if (enumerator != null)
        {
            if (string.IsNullOrWhiteSpace(DisplayMember))
            {
                while (enumerator.MoveNext())
                {
                    var o = enumerator.Current;
                    if (o is DataRowView row)
                        _items.Add(row[0]);
                    else
                        _items.Add(enumerator.Current);
                }
            }
            else
            {
                while (enumerator.MoveNext())
                {
                    var o = enumerator.Current;
                    if (o is DataRowView row)
                        _items.Add(row[DisplayMember]);
                    else
                        _items.Add(o?.GetType().GetProperty(DisplayMember)?.GetValue(o));
                }
            }
        }
    }

    public override int SelectedIndex
    {
        get
        {
            object listBoxSelectedRow = self.listBox.SelectedRow;
            return listBoxSelectedRow == null ? -1 : self.listBox.SelectedRow.Index;
        }
        set => SelectedItems.SetSelected(value, true);
    }

    [DefaultValue(null)]
    [Browsable(false)]
    public override object? SelectedValue
    {
        get
        {
            var index = SelectedIndex;
            return index == -1 ? null : _items[SelectedIndex];
        }
        set
        {
            ClearSelected();
            var index = _items.IndexOf(value);
            SelectedItems.SetSelected(index, true);
        }
    }

    [Browsable(false)]
    [Bindable(true)]
    public object? SelectedItem
    {
        get
        {
            var index = SelectedIndex;
            return index == -1 ? null : _items[SelectedIndex];
        }
        set
        {
            ClearSelected();
            var index = _items.IndexOf(value);
            SelectedItems.SetSelected(index, true);
        }
    }
    public override string GetItemText(object? item)
    {
        if(item is ItemArray.Entry entry)
        {
            return entry.Item?.ToString()??string.Empty;
        }
        return item?.ToString() ?? string.Empty;
    }
    protected void NativeInsert(int index, object? item)
    {
        var row = new ListBoxRow();
        row.HeightRequest = ItemHeight > 0 ? ItemHeight : defaultItemHeight;
        row.Add(new Gtk.Label(item?.ToString()) { Valign = Align.Center, Halign = Align.Start, Expand = true });
        self.listBox.Insert(row, index);
        if (self.listBox.IsVisible && !isUpdateing)
        {
            self.listBox.ShowAll();
        }
    }
    protected void NativeAdd(object? item)
    {
        var row = new ListBoxRow();
        row.HeightRequest = ItemHeight > 0 ? ItemHeight : defaultItemHeight;
        row.Add(new Gtk.Label(item?.ToString()) { Valign = Align.Center, Halign = Align.Start, Expand = true });
        self.listBox.Add(row);
        if (self.listBox.IsVisible && !isUpdateing)
        {
            self.listBox.ShowAll();
        }
    }
    protected void NativeClear()
    {
        var count = self.listBox.Children.Length;
        while (count > 0)
        {
            self.listBox.Remove(self.listBox.GetRowAtIndex(count - 1));
            count--;
            //System.Threading.Thread.Sleep(3);
        }
    }
    protected void NativeRemoveAt(int index)
    {
        self.listBox.Remove(self.listBox.GetRowAtIndex(index));
    }
    protected string NativeGetItemText(int index)
    {
        var row = self.listBox.GetRowAtIndex(index).Child as Gtk.Label;
        return row?.Text ?? string.Empty;
    }
    protected void OnSelectedIndexChanged(EventArgs e) {
        if (self.listBox.SelectedRow != null)
            self.listBox.SelectRow(self.listBox.SelectedRow);
    }
    #endregion
    public override ControlBindingsCollection? DataBindings => _collect;
    internal bool ShowCheckBox { get; set; }
    internal bool ShowImage { get; set; }

    public const int noMatches = -1;

    public const int defaultItemHeight = 13;

    [Localizable(true)]
    [DefaultValue(0)]
    public int ColumnWidth
    {
        get; set;
    }

    [DefaultValue(false)]
    [Browsable(false)]
    public bool UseCustomTabOffsets
    {
        get; set;
    }

    [DefaultValue(DrawMode.Normal)]
    public virtual DrawMode DrawMode
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    [DefaultValue(0)]
    [Localizable(true)]
    public int HorizontalExtent
    {
        get; set;
    }

    [DefaultValue(false)]
    [Localizable(true)]
    public bool HorizontalScrollbar
    {
        get; set;
    }

    [DefaultValue(true)]
    [Localizable(true)]
    public bool IntegralHeight
    {
        get; set;
    }

    [Localizable(true)]
    public virtual int ItemHeight
    {
        get; set;
    }

    [Localizable(true)]
    public ObjectCollection Items => _items;

    [DefaultValue(false)]
    public bool MultiColumn
    {
        get; set;
    }

    [Browsable(false)]
    public int PreferredHeight
    {
        get;
        set;
    }

    [DefaultValue(false)]
    [Localizable(true)]
    public bool ScrollAlwaysVisible
    {
        get; set;
    }

    [Browsable(false)]
    public SelectedIndexCollection SelectedIndices
    {
        get {
            var indexs = new SelectedIndexCollection(this);
            return indexs;
        }
    }

    [Browsable(false)]
    public SelectedObjectCollection SelectedItems
    {
        get
        {
            var indexs = new SelectedObjectCollection(this);
            return indexs;
        }
    }
    public SelectionMode selectionMode;
    [DefaultValue(SelectionMode.One)]
    public virtual SelectionMode SelectionMode
    {
        get => selectionMode;
        set {
            if (value == SelectionMode.None)
            {
                self.listBox.SelectionMode = Gtk.SelectionMode.None;
            }
            else if (value == SelectionMode.One)
            {
                self.listBox.SelectionMode = Gtk.SelectionMode.Single;
            }
            else if (value == SelectionMode.MultiSimple)
            {
                self.listBox.SelectionMode = Gtk.SelectionMode.Multiple;
            }
            else if (value == SelectionMode.MultiExtended)
            {
                self.listBox.SelectionMode = Gtk.SelectionMode.Multiple;
            }
        }
    }
    private void CheckNoDataSource()
    {
        //if (DataSource != null)
        //{
        //    throw new ArgumentException("SR.DataSourceLocksItems");
        //}
    }
    private bool _sorted;
    [DefaultValue(false)]
    public bool Sorted
    {
        get => _sorted; set => _sorted = value;
    }

    [Browsable(false)]
    public override string? Text
    {
        get
        {
            if (self.listBox.SelectionMode == Gtk.SelectionMode.Multiple)
            {
                var texts = self.listBox.SelectedRows.Select(row => ((Gtk.Label)row.Child).Text);
                return string.Join(",", texts);
            }

            {
                var row = self.listBox.SelectedRow;
                if (row == null)
                    return string.Empty;
                return ((Gtk.Label)row.Child).Text;
            }
        } 
        set
        {
            foreach (var row in self.listBox.Children)
            {
                if (row is ListBoxRow box)
                {
                    if (((Gtk.Label)box.Child).Text == value)
                        self.listBox.SelectRow(box);
                }
            }
        }
    }
    private int _topIndex;
    public int TopIndex
    {
        get=> _topIndex; 
        set {
            _topIndex = value;
            Timeout.Add(100, () => {
                var rowheight = ItemHeight;
                if (rowheight < 14)
                {
                    rowheight = self.listBox.Children.Length > 0 ? self.listBox.Children[0].AllocatedHeight : 18;
                }
                var adjustment = self.Vadjustment;
                adjustment.Value = value * rowheight - Height + 5;
                return false;
            });
        }
    }

    [DefaultValue(true)]
    public bool UseTabStops
    {
        get; set;
    }

    public IntegerCollection? CustomTabOffsets
    {
        get;
        set;
    }

    public new Padding Padding
    {
        get;set;
    }
    public void ClearSelected()
    {
        self.listBox.UnselectAll();
    }
    internal bool isUpdateing;
    public void BeginUpdate()
    {
        isUpdateing = true;
    }

    public void EndUpdate()
    {
        isUpdateing = false;
        self.listBox.ShowAll();
    }

    public int FindString(string s)
    {
        throw new NotImplementedException();
    }

    public int FindString(string s, int startIndex)
    {
        throw new NotImplementedException();
    }

    public int FindStringExact(string s)
    {
        throw new NotImplementedException();
    }

    public int FindStringExact(string s, int startIndex)
    {
        throw new NotImplementedException();
    }

    public int GetItemHeight(int index)
    {
        throw new NotImplementedException();
    }

    public Drawing.Rectangle GetItemRectangle(int index)
    {
        throw new NotImplementedException();
    }
    public bool GetSelected(int index)
    {
        return self.listBox.GetRowAtIndex(index).IsSelected;
    }

    public int IndexFromPoint(Drawing.Point p)
    {
        throw new NotImplementedException();
    }

    public int IndexFromPoint(int x, int y)
    {
        throw new NotImplementedException();
    }

    public override void Refresh()
    {
        self.listBox.ShowAll();
    }

    public override void ResetBackColor()
    {
			
    }

    public override void ResetForeColor()
    {
			
    }

    public void SetSelected(int index, bool value)
    {
        if (value)
            self.listBox.SelectRow(self.listBox.GetRowAtIndex(index));
        else
            self.listBox.UnselectRow(self.listBox.GetRowAtIndex(index));
    }
    public class ListBoxItem: Gtk.Label
    {
        public ListBoxItem() { 
            Xalign = 0;
        }
        public object DisplayText { get => Text;
            set => Text = value?.ToString();
        }
        public object? ItemValue { get; set; }
        public object? CheckValue { get; set; }

        public override string ToString()
        {
            return DisplayText?.ToString() ?? string.Empty;
        }
    }
}