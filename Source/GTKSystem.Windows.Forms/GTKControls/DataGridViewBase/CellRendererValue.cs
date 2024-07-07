using Gdk;
using GLib;
using Gtk;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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
                if (value == null)
                {
                    this.Text = string.Empty;
                }
                else
                {
                    this.Text = value.Text;
                    if (value.Background.Name != "0" && value.Background.Name != "transparent")
                    {
                        this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = value.Background.A / 255, Blue = value.Background.B / 255, Green = value.Background.G / 255, Red = value.Background.R / 255 };
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
                        this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = value.Background.A / 255, Blue = value.Background.B / 255, Green = value.Background.G / 255, Red = value.Background.R / 255 };
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
                        this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = value.Background.A / 255, Blue = value.Background.B / 255, Green = value.Background.G / 255, Red = value.Background.R / 255 };
                    }
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
                    if (value.Background.Name != "0" && value.Background.Name != "transparent")
                    {
                        this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = value.Background.A / 255, Blue = value.Background.B / 255, Green = value.Background.G / 255, Red = value.Background.R / 255 };
                    }
                    if (string.IsNullOrWhiteSpace(value.Text))
                    {
                        this.IconName = "";
                    }
                    else
                    {
                        try
                        {
                            if (this.Data[value.Text] == null)
                            {
                                if (value.Text.Contains("://") && Uri.TryCreate(value.Text, UriKind.Absolute, out Uri result))
                                {
                                    System.Threading.Tasks.Task.Factory.StartNew(o =>
                                    {
                                        CellRendererPixbufValue cell = (CellRendererPixbufValue)o;
                                        try
                                        {
                                            GLib.IFile file = GLib.FileFactory.NewForUri(result);
                                            GLib.FileInputStream stream = file.Read(new GLib.Cancellable());
                                            Gdk.Pixbuf nbuf = new Gdk.Pixbuf(stream, Column.Width, GridView.RowTemplate.Height, true, null);
                                            cell.Data.Add(value.Text, nbuf);
                                            Gtk.Application.Invoke((o, s) =>
                                            {
                                                cell.Pixbuf = nbuf;
                                                GridView.self.GridView.IsFocus = true;
                                                GridView.self.GridView.ScrollToPoint(10, cell.Data.Count + 1);
                                            });
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    },this);
                                }
                                else if (value.Text.Contains("."))
                                {
                                    this.Data.Add(value.Text, new Gdk.Pixbuf(value.Text.Replace("\\\\", "/").Replace("\\", "/")));
                                    this.Pixbuf = this.Data[value.Text] as Gdk.Pixbuf;
                                }
                                else
                                {
                                    this.IconName = value.Text;
                                }
                            }
                            else
                            {
                                this.Pixbuf = this.Data[value.Text] as Gdk.Pixbuf;
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
                        this.CellBackgroundRgba = new Gdk.RGBA() { Alpha = value.Background.A / 255, Blue = value.Background.B / 255, Green = value.Background.G / 255, Red = value.Background.R / 255 };
                    }
                }
            }
        }
        protected override void OnRender(Cairo.Context cr, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, CellRendererState flags)
        {
            widget.StyleContext.Save();
            widget.StyleContext.AddClass("button");
            widget.StyleContext.AddClass("GridViewCell-Button");

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
            var textExt = cr.TextExtents(this.Text ?? string.Empty);
            int space = (int)(Math.Max(6f, cell_area.Width - textExt.Width) / 2 - 6);
            base.OnRender(cr, widget, new Gdk.Rectangle(background_area.X, background_area.Y, background_area.Width, background_area.Height), new Gdk.Rectangle(cell_area.X + space, cell_area.Y, cell_area.Width - space, cell_area.Height), flags);
        }

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
