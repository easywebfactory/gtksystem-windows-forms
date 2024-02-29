using Cairo;
using Gdk;
using GLib;
using Gtk;
using System;
using System.Diagnostics.CodeAnalysis;
 
namespace System.Windows.Forms.GtkRender
{
    public class CellRendererValue : CellRendererText
    {
        [Property("cellvalue")]
        public CellValue CellValue
        {
            set
            {
                if (value == null)
                {
                    this.Text = string.Empty;
                }
                else
                {
                    this.Text = value.Text;
                    if (value.Background.Name != "0" && value.Background.Name != "transparent")
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
                if (value == null)
                {
                    this.Active = false;
                }
                else
                {
                    this.Active = (value.Text == "1" || value.Text.ToLower() == "true") ? true : false;
                    if (value.Background.Name != "0" && value.Background.Name != "transparent")
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
                if (value == null)
                {
                    this.Text = string.Empty;
                }
                else
                {
                    this.Text = value.Text;
                    if (value.Background.Name != "0" && value.Background.Name != "transparent")
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
                if (value != null)
                {
                    if (value.Background.Name != "0" && value.Background.Name != "transparent")
                    {
                        this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = Math.Min(0.6, value.Background.A), Blue = value.Background.B, Green = value.Background.G, Red = value.Background.R };
                    }
                    if (string.IsNullOrEmpty(value.Text) == false && value.Text.Contains("."))
                    {
                        try
                        {
                            this.Pixbuf = new Gdk.Pixbuf(value.Text);
                        }
                        catch {
                            this.IconName = "image-missing";
                        }
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
        public CellRendererButtonValue()
        {
            this.Alignment=Pango.Alignment.Center;
            this.Ellipsize = Pango.EllipsizeMode.End;
            this.WrapMode = Pango.WrapMode.Char;
        }
        [Property("cellvalue")]
        public CellValue CellValue
        {
            set
            {
                if (value == null)
                {
                    this.Text = string.Empty;
                }
                else
                {
                    this.Text = value.Text;
                    if (value.Background.Name != "transparent" && value.Background.Name != "0")
                    {
                        this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = Math.Min(0.6, value.Background.A), Blue = value.Background.B, Green = value.Background.G, Red = value.Background.R };
                    }
                }
            }
        }

        protected override void OnRender(Cairo.Context cr, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, CellRendererState flags)
        {
            widget.StyleContext.Save();
            if (widget.StyleContext.HasClass("GridViewCell-Button") == false)
                widget.StyleContext.AddClass("GridViewCell-Button");
            int height = cell_area.Height;
            int y = cell_area.Y;
            if (height > 36)
            {
                y = y + (cell_area.Height - 36) / 2;
                height = 36;
            }
            widget.StyleContext.RenderHandle(cr, cell_area.X + 3, y, cell_area.Width - 6, height);
            widget.StyleContext.Restore();
            widget.WidgetEvent += Widget_WidgetEvent;
            var textExt = cr.TextExtents(this.Text);
            int space = (int)(Math.Max(6f, cell_area.Width - textExt.Width) / 2 - 6);
            base.OnRender(cr, widget, new Gdk.Rectangle(background_area.X, background_area.Y, background_area.Width, background_area.Height), new Gdk.Rectangle(cell_area.X + space, cell_area.Y, cell_area.Width - space, cell_area.Height), flags);
        }
        private bool eventStarting = false;
        private void Widget_WidgetEvent(object o, WidgetEventArgs args)
        {
            if (args.Event.Type == EventType.ButtonPress)
            {
                eventStarting = true;
            }
            else if (args.Event.Type == EventType.ButtonRelease && eventStarting == true)
            {
                if (Click != null)
                    Click(o, args);
                eventStarting = false;
            }
        }
        public event WidgetEventHandler Click;
    }
    public class CellValue : IComparable, IComparable<CellValue>, IEquatable<CellValue>
    {
        public Drawing.Color Background { get; set; }// = Drawing.Color.Transparent;

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
