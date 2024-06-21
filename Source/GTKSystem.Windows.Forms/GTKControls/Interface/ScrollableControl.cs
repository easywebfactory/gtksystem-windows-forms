using GLib;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [Designer("System.Windows.Forms.Design.ScrollableControlDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class ScrollableControl : Control, IArrangedElement, IComponent, IDisposable
    {

        protected const int ScrollStateAutoScrolling = 1;

        protected const int ScrollStateHScrollVisible = 2;

        protected const int ScrollStateVScrollVisible = 4;

        protected const int ScrollStateUserHasScrolled = 8;

        protected const int ScrollStateFullDrag = 16;

        private IScrollableBoxBase scrollbase;
        public ScrollableControl():base()
        {
            scrollbase = GtkControl as IScrollableBoxBase;
        }
        public void SetScrolledWindow(IScrollableBoxBase scrolledwindow)
        {
            scrollbase = scrolledwindow;
        }
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
        //public virtual bool AutoScroll { get; set; } = false;
        private bool _AutoScroll;
        public virtual bool AutoScroll
        {
            get => _AutoScroll;
            set
            {
                _AutoScroll = value;
                if(scrollbase != null) { scrollbase.AutoScroll = value; }
            }
        }
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
        protected bool VScroll { get => scrollbase.VScroll; set => scrollbase.VScroll = value; }
        protected bool HScroll { get => scrollbase.HScroll; set => scrollbase.HScroll = value; }

        public virtual event ScrollEventHandler Scroll
        {
            add { scrollbase.Scroll += value; }
            remove { scrollbase.Scroll += value; }
        }
    }
}
