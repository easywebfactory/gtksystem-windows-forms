/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Drawing;

namespace System.Windows.Forms
{
    public class ToolStrip : Control
    {
        public readonly ToolStripBase self = new ToolStripBase();
        public override object GtkControl => self;
        public ToolStripItemCollection toolStripItemCollection;
        public ToolStrip() : base()
        {
            toolStripItemCollection = new ToolStripItemCollection(this);
            self.ActivateCurrent += ToolStripItem_Activated;
            //Dock = DockStyle.Top;
        }
        public ToolStrip(string owner) : base()
        {
            self.Hexpand = false;
            self.Vexpand = false;
            self.Valign = Gtk.Align.Start;
            self.Halign = Gtk.Align.Start;
            toolStripItemCollection = new ToolStripItemCollection(this, owner);
            self.ActivateCurrent += ToolStripItem_Activated;
            //Dock = DockStyle.Top;
        }
        private void ToolStripItem_Activated(object sender, ActivateCurrentArgs e)
        {
            if (DropDownItemClicked != null)
            {
                DropDownItemClicked(this, new ToolStripItemClickedEventArgs(new ToolStripItem()));
            }
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
        public ToolStripLayoutStyle LayoutStyle { get; set; }
        public override event EventHandler Click;
        public event EventHandler CheckedChanged;
        public event EventHandler CheckStateChanged;
        public event ToolStripItemClickedEventHandler DropDownItemClicked;
    }
}
