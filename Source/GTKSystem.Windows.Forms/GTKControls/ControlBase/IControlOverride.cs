using Gtk;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public interface IControlOverride
    {
        event DrawnHandler DrawnBackground;
        event PaintEventHandler Paint;
        Color? BackColor { get; set; }
        System.Drawing.Image BackgroundImage { get; set; }
        ImageLayout BackgroundImageLayout { get; set; }
        void AddClass(string cssClass);
        void OnAddClass();
        void OnDrawnBackground(Cairo.Context cr, Gdk.Rectangle area);
        void OnPaint(Cairo.Context cr, Gdk.Rectangle area);
    }
}
