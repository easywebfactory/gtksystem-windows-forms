﻿using Cairo;
using GTKSystem.Windows.Forms.GTKControls;

namespace System.Windows.Forms;

public sealed class ViewportBase : Gtk.Viewport, IGtkControl
{
    public IGtkControlOverride Override { get; set; }
    public ViewportBase()
    {
        Override = new GtkFormsControlOverride(this);
        Halign = Gtk.Align.Start;
        Valign = Gtk.Align.Start;
    }
    protected override bool OnDrawn(Context? cr)
    {
        var rec = new Gdk.Rectangle(0, 0, AllocatedWidth, AllocatedHeight);
        Override.OnDrawnBackground(cr, rec);
        return base.OnDrawn(cr);
    }
}