using Gtk;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class ScrollableControl : Control, IArrangedElement, IComponent, IDisposable
    {

        protected const int ScrollStateAutoScrolling = 1;

        protected const int ScrollStateHScrollVisible = 2;

        protected const int ScrollStateVScrollVisible = 4;

        protected const int ScrollStateUserHasScrolled = 8;

        protected const int ScrollStateFullDrag = 16;

        public override Rectangle DisplayRectangle { get; }

        [Localizable(true)]
        public Size AutoScrollMinSize { get; set; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point AutoScrollPosition { get; set; }
        [Localizable(true)]
        public Size AutoScrollMargin { get; set; }
        [DefaultValue(false)]
        [Localizable(true)]
        public virtual bool AutoScroll { get; set; }
        //[Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Always)]
        //public VScrollProperties VerticalScroll { get; }
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public DockPaddingEdges DockPadding { get; }
        //[Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Always)]
        //public HScrollProperties HorizontalScroll { get; }
        protected bool VScroll { get; set; }
        protected bool HScroll { get; set; }

        //protected override CreateParams CreateParams { get; }

        public event ScrollEventHandler Scroll;
    }
}
