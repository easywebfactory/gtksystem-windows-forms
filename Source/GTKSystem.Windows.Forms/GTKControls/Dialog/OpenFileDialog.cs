﻿/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
namespace System.Windows.Forms;

public sealed class OpenFileDialog : FileDialog
{
    private new string? Description => base.Description;
    public bool ReadOnlyChecked { get; set; }
    public bool ShowReadOnly { get; set; }
    public new bool Multiselect { get => base.Multiselect; set => base.Multiselect = value; }
    public string SafeFileName => Path.GetFileName(FileName) ?? string.Empty;

    public string[] SafeFileNames
    {
        get
        {
            var fullPaths = FileNames;
            if (fullPaths is null || fullPaths.Length == 0)
            {
                return [];
            }

            var safePaths = new string[fullPaths.Length];
            for (var i = 0; i < safePaths.Length; ++i)
            {
                safePaths[i] = Path.GetFileName(fullPaths[i]);
            }

            return safePaths;
        }
    }

    public Stream? OpenFile()
    {
        if (FileName != null && File.Exists(FileName))
            return File.OpenRead(FileName);
        return null;

    }

    public override DialogResult ShowDialog(IWin32Window? owner)
    {
        ActionType = Gtk.FileChooserAction.Open;
        return base.ShowDialog(owner);
    }
}