/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
namespace System.Windows.Forms
{

    public class ToolStripTextBox : ToolStripItem
    {
        public StripToolItem self = new StripToolItem();
        public override IToolMenuItem Widget { get => self; }

        internal Gtk.Entry entry = new Gtk.Entry();
        public ToolStripTextBox() : base()
        {
            entry.MaxWidthChars = 1;
            entry.WidthChars = 0;
            self.Add(entry);
        }
        public int MaxLength { get => entry.MaxLength; set => entry.MaxLength = value; }
        public override string Text { get => entry.Text; set => entry.Text = value; }
    }

}
