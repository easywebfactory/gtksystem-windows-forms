using System.Runtime.InteropServices;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;

namespace System.Windows.Forms
{
    /// <summary>
    ///  GtkMenuItem
    /// </summary>
    public class GtkMenuItem : Gtk.MenuItem
    {
        internal Gtk.Image DefaultImage { get; set; } = new Gtk.Image("image-missing", Gtk.IconSize.SmallToolbar);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr d_gtk_image_menu_item_get_image(IntPtr raw);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void d_gtk_image_menu_item_set_image(IntPtr raw, IntPtr image);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool d_gtk_image_menu_item_get_always_show_image(IntPtr raw);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void d_gtk_image_menu_item_set_always_show_image(IntPtr raw, bool always_show);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr d_gtk_image_menu_item_get_type();

        private static d_gtk_image_menu_item_get_image gtk_image_menu_item_get_image = FuncLoader.LoadFunction<d_gtk_image_menu_item_get_image>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_image_menu_item_get_image"));

        private static d_gtk_image_menu_item_set_image gtk_image_menu_item_set_image = FuncLoader.LoadFunction<d_gtk_image_menu_item_set_image>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_image_menu_item_set_image"));

        private static d_gtk_image_menu_item_get_always_show_image gtk_image_menu_item_get_always_show_image = FuncLoader.LoadFunction<d_gtk_image_menu_item_get_always_show_image>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_image_menu_item_get_always_show_image"));

        private static d_gtk_image_menu_item_set_always_show_image gtk_image_menu_item_set_always_show_image = FuncLoader.LoadFunction<d_gtk_image_menu_item_set_always_show_image>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_image_menu_item_set_always_show_image"));

        private static d_gtk_image_menu_item_get_type gtk_image_menu_item_get_type = FuncLoader.LoadFunction<d_gtk_image_menu_item_get_type>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_image_menu_item_get_type"));


        [GLib.Property("Image")]
        internal Gtk.Widget IcoImage
        {
            get
            {
                return GLib.Object.GetObject(gtk_image_menu_item_get_image(base.Handle)) as Gtk.Widget;
            }
            set
            {
                gtk_image_menu_item_set_image(base.Handle, value?.Handle ?? IntPtr.Zero);
            }
        }

        [GLib.Property("always-show-image")]
        internal bool AlwaysShowImage
        {
            get
            {
                return gtk_image_menu_item_get_always_show_image(base.Handle);
            }
            set
            {
                gtk_image_menu_item_set_always_show_image(base.Handle, value);
            }
        }

        public new static GLib.GType GType
        {
            get
            {
                IntPtr val = gtk_image_menu_item_get_type();
                return new GLib.GType(val);
            }
        }
        public GtkMenuItem()
        { 
        }
    }

}


