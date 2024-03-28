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
using System.ComponentModel.Design;
using System.Drawing;
using System.Xml.Linq;


namespace System.Windows.Forms
{
    [Designer("System.Windows.Forms.Design.UserControlDocumentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [DesignerCategory("UserControl")]
    [DefaultEvent(nameof(Load))]
    public partial class UserControl : WidgetContainerControl<Gtk.Viewport>
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
        }
        public System.Drawing.SizeF AutoScaleDimensions { get; set; }
        public System.Windows.Forms.AutoScaleMode AutoScaleMode { get; set; }
        public override BorderStyle BorderStyle { get { return base.Control.ShadowType == Gtk.ShadowType.None ? BorderStyle.None : BorderStyle.FixedSingle; } set { base.Control.BorderWidth = 1; base.Control.ShadowType = Gtk.ShadowType.In; } }
        public override ControlCollection Controls => _controls;

        protected virtual void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
        }
        protected virtual void OnParentChanged(EventArgs e) { 
        }
    }
}
