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
public class RichTextBox : ScrollableControl
{
    public readonly RichTextBoxBase self = new();
    public override object GtkControl => self;
    protected override void SetStyle(Widget widget)
    {
        base.SetStyle(self.TextView);
    }
    public RichTextBox()
    {
        self.TextView.Buffer.Changed += Buffer_Changed;
        BorderStyle = BorderStyle.Fixed3D;
    }

    private void Buffer_Changed(object? sender, EventArgs e)
    {
        if (TextChanged != null && self.IsVisible)
        {
            TextChanged?.Invoke(this, e);
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
        get { self.TextView.Buffer.GetSelectionBounds(out var start, out var end); return end.Offset - start.Offset; }
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

    public override string? Text { get => self.TextView.Buffer.Text; set => self.TextView.Buffer.Text = value; }
    public virtual bool ReadOnly
    {
        get => self.TextView.CanFocus;
        set => self.TextView.CanFocus = value;
    }

    public override event EventHandler? TextChanged;
    public void AppendText(string text)
    {
        var enditer = self.TextView.Buffer.EndIter;
        self.TextView.Buffer.Insert(ref enditer, text);
    }

    public string[] Lines => self.TextView.Buffer.Text.Split(["\r\n", "\n"], StringSplitOptions.None);
}