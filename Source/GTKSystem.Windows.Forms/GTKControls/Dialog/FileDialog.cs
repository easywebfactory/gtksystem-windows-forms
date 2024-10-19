/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Linq;
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

        public string Title { get; set; }

        public bool SupportMultiDottedExtensions { get; set; }

        public bool ShowHelp { get; set; }

        public bool RestoreDirectory { get; set; }

        public string InitialDirectory { get; set; }
        public string Description { get; set; }
        internal bool Multiselect { get; set; }
        internal string SelectedPath
        {
            get => SelectedPaths.Length > 0 ? SelectedPaths[0] : string.Empty;
            set => FileName = value;
        }
        internal string[] SelectedPaths => FileNames != null && FileNames.Length > 0 ? (string[])FileNames.Clone() : Array.Empty<string>();

        public int FilterIndex { get; set; }

        private string _filter;
        public string Filter
        {
            get
            {
                return _filter ?? string.Empty;
            }
            set
            {
                if (value == _filter)
                {
                    return;
                }
                string[] filters = value?.Split(';');
                foreach (string filter in filters)
                {
                    string[] pattern = filter.Split('|');
                    if (pattern == null || pattern.Length % 2 != 0 || pattern[1].Split('.').Length == 0)
                    {
                        throw new ArgumentException("FileDialog Invalid Filter");
                    }
                    //Gtk.FileFilter ffilter = new Gtk.FileFilter();
                    //ffilter.AddMimeType(pattern[0]);
                    //ffilter.AddPattern(pattern[1]);
                    //fileDialog.AddFilter(ffilter);
                }
                string[] array = value?.Split('|');
                if (array == null || array[1].Split('.').Length == 0)
                {
                    throw new ArgumentException("FileDialog Invalid Filter");
                }
                _filter = value;
            }
        }

        public bool AutoUpgradeEnabled { get; set; }

        public string FileName { get; set; }
        public string[] FileNames { get; internal set; }
        public bool DereferenceLinks { get; set; }

        public string DefaultExt { get; set; }

        public bool CheckPathExists { get; set; }

        public virtual bool CheckFileExists { get; set; }

        public bool AddExtension { get; set; }

        public event CancelEventHandler FileOk;
        internal Gtk.FileChooserAction ActionType { get; set; }
        public override void Reset() {
            AddExtension = true;
            Title = null;
            InitialDirectory = null;
            FileName = null;
            _filter = null;
            FilterIndex = 1;
            SupportMultiDottedExtensions = false;
        }
        protected override bool RunDialog(IWin32Window owner)
        {
            if (owner != null && owner is Form ownerform)
            {
                fileDialog = new Gtk.FileChooserDialog("选择文件", ownerform.self, ActionType);
                fileDialog.WindowPosition = Gtk.WindowPosition.CenterOnParent;
            }
            else
            {
                fileDialog = new Gtk.FileChooserDialog("选择文件", null, ActionType);
                fileDialog.WindowPosition = Gtk.WindowPosition.Center;
            }
            fileDialog.KeepAbove = true;
            fileDialog.AddButton("确定", Gtk.ResponseType.Ok);
            fileDialog.AddButton("取消", Gtk.ResponseType.Cancel);
            fileDialog.SelectMultiple = this.Multiselect;
            fileDialog.Title = this.Title ?? string.Empty;
            fileDialog.TooltipText = this.Description ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(this.SelectedPath))
                fileDialog.SetCurrentFolder(this.SelectedPath);
            else if (!string.IsNullOrWhiteSpace(this.InitialDirectory))
                fileDialog.SetCurrentFolder(this.InitialDirectory);

            
            if (!string.IsNullOrWhiteSpace(DefaultExt))
            {
                DefaultExt = DefaultExt.Trim('.');
                Gtk.FileFilter filter = new Gtk.FileFilter();
                filter.AddMimeType(DefaultExt);
                filter.AddPattern($"*.{DefaultExt}");
                fileDialog.Filter = filter;
            }
            if (_filter != null)
            {
                string[] filters = _filter.Split(';');
                foreach(string filter in filters)
                {
                    string[] pattern = filter.Split('|');
                    Gtk.FileFilter ffilter = new Gtk.FileFilter();
                    ffilter.AddMimeType(pattern[0]);
                    ffilter.AddPattern(pattern[1]);
                    fileDialog.AddFilter(ffilter);
                }
                //for (int i = 0; i < array.Length; i += 2)
                //{
                //    fileDialog.Filter.AddMimeType(array[i]);
                //    fileDialog.Filter.AddPattern(array[i + 1]);

                //}
            }
            

            int response = fileDialog.Run();
            this.FileName = fileDialog.Filename;
            this.FileNames = fileDialog.Filenames;
            this.SelectedPath = fileDialog.CurrentFolder;
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
