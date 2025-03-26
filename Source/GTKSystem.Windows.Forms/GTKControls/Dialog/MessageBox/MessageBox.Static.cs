namespace System.Windows.Forms;

public class MessageBox
{
    internal static NonStaticMessageBox Instance { get; set; } = new();

    public static IMessageBox ReplaceInstance()
    {
        Instance = new NonStaticMessageBox();
        return Instance;
    }

    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton)
    {
        return Instance.ShowCore(null, text, caption, buttons, icon);
    }

    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
    {
        return Instance.ShowCore(null, text, caption, buttons, icon);
    }

    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
    {
        return Instance.ShowCore(owner, text, caption, buttons, icon);
    }

    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
    {
        return Instance.ShowCore(null, text, caption, buttons, icon);
    }

    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
    {
        return Instance.ShowCore(owner, text, caption, buttons, icon);
    }

    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
    {
        return Instance.ShowCore(null, text, caption, buttons, icon);
    }

    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
    {
        return Instance.ShowCore(owner, text, caption, buttons, icon);
    }

    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
    {
        return Instance.ShowCore(null, text, caption, buttons, icon);
    }

    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
    {
        return Instance.ShowCore(owner, text, caption, buttons, icon);
    }

    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
    {
        return Instance.ShowCore(null, text, caption, buttons, icon);
    }

    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton)
    {
        return Instance.ShowCore(null, text, caption, buttons, icon);
    }

    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        return Instance.ShowCore(null, text, caption, buttons, icon);
    }

    public static DialogResult Show(string? text, string caption, MessageBoxButtons buttons)
    {
        return Instance.ShowCore(null, text, caption, buttons, MessageBoxIcon.None);
    }

    public static DialogResult Show(string? text, string caption)
    {
        return Instance.ShowCore(null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None);
    }

    public static DialogResult Show(string? text)
    {
        return Instance.ShowCore(null, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None);
    }

    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
    {
        return Instance.ShowCore(owner, text, caption, buttons, icon);
    }

    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton)
    {
        return Instance.ShowCore(owner, text, caption, buttons, icon);
    }

    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        return Instance.ShowCore(owner, text, caption, buttons, icon);
    }

    public static DialogResult Show(IWin32Window? owner, string? text, string caption, MessageBoxButtons buttons)
    {
        return Instance.ShowCore(owner, text, caption, buttons, MessageBoxIcon.None);
    }

    public static DialogResult Show(IWin32Window? owner, string? text, string caption)
    {
        return Instance.ShowCore(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None);
    }

    public static DialogResult Show(IWin32Window? owner, string? text)
    {
        return Instance.ShowCore(owner, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None);
    }


}