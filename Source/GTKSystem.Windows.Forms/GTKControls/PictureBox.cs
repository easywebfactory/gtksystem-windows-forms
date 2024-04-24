/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;
using GTKSystem.Windows.Forms;
using GTKSystem.Windows.Forms.Utility;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class PictureBox : Control// WidgetControl<PictureBox.GtkImage>
    {
        private PictureBox.GtkImage self;
        public override Widget Widget { get => self; }
        public override object GtkControl => self;
        public PictureBox()
        {
            self = new GtkImage();
            self.Override.AddClass("PictureBox");
            self.Halign = Gtk.Align.Center;
            self.Valign = Gtk.Align.Center;
            self.Xalign = 0.5f;
            self.Yalign = 0.5f;
            self.Realized += Control_Realized;
            //self.Shown += Self_Shown;
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

            if (BackgroundImage != null && BackgroundImage.PixbufData != null)
            {
                ImageUtility.ScaleImageByImageLayout(_image.PixbufData, width, height, out Gdk.Pixbuf newImagePixbuf, BackgroundImageLayout);
                self.Pixbuf = newImagePixbuf;
            }

            if (_image != null && _image.PixbufData != null)
            {
                ImageUtility.ScaleImageByPictureBoxSizeMode(_image.PixbufData, width, height, out Gdk.Pixbuf newImagePixbuf, SizeMode);
                self.Pixbuf = newImagePixbuf;
            }
            else if (InitialImage != null && InitialImage.PixbufData != null)
            {
                ImageUtility.ScaleImageByPictureBoxSizeMode(InitialImage.PixbufData, width, height, out Gdk.Pixbuf newImagePixbuf, SizeMode);
                self.Pixbuf = newImagePixbuf;
            }
        }

        private void Control_Realized(object sender, EventArgs e)
        {
            UpdateStyle();
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

            if (BackgroundImage != null && BackgroundImage.PixbufData != null)
            {
                ImageUtility.ScaleImageByImageLayout(_image.PixbufData, width, height, out Gdk.Pixbuf newImagePixbuf, BackgroundImageLayout);
                self.Pixbuf = newImagePixbuf;
            }

            if (_image != null && _image.PixbufData != null)
            {
                ImageUtility.ScaleImageByPictureBoxSizeMode(_image.PixbufData, width, height, out Gdk.Pixbuf newImagePixbuf, SizeMode);
                self.Pixbuf = newImagePixbuf;
            }
            else if (InitialImage != null && InitialImage.PixbufData != null)
            {
                ImageUtility.ScaleImageByPictureBoxSizeMode(InitialImage.PixbufData, width, height, out Gdk.Pixbuf newImagePixbuf, SizeMode);
                self.Pixbuf = newImagePixbuf;
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
                if (self.IsRealized && _image != null && _image.PixbufData != null)
                {
                    Control_Realized(null, null);
                }
            }
        }

        public System.Drawing.Image ErrorImage { get; set; }

        [DefaultValue(BorderStyle.None)]
        public override BorderStyle BorderStyle { get; set; }

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
                    Gdk.Rectangle rec = Widget.Allocation;
                    byte[] bytedata = memoryStream.GetBuffer();
                    _image =new Bitmap(bytedata);
 
                    ImageUtility.ScaleImageByImageLayout(_image.PixbufData, rec.Width, rec.Height, out Gdk.Pixbuf newImagePixbuf, BackgroundImageLayout);
                    self.Pixbuf = newImagePixbuf;
                }
            }
        }
        public new void Load() { if (System.IO.File.Exists(ImageLocation)) { self.File = ImageLocation; } }
        public void LoadAsync() { if (System.IO.File.Exists(ImageLocation)) { self.File = ImageLocation; } }
        public void LoadAsync(string url) { Threading.Tasks.Task.Run(() => Load(url)); }
  
        public override void EndInit()
        {

        }

        public override ImageLayout BackgroundImageLayout { get => self.Override.BackgroundImageLayout; set => self.Override.BackgroundImageLayout = value; }
        public override Drawing.Image BackgroundImage { get => self.Override.BackgroundImage; set => self.Override.BackgroundImage = value; }
        public override Color BackColor { get => self.Override.BackColor.HasValue ? self.Override.BackColor.Value : Color.Transparent; set => self.Override.BackColor = value; }

        public override event PaintEventHandler Paint
        {
            add { self.Override.Paint += value; }
            remove { self.Override.Paint -= value; }
        }
        public sealed class GtkImage : Gtk.Image
        {
            internal GtkControlOverride Override;
            internal GtkImage() : base()
            {
                this.Override = new GtkControlOverride(this);
            }
            protected override void OnShown()
            {
                Override.OnAddClass();
                base.OnShown();
            }
            protected override bool OnDrawn(Cairo.Context cr)
            {
                Gdk.Rectangle rec = this.Allocation;
                Override.OnDrawnBackground(cr, rec);
                Override.OnPaint(cr, rec);
                return base.OnDrawn(cr);
            }
        }
    }
}
