using Gtk;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms;

public class FontDialog : CommonDialog
{
    public FontDialog()
    {
        Reset();
    }

    private void Init()
    {
        Reset();
    }

    [DefaultValue(true)]
    public virtual bool AllowFullOpen { get; set; }

    private Font? _font;
    private FontChooserDialog? _dialog;
    public Font? Font { get => _font; set => _font = value; }

    [DefaultValue(false)]
    public virtual bool FullOpen { get; set; }

    [DefaultValue(false)]
    public virtual bool ShowHelp { get; set; }

    protected virtual IntPtr Instance { get; } = default;

    protected virtual int Options { get; } = default;

    public new virtual void Dispose()
    {
        _dialog?.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }

    public override void Reset()
    {
        _font = null;
    }

    internal static Window? ActiveWindow = null;
    protected override bool RunDialog(IWin32Window? owner)
    {
        _dialog = null;
        if (owner is Form ownerform)
        {
            _dialog = new FontChooserDialog(Properties.Resources.FontDialog_RunDialog_Select_Font, ownerform.self);
            _dialog.WindowPosition = WindowPosition.CenterOnParent;
        }
        else
        {
            _dialog = new FontChooserDialog(Properties.Resources.FontDialog_RunDialog_Select_Font, null);
            _dialog.WindowPosition = WindowPosition.Center;
        }
        _dialog.IconName = "font-x-generic";
        _dialog.KeepAbove = true;
        if (null != _font)
            _dialog.Font = _font.Name + " " + (int)_font.Size;
        if (FullOpen && AllowFullOpen)
            _dialog.Fullscreen();
        var res = _dialog.Run();
        var fontStyle = FontStyle.Regular;
        switch (_dialog.FontDesc.Weight)
        {
            case Pango.Weight.Bold:
            case Pango.Weight.Ultrabold:
            case Pango.Weight.Semibold:
                fontStyle |= FontStyle.Bold;
                break;
        }
        switch (_dialog.FontDesc.Style)
        {
            case Pango.Style.Italic:
            case Pango.Style.Oblique:
                fontStyle |= FontStyle.Italic;
                break;
        }
        _font = new Font(_dialog.FontDesc.Family, (int)(_dialog.FontDesc.Size / Pango.Scale.PangoScale), fontStyle);

        _dialog.Dispose();
        _dialog.Destroy();
        return res == -5;
    }

    public override string ToString() => $"{_font?.Name} {_font?.Size}"; 
}