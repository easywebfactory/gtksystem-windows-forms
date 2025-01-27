/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class RichTextBox : ScrollableControl
    {
        public readonly RichTextBoxBase self = new RichTextBoxBase();
        public override object GtkControl => self;
        protected override void SetStyle(Widget widget)
        {
            base.SetStyle(self.TextView);
        }
        public RichTextBox():base()
        {
            self.TextView.Buffer.Changed += Buffer_Changed;
            this.BorderStyle = BorderStyle.Fixed3D;
        }
        private void Buffer_Changed(object sender, EventArgs e)
        {
            if (TextChanged != null && self.IsVisible)
            {
                TextChanged(this, e);
            }
        }

        public override string Text { get => self.TextView.Buffer.Text; set => self.TextView.Buffer.Text = value; }
        public virtual bool ReadOnly { get { return self.TextView.CanFocus; } set { self.TextView.CanFocus = value; } }

        public override event EventHandler TextChanged;
        public void AppendText(string text)
        {
            var enditer = self.TextView.Buffer.EndIter;
            self.TextView.Buffer.Insert(ref enditer, text);
        }

        public string[] Lines
        {
            get { return self.TextView.Buffer.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None); }
        }
    }
}
