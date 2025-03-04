
namespace System.Windows.Forms;

/// <summary>
///  Standard cursors
/// </summary>
public static class Cursors
{
    private static Cursor? appStarting;
    private static Cursor? arrow;
    private static Cursor? cross;
    private static Cursor? defaultCursor;
    private static Cursor? iBeam;
    private static Cursor? no;
    private static Cursor? sizeAll;
    private static Cursor? sizeNesw;
    private static Cursor? sizeNs;
    private static Cursor? sizeNwse;
    private static Cursor? sizeWe;
    private static Cursor? upArrow;
    private static Cursor? wait;
    private static Cursor? help;
    private static Cursor? hSplit;
    private static Cursor? vSplit;
    private static Cursor? noMove2D;
    private static Cursor? noMoveHoriz;
    private static Cursor? noMoveVert;
    private static Cursor? panEast;
    private static Cursor? panNe;
    private static Cursor? panNorth;
    private static Cursor? panNw;
    private static Cursor? panSe;
    private static Cursor? panSouth;
    private static Cursor? panSw;
    private static Cursor? panWest;
    private static Cursor? hand;

    public static Cursor AppStarting => appStarting ??= new Cursor(Gdk.CursorType.Circle);
    public static Cursor Arrow => arrow ??= new Cursor(Gdk.CursorType.Arrow);
    public static Cursor Cross => cross ??= new Cursor(Gdk.CursorType.Cross);
    public static Cursor Default => defaultCursor ??= new Cursor(Gdk.CursorType.Arrow);
    public static Cursor Beam => iBeam ??= new Cursor(Gdk.CursorType.Xterm);
    public static Cursor No => no ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.aero_unavail.cur", 0, 0);
    public static Cursor SizeAll => sizeAll ??= new Cursor(Gdk.CursorType.Fleur);
    public static Cursor SizeNesw => sizeNesw ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.aero_nesw.cur", 15, 15);
    public static Cursor SizeNs => sizeNs ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.aero_ns.cur", 15, 15);
    public static Cursor SizeNwse => sizeNwse ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.aero_nwse.cur", 15, 15);
    public static Cursor SizeWe => sizeWe ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.aero_ew.cur", 15, 15);
    public static Cursor UpArrow => upArrow ??= new Cursor(Gdk.CursorType.SbUpArrow);
    public static Cursor WaitCursor => wait ??= new Cursor(Gdk.CursorType.Watch);
    public static Cursor Help => help ??= new Cursor(Gdk.CursorType.QuestionArrow);
    public static Cursor Hand => hand ??= new Cursor(Gdk.CursorType.Hand1);
    public static Cursor HSplit => hSplit ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.hsplit.cur", 25, 25);
    public static Cursor VSplit => vSplit ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.vsplit.cur", 25, 25);
    public static Cursor NoMove2D => noMove2D ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.nomove2d.cur", 25, 25);
    public static Cursor NoMoveHoriz => noMoveHoriz ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.nomoveh.cur", 25, 25);
    public static Cursor NoMoveVert => noMoveVert ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.nomovev.cur", 25, 25);
    public static Cursor PanEast => panEast ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.east.cur", 25, 25);
    public static Cursor PanNe => panNe ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.ne.cur", 25, 25);
    public static Cursor PanNorth => panNorth ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.north.cur", 25, 25);
    public static Cursor PanNw => panNw ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.nw.cur", 25, 25);
    public static Cursor PanSe => panSe ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.se.cur", 25, 25);
    public static Cursor PanSouth => panSouth ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.south.cur", 25, 25);
    public static Cursor PanSw => panSw ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.sw.cur", 25, 25);
    public static Cursor PanWest => panWest ??= new Cursor("GTKSystem.Windows.Forms.Resources.Cursors.west.cur", 25, 25);
}