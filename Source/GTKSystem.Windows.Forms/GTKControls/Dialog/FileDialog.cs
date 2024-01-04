/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System.ComponentModel;
using System.Text;

namespace System.Windows.Forms
{
    public abstract class FileDialog : Gtk.FileChooserDialog
    {
       // public Gtk.FileChooserDialog Dialog;
        public FileDialog()
        {
           // Dialog = new Gtk.FileChooserDialog("",new Gtk.Window(Gtk.WindowType.Toplevel) { WindowPosition=Gtk.WindowPosition.Center },new Gtk.FileChooserAction());
            base.AddButton("确定", Gtk.ResponseType.Ok);
            base.AddButton("取消", Gtk.ResponseType.Cancel);
        }
 
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public FileDialogCustomPlacesCollection CustomPlaces { get; }


        public bool ValidateNames { get; set; }

        //public string Title { get; set; }

        public bool SupportMultiDottedExtensions { get; set; }

        public bool ShowHelp { get; set; }

        public bool RestoreDirectory { get; set; }

        public string InitialDirectory { get; set; }

        public int FilterIndex { get; set; }

        private string filter;
        public new string Filter
        {
            get
            {
                return filter;
            }
            set
            {
                filter = value;
                string[] array = value.Split('|');

                if (array == null || array.Length % 2 != 0)
                {
                    throw new ArgumentException("FileDialog Invalid Filter");
                }
                base.Filter = new Gtk.FileFilter();
                for (int i = 0; i < array.Length; i += 2)
                {
                    base.Filter.AddMimeType(array[i]);
                    base.Filter.AddPattern(array[i + 1]);
                }
            }
        }

        public bool AutoUpgradeEnabled { get; set; }

        public string FileName { get { return base.Filename; } set { } }
        public string[] FileNames { get { return base.Filenames; } }

        public bool DereferenceLinks { get; set; }

        public string DefaultExt { get; set; }

        public bool CheckPathExists { get; set; }

        public virtual bool CheckFileExists { get; set; }

        public bool AddExtension { get; set; }

        public event CancelEventHandler FileOk;
        public virtual void Reset() { }

        public DialogResult ShowDialog()
        {
            int res = base.Run();
            base.HideOnDelete();
            Gtk.ResponseType resp = Enum.Parse<Gtk.ResponseType>(res.ToString());
            if (resp == Gtk.ResponseType.Yes)
                return DialogResult.Yes;
            else if (resp == Gtk.ResponseType.No)
                return DialogResult.No;
            else if (resp == Gtk.ResponseType.Ok)
                return DialogResult.OK;
            else if (resp == Gtk.ResponseType.Cancel)
                return DialogResult.Cancel;
            else if (resp == Gtk.ResponseType.Reject)
                return DialogResult.Abort;
            else if (resp == Gtk.ResponseType.Help)
                return DialogResult.Retry;
            else if (resp == Gtk.ResponseType.Close)
                return DialogResult.Ignore;
            else if (resp == Gtk.ResponseType.None)
                return DialogResult.None;
            else if (resp == Gtk.ResponseType.DeleteEvent)
                return DialogResult.None;
            else
                return DialogResult.None;
        }

    }
}
