using System.Drawing;
using Cairo;
using Gdk;
using Gtk;
using Color = System.Drawing.Color;
using Graphics = System.Drawing.Graphics;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;

namespace System.Windows.Forms;

public delegate void PaintGraphicsEventHandler(Context? cr, Rectangle rec);
public class GtkControlOverride: IControlOverride, IGtkControlOverride
{
    private readonly Widget? container;
    private Pixbuf? imagePixbuf;
    public GtkControlOverride(Widget? container)
    {
        this.container = container;
    }
    public event DrawnHandler? DrawnBackground;
    public event PaintEventHandler? Paint;
    public Color? BackColor { get; set; }
    private Image? backgroundImage;
    public Image? BackgroundImage { get => backgroundImage;
        set { backgroundImage = value; backgroundPixbuf = null; } }
    public ImageLayout BackgroundImageLayout { get; set; }
    public Image? Image { get; set; }
    public ContentAlignment ImageAlign { get; set; }

    private readonly List<string> cssList = [];
    public void AddClass(string cssClass)
    {
        cssList.Add(cssClass);
    }

    protected virtual void OnDrawnBackground(DrawnArgs e)
    {
        DrawnBackground?.Invoke(this, e);
    }

    void IGtkControlOverride.OnPaint(PaintEventArgs e)
    {
        Paint?.Invoke(this, e);
    }

    public void RemoveClass(string cssClass)
    {
        cssList.Remove(cssClass);
    }
    public void OnAddClass()
    {
        foreach (var cssClass in cssList)
        {
            if(container?.StyleContext.HasClass(cssClass)??false)
                container.StyleContext.RemoveClass(cssClass);
            container?.StyleContext.AddClass(cssClass);
        }
        ClearNativeBackground();
    }
    public void ClearNativeBackground()
    {
    }
    private Pixbuf? backgroundPixbuf;
    public void DrawnBackColor(Context cr, Gdk.Rectangle area)
    {
        if (BackColor.HasValue)
        {
            cr.Save();
            cr.SetSourceRGBA(BackColor.Value.R / 255f, BackColor.Value.G / 255f, BackColor.Value.B / 255f, BackColor.Value.A / 255f);
            cr.Paint();
            cr.Restore();
        }
    }
    public void OnDrawnBackground(Context? cr, Gdk.Rectangle area)
    {
        if (BackColor.HasValue && cr != null)
        {
            cr.Save();
            cr.SetSourceRGBA(BackColor.Value.R / 255f, BackColor.Value.G / 255f, BackColor.Value.B / 255f,
                BackColor.Value.A / 255f);
            cr.Paint();
            cr.Restore();
        }
        if (BackgroundImage is { PixbufData: not null })
        {
            if (backgroundPixbuf == null || backgroundPixbuf.Width != area.Width || backgroundPixbuf.Height != area.Height)
            {
                ImageUtility.ScaleImageByImageLayout(BackgroundImage.PixbufData, area.Width, area.Height, out backgroundPixbuf, BackgroundImageLayout);
            }
            ImageUtility.DrawImage(cr, backgroundPixbuf, area, ContentAlignment.TopLeft);
        }

        if (DrawnBackground != null)
        {
            var args = new DrawnArgs { Args = [cr] };
            DrawnBackground(container, args);
        }
    }
    
    public void OnDrawnImage(Context? cr, Gdk.Rectangle area)
    {
        if (Image is { PixbufData: not null })
        {
            if (imagePixbuf == null || imagePixbuf.Width != area.Width || imagePixbuf.Height != area.Height)
            {
                var imagepixbuf = new Pixbuf(Image.PixbufData);
                imagePixbuf = imagepixbuf.ScaleSimple(area.Width, area.Height, InterpType.Nearest);
            }
            ImageUtility.DrawImage(cr, imagePixbuf, area, ImageAlign);
        }
    }
        
    public event PaintGraphicsEventHandler? PaintGraphics;

    public void OnPaint(Context? cr, Gdk.Rectangle area)
    {
        PaintGraphics?.Invoke(cr, new Rectangle(area.X, area.Y, area.Width, area.Height));
        Paint?.Invoke(container, new PaintEventArgs(new Graphics(container, cr, area), new Rectangle(area.X, area.Y, area.Width, area.Height)));
    }
}