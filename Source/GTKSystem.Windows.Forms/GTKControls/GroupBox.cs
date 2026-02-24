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

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class GroupBox : ContainerControl
    {
        public readonly GroupBoxBase self = new GroupBoxBase();
        public override object GtkControl => self;
        private Gtk.Overlay contaner = new Gtk.Overlay();
        private ControlCollection _controls = null;
        private Gtk.ScrolledWindow fixedcontaner = new Gtk.ScrolledWindow();
        public GroupBox() : base()
        {
            self.Override.sender = this;
            _controls = new ControlCollection(this, contaner);
            contaner.Halign = Align.Fill;
            contaner.Valign = Align.Fill;
            Gtk.DrawingArea background = new Gtk.DrawingArea();
            background.Events = Gdk.EventMask.EnterNotifyMask;
            background.Drawn += Background_Drawn;
            contaner.Add(background);
            fixedcontaner.SizeAllocated += Fixedcontaner_SizeAllocated;
            fixedcontaner.WidgetEvent += Fixedcontaner_WidgetEvent;
            fixedcontaner.HscrollbarPolicy = PolicyType.Never;
            fixedcontaner.VscrollbarPolicy = PolicyType.External;
            fixedcontaner.Add(contaner);
            self.Child = fixedcontaner;
        }

        private void Fixedcontaner_WidgetEvent(object o, WidgetEventArgs args)
        {
            fixedcontaner.Vadjustment.Value = 20;
            args.RetVal = true;
        }

        private void Fixedcontaner_SizeAllocated(object o, SizeAllocatedArgs args)
        {
            fixedcontaner.Vadjustment.Value = 20;
        }
        public override Size Size
        {
            get => base.Size;
            set
            {
                base.Size = value;
                contaner.WidthRequest = Math.Max(1, value.Width - fixedcontaner.MarginStart - fixedcontaner.MarginEnd);
                contaner.HeightRequest = Math.Max(1, value.Height - fixedcontaner.MarginTop - fixedcontaner.MarginBottom);
                fixedcontaner.Vadjustment.Value = 20;
            }
        }
        private void Background_Drawn(object o, DrawnArgs args)
        {
            self.Override.OnPaint(args.Cr);
        }
        public override string Text { get { return self.Label; } set { self.Label = value; } }
        public override ControlCollection Controls => _controls;
        public override Padding Padding
        {
            get => base.Padding;
            set
            {
                base.Padding = value;
                fixedcontaner.MarginStart = value.Left;
                fixedcontaner.MarginTop = value.Top;
                fixedcontaner.MarginEnd = value.Right;
                fixedcontaner.MarginBottom = value.Bottom;
            }
        }
        public override void SuspendLayout()
        {
            _Created = false;
        }
        public override void ResumeLayout(bool resume)
        {
            _Created = resume == false;
        }

        public override void PerformLayout()
        {
            _Created = true;
        }

    }
}
