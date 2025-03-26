/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public class Button : Control
{
    public readonly ButtonBase self = new();
    public override object GtkControl => self;

    public override string Text { get => ((Gtk.Label)self.Child).Text; set => ((Gtk.Label)self.Child).Text = value; }

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