/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;

namespace System.Windows.Forms;

internal class NonStaticMessageBox : IMessageBox
{
    public event EventHandler<DialogEventArgs>? DialogAvailable;

    /// <summary>
    ///  Displays a message box with specified text, caption, and style with Help Button.
    /// </summary>
    public DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style and Help file Path .
    /// </summary>
    public DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style and Help file Path for a IWin32Window.
    /// </summary>
    public DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
    {
        return ShowCore(owner, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path and keyword.
    /// </summary>
    public DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path and keyword for a IWin32Window.
    /// </summary>
    public DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
    {
        return ShowCore(owner, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path and HelpNavigator.
    /// </summary>
    public DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path and HelpNavigator for IWin32Window.
    /// </summary>
    public DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
    {
        return ShowCore(owner, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path ,HelpNavigator and object.
    /// </summary>
    public DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path ,HelpNavigator and object for a IWin32Window.
    /// </summary>
    public DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
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
    public DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    public DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    public DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        return ShowCore(null, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    public DialogResult Show(string? text, string caption, MessageBoxButtons buttons)
    {
        return ShowCore(null, text, caption, buttons, MessageBoxIcon.None);
    }

    /// <summary>
    ///  Displays a message box with specified text and caption.
    /// </summary>
    public DialogResult Show(string? text, string caption)
    {
        return ShowCore(null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None);
    }

    /// <summary>
    ///  Displays a message box with specified text.
    /// </summary>
    public DialogResult Show(string? text)
    {
        return ShowCore(null, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    public DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
    {
        return ShowCore(owner, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    public DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton)
    {
        return ShowCore(owner, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    public DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        return ShowCore(owner, text, caption, buttons, icon);
    }

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    public DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons)
    {
        return ShowCore(owner, text, caption, buttons, MessageBoxIcon.None);
    }

    /// <summary>
    ///  Displays a message box with specified text and caption.
    /// </summary>
    public DialogResult Show(IWin32Window? owner, string? text, string caption)
    {
        return ShowCore(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None);
    }

    /// <summary>
    ///  Displays a message box with specified text.
    /// </summary>
    public DialogResult Show(IWin32Window? owner, string? text)
    {
        return ShowCore(owner, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None);
    }

    private Window? activeWindow; // Caching significance
    internal DialogResult ShowCore(IWin32Window? owner, string? text, string caption,
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
            if (window != null)
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
            return DialogResult.OK;
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

    private int ShowMessageDialogCore(Window owner, WindowPosition position, string text, string caption, MessageBoxButtons buttons, params object[] icon)
    {
        var buttonsType = ButtonsType.Close;
        if (buttons == MessageBoxButtons.OK)
            buttonsType = ButtonsType.Ok;
        else if (buttons == MessageBoxButtons.OKCancel)
            buttonsType = ButtonsType.OkCancel;
        else if (buttons == MessageBoxButtons.YesNo)
            buttonsType = ButtonsType.YesNo;
        else if (buttons == MessageBoxButtons.YesNoCancel)
            buttonsType = ButtonsType.YesNo;
        else if (buttons == MessageBoxButtons.AbortRetryIgnore)
            buttonsType = ButtonsType.OkCancel;
        else if (buttons == MessageBoxButtons.RetryCancel)
            buttonsType = ButtonsType.OkCancel;


        var dia = new MessageDialog(owner, DialogFlags.DestroyWithParent, MessageType.Info, buttonsType, text);
        dia.SetPosition(position);
        dia.StyleContext.AddClass("DefaultThemeStyle");
        dia.StyleContext.AddClass("MessageBox");
        dia.BorderWidth = 10;
        dia.KeepAbove = true;
        dia.KeepBelow = false;
        dia.Title = caption;
        dia.Response += Dia_Response;
        return dia.Run();
    }

    private int ShowCore(Window? owner, WindowPosition position, string text, string caption,
        MessageBoxButtons buttons, MessageBoxIcon icon, params object[] args)
    {
        var dia = new Dialog(caption, owner, DialogFlags.DestroyWithParent);
        OnDialogAvailable(new DialogEventArgs(dia));
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
            msgbox.PackStart(Image.LoadFromResource("System.Windows.Forms.Resources.System.dialog-question.png"), false, false, 5);
        else if (icon == MessageBoxIcon.Warning || icon == MessageBoxIcon.Exclamation)
            msgbox.PackStart(Image.LoadFromResource("System.Windows.Forms.Resources.System.dialog-warning.png"), false, false, 5);
        else if (icon == MessageBoxIcon.Information || icon == MessageBoxIcon.Asterisk)
            msgbox.PackStart(Image.LoadFromResource("System.Windows.Forms.Resources.System.dialog-information.png"), false, false, 5);
        else if (icon == MessageBoxIcon.Error || icon == MessageBoxIcon.Stop || icon == MessageBoxIcon.Hand)
            msgbox.PackStart(Image.LoadFromResource("System.Windows.Forms.Resources.System.dialog-error.png"), false, false, 5);
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
        pag.GetPixelSize(out var width, out var height);
        if (width > maxwidth)
        {
            dia.SetSizeRequest(maxwidth, 100);
            dia.SetDefaultSize(maxwidth, 100);
            content.Wrap = true;
            content.LineWrap = true;
            content.LineWrapMode = Pango.WrapMode.Word;
        }
        msgbox.PackStart(content, false, true, 5);
        dia.ContentArea?.PackStart(msgbox, false, true, 0);

        dia.Icon = new Gdk.Pixbuf(typeof(MessageBox).Assembly, "System.Windows.Forms.Resources.System.help-faq.png");
        if (buttons == MessageBoxButtons.OK)
        {
            dia.AddButton(Properties.Resources.MessageBox_ShowCore_OK, ResponseType.Ok);
        }
        else if (buttons == MessageBoxButtons.OKCancel)
        {
            dia.AddButton(Properties.Resources.MessageBox_ShowCore_OK, ResponseType.Ok);
            dia.AddButton(Properties.Resources.MessageBox_ShowCore_Cancel,
                ResponseType.Cancel);
        }
        else if (buttons == MessageBoxButtons.YesNo)
        {
            dia.AddButton(Properties.Resources.MessageBox_ShowCore_Yes, ResponseType.Yes);
            dia.AddButton(Properties.Resources.MessageBox_ShowCore_No, ResponseType.No);
        }
        else if (buttons == MessageBoxButtons.YesNoCancel)
        {
            dia.AddButton(Properties.Resources.MessageBox_ShowCore_Yes, ResponseType.Yes);
            dia.AddButton(Properties.Resources.MessageBox_ShowCore_No, ResponseType.No);
            dia.AddButton(Properties.Resources.MessageBox_ShowCore_Cancel,
                ResponseType.Cancel);
        }
        else if (buttons == MessageBoxButtons.AbortRetryIgnore)
        {
            dia.AddButton(Properties.Resources.MessageBox_ShowCore_Reject,
                ResponseType.Reject);
            dia.AddButton(Properties.Resources.MessageBox_ShowCore_Help, ResponseType.Help);
            dia.AddButton(Properties.Resources.MessageBox_ShowCore_Close, ResponseType.Close);
        }
        else if (buttons == MessageBoxButtons.RetryCancel)
        {
            dia.AddButton(Properties.Resources.MessageBox_ShowCore_Help, ResponseType.Help);
            dia.AddButton(Properties.Resources.MessageBox_ShowCore_Cancel,
                ResponseType.Cancel);
        }
        dia.ShowAll();
        return dia.Run();
    }

    protected virtual void OnDialogAvailable(DialogEventArgs e)
    {
        DialogAvailable?.Invoke(this, e);
    }

    private void Dia_Response(object o, ResponseArgs args)
    {
        var dia = (Dialog?)o;
        if (dia != null)
        {
            dia.PangoContext.Dispose();
            dia.Dispose();
        }
    }
}