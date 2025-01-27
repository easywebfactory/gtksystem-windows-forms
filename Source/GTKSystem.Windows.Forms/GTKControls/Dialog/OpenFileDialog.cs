/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using System.IO;

namespace System.Windows.Forms
{
    public sealed class OpenFileDialog : FileDialog
    {
        public OpenFileDialog()
        {
            
        }
        private new string Description => base.Description;
        public bool ReadOnlyChecked { get; set; }
        public bool ShowReadOnly { get; set; }
        public new bool Multiselect { get => base.Multiselect; set => base.Multiselect = value; }
        public string SafeFileName => Path.GetFileName(FileName) ?? string.Empty;

        public string[] SafeFileNames
        {
            get
            {
                string[] fullPaths = FileNames;
                if (fullPaths is null || fullPaths.Length == 0)
                {
                    return Array.Empty<string>();
                }

                string[] safePaths = new string[fullPaths.Length];
                for (int i = 0; i < safePaths.Length; ++i)
                {
                    safePaths[i] = Path.GetFileName(fullPaths[i]);
                }

                return safePaths;
            }
        }

        public Stream OpenFile() {
            if (System.IO.File.Exists(base.FileName))
                return System.IO.File.OpenRead(base.FileName);
            else
                return null;

        }

        public override DialogResult ShowDialog(IWin32Window owner)
        {
            ActionType = Gtk.FileChooserAction.Open;
            return base.ShowDialog(owner);
        }
    }
}
