using Cairo;
using Gtk;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;

namespace System.Windows.Forms;

public interface IControlOverride
{
    event DrawnHandler DrawnBackground;
    event PaintEventHandler? Paint;
    event PaintGraphicsEventHandler? PaintGraphics;
    Color? BackColor { get; set; }
    Image? BackgroundImage { get; set; }
    ImageLayout BackgroundImageLayout { get; set; }
    void AddClass(string cssClass);
    void OnAddClass();
    void OnDrawnBackground(Context? cr, Gdk.Rectangle area);
    void OnPaint(Context? cr, Gdk.Rectangle area);
}