/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public class Button : Control
{
    public readonly ButtonBase self = new();
    public override object GtkControl => self;
    public Button()
    {
        self.Clicked += Self_Clicked;
    }
    private void Self_Clicked(object? sender, EventArgs e)
    {
        if(Click!= null && self.IsVisible) { Click?.Invoke(this, EventArgs.Empty); }
    }

    public override string? Text { get => ((Gtk.Label)self.Child).Text; set => ((Gtk.Label)self.Child).Text = value; }

    public override event EventHandler? Click;
    public override RightToLeft RightToLeft { get => self.Direction == Gtk.TextDirection.Rtl ? RightToLeft.Yes : RightToLeft.No;
        set => self.Direction = value == RightToLeft.Yes ? Gtk.TextDirection.Rtl : Gtk.TextDirection.Ltr;
    }
    public Drawing.ContentAlignment TextAlign
    {
        get => textAlign;
        set
        {
            textAlign = value;
            if (value == Drawing.ContentAlignment.TopLeft)
            {
                self.Xalign = 0.0f;
                self.Yalign = 0.0f;
            }
            else if (value == Drawing.ContentAlignment.TopCenter)
            {
                self.Xalign = 0.5f;
                self.Yalign = 0.0f;
            }
            else if (value == Drawing.ContentAlignment.TopRight)
            {
                self.Xalign = 1.0f;
                self.Yalign = 0.0f;
            }
            else if (value == Drawing.ContentAlignment.MiddleLeft)
            {
                self.Xalign = 0.0f;
                self.Yalign = 0.5f;
            }
            else if (value == Drawing.ContentAlignment.MiddleCenter)
            {
                self.Xalign = 0.5f;
                self.Yalign = 0.5f;
            }
            else if (value == Drawing.ContentAlignment.MiddleRight)
            {
                self.Xalign = 1.0f;
                self.Yalign = 0.5f;
            }
            else if (value == Drawing.ContentAlignment.BottomLeft)
            {
                self.Xalign = 0.0f;
                self.Yalign = 1.0f;
            }
            else if (value == Drawing.ContentAlignment.BottomCenter)
            {
                self.Xalign = 0.5f;
                self.Yalign = 1.0f;
            }
            else if (value == Drawing.ContentAlignment.BottomRight)
            {
                self.Xalign = 1.0f;
                self.Yalign = 1.0f;
            }

        }
    }
    private Drawing.ContentAlignment textAlign;
}