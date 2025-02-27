/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;
using System.Drawing;
using Gtk;

namespace System.Windows.Forms;

public class ColorDialog : CommonDialog
{
    public ColorChooserDialog? colorChooserDialog;
    private int[] customColors = [];

    [DefaultValue(true)] public virtual bool AllowFullOpen { get; set; } = true;

    [DefaultValue(false)]
    public virtual bool AnyColor { get; set; }

    public Color Color { get; set; } = Color.Black;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int[] CustomColors
    {
        get => customColors;
        set
        {
            if (value == null)
            {
                value = [];
            }
            customColors = value;
        }
    }

    [DefaultValue(false)]
    public virtual bool FullOpen { get; set; }

    [DefaultValue(false)]
    public virtual bool ShowHelp { get; set; }

    [DefaultValue(false)]
    public virtual bool SolidColorOnly { get; set; }

    protected virtual IntPtr Instance { get; } = default;

    protected virtual int Options { get; } = default;


    public override void Reset()
    {
        Color = Color.Black;
    }
    protected override bool RunDialog(IWin32Window? owner)
    {
        if (owner is Form ownerform)
        {
            colorChooserDialog = new ColorChooserDialog("选择颜色", ownerform.self);
            colorChooserDialog.WindowPosition = WindowPosition.CenterOnParent;
        }
        else
        {
            colorChooserDialog = new ColorChooserDialog("选择颜色", null);
            colorChooserDialog.WindowPosition = WindowPosition.Center;
        }
        colorChooserDialog.KeepAbove = true;
        if (Color.Name != "0")
            colorChooserDialog.Rgba = new Gdk.RGBA { Alpha = (double)Color.A / 255, Red = (double)Color.R / 255, Green = (double)Color.G / 255, Blue = (double)Color.B / 255 };
        if (FullOpen && AllowFullOpen)
            colorChooserDialog.Fullscreen();
        var res = colorChooserDialog.Run();
        colorChooserDialog.ParentWindow = null;
        colorChooserDialog.HideOnDelete();
        var colorSelection = colorChooserDialog.Rgba;
        Color = Color.FromArgb((int)(colorSelection.Alpha * 255), (int)Math.Round(colorSelection.Red * 255, 0), (int)Math.Round(colorSelection.Green * 255, 0), (int)Math.Round(colorSelection.Blue * 255, 0));
        return res == -5;
    }
 
    public override string ToString() { return $"System.Windows.Forms.ColorDialog,  Color: Color [{Color.Name}]"; }
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