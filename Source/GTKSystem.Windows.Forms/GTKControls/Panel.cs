/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using GLib;
using Gtk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class Panel : WidgetContainerControl<Panel.GtkPanel>
    {
        private Gtk.Fixed contaner = new Gtk.Fixed();
        private Gtk.ScrolledWindow scrolledwindow = new Gtk.ScrolledWindow();
        private ControlCollection _controls;

        public Panel() : base()
        {
            this.Control.owner = this;
            this.Control.AddClass("Panel");
            base.Control.MarginStart = 0;
            base.Control.MarginTop = 0;
            base.Control.ShadowType = Gtk.ShadowType.In;
            base.Control.BorderWidth = 0;
            _controls = new ControlCollection(this, contaner);

            contaner.MarginStart = 0;
            contaner.MarginTop = 0;
            contaner.Halign = Align.Fill;
            contaner.Valign = Align.Fill;

            scrolledwindow.Halign = Align.Fill;
            scrolledwindow.Valign = Align.Fill;
            scrolledwindow.VscrollbarPolicy = PolicyType.Never;
            scrolledwindow.HscrollbarPolicy = PolicyType.Never;
            scrolledwindow.Child = contaner;
            base.Control.Child = scrolledwindow;
        }
        public override BorderStyle BorderStyle { get { return base.Control.ShadowType == Gtk.ShadowType.None ? BorderStyle.None : BorderStyle.FixedSingle; } set { base.Control.BorderWidth = 1; base.Control.ShadowType = Gtk.ShadowType.In; } }
        public override ControlCollection Controls => _controls;
        public override bool AutoScroll { 
            get => base.AutoScroll; 
            set { 
                base.AutoScroll = value;
                if (value == true)
                {
                    scrolledwindow.VscrollbarPolicy = PolicyType.Automatic;
                    scrolledwindow.HscrollbarPolicy = PolicyType.Automatic;
                }
            } 
        }

        public override Drawing.Image BackgroundImage { get => base.Control.BackgroundImage; set => base.Control.BackgroundImage = value; }
        public override Color BackColor { get => base.Control.BackColor.HasValue ? base.Control.BackColor.Value : Color.FromName("0"); set => base.Control.BackColor = value; }

        public override event PaintEventHandler Paint
        {
            add { this.Control.Paint += value; }
            remove { this.Control.Paint -= value; }
        }
        public sealed class GtkPanel : Gtk.Viewport
        {
            public Panel owner { set; get; }
            public GtkPanel(Panel owner) : base()
            {
                this.owner = owner;
            }
            public GtkPanel() : base()
            {
            }

            public event DrawnHandler DrawnBackground;
            public event PaintEventHandler Paint;
            public Drawing.Color? BackColor { get; set; } //= ColorTranslator.FromHtml("#FBFBFB"); //"{Name = Control, ARGB = (255, 240, 240, 240)}"
            public Drawing.Image BackgroundImage { get; set; }
            private Gdk.Pixbuf backgroundPixbuf;
            private List<string> cssList = new List<string>();
            public void AddClass(string cssClass)
            {
                cssList.Add(cssClass);
            }
            protected override void OnRealized()
            {
                if (BackColor.HasValue == false && BackgroundImage == null)
                {
                    foreach (string cssClass in cssList)
                        this.StyleContext.AddClass(cssClass);
                }
                else
                {
                    this.StyleContext.AddClass("BackgroundTransparent");
                }
                base.OnRealized();
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

            protected override void OnResizeChecked()
            {
                base.OnResizeChecked();
                
                if (BackgroundImage != null && owner != null)
                {
                    Gdk.Rectangle area = this.Allocation;
                    if (backgroundPixbuf == null || backgroundPixbuf.Width != area.Width || backgroundPixbuf.Height != area.Height)
                    {
                        backgroundPixbuf = new Gdk.Pixbuf(new Cairo.ImageSurface(Cairo.Format.ARGB32, area.Width, area.Height),0,0, area.Width, area.Height);
                        owner.ScaleImage(area.Width, area.Height, ref backgroundPixbuf, BackgroundImage.PixbufData, PictureBoxSizeMode.AutoSize, owner.BackgroundImageLayout == ImageLayout.None ? ImageLayout.Tile : owner.BackgroundImageLayout);
                    }
                }
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

                if (backgroundPixbuf != null && owner != null)
                {
                    cr.Save();
                    cr.Translate(4, 4);
                    Gdk.CairoHelper.SetSourcePixbuf(cr, backgroundPixbuf, 0, 0);
                    using (var p = cr.GetSource())
                    {
                        if (p is Cairo.SurfacePattern pattern)
                        {
                            if (area.Width > backgroundPixbuf.Width || area.Height > backgroundPixbuf.Height)
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
