/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Atk;
using Gdk;
using GLib;
using Gtk;
using Pango;

namespace System.Windows.Forms.GtkRender
{
    public interface ICellRenderer
    {
        DataGridViewCell CellValue { set; }
        DataGridViewCellStyle ColumnStyle { set; }
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
        private DataGridViewColumn Column;
        private CellReandererUtility utility;
        public CellRendererValue(DataGridViewColumn view)
        {
            Column = view;
            utility = new CellReandererUtility();
        }
        [Property("cellvalue")]
        public DataGridViewCell CellValue
        {
            set
            {
                if (value != null)
                {
                    DataGridViewCellStyle style = value.Style;
                    if (style == null)
                        style = ColumnStyle;
                    if (style == null)
                        style = value.InheritedStyle;

                    utility.SetTextWithStyle(this, style, value.Value);
                }
            }
        }
        public DataGridViewCellStyle ColumnStyle { get; set; }
        public bool Activatable { get; set; }
        string ICellRenderer.Markup { get; set; }
    }
    public class CellRendererToggleValue : CellRendererToggle, ICellRenderer
    {
        DataGridViewColumn Column;
        private CellReandererUtility utility;
        public CellRendererToggleValue(DataGridViewColumn view)
        {
            Column = view;
            utility = new CellReandererUtility();
        }
        [Property("cellvalue")]
        public DataGridViewCell CellValue
        {
            set
            {
                if (value != null)
                {
                    this.Active = (value.Value?.ToString() == "1" || value.Value?.ToString()?.ToLower() == "true");
                    DataGridViewCellStyle style = value.Style;
                    if (style == null)
                        style = ColumnStyle;
                    if (style == null)
                        style = value.InheritedStyle;
                    utility.SetControlWithStyle(this, style);
                }
            }
        }
        public DataGridViewCellStyle ColumnStyle { get; set; }
        public Pango.WrapMode WrapMode { get; set; }
        public int WrapWidth { get; set; }
        public int WidthChars { get; set; }
        public string Markup { get; set; }
        public bool Editable { get; set; }
    }
    public class CellRendererComboValue : CellRendererCombo, ICellRenderer
    {
        DataGridViewColumn Column;
        private CellReandererUtility utility;
        public CellRendererComboValue(DataGridViewColumn view)
        {
            Column = view;
            utility = new CellReandererUtility();
        }
        [Property("cellvalue")]
        public DataGridViewCell CellValue
        {
            set
            {
                if (value != null)
                {
                    DataGridViewCellStyle style = value.Style;
                    if (style == null)
                        style = ColumnStyle;
                    if (style == null)
                        style = value.InheritedStyle;
                    utility.SetTextWithStyle(this, style, value.Value);
                }
            }
        }
        public DataGridViewCellStyle ColumnStyle { get; set; }
        public bool Activatable { get; set; }
        string ICellRenderer.Markup { get; set; }
    }
    public class CellRendererPixbufValue : CellRendererPixbuf, ICellRenderer
    {
        DataGridViewColumn Column;
        private CellReandererUtility utility;
        public CellRendererPixbufValue(DataGridViewColumn view)
        {
            Column = view;
            utility = new CellReandererUtility();
        }
        [Property("cellvalue")]
        public DataGridViewCell CellValue
        {
            set
            {
                if (value != null)
                {
                    try
                    {
                        DataGridViewCellStyle style = value.Style;
                        if (style == null)
                            style = ColumnStyle;
                        if (style == null)
                            style = value.InheritedStyle;
                        utility.SetControlWithStyle(this, style);
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
                            this.Pixbuf = new Gdk.Pixbuf(value.Value.ToString().Replace("\\\\", "//").Replace("\\", "/"));
                        }
                    }
                    catch
                    {
                        this.IconName = "image-missing";
                    }
                }
            }
        }
        public DataGridViewCellStyle ColumnStyle { get; set; }
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
        private CellReandererUtility utility;
        public CellRendererButtonValue(DataGridViewColumn view)
        {
            Column = view;
            utility = new CellReandererUtility();
            this.SetAlignment(0.5f, 0.5f);
            this.Ellipsize = Pango.EllipsizeMode.End;
        }
        [Property("cellvalue")]
        public DataGridViewCell CellValue
        {
            set
            {
                if (value != null)
                {
                    DataGridViewCellStyle style = value.Style;
                    if (style == null)
                        style = ColumnStyle;
                    if (style == null)
                        style = value.InheritedStyle;
                    utility.SetTextWithStyle(this, style, value.Value);
                    base.SetAlignment(0.5f, 0.5f);
                }
            }
        }
        public DataGridViewCellStyle ColumnStyle { get; set; }
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

    public class CellReandererUtility
    {
        internal void SetControlWithStyle(CellRenderer cell, DataGridViewCellStyle cellstyle)
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
        internal void SetTextWithStyle(CellRendererText cell, DataGridViewCellStyle cellstyle, object value)
        {
            SetTextWithStyle(cell, cellstyle);
            cell.Text = GetFormatText(value, cellstyle);
        }
        internal void SetTextWithStyle(CellRendererText cell, DataGridViewCellStyle cellstyle)
        {
            if (cellstyle != null)
            {
                if (cellstyle?.WrapMode == DataGridViewTriState.True)
                {
                    cell.WrapMode = Pango.WrapMode.WordChar;
                    cell.WrapWidth = 0;
                    cell.WidthChars = 0;
                }
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
        internal string GetFormatText(object text, DataGridViewCellStyle cellstyle)
        {
            if (text == null && cellstyle != null)
                text = Convert.ToString(cellstyle.NullValue) ?? string.Empty;
            if (cellstyle != null && string.IsNullOrWhiteSpace(cellstyle.Format) == false)
                return string.Format(cellstyle.Format, text);
            else
                return text?.ToString();
        }
    }
}
