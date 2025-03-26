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
public class RichTextBox : ScrollableControl
{
    public readonly RichTextBoxBase self = new();
    public override object GtkControl => self;

    protected override void SetStyle(Widget widget)
    {
        self.TextView.Name = Name;
        base.SetStyle(self.TextView);
    }

    public RichTextBox()
    {
        self.TextView.Buffer.Changed += Buffer_Changed;
        BorderStyle = BorderStyle.Fixed3D;
    }

    private void Buffer_Changed(object? sender, EventArgs e)
    {
        if (self.IsVisible)
        {
            OnTextChanged(e);
        }
    }

    public int SelectionStart
    {
        get
        {
            if (self.TextView.Buffer.HasSelection)
            {
                self.TextView.Buffer.GetSelectionBounds(out var start, out _);
                return start.Offset;
            }

            return self.TextView.Buffer.CursorPosition;
        }
    }

    [Browsable(false)]
    public virtual int SelectionLength
    {
        get
        {
            self.TextView.Buffer.GetSelectionBounds(out var start, out var end);
            return end.Offset - start.Offset;
        }
        set
        {

            var start = self.TextView.Buffer.GetIterAtOffset(self.TextView.Buffer.CursorPosition);
            var end = self.TextView.Buffer.GetIterAtOffset(self.TextView.Buffer.CursorPosition + value);
            self.TextView.Buffer.SelectRange(start, end);
        }
    }

    public void InsertTextAtCursor(string text)
    {
        if (text == null) return;
        self.TextView.Buffer.InsertAtCursor(text);
    }

    public override string Text
    {
        get => self.TextView.Buffer.Text;
        set => self.TextView.Buffer.Text = value;
    }

    public virtual bool ReadOnly
    {
        get => self.TextView.CanFocus;
        set => self.TextView.CanFocus = value;
    }

    public void AppendText(string text)
    {
        var enditer = self.TextView.Buffer.EndIter;
        self.TextView.Buffer.Insert(ref enditer, text);
    }

    public string[] Lines => self.TextView.Buffer.Text.Split(["\r\n", "\n"], StringSplitOptions.None);
}