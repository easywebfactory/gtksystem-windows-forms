/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class PictureBox : WidgetControl<Gtk.Image>
    {
        public PictureBox() : base()
        {
            Widget.StyleContext.AddClass("PictureBox");
            base.Control.SetAlignment(0.5f, 0.5f);
            base.Control.Xalign = 0.5f;
            base.Control.Yalign = 0.5f;
            base.Control.Realized += Control_Realized;
        }

        private void Control_Realized(object sender, EventArgs e)
        {

            if (BackgroundImage != null && BackgroundImage.PixbufData != null)
            {
                Gdk.Pixbuf imagePixbuf = new Gdk.Pixbuf(IntPtr.Zero);
                base.ScaleImage(ref imagePixbuf, BackgroundImage.PixbufData, PictureBoxSizeMode.AutoSize, BackgroundImageLayout == ImageLayout.None ? ImageLayout.Tile : BackgroundImageLayout);
                base.Control.Pixbuf = imagePixbuf;
            }

            if (_image != null && _image.PixbufData != null)
            {
                Gdk.Pixbuf imagePixbuf = new Gdk.Pixbuf(IntPtr.Zero);
                base.ScaleImage(ref imagePixbuf, _image.PixbufData, SizeMode, ImageLayout.None);
                base.Control.Pixbuf = imagePixbuf;
            }
            else if (InitialImage != null && InitialImage.PixbufData != null)
            {
                Gdk.Pixbuf imagePixbuf = new Gdk.Pixbuf(IntPtr.Zero);
                base.ScaleImage(ref imagePixbuf, InitialImage.PixbufData, SizeMode, ImageLayout.None);
                base.Control.Pixbuf = imagePixbuf;
            }
        }

        public PictureBoxSizeMode SizeMode { get; set; }

        public System.Drawing.Image InitialImage { get; set; }

        public string ImageLocation { get; set; }

        private System.Drawing.Image _image;
        public System.Drawing.Image Image { 
            get { return _image; }
            set {
                _image = value;
                if (base.Control.IsRealized && _image != null && _image.PixbufData != null)
                {
                    Gdk.Pixbuf imagePixbuf = new Gdk.Pixbuf(IntPtr.Zero);
                    base.ScaleImage(ref imagePixbuf, _image.PixbufData, SizeMode, ImageLayout.None);
                    base.Control.Pixbuf = imagePixbuf;
                }
            }
        }

        public System.Drawing.Image ErrorImage { get; set; }

        [DefaultValue(BorderStyle.None)]
        public BorderStyle BorderStyle { get; set; }

        public void CancelAsync() { }
        public new void Load(string url) {
            GLib.IFile file = null;
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri result)){
                file = GLib.FileFactory.NewForUri(result);
            }
            else
            {
                file = GLib.FileFactory.NewForPath(url.Replace("\\\\", "/").Replace("\\", "/"));
            }
            GLib.FileInputStream stream = file.Read(new GLib.Cancellable());
            if (stream != null)
            {
                byte[] buffer = new byte[1024];
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    GLib.Cancellable cancelback = new GLib.Cancellable();
                    while (stream.ReadAll(buffer, 1024, out ulong bytes_read, cancelback))
                    {
                        if (bytes_read < 1024)
                        {
                            for (ulong i = 0; i < bytes_read; i++)
                                memoryStream.WriteByte(buffer[i]);

                            // cancelback.Cancel();
                            break;
                        }
                        else
                            memoryStream.Write(buffer);
                    }
                    byte[] bytedata = memoryStream.GetBuffer();
                    _image =new Bitmap(bytedata);
                    Gdk.Pixbuf imagePixbuf = new Gdk.Pixbuf(IntPtr.Zero);
                    base.ScaleImage(ref imagePixbuf, bytedata, SizeMode, ImageLayout.None);
                    base.Control.Pixbuf = imagePixbuf;
                }
            }
        }
        public new void Load() { if (System.IO.File.Exists(ImageLocation)) { base.Control.File = ImageLocation; } }
        public void LoadAsync() { if (System.IO.File.Exists(ImageLocation)) { base.Control.File = ImageLocation; } }
        public void LoadAsync(string url) { Threading.Tasks.Task.Run(() => Load(url)); }
  
        public override void EndInit()
        {

        }
    }
}
