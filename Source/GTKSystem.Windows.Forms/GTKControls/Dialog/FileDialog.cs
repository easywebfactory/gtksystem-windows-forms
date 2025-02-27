/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;
using Gtk;

namespace System.Windows.Forms;

public abstract class FileDialog : CommonDialog
{
    public FileChooserDialog? fileDialog;

    public bool ValidateNames { get; set; } = true;

    public string Title { get; set; } = string.Empty;

    public bool SupportMultiDottedExtensions { get; set; }

    public bool ShowHelp { get; set; }

    public bool RestoreDirectory { get; set; }

    public string InitialDirectory { get; set; } = string.Empty;

    public string Description
    {
        get => description;
        set => description = value??string.Empty;
    }

    internal bool Multiselect { get; set; }
    internal string SelectedPath
    {
        get => SelectedPaths.Length > 0 ? SelectedPaths[0] : string.Empty;
        set => FileName = value;
    }
    internal string[] SelectedPaths => FileNames is { Length: > 0 } ? (string[])FileNames.Clone() : [];

    public int FilterIndex { get; set; } = 1;

    private string? _filter;
    private string defaultExt = string.Empty;
    private string[] fileNames = [];
    private string fileName = string.Empty;
    private string description = string.Empty;

    public string? Filter
    {
        get => _filter ?? string.Empty;
        set
        {
            if (value == _filter)
            {
                return;
            }

            if (value == null)
            {
                _filter = string.Empty;
                return;
            }
            var filters = value.Split(';');
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    var pattern = filter.Split('|');
                    if (pattern == null || pattern.Length % 2 != 0 || pattern[1].Split('.').Length == 0)
                    {
                        throw new ArgumentException("FileDialog Invalid Filter");
                    }
                    //Gtk.FileFilter ffilter = new Gtk.FileFilter();
                    //ffilter.AddMimeType(pattern[0]);
                    //ffilter.AddPattern(pattern[1]);
                    //fileDialog.AddFilter(ffilter);
                }
            }

            var array = value.Split('|');
            if (array == null || array[1].Split('.').Length == 0)
            {
                throw new ArgumentException("FileDialog Invalid Filter");
            }
            _filter = value;
        }
    }

    public bool AutoUpgradeEnabled { get; set; }

    public string FileName
    {
        get => fileName;
        set => fileName = value??string.Empty;
    }

    public string[] FileNames
    {
        get
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                return [FileName];
            }
            return fileNames;
        }
        internal set => fileNames = value;
    }

    public bool DereferenceLinks { get; set; } = true;

    public string DefaultExt
    {
        get => defaultExt;
        set => defaultExt = value?.TrimStart('.')??string.Empty;
    }

    public bool CheckPathExists { get; set; } = true;

    public virtual bool CheckFileExists { get; set; } = true;

    public bool AddExtension { get; set; } = true;

    public virtual event CancelEventHandler? FileOk;
    internal FileChooserAction ActionType { get; set; }
    public override void Reset() {
        AddExtension = true;
        Title = string.Empty;
        InitialDirectory = string.Empty;
        FileName = string.Empty;
        _filter = string.Empty;
        DefaultExt = string.Empty;
        FilterIndex = 1;
        SupportMultiDottedExtensions = false;
    }
    protected override bool RunDialog(IWin32Window? owner)
    {
        if (owner is Form ownerform)
        {
            fileDialog = new FileChooserDialog("选择文件", ownerform.self, ActionType);
            fileDialog.WindowPosition = WindowPosition.CenterOnParent;
        }
        else
        {
            fileDialog = new FileChooserDialog("选择文件", null, ActionType);
            fileDialog.WindowPosition = WindowPosition.Center;
        }
        fileDialog.KeepAbove = true;
        fileDialog.AddButton("确定", ResponseType.Ok);
        fileDialog.AddButton("取消", ResponseType.Cancel);
        fileDialog.SelectMultiple = Multiselect;
        fileDialog.Title = Title ?? string.Empty;
        fileDialog.TooltipText = Description ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(SelectedPath))
            fileDialog.SetCurrentFolder(SelectedPath);
        else if (!string.IsNullOrWhiteSpace(InitialDirectory))
            fileDialog.SetCurrentFolder(InitialDirectory);

            
        if (!string.IsNullOrWhiteSpace(DefaultExt))
        {
            DefaultExt = DefaultExt.Trim('.');
            var filter = new FileFilter();
            filter.AddMimeType(DefaultExt);
            filter.AddPattern($"*.{DefaultExt}");
            fileDialog.Filter = filter;
        }
        if (_filter != null)
        {
            var filters = _filter.Split(';');
            foreach(var filter in filters)
            {
                var pattern = filter.Split('|');
                var ffilter = new FileFilter();
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
            

        var response = fileDialog.Run();
        FileName = fileDialog.Filename;
        FileNames = fileDialog.Filenames;
        SelectedPath = fileDialog.CurrentFolder;
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