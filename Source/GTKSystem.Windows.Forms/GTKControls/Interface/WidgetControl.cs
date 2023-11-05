using Gtk;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;


namespace System.Windows.Forms
{
    public abstract class WidgetControl<T>: ISynchronizeInvoke, IComponent, IDisposable, IControl, ISupportInitialize
    {
        private Gtk.Widget _widget;
        public Gtk.Widget Widget { get { return _widget; } 
        }
        public Gtk.Container Container { get; private set; }
        private T _control;
        public T Control { 
            get { return _control; } 
            set {
                if (value == null)
                {
                    _control = default(T);
                    _widget = null;
                    Container = null;
                }
                else
                {
                    _control = value;
                    _widget = _control as Gtk.Widget;
                    Container = _control as Gtk.Container;
                    Dock = DockStyle.None;
                    Widget.MarginStart = 0;
                    Widget.MarginTop = 0;
                }
            }
        }
        public WidgetControl(params object[] args)
        {
            object widget = Activator.CreateInstance(typeof(T), args);
            _control = (T)widget;
            _widget = widget as Gtk.Widget;
            Container = widget as Gtk.Container;
            Dock = DockStyle.None;
            Widget.MarginStart = 0;
            Widget.MarginTop = 0;
            Widget.Drawn += Widget_Drawn;
        }

        private void Widget_Drawn(object o, DrawnArgs args)
        {
            Gdk.Rectangle rec = Widget.Allocation;
            if (Control is Gtk.Button || Control is Gtk.Image)
            {
                //由于绘画会覆盖容器内部所有子控件，不合适容器控件使用，只对button和picturebox设置背景
                if (this.BackColor != null && this.BackColor.Name != "0")
                {
                    DrawBackgroundColor(args.Cr, Widget, this.BackColor, rec);
                }
                if (_BackgroundImageBytes != null)
                {
                    if (backgroundPixbuf == null)
                    {
                        Gdk.Pixbuf imagePixbuf = new Gdk.Pixbuf(IntPtr.Zero);
                        ScaleImage(ref imagePixbuf, _BackgroundImageBytes, PictureBoxSizeMode.AutoSize, BackgroundImageLayout == ImageLayout.None ? ImageLayout.Tile : BackgroundImageLayout);
                        backgroundPixbuf = imagePixbuf.ScaleSimple(imagePixbuf.Width - 8, imagePixbuf.Height - 6, Gdk.InterpType.Tiles);
                    }
                   // Gdk.Pixbuf imagePixbuf = new Gdk.Pixbuf(_BackgroundImageBytes);
                    DrawBackgroundImage(args.Cr, backgroundPixbuf, rec);
                }
                if ((this.BackColor != null && this.BackColor.Name != "0") || backgroundPixbuf != null)
                {
                    if (string.IsNullOrEmpty(this.Text) == false)
                    {
                        DrawBackgroundText(args.Cr, Widget, rec);
                    }
                }
            }

            if (Paint != null)
                Paint(o, new PaintEventArgs(new Graphics(this.Widget,args.Cr, Widget.Allocation), new Drawing.Rectangle(rec.X, rec.Y, rec.Width, rec.Height)));
        }
        Gdk.Pixbuf backgroundPixbuf;
        void DrawBackgroundColor(Cairo.Context ctx, Gtk.Widget control, Drawing.Color backcolor, Gdk.Rectangle rec)
        {
            ctx.Save();
            ctx.SetSourceRGB(backcolor.R / 255f, backcolor.G / 255f, backcolor.B / 255f);
            ctx.Rectangle(2, 2, rec.Width - 4, rec.Height - 4);
            ctx.Fill();
            ctx.Restore();
        }
        void DrawBackgroundImage(Cairo.Context ctx, Gdk.Pixbuf img, Gdk.Rectangle rec)
        {
            Gdk.Size size = new Gdk.Size(rec.Width - 4, rec.Height - 4);
            ctx.Save();
            ctx.Translate(2, 2);
            //ctx.Scale(size.Width / (double)img.Width, size.Height / (double)img.Height);
            if (size.Width > img.Width)
            {
                ctx.Scale(size.Width / (double)img.Width, size.Width / (double)img.Width);
            }
            else if (size.Height > img.Height)
            {
                ctx.Scale(size.Height / (double)img.Height, size.Height / (double)img.Height);
            }
            Gdk.CairoHelper.SetSourcePixbuf(ctx, img, 0, 0);

            using (var p = ctx.GetSource())
            {
                if (p is Cairo.SurfacePattern pattern)
                {
                    if (size.Width > img.Width || size.Height > img.Height)
                    {
                        pattern.Filter = Cairo.Filter.Fast;
                    }
                    else
                        pattern.Filter = Cairo.Filter.Good;
                }
            }
            ctx.Paint();
            ctx.Restore();
        }
        void DrawBackgroundText(Cairo.Context ctx, Gtk.Widget control, Gdk.Rectangle rec)
        {
            if (Control is Gtk.Button button)
                button.Child.Visible = false;
            string text = this.Text;
            if (string.IsNullOrEmpty(text) == false)
            {
                ctx.Save();
                float textleng = 0;
                foreach (char w in text)
                {
                    if (char.IsLower(w) && char.IsLetter(w))
                        textleng += 0.5f;
                    else if (char.IsDigit(w))
                        textleng += 0.5f;
                    else
                        textleng += 1f;
                }
                float textSize = 15f;
                if (this.Font != null)
                {
                    textSize = this.Font.Size;
                    if (this.Font.Unit == GraphicsUnit.Point)
                        textSize = this.Font.Size * 1 / 72 * 96;
                    if (this.Font.Unit == GraphicsUnit.Inch)
                        textSize = this.Font.Size * 96;
                }
                var x = (int)((rec.Width - textleng * textSize) * 0.5f - 3);
                var y = (int)((rec.Height + textSize) * 0.5f - 3);
                if (x < 0) x = 0;
                if (y < 0) y = 0;
                ctx.Translate(x, y);
                if (this.ForeColor != null && this.ForeColor.Name != "0")
                    ctx.SetSourceRGBA(this.ForeColor.R / 255f, this.ForeColor.G / 255f, this.ForeColor.B / 255f, 1);

                Pango.Context pangocontext = control.PangoContext;
                string family = pangocontext.FontDescription.Family;
                ctx.SelectFontFace(family, Cairo.FontSlant.Normal, Cairo.FontWeight.Normal);
                ctx.SetFontSize(textSize);
                ctx.ShowText(text);
                ctx.Stroke();
                ctx.Restore();
            }
        }
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
        public BindingContext BindingContext { get; set; }

        public int Bottom { get; }

        public Rectangle Bounds { get; set; }

        public bool CanFocus { get { return Widget.CanFocus; } }

        public bool CanSelect { get; }

        public bool Capture { get; set; }
        public bool CausesValidation { get; set; }
        public string CompanyName { get; }

        public bool ContainsFocus { get; }

        public ContextMenuStrip ContextMenuStrip { get; set; }

        public virtual ControlCollection Controls { get; }

        public bool Created => _Created;
        internal bool _Created;

        public Cursor Cursor { get; set; }

        public ControlBindingsCollection DataBindings { get; }

        public int DeviceDpi { get; }

        public Rectangle DisplayRectangle { get; }

        public bool Disposing { get; }

        public DockStyle Dock
        {
            get
            {
                if (Enum.TryParse(Widget.Data["Dock"].ToString(), false, out DockStyle result))
                    return result;
                else
                    return DockStyle.None;
            }
            set { 
                Widget.Data["Dock"] = value.ToString(); 
                if (value == DockStyle.Fill) { 
                    Container.ResizeMode = Gtk.ResizeMode.Parent; 
                } 
            }
        }
        public bool Enabled { get { return Widget.Sensitive; } set { Widget.Sensitive = value; } }

        public bool Focused { get { return Widget.IsFocus; } }

        public Font Font { get; set; }

        private Color foreColor;
        public Color ForeColor { 
            get { return foreColor; } 
            set { 
                foreColor = value; 
                Widget.ModifyFg(Gtk.StateType.Normal,new Gdk.Color(value.R,value.G,value.B)); 
            }
        }

        public bool HasChildren { get; }

        public int Height { get { return Widget.HeightRequest; } set { Widget.HeightRequest = value; } }
        public ImeMode ImeMode { get; set; }

        public bool InvokeRequired { get; }

        public bool IsAccessible { get; set; }

        public bool IsDisposed { get; }

        public bool IsHandleCreated { get; }

        public bool IsMirrored { get; }

        public LayoutEngine LayoutEngine { get; }

        public int Left { get; set; }
        public virtual Point Location
        {
            get
            {
                return new Point(Widget.MarginStart, Widget.MarginTop);
            }
            set
            {
                Widget.MarginTop = Math.Max(0, value.Y);
                Widget.MarginStart = Math.Max(0, value.X);

                Widget.Data["InitMarginStart"] = Widget.MarginStart;
                Widget.Data["InitMarginTop"] = Widget.MarginTop;
            }
        }
        public Padding Margin { get; set; }
        public Size MaximumSize { get; set; }
        public Size MinimumSize { get; set; }
        public string Name { get { return Widget.Name; } set { Widget.Name = value; } }
        public Padding Padding { get; set; }
        public Gtk.Widget Parent { get { return Widget.Parent; } set { Widget.Parent = value; } }
        public Size PreferredSize { get; }
        public string ProductName { get; }
        public string ProductVersion { get; }
        public bool RecreatingHandle { get; }
        public Region Region { get; set; }
        public int Right { get; }

        public RightToLeft RightToLeft { get; set; }
        public ISite Site { get; set; }
        public Size Size
        {
            get
            {
                return new Size(Widget.WidthRequest, Widget.HeightRequest);
            }
            set
            {
                Widget.SetSizeRequest(value.Width, value.Height);
                Widget.Data["InitWidth"] = value.Width;
                Widget.Data["InitHeight"] = value.Height;
            }
        }
        public int TabIndex { get; set; }
        public bool TabStop { get; set; }
        public object Tag { get; set; }
        public virtual string Text { get; set; }
        public int Top { get; set; }

        public Control TopLevelControl { get; }

        public bool UseWaitCursor { get; set; }
        public bool Visible { get { return Widget.Visible; } set { Widget.Visible = value; } }
        public int Width { get { return Widget.WidthRequest; } set { Widget.WidthRequest = value; } }
        public IWindowTarget WindowTarget { get; set; }
        Control IControl.Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public virtual event EventHandler AutoSizeChanged;
        public virtual event EventHandler BackColorChanged;
        public virtual event EventHandler BackgroundImageChanged;
        public virtual event EventHandler BackgroundImageLayoutChanged;
        public virtual event EventHandler BindingContextChanged;
        public virtual event EventHandler CausesValidationChanged;
        public virtual event UICuesEventHandler ChangeUICues;
        public virtual event EventHandler Click
        {
            add { Widget.ButtonReleaseEvent += (object o, ButtonReleaseEventArgs args) => { value.Invoke(o, args); }; }
            remove { Widget.ButtonReleaseEvent -= (object o, ButtonReleaseEventArgs args) => { value.Invoke(o, args); }; }
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
        //    add { Widget.DragDrop += (object o, Gtk.DragDropArgs args) => { value.Invoke(o, new DragEventArgs(null, Convert.ToInt32(args.RetVal), args.X, args.Y, DragDropEffects.All, DragDropEffects.Move)); }; }
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
                Widget.EnterNotifyEvent += (object o, Gtk.EnterNotifyEventArgs args) => { value.Invoke(o, args); };
                Widget.FocusInEvent += (object o, FocusInEventArgs args) => { value.Invoke(o, args); };
            }
            remove
            {
                Widget.EnterNotifyEvent -= (object o, Gtk.EnterNotifyEventArgs args) => { value.Invoke(o, args); };
                Widget.FocusInEvent -= (object o, FocusInEventArgs args) => { value.Invoke(o, args); };
            }
        }

        public virtual event EventHandler FontChanged;
        public virtual event EventHandler ForeColorChanged;
        public virtual event GiveFeedbackEventHandler GiveFeedback;
        public virtual event EventHandler GotFocus
        {
            add { Widget.FocusInEvent += (object o, FocusInEventArgs args) => { value.Invoke(o, new EventArgs()); }; }
            remove { Widget.FocusInEvent -= (object o, FocusInEventArgs args) => { value.Invoke(o, new EventArgs()); }; }
        }
        public virtual event EventHandler HandleCreated;
        public virtual event EventHandler HandleDestroyed;
        public virtual event HelpEventHandler HelpRequested;
        public virtual event EventHandler ImeModeChanged;
        public virtual event InvalidateEventHandler Invalidated;
        public virtual event KeyEventHandler KeyDown
        {
            add { Widget.KeyPressEvent += (object o, Gtk.KeyPressEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(o, new KeyEventArgs(result)); }; }
            remove { Widget.KeyPressEvent -= (object o, Gtk.KeyPressEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(o, new KeyEventArgs(result)); }; }
        }
        public virtual event KeyPressEventHandler KeyPress
        {
            add { Widget.KeyReleaseEvent += (object o, Gtk.KeyReleaseEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(o, new KeyPressEventArgs(args.Event.Key.ToString()[0])); }; }
            remove { Widget.KeyReleaseEvent -= (object o, Gtk.KeyReleaseEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(o, new KeyPressEventArgs(args.Event.Key.ToString()[0])); }; }
        }
        public virtual event KeyEventHandler KeyUp
        {
            add { Widget.KeyReleaseEvent += (object o, Gtk.KeyReleaseEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(o, new KeyEventArgs(result)); }; }
            remove { Widget.KeyReleaseEvent -= (object o, Gtk.KeyReleaseEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(o, new KeyEventArgs(result)); }; }
        }
        public virtual event LayoutEventHandler Layout;
        public virtual event EventHandler Leave
        {
            add { Widget.LeaveNotifyEvent += (object o, LeaveNotifyEventArgs args) => { value.Invoke(o, args);  }; }
            remove { Widget.LeaveNotifyEvent -= (object o, LeaveNotifyEventArgs args) => { value.Invoke(o, args); }; }
        }
        public virtual event EventHandler LocationChanged;
        public virtual event EventHandler LostFocus
        {
            add { Widget.FocusOutEvent += (object o, FocusOutEventArgs args) => { value.Invoke(o, new EventArgs()); }; }
            remove { Widget.FocusOutEvent -= (object o, FocusOutEventArgs args) => { value.Invoke(o, new EventArgs()); }; }
        }
        public virtual event EventHandler MarginChanged;
        public virtual event EventHandler MouseCaptureChanged;
        public virtual event MouseEventHandler MouseClick
        {
            add { Widget.ButtonReleaseEvent += (object o, ButtonReleaseEventArgs args) => { Enum.TryParse<MouseButtons>(args.Event.Button.ToString(), out MouseButtons result);  value.Invoke(o, new MouseEventArgs(result, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
            remove { Widget.ButtonReleaseEvent -= (object o, ButtonReleaseEventArgs args) => { Enum.TryParse<MouseButtons>(args.Event.Button.ToString(), out MouseButtons result); value.Invoke(o, new MouseEventArgs(result, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
        }
        public virtual event MouseEventHandler MouseDoubleClick;
        public virtual event MouseEventHandler MouseDown
        {
            add { Widget.ButtonPressEvent += (object o, ButtonPressEventArgs args) => { Enum.TryParse<MouseButtons>(args.Event.Button.ToString(), out MouseButtons result); value.Invoke(o, new MouseEventArgs(result, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
            remove { Widget.ButtonPressEvent -= (object o, ButtonPressEventArgs args) => { Enum.TryParse<MouseButtons>(args.Event.Button.ToString(), out MouseButtons result); value.Invoke(o, new MouseEventArgs(result, 1, (int) args.Event.X, (int) args.Event.Y, 0)); }; }
        }
        public virtual event EventHandler MouseEnter
        {
            add { Widget.EnterNotifyEvent += (object o, Gtk.EnterNotifyEventArgs args) => { value.Invoke(o, args); }; }
            remove { Widget.EnterNotifyEvent -= (object o, Gtk.EnterNotifyEventArgs args) => { value.Invoke(o, args); }; }
        }
        public virtual event EventHandler MouseHover;
        public virtual event EventHandler MouseLeave;
        public virtual event MouseEventHandler MouseMove
        {
            add { Widget.MotionNotifyEvent += (object o, MotionNotifyEventArgs args) => { value.Invoke(o, new MouseEventArgs(MouseButtons.None, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
            remove { Widget.MotionNotifyEvent -= (object o, MotionNotifyEventArgs args) => { value.Invoke(o, new MouseEventArgs(MouseButtons.None, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
        }
        public virtual event MouseEventHandler MouseUp;
        public virtual event MouseEventHandler MouseWheel;
        public virtual event EventHandler Move
        {
            add { Widget.MotionNotifyEvent += (object o, MotionNotifyEventArgs args) => { value.Invoke(o, args); }; }
            remove { Widget.MotionNotifyEvent -= (object o, MotionNotifyEventArgs args) => { value.Invoke(o, args); }; }
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
            add { Widget.SizeAllocated += (object o, SizeAllocatedArgs args) => { value.Invoke(o, args); }; }
            remove { Widget.SizeAllocated -= (object o, SizeAllocatedArgs args) => { value.Invoke(o, args); }; }
        }
        public virtual event EventHandler StyleChanged;
        public virtual event EventHandler SystemColorsChanged;
        public virtual event EventHandler TabIndexChanged;
        public virtual event EventHandler TabStopChanged;
        public virtual event EventHandler TextChanged;

        CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
        public virtual event EventHandler Validated
        {
            add { Widget.FocusOutEvent += (object o, FocusOutEventArgs args) => { if (cancelEventArgs.Cancel == false) { value.Invoke(o, new EventArgs()); } }; }
            remove { Widget.FocusOutEvent -= (object o, FocusOutEventArgs args) => { if (cancelEventArgs.Cancel == false) { value.Invoke(o, new EventArgs()); } }; }
        }
        public virtual event CancelEventHandler Validating
        {
            add { Widget.FocusOutEvent += (object o, FocusOutEventArgs args) => { cancelEventArgs.Cancel = false; value.Invoke(o, cancelEventArgs); }; }
            remove { Widget.FocusOutEvent -= (object o, FocusOutEventArgs args) => { cancelEventArgs.Cancel = false; value.Invoke(o, cancelEventArgs); }; }
        }
        public virtual event EventHandler VisibleChanged;
        public virtual event EventHandler Disposed;
        public virtual event EventHandler Load
        {
            add { Widget.Realized += (object sender, EventArgs e) => { value.Invoke(sender, e); }; }
            remove { Widget.Realized -= (object sender, EventArgs e) => { value.Invoke(sender, e); }; }
        }
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

        public virtual void ResumeLayout()
        {
            _Created = true;
        }

        public virtual void ResumeLayout(bool performLayout)
        {
            _Created = performLayout == false;
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

        public void Update()
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

        public virtual void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_widget != null)
            {
                this.backgroundPixbuf = null;
                this.backgroundImage = null;
                this._BackgroundImageBytes = null;
                _widget.Destroy();
            }
            else
            {
                this.backgroundPixbuf = null;
                this.backgroundImage = null;
                this._BackgroundImageBytes = null;
                Control = default(T);
                _widget = null;
                Container = null;
            }
        }

        public bool UseVisualStyleBackColor { get; set; }

        protected void ScaleImage(ref Gdk.Pixbuf imagePixbuf, byte[] imagebytes, PictureBoxSizeMode sizeMode, ImageLayout backgroundMode)
        {
            if (imagebytes != null)
            {
                Gdk.Pixbuf pix = new Gdk.Pixbuf(imagebytes);
                int width = this.Size.Width > 0 ? this.Size.Width : pix.Width;
                int height = this.Size.Height > 0 ? this.Size.Height : pix.Height;

                if (width > 0 && height > 0)
                {
                    //Gdk.Pixbuf showpix = new Gdk.Pixbuf(new Cairo.ImageSurface(Cairo.Format.Argb32, width, height), 0, 0, width, height);
                    int newwidth = Math.Min(pix.Width, width);
                    int newheight = Math.Min(pix.Height, height);
                    Gdk.Pixbuf showpix = new Gdk.Pixbuf(new Cairo.ImageSurface(Cairo.Format.Argb32, newwidth, newheight), 0, 0, newwidth, newheight);
                    if (sizeMode == PictureBoxSizeMode.Normal && backgroundMode == ImageLayout.None)
                    {
                        pix.CopyArea(0, 0, Math.Min(pix.Width, showpix.Width), Math.Min(pix.Height, showpix.Height), showpix, 0, 0);
                    }
                    else if (sizeMode == PictureBoxSizeMode.StretchImage || backgroundMode == ImageLayout.Stretch)
                    {
                        Gdk.Pixbuf newpix = pix.ScaleSimple(width, height, Gdk.InterpType.Tiles);
                        newpix.CopyArea(0, 0, newpix.Width, newpix.Height, showpix, 0, 0);
                    }
                    else if (sizeMode == PictureBoxSizeMode.CenterImage || backgroundMode == ImageLayout.Center)
                    {
                        int offsetx = (pix.Width - showpix.Width) / 2;
                        int offsety = (pix.Height - showpix.Height) / 2;
                        pix.CopyArea(offsetx > 0 ? offsetx : 0, offsety > 0 ? offsety : 0, Math.Min(pix.Width, showpix.Width), Math.Min(pix.Height, showpix.Height), showpix, offsetx < 0 ? offsetx : 0, offsety < 0 ? offsety : 0);
                    }
                    else if (sizeMode == PictureBoxSizeMode.Zoom || backgroundMode == ImageLayout.Zoom)
                    {
                        if (pix.Width / width > pix.Height / height)
                        {
                            //图片的宽高比大于设置宽高比，以宽为准
                            Gdk.Pixbuf newpix = pix.ScaleSimple(width, width * pix.Height / pix.Width, Gdk.InterpType.Tiles);
                            newpix.CopyArea(0, 0, newpix.Width, newpix.Height, showpix, (showpix.Width - newpix.Width) / 2, (showpix.Height - newpix.Height) / 2);
                        }
                        else
                        {
                            Gdk.Pixbuf newpix = pix.ScaleSimple(height * pix.Width / pix.Height, height, Gdk.InterpType.Tiles);
                            newpix.CopyArea(0, 0, newpix.Width, newpix.Height, showpix, (showpix.Width - newpix.Width) / 2, (showpix.Height - newpix.Height) / 2);
                        }
                    }
                    else if (sizeMode == PictureBoxSizeMode.AutoSize)
                    {
                        pix.CopyArea(0, 0, Math.Min(pix.Width, showpix.Width), Math.Min(pix.Height, showpix.Height), showpix, 0, 0);
                    }

                    if (backgroundMode == ImageLayout.Tile)
                    {
                        //平铺背景图
                        if (showpix.Width < width || showpix.Height < height)
                        {
                            Gdk.Pixbuf backgroundpix = new Gdk.Pixbuf(new Cairo.ImageSurface(Cairo.Format.Argb32, width, height), 0, 0, width, height);
                            for (int y = 0; y < height; y += showpix.Height)
                            {
                                for (int x = 0; x < width; x += showpix.Width)
                                {
                                    showpix.CopyArea(0, 0, width - x > showpix.Width ? showpix.Width : width - x, height - y > showpix.Height ? showpix.Height : height - y, backgroundpix, x, y);
                                }
                            }
                            imagePixbuf = backgroundpix;
                        }
                        else
                        {
                            imagePixbuf = showpix;
                        }
                    }
                    else
                    {
                        imagePixbuf = showpix;
                    }
                }
            }
        }
    }
}
