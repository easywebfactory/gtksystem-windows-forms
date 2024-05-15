﻿/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class GroupBox : ContainerControl
    {
        public readonly GroupBoxBase self = new GroupBoxBase();
        public override object GtkControl => self;
        private Gtk.Fixed contaner = new Gtk.Fixed();
        private ControlCollection _controls = null;
        public GroupBox() : base()
        {
            _controls = new ControlCollection(this, contaner);
            self.Child = contaner;
        }

        public override string Text { get { return self.Label; } set { self.Label = value; } }
        public override ControlCollection Controls => _controls;

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
