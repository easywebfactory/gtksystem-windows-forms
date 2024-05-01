using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using GLib;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{

    internal class FuncLoader
    {
        private class Windows
        {
            [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
            public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

            [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
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

        private const int RTLD_LAZY = 1;

        private const int RTLD_GLOBAL = 256;

        public static bool IsWindows;

        public static bool IsOSX;

        [DllImport("libc")]
        private static extern int uname(IntPtr buf);

        static FuncLoader()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.Win32NT:
                case PlatformID.WinCE:
                    IsWindows = true;
                    break;
                case PlatformID.MacOSX:
                    IsOSX = true;
                    break;
                case PlatformID.Unix:
                    try
                    {
                        IntPtr intPtr = Marshal.AllocHGlobal(8192);
                        if (uname(intPtr) == 0 && Marshal.PtrToStringAnsi(intPtr) == "Darwin")
                        {
                            IsOSX = true;
                        }

                        Marshal.FreeHGlobal(intPtr);
                    }
                    catch
                    {
                    }

                    break;
                case PlatformID.Xbox:
                    break;
            }
        }

        public static IntPtr LoadLibrary(string libname)
        {
            if (IsWindows)
            {
                return Windows.LoadLibraryW(libname);
            }

            if (IsOSX)
            {
                return OSX.dlopen(libname, 257);
            }

            return Linux.dlopen(libname, 257);
        }

        public static IntPtr GetProcAddress(IntPtr library, string function)
        {
            IntPtr zero = IntPtr.Zero;
            if (IsWindows)
            {
                return Windows.GetProcAddress(library, function);
            }

            if (IsOSX)
            {
                return OSX.dlsym(library, function);
            }

            return Linux.dlsym(library, function);
        }

        public static T LoadFunction<T>(IntPtr procaddress)
        {
            if (procaddress == IntPtr.Zero)
            {
                return default;
            }

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
        GtkSource
    }

}