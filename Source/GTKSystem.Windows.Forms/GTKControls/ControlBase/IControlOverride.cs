using System.Drawing;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public interface IControlOverride
    {
        event PaintEventHandler Paint;
        event PaintGraphicsEventHandler PaintGraphics;
        Color? BackColor { get; set; }
        System.Drawing.Image BackgroundImage { get; set; }
        ImageLayout BackgroundImageLayout { get; set; }
        void OnDrawnBackground(Cairo.Context cr, Gdk.Rectangle area);
        void OnPaint(Cairo.Context cr, Gdk.Rectangle area);
    }
}
