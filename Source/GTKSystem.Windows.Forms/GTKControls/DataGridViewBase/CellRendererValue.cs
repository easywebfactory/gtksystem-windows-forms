using GLib;
using Gtk;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.Windows.Forms.GtkRender
{
    public class CellRendererValue : CellRendererText
    {
        [Property("cellvalue")]
        public CellValue CellValue
        {
            set
            {
                this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = 0, Blue = 255, Green = 255, Red = 255 };
                if (value == null)
                {
                    this.Text = string.Empty;
                }
                else
                {
                    this.Text = value.Text;
                    if (value.Background != null)
                    {
                        this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = Math.Min(0.6, value.Background.A), Blue = value.Background.B, Green = value.Background.G, Red = value.Background.R };
                    }
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
                this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = 0, Blue = 255, Green = 255, Red = 255 };
                if (value == null)
                {
                    this.Active = false;
                }
                else
                {
                    this.Active = (value.Text == "1" || value.Text.ToLower() == "true") ? true : false;
                    if (value.Background != null)
                    {
                        this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = Math.Min(0.6, value.Background.A), Blue = value.Background.B, Green = value.Background.G, Red = value.Background.R };
                    }
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
                this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = 0, Blue = 255, Green = 255, Red = 255 };
                if (value == null)
                {
                    this.Text = string.Empty;
                }
                else
                {
                    this.Text = value.Text;
                    if (value.Background != null)
                    {
                        this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = Math.Min(0.6, value.Background.A), Blue = value.Background.B, Green = value.Background.G, Red = value.Background.R };
                    }
                }
            }
        }
    }
    public class CellRendererPixbufValue : CellRendererPixbuf
    {
        [Property("cellvalue")]
        public CellValue CellValue
        {
            set
            {
                this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = 0, Blue = 255, Green = 255, Red = 255 };
                if (value != null)
                {
                    if (value.Background != null)
                    {
                        this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = Math.Min(0.6, value.Background.A), Blue = value.Background.B, Green = value.Background.G, Red = value.Background.R };
                    }
                    if (string.IsNullOrEmpty(value.Text) == false && value.Text.IndexOf("/") > 0)
                    {
                        try
                        {
                            this.Pixbuf = new Gdk.Pixbuf(value.Text);
                        }
                        catch { }
                    }
                    else
                    {
                        this.IconName = value.Text;
                    }

                }
            }
        }
    }

    public class CellRendererButtonValue : CellRendererText
    {
        [Property("cellvalue")]
        public CellValue CellValue
        {
            set
            {
                this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = 0, Blue = 255, Green = 255, Red = 255 };
                if (value == null)
                {
                    this.Text = string.Empty;
                }
                else
                {
                    this.Text = value.Text;
                    if (value.Background != null)
                    {
                        this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = Math.Min(0.6, value.Background.A), Blue = value.Background.B, Green = value.Background.G, Red = value.Background.R };
                    }
                }
            }
        }

        protected override void OnRender(Cairo.Context cr, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, CellRendererState flags)
        {
            int x = (int)(cell_area.X + this.Xpad);
            int y = (int)(cell_area.Y + this.Ypad);
            int width = (int)(cell_area.Width - this.Xpad * 2);
            int height = (int)(cell_area.Height - this.Ypad * 2);
            width = Math.Max(10, width - 10);
            x = Math.Max(5, x + 5);
            widget.StyleContext.AddClass("GridViewCell-Button");
            widget.StyleContext.AddClass("BorderRadiusStyle");
            widget.StyleContext.Save();
            widget.StyleContext.RenderHandle(cr, x, y - 2, width, height + 4);
            widget.StyleContext.Restore();

            if (string.IsNullOrEmpty(this.Text))
                this.Text = "button";
            float textleng = 0;
            foreach (char w in this.Text)
            {
                if (char.IsLower(w) && char.IsLetter(w))
                    textleng += 0.5f;
                else if (char.IsDigit(w))
                    textleng += 0.5f;
                else
                    textleng += 1f;
            }
            int space = (int)Math.Max(16f, width - textleng*12-6);
            base.OnRender(cr, widget, new Gdk.Rectangle(background_area.X, background_area.Y, background_area.Width, background_area.Height), new Gdk.Rectangle(cell_area.X + space/2, cell_area.Y, cell_area.Width- space, cell_area.Height), flags);

        }
    }
    public class CellValue : IComparable, IComparable<CellValue>, IEquatable<CellValue>
    {
        public Drawing.Color Background { get; set; } = Drawing.Color.Transparent;

        public string Text { get; set; } = string.Empty;

        public int CompareTo(object obj)
        {
            return this.GetHashCode().CompareTo(obj.GetHashCode());
        }

        public int CompareTo([AllowNull] CellValue other)
        {
            return this.Text.CompareTo(other.Text);
        }

        public bool Equals([AllowNull] CellValue other)
        {
            return this.Text == other.Text;
        }

        public override string ToString()
        {
            return Text;
        }

    }

}
