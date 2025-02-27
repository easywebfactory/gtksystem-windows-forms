/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Gdk;
using GLib;
using Gtk;
using Image = System.Drawing.Image;
using Rectangle = Gdk.Rectangle;

namespace System.Windows.Forms;

public class CellRendererValue : CellRendererText
{
    readonly DataGridViewColumn column;
    public CellRendererValue(DataGridViewColumn view)
    {
        column = view;
    }
    [Property("cellvalue")]
    public CellValue CellValue
    {
        set => value?.SetTextWithStyle(column, this);
    }
}
public class CellRendererToggleValue : CellRendererToggle
{
    readonly DataGridViewColumn column;
    public CellRendererToggleValue(DataGridViewColumn view)
    {
        column = view;
    }
    [Property("cellvalue")]
    public CellValue CellValue
    {
        set
        {
            if (value != null)
            {
                Active = value.Text == "1" || value.Text?.ToLower() == "true";
                value.SetControlWithStyle(column, this);
            }
        }
    }
}
public class CellRendererComboValue : CellRendererCombo
{
    readonly DataGridViewColumn column;
    public CellRendererComboValue(DataGridViewColumn view)
    {
        column = view;
    }
    [Property("cellvalue")]
    public CellValue CellValue
    {
        set => value?.SetTextWithStyle(column, this);
    }
}
public class CellRendererPixbufValue : CellRendererPixbuf
{
    readonly DataGridViewColumn column;
    public CellRendererPixbufValue(DataGridViewColumn view)
    {
        column = view;
    }
    [Property("cellvalue")]
    public CellValue CellValue
    {
        set
        {
            if (value != null)
            {
                try
                {
                    value.SetControlWithStyle(column, this);
                    if (typeof(Image) == value.ValueType || typeof(Bitmap) == value.ValueType)
                    {
                        Pixbuf = ((Image?)value.Value)?.Pixbuf;
                    }
                    else if (value.ValueType == typeof(byte[]))
                    {
                        Pixbuf = new Pixbuf((byte[]?)value.Value);
                    }
                    else
                    {
                        Pixbuf = new Pixbuf(value.Text?.Replace("\\\\", "//").Replace("\\", "/"));
                    }
                }
                catch
                {
                    IconName = "image-missing";
                }
            }
        }
    }
}
public class CellRendererButtonValue : CellRendererText
{
    readonly DataGridViewColumn column;
    public CellRendererButtonValue(DataGridViewColumn view)
    {
        column = view;
        SetAlignment(0.5f, 0.5f);
        Ellipsize = Pango.EllipsizeMode.End;
    }
    [Property("cellvalue")]
    public CellValue CellValue
    {
        set => value?.SetTextWithStyle(column, this);
    }
    protected override void OnRender(Cairo.Context cr, Widget widget, Rectangle backgroundArea, Rectangle cellArea, CellRendererState flags)
    {
        widget.StyleContext.AddClass("button");
        widget.StyleContext.AddClass("GridViewCell-Button");
        widget.StyleContext.Save();
        var height = cellArea.Height;
        var y = cellArea.Y;
        if (height > 36)
        {
            y = y + (cellArea.Height - 36) / 2;
            height = 36;
        }
        widget.StyleContext.RenderFrame(cr, cellArea.X + 3, y, cellArea.Width - 6, height);
        widget.StyleContext.RenderBackground(cr, cellArea.X + 3, y, cellArea.Width - 6, height);
        widget.StyleContext.Restore();
        base.OnRender(cr, widget, new Rectangle(backgroundArea.X, backgroundArea.Y, backgroundArea.Width, backgroundArea.Height), new Rectangle(cellArea.X, cellArea.Y, cellArea.Width, cellArea.Height), flags);
    }
}
public class CellValue : IComparable, IComparable<CellValue>, IEquatable<CellValue>
{
    private DataGridViewCellStyle? _style;
    public DataGridViewCellStyle? Style { get => _style; set => _style = value; }
    public Type? ValueType { get; set; }
    private object? _value;
    public object? Value { get=> _value; set { _value = value; ValueType = value?.GetType(); } }
    public string Text => _value?.ToString()??string.Empty;

    internal void SetControlWithStyle(DataGridViewColumn column, CellRenderer cell) {
        if (column.DefaultCellStyle != null)
            _style = column.DefaultCellStyle;
        if (_style != null)
        {
            if (_style.BackColor.Name != "0")
                cell.CellBackgroundRgba = new RGBA { Alpha = _style.BackColor.A / 255f, Blue = _style.BackColor.B / 255f, Green = _style.BackColor.G / 255f, Red = _style.BackColor.R / 255f };
            if (_style.Alignment == DataGridViewContentAlignment.TopLeft)
                cell.SetAlignment(0, 0);
            else if (_style.Alignment == DataGridViewContentAlignment.TopCenter)
                cell.SetAlignment(0.5f, 0);
            else if (_style.Alignment == DataGridViewContentAlignment.TopRight)
                cell.SetAlignment(1.0f, 0);
            else if (_style.Alignment == DataGridViewContentAlignment.MiddleLeft)
                cell.SetAlignment(0, 0.5f);
            else if (_style.Alignment == DataGridViewContentAlignment.MiddleCenter)
                cell.SetAlignment(0.5f, 0.5f);
            else if (_style.Alignment == DataGridViewContentAlignment.MiddleRight)
                cell.SetAlignment(1.0f, 0.5f);
            else if (_style.Alignment == DataGridViewContentAlignment.BottomLeft)
                cell.SetAlignment(0, 1f);
            else if (_style.Alignment == DataGridViewContentAlignment.BottomCenter)
                cell.SetAlignment(0.5f, 1f);
            else if (_style.Alignment == DataGridViewContentAlignment.BottomRight)
                cell.SetAlignment(1.0f, 1f);
        }
    }
    internal void SetTextWithStyle(DataGridViewColumn column, CellRendererText cell)
    {
        if (column.DefaultCellStyle != null)
            _style = column.DefaultCellStyle;
        if (_style != null)
        {
            if (_style.ForeColor.Name != "0")
                cell.ForegroundRgba = new RGBA { Alpha = _style.ForeColor.A / 255f, Blue = _style.ForeColor.B / 255f, Green = _style.ForeColor.G / 255f, Red = _style.ForeColor.R / 255f };
            if (_style.BackColor.Name != "0")
                cell.CellBackgroundRgba = new RGBA { Alpha = _style.BackColor.A / 255f, Blue = _style.BackColor.B / 255f, Green = _style.BackColor.G / 255f, Red = _style.BackColor.R / 255f };
            if (_style.Alignment == DataGridViewContentAlignment.TopLeft)
                cell.SetAlignment(0, 0);
            else if (_style.Alignment == DataGridViewContentAlignment.TopCenter)
                cell.SetAlignment(0.5f, 0);
            else if (_style.Alignment == DataGridViewContentAlignment.TopRight)
                cell.SetAlignment(1.0f, 0);
            else if (_style.Alignment == DataGridViewContentAlignment.MiddleLeft)
                cell.SetAlignment(0, 0.5f);
            else if (_style.Alignment == DataGridViewContentAlignment.MiddleCenter)
                cell.SetAlignment(0.5f, 0.5f);
            else if (_style.Alignment == DataGridViewContentAlignment.MiddleRight)
                cell.SetAlignment(1.0f, 0.5f);
            else if (_style.Alignment == DataGridViewContentAlignment.BottomLeft)
                cell.SetAlignment(0, 1f);
            else if (_style.Alignment == DataGridViewContentAlignment.BottomCenter)
                cell.SetAlignment(0.5f, 1f);
            else if (_style.Alignment == DataGridViewContentAlignment.BottomRight)
                cell.SetAlignment(1.0f, 1f);

            if (_style.WrapMode != DataGridViewTriState.NotSet)
                cell.WrapMode = _style.WrapMode == DataGridViewTriState.True ? Pango.WrapMode.WordChar : Pango.WrapMode.Word;
        }
        cell.Text = GetFormatText(_value);
    }
    internal string? GetFormatText(object? text)
    {
        if (text == null && _style != null)
            text = Convert.ToString(_style.NullValue) ?? string.Empty;
        if (_style != null && string.IsNullOrWhiteSpace(_style.Format) == false)
            return string.Format(_style.Format!, text);
        return text?.ToString();
    }
    public int CompareTo(object? obj)
    {
        if (obj is CellValue cell)
            return CompareTo(cell);
        return -1;
    }

    public int CompareTo([AllowNull] CellValue other)
    {
        if(other != null)
        {
            if (_value == other.Value && _style != null && other.Style != null)
                return _style.GetHashCode().CompareTo(other.Style.GetHashCode());
        }
        return -1;
    }

    public bool Equals([AllowNull] CellValue other)
    {
        return CompareTo(other) == 0;
    }

    public override string? ToString()
    {
        return _value?.ToString();
    }

}