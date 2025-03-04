/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using System.Text.RegularExpressions;

namespace System.Windows.Forms;

public class MaskedTextBox : TextBox
{
    public MaskedTextBox()
    {
        self.AddClass("MaskedTextBox");
        self.Backspace += Control_Backspace;
        self.TextInserted += Control_TextInserted;
        self.Shown += Control_Shown;
    }
    internal MaskedTextBox(string cssClass)
    {
        self.AddClass(cssClass);
        self.Backspace += Control_Backspace;
        self.TextInserted += Control_TextInserted;
        self.Shown += Control_Shown;
    }
    public override string Text { get => self.Text;
        set { isMasking = false; self.Text = value ?? ""; isMasking = true; } }
    private void Control_Shown(object? sender, EventArgs e)
    {
        if (passwordChar != '\0')
        {
            self.Visibility = false;
        }
        else if (!string.IsNullOrWhiteSpace(Mask))
        {//按格式化赋值
            var txt = Regex.Match(Text, "\\d").Value;
            var windex = -1;
            Text = Regex.Replace(Mask, "\\d", _ =>
            {
                windex++;
                if (txt.Length > windex)
                {
                    return txt[windex].ToString();
                }

                return "_";
            });
        }
    }
    string? correctText;
    private void Control_TextInserted(object? o, Gtk.TextInsertedArgs args)
    {
        if (self.IsRealized && isBackspace == false)
        {
            var position = args.Position;
            var newText = args.NewText;
            if (IsMaskPassword)
            {
                if (newText.Length > 1 || self.Text.Length != (correctText??string.Empty).Length + 1)
                {
                    if (correctText != null)
                        self.Text = correctText;
                }
                else if ((correctText ?? string.Empty).Length > position && newText.Length == 1)
                {
                    if (IsNumberText(correctText!.Substring(position - 1, 1)) && IsNumberText(newText))
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
    bool isBackspace;
    private void Control_Backspace(object? sender, EventArgs e)
    {
        if (IsMaskPassword)
        {
            //格式化掩码，只改数字
            var position = self.CursorPosition;
            if (self.Text.Length + 1 == (correctText ?? string.Empty).Length) //删除一个字符
            {
                isBackspace = true;
                if (IsNumberChar(correctText![position]))
                {
                    self.InsertText("_", ref position);
                }
                else
                {
                    self.InsertText(correctText[position].ToString(), ref position);
                }
            }
            else if (self.Text.Length + 1 < (correctText ?? string.Empty).Length) //选择多字符删除
            {
                self.Text = correctText;
            }
        }
    }

    private bool IsNumberText(string text)
    {
        foreach (var w in text)
        {
            if (!IsNumberChar(w))
                return false;
        }
        return true;
    }
    private bool IsNumberChar(char w)
    {
        return char.IsNumber(w) || w == '_';
    }

    public string Mask { get; set; } = string.Empty;
    private char passwordChar;
    public override char PasswordChar { get => passwordChar; set { passwordChar = value; self.InvisibleChar = value; } }
    public Type? ValidatingType { get; set; }
    public MaskFormat TextMaskFormat { get; set; }
    internal bool isMasking = true;
    private bool IsMaskPassword => !string.IsNullOrWhiteSpace(Mask) && isMasking;

    public new string[] Lines => string.IsNullOrEmpty(Text) ? [] : Text.Replace("\r\n", "\n").Split('\n');
}