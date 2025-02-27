using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Windows.Forms;

internal class FuncLoader
{
    private class Linux
    {
        [DllImport("libdl.so.2")]
        public static extern IntPtr dlopen(string path, int flags);

        [DllImport("libdl.so.2")]
        public static extern IntPtr dlsym(IntPtr handle, string symbol);
    }

    private class Osx
    {
        [DllImport("/usr/lib/libSystem.dylib")]
        public static extern IntPtr dlopen(string path, int flags);

        [DllImport("/usr/lib/libSystem.dylib")]
        public static extern IntPtr dlsym(IntPtr handle, string symbol);
    }

    private class Unix
    {
        [DllImport("libc")]
        public static extern IntPtr dlopen(string path, int flags);

        [DllImport("libc")]
        public static extern IntPtr dlsym(IntPtr handle, string symbol);
    }

    [DllImport("libc")]
    private static extern int uname(IntPtr buf);

    private const int rtldLazy = 0x0001;
    private const int rtldGlobal = 0x0100;

    public static bool isWindows, isOsx, isLinux;

    static FuncLoader()
    {
        switch (Environment.OSVersion.Platform)
        {
            case PlatformID.Win32NT:
            case PlatformID.Win32S:
            case PlatformID.Win32Windows:
            case PlatformID.WinCE:
                isWindows = true;
                break;
            case PlatformID.MacOSX:
                isOsx = true;
                break;
            case PlatformID.Unix:
                try
                {
                    var buf = Marshal.AllocHGlobal(8192);
                    if (uname(buf) == 0 && Marshal.PtrToStringAnsi(buf) == "Darwin")
                        isOsx = true;
                    if (uname(buf) == 0 && Marshal.PtrToStringAnsi(buf) == "Linux")
                        isLinux = true;

                    Marshal.FreeHGlobal(buf);
                }
                catch (Exception ex)
                {
                    Trace.Write(ex);
                }

                break;
        }
    }

    public static IntPtr LoadLibrary(string libname)
    {
        if (isWindows)
            return Windows.LoadLibraryW(libname);

        if (isOsx)
            return Osx.dlopen(libname, rtldGlobal | rtldLazy);

        if (isLinux)
            return Linux.dlopen(libname, rtldGlobal | rtldLazy);

        return Unix.dlopen(libname, rtldGlobal | rtldLazy);
    }

    public static IntPtr GetProcAddress(IntPtr library, string function)
    {
        IntPtr procAddress;

        if (isWindows)
            procAddress = Windows.GetProcAddress(library, function);
        else if (isOsx)
            procAddress = Osx.dlsym(library, function);
        else if (isLinux)
            procAddress = Linux.dlsym(library, function);
        else
            procAddress = Unix.dlsym(library, function);

        return procAddress;
    }

    public static T? LoadFunction<T>(IntPtr procaddress)
    {
        return procaddress == IntPtr.Zero ? default : Marshal.GetDelegateForFunctionPointer<T>(procaddress);
    }
}

internal class Windows
{
    [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr LoadLibraryW(string lpszLib);
}

internal class GLibrary
{
    private static readonly Dictionary<Library, IntPtr> _libraries;

    private static Dictionary<string, IntPtr> _customlibraries;

    private static readonly Dictionary<Library, string[]> _libraryDefinitions;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetDllDirectory(string lpPathName);

    static GLibrary()
    {
        _customlibraries = new Dictionary<string, IntPtr>();
        _libraries = new Dictionary<Library, IntPtr>();
        _libraryDefinitions = new Dictionary<Library, string[]>
        {
            [Library.GLib] = ["libglib-2.0-0.dll", "libglib-2.0.so.0", "libglib-2.0.0.dylib", "glib-2.dll"],
            [Library.GObject] = ["libgobject-2.0-0.dll", "libgobject-2.0.so.0", "libgobject-2.0.0.dylib", "gobject-2.dll"],
            [Library.Cairo] = ["libcairo-2.dll", "libcairo.so.2", "libcairo.2.dylib", "cairo.dll"],
            [Library.Gio] = ["libgio-2.0-0.dll", "libgio-2.0.so.0", "libgio-2.0.0.dylib", "gio-2.dll"],
            [Library.Atk] = ["libatk-1.0-0.dll", "libatk-1.0.so.0", "libatk-1.0.0.dylib", "atk-1.dll"],
            [Library.Pango] = ["libpango-1.0-0.dll", "libpango-1.0.so.0", "libpango-1.0.0.dylib", "pango-1.dll"],
            [Library.Gdk] = ["libgdk-3-0.dll", "libgdk-3.so.0", "libgdk-3.0.dylib", "gdk-3.dll"],
            [Library.GdkPixbuf] = ["libgdk_pixbuf-2.0-0.dll", "libgdk_pixbuf-2.0.so.0", "libgdk_pixbuf-2.0.dylib", "gdk_pixbuf-2.dll"],
            [Library.Gtk] = ["libgtk-3-0.dll", "libgtk-3.so.0", "libgtk-3.0.dylib", "gtk-3.dll"],
            [Library.PangoCairo] = ["libpangocairo-1.0-0.dll", "libpangocairo-1.0.so.0", "libpangocairo-1.0.0.dylib", "pangocairo-1.dll"],
            [Library.GtkSource] = ["libgtksourceview-4-0.dll", "libgtksourceview-4.so.0", "libgtksourceview-4.0.dylib", "gtksourceview-4.dll"],
            [Library.Webkit] = ["libwebkit2gtk-4.0.dll", "libwebkit2gtk-4.0.so.37", "libwebkit2gtk-4.0.dylib", "libwebkit2gtk-4.0.0.dll"]
        };
    }

    public static IntPtr Load(Library library)
    {
        IntPtr value;
        if (_libraries.TryGetValue(library, out value))
        {
            return value;
        }

        if (FuncLoader.isWindows)
        {
            value = FuncLoader.LoadLibrary(_libraryDefinitions[library][0]);
            if (value == IntPtr.Zero)
            {
                SetDllDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Gtk", "3.24.24"));
                value = FuncLoader.LoadLibrary(_libraryDefinitions[library][0]);
            }
        }
        else if (FuncLoader.isOsx)
        {
            value = FuncLoader.LoadLibrary(_libraryDefinitions[library][2]);
            if (value == IntPtr.Zero)
            {
                value = FuncLoader.LoadLibrary("/usr/local/lib/" + _libraryDefinitions[library][2]);
            }
        }
        else
        {
            value = FuncLoader.LoadLibrary(_libraryDefinitions[library][1]);
        }

        if (value == IntPtr.Zero)
        {
            for (var i = 0; i < _libraryDefinitions[library].Length; i++)
            {
                value = FuncLoader.LoadLibrary(_libraryDefinitions[library][i]);
                if (value != IntPtr.Zero)
                {
                    break;
                }
            }
        }

        if (value == IntPtr.Zero)
        {
            throw new DllNotFoundException(library + ": " + string.Join(", ", _libraryDefinitions[library]));
        }

        _libraries[library] = value;
        return value;
    }
}

internal enum Library
{
    GLib,
    GObject,
    Cairo,
    Gio,
    Atk,
    Pango,
    PangoCairo,
    Gdk,
    GdkPixbuf,
    Gtk,
    GtkSource,
    Webkit
}