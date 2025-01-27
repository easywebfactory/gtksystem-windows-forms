using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{

    internal
class FuncLoader
    {
        private class Windows
        {
            [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
            public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

            [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr LoadLibraryW(string lpszLib);
        }

        private class Linux
        {
            [DllImport("libdl.so.2")]
            public static extern IntPtr dlopen(string path, int flags);

            [DllImport("libdl.so.2")]
            public static extern IntPtr dlsym(IntPtr handle, string symbol);
        }

        private class OSX
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

        private const int RTLD_LAZY = 0x0001;
        private const int RTLD_GLOBAL = 0x0100;

        public static bool IsWindows, IsOSX, IsLinux;

        static FuncLoader()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    IsWindows = true;
                    break;
                case PlatformID.MacOSX:
                    IsOSX = true;
                    break;
                case PlatformID.Unix:
                    try
                    {
                        var buf = Marshal.AllocHGlobal(8192);
                        if (uname(buf) == 0 && Marshal.PtrToStringAnsi(buf) == "Darwin")
                            IsOSX = true;
                        if (uname(buf) == 0 && Marshal.PtrToStringAnsi(buf) == "Linux")
                            IsLinux = true;

                        Marshal.FreeHGlobal(buf);
                    }
                    catch { }

                    break;
            }
        }

        public static IntPtr LoadLibrary(string libname)
        {
            if (IsWindows)
                return Windows.LoadLibraryW(libname);

            if (IsOSX)
                return OSX.dlopen(libname, RTLD_GLOBAL | RTLD_LAZY);

            if (IsLinux)
                return Linux.dlopen(libname, RTLD_GLOBAL | RTLD_LAZY);

            return Unix.dlopen(libname, RTLD_GLOBAL | RTLD_LAZY);
        }

        public static IntPtr GetProcAddress(IntPtr library, string function)
        {
            var ret = IntPtr.Zero;

            if (IsWindows)
                ret = Windows.GetProcAddress(library, function);
            else if (IsOSX)
                ret = OSX.dlsym(library, function);
            else if (IsLinux)
                ret = Linux.dlsym(library, function);
            else
                ret = Unix.dlsym(library, function);

            return ret;
        }

        public static T LoadFunction<T>(IntPtr procaddress)
        {
            if (procaddress == IntPtr.Zero)
                return default(T);

            return Marshal.GetDelegateForFunctionPointer<T>(procaddress);
        }
    }


    internal class GLibrary
    {
        private static Dictionary<Library, IntPtr> _libraries;

        private static Dictionary<string, IntPtr> _customlibraries;

        private static Dictionary<Library, string[]> _libraryDefinitions;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);

        static GLibrary()
        {
            _customlibraries = new Dictionary<string, IntPtr>();
            _libraries = new Dictionary<Library, IntPtr>();
            _libraryDefinitions = new Dictionary<Library, string[]>();
            _libraryDefinitions[Library.GLib] = new string[4] { "libglib-2.0-0.dll", "libglib-2.0.so.0", "libglib-2.0.0.dylib", "glib-2.dll" };
            _libraryDefinitions[Library.GObject] = new string[4] { "libgobject-2.0-0.dll", "libgobject-2.0.so.0", "libgobject-2.0.0.dylib", "gobject-2.dll" };
            _libraryDefinitions[Library.Cairo] = new string[4] { "libcairo-2.dll", "libcairo.so.2", "libcairo.2.dylib", "cairo.dll" };
            _libraryDefinitions[Library.Gio] = new string[4] { "libgio-2.0-0.dll", "libgio-2.0.so.0", "libgio-2.0.0.dylib", "gio-2.dll" };
            _libraryDefinitions[Library.Atk] = new string[4] { "libatk-1.0-0.dll", "libatk-1.0.so.0", "libatk-1.0.0.dylib", "atk-1.dll" };
            _libraryDefinitions[Library.Pango] = new string[4] { "libpango-1.0-0.dll", "libpango-1.0.so.0", "libpango-1.0.0.dylib", "pango-1.dll" };
            _libraryDefinitions[Library.Gdk] = new string[4] { "libgdk-3-0.dll", "libgdk-3.so.0", "libgdk-3.0.dylib", "gdk-3.dll" };
            _libraryDefinitions[Library.GdkPixbuf] = new string[4] { "libgdk_pixbuf-2.0-0.dll", "libgdk_pixbuf-2.0.so.0", "libgdk_pixbuf-2.0.dylib", "gdk_pixbuf-2.dll" };
            _libraryDefinitions[Library.Gtk] = new string[4] { "libgtk-3-0.dll", "libgtk-3.so.0", "libgtk-3.0.dylib", "gtk-3.dll" };
            _libraryDefinitions[Library.PangoCairo] = new string[4] { "libpangocairo-1.0-0.dll", "libpangocairo-1.0.so.0", "libpangocairo-1.0.0.dylib", "pangocairo-1.dll" };
            _libraryDefinitions[Library.GtkSource] = new string[4] { "libgtksourceview-4-0.dll", "libgtksourceview-4.so.0", "libgtksourceview-4.0.dylib", "gtksourceview-4.dll" };
            _libraryDefinitions[Library.Webkit] = new[] { "libwebkit2gtk-4.0.dll", "libwebkit2gtk-4.0.so.37", "libwebkit2gtk-4.0.dylib", "libwebkit2gtk-4.0.0.dll" };
        }

        public static IntPtr Load(Library library)
        {
            IntPtr value = IntPtr.Zero;
            if (_libraries.TryGetValue(library, out value))
            {
                return value;
            }

            if (FuncLoader.IsWindows)
            {
                value = FuncLoader.LoadLibrary(_libraryDefinitions[library][0]);
                if (value == IntPtr.Zero)
                {
                    SetDllDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Gtk", "3.24.24"));
                    value = FuncLoader.LoadLibrary(_libraryDefinitions[library][0]);
                }
            }
            else if (FuncLoader.IsOSX)
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
                for (int i = 0; i < _libraryDefinitions[library].Length; i++)
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
                throw new DllNotFoundException(library.ToString() + ": " + string.Join(", ", _libraryDefinitions[library]));
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

}