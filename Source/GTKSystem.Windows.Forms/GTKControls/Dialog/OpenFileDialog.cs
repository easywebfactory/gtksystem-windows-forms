/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System.IO;
using System.Text;

namespace System.Windows.Forms
{
    public sealed class OpenFileDialog : FileDialog
    {
        public OpenFileDialog()
        {
            base.Action = Gtk.FileChooserAction.Open;
        }

        public bool Multiselect { get { return base.SelectMultiple; } set { base.SelectMultiple = value; } }

        public bool ReadOnlyChecked { get; set; }

        public bool ShowReadOnly { get; set; }

        public string SafeFileName { get; }


        public string[] SafeFileNames { get; }

        public Stream OpenFile() {
            if (System.IO.File.Exists(base.FileName))
                return System.IO.File.OpenRead(base.FileName);
            else
                return null;

        }

        public override void Reset() { }
    }
}
