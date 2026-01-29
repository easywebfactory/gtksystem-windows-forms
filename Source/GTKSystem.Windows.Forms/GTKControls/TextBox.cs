/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class TextBox: Control
    {
        public readonly TextBoxBase self = new TextBoxBase();
        public override object GtkControl => self;
        public TextBox() : base()
        {
            self.Override.sender = this;
            self.MaxWidthChars = 1;
            self.WidthChars = 0;
            self.Valign = Gtk.Align.Start;
            self.Halign = Gtk.Align.Start;
            self.Changed += Self_Changed;
        }
        private void Self_Changed(object sender, EventArgs e)
        {
            if (TextChanged != null && self.IsVisible) { TextChanged(this, EventArgs.Empty); }
        }

        public string PlaceholderText { get { return self.PlaceholderText; } set { self.PlaceholderText = value ?? ""; } }
        public override string Text { get { return self.Text; } set { self.Text = value ?? ""; } }
        public virtual char PasswordChar { get => self.InvisibleChar; set { self.InvisibleChar = value; self.Visibility = false; } }
        public HorizontalAlignment TextAlign
        {
            get { return textAlign; }
            set
            {
                textAlign = value;
                if (value == HorizontalAlignment.Left)
                {
                    self.Xalign = 0.0f;
                }
                else if (value == HorizontalAlignment.Center)
                {
                    self.Xalign = 0.5f;
                }
                else if (value == HorizontalAlignment.Right)
                {
                    self.Xalign = 1.0f;
                }
            }
        }
        private HorizontalAlignment textAlign;
        public virtual bool ReadOnly { get { return self.IsEditable == false; } set { self.IsEditable = value == false; } }
        public override event EventHandler TextChanged;
        public bool Multiline { get; set; }
        public int MaxLength { get => self.MaxLength; set => self.MaxLength = value; }
        public string[] Lines
        {
            get
            {
                return [self.Text];
            }
            set
            {
                if (value is null || value.Length == 0)
                {
                    Text = string.Empty;
                }
                else
                {
                    Text = string.Join(string.Empty, value);
                }
            }
        }
        public int SelectionStart { 
            get { self.GetSelectionBounds(out int start, out int end); return start; }
            set { Select(value, SelectionLength); }
        }
        public virtual int SelectionLength
        {
            get { self.GetSelectionBounds(out int start, out int end); return end - start; }
            set
            {
                Select(SelectionStart, value);
            }
        }
        public void Select(int start, int length)
        {
            self.SelectRegion(start, start + length);
        }
        public string SelectedText
        {
            get
            {
                if (self.GetSelectionBounds(out int start, out int end))
                    return self.Text.Substring(start, end - start);
                else
                    return string.Empty;
            }
            set
            {
                if (self.GetSelectionBounds(out int start, out int end))
                {
                    self.DeleteSelection();
                    self.InsertText(value, ref start);
                }
            }
        }
        public void InsertTextAtCursor(string text)
        {
            if(text == null) return;
            int posi = self.CursorPosition;
            self.InsertText(text,ref posi);
        }
        public void SelectAll()
        {
            Select(0, -1);
        }
        public void DeselectAll()
        {
            SelectionLength = 0;
        }
    }
}
