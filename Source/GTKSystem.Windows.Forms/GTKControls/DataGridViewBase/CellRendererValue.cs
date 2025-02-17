/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gdk;
using GLib;
using Gtk;

namespace System.Windows.Forms.GtkRender
{
    public interface ICellRenderer
    {
        CellValue CellValue { set; }
        DataGridViewCellStyle DefaultStyle { set; }
        Pango.WrapMode WrapMode { get; set; }
        int WrapWidth { get; set; }
        int WidthChars { get; set; }
        int Height { get; set; }
        string Markup { get; set; }
        bool Editable { get; set; }
        bool Activatable { get; set; }
        Gtk.CellRendererMode Mode { get; set; }
    }

    public class CellRendererValue : CellRendererText, ICellRenderer
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
                    value.SetTextWithStyle(this);
                }
            }
        }
        public DataGridViewCellStyle DefaultStyle
        {
            set
            {
                if (value?.WrapMode == DataGridViewTriState.True)
                {
                    this.WrapMode = Pango.WrapMode.WordChar;
                    this.WrapWidth = 0;
                    this.WidthChars = 0;
                }
                CellValue cellValue = new CellValue();
                cellValue.SetTextWithStyle(value, this);
            }
        }
        public bool Activatable { get; set; }
        string ICellRenderer.Markup { get; set; }
    }
    public class CellRendererToggleValue : CellRendererToggle, ICellRenderer
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
                    value.SetControlWithStyle(this);
                }
            }
        }
        public DataGridViewCellStyle DefaultStyle {
            set
            {
                if (value?.WrapMode == DataGridViewTriState.True)
                {
                    this.WrapMode = Pango.WrapMode.WordChar;
                    this.WrapWidth = 0;
                    this.WidthChars = 0;
                }
                CellValue cellValue = new CellValue();
                cellValue.SetControlWithStyle(value, this);
            }
        }

        public Pango.WrapMode WrapMode { get; set; }
        public int WrapWidth { get; set; }
        public int WidthChars { get; set; }
        public string Markup { get; set; }
        public bool Editable { get; set; }
    }
    public class CellRendererComboValue : CellRendererCombo, ICellRenderer
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
                    value.SetTextWithStyle(this);
                }
            }
        }
        public DataGridViewCellStyle DefaultStyle
        {
            set
            {
                if (value?.WrapMode == DataGridViewTriState.True)
                {
                    this.WrapMode = Pango.WrapMode.WordChar;
                    this.WrapWidth = 0;
                    this.WidthChars = 0;
                }
                CellValue cellValue = new CellValue();
                cellValue.SetTextWithStyle(value, this);
            }
        }
        public bool Activatable { get; set; }
        string ICellRenderer.Markup { get; set; }
    }
    public class CellRendererPixbufValue : CellRendererPixbuf, ICellRenderer
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
                        value.SetControlWithStyle(this);
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
        public DataGridViewCellStyle DefaultStyle
        {
            set
            {
                if (value?.WrapMode == DataGridViewTriState.True)
                {
                    this.WrapMode = Pango.WrapMode.WordChar;
                    this.WrapWidth = 0;
                    this.WidthChars = 0;
                }
                CellValue cellValue = new CellValue();
                cellValue.SetControlWithStyle(value, this);
            }
        }
        public Pango.WrapMode WrapMode { get; set; }
        public int WrapWidth { get; set; }
        public int WidthChars { get; set; }
        public string Markup { get; set; }
        public bool Editable { get; set; }
        public bool Activatable { get; set; }
    }
    public class CellRendererButtonValue : CellRendererText, ICellRenderer
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
                //_value = value;
                if (value != null)
                {
                    value.SetTextWithStyle(this);
                    base.SetAlignment(0.5f, 0.5f);
                }
            }
        }
        //private CellValue _value;
        public DataGridViewCellStyle DefaultStyle
        {
            set
            {
                if (value?.WrapMode == DataGridViewTriState.True)
                {
                    this.WrapMode = Pango.WrapMode.WordChar;
                    this.WrapWidth = 0;
                    this.WidthChars = 0;
                }
                CellValue cellValue = new CellValue();
                cellValue.SetTextWithStyle(value, this);
                base.SetAlignment(0.5f, 0.5f);
            }
        }
        public bool Activatable { get; set; }
        string ICellRenderer.Markup { get; set; }
        protected override void OnRender(Cairo.Context cr, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, CellRendererState flags)
        {
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
    public class CellValue
    {
        private DataGridViewCellStyle _style;
        public DataGridViewCellStyle Style { get => _style; set => _style = value; }
        private Type _valueType;
        public Type ValueType { get { return _valueType == null ? _value?.GetType() : _valueType; } set { _valueType = value; } }
        private object _value;
        public object Value { get => _value; set { _value = value; } }
        public string Text { get => _value?.ToString(); }
        public string Path { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        internal void SetControlWithStyle(CellRenderer cell)
        {
            SetControlWithStyle(_style, cell);
        }
        internal void SetControlWithStyle(DataGridViewCellStyle cellstyle, CellRenderer cell)
        {
            if (cellstyle != null)
            {
                if (cellstyle.BackColor.Name != "0")
                    cell.CellBackgroundRgba = new Gdk.RGBA() { Alpha = cellstyle.BackColor.A / 255f, Blue = cellstyle.BackColor.B / 255f, Green = cellstyle.BackColor.G / 255f, Red = cellstyle.BackColor.R / 255f };
                if (cellstyle.Alignment == DataGridViewContentAlignment.TopLeft)
                    cell.SetAlignment(0, 0);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.TopCenter)
                    cell.SetAlignment(0.5f, 0);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.TopRight)
                    cell.SetAlignment(1.0f, 0);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.MiddleLeft)
                    cell.SetAlignment(0, 0.5f);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.MiddleCenter)
                    cell.SetAlignment(0.5f, 0.5f);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.MiddleRight)
                    cell.SetAlignment(1.0f, 0.5f);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.BottomLeft)
                    cell.SetAlignment(0, 1f);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.BottomCenter)
                    cell.SetAlignment(0.5f, 1f);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.BottomRight)
                    cell.SetAlignment(1.0f, 1f);
            }
        }
        internal void SetTextWithStyle(CellRendererText cell)
        {
            SetTextWithStyle(_style, cell);
            cell.Text = GetFormatText(_value);
        }
        internal void SetTextWithStyle(DataGridViewCellStyle cellstyle, CellRendererText cell)
        {
            if (cellstyle != null)
            {
                if (cellstyle.ForeColor.Name != "0")
                    cell.ForegroundRgba = new Gdk.RGBA() { Alpha = cellstyle.ForeColor.A / 255f, Blue = cellstyle.ForeColor.B / 255f, Green = cellstyle.ForeColor.G / 255f, Red = cellstyle.ForeColor.R / 255f };
                if (cellstyle.BackColor.Name != "0")
                    cell.CellBackgroundRgba = new Gdk.RGBA() { Alpha = cellstyle.BackColor.A / 255f, Blue = cellstyle.BackColor.B / 255f, Green = cellstyle.BackColor.G / 255f, Red = cellstyle.BackColor.R / 255f };
                if (cellstyle.Alignment == DataGridViewContentAlignment.TopLeft)
                    cell.SetAlignment(0, 0);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.TopCenter)
                    cell.SetAlignment(0.5f, 0);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.TopRight)
                    cell.SetAlignment(1.0f, 0);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.MiddleLeft)
                    cell.SetAlignment(0, 0.5f);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.MiddleCenter)
                    cell.SetAlignment(0.5f, 0.5f);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.MiddleRight)
                    cell.SetAlignment(1.0f, 0.5f);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.BottomLeft)
                    cell.SetAlignment(0, 1f);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.BottomCenter)
                    cell.SetAlignment(0.5f, 1f);
                else if (cellstyle.Alignment == DataGridViewContentAlignment.BottomRight)
                    cell.SetAlignment(1.0f, 1f);

                if (cellstyle.WrapMode != DataGridViewTriState.NotSet)
                    cell.WrapMode = cellstyle.WrapMode == DataGridViewTriState.True ? Pango.WrapMode.WordChar : Pango.WrapMode.Word;
            }
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
        public override string ToString()
        {
            return _value?.ToString();
        }

    }

}
