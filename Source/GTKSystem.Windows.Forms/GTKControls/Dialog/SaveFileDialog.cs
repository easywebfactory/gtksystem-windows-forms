/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using System.IO;

namespace System.Windows.Forms
{
    public sealed class SaveFileDialog : FileDialog
    {
        public SaveFileDialog()
        {
        }
        private new string Description => base.Description;
        public bool CheckWriteAccess
        {
            get => true;
            set { }
        }
        public Stream OpenFile()
        {
            string filename = FileName;
            if(string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename");
            }
            return new FileStream(filename, FileMode.Create, FileAccess.ReadWrite);
        }
        public override DialogResult ShowDialog(IWin32Window owner)
        {
            ActionType = Gtk.FileChooserAction.Save;
            return base.ShowDialog(owner);
        }
    }
}
