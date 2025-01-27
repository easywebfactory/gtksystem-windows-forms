/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using GTKSystem.Windows.Forms.Utility;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class PictureBox : Control
    {
        private readonly PictureBoxBase self = new PictureBoxBase();
        public override object GtkControl => self;
        public PictureBox()
        {
            self.Shown += Self_Shown;
        }

        private void Self_Shown(object sender, EventArgs e)
        {
            int width = Width;
            int height = Height;
            if (this.MaximumSize.Width > 0)
            {
                width = Math.Min(this.MaximumSize.Width, Width);
            }
            if (this.MaximumSize.Height > 0)
            {
                height = Math.Min(this.MaximumSize.Height, Height);
            }
            if (this.MinimumSize.Width > 0)
            {
                width = Math.Min(this.MinimumSize.Width, width);
            }
            if (this.MinimumSize.Height > 0)
            {
                height = Math.Min(this.MinimumSize.Height, height);
            }
            if (_image != null && _image.Pixbuf != null)
            {
                ImageUtility.ScaleImageByPictureBoxSizeMode(_image.Pixbuf, width, height, out Gdk.Pixbuf newImagePixbuf, SizeMode);
                self.Pixbuf = newImagePixbuf;
            }
            else if (InitialImage != null && InitialImage.Pixbuf != null)
            {
                ImageUtility.ScaleImageByPictureBoxSizeMode(InitialImage.Pixbuf, width, height, out Gdk.Pixbuf newImagePixbuf, SizeMode);
                self.Pixbuf = newImagePixbuf;
            }
        }


        public PictureBoxSizeMode SizeMode { get; set; }

        public System.Drawing.Image InitialImage { get; set; }
        private string _ImageLocation;
        public string ImageLocation { get { return _ImageLocation; } set { _ImageLocation = value; Load(value); } }

        private System.Drawing.Image _image;
        public override System.Drawing.Image Image { 
            get { return _image; }
            set {
                _image = value;
                if (self.IsRealized && _image != null && _image.PixbufData != null)
                {
                    Self_Shown(null, null);
                }
            }
        }

        public System.Drawing.Image ErrorImage { get; set; }

        [DefaultValue(BorderStyle.None)]
        public override BorderStyle BorderStyle { get; set; }

        public void CancelAsync() { }
        public new void Load(string url) {
            if(string.IsNullOrWhiteSpace(url))
            { return; }
            else if (url.Contains("://") && Uri.TryCreate(url, UriKind.Absolute, out Uri result)){
                GLib.IFile file = GLib.FileFactory.NewForUri(result);
                GLib.FileInputStream stream = file.Read(new GLib.Cancellable());
                Gdk.Pixbuf pixbuf = new Gdk.Pixbuf(stream, new GLib.Cancellable());
                _image = new Bitmap(0, 0);
                _image.Pixbuf = pixbuf;
            }
            else
            {
                Gdk.Pixbuf pixbuf = new Gdk.Pixbuf(url.Replace("\\\\", "/").Replace("\\", "/"));
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
                if (System.IO.File.Exists(ImageLocation))
                {
                    Load(ImageLocation);
                }
            }
            catch { }
        }
        public void LoadAsync() { 
            if (System.IO.File.Exists(ImageLocation)) { 
                LoadAsync(ImageLocation);
            } 
        }
        public void LoadAsync(string url) {
            Threading.Tasks.Task.Run(() => Gtk.Application.Invoke(new EventHandler((o, e) => { 
                Load(url);
            })));
        }
  
        public override void EndInit()
        {

        }
    }
}
