/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class ColorDialog : CommonDialog
    {
        public Gtk.ColorChooserDialog colorChooserDialog;
        public ColorDialog() : base()
        {

        }

        [DefaultValue(true)]
        public virtual bool AllowFullOpen { get; set; }

        [DefaultValue(false)]
        public virtual bool AnyColor { get; set; }
        public Color Color { get; set; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int[] CustomColors { get; set; }

        [DefaultValue(false)]
        public virtual bool FullOpen { get; set; }

        [DefaultValue(false)]
        public virtual bool ShowHelp { get; set; }

        [DefaultValue(false)]
        public virtual bool SolidColorOnly { get; set; }

        protected virtual IntPtr Instance { get; }

        protected virtual int Options { get; }


        public override void Reset()
        {
            Color = Color.Black;
        }
        protected override bool RunDialog(IWin32Window owner)
        {
            if (owner != null && owner is Form ownerform)
            {
                colorChooserDialog = new Gtk.ColorChooserDialog(Gtk.Windows.Forms.Properties.Resources.ColorDialog_RunDialog_Choose_color, ownerform.self);
                colorChooserDialog.WindowPosition = Gtk.WindowPosition.CenterOnParent;
            }
            else
            {
                colorChooserDialog = new Gtk.ColorChooserDialog(Gtk.Windows.Forms.Properties.Resources.ColorDialog_RunDialog_Choose_color, null);
                colorChooserDialog.WindowPosition = Gtk.WindowPosition.Center;
            }
            colorChooserDialog.KeepAbove = true;
            if (Color.Name != "0")
                colorChooserDialog.Rgba = new Gdk.RGBA() { Alpha = (double)Color.A / 255, Red = (double)Color.R / 255, Green = (double)Color.G / 255, Blue = (double)Color.B / 255 };
            if (FullOpen && AllowFullOpen)
                colorChooserDialog.Fullscreen();
            int res = colorChooserDialog.Run();
            colorChooserDialog.ParentWindow = null;
            colorChooserDialog.HideOnDelete();
            Gdk.RGBA colorSelection = colorChooserDialog.Rgba;
            this.Color = Color.FromArgb((int)(colorSelection.Alpha * 255), (int)Math.Round(colorSelection.Red * 255, 0), (int)Math.Round(colorSelection.Green * 255, 0), (int)Math.Round(colorSelection.Blue * 255, 0));
            return res == -5;
        }
 
        public override string ToString() { return this.Color.Name; }
        protected override void Dispose(bool disposing)
        {
            if (colorChooserDialog != null)
            {
                colorChooserDialog.Dispose();
                colorChooserDialog = null;
            }
            base.Dispose(disposing);
        }
    }
}
