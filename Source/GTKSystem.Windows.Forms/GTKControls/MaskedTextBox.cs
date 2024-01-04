/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System;
using System.Text.RegularExpressions;

namespace System.Windows.Forms
{
    public class MaskedTextBox: TextBox
    {
        public MaskedTextBox():base()
        {
            Widget.StyleContext.AddClass("MaskedTextBox");
            Widget.StyleContext.AddClass("BorderRadiusStyle");
            base.Control.Backspace += Control_Backspace;
            base.Control.TextInserted += Control_TextInserted;
            base.Control.Shown += Control_Shown;
        }
        public override string Text { get { return base.Control.Text; } set { base.Control.Text = value; } }
        private void Control_Shown(object sender, EventArgs e)
        {
            if (_PasswordChar != '\0')
            {
                base.Control.Visibility = false;
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
            if (base.Control.IsRealized && isBackspace == false)
            {
                int position = args.Position;
                string new_text = args.NewText;
                if (IsMaskPassword == true)
                {
                    if(new_text.Length > 1 || base.Control.Text.Length != correctText.Length + 1)
                    {
                        base.Control.Text = correctText;
                    }
                    else if (correctText.Length > position && new_text.Length == 1)
                    {
                        if (IsNumberText(correctText.Substring(position-1, 1)) && IsNumberText(new_text))
                        {
                            //正常
                            base.Control.DeleteText(position, position+1);
                        }
                        else
                        {
                            base.Control.Text = correctText;
                        }
                    }
                    else
                    {
                        base.Control.DeleteText(position - 1, position);
                    }
                }
            }
            isBackspace = false;
            correctText = base.Control.Text;
        }
        bool isBackspace = false;
        private void Control_Backspace(object sender, EventArgs e)
        {
            if (IsMaskPassword == true)
            {
                //格式化掩码，只改数字
                int position = base.Control.CursorPosition;
                if (base.Control.Text.Length + 1 == correctText.Length) //删除一个字符
                {
                    isBackspace = true;
                    if (IsNumberChar(correctText[position]))
                    {
                        base.Control.InsertText("_", ref position);
                    }
                    else
                    {
                        base.Control.InsertText(correctText[position].ToString(), ref position);
                    }
                }
                else if (base.Control.Text.Length + 1 < correctText.Length) //选择多字符删除
                {
                    base.Control.Text = correctText;
                }
            }
        }

        private bool IsNumberText(string text)
        {
            foreach(char w in text)
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
        public override char PasswordChar { get => _PasswordChar; set { _PasswordChar = value; base.Control.InvisibleChar = value; } }
        public Type ValidatingType { get; set; }
        public MaskFormat TextMaskFormat { get; set; }

        private bool IsMaskPassword
        {
            get { return !string.IsNullOrWhiteSpace(Mask); }
        }
    }
}
