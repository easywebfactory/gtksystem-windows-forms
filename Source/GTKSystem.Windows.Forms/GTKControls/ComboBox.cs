/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public partial class ComboBox: ListControl
{
    public readonly ComboBoxBase self = new();
    public override object GtkControl => self;
    private readonly ObjectCollection itemsData;
    public ComboBox()
    {
        self.Entry.HasFrame = false;
        self.Entry.WidthChars = 0;

        itemsData = new ObjectCollection(this);
        self.Realized += Self_Realized;
        self.Changed += Self_Changed;
    }

    private void Self_Changed(object? sender, EventArgs e)
    {
        if (self.IsVisible)
        {
            ((EventHandler)events["SelectedIndexChanged"])?.Invoke(this, e);
            ((EventHandler)events["SelectedValueChanged"])?.Invoke(this, e);
            ((EventHandler)events["SelectedItemChanged"])?.Invoke(this, e);
        }
    }

    private void Self_Realized(object? sender, EventArgs e)
    {
        OnSetDataSource();
        var ws = ((Box)self.Children[0].Parent).Children[1] as ToggleButton;
        if (ws != null)
        {
            ws.Toggled += Ws_Toggled;
            if (DropDownStyle == ComboBoxStyle.DropDownList)
            {
                self.Entry.IsEditable = false;
                self.Entry.CanFocus = false;
                self.Entry.NoShowAll = true;
                self.Entry.WidthRequest = 1;
                ws.WidthRequest = self.WidthRequest;
                ws.Label = self.Entry.Text;
                ws.Image = Gtk.Image.NewFromIconName("pan-down", IconSize.Button);
                ws.ImagePosition = PositionType.Right;
                ws.AlwaysShowImage = true;
                ws.Valign = Align.Center;
                ws.Yalign = 0.5f;
                ws.Xalign = 0.95f;
                ws.Hexpand = true;
                ws.Image.Halign = Align.End;
                ws.Image.Valign = Align.Center;
                ws.Drawn += Ws_Drawn;
            }
        }
    }
    public event EventHandler? DropDown;
    private void Ws_Toggled(object? sender, EventArgs e)
    {
        DropDown?.Invoke(this, e);
    }
    private void Ws_Drawn(object? o, DrawnArgs args)
    {
        self.Entry.Visible = false;
        var ws = o as ToggleButton;
        if (ws != null)
        {
            ws.WidthRequest = -1;
            var pangocontext = ws.PangoContext;
            var family = pangocontext.FontDescription.Family;
            args.Cr.SelectFontFace(family, Cairo.FontSlant.Normal, Cairo.FontWeight.Normal);
            if (ForeColor.Name == "0")
            {
                var color = ws.StyleContext.GetColor(StateFlags.Normal);
                args.Cr.SetSourceRGBA(color.Red, color.Green, color.Blue, color.Alpha);
            }
            else
            {
                var foreColorR = ForeColor.R / 255;
                var foreColorG = ForeColor.G / 255;
                var foreColorB = ForeColor.B / 255;
                var foreColorA = ForeColor.A / 255;
                args.Cr.SetSourceRGBA(foreColorR, foreColorG, foreColorB, foreColorA);
            }

            var fontsize = pangocontext.FontDescription.Size / Pango.Scale.PangoScale * 1.5;
            args.Cr.Save();
            args.Cr.Translate(10, (ws.AllocatedHeight - fontsize) / 2 + fontsize - 2);
            args.Cr.SetFontSize(fontsize);
            args.Cr.TextExtents(self.Entry.Text);
            var text = self.Entry.Text;
            while (text.Length > 1 && args.Cr.TextExtents(text).Width > ws.AllocatedWidth - 40)
                text = text.Substring(0, text.Length - 1);

            args.Cr.ShowText(text);
        }

        args.Cr.Restore();
    }

    private ComboBoxStyle dropDownStyle;
    public ComboBoxStyle DropDownStyle { 
        get=> dropDownStyle; 
        set {
            dropDownStyle = value;
            if (value == ComboBoxStyle.DropDown)
            {
                self.StyleContext.RemoveClass("DropDownList");
            }
            else if (value == ComboBoxStyle.DropDownList)
            {
                self.StyleContext.AddClass("DropDownList");
                self.Entry.IsEditable = false;
                self.Entry.CanFocus = false;
            }
        }
    }


    public override string? Text { get => self.Entry.Text; set => self.Entry.Text = value;
    }
    public object? SelectedItem { 
        get => SelectedIndex == -1 ? null : itemsData[SelectedIndex];
        set { var _index = itemsData.IndexOf(value); if (_index != -1) { SelectedIndex = _index; } } 
    }
    internal int _selectedIndex;
    public override int SelectedIndex { get => self.Active;
        set { self.Active = value; _selectedIndex = value; if (value == -1) { Text = ""; } } }
    public ObjectCollection Items => itemsData;

    public override string GetItemText(object? item)
    {
        if (item is ObjectCollection.Entry entry)
        {
            return entry.Item?.ToString()??string.Empty;
        }
        return item?.ToString()?? string.Empty;
    }
    public string NativeGetItemText(int index)
    {
        return itemsData[index]?.ToString() ?? string.Empty;
    }
    private bool _sorted;
    public bool Sorted { get=> _sorted; set=> _sorted = value; }
    public object? dataSource;
    public override object? DataSource
    {
        get => dataSource;
        set {
            dataSource = value;
            if (self.IsVisible)
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
        itemsData.Clear();
        if (enumerator != null)
        {
            if (string.IsNullOrWhiteSpace(DisplayMember))
            {
                while (enumerator.MoveNext())
                {
                    var o = enumerator.Current;
                    if (o is DataRowView row)
                        itemsData.Add(row[0]);
                    else
                        itemsData.Add(enumerator.Current);
                }
            }
            else
            {
                while (enumerator.MoveNext())
                {
                    var o = enumerator.Current;
                    if(o is DataRowView row)
                        itemsData.Add(row[DisplayMember]);
                    else
                        itemsData.Add(o?.GetType().GetProperty(DisplayMember)?.GetValue(o));
                }
            }
        }
    }

}