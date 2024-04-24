/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class Label : WidgetControl<Label.GtkLabel>
    {
        public Label() : base() {
            this.Control.AddClass("Label");
            this.Control.Xalign = 0.08f;
            this.Control.Yalign = 0.08f;
        }

        public override string Text { get => base.Control.Text; set => base.Control.Text = value; }
        public override RightToLeft RightToLeft { get { return this.Control.Direction == Gtk.TextDirection.Rtl ? RightToLeft.Yes : RightToLeft.No; } set { this.Control.Direction = value == RightToLeft.Yes ?  Gtk.TextDirection.Rtl : Gtk.TextDirection.Ltr; } }
        public System.Drawing.ContentAlignment TextAlign { 
            get { return textAlign; } 
            set { 
                textAlign = value;
                if (value == System.Drawing.ContentAlignment.TopLeft)
                {
                    this.Control.Xalign = 0.08f;
                    this.Control.Yalign = 0.08f;
                }
                else if (value == System.Drawing.ContentAlignment.TopCenter)
                {
                    this.Control.Xalign = 0.5f; 
                    this.Control.Yalign = 0.08f;
                }
                else if (value == System.Drawing.ContentAlignment.TopRight)
                {
                    this.Control.Xalign = 0.92f;
                    this.Control.Yalign = 0.08f;
                }
                else if (value == System.Drawing.ContentAlignment.MiddleLeft)
                {
                    this.Control.Xalign = 0.08f;
                    this.Control.Yalign = 0.5f;
                }
                else if (value == System.Drawing.ContentAlignment.MiddleCenter)
                {
                    this.Control.Xalign = 0.5f;
                    this.Control.Yalign = 0.5f;
                }
                else if (value == System.Drawing.ContentAlignment.MiddleRight)
                {
                    this.Control.Xalign = 0.92f;
                    this.Control.Yalign = 0.5f;
                }
                else if (value == System.Drawing.ContentAlignment.BottomLeft)
                {
                    this.Control.Xalign = 0.08f;
                    this.Control.Yalign = 0.08f;
                }
                else if (value == System.Drawing.ContentAlignment.MiddleCenter)
                {
                    this.Control.Xalign = 0.5f;
                    this.Control.Yalign = 0.08f;
                }
                else if (value == System.Drawing.ContentAlignment.MiddleRight)
                {
                    this.Control.Xalign = 0.92f;
                    this.Control.Yalign = 0.08f;
                }

            }
        }
        private System.Drawing.ContentAlignment textAlign;
        public override Drawing.Image BackgroundImage { get => base.Control.BackgroundImage; set => base.Control.BackgroundImage = value; }
        public override Color BackColor { get => base.Control.BackColor.HasValue ? base.Control.BackColor.Value : Color.Transparent; set => base.Control.BackColor = value; }
        public override event PaintEventHandler Paint
        {
            add { this.Control.Paint += value; }
            remove { this.Control.Paint -= value; }
        }
        public sealed class GtkLabel : Gtk.Label
        {
            public event DrawnHandler DrawnBackground;
            public event PaintEventHandler Paint;
            public Drawing.Color? BackColor { get; set; }
            public Drawing.Image BackgroundImage { get; set; }
            private List<string> cssList = new List<string>();
            public void AddClass(string cssClass)
            {
                cssList.Add(cssClass);
            }
            protected override void OnShown()
            {
                if (BackColor.HasValue == false && BackgroundImage == null)
                {
                    foreach (string cssClass in cssList)
                    {
                        this.StyleContext.AddClass(cssClass);
                    }
                }
                else
                {
                    this.StyleContext.AddClass("BackgroundTransparent");
                }
                base.OnShown();
            }
            protected override bool OnDrawn(Cairo.Context cr)
            {
                DrawnArgs args = new DrawnArgs() { Args = new object[] { cr } };
                if (DrawnBackground != null)
                {
                    DrawnBackground(this, args);
                }
                Gdk.Rectangle rec = this.Allocation;
                OnDrawnBackground(cr, rec);
                OnPaint(cr, rec);
                return base.OnDrawn(cr);
            }
            public void OnDrawnBackground(Cairo.Context cr, Gdk.Rectangle area)
            {
                if (BackColor.HasValue)
                {
                    cr.Save();
                    cr.SetSourceRGB(BackColor.Value.R / 255f, BackColor.Value.G / 255f, BackColor.Value.B / 255f);
                    cr.Rectangle(2, 2, area.Width - 4, area.Height - 4);
                    cr.Fill();
                    cr.Restore();
                }
                if (BackgroundImage != null)
                {
                    Gdk.Pixbuf img = new Gdk.Pixbuf(BackgroundImage.PixbufData);
                    cr.Save();
                    cr.Translate(4, 4);
                    Gdk.CairoHelper.SetSourcePixbuf(cr, img, 0, 0);
                    using (var p = cr.GetSource())
                    {
                        if (p is Cairo.SurfacePattern pattern)
                        {
                            if (area.Width > img.Width || area.Height > img.Height)
                            {
                                pattern.Filter = Cairo.Filter.Fast;
                            }
                            else
                                pattern.Filter = Cairo.Filter.Good;
                        }
                    }
                    cr.Paint();
                    cr.Restore();
                }
            }
            public void OnPaint(Cairo.Context cr, Gdk.Rectangle area)
            {
                if (Paint != null)
                    Paint(this, new PaintEventArgs(new Graphics(this, cr, area), new Drawing.Rectangle(area.X, area.Y, area.Width, area.Height)));
            }
        }
    }
}
