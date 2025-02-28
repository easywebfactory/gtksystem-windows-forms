/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;

namespace System.Windows.Forms;

public class MessageBox
{
    /// <summary>
    ///  Displays a message box with specified text, caption, and style with Help Button.
    /// </summary>
    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style and Help file Path .
    /// </summary>
    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style and Help file Path for a IWin32Window.
    /// </summary>
    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
    {
        return ShowCore(owner, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path and keyword.
    /// </summary>
    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path and keyword for a IWin32Window.
    /// </summary>
    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
    {
        return ShowCore(owner, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path and HelpNavigator.
    /// </summary>
    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path and HelpNavigator for IWin32Window.
    /// </summary>
    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
    {
        return ShowCore(owner, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path ,HelpNavigator and object.
    /// </summary>
    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path ,HelpNavigator and object for a IWin32Window.
    /// </summary>
    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
    {
        return ShowCore(owner, text, caption, buttons, icon);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //END ADD                                                                                                      //
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons)
    {
        return ShowCore(null, text, caption, buttons, MessageBoxIcon.None);
    }

    /// <summary>
    ///  Displays a message box with specified text and caption.
    /// </summary>
    public static DialogResult Show(string? text, string caption)
    {
        return ShowCore(null, text, caption, MessageBoxButtons.Ok, MessageBoxIcon.None);
    }

    /// <summary>
    ///  Displays a message box with specified text.
    /// </summary>
    public static DialogResult Show(string? text)
    {
        return ShowCore(null, text, string.Empty, MessageBoxButtons.Ok, MessageBoxIcon.None);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
    {
        return ShowCore(owner, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton)
    {
        return ShowCore(owner, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        return ShowCore(owner, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons)
    {
        return ShowCore(owner, text, caption, buttons, MessageBoxIcon.None);
    }

    /// <summary>
    ///  Displays a message box with specified text and caption.
    /// </summary>
    public static DialogResult Show(IWin32Window? owner, string? text, string caption)
    {
        return ShowCore(owner, text, caption, MessageBoxButtons.Ok, MessageBoxIcon.None);
    }

    /// <summary>
    ///  Displays a message box with specified text.
    /// </summary>
    public static DialogResult Show(IWin32Window? owner, string? text)
    {
        return ShowCore(owner, text, string.Empty, MessageBoxButtons.Ok, MessageBoxIcon.None);
    }

    private static Window? activeWindow; //有缓存意义
    private static DialogResult ShowCore(IWin32Window? owner, string? text, string caption,
        MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        int irun;
        if (owner is Form control)
        {
            //irun = ShowMessageDialogCore(control.self, Gtk.WindowPosition.CenterOnParent, text, caption, buttons, icon, defaultButton, options, showHelp);
            irun = ShowCore((Window)control.Widget, WindowPosition.CenterOnParent, text, caption, buttons, icon);
        }
        else
        {
            var window = Window.ListToplevels().LastOrDefault(o => o is FormBase && o.IsActive);
            if(window != null)
            {
                activeWindow = window;
            }
            //irun = ShowMessageDialogCore(null, Gtk.WindowPosition.Center, text, caption, buttons, icon, defaultButton, options, showHelp);
            irun = ShowCore(activeWindow, WindowPosition.CenterOnParent, text, caption, buttons, icon);
        }

            var resp = (ResponseType)Enum.Parse(typeof(ResponseType), irun.ToString());
            if (resp == ResponseType.Yes)
                return DialogResult.Yes;
            if (resp == ResponseType.No)
                return DialogResult.No;
            if (resp == ResponseType.Ok)
                return DialogResult.Ok;
            if (resp == ResponseType.Cancel)
                return DialogResult.Cancel;
            if (resp == ResponseType.Reject)
                return DialogResult.Abort;
            if (resp == ResponseType.Help)
                return DialogResult.Retry;
            if (resp == ResponseType.Close)
                return DialogResult.Ignore;
            if (resp == ResponseType.None)
                return DialogResult.None;
            if (resp == ResponseType.DeleteEvent)
                return DialogResult.None;
            return DialogResult.None;
    }

    private static int ShowCore(Window? owner, WindowPosition position, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        var dia = new Dialog(caption, owner, DialogFlags.DestroyWithParent);
        dia.KeepAbove = true;
        dia.KeepBelow = false;
        dia.TypeHint = Gdk.WindowTypeHint.Dialog;
        dia.SetPosition(position);
        dia.StyleContext.AddClass("DefaultThemeStyle");
        dia.StyleContext.AddClass("MessageBox");
        dia.SetSizeRequest(300, 100);
        dia.SetDefaultSize(300, 100);
        dia.Response += Dia_Response;
        dia.ContentArea.Spacing = 10;
        dia.ContentArea.Expand = false;
        dia.ContentArea.Halign = Align.Fill;
        dia.ContentArea.Valign = Align.Start;
        dia.BorderWidth = 20;
        dia.Valign = Align.Fill;
        dia.Halign = Align.Fill;
        var msgbox = new Box(Gtk.Orientation.Horizontal, 10);
        msgbox.Valign = Align.Start;
        msgbox.Halign = Align.Fill;
        if (icon == MessageBoxIcon.Question)
            msgbox.PackStart(Image.NewFromIconName("dialog-question", IconSize.Dialog), false, false, 5);
        else if (icon == MessageBoxIcon.Warning || icon == MessageBoxIcon.Exclamation)
            msgbox.PackStart(Image.NewFromIconName("dialog-warning", IconSize.Dialog), false, false, 5);
        else if (icon == MessageBoxIcon.Information || icon == MessageBoxIcon.Asterisk)
            msgbox.PackStart(Image.NewFromIconName("dialog-information", IconSize.Dialog), false, false, 5);
        else if (icon == MessageBoxIcon.Error || icon == MessageBoxIcon.Stop || icon == MessageBoxIcon.Hand)
            msgbox.PackStart(Image.NewFromIconName("dialog-error", IconSize.Dialog), false, false, 5);
        var content = new Gtk.Label(text) { MarginEnd = 30 };
        content.Halign = Align.Fill;
        content.Valign = Align.Start;
        var maxwidth = 300;
        if (Gdk.Screen.Default.Display.NMonitors > 0)
        {
            var rectangle = Gdk.Screen.Default.Display.GetMonitor(0).Workarea;
            maxwidth = rectangle.Width / 2;
        }
        var pag = content.CreatePangoLayout(text);
        pag.GetPixelSize(out var width, out _);
        if (width > maxwidth)
        {
            dia.SetSizeRequest(maxwidth, 100);
            dia.SetDefaultSize(maxwidth, 100);
            content.Wrap = true;
            content.LineWrap = true;
            content.LineWrapMode = Pango.WrapMode.Word;
        }
        msgbox.PackStart(content, false, true, 5);
        dia.ContentArea.PackStart(msgbox, false, true, 0);
         
        var iconTheme = new IconTheme();
        var pixbuf = iconTheme.LoadIcon("dialog-information", 16, IconLookupFlags.DirLtr);
        dia.Icon = pixbuf;
        if (buttons == MessageBoxButtons.Ok)
        {
            dia.AddButton("确定", ResponseType.Ok);
        }
        else if (buttons == MessageBoxButtons.OkCancel)
        {
            dia.AddButton("确定", ResponseType.Ok);
            dia.AddButton("取消", ResponseType.Cancel);
        }
        else if (buttons == MessageBoxButtons.YesNo)
        {
            dia.AddButton("是", ResponseType.Yes);
            dia.AddButton("否", ResponseType.No);
        }
        else if (buttons == MessageBoxButtons.YesNoCancel)
        {
            dia.AddButton("是", ResponseType.Yes);
            dia.AddButton("否", ResponseType.No);
            dia.AddButton("取消", ResponseType.Cancel);
        }
        else if (buttons == MessageBoxButtons.AbortRetryIgnore)
        {
            dia.AddButton("放弃", ResponseType.Reject);
            dia.AddButton("重试", ResponseType.Help);
            dia.AddButton("忽略", ResponseType.Close);
        }
        else if (buttons == MessageBoxButtons.RetryCancel)
        {
            dia.AddButton("重试", ResponseType.Help);
            dia.AddButton("取消", ResponseType.Cancel);
        }
        dia.ShowAll();
        return dia.Run();
    }
    private static void Dia_Response(object? o, ResponseArgs args)
    {
        var dia = o as Dialog;
        if (dia != null)
        {
            dia.PangoContext.Dispose();
            dia.Dispose();
            dia.Destroy();
        }
    }
}