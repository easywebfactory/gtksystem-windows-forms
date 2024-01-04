using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace System.Windows.Forms
{
    public class Control_DotNet : IControl
    {
        public Gtk.Widget Widget { get; }
        public AccessibleObject AccessibilityObject { get; }

        public string AccessibleDefaultActionDescription { get; set; }
        public string AccessibleDescription { get; set; }
        public string AccessibleName { get; set; }
        public AccessibleRole AccessibleRole { get; set; }
        public bool AllowDrop { get; set; }
        public AnchorStyles Anchor { get; set; }
        public Point AutoScrollOffset { get; set; }
        public bool AutoSize { get; set; }
        public Color BackColor { get; set; }
        public Image BackgroundImage { get; set; }
        public ImageLayout BackgroundImageLayout { get; set; }
        public BindingContext BindingContext { get; set; }

        public int Bottom { get; }

        public Rectangle Bounds { get; set; }

        public bool CanFocus { get; }

        public bool CanSelect { get; }

        public bool Capture { get; set; }
        public bool CausesValidation { get; set; }

        public Rectangle ClientRectangle { get; }

        public Size ClientSize { get; set; }

        public string CompanyName { get; }

        public bool ContainsFocus { get; }

        public ContextMenuStrip ContextMenuStrip { get; set; }

        public ControlCollection Controls { get; }

        public bool Created { get; }

        public Cursor Cursor { get; set; }

        public ControlBindingsCollection DataBindings { get; }

        public int DeviceDpi { get; }

        public Rectangle DisplayRectangle { get; }

        public bool Disposing { get; }

        public DockStyle Dock { get; set; }
        public bool Enabled { get; set; }

        public bool Focused { get; }

        public Font Font { get; set; }
        public Color ForeColor { get; set; }

        public IntPtr Handle { get; }

        public bool HasChildren { get; }

        public int Height { get; set; }
        public ImeMode ImeMode { get; set; }

        public bool InvokeRequired { get; }

        public bool IsAccessible { get; set; }

        public bool IsDisposed { get; }

        public bool IsHandleCreated { get; }

        public bool IsMirrored { get; }

        public LayoutEngine LayoutEngine { get; }

        public int Left { get; set; }
        public Point Location { get; set; }
        public Padding Margin { get; set; }
        public Size MaximumSize { get; set; }
        public Size MinimumSize { get; set; }
        public string Name { get; set; }
        public Padding Padding { get; set; }
        public Control Parent { get; set; }

        public Size PreferredSize { get; }

        public string ProductName { get; }

        public string ProductVersion { get; }

        public bool RecreatingHandle { get; }

        public Region Region { get; set; }

        public int Right { get; }

        public RightToLeft RightToLeft { get; set; }
        public ISite Site { get; set; }
        public Size Size { get; set; }
        public int TabIndex { get; set; }
        public bool TabStop { get; set; }
        public object Tag { get; set; }
        public string Text { get; set; }
        public int Top { get; set; }

        public Control TopLevelControl { get; }

        public bool UseWaitCursor { get; set; }
        public bool Visible { get; set; }
        public int Width { get; set; }
        public IWindowTarget WindowTarget { get; set; }

        ControlCollection IControl.Controls => throw new NotImplementedException();

        bool IControl.UseVisualStyleBackColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler AutoSizeChanged;
        public event EventHandler BackColorChanged;
        public event EventHandler BackgroundImageChanged;
        public event EventHandler BackgroundImageLayoutChanged;
        public event EventHandler BindingContextChanged;
        public event EventHandler CausesValidationChanged;
        public event UICuesEventHandler ChangeUICues;
        public event EventHandler Click;
        public event EventHandler ClientSizeChanged;
        public event EventHandler ContextMenuStripChanged;
        public event ControlEventHandler ControlAdded;
        public event ControlEventHandler ControlRemoved;
        public event EventHandler CursorChanged;
        public event EventHandler DockChanged;
        public event EventHandler DoubleClick;
        public event EventHandler DpiChangedAfterParent;
        public event EventHandler DpiChangedBeforeParent;
        public event DragEventHandler DragDrop;
        public event DragEventHandler DragEnter;
        public event EventHandler DragLeave;
        public event DragEventHandler DragOver;
        public event EventHandler EnabledChanged;
        public event EventHandler Enter;
        public event EventHandler FontChanged;
        public event EventHandler ForeColorChanged;
        public event GiveFeedbackEventHandler GiveFeedback;
        public event EventHandler GotFocus;
        public event EventHandler HandleCreated;
        public event EventHandler HandleDestroyed;
        public event HelpEventHandler HelpRequested;
        public event EventHandler ImeModeChanged;
        public event InvalidateEventHandler Invalidated;
        public event KeyEventHandler KeyDown;
        public event KeyPressEventHandler KeyPress;
        public event KeyEventHandler KeyUp;
        public event LayoutEventHandler Layout;
        public event EventHandler Leave;
        public event EventHandler LocationChanged;
        public event EventHandler LostFocus;
        public event EventHandler MarginChanged;
        public event EventHandler MouseCaptureChanged;
        public event MouseEventHandler MouseClick;
        public event MouseEventHandler MouseDoubleClick;
        public event MouseEventHandler MouseDown;
        public event EventHandler MouseEnter;
        public event EventHandler MouseHover;
        public event EventHandler MouseLeave;
        public event MouseEventHandler MouseMove;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseWheel;
        public event EventHandler Move;
        public event EventHandler PaddingChanged;
        public event PaintEventHandler Paint;
        public event EventHandler ParentChanged;
        public event PreviewKeyDownEventHandler PreviewKeyDown;
        public event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp;
        public event QueryContinueDragEventHandler QueryContinueDrag;
        public event EventHandler RegionChanged;
        public event EventHandler Resize;
        public event EventHandler RightToLeftChanged;
        public event EventHandler SizeChanged;
        public event EventHandler StyleChanged;
        public event EventHandler SystemColorsChanged;
        public event EventHandler TabIndexChanged;
        public event EventHandler TabStopChanged;
        public event EventHandler TextChanged;
        public event EventHandler Validated;
        public event CancelEventHandler Validating;
        public event EventHandler VisibleChanged;

        public IAsyncResult BeginInvoke(Delegate method)
        {
            return BeginInvoke(method, null);
        }

        public IAsyncResult BeginInvoke(Delegate method, params object[] args)
        { 
            return new AsyncResult();
        }

        public void BringToFront()
        {
            
        }

        public bool Contains(Control ctl)
        {
            return false;
        }

        public void CreateControl()
        {
            
        }

        public Graphics CreateGraphics()
        {
            return null;
        }

        public DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            return DragDropEffects.None;
        }

        public void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds)
        {
            
        }

        public object EndInvoke(IAsyncResult asyncResult)
        {
            return asyncResult.AsyncState;
        }

        public Form FindForm()
        {
            return null;
        }

        public bool Focus()
        {
            return false;
        }

        public Control GetChildAtPoint(Point pt)
        {
            return null;
        }

        public Control GetChildAtPoint(Point pt, GetChildAtPointSkip skipValue)
        {
            return null;
        }

        public IContainerControl GetContainerControl()
        {
            return null; 
        }

        public Control GetNextControl(Control ctl, bool forward)
        {
            return ctl;
        }

        public Size GetPreferredSize(Size proposedSize)
        {
            return proposedSize;
        }

        public void Hide()
        {
            
        }

        public void Invalidate()
        {
            
        }

        public void Invalidate(bool invalidateChildren)
        {
            
        }

        public void Invalidate(Rectangle rc)
        {
            
        }

        public void Invalidate(Rectangle rc, bool invalidateChildren)
        {
            
        }

        public void Invalidate(Region region)
        {
            
        }

        public void Invalidate(Region region, bool invalidateChildren)
        {
            
        }

        public object Invoke(Delegate method)
        {
            return null;
        }

        public object Invoke(Delegate method, params object[] args)
        {
            return null;
        }

        public int LogicalToDeviceUnits(int value)
        {
            return value;
        }

        public Size LogicalToDeviceUnits(Size value)
        {
            return value;
        }

        public void PerformLayout()
        {
            
        }

        public void PerformLayout(Control affectedControl, string affectedProperty)
        {
            
        }

        public Point PointToClient(Point p)
        {
            return p;
        }

        public Point PointToScreen(Point p)
        {
            return p;
        }

        public PreProcessControlState PreProcessControlMessage(ref Message msg)
        {
            return PreProcessControlState.MessageNotNeeded;
        }

        public bool PreProcessMessage(ref Message msg)
        {
            return false;
        }

        public Rectangle RectangleToClient(Rectangle r)
        {
            return r;
        }

        public Rectangle RectangleToScreen(Rectangle r)
        {
            return r;
        }

        public void Refresh()
        {
            
        }

        public void ResetBackColor()
        {
            
        }

        public void ResetBindings()
        {
            
        }

        public void ResetCursor()
        {
            
        }

        public void ResetFont()
        {
            
        }

        public void ResetForeColor()
        {
            
        }

        public void ResetImeMode()
        {
            
        }

        public void ResetRightToLeft()
        {
            
        }

        public void ResetText()
        {
            
        }

        public void ResumeLayout()
        {
            
        }

        public void ResumeLayout(bool performLayout)
        {
            
        }

        public void Scale(float ratio)
        {
            
        }

        public void Scale(float dx, float dy)
        {
            
        }

        public void Scale(SizeF factor)
        {
            
        }

        public void ScaleBitmapLogicalToDevice(ref Bitmap logicalBitmap)
        {
            
        }

        public void Select()
        {
            
        }

        public bool SelectNextControl(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
        {
            return false;
        }

        public void SendToBack()
        {
            
        }

        public void SetBounds(int x, int y, int width, int height)
        {
            
        }

        public void SetBounds(int x, int y, int width, int height, BoundsSpecified specified)
        {
            
        }

        public void Show()
        {
            
        }

        public void SuspendLayout()
        {
            
        }

        public void Update()
        {
            
        }
    }
}
