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

        public static Cursor AppStarting => s_appStarting ??= new Cursor(Gdk.CursorType.Circle);
        public static Cursor Arrow => s_arrow ??= new Cursor(Gdk.CursorType.Arrow);
        public static Cursor Cross => s_cross ??= new Cursor(Gdk.CursorType.Cross);
        public static Cursor Default => s_defaultCursor ??= new Cursor(Gdk.CursorType.Arrow);
        public static Cursor IBeam => s_iBeam ??= new Cursor(Gdk.CursorType.Xterm);

        public static Cursor No =>
            s_no ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.aero_unavail.cur", 0, 0);

        public static Cursor SizeAll => s_sizeAll ??= new Cursor(Gdk.CursorType.Fleur);

        public static Cursor SizeNESW =>
            s_sizeNESW ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.aero_nesw.cur", 15, 15);

        public static Cursor SizeNS =>
            s_sizeNS ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.aero_ns.cur", 15, 15);

        public static Cursor SizeNWSE =>
            s_sizeNWSE ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.aero_nwse.cur", 15, 15);

        public static Cursor SizeWE =>
            s_sizeWE ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.aero_ew.cur", 15, 15);

        public static Cursor UpArrow => s_upArrow ??= new Cursor(Gdk.CursorType.SbUpArrow);
        public static Cursor WaitCursor => s_wait ??= new Cursor(Gdk.CursorType.Watch);
        public static Cursor Help => s_help ??= new Cursor(Gdk.CursorType.QuestionArrow);
        public static Cursor Hand => s_hand ??= new Cursor(Gdk.CursorType.Hand1);

        public static Cursor HSplit =>
            s_hSplit ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.hsplit.cur", 25, 25);

        public static Cursor VSplit =>
            s_vSplit ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.vsplit.cur", 25, 25);

        public static Cursor NoMove2D =>
            s_noMove2D ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.nomove2d.cur", 25, 25);

        public static Cursor NoMoveHoriz =>
            s_noMoveHoriz ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.nomoveh.cur", 25, 25);

        public static Cursor NoMoveVert =>
            s_noMoveVert ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.nomovev.cur", 25, 25);

        public static Cursor PanEast =>
            s_panEast ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.east.cur", 25, 25);

        public static Cursor PanNE =>
            s_panNE ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.ne.cur", 25, 25);

        public static Cursor PanNorth =>
            s_panNorth ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.north.cur", 25, 25);

        public static Cursor PanNW =>
            s_panNW ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.nw.cur", 25, 25);

        public static Cursor PanSE =>
            s_panSE ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.se.cur", 25, 25);

        public static Cursor PanSouth =>
            s_panSouth ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.south.cur", 25, 25);

        public static Cursor PanSW =>
            s_panSW ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.sw.cur", 25, 25);

        public static Cursor PanWest =>
            s_panWest ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.west.cur", 25, 25);
    }
}