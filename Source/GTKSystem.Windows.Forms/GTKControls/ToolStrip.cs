/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using Gtk;

namespace System.Windows.Forms
{
    public class ToolStrip : WidgetControl<Gtk.MenuBar>
    {
        public ToolStripItemCollection toolStripItemCollection;
        public ToolStrip() : base()
        {
            this.Control.StyleContext.AddClass("ToolStrip");
            this.Control.Hexpand = false;
            this.Control.Vexpand = false;
            this.Control.Valign = Gtk.Align.Start;
            this.Control.Halign = Gtk.Align.Start;
            this.Control.HeightRequest = 20;
            toolStripItemCollection = new ToolStripItemCollection(this);
            base.Control.ActivateCurrent += ToolStripItem_Activated;
            Dock = DockStyle.Top;
        }
        public ToolStrip(string owner) : base()
        {
            this.Control.StyleContext.AddClass("ToolStrip");
            this.Control.Hexpand = false;
            this.Control.Vexpand = false;
            this.Control.Valign = Gtk.Align.Start;
            this.Control.Halign = Gtk.Align.Start;
            toolStripItemCollection = new ToolStripItemCollection(this, owner);
            base.Control.ActivateCurrent += ToolStripItem_Activated;
        }
        private void ToolStripItem_Activated(object sender, ActivateCurrentArgs e)
        {
            if (Click != null)
            {
                Click(sender, e);
            }
            if (CheckedChanged != null)
            {
                CheckedChanged(this, e);
            }
            if (CheckStateChanged != null)
            {
                CheckStateChanged(this, e);
            }
            if (DropDownItemClicked != null)
            {
                DropDownItemClicked(this, new ToolStripItemClickedEventArgs(new ToolStripItem()));
            }
        }
        public override Size Size { get => base.Size; set => base.Size = new Size(value.Width, 30); }
        public ToolStripItemCollection Items
        {
            get
            {
                return toolStripItemCollection;
            }
        }

        public Size ImageScalingSize { get; set; }

        public override event EventHandler Click;
        public event EventHandler CheckedChanged;
        public event EventHandler CheckStateChanged;
        public event ToolStripItemClickedEventHandler DropDownItemClicked;
    }
}
