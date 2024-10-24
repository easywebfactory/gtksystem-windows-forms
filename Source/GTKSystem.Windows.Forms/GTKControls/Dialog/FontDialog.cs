using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Windows.Forms;
using Gtk;

namespace System.Windows.Forms
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
                fontChooserDialog = new Gtk.FontChooserDialog("选择字体", null);
                fontChooserDialog.WindowPosition = Gtk.WindowPosition.Center;
            }
            fontChooserDialog.KeepAbove = true;
            if (null != _font)
                fontChooserDialog.Font = _font.Name + " " + (int)_font.Size;
            if (FullOpen && AllowFullOpen)
                fontChooserDialog.Fullscreen();
            int res = fontChooserDialog.Run();
            FontStyle fontStyle = FontStyle.Regular;
            switch (fontChooserDialog.FontDesc.Weight)
            {
                case Pango.Weight.Bold:
                case Pango.Weight.Ultrabold:
                case Pango.Weight.Semibold:
                    fontStyle |= FontStyle.Bold; 
                    break;
            }
            switch (fontChooserDialog.FontDesc.Style)
            {
                case Pango.Style.Italic:
                case Pango.Style.Oblique:
                    fontStyle |= FontStyle.Italic;
                    break;
            }
            _font = new Font(fontChooserDialog.FontDesc.Family, (int)(fontChooserDialog.FontDesc.Size / Pango.Scale.PangoScale), fontStyle);

            fontChooserDialog.HideOnDelete();
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
