using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public interface IControl_Sample
    {
        bool UseVisualStyleBackColor { get; set; }
        bool AutoSize { get; set; }
        string AccessibleDescription { get; set; }
        string AccessibleName { get; set; }
        AccessibleRole AccessibleRole { get; set; }
        bool AllowDrop { get; set; }

        AnchorStyles Anchor { get; set; }

        //Color BackColor { get; set; }
        Image BackgroundImage { get; set; }
        ImageLayout BackgroundImageLayout { get; set; }
        bool CausesValidation { get; set; }
        ContextMenuStrip ContextMenuStrip { get; set; }
        Cursor Cursor { get; set; }
        DockStyle Dock { get; set; }
        bool Enabled { get; set; }

        Font Font { get; set; }

        //Color ForeColor { get; set; }
        int Height { get; set; }

        ImeMode ImeMode { get; set; }

        //int Left { get; set; }
        //Point Location { get; set; }
        //Padding Margin { get; set; }
        Size MaximumSize { get; set; }

        Size MinimumSize { get; set; }

        // string Name { get; set; }
        Padding Padding { get; set; }

        //Control Parent { get; set; }
        RightToLeft RightToLeft { get; set; }

        //Size Size { get; set; }
        int TabIndex { get; set; }
        bool TabStop { get; set; }

        object Tag { get; set; }

        //string Text { get; set; }
        int Top { get; set; }

        bool UseWaitCursor { get; set; }

        //bool Visible { get; set; }
        int Width { get; set; }

        event EventHandler BackColorChanged;
        event EventHandler BackgroundImageChanged;
        event EventHandler BackgroundImageLayoutChanged;
        event EventHandler BindingContextChanged;
        event EventHandler CausesValidationChanged;
        event UICuesEventHandler ChangeUICues;
        event EventHandler Click;
        event EventHandler ClientSizeChanged;
        event EventHandler ContextMenuStripChanged;
        event EventHandler CursorChanged;
        event EventHandler DockChanged;
        event EventHandler DoubleClick;
        event EventHandler DpiChangedAfterParent;
        event EventHandler DpiChangedBeforeParent;
        event DragEventHandler DragDrop;
        event DragEventHandler DragEnter;
        event EventHandler DragLeave;
        event DragEventHandler DragOver;
        event EventHandler EnabledChanged;
        event EventHandler Enter;
        event EventHandler FontChanged;
        event EventHandler ForeColorChanged;
        event GiveFeedbackEventHandler GiveFeedback;
        event HelpEventHandler HelpRequested;
        event EventHandler ImeModeChanged;
        event KeyEventHandler KeyDown;
        event KeyPressEventHandler KeyPress;
        event KeyEventHandler KeyUp;
        event LayoutEventHandler Layout;
        event EventHandler Leave;
        event EventHandler LocationChanged;
        event EventHandler MarginChanged;
        event EventHandler MouseCaptureChanged;
        event MouseEventHandler MouseClick;
        event MouseEventHandler MouseDoubleClick;
        event MouseEventHandler MouseDown;
        event EventHandler MouseEnter;
        event EventHandler MouseHover;
        event EventHandler MouseLeave;
        event MouseEventHandler MouseMove;
        event MouseEventHandler MouseUp;
        event EventHandler Move;
        event EventHandler PaddingChanged;
        event PaintEventHandler Paint;
        event EventHandler ParentChanged;
        event PreviewKeyDownEventHandler PreviewKeyDown;
        event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp;
        event QueryContinueDragEventHandler QueryContinueDrag;
        event EventHandler RegionChanged;
        event EventHandler RightToLeftChanged;
        event EventHandler SizeChanged;
        event EventHandler StyleChanged;
        event EventHandler SystemColorsChanged;
        event EventHandler TabIndexChanged;
        event EventHandler TabStopChanged;
        event EventHandler TextChanged;
        event EventHandler Validated;
        event CancelEventHandler Validating;
        event EventHandler VisibleChanged;

        void BringToFront();
        bool Contains(Control ctl);
        void CreateControl();
        Graphics CreateGraphics();
        DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects);
        void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds);
        Form FindForm();
        Control GetChildAtPoint(Point pt);
        Control GetChildAtPoint(Point pt, GetChildAtPointSkip skipValue);
        IContainerControl GetContainerControl();
        Control GetNextControl(Control ctl, bool forward);
        void Hide();
        void Invalidate();
        void Invalidate(bool invalidateChildren);
        void Invalidate(Rectangle rc);
        void Invalidate(Rectangle rc, bool invalidateChildren);
        void Invalidate(Region region);
        void Invalidate(Region region, bool invalidateChildren);
        object Invoke(Delegate method);
        object Invoke(Delegate method, params object[] args);
        int LogicalToDeviceUnits(int value);
        Size LogicalToDeviceUnits(Size value);
        void PerformLayout();
        void PerformLayout(Control affectedControl, string affectedProperty);
        Point PointToClient(Point p);
        Point PointToScreen(Point p);
        bool PreProcessMessage(ref Message msg);
        Rectangle RectangleToClient(Rectangle r);
        Rectangle RectangleToScreen(Rectangle r);
        void Refresh();
        void ResetText();
        void ResumeLayout();
        void ResumeLayout(bool performLayout);
        void ScaleBitmapLogicalToDevice(ref Bitmap logicalBitmap);
        void Select();
        bool SelectNextControl(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap);
        void SendToBack();
        void SetBounds(int x, int y, int width, int height);
        void SetBounds(int x, int y, int width, int height, BoundsSpecified specified);
        void Show();
        void SuspendLayout();
        void Update();
    }
}