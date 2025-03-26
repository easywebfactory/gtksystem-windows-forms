/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;
using Container = Gtk.Container;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class RadioButton : Control
    {
        public readonly RadioButtonBase self = new();
        public override object GtkControl => self;

        public RadioButton() : base()
        {
            self.ParentSet += Self_ParentSet;
        }

        private void Self_ParentSet(object o, Gtk.ParentSetArgs args)
        {
            if (self.Parent is Container con)
            {
                foreach (var widget in con.Children)
                {
                    if (widget is Gtk.RadioButton group)
                    {
                        ((Gtk.RadioButton)o).Group = new Gtk.RadioButton[0];
                        // Add the first radio group in the container
                        ((Gtk.RadioButton)o).JoinGroup(group);
                        break;
                    }
                }
            }

            self.Active = Checked;
            self.Toggled += Self_Toggled;
        }

        private void Self_Toggled(object? sender, EventArgs e)
        {
            if (CheckedChanged != null && self.IsVisible)
                CheckedChanged(this, e);
        }

        public event EventHandler? CheckedChanged;

        public override string Text
        {
            get => self.Label;
            set => self.Label = value;
        }

        public bool Checked
        {
            get => self.Active;
            set
            {
                @checked = true;
                self.Active = true;
            }
        }

        private bool @checked;
    }
}