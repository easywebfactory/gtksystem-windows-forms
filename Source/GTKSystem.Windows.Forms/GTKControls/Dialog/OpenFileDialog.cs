/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
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
