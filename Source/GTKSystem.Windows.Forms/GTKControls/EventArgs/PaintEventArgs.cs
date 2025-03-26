﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms;

/// <summary>
///  Provides data for the <see cref='Control.Paint'/>
///  event.
///  NOTE: Please keep this class consistent with PrintPageEventArgs.
/// </summary>
public class PaintEventArgs : EventArgs, IDisposable
{
    /// <summary>
    ///  Graphics object with which painting should be done.
    /// </summary>
    private readonly Graphics? _graphics;

    /// <summary>
    ///  See ResetGraphics()
    /// </summary>
#pragma warning disable CS0169 // Field is never used
    private GraphicsState? _savedGraphicsState;
#pragma warning restore CS0169 // Field is never used

    /// <summary>
    ///  DC (Display context) for obtaining the graphics object. Used to delay
    ///  getting the graphics object until absolutely necessary (for perf reasons)
    /// </summary>
    private readonly IntPtr _dc = IntPtr.Zero;

    private IntPtr oldPal = IntPtr.Zero;

#if DEBUG
    private static readonly TraceSwitch paintEventFinalizationSwitch = new("PaintEventFinalization", "Tracks the creation and finalization of PaintEvent objects");

    internal static string GetAllocationStack()
    {
        if (paintEventFinalizationSwitch.TraceVerbose)
        {
            return Environment.StackTrace;
        }

        return "Enabled 'PaintEventFinalization' trace switch to see stack of allocation";
    }
    private readonly string allocationSite = GetAllocationStack();
#endif

    /// <summary>
    ///  Initializes a new instance of the <see cref='PaintEventArgs'/>
    ///  class with the specified graphics and clipping rectangle.
    /// </summary>
    public PaintEventArgs(Graphics? graphics, Rectangle clipRect)
    {
        _graphics = graphics ?? throw new ArgumentNullException(nameof(graphics));
        ClipRectangle = clipRect;
    }

    /// <summary>
    ///  Internal version of constructor for performance
    ///  We try to avoid getting the graphics object until needed
    /// </summary>
    internal PaintEventArgs(IntPtr dc, Rectangle clipRect)
    {
        Debug.Assert(dc != IntPtr.Zero, "dc is not initialized.");

        _dc = dc;
        ClipRectangle = clipRect;
    }

    ~PaintEventArgs() => Dispose(false);

    /// <summary>
    ///  Gets the rectangle in which to paint.
    /// </summary>
    public Rectangle ClipRectangle { get; }

    /// <summary>
    ///  Gets the HDC this paint event is connected to.  If there is no associated
    ///  HDC, or the GDI+ Graphics object has been created (meaning GDI+ now owns the
    ///  HDC), 0 is returned.
    /// </summary>
    internal IntPtr Hdc => _graphics == null ? _dc : IntPtr.Zero;

    /// <summary>
    ///  Gets the <see cref='Drawing.Graphics'/> object used to paint.
    /// </summary>
    public Graphics? Graphics => _graphics;

    /// <summary>
    ///  Disposes of the resources (other than memory) used by the
    /// <see cref='PaintEventArgs'/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {

        if (oldPal != IntPtr.Zero && _dc != IntPtr.Zero)
        {
            //SafeNativeMethods.SelectPalette(new HandleRef?.Invoke(this, _dc), new HandleRef?.Invoke(this, oldPal), 0);
            oldPal = IntPtr.Zero;
        }
    }

    /// <summary>
    ///  If ControlStyles.AllPaintingInWmPaint, we call this method
    ///  after OnPaintBackground so it appears to OnPaint that it's getting a fresh
    ///  Graphics.  We want to make sure AllPaintingInWmPaint is purely an optimization,
    ///  and doesn't change behavior, so we need to make sure any clipping regions established
    ///  in OnPaintBackground don't apply to OnPaint.
    /// </summary>
    internal void ResetGraphics()
    {
  
    }
}