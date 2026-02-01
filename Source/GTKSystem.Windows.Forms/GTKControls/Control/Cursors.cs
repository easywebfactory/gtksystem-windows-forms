
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

        public static Cursor AppStarting => s_appStarting ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.circle.cur", 0, 0);
        public static Cursor Arrow => s_arrow ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.arrow.cur", 0, 0);
        public static Cursor Cross => s_cross ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.cross.cur", 10, 10);
        public static Cursor Default => s_defaultCursor ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.arrow.cur", 0, 0);
        public static Cursor IBeam => s_iBeam ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.xterm.cur", 0, 0);
        public static Cursor No => s_no ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.not-allowed.cur", 0, 0);
        public static Cursor SizeAll => s_sizeAll ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.size_all.cur", 10, 10);
        public static Cursor SizeNESW => s_sizeNESW ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.nesw-resize.cur", 10, 10);
        public static Cursor SizeNS => s_sizeNS ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.ns-resize.cur", 0, 10);
        public static Cursor SizeNWSE => s_sizeNWSE ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.nwse-resize.cur", 10, 10);
        public static Cursor SizeWE => s_sizeWE ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.ew-resize.cur", 10, 0);
        public static Cursor UpArrow => s_upArrow ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.sb_up_arrow.cur", 0, 0);
        public static Cursor WaitCursor => s_wait ??= new Cursor(Gdk.CursorType.Watch);
        public static Cursor Help => s_help ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.help.cur", 0, 0);
        public static Cursor Hand => s_hand ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.hand.cur", 0, 0);
        public static Cursor HSplit => s_hSplit ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.size_ver.cur", 0, 10);
        public static Cursor VSplit => s_vSplit ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.size_hor.cur", 10, 0);
        public static Cursor NoMove2D => s_noMove2D ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.move.cur", 10, 10);
        public static Cursor NoMoveHoriz => s_noMoveHoriz ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.size_ver.cur", 0, 10);
        public static Cursor NoMoveVert => s_noMoveVert ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.size_hor.cur", 10, 0);
        public static Cursor PanEast => s_panEast ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.e-resize.cur", 20, 0);
        public static Cursor PanNE => s_panNE ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.ne-resize.cur", 20, 0);
        public static Cursor PanNorth => s_panNorth ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.n-resize.cur", 10, 0);
        public static Cursor PanNW => s_panNW ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.nw-resize.cur", 0, 0);
        public static Cursor PanSE => s_panSE ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.se-resize.cur", 20, 20);
        public static Cursor PanSouth => s_panSouth ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.s-resize.cur", 0, 20);
        public static Cursor PanSW => s_panSW ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.sw-resize.cur", 0, 20);
        public static Cursor PanWest => s_panWest ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.w-resize.cur", 0, 0);
    }
}