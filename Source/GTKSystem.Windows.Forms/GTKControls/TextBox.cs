/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;
using System.ComponentModel;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public class TextBox : Control
{
    public readonly TextBoxBase self = new TextBoxBase();
    public override object GtkControl => self;

    public TextBox()
    {
        self.MaxWidthChars = 1;
        self.WidthChars = 0;
        self.Valign = Align.Start;
        self.Halign = Align.Start;
        self.Changed += Self_Changed;
        self.TextInserted += Self_TextInserted;
        self.KeyPressEvent += Self_KeyPressEvent;
    }

    public void AppendText(string? text)
    {
        Text += text ?? string.Empty;
    }

    private void Self_KeyPressEvent(object? o, Gtk.KeyPressEventArgs args)
    {
        if (KeyDown != null)
        {
            if (args.Event is { } eventkey)
            {
                var keys = (Keys)eventkey.HardwareKeycode;
                KeyDown?.Invoke(this, new KeyEventArgs(keys));
            }
        }
    }

    public override event KeyEventHandler? KeyDown;

    private void Self_TextInserted(object? o, TextInsertedArgs args)
    {
        if (KeyDown != null && GetType().Name == "TextBox")
        {
            var keytext = args.NewText.ToUpper();
            if (char.IsNumber(args.NewText[0]))
                keytext = "D" + keytext;
            var keyv = Enum.GetValues(typeof(Keys)).Cast<Keys>().Where(k =>
            {
                return Enum.GetName(typeof(Keys), k) == keytext;
            });
            foreach (var key in keyv)
                KeyDown?.Invoke(this, new KeyEventArgs(key));
        }
    }

    private void Self_Changed(object sender, EventArgs e)
    {
        if (TextChanged != null && self.IsVisible)
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public string[] Lines => string.IsNullOrEmpty(Text) ? [] : Text.Replace("\r\n", "\n").Split('\n');

    public string PlaceholderText
    {
        get => self.PlaceholderText;
        set => self.PlaceholderText = value ?? "";
    }

    public override string Text
    {
        get => self.Text;
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        set
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        {
            var selfText = value ?? "";
            self.Text = selfText;
        }
    }

    public virtual char PasswordChar
    {
        get => self.InvisibleChar;
        set
        {
            self.InvisibleChar = value;
            self.Visibility = false;
        }
    }

    public virtual bool ReadOnly
    {
        get => self.IsEditable == false;
        set => self.IsEditable = value == false;
    }

    public override event EventHandler? TextChanged;
    public bool Multiline { get; set; }

    public int MaxLength
    {
        get => self.MaxLength;
        set => self.MaxLength = value;
    }

    public int SelectionStart
    {
        get
        {
            self.GetSelectionBounds(out int start, out _);
            return start;
        }
    }

    [Browsable(false)]
    public virtual int SelectionLength
    {
        get
        {
            self.GetSelectionBounds(out var start, out var end);
            return end - start;
        }
        set => self.SelectRegion(self.CursorPosition, self.CursorPosition + value);
    }

    public void InsertTextAtCursor(string text)
    {
        if (text == null) return;
        var posi = self.CursorPosition;
        self.InsertText(text, ref posi);
    }
}