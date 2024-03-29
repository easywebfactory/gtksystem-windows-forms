﻿/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */

using Gtk;
using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class SplitContainer : WidgetContainerControl<Gtk.Paned>
    {
        public SplitContainer() : base(Gtk.Orientation.Vertical)
        {
            Widget.StyleContext.AddClass("SplitContainer");
            base.Control.BorderWidth = 1;
            base.Control.WideHandle = true;
            //base.Control.PositionSet = false;

            base.Control.Orientation = Gtk.Orientation.Horizontal;

            _panel1 = new SplitterPanel(this);
            _panel2 = new SplitterPanel(this);
            //_panel1.Control.Hexpand = true;
            //_panel1.Control.Vexpand = true;
            //_panel1.Control.Halign = Gtk.Align.Fill;
            //_panel1.Control.Valign = Gtk.Align.Fill;
            //_panel2.Control.Hexpand = true;
            //_panel2.Control.Vexpand = true;
            //_panel2.Control.Halign = Gtk.Align.Fill;
            //_panel2.Control.Valign = Gtk.Align.Fill;


            base.Control.Add1(_panel1.Control);
            base.Control.Add2(_panel2.Control);

            base.Control.Realized += Control_Realized;
        }

        private void Control_Realized(object sender, EventArgs e)
        {
            if (base.Control.Orientation == Gtk.Orientation.Horizontal)
            {
                _panel1.Width = this.SplitterDistance;
            }
            else
            {
                _panel1.Height = this.SplitterDistance;
            }
        }

        private SplitterPanel _panel1;
        private SplitterPanel _panel2;

        public override BorderStyle BorderStyle { get { return base.Control.BorderWidth == 1 ? BorderStyle.FixedSingle : BorderStyle.None; } set { base.Control.BorderWidth = 1; } }
        public SplitterPanel Panel1
        {
            get
            {
                return _panel1;
            }
            set
            {
                _panel1 = value;
            }
        }
        public SplitterPanel Panel2 {
            get
            {
                return _panel2;
            }
            set
            {
                _panel2 = value;
            }
        }

        public int SplitterDistance { get; set; }
        private int _SplitterWidth;
        public int SplitterWidth { get { return _SplitterWidth; } set { _SplitterWidth = value; base.Control.WideHandle = value > 2; } }
        public int SplitterIncrement { get; set; }
        public Orientation Orientation {
            get { return base.Control.Orientation == Gtk.Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal; }
            set {
                base.Control.Orientation = value == Orientation.Horizontal ? Gtk.Orientation.Vertical : Gtk.Orientation.Horizontal;
            }
        }
    }
}
