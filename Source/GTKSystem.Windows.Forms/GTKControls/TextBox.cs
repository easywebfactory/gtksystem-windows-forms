/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Linq;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class TextBox: Control
    {
        public readonly TextBoxBase self = new TextBoxBase();
        public override object GtkControl => self;
        public TextBox() : base()
        {
            self.MaxWidthChars = 1;
            self.WidthChars = 0;

            self.Valign = Gtk.Align.Start;
            self.Halign = Gtk.Align.Start;
            self.Changed += Self_Changed;
            self.TextInserted += Self_TextInserted;
            self.KeyPressEvent += Self_KeyPressEvent;
        }

        private void Self_KeyPressEvent(object o, Gtk.KeyPressEventArgs args)
        {
            if (KeyDown != null)
            {
                if (args.Event is Gdk.EventKey eventkey)
                {
                    Keys keys = (Keys)eventkey.HardwareKeycode;
                    KeyDown(this, new KeyEventArgs(keys));
                }
            }
        }

        public override event KeyEventHandler KeyDown;
        private void Self_TextInserted(object o, TextInsertedArgs args)
        {
            if (KeyDown != null && this.GetType().Name == "TextBox")
            {
                string keytext = args.NewText.ToUpper();
                if (char.IsNumber(args.NewText[0]))
                    keytext = "D" + keytext;
                var keyv = Enum.GetValues(typeof(Keys)).Cast<Keys>().Where(k => {
                    return Enum.GetName(typeof(Keys), k) == keytext;
                });
                foreach (var key in keyv) 
                    KeyDown(this, new KeyEventArgs(key));
            }
        }

        private void Self_Changed(object sender, EventArgs e)
        {
            if (TextChanged != null && self.IsVisible) { TextChanged(this, EventArgs.Empty); }
        }

        public string PlaceholderText { get { return self.PlaceholderText; } set { self.PlaceholderText = value ?? ""; } }
        public override string Text { get { return self.Text; } set { self.Text = value ?? ""; } }
        public virtual char PasswordChar { get => self.InvisibleChar; set { self.InvisibleChar = value; self.Visibility = false; } }
        public virtual bool ReadOnly { get { return self.IsEditable == false; } set { self.IsEditable = value == false;  } }
        public override event EventHandler TextChanged;
        public bool Multiline { get; set; }
        public int SelectionStart { get { self.GetSelectionBounds(out int start, out int end); return start; } }

        [System.ComponentModel.Browsable(false)]
        public virtual int SelectionLength
        {
            get { self.GetSelectionBounds(out int start, out int end); return end - start; }
            set
            {
                self.SelectRegion(self.CursorPosition, self.CursorPosition + value);
            }
        }
        public void InsertTextAtCursor(string text)
        {
            if(text == null) return;
            int posi = self.CursorPosition;
            self.InsertText(text,ref posi);
        }
    }
}
