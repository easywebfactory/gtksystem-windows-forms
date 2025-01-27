  /*
   * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
   * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
   * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
   * author:chenhongjin
   */
using Gdk;
using GLib;
using Gtk;
using System.Diagnostics.CodeAnalysis;

namespace System.Windows.Forms.GtkRender
{
    public class CellRendererValue : CellRendererText
    {
        DataGridViewColumn Column;
        public CellRendererValue(DataGridViewColumn view)
        {
            Column = view;
        }
        [Property("cellvalue")]
        public CellValue CellValue
        {
            set
            {
                if (value != null)
                {
                    value.SetTextWithStyle(Column, this);
                }
            }
        }
    }
    public class CellRendererToggleValue : CellRendererToggle
    {
        DataGridViewColumn Column;
        public CellRendererToggleValue(DataGridViewColumn view)
        {
            Column = view;
        }
        [Property("cellvalue")]
        public CellValue CellValue
        {
            set
            {
                if (value != null)
                {
                    this.Active = (value.Text == "1" || value.Text?.ToLower() == "true");
                    value.SetControlWithStyle(Column, this);
                }
            }
        }
    }
    public class CellRendererComboValue : CellRendererCombo
    {
        DataGridViewColumn Column;
        public CellRendererComboValue(DataGridViewColumn view)
        {
            Column = view;
        }
        [Property("cellvalue")]
        public CellValue CellValue
        {
            set
            {
                if (value != null)
                {
                    value.SetTextWithStyle(Column, this);
                }
            }
        }
    }
    public class CellRendererPixbufValue : CellRendererPixbuf
    {
        DataGridViewColumn Column;
        public CellRendererPixbufValue(DataGridViewColumn view)
        {
            Column = view;
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
                        value.SetControlWithStyle(Column, this);
                        if (typeof(Drawing.Image).Equals(value.ValueType) || typeof(Drawing.Bitmap).Equals(value.ValueType))
                        {
                            this.Pixbuf = ((Drawing.Image)value.Value).Pixbuf;
                        }
                        else if (value.ValueType == typeof(byte[]))
                        {
                            this.Pixbuf = new Pixbuf((byte[])value.Value);
                        }
                        else
                        {
                            this.Pixbuf = new Gdk.Pixbuf(value.Text.Replace("\\\\", "//").Replace("\\", "/"));
                        }
                    }
                    catch
                    {
                        this.IconName = "image-missing";
                    }
                }
            }
        }
    }
    public class CellRendererButtonValue : CellRendererText
    {
        DataGridViewColumn Column;
        public CellRendererButtonValue(DataGridViewColumn view)
        {
            Column = view;
            this.SetAlignment(0.5f, 0.5f);
            this.Ellipsize = Pango.EllipsizeMode.End;
        }
        [Property("cellvalue")]
        public CellValue CellValue
        {
            set
            {
                if (value != null)
                {
                    value.SetTextWithStyle(Column, this);
                }
            }
        }
        protected override void OnRender(Cairo.Context cr, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, CellRendererState flags)
        {
            widget.StyleContext.AddClass("button");
            widget.StyleContext.AddClass("GridViewCell-Button");
            widget.StyleContext.Save();
            int height = cell_area.Height;
            int y = cell_area.Y;
            if (height > 36)
            {
                y = y + (cell_area.Height - 36) / 2;
                height = 36;
            }
            widget.StyleContext.RenderFrame(cr, cell_area.X + 3, y, cell_area.Width - 6, height);
            widget.StyleContext.RenderBackground(cr, cell_area.X + 3, y, cell_area.Width - 6, height);
            widget.StyleContext.Restore();
            base.OnRender(cr, widget, new Gdk.Rectangle(background_area.X, background_area.Y, background_area.Width, background_area.Height), new Gdk.Rectangle(cell_area.X, cell_area.Y, cell_area.Width, cell_area.Height), flags);
        }
    }
    public class CellValue : IComparable, IComparable<CellValue>, IEquatable<CellValue>
    {
        private DataGridViewCellStyle _style;
        public DataGridViewCellStyle Style { get => _style; set => _style = value; }
        public Type ValueType { get; set; }
        private object _value;
        public object Value { get=> _value; set { _value = value; ValueType = value?.GetType(); } }
        public string Text { get => _value?.ToString(); }
        internal void SetControlWithStyle(DataGridViewColumn Column, CellRenderer cell) {
            if (Column.DefaultCellStyle != null)
                _style = Column.DefaultCellStyle;
            if (_style != null)
            {
                if (_style.BackColor.Name != "0")
                    cell.CellBackgroundRgba = new Gdk.RGBA() { Alpha = _style.BackColor.A / 255f, Blue = _style.BackColor.B / 255f, Green = _style.BackColor.G / 255f, Red = _style.BackColor.R / 255f };
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
        internal void SetTextWithStyle(DataGridViewColumn Column, CellRendererText cell)
        {
            if (Column.DefaultCellStyle != null)
                _style = Column.DefaultCellStyle;
            if (_style != null)
            {
                if (_style.ForeColor.Name != "0")
                    cell.ForegroundRgba = new Gdk.RGBA() { Alpha = _style.ForeColor.A / 255f, Blue = _style.ForeColor.B / 255f, Green = _style.ForeColor.G / 255f, Red = _style.ForeColor.R / 255f };
                if (_style.BackColor.Name != "0")
                    cell.CellBackgroundRgba = new Gdk.RGBA() { Alpha = _style.BackColor.A / 255f, Blue = _style.BackColor.B / 255f, Green = _style.BackColor.G / 255f, Red = _style.BackColor.R / 255f };
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
        internal string GetFormatText(object text)
        {
            if (text == null && _style != null)
                text = Convert.ToString(_style.NullValue) ?? string.Empty;
            if (_style != null && string.IsNullOrWhiteSpace(_style.Format) == false)
                return string.Format(_style.Format, text);
            else
                return text?.ToString();
        }
        public int CompareTo(object obj)
        {
            if (obj is CellValue cell)
                return CompareTo(cell);
            else
                return -1;
        }

        public int CompareTo([AllowNull] CellValue other)
        {
            if(other != null)
            {
                if (this._value == other.Value && this._style != null && other.Style != null)
                    return this._style.GetHashCode().CompareTo(other.Style.GetHashCode());
            }
            return -1;
        }

        public bool Equals([AllowNull] CellValue other)
        {
            return CompareTo(other) == 0;
        }

        public override string ToString()
        {
            return _value?.ToString();
        }

    }

}
