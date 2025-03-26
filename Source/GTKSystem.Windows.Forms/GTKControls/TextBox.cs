/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using System.ComponentModel;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public class TextBox : Control
{
    public readonly TextBoxBase self = new();
    private bool _shortcutsEnabled;
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
        self.ClipboardPasted += (o, args) =>
        {
            OnPaste(args);
        };
    }

    public bool ShortcutsEnabled
    {
        get
        {
            return _shortcutsEnabled;
        }
        set
        {
            _shortcutsEnabled = value;
            if (!_shortcutsEnabled)
            {
                self.KeyPressEvent += OnSelfOnKeyPressEvent;
            }
            else
            {
                self.KeyPressEvent -= OnSelfOnKeyPressEvent;
            }
        }
    }

    protected virtual void OnSelfOnKeyPressEvent(object s, Gtk.KeyPressEventArgs args)
    {
        // Detect Ctrl + C, Ctrl + V, Ctrl + X, Ctrl + Ins, Shift + Ins, Shift + Delete
        var isCtrl = (args.Event.State & Gdk.ModifierType.ControlMask) != 0;
        var isShift = (args.Event.State & Gdk.ModifierType.ShiftMask) != 0;
        if ((isCtrl && (args.Event.Key == Gdk.Key.c || args.Event.Key == Gdk.Key.C /* Copy */ ||
                        args.Event.Key == Gdk.Key.v || args.Event.Key == Gdk.Key.V /* Paste */ ||
                        args.Event.Key == Gdk.Key.x || args.Event.Key == Gdk.Key.X /* Cut */)) ||
            (isCtrl && args.Event.Key == Gdk.Key.Insert) || // Ctrl + Ins (Copy)
            (isShift && args.Event.Key == Gdk.Key.Delete) || // Shift + Del (Cut)
            (isShift && args.Event.Key == Gdk.Key.Insert)) // Shift + Ins (Paste)
        {
            args.RetVal = true; // Block the event
        }
    }

    protected virtual void OnPaste(EventArgs e)
    {

    }

    public void AppendText(string? text)
    {
        Text += text ?? string.Empty;
    }

    private void Self_KeyPressEvent(object? o, Gtk.KeyPressEventArgs args)
    {
        if (args.Event is { } eventkey)
        {
            var keys = (Keys)eventkey.HardwareKeycode;
            OnKeyDown(new KeyEventArgs(keys));
        }
    }

    private void Self_TextInserted(object? o, TextInsertedArgs args)
    {
        if (GetType().Name == "TextBox")
        {
            var keytext = args.NewText.ToUpper();
            if (char.IsNumber(args.NewText[0]))
                keytext = "D" + keytext;
            var keyv = Enum.GetValues(typeof(Keys)).Cast<Keys>().Where(k =>
            {
                return Enum.GetName(typeof(Keys), k) == keytext;
            });
            foreach (var key in keyv)
                OnKeyDown(new KeyEventArgs(key));
        }
    }

    private void Self_Changed(object? sender, EventArgs e)
    {
        if (self.IsVisible)
        {
            OnTextChanged(EventArgs.Empty);
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
            self.GetSelectionBounds(out var start, out _);
            return start;
        }
        set
        {
            self.GetSelectionBounds(out var startPos, out _);
            self.SelectRegion(startPos, startPos + value);
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

    public void Clear()
    {
        Text = string.Empty;
    }
}