/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class PictureBox : Control
    {
        public readonly PictureBoxBase self = new PictureBoxBase();
        public override object GtkControl => self;
        public PictureBox()
        {
            self.Override.sender = this;
        }
        public override Drawing.Size MaximumSize { get => self.MaximumSize; set => self.MaximumSize = value; }
        public override Drawing.Size MinimumSize { get => self.MinimumSize; set => self.MinimumSize = value; }
        public PictureBoxSizeMode SizeMode { get => self.SizeMode; set { self.SizeMode = value; } }
        private System.Drawing.Image _initialImage;
        public System.Drawing.Image InitialImage { get => _initialImage; set { _initialImage = value; if (_image == null) { this.Image = value; } } }
        private string _ImageLocation;
        public string ImageLocation { get => _ImageLocation; set { _ImageLocation = value; Load(value); } }

        private System.Drawing.Image _image;
        public override System.Drawing.Image Image
        {
            get { return _image; }
            set
            {
                _image = value;
                if (_image != null)
                    self.Image = _image.Pixbuf;
            }
        }
        public System.Drawing.Image ErrorImage { get; set; }

        public override ImageLayout BackgroundImageLayout { get => self.BackgroundImageLayout; set => self.BackgroundImageLayout = value; }
        private System.Drawing.Image _backgroundImage;
        public override Image BackgroundImage { get => _backgroundImage; set { _backgroundImage = value; self.BackgroundImage = value.Pixbuf; } }

        public void CancelAsync() { }
        public new void Load(string url)
        {
            self.Child?.Destroy();
            if (string.IsNullOrWhiteSpace(url))
            { return; }
            else if (url.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
            {
                //支持动画，动画图片不缩放和定位
                Gtk.Image image = new Gtk.Image(new Gdk.PixbufAnimation(url.Replace("\\\\", "/").Replace("\\", "/")));
                self.Child = image;
            }
            else if (url.Contains("://") && Uri.TryCreate(url, UriKind.Absolute, out Uri result))
            {
                GLib.IFile file = GLib.FileFactory.NewForUri(result);
                GLib.FileInputStream stream = file.Read(new GLib.Cancellable());
                Gdk.Pixbuf pixbuf = new Gdk.Pixbuf(stream, new GLib.Cancellable());
                this.Image = new Bitmap(0, 0) { Pixbuf = pixbuf };
            }
            else
            {
                Gdk.Pixbuf pixbuf = new Gdk.Pixbuf(url.Replace("\\\\", "/").Replace("\\", "/"));
                this.Image = new Bitmap(0, 0) { Pixbuf = pixbuf };
            }

        }
        public void LoadAsync(string url)
        {
            Threading.Tasks.Task.Run(() => Load(url));
        }

        public override void EndInit()
        {

        }
    }
}
