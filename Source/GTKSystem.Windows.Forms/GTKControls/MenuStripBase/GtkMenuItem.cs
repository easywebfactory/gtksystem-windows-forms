using System.Runtime.InteropServices;

namespace System.Windows.Forms;

/// <summary>
///  GtkMenuItem
/// </summary>
public class GtkMenuItem : Gtk.MenuItem
{
    internal Gtk.Image DefaultImage { get; set; } = new("image-missing", Gtk.IconSize.SmallToolbar);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr DGtkImageMenuItemGetImage(IntPtr raw);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void DGtkImageMenuItemSetImage(IntPtr raw, IntPtr image);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool DGtkImageMenuItemGetAlwaysShowImage(IntPtr raw);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void DGtkImageMenuItemSetAlwaysShowImage(IntPtr raw, bool alwaysShow);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr DGtkImageMenuItemGetType();

    private static readonly DGtkImageMenuItemGetImage gtkImageMenuItemGetImage = FuncLoader.LoadFunction<DGtkImageMenuItemGetImage>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_image_menu_item_get_image"))!;

    private static readonly DGtkImageMenuItemSetImage gtkImageMenuItemSetImage = FuncLoader.LoadFunction<DGtkImageMenuItemSetImage>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_image_menu_item_set_image"))!;

    private static readonly DGtkImageMenuItemGetAlwaysShowImage gtkImageMenuItemGetAlwaysShowImage = FuncLoader.LoadFunction<DGtkImageMenuItemGetAlwaysShowImage>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_image_menu_item_get_always_show_image"))!;

    private static readonly DGtkImageMenuItemSetAlwaysShowImage gtkImageMenuItemSetAlwaysShowImage = FuncLoader.LoadFunction<DGtkImageMenuItemSetAlwaysShowImage>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_image_menu_item_set_always_show_image"))!;

    private static readonly DGtkImageMenuItemGetType gtkImageMenuItemGetType = FuncLoader.LoadFunction<DGtkImageMenuItemGetType>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_image_menu_item_get_type"))!;


    [GLib.Property("Image")]
    internal Gtk.Widget? IcoImage
    {
        get => GetObject(gtkImageMenuItemGetImage(Handle)) as Gtk.Widget;
        set => gtkImageMenuItemSetImage(Handle, value?.Handle ?? IntPtr.Zero);
    }

    [GLib.Property("always-show-image")]
    internal bool AlwaysShowImage
    {
        get => gtkImageMenuItemGetAlwaysShowImage(Handle);
        set => gtkImageMenuItemSetAlwaysShowImage(Handle, value);
    }

    public new static GLib.GType GType
    {
        get
        {
            var val = gtkImageMenuItemGetType();
            return new GLib.GType(val);
        }
    }
}