/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

namespace System.Windows.Forms
{
    public sealed class FolderBrowserDialog : FileDialog
    {
        public FolderBrowserDialog()
        {
            
        }
        public override void Reset()
        {
            RootFolder = Environment.SpecialFolder.Desktop;
            Description = string.Empty;
            SelectedPath = string.Empty;
            SelectedPathNeedsCheck = false;
            ShowNewFolderButton = true;
        }
        public Environment.SpecialFolder RootFolder { get; set; } = Environment.SpecialFolder.Desktop;
        public new string SelectedPath
        {
            get => base.SelectedPath;
            set => base.SelectedPath = value;
        }
        private new string[] SelectedPaths => base.SelectedPaths;
        private new bool Multiselect => base.Multiselect;
        private new string Title => base.Title;
        public bool ShowNewFolderButton { get; set; }
        public bool SelectedPathNeedsCheck { get; set; }
        public override DialogResult ShowDialog(IWin32Window owner)
        {
            ActionType = Gtk.FileChooserAction.SelectFolder;
            return base.ShowDialog(owner);
        }
    }
}
