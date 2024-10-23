using Atk;
using Gdk;
using GLib;
using Gtk;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace System.Windows.Forms.GtkRender
{
    public class CellRendererValue : CellRendererText
    {
        public CellRendererValue()
        {
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
    }
    public class CellRendererToggleValue : CellRendererToggle
    {
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
    }
    public class CellRendererComboValue : CellRendererCombo
    {
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
    }
    public class CellRendererPixbufValue : CellRendererPixbuf
    {
        DataGridView GridView;
        DataGridViewColumn Column;
        public CellRendererPixbufValue(DataGridViewColumn view)
        {
            Column = view;
            GridView = view.DataGridView;
        }
        [Property("cellvalue")]
        public CellValue CellValue
        {
            set
            {
                if (value != null)
                {
                    if (value.ValueType == typeof(byte[]))
                    {
                        value.SetControlWithStyle(this);
                        this.Pixbuf = new Pixbuf((byte[])value.Value);
                    }
                    else
                    {
                        string text = value.Text;
                        if (string.IsNullOrWhiteSpace(text) == false && this.Data.ContainsKey(text))
                        {
                            value.SetControlWithStyle(this);
                            if (this.Data[text] is Gdk.Pixbuf pixbuf)
                                this.Pixbuf = pixbuf;
                            else
                                this.IconName = text;
                        }
                        else
                        {
                            value.SetControlWithStyle(this);
                            if (string.IsNullOrWhiteSpace(text))
                            {
                                this.IconName = "";
                            }
                            else
                            {
                                try
                                {
                                    if (text.Contains("://"))
                                    {
                                        HttpClient httpClient = new HttpClient();
                                        httpClient.GetStreamAsync(text).ContinueWith((x, o) =>
                                        {
                                            string key = o.ToString();
                                            try
                                            {
                                                Gdk.Pixbuf obuf = new Gdk.Pixbuf(x.Result);
                                                Gdk.Pixbuf nbuf = obuf.ScaleSimple(Column.Width < 1 ? obuf.Width : Column.Width, Column.RowHeight < 1 ? obuf.Height : Column.RowHeight, Gdk.InterpType.Tiles);
                                                if (this.Data.ContainsKey(key) == false)
                                                    this.Data.Add(key, nbuf);
                                                Gtk.Application.Invoke((o, s) =>
                                                {
                                                    this.Pixbuf = nbuf;
                                                });
                                            }
                                            catch (Exception ex)
                                            {
                                                //Console.WriteLine(text + ", " + ex.ToString());
                                                if (this.Data.ContainsKey(key) == false)
                                                    this.Data.Add(key, "image-missing");
                                                Gtk.Application.Invoke((o, s) =>
                                                {
                                                    this.IconName = "image-missing";
                                                });
                                            }
                                        }, text);

                                    }
                                    else if (text.Contains("."))
                                    {
                                        Gdk.Pixbuf nbuf = new Gdk.Pixbuf(text.Replace("\\\\", "//").Replace("\\", "/"));
                                        this.Data.Add(text, nbuf);
                                        this.Pixbuf = nbuf;
                                    }
                                    else
                                    {
                                        this.Data.Add(text, text);
                                        this.IconName = text;
                                    }
                                }
                                catch
                                {
                                    this.Data.Add(text, "image-missing");
                                    this.IconName = "image-missing";
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public class CellRendererButtonValue : CellRendererText
    {
        public CellRendererButtonValue()
        {
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
                    value.SetTextWithStyle(this);
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
        internal void SetControlWithStyle(CellRenderer cell) {
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
        internal void SetTextWithStyle(CellRendererText cell)
        {
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
