using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms.Layout;

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

        protected virtual IScrollableBoxBase scrollbase { get; set; }
        public ScrollableControl():base()
        {
            scrollbase = GtkControl as IScrollableBoxBase;
        }
        public void SetScrolledWindow(IScrollableBoxBase scrolledwindow)
        {
            scrollbase = scrolledwindow;
        }
        public override Rectangle DisplayRectangle { get; }
        public Size AutoScrollMinSize { get; set; }
        public Point AutoScrollPosition { get; set; }
        public Size AutoScrollMargin { get; set; }
        private bool _AutoScroll = true;
        public virtual bool AutoScroll
        {
            get => _AutoScroll;
            set
            {
                _AutoScroll = value;
                if (AutoSize == false && scrollbase != null)
                {
                    scrollbase.AutoScroll = value;
                }
                if (this.Widget is Gtk.ScrolledWindow sw)
                {
                    Gtk.Widget widget = sw.Child;
                    if (sw.Child is Gtk.Viewport view)
                    {
                        if (view.Child is Gtk.Overlay lay)
                        {
                            SetScrollLayout(lay);
                        }
                    }
                    else if (sw.Child is Gtk.Overlay lay)
                    {
                        SetScrollLayout(lay);
                    }
                }
            }
        }
        private void SetScrollLayout(Gtk.Overlay lay)
        {
            if (_AutoScroll == false)
                lay.SetSizeRequest(-1, -1);
            else
            {
                lay.SetSizeRequest(1, 1);
                foreach (Gtk.Widget widget in lay.Children)
                {
                    lay.WidthRequest = Math.Max(lay.WidthRequest, widget.MarginStart + widget.WidthRequest);
                    lay.HeightRequest = Math.Max(lay.HeightRequest, widget.MarginTop + widget.HeightRequest);
                }
            }
        }
        public override bool AutoSize
        {
            get => base.AutoSize;
            set
            {
                base.AutoSize = value;
                if (value == true)
                {
                    if (scrollbase != null) { scrollbase.AutoScroll = false; }
                }
                else
                {
                    if (scrollbase != null) { scrollbase.AutoScroll = _AutoScroll; }
                }
            }
        }
        //public VScrollProperties VerticalScroll { get; }
        //public DockPaddingEdges DockPadding { get; }
        //public HScrollProperties HorizontalScroll { get; }
        protected bool VScroll { get => scrollbase.VScroll; set => scrollbase.VScroll = value; }
        protected bool HScroll { get => scrollbase.HScroll; set => scrollbase.HScroll = value; }

        public virtual event ScrollEventHandler Scroll
        {
            add { if (scrollbase != null) { scrollbase.Scroll += value; } }
            remove { if (scrollbase != null) { scrollbase.Scroll -= value; } }
        }
        public void ScrollControlIntoView(System.Windows.Forms.Control activeControl)
        {
            if (activeControl != null && activeControl.Widget != null && scrollbase != null && AutoScroll == true)
            {
                if (this.Widget != null)
                {
                    activeControl.Widget.Window.GetOrigin(out int sx, out int sy);
                    this.Widget.Window.GetOrigin(out int px, out int py);
                    int hvalue = HScroll ? sx - px : -1;
                    int vvalue = VScroll ? sy - py : -1;
                    if (this.Widget is Gtk.Window win)
                    {
                        vvalue -= 20;
                        if (win.Titlebar != null)
                            vvalue -= win.Titlebar.AllocatedHeight;
                    }
                    Gtk.ScrolledWindow scrolled = scrollbase.ScrolledWindow;
                    scrollbase.ScrollView(hvalue + scrolled.Hadjustment.Value, vvalue + scrolled.Vadjustment.Value);

                }
            }
        }
    }
}
