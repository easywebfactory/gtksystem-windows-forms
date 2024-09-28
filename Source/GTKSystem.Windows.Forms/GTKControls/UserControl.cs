/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Xml.Linq;


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
            self.Add(contaner);
            self.Override.Paint += Override_Paint;
            self.ParentSet += Self_ParentSet;
            self.Realized += (sender, e) =>
            {
                base.InitStyle((Gtk.Widget)sender);
                OnLoad(EventArgs.Empty);
            };
        }
        private void Self_ParentSet(object o, ParentSetArgs args)
        {
            OnParentChanged(EventArgs.Empty);
        }

        private void Override_Paint(object sender, PaintEventArgs e)
        {
            OnPaint(e);
        }

        /// <summary>
        /// 在控件第一次变为可见之前发生。
        /// </summary>
        public virtual event EventHandler Load;
        protected virtual void OnLoad(EventArgs e) => Load?.Invoke(this, e);

        public System.Drawing.SizeF AutoScaleDimensions { get; set; }
        public System.Windows.Forms.AutoScaleMode AutoScaleMode { get; set; }
        public override ControlCollection Controls => _controls;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
        }
        public override void SuspendLayout()
        {
            base.SuspendLayout();
        }
        public override void ResumeLayout(bool performLayout)
        {
            base.ResumeLayout(performLayout);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
