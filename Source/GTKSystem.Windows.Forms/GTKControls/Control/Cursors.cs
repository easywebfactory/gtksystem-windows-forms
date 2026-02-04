
using GTKSystem.Windows.Forms.Resources;

namespace System.Windows.Forms
{
    /// <summary>
    ///  Standard cursors
    /// </summary>
    public static class Cursors
    {
        private static Cursor? s_appStarting;
        private static Cursor? s_arrow;
        private static Cursor? s_cross;
        private static Cursor? s_defaultCursor;
        private static Cursor? s_iBeam;
        private static Cursor? s_no;
        private static Cursor? s_sizeAll;
        private static Cursor? s_sizeNESW;
        private static Cursor? s_sizeNS;
        private static Cursor? s_sizeNWSE;
        private static Cursor? s_sizeWE;
        private static Cursor? s_upArrow;
        private static Cursor? s_wait;
        private static Cursor? s_help;
        private static Cursor? s_hSplit;
        private static Cursor? s_vSplit;
        private static Cursor? s_noMove2D;
        private static Cursor? s_noMoveHoriz;
        private static Cursor? s_noMoveVert;
        private static Cursor? s_panEast;
        private static Cursor? s_panNE;
        private static Cursor? s_panNorth;
        private static Cursor? s_panNW;
        private static Cursor? s_panSE;
        private static Cursor? s_panSouth;
        private static Cursor? s_panSW;
        private static Cursor? s_panWest;
        private static Cursor? s_hand;

        public static Cursor AppStarting => s_appStarting ??= new Cursor(AssemblyResources.ToCursorsUri("circle.cur"), 0, 0);
        public static Cursor Arrow => s_arrow ??= new Cursor(AssemblyResources.ToCursorsUri("arrow.cur"), 0, 0);
        public static Cursor Cross => s_cross ??= new Cursor(AssemblyResources.ToCursorsUri("cross.cur"), 10, 10);
        public static Cursor Default => s_defaultCursor;
        public static Cursor IBeam => s_iBeam ??= new Cursor(AssemblyResources.ToCursorsUri("xterm.cur"), 0, 0);
        public static Cursor No => s_no ??= new Cursor(AssemblyResources.ToCursorsUri("not-allowed.cur"), 0, 0);
        public static Cursor SizeAll => s_sizeAll ??= new Cursor(AssemblyResources.ToCursorsUri("size_all.cur"), 10, 10);
        public static Cursor SizeNESW => s_sizeNESW ??= new Cursor(AssemblyResources.ToCursorsUri("nesw-resize.cur"), 10, 10);
        public static Cursor SizeNS => s_sizeNS ??= new Cursor(AssemblyResources.ToCursorsUri("ns-resize.cur"), 0, 10);
        public static Cursor SizeNWSE => s_sizeNWSE ??= new Cursor(AssemblyResources.ToCursorsUri("nwse-resize.cur"), 10, 10);
        public static Cursor SizeWE => s_sizeWE ??= new Cursor(AssemblyResources.ToCursorsUri("ew-resize.cur"), 10, 0);
        public static Cursor UpArrow => s_upArrow ??= new Cursor(AssemblyResources.ToCursorsUri("sb_up_arrow.cur"), 0, 0);
        public static Cursor WaitCursor => s_wait ??= new Cursor(Gdk.CursorType.Watch);
        public static Cursor Help => s_help ??= new Cursor(AssemblyResources.ToCursorsUri("help.cur"), 0, 0);
        public static Cursor Hand => s_hand ??= new Cursor(AssemblyResources.ToCursorsUri("hand.cur"), 0, 0);
        public static Cursor HSplit => s_hSplit ??= new Cursor(AssemblyResources.ToCursorsUri("size_ver.cur"), 0, 10);
        public static Cursor VSplit => s_vSplit ??= new Cursor(AssemblyResources.ToCursorsUri("size_hor.cur"), 10, 0);
        public static Cursor NoMove2D => s_noMove2D ??= new Cursor(AssemblyResources.ToCursorsUri("move.cur"), 10, 10);
        public static Cursor NoMoveHoriz => s_noMoveHoriz ??= new Cursor(AssemblyResources.ToCursorsUri("size_ver.cur"), 0, 10);
        public static Cursor NoMoveVert => s_noMoveVert ??= new Cursor(AssemblyResources.ToCursorsUri("size_hor.cur"), 10, 0);
        public static Cursor PanEast => s_panEast ??= new Cursor(AssemblyResources.ToCursorsUri("e-resize.cur"), 20, 0);
        public static Cursor PanNE => s_panNE ??= new Cursor(AssemblyResources.ToCursorsUri("ne-resize.cur"), 20, 0);
        public static Cursor PanNorth => s_panNorth ??= new Cursor(AssemblyResources.ToCursorsUri("n-resize.cur"), 10, 0);
        public static Cursor PanNW => s_panNW ??= new Cursor(AssemblyResources.ToCursorsUri("nw-resize.cur"), 0, 0);
        public static Cursor PanSE => s_panSE ??= new Cursor(AssemblyResources.ToCursorsUri("se-resize.cur"), 20, 20);
        public static Cursor PanSouth => s_panSouth ??= new Cursor(AssemblyResources.ToCursorsUri("s-resize.cur"), 0, 20);
        public static Cursor PanSW => s_panSW ??= new Cursor(AssemblyResources.ToCursorsUri("sw-resize.cur"), 0, 20);
        public static Cursor PanWest => s_panWest ??= new Cursor(AssemblyResources.ToCursorsUri("w-resize.cur"), 0, 0);
    }
}