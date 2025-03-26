namespace System.Windows.Forms;

public interface IMessageBox
{
    event EventHandler<DialogEventArgs>? DialogAvailable;

    /// <summary>
    ///  Displays a message box with specified text, caption, and style with Help Button.
    /// </summary>
    DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton);

    /// <summary>
    ///  Displays a message box with specified text, caption, style and Help file Path .
    /// </summary>
    DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath);

    /// <summary>
    ///  Displays a message box with specified text, caption, style and Help file Path for a IWin32Window.
    /// </summary>
    DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath);

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path and keyword.
    /// </summary>
    DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword);

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path and keyword for a IWin32Window.
    /// </summary>
    DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword);

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path and HelpNavigator.
    /// </summary>
    DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator);

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path and HelpNavigator for IWin32Window.
    /// </summary>
    DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator);

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path ,HelpNavigator and object.
    /// </summary>
    DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param);

    /// <summary>
    ///  Displays a message box with specified text, caption, style, Help file Path ,HelpNavigator and object for a IWin32Window.
    /// </summary>
    DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param);

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options);

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton);

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    DialogResult Show(string? text, string caption, MessageBoxButtons buttons);

    /// <summary>
    ///  Displays a message box with specified text and caption.
    /// </summary>
    DialogResult Show(string? text, string caption);

    /// <summary>
    ///  Displays a message box with specified text.
    /// </summary>
    DialogResult Show(string? text);

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options);

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton);

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);

    /// <summary>
    ///  Displays a message box with specified text, caption, and style.
    /// </summary>
    DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons);

    /// <summary>
    ///  Displays a message box with specified text and caption.
    /// </summary>
    DialogResult Show(IWin32Window? owner, string? text, string caption);

    /// <summary>
    ///  Displays a message box with specified text.
    /// </summary>
    DialogResult Show(IWin32Window? owner, string? text);
}