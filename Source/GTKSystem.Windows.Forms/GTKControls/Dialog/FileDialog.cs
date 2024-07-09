/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;
using System.ComponentModel;
using System.Text;

namespace System.Windows.Forms
{
    public abstract class FileDialog : CommonDialog
    {
        public Gtk.FileChooserDialog fileDialog;
        public FileDialog()
        {

        }

        public bool ValidateNames { get; set; }

        //public string Title { get; set; }

        public bool SupportMultiDottedExtensions { get; set; }

        public bool ShowHelp { get; set; }

        public bool RestoreDirectory { get; set; }

        public string InitialDirectory { get; set; }
        public string Description { get; set; }

        public string SelectedPath { get; set; }

        public int FilterIndex { get; set; }

        private string filter;
        public string Filter
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
                //base.Filter = new Gtk.FileFilter();
                //for (int i = 0; i < array.Length; i += 2)
                //{
                //    base.Filter.AddMimeType(array[i]);
                //    base.Filter.AddPattern(array[i + 1]);
                //}
            }
        }

        public bool AutoUpgradeEnabled { get; set; }

        public string FileName { get; set; }
        public string[] FileNames { get { return fileDialog?.Filenames; } }
        public bool Multiselect { get; set; }
        public bool DereferenceLinks { get; set; }

        public string DefaultExt { get; set; }

        public bool CheckPathExists { get; set; }

        public virtual bool CheckFileExists { get; set; }

        public bool AddExtension { get; set; }

        public event CancelEventHandler FileOk;
        public override void Reset() { }
        protected override bool RunDialog(IWin32Window owner)
        {
            if (owner != null && owner is Form ownerform)
            {
                fileDialog = new Gtk.FileChooserDialog("", ownerform.self, Gtk.FileChooserAction.SelectFolder);
                fileDialog.WindowPosition = Gtk.WindowPosition.CenterOnParent;
            }
            else
            {
                fileDialog = new Gtk.FileChooserDialog("", null, Gtk.FileChooserAction.SelectFolder);
                fileDialog.WindowPosition = Gtk.WindowPosition.Center;
            }

            //Dialog.AddButton("确定", Gtk.ResponseType.Ok);
            //Dialog.AddButton("取消", Gtk.ResponseType.Cancel);
            fileDialog.SelectMultiple = this.Multiselect;
            fileDialog.Title = this.Description;
            if(!string.IsNullOrWhiteSpace(this.SelectedPath))
                fileDialog.SetCurrentFolder(this.SelectedPath);
            fileDialog.Filter = new Gtk.FileFilter();
            string[] array = this.Filter.Split('|');

            if (array == null || array.Length % 2 != 0)
            {
                throw new ArgumentException("FileDialog Invalid Filter");
            }
            for (int i = 0; i < array.Length; i += 2)
            {
                fileDialog.Filter.AddMimeType(array[i]);
                fileDialog.Filter.AddPattern(array[i + 1]);
            }
            
            int response = fileDialog.Run();
            this.FileName = fileDialog.Filename;
            fileDialog.HideOnDelete();
            return response == -5;
        }

        protected override void Dispose(bool disposing)
        {
            if (fileDialog != null)
            {
                fileDialog.Dispose();
                fileDialog = null;
            }
            base.Dispose(disposing);
        }
    }
}
