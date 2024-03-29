﻿/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class RichTextBox : WidgetControl<Gtk.Viewport>
    {
        private Gtk.TextView textView = new Gtk.TextView();
        public RichTextBox():base()
        {
            Widget.StyleContext.AddClass("RichTextBox");
            textView.BorderWidth = 1;
            textView.WrapMode = Gtk.WrapMode.Char;
            textView.Halign = Gtk.Align.Fill;
            textView.Valign = Gtk.Align.Fill;
            textView.Expand = true;
            Gtk.ScrolledWindow scrolledWindow = new Gtk.ScrolledWindow();
            scrolledWindow.Child = textView;
            this.Control.Child = scrolledWindow;
        }
        protected override void SetStyle(Widget widget)
        {
            base.SetStyle(textView);
        }
        public override string Text { get => textView.Buffer.Text; set => textView.Buffer.Text = value; }

        public override event EventHandler TextChanged
        {
            add { textView.Buffer.Changed += (object o, EventArgs args) => { value.Invoke(this, args); }; }
            remove { textView.Buffer.Changed -= (object o, EventArgs args) => { value.Invoke(this, args); }; }
        }

        public void AppendText(string text)
        {
            var enditer = textView.Buffer.EndIter;
            textView.Buffer.Insert(ref enditer, text);
        }

        public string[] Lines
        {
            get { return textView.Buffer.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None); }
        }
        public override Color BackColor { get => base.BackColor; set => base.BackColor = value; }
    }
}
