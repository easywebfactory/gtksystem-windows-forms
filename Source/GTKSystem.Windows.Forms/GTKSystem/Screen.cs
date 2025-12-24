// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing;

namespace System.Windows.Forms;

/// <summary>
///  Represents a display device or multiple display devices on a single system.
/// </summary>
public partial class Screen
{
    private readonly Rectangle _bounds;
    private Rectangle _workingArea = Rectangle.Empty;
    private readonly bool _primary;
    private readonly string _deviceName;

    private readonly int _bitDepth;
    private static Screen[]? s_screens;
    private Gdk.Monitor _monitor;
    internal Screen(int monitornum):this(Gdk.Display.Default.GetMonitor(monitornum))
    {
    }
    internal Screen(Gdk.Monitor monitor)
    {
        _monitor = monitor;
        _primary = monitor.IsPrimary;
        _deviceName = monitor.Display.Name;
        _bitDepth = monitor.Display.DefaultScreen.SystemVisual.Depth;
        Gdk.Rectangle rect = monitor.Workarea;
        _workingArea = new Rectangle(rect.Left, rect.Top, rect.Width, rect.Height);
        Gdk.Rectangle rect2 = monitor.Geometry;
        _bounds = new Rectangle(rect2.Left, rect2.Top, rect2.Width, rect2.Height);
    }
    public static Screen[] AllScreens
    {
        get
        {
            if (s_screens is null)
            {
                int nmonitors = Gdk.Display.Default.NMonitors;
                s_screens = new Screen[nmonitors];
                for (int i = 0; i < nmonitors; i++)
                {
                    s_screens[i] = new Screen(i);
                }
            }
            return s_screens;
        }
    }
    public int BitsPerPixel => _bitDepth;
    public Rectangle Bounds => _bounds;
    public string DeviceName => _deviceName;
    public bool Primary => _primary;
    public static Screen? PrimaryScreen
    {
        get
        {
             return new Screen(Gdk.Display.Default.PrimaryMonitor);
        }
    }
    public Rectangle WorkingArea
    {
        get
        {
            return _workingArea;
        }
    }
    public override bool Equals(object? obj) => obj is Screen comp && _monitor.Handle == comp._monitor.Handle;
    public static Screen FromPoint(Point point)
    {
        Gdk.Monitor monitor = Gdk.Display.Default.GetMonitorAtPoint(point.X, point.Y);
        if (monitor != null)
            return new Screen(monitor);
        else
            return new Screen(Gdk.Display.Default.PrimaryMonitor);
    }
    public static Screen FromRectangle(Rectangle rect)
    {
        return FromPoint(rect.Location);
    }
    public static Screen FromControl(Control control)
    {
        if (control == null || control.Widget?.Window == null)
            throw new ArgumentNullException();
        return FromHandle(control.Widget.Window.Handle);
    }
    public static Screen FromHandle(IntPtr hwnd)
    {
        if (Gtk.Window.TryGetObject(hwnd) is Gdk.Window dwin)
        {
            Gdk.Monitor monitor = dwin.Display.GetMonitorAtWindow(dwin);
            return new Screen(monitor);
        }
        if (Gtk.Window.TryGetObject(hwnd) is Gtk.Window win)
        {
           Gdk.Monitor monitor = win.Display.GetMonitorAtWindow(win.Window);
            return new Screen(monitor);
        }
        return new Screen(Gdk.Display.Default.PrimaryMonitor);
    }
    public static Rectangle GetWorkingArea(Point pt) => FromPoint(pt).WorkingArea;
    public static Rectangle GetWorkingArea(Rectangle rect) => FromRectangle(rect).WorkingArea;
    public static Rectangle GetWorkingArea(Control ctl) => FromControl(ctl).WorkingArea;
    public static Rectangle GetBounds(Point pt) => FromPoint(pt).Bounds;
    public static Rectangle GetBounds(Rectangle rect) => FromRectangle(rect).Bounds;
    public static Rectangle GetBounds(Control ctl) => FromControl(ctl).Bounds;
    public override int GetHashCode() => _monitor.GetHashCode();
    public override string ToString()
        => $"{GetType().Name}[Bounds={_bounds} WorkingArea={WorkingArea} Primary={_primary} DeviceName={_deviceName}";
}
