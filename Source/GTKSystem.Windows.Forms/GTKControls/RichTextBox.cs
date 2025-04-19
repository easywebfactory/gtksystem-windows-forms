/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using GLib;
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Text;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class RichTextBox : ScrollableControl
    {
        public readonly RichTextBoxBase self = new RichTextBoxBase();
        public override object GtkControl => self;
        protected override void SetStyle(Widget widget)
        {
            self.TextView.Name = this.Name;
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
            get {
                return self.TextView.Buffer.Text.Split([Environment.NewLine], StringSplitOptions.None); 
            }
            set
            {
                if (value is null || value.Length == 0)
                {
                    Text = string.Empty;
                }
                else
                {
                    Text = string.Join(Environment.NewLine, value);
                }
            }
        }

        public int SelectionStart { 
            get { self.TextView.Buffer.GetSelectionBounds(out TextIter start, out TextIter end); return start.Offset; }
            set { Select(value, SelectionLength); }
        }
        
        [System.ComponentModel.Browsable(false)]
        public virtual int SelectionLength
        {
            get { self.TextView.Buffer.GetSelectionBounds(out TextIter start, out TextIter end); return end.Offset - start.Offset; }
            set
            {
                Select(SelectionStart, value);
            }
        }
        public void Select(int start, int length)
        {
            TextIter startiter = self.TextView.Buffer.GetIterAtOffset(start);
            TextIter enditer = self.TextView.Buffer.GetIterAtOffset(start + length);
            self.TextView.Buffer.SelectRange(startiter, enditer);
        }
        public void InsertTextAtCursor(string text)
        {
            if (text == null) return;
            self.TextView.Buffer.InsertAtCursor(text);
        }
    }
}
