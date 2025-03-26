// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET462_OR_GREATER
namespace System.Drawing;
#else
namespace System.Drawing.Gtk;
#endif

#if NET462_OR_GREATER
using GtkColor =System.Drawing.Color;
#else
using GtkColor = System.Drawing.Gtk.Color;
#endif
public static class SystemColors
{
    public static Color ActiveBorder => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.ActiveBorder).ToArgb());
    public static Color ActiveCaption => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.ActiveCaption).ToArgb());
    public static Color ActiveCaptionText => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.ActiveCaptionText).ToArgb());
    public static Color AppWorkspace => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.AppWorkspace).ToArgb());

    public static Color ButtonFace => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.ButtonFace).ToArgb());
    public static Color ButtonHighlight => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.ButtonHighlight).ToArgb());
    public static Color ButtonShadow => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.ButtonShadow).ToArgb());

    public static Color Control => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.Control).ToArgb());
    public static Color ControlDark => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.ControlDark).ToArgb());
    public static Color ControlDarkDark => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.ControlDarkDark).ToArgb());
    public static Color ControlLight => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.ControlLight).ToArgb());
    public static Color ControlLightLight => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.ControlLightLight).ToArgb());
    public static Color ControlText => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.ControlText).ToArgb());

    public static Color Desktop => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.Desktop).ToArgb());

    public static Color GradientActiveCaption => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.GradientActiveCaption).ToArgb());
    public static Color GradientInactiveCaption => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.GradientInactiveCaption).ToArgb());
    public static Color GrayText => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.GrayText).ToArgb());

    public static Color Highlight => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.Highlight).ToArgb());
    public static Color HighlightText => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.HighlightText).ToArgb());
    public static Color HotTrack => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.HotTrack).ToArgb());

    public static Color InactiveBorder => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.InactiveBorder).ToArgb());
    public static Color InactiveCaption => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.InactiveCaption).ToArgb());
    public static Color InactiveCaptionText => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.InactiveCaptionText).ToArgb());
    public static Color Info => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.Info).ToArgb());
    public static Color InfoText => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.InfoText).ToArgb());

    public static Color Menu => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.Menu).ToArgb());
    public static Color MenuBar => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.MenuBar).ToArgb());
    public static Color MenuHighlight => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.MenuHighlight).ToArgb());
    public static Color MenuText => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.MenuText).ToArgb());

    public static Color ScrollBar => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.ScrollBar).ToArgb());

    public static Color Window => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.Window).ToArgb());
    public static Color WindowFrame => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.WindowFrame).ToArgb());
    public static Color WindowText => Color.FromArgb(GtkColor.FromKnownColor(KnownColor.WindowText).ToArgb());
}