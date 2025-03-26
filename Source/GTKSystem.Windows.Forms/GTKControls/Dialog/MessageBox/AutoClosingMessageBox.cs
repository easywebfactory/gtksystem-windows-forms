using Gtk;
using System.Security.Cryptography;

namespace System.Windows.Forms;

using FormsMessageBoxButtons = MessageBoxButtons;
using FormsMessageBox = MessageBox;

public class AutoClosingMessageBox
{
    internal static IMessageBox? MessageBox;

    public static void Init(int interval = 5000)
    {
        Instance.Initialize(interval);
    }

    [ThreadStatic]
    private static AutoClosingMessageBox? instance;

    static AutoClosingMessageBox()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
    }

    private AutoClosingMessageBox()
    {
        instance = this;
        MessageBox = FormsMessageBox.ReplaceInstance();
        Instance.Initialize();
        MessageBox.DialogAvailable += FormsMessageBox_DialogAvailable;
    }

    private void Initialize(int interval = 5000)
    {
        MessageBoxTimeout = interval;
    }

    private Dialog? _dialog;

    private void AutoDisposeAfterTimeout()
    {
        _ = Task.Run(async () =>
        {
            await Task.Delay(System.TimeSpan.FromMilliseconds(MessageBoxTimeout));
            if (_dialog != null)
            {
                _dialog.PangoContext?.Dispose();
                _dialog.Dispose();
                GC.SuppressFinalize(_dialog);
                _dialog = null;
            }
        });
    }

    public int MessageBoxTimeout { get; set; }

    private void FormsMessageBox_DialogAvailable(object? sender, DialogEventArgs e)
    {
        _dialog = e.Dialog;
    }

    public static AutoClosingMessageBox Instance
    {
        get { return instance ??= new AutoClosingMessageBox(); }
    }

    public DialogResult Show(string? text, string? caption)
    {
        AutoDisposeAfterTimeout();
        return (DialogResult)(int)MessageBox!.Show(text, caption??Path.GetFileNameWithoutExtension(Application.ExecutablePath));
    }

    public DialogResult Show(string text)
    {
        AutoDisposeAfterTimeout();
        return (DialogResult)(int)MessageBox!.Show(text);
    }

    public DialogResult Show(string text, string caption, MessageBoxButtons buttons)
    {
        AutoDisposeAfterTimeout();
        return (DialogResult)(int)MessageBox!.Show(text, caption, (FormsMessageBoxButtons)(int)buttons);
    }

    public DialogResult Show(string? text, string caption, MessageBoxButtons buttons, MessageBoxIcon messageBoxIcon)
    {
        AutoDisposeAfterTimeout();
        return (DialogResult)(int)MessageBox!.Show(text, caption, (FormsMessageBoxButtons)(int)buttons);
    }
}