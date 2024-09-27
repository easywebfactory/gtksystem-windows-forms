using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.Dialog
{
    public class FontDialog : CommonDialog
    {
        public Gtk.FontChooserDialog fontChooserDialog;
        public FontDialog() : base()
        {
            this.Reset();
        }

        [DefaultValue(true)]
        public virtual bool AllowFullOpen { get; set; }

        private Font? _font;
        public Font? Font { get => _font; set => _font = value; }

        [DefaultValue(false)]
        public virtual bool FullOpen { get; set; }

        [DefaultValue(false)]
        public virtual bool ShowHelp { get; set; }

        protected virtual IntPtr Instance { get; }

        protected virtual int Options { get; }

        public override void Reset()
        {
            _font = null;
        }

        private static Gtk.Window ActiveWindow = null;
        protected override bool RunDialog(IWin32Window owner)
        {
            if (owner != null && owner is Form ownerform)
            {
                fontChooserDialog = new Gtk.FontChooserDialog("选择字体", ownerform.self);
                fontChooserDialog.WindowPosition = Gtk.WindowPosition.CenterOnParent;
            }
            else
            {
                Gtk.Window window = Gtk.Window.ListToplevels().LastOrDefault(o => o is FormBase && o.IsActive);
                if (window != null)
                {
                    ActiveWindow = window;
                }
                fontChooserDialog = new Gtk.FontChooserDialog("选择字体", ActiveWindow);
                fontChooserDialog.WindowPosition = Gtk.WindowPosition.CenterOnParent;
            }
            fontChooserDialog.TypeHint = Gdk.WindowTypeHint.Dialog;
            if (null != _font)
                fontChooserDialog.Font = _font.Name + " " + (int)_font.Size;
            if (FullOpen && AllowFullOpen)
                fontChooserDialog.Fullscreen();
            int res = fontChooserDialog.Run();
            fontChooserDialog.ParentWindow = null;
            fontChooserDialog.HideOnDelete();

            var sFont = fontChooserDialog.Font;// 格式为: 字体名称 SIZE
            var sFontArray = sFont.Split(' ');
            var sFontSize = sFontArray.LastOrDefault();
            var sFontName = sFont.Substring(0, sFont.Length - sFontSize.Length).TrimEnd();
            _font = new Font(sFontName, int.Parse(sFontSize));

            return res == -5;
        }

        public override string ToString() => $"{_font.Name} {_font.Size}"; 
        protected override void Dispose(bool disposing)
        {
            if (fontChooserDialog != null)
            {
                fontChooserDialog.Dispose();
                fontChooserDialog = null;
            }
            base.Dispose(disposing);
        }
    }
}
