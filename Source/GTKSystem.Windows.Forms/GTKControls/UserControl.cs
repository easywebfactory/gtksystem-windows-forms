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
using System.ComponentModel.Design;
using System.Drawing;
using System.Xml.Linq;


namespace System.Windows.Forms
{
    [DesignerCategory("UserControl")]
    [DefaultEvent(nameof(Load))]
    public partial class UserControl : WidgetContainerControl<UserControl.GtkViewport>
    {
        private Gtk.Layout contaner;
        private ControlCollection _controls;

        public UserControl() : base()
        {
            base.Control.MarginStart = 0;
            base.Control.MarginTop = 0;
            base.Control.BorderWidth = 0;
            base.Control.Halign = Align.Start;
            base.Control.Valign = Align.Start;
            base.Control.Expand = false;
            base.Control.Hexpand = false;
            base.Control.Vexpand = false;
            base.Control.StyleContext.AddClass("UserControl");
            contaner = new Gtk.Layout(new Gtk.Adjustment(IntPtr.Zero), new Gtk.Adjustment(IntPtr.Zero));
            contaner.MarginStart = 0;
            contaner.MarginTop = 0;
            contaner.BorderWidth = 0;
            contaner.Halign = Align.Fill;
            contaner.Valign = Align.Fill;
            contaner.Expand = true;
            contaner.Hexpand = true;
            contaner.Vexpand = true;
            _controls = new ControlCollection(this, contaner);

            base.Control.Child = contaner;
            base.Control.DrawnBackground += Control_DrawnBackground;
            base.Control.Drawn += Control_Drawn;
        }

        private void Control_DrawnBackground(object o, DrawnArgs args)
        {
            Gdk.Rectangle rec = Widget.Allocation;
            Graphics g = new Graphics(this.Widget, args.Cr, rec);
            using SolidBrush brush = new SolidBrush(Color.Yellow);
            g.FillRectangle(brush, new Rectangle(0, 0, rec.Width, rec.Height));
            using SolidBrush brush2 = new SolidBrush(Color.DarkSlateBlue);
            g.DrawEllipse(new Pen(brush2,5), new Rectangle(20, 50, 50, 30));
        }

        public Color _backgroundColor = Color.White;
        [Category("Appearance")]
        [Description("mytest")]
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set { _backgroundColor = value; Invalidate(); }
        }
        private void Control_Drawn(object o, DrawnArgs args)
        {
            Gdk.Rectangle rec = Widget.Allocation;
            PaintEventArgs paintEventArgs = new PaintEventArgs(new Graphics(this.Widget, args.Cr, rec), new Drawing.Rectangle(rec.X, rec.Y, rec.Width, rec.Height));

            OnPaint(paintEventArgs);
        }

        public override event EventHandler Load;
        public System.Drawing.SizeF AutoScaleDimensions { get; set; }
        public System.Windows.Forms.AutoScaleMode AutoScaleMode { get; set; }
        public override BorderStyle BorderStyle { get { return base.Control.ShadowType == Gtk.ShadowType.None ? BorderStyle.None : BorderStyle.FixedSingle; } set { base.Control.BorderWidth = 1; base.Control.ShadowType = Gtk.ShadowType.In; } }
        public override ControlCollection Controls => _controls;

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
        }
        protected override void OnParentChanged(EventArgs e)
        {
        }
        public override void SuspendLayout()
        {

        }
        public override void ResumeLayout(bool performLayout)
        {

        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        public override Drawing.Image BackgroundImage { get => base.Control.BackgroundImage; set => base.Control.BackgroundImage = value; }
        public override Color BackColor { get => base.Control.BackColor.HasValue ? base.Control.BackColor.Value : Color.Transparent; set => base.Control.BackColor = value; }
        public override event PaintEventHandler Paint
        {
            add { this.Control.Paint += value; }
            remove { this.Control.Paint -= value; }
        }
        public sealed class GtkViewport : Gtk.Viewport
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
