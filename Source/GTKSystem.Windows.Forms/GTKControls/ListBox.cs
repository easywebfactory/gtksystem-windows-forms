/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
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

    public override IControlGtk Self
    {
        get => self;
    }

    protected override void SetStyle(Widget widget)
    {
        self.ListBox.Name = Name;
        base.SetStyle(self.ListBox);
    }

    private readonly ControlBindingsCollection _collect;
    private readonly ObjectCollection _items;

    public ListBox()
    {
        self.ListBox.Halign = Align.Fill;
        self.ListBox.Valign = Align.Fill;
        self.ListBox.Hexpand = true;
        self.ListBox.Vexpand = true;
        _collect = new ControlBindingsCollection(this);
        _items = new ObjectCollection(this);
        self.ListBox.Realized += Self_Realized;
        self.ListBox.SelectedRowsChanged += ListBox_SelectedRowsChanged;
        BorderStyle = BorderStyle.Fixed3D;
    }

    private void ListBox_SelectedRowsChanged(object? sender, EventArgs e)
    {
        OnSelectedRowsChanged(e);
    }

    protected virtual void OnSelectedRowsChanged(EventArgs e)
    {
        if (self.ListBox.IsVisible)
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
                self.ListBox.AddNotification(binding.PropertyName, PropertyNotity);
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
        set
        {
            dataSource = value;
            if (self.ListBox.IsVisible)
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
            object listBoxSelectedRow = self.ListBox.SelectedRow;
            return listBoxSelectedRow == null ? -1 : self.ListBox.SelectedRow.Index;
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
        if (item is ItemArray.Entry entry)
        {
            return entry.Item?.ToString() ?? string.Empty;
        }

        return item?.ToString() ?? string.Empty;
    }

    protected void NativeInsert(int index, object? item)
    {
        var row = new ListBoxRow();
        row.HeightRequest = ItemHeight > 0 ? ItemHeight : defaultItemHeight;
        row.Add(new Gtk.Label(item?.ToString()) { Valign = Align.Center, Halign = Align.Start, Expand = true });
        self.ListBox.Insert(row, index);
        if (self.ListBox.IsVisible && !isUpdateing)
        {
            self.ListBox.ShowAll();
        }
    }

    protected void NativeAdd(object? item)
    {
        var row = new ListBoxRow();
        row.HeightRequest = ItemHeight > 0 ? ItemHeight : defaultItemHeight;
        row.Add(new Gtk.Label(item?.ToString()) { Valign = Align.Center, Halign = Align.Start, Expand = true });
        self.ListBox.Add(row);
        if (self.ListBox.IsVisible && !isUpdateing)
        {
            self.ListBox.ShowAll();
        }
    }

    protected void NativeClear()
    {
        var count = self.ListBox.Children.Length;
        while (count > 0)
        {
            self.ListBox.Remove(self.ListBox.GetRowAtIndex(count - 1));
            count--;
            //System.Threading.Thread.Sleep(3);
        }
    }

    protected void NativeRemoveAt(int index)
    {
        self.ListBox.Remove(self.ListBox.GetRowAtIndex(index));
    }

    protected string NativeGetItemText(int index)
    {
        var row = self.ListBox.GetRowAtIndex(index).Child as Gtk.Label;
        return row?.Text ?? string.Empty;
    }

    protected void OnSelectedIndexChanged(EventArgs e)
    {
        if (self.ListBox.SelectedRow != null)
            self.ListBox.SelectRow(self.ListBox.SelectedRow);
    }

    #endregion

    public override ControlBindingsCollection DataBindings => _collect;
    internal bool ShowCheckBox { get; set; }
    internal bool ShowImage { get; set; }

    public const int noMatches = -1;

    public const int defaultItemHeight = 13;

    [Localizable(true)] [DefaultValue(0)] public int ColumnWidth { get; set; }

    [DefaultValue(false)]
    [Browsable(false)]
    public bool UseCustomTabOffsets { get; set; }

    [DefaultValue(DrawMode.Normal)]
    public virtual DrawMode DrawMode
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    [DefaultValue(0)] [Localizable(true)] public int HorizontalExtent { get; set; }

    [DefaultValue(false)]
    [Localizable(true)]
    public bool HorizontalScrollbar { get; set; }

    [DefaultValue(true)]
    [Localizable(true)]
    public bool IntegralHeight { get; set; }

    [Localizable(true)] public virtual int ItemHeight { get; set; }

    [Localizable(true)] public ObjectCollection Items => _items;

    [DefaultValue(false)] public bool MultiColumn { get; set; }

    [Browsable(false)] public int PreferredHeight { get; set; }

    [DefaultValue(false)]
    [Localizable(true)]
    public bool ScrollAlwaysVisible { get; set; }

    [Browsable(false)]
    public SelectedIndexCollection SelectedIndices
    {
        get
        {
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
        set
        {
            if (value == SelectionMode.None)
            {
                self.ListBox.SelectionMode = Gtk.SelectionMode.None;
            }
            else if (value == SelectionMode.One)
            {
                self.ListBox.SelectionMode = Gtk.SelectionMode.Single;
            }
            else if (value == SelectionMode.MultiSimple)
            {
                self.ListBox.SelectionMode = Gtk.SelectionMode.Multiple;
            }
            else if (value == SelectionMode.MultiExtended)
            {
                self.ListBox.SelectionMode = Gtk.SelectionMode.Multiple;
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
        get => _sorted;
        set => _sorted = value;
    }

    [Browsable(false)]
    public override string Text
    {
        get
        {
            if (self.ListBox.SelectionMode == Gtk.SelectionMode.Multiple)
            {
                var texts = self.ListBox.SelectedRows.Select(row => ((Gtk.Label)row.Child).Text);
                return string.Join(",", texts);
            }

            {
                var row = self.ListBox.SelectedRow;
                if (row == null)
                    return string.Empty;
                return ((Gtk.Label)row.Child).Text;
            }
        }
        set
        {
            foreach (var row in self.ListBox.Children)
            {
                if (row is ListBoxRow box)
                {
                    if (((Gtk.Label)box.Child).Text == value)
                        self.ListBox.SelectRow(box);
                }
            }
        }
    }

    private int _topIndex;

    public int TopIndex
    {
        get => _topIndex;
        set
        {
            _topIndex = value;
            Timeout.Add(100, () =>
            {
                var rowheight = ItemHeight;
                if (rowheight < 14)
                {
                    rowheight = self.ListBox.Children.Length > 0 ? self.ListBox.Children[0].AllocatedHeight : 18;
                }

                var adjustment = self.Vadjustment;
                adjustment.Value = value * rowheight - Height + 5;
                return false;
            });
        }
    }

    [DefaultValue(true)] public bool UseTabStops { get; set; }

    public IntegerCollection? CustomTabOffsets { get; set; }

    public new Padding Padding { get; set; }

    public void ClearSelected()
    {
        self.ListBox.UnselectAll();
    }

    internal bool isUpdateing;

    public void BeginUpdate()
    {
        isUpdateing = true;
    }

    public void EndUpdate()
    {
        isUpdateing = false;
        self.ListBox.ShowAll();
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
        return self.ListBox.GetRowAtIndex(index).IsSelected;
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
        self.ListBox.ShowAll();
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
            self.ListBox.SelectRow(self.ListBox.GetRowAtIndex(index));
        else
            self.ListBox.UnselectRow(self.ListBox.GetRowAtIndex(index));
    }

    public class ListBoxItem : Gtk.Label
    {
        public ListBoxItem()
        {
            Xalign = 0;
        }

        public object DisplayText
        {
            get => Text;
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