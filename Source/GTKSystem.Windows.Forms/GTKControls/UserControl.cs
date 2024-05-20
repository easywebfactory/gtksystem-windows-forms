/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Drawing;


namespace System.Windows.Forms
{
    [DesignerCategory("UserControl")]
    [ToolboxItem(true)]
    public partial class UserControl : ContainerControl
    {
        public readonly UserControlBase self = new UserControlBase();
        public override object GtkControl => self;
        private Gtk.Layout contaner;
        private ControlCollection _controls;

        public UserControl() : base()
        {
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

            self.Child = contaner;
            //self.Drawn += Control_Drawn;
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
        public override BorderStyle BorderStyle { get { return self.ShadowType == Gtk.ShadowType.None ? BorderStyle.None : BorderStyle.FixedSingle; } set { self.BorderWidth = 1; self.ShadowType = Gtk.ShadowType.In; } }
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
    }
}
