/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */


using GTKSystem.Resources.Extensions;
using System.IO;

namespace System.Windows.Forms
{
    public sealed class SaveFileDialog : FileDialog
    {
        public SaveFileDialog()
        {
        }
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
