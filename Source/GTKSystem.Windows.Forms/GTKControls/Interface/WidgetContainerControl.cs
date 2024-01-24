/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */

using GLib;
using Gtk;
using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    /// <summary>
    /// WidgetContainerControl与WidgetControl属性方法基本一样，主要是分区继承的Control，ContainerControl
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WidgetContainerControl<T> : ContainerControl, ISynchronizeInvoke, IComponent, IDisposable, IControl, ISupportInitialize
    {
        private Gtk.Widget _widget;
        public override Gtk.Widget Widget
        {
            get { return _widget; }
        }
        public new Gtk.Container Container { get; private set; }
        private T _control;
        public T Control
        {
            get { return _control; }
        }
        private object _gtkControl;
        public override object GtkControl => _gtkControl;
        public WidgetContainerControl(params object[] args)
        {
            object widget = Activator.CreateInstance(typeof(T), args);
            _gtkControl = widget;
            _control = (T)widget;
            _widget = widget as Gtk.Widget;
            Container = widget as Gtk.Container;
            Dock = DockStyle.None;
            _widget.MarginStart = 0;
            _widget.MarginTop = 0;
            _widget.Drawn += Widget_Drawn;
            _widget.StyleContext.AddClass("DefaultThemeStyle"); 
        }

        private void Widget_Drawn(object o, DrawnArgs args)
        {
            string backcolorname = $"backcolor{o.GetHashCode()}";
            string fontcolorname = $"fontcolor{o.GetHashCode()}";
            string css = "";
            if (this.BackColor.Name != "Control" && this.BackColor.Name != "0")
            {
                string color = $"#{Convert.ToString(this.BackColor.R, 16).PadLeft(2, '0')}{Convert.ToString(this.BackColor.G, 16).PadLeft(2, '0')}{Convert.ToString(this.BackColor.B, 16).PadLeft(2, '0')}";
                css += $" .{backcolorname}{{background:{color};background-color:{color};}}";
            }
            if (this.ForeColor.Name != "Control" && this.ForeColor.Name != "0")
            {
                string color = $"#{Convert.ToString(this.ForeColor.R, 16).PadLeft(2, '0')}{Convert.ToString(this.ForeColor.G, 16).PadLeft(2, '0')}{Convert.ToString(this.ForeColor.B, 16).PadLeft(2, '0')}";
                css += $" .{fontcolorname}{{color:{color};}}";
            }
            CssProvider provider = new CssProvider();
            if (provider.LoadFromData(css))
            {
                _widget.StyleContext.AddProvider(provider, 900);
                _widget.StyleContext.AddClass(backcolorname);
                _widget.StyleContext.AddClass(fontcolorname);
            }

            Gdk.Rectangle rec = Widget.Allocation;
            if (Control is Gtk.Button || Control is Gtk.Image)
            {
                //由于绘画会覆盖容器内部所有子控件，不合适容器控件使用，只对button和picturebox设置背景
                if (_BackgroundImageBytes != null)
                {
                    if (backgroundPixbuf == null)
                    {
                        Gdk.Pixbuf imagePixbuf = new Gdk.Pixbuf(IntPtr.Zero);
                        ScaleImage(ref imagePixbuf, _BackgroundImageBytes, PictureBoxSizeMode.AutoSize, BackgroundImageLayout == ImageLayout.None ? ImageLayout.Tile : BackgroundImageLayout);
                        backgroundPixbuf = imagePixbuf.ScaleSimple(imagePixbuf.Width - 8, imagePixbuf.Height - 6, Gdk.InterpType.Tiles);
                    }
                    DrawBackgroundImage(args.Cr, backgroundPixbuf, rec);
                }
            }
            if (Paint != null)
                Paint(o, new PaintEventArgs(new Graphics(this.Widget, args.Cr, Widget.Allocation), new Drawing.Rectangle(rec.X, rec.Y, rec.Width, rec.Height)));
        }

        private Gdk.Pixbuf backgroundPixbuf;
        internal void DrawBackgroundColor(Cairo.Context ctx, Gtk.Widget control, Drawing.Color backcolor, Gdk.Rectangle rec)
        {
            ctx.Save();
            ctx.SetSourceRGB(backcolor.R / 255f, backcolor.G / 255f, backcolor.B / 255f);
            ctx.Rectangle(2, 2, rec.Width - 4, rec.Height - 4);
            ctx.Fill();
            ctx.Restore();
        }
        internal void DrawBackgroundImage(Cairo.Context ctx, Gdk.Pixbuf img, Gdk.Rectangle rec)
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
        internal void DrawBackgroundText(Cairo.Context ctx, Gtk.Widget control, Gdk.Rectangle rec)
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
                if (this.ForeColor.Name != "Control" && this.ForeColor.Name != "0")
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
        public override AccessibleObject AccessibilityObject { get; }

        public override string AccessibleDefaultActionDescription { get; set; }
        public override string AccessibleDescription { get; set; }
        public override string AccessibleName { get; set; }
        public override AccessibleRole AccessibleRole { get; set; }
        public override bool AllowDrop { get; set; }
        public override AnchorStyles Anchor { get; set; }
        public override Point AutoScrollOffset { get; set; }
        public override bool AutoSize { get; set; }
        public override Color BackColor { get; set; }

        private byte[] _BackgroundImageBytes;
        private System.Drawing.Image backgroundImage;
        public override System.Drawing.Image BackgroundImage
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
        public override ImageLayout BackgroundImageLayout { get; set; }
        public override BindingContext BindingContext { get; set; }

        public override int Bottom { get; }

        public override Rectangle Bounds { get; set; }

        public override bool CanFocus { get { return Widget.CanFocus; } }

        public override bool CanSelect { get; }

        public override bool Capture { get; set; }
        public override bool CausesValidation { get; set; }
        public override string CompanyName { get; }

        public override bool ContainsFocus { get; }

        public override ContextMenuStrip ContextMenuStrip { get; set; }

        public override ControlCollection Controls { get; }

        public override bool Created => _Created;
        internal bool _Created;

        public override Cursor Cursor { get; set; }

        public override ControlBindingsCollection DataBindings { get; }

        public override int DeviceDpi { get; }

        public override Rectangle DisplayRectangle { get; }

        public override bool Disposing { get; }

        public override DockStyle Dock
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
                if (value == DockStyle.Fill)
                {
                    Container.ResizeMode = Gtk.ResizeMode.Parent;
                }
            }
        }
        public override bool Enabled { get { return Widget.Sensitive; } set { Widget.Sensitive = value; } }

        public override bool Focused { get { return Widget.IsFocus; } }

        public override Font Font { get; set; }

        public override Color ForeColor { get; set; }

        public override bool HasChildren { get; }

        public override int Height { get { return Widget.HeightRequest; } set { Widget.HeightRequest = value; } }
        public override ImeMode ImeMode { get; set; }

        public override bool InvokeRequired { get; }

        public override bool IsAccessible { get; set; }

        public override bool IsDisposed { get; }

        public override bool IsHandleCreated { get; }

        public override bool IsMirrored { get; }

        public override LayoutEngine LayoutEngine { get; }

        public override int Left { get; set; }
        public override Point Location
        {
            get
            {
                return new Point(Widget.MarginStart, Widget.MarginTop);
            }
            set
            {
                Left = value.X;
                Top = value.Y;
                if (Widget.Parent is Gtk.FlowBoxChild)
                {
                    Widget.Data["InitMarginStart"] = Widget.MarginStart;
                    Widget.Data["InitMarginTop"] = Widget.MarginTop;
                }
                else
                {
                    Widget.MarginTop = Math.Max(0, value.Y);
                    Widget.MarginStart = Math.Max(0, value.X);
                    Widget.Data["InitMarginStart"] = Widget.MarginStart;
                    Widget.Data["InitMarginTop"] = Widget.MarginTop;
                }
            }
        }
        //public override Padding Margin { get; set; }
        //public override Size MaximumSize { get; set; }
        //public override Size MinimumSize { get; set; }
        public override string Name { get { return Widget.Name; } set { Widget.Name = value; } }
        public override Padding Padding { get; set; }
        public Gtk.Widget WidgetParent { get { return Widget.Parent; } set { Widget.Parent = value; } }
        public override Control Parent { get; set; }
        public override Size PreferredSize { get; }
        public override string ProductName { get; }
        public override string ProductVersion { get; }
        public override bool RecreatingHandle { get; }
        public override Region Region { get; set; }
        public override int Right { get; }

        public override RightToLeft RightToLeft { get; set; }
        public override ISite Site { get; set; }
        public override Size Size
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
        public override int TabIndex { get; set; }
        public override bool TabStop { get; set; }
        public override object Tag { get; set; }
        public override string Text { get; set; }
        public override int Top { get; set; }

        public override Control TopLevelControl { get; }

        public override bool UseWaitCursor { get; set; }
        public override bool Visible { get { return Widget.Visible; } set { Widget.Visible = value; } }
        public override int Width { get { return Widget.WidthRequest; } set { Widget.WidthRequest = value; } }
        public override IWindowTarget WindowTarget { get; set; }
        public override event EventHandler AutoSizeChanged;
        public override event EventHandler BackColorChanged;
        public override event EventHandler BackgroundImageChanged;
        public override event EventHandler BackgroundImageLayoutChanged;
        public override event EventHandler BindingContextChanged;
        public override event EventHandler CausesValidationChanged;
        public override event UICuesEventHandler ChangeUICues;
        public override event EventHandler Click
        {
            add { Widget.ButtonReleaseEvent += (object o, ButtonReleaseEventArgs args) => { value.Invoke(this, args); }; }
            remove { Widget.ButtonReleaseEvent -= (object o, ButtonReleaseEventArgs args) => { value.Invoke(this, args); }; }
        }

        public override event EventHandler ClientSizeChanged;
        public override event EventHandler ContextMenuStripChanged;
        public override event ControlEventHandler ControlAdded;
        public override event ControlEventHandler ControlRemoved;
        public override event EventHandler CursorChanged;
        public override event EventHandler DockChanged;
        public override event EventHandler DoubleClick;
        public override event EventHandler DpiChangedAfterParent;
        public override event EventHandler DpiChangedBeforeParent;
        public override event DragEventHandler DragDrop;
        //{
        //    add { Widget.DragDrop += (object o, Gtk.DragDropArgs args) => { value.Invoke(this, new DragEventArgs(null, Convert.ToInt32(args.RetVal), args.X, args.Y, DragDropEffects.All, DragDropEffects.Move)); }; }
        //    remove { Widget.DragDrop -= (object o, Gtk.DragDropArgs args) => { }; }
        //}

        public override event DragEventHandler DragEnter;
        public override event EventHandler DragLeave;
        public override event DragEventHandler DragOver;
        public override event EventHandler EnabledChanged;
        public override event EventHandler Enter
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

        public override event EventHandler FontChanged;
        public override event EventHandler ForeColorChanged;
        public override event GiveFeedbackEventHandler GiveFeedback;
        public override event EventHandler GotFocus
        {
            add { Widget.FocusInEvent += (object o, FocusInEventArgs args) => { value.Invoke(this, new EventArgs()); }; }
            remove { Widget.FocusInEvent -= (object o, FocusInEventArgs args) => { value.Invoke(this, new EventArgs()); }; }
        }
        public override event EventHandler HandleCreated;
        public override event EventHandler HandleDestroyed;
        public override event HelpEventHandler HelpRequested;
        public override event EventHandler ImeModeChanged;
        public override event InvalidateEventHandler Invalidated;
        public override event KeyEventHandler KeyDown
        {
            add { Widget.KeyPressEvent += (object o, Gtk.KeyPressEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(this, new KeyEventArgs(result)); }; }
            remove { Widget.KeyPressEvent -= (object o, Gtk.KeyPressEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(this, new KeyEventArgs(result)); }; }
        }
        public override event KeyPressEventHandler KeyPress
        {
            add { Widget.KeyReleaseEvent += (object o, Gtk.KeyReleaseEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(this, new KeyPressEventArgs(args.Event.Key.ToString()[0])); }; }
            remove { Widget.KeyReleaseEvent -= (object o, Gtk.KeyReleaseEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(this, new KeyPressEventArgs(args.Event.Key.ToString()[0])); }; }
        }
        public override event KeyEventHandler KeyUp
        {
            add { Widget.KeyReleaseEvent += (object o, Gtk.KeyReleaseEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(this, new KeyEventArgs(result)); }; }
            remove { Widget.KeyReleaseEvent -= (object o, Gtk.KeyReleaseEventArgs args) => { Enum.TryParse<Keys>(args.Event.Key.ToString(), out Keys result); value.Invoke(this, new KeyEventArgs(result)); }; }
        }
        public override event LayoutEventHandler Layout;
        public override event EventHandler Leave
        {
            add { Widget.LeaveNotifyEvent += (object o, LeaveNotifyEventArgs args) => { value.Invoke(this, args); }; }
            remove { Widget.LeaveNotifyEvent -= (object o, LeaveNotifyEventArgs args) => { value.Invoke(this, args); }; }
        }
        public override event EventHandler LocationChanged;
        public override event EventHandler LostFocus
        {
            add { Widget.FocusOutEvent += (object o, FocusOutEventArgs args) => { value.Invoke(this, new EventArgs()); }; }
            remove { Widget.FocusOutEvent -= (object o, FocusOutEventArgs args) => { value.Invoke(this, new EventArgs()); }; }
        }
        public override event EventHandler MarginChanged;
        public override event EventHandler MouseCaptureChanged;
        public override event MouseEventHandler MouseClick
        {
            add { Widget.ButtonReleaseEvent += (object o, ButtonReleaseEventArgs args) => { Enum.TryParse<MouseButtons>(args.Event.Button.ToString(), out MouseButtons result); value.Invoke(this, new MouseEventArgs(result, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
            remove { Widget.ButtonReleaseEvent -= (object o, ButtonReleaseEventArgs args) => { Enum.TryParse<MouseButtons>(args.Event.Button.ToString(), out MouseButtons result); value.Invoke(this, new MouseEventArgs(result, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
        }
        public override event MouseEventHandler MouseDoubleClick;
        public override event MouseEventHandler MouseDown
        {
            add { Widget.ButtonPressEvent += (object o, ButtonPressEventArgs args) => { Enum.TryParse<MouseButtons>(args.Event.Button.ToString(), out MouseButtons result); value.Invoke(this, new MouseEventArgs(result, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
            remove { Widget.ButtonPressEvent -= (object o, ButtonPressEventArgs args) => { Enum.TryParse<MouseButtons>(args.Event.Button.ToString(), out MouseButtons result); value.Invoke(this, new MouseEventArgs(result, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
        }
        public override event EventHandler MouseEnter
        {
            add { Widget.EnterNotifyEvent += (object o, Gtk.EnterNotifyEventArgs args) => { value.Invoke(this, args); }; }
            remove { Widget.EnterNotifyEvent -= (object o, Gtk.EnterNotifyEventArgs args) => { value.Invoke(this, args); }; }
        }
        public override event EventHandler MouseHover;
        public override event EventHandler MouseLeave;
        public override event MouseEventHandler MouseMove
        {
            add { Widget.MotionNotifyEvent += (object o, MotionNotifyEventArgs args) => { value.Invoke(this, new MouseEventArgs(MouseButtons.None, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
            remove { Widget.MotionNotifyEvent -= (object o, MotionNotifyEventArgs args) => { value.Invoke(this, new MouseEventArgs(MouseButtons.None, 1, (int)args.Event.X, (int)args.Event.Y, 0)); }; }
        }
        public override event MouseEventHandler MouseUp;
        public override event MouseEventHandler MouseWheel;
        public override event EventHandler Move
        {
            add { Widget.MotionNotifyEvent += (object o, MotionNotifyEventArgs args) => { value.Invoke(this, args); }; }
            remove { Widget.MotionNotifyEvent -= (object o, MotionNotifyEventArgs args) => { value.Invoke(this, args); }; }
        }
        public override event EventHandler PaddingChanged;
        public override event PaintEventHandler Paint;
        public override event EventHandler ParentChanged;
        public override event PreviewKeyDownEventHandler PreviewKeyDown;
        public override event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp;
        public override event QueryContinueDragEventHandler QueryContinueDrag;
        public override event EventHandler RegionChanged;
        public override event EventHandler Resize;
        public override event EventHandler RightToLeftChanged;
        public override event EventHandler SizeChanged
        {
            add { Widget.SizeAllocated += (object o, SizeAllocatedArgs args) => { value.Invoke(this, args); }; }
            remove { Widget.SizeAllocated -= (object o, SizeAllocatedArgs args) => { value.Invoke(this, args); }; }
        }
        public override event EventHandler StyleChanged;
        public override event EventHandler SystemColorsChanged;
        public override event EventHandler TabIndexChanged;
        public override event EventHandler TabStopChanged;
        public override event EventHandler TextChanged;

        CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
        public override event EventHandler Validated
        {
            add { Widget.FocusOutEvent += (object o, FocusOutEventArgs args) => { if (cancelEventArgs.Cancel == false) { value.Invoke(this, new EventArgs()); } }; }
            remove { Widget.FocusOutEvent -= (object o, FocusOutEventArgs args) => { if (cancelEventArgs.Cancel == false) { value.Invoke(this, new EventArgs()); } }; }
        }
        public override event CancelEventHandler Validating
        {
            add { Widget.FocusOutEvent += (object o, FocusOutEventArgs args) => { cancelEventArgs.Cancel = false; value.Invoke(this, cancelEventArgs); }; }
            remove { Widget.FocusOutEvent -= (object o, FocusOutEventArgs args) => { cancelEventArgs.Cancel = false; value.Invoke(this, cancelEventArgs); }; }
        }
        public override event EventHandler VisibleChanged;
        //public event EventHandler Disposed;
        public virtual event EventHandler Load
        {
            add { Widget.Realized += (object sender, EventArgs e) => { value.Invoke(sender, e); }; }
            remove { Widget.Realized -= (object sender, EventArgs e) => { value.Invoke(sender, e); }; }
        }
        public override IAsyncResult BeginInvoke(Delegate method)
        {
            return BeginInvoke(method, null);
        }

        public override IAsyncResult BeginInvoke(Delegate method, params object[] args)
        {
            return new AsyncResult();
        }

        public override void BringToFront()
        {

        }

        public override bool Contains(Control ctl)
        {
            return false;
        }

        public override void CreateControl()
        {

        }

        public override Graphics CreateGraphics()
        {
            Graphics g = new Graphics(this.Widget, new Cairo.Context(this.Widget.Handle, true), Widget.Allocation);
            return g;
        }

        public override DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            return DragDropEffects.None;
        }

        public override void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds)
        {

        }

        public override object EndInvoke(IAsyncResult asyncResult)
        {
            return asyncResult.AsyncState;
        }

        public override Form FindForm()
        {
            return null;
        }

        public override bool Focus()
        {
            return false;
        }

        public override Control GetChildAtPoint(Point pt)
        {
            return null;
        }

        public override Control GetChildAtPoint(Point pt, GetChildAtPointSkip skipValue)
        {
            return null;
        }

        public override IContainerControl GetContainerControl()
        {
            return null;
        }

        public override Control GetNextControl(Control ctl, bool forward)
        {
            return ctl;
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            return proposedSize;
        }

        public override void Invalidate()
        {

        }

        public override void Invalidate(bool invalidateChildren)
        {

        }

        public override void Invalidate(Rectangle rc)
        {

        }

        public override void Invalidate(Rectangle rc, bool invalidateChildren)
        {

        }

        public override void Invalidate(Region region)
        {

        }

        public override void Invalidate(Region region, bool invalidateChildren)
        {

        }

        public override object Invoke(Delegate method)
        {
            return null;
        }

        public override object Invoke(Delegate method, params object[] args)
        {
            return null;
        }

        public override int LogicalToDeviceUnits(int value)
        {
            return value;
        }

        public override Size LogicalToDeviceUnits(Size value)
        {
            return value;
        }

        public override Point PointToClient(Point p)
        {
            return p;
        }

        public override Point PointToScreen(Point p)
        {
            return p;
        }

        public override PreProcessControlState PreProcessControlMessage(ref Message msg)
        {
            return PreProcessControlState.MessageNotNeeded;
        }

        public override bool PreProcessMessage(ref Message msg)
        {
            return false;
        }

        public override Rectangle RectangleToClient(Rectangle r)
        {
            return r;
        }

        public override Rectangle RectangleToScreen(Rectangle r)
        {
            return r;
        }

        public override void Refresh()
        {

        }

        public override void ResetBackColor()
        {

        }

        public override void ResetBindings()
        {

        }

        public override void ResetCursor()
        {

        }

        public override void ResetFont()
        {

        }

        public override void ResetForeColor()
        {

        }

        public override void ResetImeMode()
        {

        }

        public override void ResetRightToLeft()
        {

        }

        public override void ResetText()
        {

        }

        public override void ResumeLayout()
        {
            _Created = true;
        }

        public override void ResumeLayout(bool performLayout)
        {
            _Created = performLayout == false;
        }

        public override void Scale(float ratio)
        {

        }

        public override void Scale(float dx, float dy)
        {

        }

        public override void Scale(SizeF factor)
        {

        }

        public override void ScaleBitmapLogicalToDevice(ref Bitmap logicalBitmap)
        {

        }

        public override void Select()
        {

        }

        public override bool SelectNextControl(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
        {
            return false;
        }

        public override void SendToBack()
        {

        }

        public override void SetBounds(int x, int y, int width, int height)
        {

        }

        public override void SetBounds(int x, int y, int width, int height, BoundsSpecified specified)
        {

        }

        public override void SuspendLayout()
        {
            _Created = false;
        }

        public override void PerformLayout()
        {
            _Created = true;
        }

        public override void PerformLayout(Control affectedControl, string affectedProperty)
        {
            _Created = true;
        }

        public override void Update()
        {

        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void BeginInit()
        {

        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void EndInit()
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
                    _control = default(T);
                    _widget = null;
                    Container = null;
                }
            }
            catch { }
            base.Dispose(disposing);
        }

        public override bool UseVisualStyleBackColor { get; set; }

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
