using GLib;
using Gtk;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;
using System.Windows.Forms.Design;
using static System.Windows.Forms.Button;

namespace System.Windows.Forms
{
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    [Designer(typeof(ControlDesigner))]
    //[Designer("System.Windows.Forms.Design.ControlDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    //[DesignerSerializer("System.Windows.Forms.Design.ControlCodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [ToolboxItemFilter("System.Windows.Forms")]
    public partial class Control: Component, IControl, ISynchronizeInvoke, IComponent, IDisposable, ISupportInitialize
    {
        private Gtk.Application app = Application.Init();
        public virtual string unique_key { get; protected set; }
        //public virtual T CreateControl<T>(params object[] args)
        //{
        //    object widget = Activator.CreateInstance(typeof(T), args);
        //    GtkControl = widget;
        //    this.Widget = widget as Gtk.Widget;
        //    GtkContainer = widget as Gtk.Container;
        //    Dock = DockStyle.None;
        //    this.Widget.MarginStart = 0;
        //    this.Widget.MarginTop = 0;
        //    this.Widget.StyleContext.AddClass("DefaultThemeStyle");
        //    return (T)widget;
        //}
        public virtual Gtk.Widget Widget { get; private set; }
        public virtual Gtk.Container GtkContainer { get => Widget as Gtk.Container; }
        public virtual object GtkControl { get => Widget; }
        public Control()
        {
            this.unique_key = Guid.NewGuid().ToString();
            
            if (this.Widget != null)
            {
                this.Dock = DockStyle.None;
                this.Widget.StyleContext.AddClass("DefaultThemeStyle");
            }
        }
        //===================

        protected virtual void UpdateStyle()
        {
            SetStyle(this.Widget);
        }
        protected virtual void SetStyle(Gtk.Widget widget)
        {
            string stylename = $"s{unique_key}";
            StringBuilder style = new StringBuilder();
            if (this.BackColor.Name != "Control" && this.BackColor.Name != "0")
            {
                string color = $"rgba({this.BackColor.R},{this.BackColor.G},{this.BackColor.B},{this.BackColor.A})";
                // style.AppendFormat("background-color:{0};background:{0};", color);
            }
            if (this.ForeColor.Name != "Control" && this.ForeColor.Name != "0")
            {
                string color = $"rgba({this.ForeColor.R},{this.ForeColor.G},{this.ForeColor.B},{this.ForeColor.A})";
                style.AppendFormat("color:{0};", color);
            }
            if (this.Font != null)
            {
                Pango.AttrList attributes = new Pango.AttrList();
                float textSize = this.Font.Size;
                if (this.Font.Unit == GraphicsUnit.Point)
                    textSize = this.Font.Size / 72 * 96;
                else if (this.Font.Unit == GraphicsUnit.Inch)
                    textSize = this.Font.Size * 96;

                style.AppendFormat("font-size:{0}px;", (int)textSize);
                if (string.IsNullOrWhiteSpace(Font.FontFamily.Name) == false)
                {
                    style.AppendFormat("font-family:\"{0}\";", Font.FontFamily.Name);
                    attributes.Insert(new Pango.AttrFontDesc(new Pango.FontDescription() { Family = Font.FontFamily.Name, Size = (int)(textSize * Pango.Scale.PangoScale * 0.7) }));
                }

                string[] fontstyle = Font.Style.ToString().ToLower().Split(new char[] { ',', ' ' });
                foreach (string sty in fontstyle)
                {
                    if (sty == "bold")
                    {
                        style.Append("font-weight:bold;");
                        attributes.Insert(new Pango.AttrWeight(Pango.Weight.Bold));
                    }
                    else if (sty == "italic")
                    {
                        style.Append("font-style:italic;");
                        attributes.Insert(new Pango.AttrStyle(Pango.Style.Italic));
                    }
                    else if (sty == "underline")
                    {
                        style.Append("text-decoration:underline;");
                        attributes.Insert(new Pango.AttrUnderline(Pango.Underline.Low));
                    }
                    else if (sty == "strikeout")
                    {
                        style.Append("text-decoration:line-through;");
                        attributes.Insert(new Pango.AttrStrikethrough(true));
                    }
                }
                if (widget is Gtk.Label gtklabel)
                {
                    gtklabel.Attributes = attributes;
                }
            }

            StringBuilder css = new StringBuilder();
            css.AppendLine($".{stylename}{{{style.ToString()}}}");
            if (widget is Gtk.TextView)
            {
                css.AppendLine($".{stylename} text{{{style.ToString()}}}");
                css.AppendLine($".{stylename} .view{{{style.ToString()}}}");
            }
            CssProvider provider = new CssProvider();
            if (provider.LoadFromData(css.ToString()))
            {
                widget.StyleContext.AddProvider(provider, 900);
                widget.StyleContext.RemoveClass(stylename);
                widget.StyleContext.AddClass(stylename);
            }
        }

        public virtual AccessibleObject AccessibilityObject { get; }

        public virtual string AccessibleDefaultActionDescription { get; set; }
        public virtual string AccessibleDescription { get; set; }
        public virtual string AccessibleName { get; set; }
        public virtual AccessibleRole AccessibleRole { get; set; }
        public virtual bool AllowDrop { get; set; }
        public virtual AnchorStyles Anchor { get; set; }
        public virtual Point AutoScrollOffset { get; set; }
        public virtual bool AutoSize { get; set; }
        private Color _BackColor;
        public virtual Color BackColor { get => _BackColor; set { _BackColor = value; UpdateStyle(); } }

        private byte[] _BackgroundImageBytes;
        private System.Drawing.Image backgroundImage;
        public virtual System.Drawing.Image BackgroundImage
        {
            get => backgroundImage;
            set
            {
                backgroundImage = value;
                if (value != null)
                {
                    _BackgroundImageBytes = new byte[value.PixbufData.Length];
                    value.PixbufData.CopyTo(_BackgroundImageBytes, 0);
                }
            }
        }
        public virtual ImageLayout BackgroundImageLayout { get; set; }
        public virtual BindingContext BindingContext { get; set; }

        public virtual int Bottom { get; }

        public virtual Rectangle Bounds { get; set; }

        public virtual bool CanFocus { get { return Widget.CanFocus; } }

        public virtual bool CanSelect { get; }

        public virtual bool Capture { get; set; }
        public virtual bool CausesValidation { get; set; }
        public virtual string CompanyName { get; }

        public virtual bool ContainsFocus { get; }

        public virtual ContextMenuStrip ContextMenuStrip { get; set; }

        public virtual ControlCollection Controls { get; }

        public virtual bool Created => _Created;
        internal bool _Created;

        public virtual Cursor Cursor { get; set; }

        public virtual ControlBindingsCollection DataBindings { get; }

        public virtual int DeviceDpi { get; }

        public virtual Rectangle DisplayRectangle { get; }

        public virtual bool Disposing { get; }

        public virtual DockStyle Dock
        {
            get
            {
                if (Enum.TryParse(Widget.Data["Dock"].ToString(), false, out DockStyle result))
                    return result;
                else
                    return DockStyle.None;
            }
            set
            {
                Widget.Data["Dock"] = value.ToString();
            }
        }
        public virtual bool Enabled { get { return Widget.Sensitive; } set { Widget.Sensitive = value; } }

        public virtual bool Focused { get { return Widget.IsFocus; } }
        private Font _Font;
        public virtual Font Font { get => _Font; set { _Font = value; UpdateStyle(); } }
        private Color _ForeColor;
        public virtual Color ForeColor { get => _ForeColor; set { _ForeColor = value; UpdateStyle(); } }

        public virtual bool HasChildren { get; }

        public virtual int Height { get { return Widget.HeightRequest; } set { Widget.HeightRequest = value; } }
        public virtual ImeMode ImeMode { get; set; }

        public virtual bool InvokeRequired { get; }

        public virtual bool IsAccessible { get; set; }

        public virtual bool IsDisposed { get; }

        public virtual bool IsHandleCreated { get; }

        public virtual bool IsMirrored { get; }

        public virtual LayoutEngine LayoutEngine { get; }

        public virtual int Left
        {
            get;
            set;
        }

        public virtual Point Location
        {
            get
            {
                return new Point(Left, Top);
            }
            set
            {
                Left = value.X;
                Top = value.Y;
            }
        }
        //public virtual Padding Margin { get; set; }
        //public virtual Size MaximumSize { get; set; }
        //public virtual Size MinimumSize { get; set; }
        public virtual string Name { get { return Widget.Name; } set { Widget.Name = value; } }
        public virtual Padding Padding { get; set; }
        public Gtk.Widget WidgetParent { get { return Widget.Parent; } set { Widget.Parent = value; } }
        public virtual Control Parent { get; set; }
        public virtual Size PreferredSize { get; }
        public virtual string ProductName { get; }
        public virtual string ProductVersion { get; }
        public virtual bool RecreatingHandle { get; }
        public virtual Drawing.Region Region { get; set; }
        public virtual int Right { get; }

        public virtual RightToLeft RightToLeft { get; set; }
        public virtual ISite Site { get; set; }
        public virtual Size Size
        {
            get
            {
                return new Size(Widget.WidthRequest, Widget.HeightRequest);
            }
            set
            {
                Widget.SetSizeRequest(value.Width, value.Height);
            }
        }
        public virtual int TabIndex { get; set; }
        public virtual bool TabStop { get; set; }
        public virtual object Tag { get; set; }
        public virtual string Text { get; set; }
        public virtual int Top
        {
            get;
            set;
        }

        public virtual Control TopLevelControl { get; }

        public virtual bool UseWaitCursor { get; set; }
        public virtual bool Visible { get { return Widget.Visible; } set { Widget.Visible = value; Widget.NoShowAll = value == false; } }
        public virtual int Width { get { return Widget.WidthRequest; } set { Widget.WidthRequest = value; } }
        public virtual IWindowTarget WindowTarget { get; set; }
        public virtual event EventHandler AutoSizeChanged;
        public virtual event EventHandler BackColorChanged;
        public virtual event EventHandler BackgroundImageChanged;
        public virtual event EventHandler BackgroundImageLayoutChanged;
        public virtual event EventHandler BindingContextChanged;
        public virtual event EventHandler CausesValidationChanged;
        public virtual event UICuesEventHandler ChangeUICues;
        public virtual event EventHandler Click
        {
            add { Widget.ButtonReleaseEvent += (object o, ButtonReleaseEventArgs args) => { value.Invoke(this, args); }; }
            remove { Widget.ButtonReleaseEvent -= (object o, ButtonReleaseEventArgs args) => { value.Invoke(this, args); }; }
        }

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
        //{
        //    add { Widget.DragDrop += (object o, Gtk.DragDropArgs args) => { value.Invoke(this, new DragEventArgs(null, Convert.ToInt32(args.RetVal), args.X, args.Y, DragDropEffects.All, DragDropEffects.Move)); }; }
        //    remove { Widget.DragDrop -= (object o, Gtk.DragDropArgs args) => { }; }
        //}

        public virtual event DragEventHandler DragEnter;
        public virtual event EventHandler DragLeave;
        public virtual event DragEventHandler DragOver;
        public virtual event EventHandler EnabledChanged;
        public virtual event EventHandler Enter
        {
            add
            {
                Widget.EnterNotifyEvent += (object o, Gtk.EnterNotifyEventArgs args) => { value.Invoke(this, args); };
                Widget.FocusInEvent += (object o, FocusInEventArgs args) => { value.Invoke(this, args); };
            }
            remove
            {
                Widget.EnterNotifyEvent -= (object o, Gtk.EnterNotifyEventArgs args) => { value.Invoke(this, args); };
                Widget.FocusInEvent -= (object o, FocusInEventArgs args) => { value.Invoke(this, args); };
            }
        }

        public virtual event EventHandler FontChanged;
        public virtual event EventHandler ForeColorChanged;
        public virtual event GiveFeedbackEventHandler GiveFeedback;
        public virtual event EventHandler GotFocus
        {
            add { Widget.FocusInEvent += (object o, FocusInEventArgs args) => { value.Invoke(this, new EventArgs()); }; }
            remove { Widget.FocusInEvent -= (object o, FocusInEventArgs args) => { value.Invoke(this, new EventArgs()); }; }
        }
        public virtual event EventHandler HandleCreated;
        public virtual event EventHandler HandleDestroyed;
        public virtual event HelpEventHandler HelpRequested;
        public virtual event EventHandler ImeModeChanged;
        public virtual event InvalidateEventHandler Invalidated;
        public virtual event KeyEventHandler KeyDown
        {
            add { Widget.KeyPressEvent += (object o, Gtk.KeyPressEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(this, new KeyEventArgs(result)); }; }
            remove { Widget.KeyPressEvent -= (object o, Gtk.KeyPressEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(this, new KeyEventArgs(result)); }; }
        }
        public virtual event KeyPressEventHandler KeyPress
        {
            add { Widget.KeyReleaseEvent += (object o, Gtk.KeyReleaseEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(this, new KeyPressEventArgs(args.Event.Key.ToString()[0])); }; }
            remove { Widget.KeyReleaseEvent -= (object o, Gtk.KeyReleaseEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(this, new KeyPressEventArgs(args.Event.Key.ToString()[0])); }; }
        }
        public virtual event KeyEventHandler KeyUp
        {
            add { Widget.KeyReleaseEvent += (object o, Gtk.KeyReleaseEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(this, new KeyEventArgs(result)); }; }
            remove { Widget.KeyReleaseEvent -= (object o, Gtk.KeyReleaseEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(this, new KeyEventArgs(result)); }; }
        }
        public virtual event LayoutEventHandler Layout;
        public virtual event EventHandler Leave
        {
            add { Widget.LeaveNotifyEvent += (object o, LeaveNotifyEventArgs args) => { value.Invoke(this, args); }; }
            remove { Widget.LeaveNotifyEvent -= (object o, LeaveNotifyEventArgs args) => { value.Invoke(this, args); }; }
        }
        public virtual event EventHandler LocationChanged;
        public virtual event EventHandler LostFocus
        {
            add { Widget.FocusOutEvent += (object o, FocusOutEventArgs args) => { value.Invoke(this, new EventArgs()); }; }
            remove { Widget.FocusOutEvent -= (object o, FocusOutEventArgs args) => { value.Invoke(this, new EventArgs()); }; }
        }
        public virtual event EventHandler MarginChanged;
        public virtual event EventHandler MouseCaptureChanged;
        public virtual event MouseEventHandler MouseClick
        {
            add { Widget.ButtonReleaseEvent += (object o, ButtonReleaseEventArgs args) => { Enum.TryParse<MouseButtons>(args.Event.Button.ToString(), out MouseButtons result); value.Invoke(this, new MouseEventArgs(result, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
            remove { Widget.ButtonReleaseEvent -= (object o, ButtonReleaseEventArgs args) => { Enum.TryParse<MouseButtons>(args.Event.Button.ToString(), out MouseButtons result); value.Invoke(this, new MouseEventArgs(result, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
        }
        public virtual event MouseEventHandler MouseDoubleClick;
        public virtual event MouseEventHandler MouseDown
        {
            add { Widget.ButtonPressEvent += (object o, ButtonPressEventArgs args) => { Enum.TryParse<MouseButtons>(args.Event.Button.ToString(), out MouseButtons result); value.Invoke(this, new MouseEventArgs(result, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
            remove { Widget.ButtonPressEvent -= (object o, ButtonPressEventArgs args) => { Enum.TryParse<MouseButtons>(args.Event.Button.ToString(), out MouseButtons result); value.Invoke(this, new MouseEventArgs(result, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
        }
        public virtual event EventHandler MouseEnter
        {
            add { Widget.EnterNotifyEvent += (object o, Gtk.EnterNotifyEventArgs args) => { value.Invoke(this, args); }; }
            remove { Widget.EnterNotifyEvent -= (object o, Gtk.EnterNotifyEventArgs args) => { value.Invoke(this, args); }; }
        }
        public virtual event EventHandler MouseHover;
        public virtual event EventHandler MouseLeave;
        public virtual event MouseEventHandler MouseMove
        {
            add { Widget.MotionNotifyEvent += (object o, MotionNotifyEventArgs args) => { value.Invoke(this, new MouseEventArgs(MouseButtons.None, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
            remove { Widget.MotionNotifyEvent -= (object o, MotionNotifyEventArgs args) => { value.Invoke(this, new MouseEventArgs(MouseButtons.None, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
        }
        public virtual event MouseEventHandler MouseUp;
        public virtual event MouseEventHandler MouseWheel;
        public virtual event EventHandler Move
        {
            add { Widget.MotionNotifyEvent += (object o, MotionNotifyEventArgs args) => { value.Invoke(this, args); }; }
            remove { Widget.MotionNotifyEvent -= (object o, MotionNotifyEventArgs args) => { value.Invoke(this, args); }; }
        }
        public virtual event EventHandler PaddingChanged;
        public virtual event PaintEventHandler Paint;
        public virtual event EventHandler ParentChanged;
        public virtual event PreviewKeyDownEventHandler PreviewKeyDown;
        public virtual event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp;
        public virtual event QueryContinueDragEventHandler QueryContinueDrag;
        public virtual event EventHandler RegionChanged;
        public virtual event EventHandler Resize;
        public virtual event EventHandler RightToLeftChanged;
        public virtual event EventHandler SizeChanged
        {
            add { Widget.SizeAllocated += (object o, SizeAllocatedArgs args) => { value.Invoke(this, args); }; }
            remove { Widget.SizeAllocated -= (object o, SizeAllocatedArgs args) => { value.Invoke(this, args); }; }
        }
        public virtual event EventHandler StyleChanged;
        public virtual event EventHandler SystemColorsChanged;
        public virtual event EventHandler TabIndexChanged;
        public virtual event EventHandler TabStopChanged;
        public virtual event EventHandler TextChanged;

        CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
        public virtual event EventHandler Validated
        {
            add { Widget.FocusOutEvent += (object o, FocusOutEventArgs args) => { if (cancelEventArgs.Cancel == false) { value.Invoke(this, new EventArgs()); } }; }
            remove { Widget.FocusOutEvent -= (object o, FocusOutEventArgs args) => { if (cancelEventArgs.Cancel == false) { value.Invoke(this, new EventArgs()); } }; }
        }
        public virtual event CancelEventHandler Validating
        {
            add { Widget.FocusOutEvent += (object o, FocusOutEventArgs args) => { cancelEventArgs.Cancel = false; value.Invoke(this, cancelEventArgs); }; }
            remove { Widget.FocusOutEvent -= (object o, FocusOutEventArgs args) => { cancelEventArgs.Cancel = false; value.Invoke(this, cancelEventArgs); }; }
        }
        public virtual event EventHandler VisibleChanged;
        //public event EventHandler Disposed;
        public virtual event EventHandler Load
        {
            add { Widget.Realized += (object sender, EventArgs e) => { value.Invoke(sender, e); }; }
            remove { Widget.Realized -= (object sender, EventArgs e) => { value.Invoke(sender, e); }; }
        }

        public virtual IAsyncResult BeginInvoke(Delegate method, params object[]? args)
        {
            System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.StartNew(state =>
            {
                method.DynamicInvoke((object[])state);
            }, args);

            return task;
        }
        public virtual IAsyncResult BeginInvoke(Delegate method)
        {
            return BeginInvoke(method, null);
        }
        public virtual IAsyncResult BeginInvoke(Action method)
        {
            System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.StartNew(method);
            return task;
        }
        public virtual object EndInvoke(IAsyncResult asyncResult)
        {
            if (asyncResult is System.Threading.Tasks.Task task)
            {
                if (task.IsCompleted == false && task.IsCanceled == false && task.IsFaulted == false)
                    task.GetAwaiter().GetResult();
            }
            return asyncResult.AsyncState;
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
            Graphics g = new Graphics(this.Widget, new Cairo.Context(this.Widget.Handle, true), Widget.Allocation);
            return g;
        }

        public virtual DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            return DragDropEffects.None;
        }

        public virtual void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds)
        {

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

        public virtual void Invalidate(Drawing.Region region)
        {

        }

        public virtual void Invalidate(Drawing.Region region, bool invalidateChildren)
        {

        }

        public virtual object Invoke(Delegate method)
        {
            return Invoke(method, null);
        }

        public virtual object Invoke(Delegate method, params object[] args)
        {
            return method.DynamicInvoke(args);
        }
        public virtual void Invoke(Action method)
        {
            method.Invoke();
        }
        public virtual O Invoke<O>(Func<O> method)
        {
            return method.Invoke();
        }
        public virtual int LogicalToDeviceUnits(int value)
        {
            return value;
        }

        public virtual Size LogicalToDeviceUnits(Size value)
        {
            return value;
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
            _Created = true;
        }

        public virtual void ResumeLayout(bool performLayout)
        {
            _Created = performLayout == false;
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
        public virtual Rectangle ClientRectangle { get; }

        public virtual Size ClientSize { get; set; }

        public virtual IntPtr Handle { get => this.Widget.Handle; }

        public virtual Padding Margin { get; set; }
        public virtual Size MaximumSize { get; set; }
        public virtual Size MinimumSize { get; set; }

        //public virtual ISite Site { get; set; }

        public virtual BorderStyle BorderStyle { get; set; }

        public virtual void Hide()
        {
            if (this.GtkControl is Misc con)
            {
                con.Hide();
                con.NoShowAll = true;
            }
        }

        public virtual void Show()
        {

        }
        protected virtual void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
        }
        protected virtual void OnParentChanged(EventArgs e)
        {
        }

        public virtual void SuspendLayout()
        {
            _Created = false;
        }

        public virtual void PerformLayout()
        {
            _Created = true;
        }

        public virtual void PerformLayout(Control affectedControl, string affectedProperty)
        {
            _Created = true;
        }

        public virtual void Update()
        {

        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual void BeginInit()
        {

        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual void EndInit()
        {

        }

        public new void Dispose()
        {
            Dispose(true);
            base.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (this.Widget != null)
                {
                    this.backgroundImage = null;
                    this._BackgroundImageBytes = null;
                    this.Widget.Destroy();
                }
            }
            catch { }
            base.Dispose(disposing);
        }

        public virtual bool UseVisualStyleBackColor { get; set; }

        //=========================
     
    }
}
