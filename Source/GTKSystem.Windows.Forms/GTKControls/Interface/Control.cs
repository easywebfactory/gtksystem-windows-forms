using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class Control: Component, IControl, ISynchronizeInvoke, IComponent, IDisposable, ISupportInitialize//,IEnumerable<Gtk.Widget>
    {
        public virtual T CreateControl<T>(params object[] args)
        {
            object widget = Activator.CreateInstance(typeof(T), args);
            GtkControl = widget;
            _widget = widget as Gtk.Widget;
            Container = widget as Gtk.Container;
            Dock = DockStyle.None;
            _widget.MarginStart = 0;
            _widget.MarginTop = 0;
            _widget.StyleContext.AddClass("DefaultThemeStyle");
            return (T)widget;
        }
        private Gtk.Widget _widget;
        public virtual Gtk.Widget Widget
        {
            get { return _widget; }
        }
        public new Gtk.Container Container { get; private set; }

        //public virtual Gtk.Widget Widget { get; }
        public virtual object GtkControl { get; private set; }
        public virtual AccessibleObject AccessibilityObject { get; }

        public virtual string AccessibleDefaultActionDescription { get; set; }
        public virtual string AccessibleDescription { get; set; }
        public virtual string AccessibleName { get; set; }
        public virtual AccessibleRole AccessibleRole { get; set; }
        public virtual bool AllowDrop { get; set; }
        public virtual AnchorStyles Anchor { get; set; }
        public virtual Point AutoScrollOffset { get; set; }
        public virtual bool AutoSize { get; set; }
        public virtual Color BackColor { get; set; }
        public virtual Drawing.Image BackgroundImage { get; set; }
        public virtual ImageLayout BackgroundImageLayout { get; set; }
        public virtual BindingContext BindingContext { get; set; }

        public virtual int Bottom { get; }

        public virtual Rectangle Bounds { get; set; }

        public virtual bool CanFocus { get; }

        public virtual bool CanSelect { get; }

        public virtual bool Capture { get; set; }
        public virtual bool CausesValidation { get; set; }

        public virtual Rectangle ClientRectangle { get; }

        public virtual Size ClientSize { get; set; }

        public virtual string CompanyName { get; }

        public virtual bool ContainsFocus { get; }

        public virtual ContextMenuStrip ContextMenuStrip { get; set; }

        public virtual ControlCollection Controls { get; }

        public virtual bool Created { get; }

        public virtual Cursor Cursor { get; set; }

        public virtual ControlBindingsCollection DataBindings { get; }

        public virtual int DeviceDpi { get; }

        public virtual Rectangle DisplayRectangle { get; }

        public virtual bool Disposing { get; }

        public virtual DockStyle Dock { get; set; }
        public virtual bool Enabled { get; set; }

        public virtual bool Focused { get; }

        public virtual Font Font { get; set; }
        public virtual Color ForeColor { get; set; }

        public virtual IntPtr Handle { get; }

        public virtual bool HasChildren { get; }

        public virtual int Height { get; set; }
        public virtual ImeMode ImeMode { get; set; }

        public virtual bool InvokeRequired { get; }

        public virtual bool IsAccessible { get; set; }

        public virtual bool IsDisposed { get; }

        public virtual bool IsHandleCreated { get; }

        public virtual bool IsMirrored { get; }

        public virtual LayoutEngine LayoutEngine { get; }

        public virtual int Left { get; set; }
        public virtual Point Location { get; set; }
        public virtual Padding Margin { get; set; }
        public virtual Size MaximumSize { get; set; }
        public virtual Size MinimumSize { get; set; }
        public virtual string Name { get; set; }
        public virtual Padding Padding { get; set; }
        public virtual Control Parent { get; set; }

        public virtual Size PreferredSize { get; }

        public virtual string ProductName { get; }

        public virtual string ProductVersion { get; }

        public virtual bool RecreatingHandle { get; }

        public virtual Region Region { get; set; }

        public virtual int Right { get; }

        public virtual RightToLeft RightToLeft { get; set; }
        //public virtual ISite Site { get; set; }
        public virtual Size Size { get; set; }
        public virtual int TabIndex { get; set; }
        public virtual bool TabStop { get; set; }
        public virtual object Tag { get; set; }
        public virtual string Text { get; set; }
        public virtual int Top { get; set; }

        public virtual Control TopLevelControl { get; }

        public virtual bool UseWaitCursor { get; set; }
        public virtual bool Visible { get; set; }
        public virtual int Width { get; set; }
        public virtual IWindowTarget WindowTarget { get; set; }

        public virtual bool UseVisualStyleBackColor { get; set; }

        public virtual event EventHandler AutoSizeChanged;
        public virtual event EventHandler BackColorChanged;
        public virtual event EventHandler BackgroundImageChanged;
        public virtual event EventHandler BackgroundImageLayoutChanged;
        public virtual event EventHandler BindingContextChanged;
        public virtual event EventHandler CausesValidationChanged;
        public virtual event UICuesEventHandler ChangeUICues;
        public virtual event EventHandler Click;
        public virtual event EventHandler ClientSizeChanged;
        public virtual event EventHandler ContextMenuStripChanged;
        public virtual event ControlEventHandler ControlAdded;
        public virtual event ControlEventHandler ControlRemoved;
        public virtual event EventHandler CursorChanged;
        public virtual event EventHandler DockChanged;
        public virtual event EventHandler DoubleClick;
        public virtual event EventHandler DpiChangedAfterParent;
        public virtual event EventHandler DpiChangedBeforeParent;
        public virtual event DragEventHandler DragDrop;
        public virtual event DragEventHandler DragEnter;
        public virtual event EventHandler DragLeave;
        public virtual event DragEventHandler DragOver;
        public virtual event EventHandler EnabledChanged;
        public virtual event EventHandler Enter;
        public virtual event EventHandler FontChanged;
        public virtual event EventHandler ForeColorChanged;
        public virtual event GiveFeedbackEventHandler GiveFeedback;
        public virtual event EventHandler GotFocus;
        public virtual event EventHandler HandleCreated;
        public virtual event EventHandler HandleDestroyed;
        public virtual event HelpEventHandler HelpRequested;
        public virtual event EventHandler ImeModeChanged;
        public virtual event InvalidateEventHandler Invalidated;
        public virtual event KeyEventHandler KeyDown;
        public virtual event KeyPressEventHandler KeyPress;
        public virtual event KeyEventHandler KeyUp;
        public virtual event LayoutEventHandler Layout;
        public virtual event EventHandler Leave;
        public virtual event EventHandler LocationChanged;
        public virtual event EventHandler LostFocus;
        public virtual event EventHandler MarginChanged;
        public virtual event EventHandler MouseCaptureChanged;
        public virtual event MouseEventHandler MouseClick;
        public virtual event MouseEventHandler MouseDoubleClick;
        public virtual event MouseEventHandler MouseDown;
        public virtual event EventHandler MouseEnter;
        public virtual event EventHandler MouseHover;
        public virtual event EventHandler MouseLeave;
        public virtual event MouseEventHandler MouseMove;
        public virtual event MouseEventHandler MouseUp;
        public virtual event MouseEventHandler MouseWheel;
        public virtual event EventHandler Move;
        public virtual event EventHandler PaddingChanged;
        public virtual event PaintEventHandler Paint;
        public virtual event EventHandler ParentChanged;
        public virtual event PreviewKeyDownEventHandler PreviewKeyDown;
        public virtual event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp;
        public virtual event QueryContinueDragEventHandler QueryContinueDrag;
        public virtual event EventHandler RegionChanged;
        public virtual event EventHandler Resize;
        public virtual event EventHandler RightToLeftChanged;
        public virtual event EventHandler SizeChanged;
        public virtual event EventHandler StyleChanged;
        public virtual event EventHandler SystemColorsChanged;
        public virtual event EventHandler TabIndexChanged;
        public virtual event EventHandler TabStopChanged;
        public virtual event EventHandler TextChanged;
        public virtual event EventHandler Validated;
        public virtual event CancelEventHandler Validating;
        public virtual event EventHandler VisibleChanged;


        public virtual IAsyncResult BeginInvoke(Delegate method)
        {
            return BeginInvoke(method, null);
        }
        public virtual IAsyncResult BeginInvoke(Delegate method, params object[] args)
        {
            return new AsyncResult();
        }

        public virtual void BringToFront()
        {

        }

        public virtual bool Contains(Control ctl)
        {
            return false;
        }

        public virtual void CreateControl()
        {

        }

        public virtual Graphics CreateGraphics()
        {
            return null;
        }

        public virtual DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            return DragDropEffects.None;
        }

        public virtual void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds)
        {

        }

        public virtual object EndInvoke(IAsyncResult asyncResult)
        {
            return asyncResult.AsyncState;
        }

        public virtual Form FindForm()
        {
            return null;
        }

        public virtual bool Focus()
        {
            return false;
        }

        public virtual Control GetChildAtPoint(Point pt)
        {
            return null;
        }

        public virtual Control GetChildAtPoint(Point pt, GetChildAtPointSkip skipValue)
        {
            return null;
        }

        public virtual IContainerControl GetContainerControl()
        {
            return null;
        }

        public virtual Control GetNextControl(Control ctl, bool forward)
        {
            return ctl;
        }

        public virtual Size GetPreferredSize(Size proposedSize)
        {
            return proposedSize;
        }

        public virtual void Hide()
        {

        }

        public virtual void Invalidate()
        {

        }

        public virtual void Invalidate(bool invalidateChildren)
        {

        }

        public virtual void Invalidate(Rectangle rc)
        {

        }

        public virtual void Invalidate(Rectangle rc, bool invalidateChildren)
        {

        }

        public virtual void Invalidate(Region region)
        {

        }

        public virtual void Invalidate(Region region, bool invalidateChildren)
        {

        }

        public virtual object Invoke(Delegate method)
        {
            return null;
        }

        public virtual object Invoke(Delegate method, params object[] args)
        {
            return null;
        }

        public virtual int LogicalToDeviceUnits(int value)
        {
            return value;
        }

        public virtual Size LogicalToDeviceUnits(Size value)
        {
            return value;
        }

        public virtual void PerformLayout()
        {

        }

        public virtual void PerformLayout(Control affectedControl, string affectedProperty)
        {

        }

        public virtual Point PointToClient(Point p)
        {
            return p;
        }

        public virtual Point PointToScreen(Point p)
        {
            return p;
        }

        public virtual PreProcessControlState PreProcessControlMessage(ref Message msg)
        {
            return PreProcessControlState.MessageNotNeeded;
        }

        public virtual bool PreProcessMessage(ref Message msg)
        {
            return false;
        }

        public virtual Rectangle RectangleToClient(Rectangle r)
        {
            return r;
        }

        public virtual Rectangle RectangleToScreen(Rectangle r)
        {
            return r;
        }

        public virtual void Refresh()
        {

        }

        public virtual void ResetBackColor()
        {

        }

        public virtual void ResetBindings()
        {

        }

        public virtual void ResetCursor()
        {

        }

        public virtual void ResetFont()
        {

        }

        public virtual void ResetForeColor()
        {

        }

        public virtual void ResetImeMode()
        {

        }

        public virtual void ResetRightToLeft()
        {

        }

        public virtual void ResetText()
        {

        }

        public virtual void ResumeLayout()
        {

        }

        public virtual void ResumeLayout(bool performLayout)
        {

        }

        public virtual void Scale(float ratio)
        {

        }

        public virtual void Scale(float dx, float dy)
        {

        }

        public virtual void Scale(SizeF factor)
        {

        }

        public virtual void ScaleBitmapLogicalToDevice(ref Bitmap logicalBitmap)
        {

        }

        public virtual void Select()
        {

        }

        public virtual bool SelectNextControl(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
        {
            return false;
        }

        public virtual void SendToBack()
        {

        }

        public virtual void SetBounds(int x, int y, int width, int height)
        {

        }

        public virtual void SetBounds(int x, int y, int width, int height, BoundsSpecified specified)
        {

        }

        public virtual void Show()
        {

        }

        public virtual void SuspendLayout()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void BeginInit()
        {
             
        }

        public virtual void EndInit()
        {
             
        }


    }
}
