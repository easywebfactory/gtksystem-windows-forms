/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using System.Text.RegularExpressions;

namespace System.Windows.Forms
{
    public class MaskedTextBox : TextBox
    {
        public MaskedTextBox() : base()
        {
            self.AddClass("MaskedTextBox");
            self.Backspace += Control_Backspace;
            self.TextInserted += Control_TextInserted;
            self.Shown += Control_Shown;
        }
        internal MaskedTextBox(string cssClass) : base()
        {
            self.AddClass(cssClass);
            self.Backspace += Control_Backspace;
            self.TextInserted += Control_TextInserted;
            self.Shown += Control_Shown;
        }
        public override string Text { get { return self.Text; } set { IsMasking = false; self.Text = value ?? ""; IsMasking = true; } }
        private void Control_Shown(object sender, EventArgs e)
        {
            if (_PasswordChar != '\0')
            {
                self.Visibility = false;
            }
            else if (!string.IsNullOrWhiteSpace(Mask))
            {//按格式化赋值
                string txt = Regex.Match(Text, "\\d").Value;
                int windex = -1;
                Text = Regex.Replace(Mask, "\\d", (Match m) =>
                {
                    windex++;
                    if (txt.Length > windex)
                    {
                        return txt[windex].ToString();
                    }
                    else
                    {
                        return "_";
                    }
                });
            }
        }
        string correctText;
        private void Control_TextInserted(object o, Gtk.TextInsertedArgs args)
        {
            if (self.IsRealized && isBackspace == false)
            {
                int position = args.Position;
                string new_text = args.NewText;
                if (IsMaskPassword == true)
                {
                    if (new_text.Length > 1 || self.Text.Length != correctText.Length + 1)
                    {
                        if (correctText != null)
                            self.Text = correctText;
                    }
                    else if (correctText.Length > position && new_text.Length == 1)
                    {
                        if (IsNumberText(correctText.Substring(position - 1, 1)) && IsNumberText(new_text))
                        {
                            //正常
                            self.DeleteText(position, position + 1);
                        }
                        else
                        {
                            self.Text = correctText;
                        }
                    }
                    else
                    {
                        self.DeleteText(position - 1, position);
                    }
                }
            }
            isBackspace = false;
            correctText = self.Text;
        }
        bool isBackspace = false;
        private void Control_Backspace(object sender, EventArgs e)
        {
            if (IsMaskPassword == true)
            {
                //格式化掩码，只改数字
                int position = self.CursorPosition;
                if (self.Text.Length + 1 == correctText.Length) //删除一个字符
                {
                    isBackspace = true;
                    if (IsNumberChar(correctText[position]))
                    {
                        self.InsertText("_", ref position);
                    }
                    else
                    {
                        self.InsertText(correctText[position].ToString(), ref position);
                    }
                }
                else if (self.Text.Length + 1 < correctText.Length) //选择多字符删除
                {
                    self.Text = correctText;
                }
            }
        }

        private bool IsNumberText(string text)
        {
            foreach (char w in text)
            {
                if (!IsNumberChar(w))
                    return false;
            }
            return true;
        }
        private bool IsNumberChar(char w)
        {
            return (char.IsNumber(w) || w == '_');
        }

        public string Mask { get; set; }
        private char _PasswordChar;
        public override char PasswordChar { get => _PasswordChar; set { _PasswordChar = value; self.InvisibleChar = value; } }
        public Type ValidatingType { get; set; }
        public MaskFormat TextMaskFormat { get; set; }
        internal bool IsMasking = true;
        private bool IsMaskPassword
        {
            get { return !string.IsNullOrWhiteSpace(Mask) && IsMasking; }
        }

        public string[] Lines => string.IsNullOrEmpty(Text) ? new string[0] : Text.Replace("\r\n", "\n").Split('\n');
    }
}
