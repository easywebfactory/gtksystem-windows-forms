/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using Gdk;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public class PictureBox : Control
{
    private readonly PictureBoxBase self = new();
    public override object GtkControl => self;
    public PictureBox()
    {
        self.Shown += Self_Shown;
    }

    private void Self_Shown(object? sender, EventArgs? e)
    {
        var width = Width;
        var height = Height;
        if (MaximumSize.Width > 0)
        {
            width = Math.Min(MaximumSize.Width, Width);
        }
        if (MaximumSize.Height > 0)
        {
            height = Math.Min(MaximumSize.Height, Height);
        }
        if (MinimumSize.Width > 0)
        {
            width = Math.Min(MinimumSize.Width, width);
        }
        if (MinimumSize.Height > 0)
        {
            height = Math.Min(MinimumSize.Height, height);
        }
        if (_image is { Pixbuf: not null })
        {
            ImageUtility.ScaleImageByPictureBoxSizeMode(_image.Pixbuf, width, height, out var newImagePixbuf, SizeMode);
            self.Pixbuf = newImagePixbuf;
        }
        else if (InitialImage is { Pixbuf: not null })
        {
            ImageUtility.ScaleImageByPictureBoxSizeMode(InitialImage.Pixbuf, width, height, out var newImagePixbuf, SizeMode);
            self.Pixbuf = newImagePixbuf;
        }
    }


    public PictureBoxSizeMode SizeMode { get; set; }

    public Image? InitialImage { get; set; }
    private string? imageLocation;
    public string? ImageLocation { get => imageLocation;
        set { imageLocation = value; Load(value); } }

    private Image? _image;
    public override Image? Image { 
        get => _image;
        set {
            _image = value;
            if (self.IsRealized && _image is { PixbufData: not null })
            {
                Self_Shown(null, null);
            }
        }
    }

    public Image? ErrorImage { get; set; }

    [DefaultValue(BorderStyle.None)]
    public override BorderStyle BorderStyle { get; set; }

    public void CancelAsync() { }
    public new void Load(string? url) {
        if(string.IsNullOrWhiteSpace(url))
        { return; }

        if ((url??string.Empty).Contains("://") && Uri.TryCreate(url, UriKind.Absolute, out var result)){
            var file = GLib.FileFactory.NewForUri(result);
            var stream = file.Read(new GLib.Cancellable());
            var pixbuf = new Pixbuf(stream, new GLib.Cancellable());
            _image = new Bitmap(0, 0);
            _image.Pixbuf = pixbuf;
        }
        else
        {
            var pixbuf = new Pixbuf((url ?? string.Empty).Replace("\\\\", "/").Replace("\\", "/"));
            _image = new Bitmap(0, 0);
            _image.Pixbuf = pixbuf;
        }
        if(self.IsMapped && self.IsVisible)
        {
            Self_Shown(null, null);
        }
    }
    public new void Load()
    {
        try
        {
            if (IO.File.Exists(ImageLocation))
            {
                Load(ImageLocation);
            }
        }
        catch (Exception e)
        {
            Trace.Write(e);
        }
    }
    public void LoadAsync() { 
        if (IO.File.Exists(ImageLocation)) { 
            LoadAsync(ImageLocation);
        } 
    }
    public void LoadAsync(string? url) {
        Threading.Tasks.Task.Run(() => Gtk.Application.Invoke((_, _) => { 
            Load(url);
        }));
    }
  
    public override void EndInit()
    {

    }
}