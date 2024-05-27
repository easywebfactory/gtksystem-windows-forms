using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms.Design;

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
        public string unique_key { get; protected set; }

        public virtual Gtk.Widget Widget { get => GtkControl as Gtk.Widget; }
        public virtual Gtk.Container GtkContainer { get => GtkControl as Gtk.Container; }
        public IControlGtk ISelf { get => (IControlGtk)GtkControl; }
        public virtual object GtkControl { get; set; }
        CssProvider provider = new CssProvider();
        public Control()
        {
            this.unique_key = Guid.NewGuid().ToString();

            if (this.Widget != null)
            {
                this.Dock = DockStyle.None;
                this.Widget.StyleContext.AddClass("DefaultThemeStyle");
                Gtk.Widget widget = this.Widget;

                widget.ButtonPressEvent += Widget_ButtonPressEvent;
                widget.ButtonReleaseEvent += Widget_ButtonReleaseEvent;
                widget.EnterNotifyEvent += Widget_EnterNotifyEvent;
                widget.FocusInEvent += Widget_FocusInEvent;
                widget.FocusOutEvent += Widget_FocusOutEvent;
                widget.KeyPressEvent += Widget_KeyPressEvent;
                widget.KeyReleaseEvent += Widget_KeyReleaseEvent;
                widget.LeaveNotifyEvent += Widget_LeaveNotifyEvent;
                widget.MotionNotifyEvent += Widget_MotionNotifyEvent;
                widget.GrabNotify += Widget_GrabNotify;
                widget.Realized += Widget_Realized;
                widget.WidgetEventAfter += Widget_WidgetEventAfter;

                this.Widget.StyleContext.AddProvider(provider, 900);
                this.Widget.StyleContext.AddClass("forestyle");
            }
        }
        #region events
        private void Widget_WidgetEventAfter(object o, WidgetEventAfterArgs args)
        {
            if (args.Event.Type == Gdk.EventType.KeyPress)
            {
                if (KeyDown != null)
                {
                    if (args.Event is Gdk.EventKey eventkey)
                    {
                        Keys keys = (Keys)eventkey.HardwareKeycode;
                        KeyDown(this, new KeyEventArgs(keys));
                    }
                }
            }
        }

        private void Widget_Realized(object sender, EventArgs e)
        {
            UpdateForeStyle();
            if (Load != null)
                Load(this, e);
        }
        private void Widget_GrabNotify(object o, GrabNotifyArgs args)
        {
            if (Validating != null && args.WasGrabbed == false)
                Validating(this, cancelEventArgs);
            if (Validated != null && args.WasGrabbed == true)
                Validated(this, cancelEventArgs);
        }

        private void Widget_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            if (MouseDown != null)
            {
                MouseButtons result = MouseButtons.None;
                if (args.Event.Button == 1)
                    result = MouseButtons.Left;
                else if (args.Event.Button == 3)
                    result = MouseButtons.Right;
                else if (args.Event.Button == 2)
                    result = MouseButtons.Middle;
                MouseDown(this, new MouseEventArgs(result, 1, (int)args.Event.X, (int)args.Event.Y, 0));
            }
        }
        private void Widget_FocusOutEvent(object o, FocusOutEventArgs args)
        {
            if (LostFocus != null)
                LostFocus(this, args);
        }

        private void Widget_MotionNotifyEvent(object o, MotionNotifyEventArgs args)
        {
            if (Move != null)
                Move(this, args);
            if (MouseMove != null)
                MouseMove(this, new MouseEventArgs(MouseButtons.None, 1, (int)args.Event.X, (int)args.Event.Y, 0));
        }

        private void Widget_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
        {
            if (Leave != null)
                Leave(this, args);
            if (MouseHover != null)
                MouseHover(this, args);
        }

        private void Widget_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
        {
            if (KeyUp != null)
            {
                Keys keys = (Keys)args.Event.HardwareKeycode;
                KeyUp(this, new KeyEventArgs(keys));
            }
        }

        private void Widget_KeyPressEvent(object o, Gtk.KeyPressEventArgs args)
        {
            if (KeyPress != null)
            {
                Keys keys = (Keys)args.Event.HardwareKeycode;
                KeyPress(this, new KeyPressEventArgs(Convert.ToChar(keys)));
            }
        }

        private void Widget_FocusInEvent(object o, FocusInEventArgs args)
        {
            if (GotFocus != null)
                GotFocus(this, args);
        }

        private void Widget_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
        {
            if (Enter != null)
                Enter(this, args);
            if (MouseEnter != null)
                MouseEnter(this, args);
        }

        private void Widget_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
        {
            //Console.WriteLine($"Widget_ButtonReleaseEvent：{args.Event.Type.ToString()},{args.Event.Device.Name}");
            
            if (MouseUp != null)
            {
                MouseButtons result = MouseButtons.None;
                if (args.Event.Button == 1)
                    result = MouseButtons.Left;
                else if (args.Event.Button == 3)
                    result = MouseButtons.Right;
                else if (args.Event.Button == 2)
                    result = MouseButtons.Middle;

                MouseUp(this, new MouseEventArgs(result, 1, (int)args.Event.X, (int)args.Event.Y, 0));
            }
        }
        #endregion

        //===================

        protected virtual void UpdateForeStyle()
        {
            if (this.Widget != null && this.Widget.IsRealized)
                SetStyle(this.Widget);
        }
        protected virtual void UpdateBackgroundStyle()
        {
            if (this.Widget != null && this.Widget.IsRealized)
                ISelf.Override.OnAddClass();
        }
        protected virtual void SetStyle(Gtk.Widget widget)
        {
            Pango.AttrList attributes = new Pango.AttrList();
            string stylename = $"forestyle";
            StringBuilder style = new StringBuilder();
            if (this.ForeColor.Name != "0")
            {
                string color = $"rgba({this.ForeColor.R},{this.ForeColor.G},{this.ForeColor.B},{this.ForeColor.A})";
                style.AppendFormat("color:{0};", color);
                attributes.Insert(new Pango.AttrBackground(Convert.ToUInt16(this.ForeColor.R * 257), Convert.ToUInt16(this.ForeColor.G * 257), Convert.ToUInt16(this.ForeColor.B * 257)));
            }
            if (this.Font != null)
            {
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
            css.AppendLine(".BGTransparent{background:transparent;background-color:transparent;}");
            provider.LoadFromData(css.ToString());
        }
        protected virtual void SetStyle(ControlStyles styles,bool value)
        {

        }

        #region 背景
        public virtual bool UseVisualStyleBackColor { get; set; } = true;
        public virtual Color VisualStyleBackColor { get; }
        public virtual ImageLayout BackgroundImageLayout { get => ISelf.Override.BackgroundImageLayout; set => ISelf.Override.BackgroundImageLayout = value; }
        public virtual Drawing.Image BackgroundImage { get => ISelf.Override.BackgroundImage; set { ISelf.Override.BackgroundImage = value; ISelf.Override.OnPaint(); Invalidate(); } }
        public virtual Color BackColor
        {
            get
            {
                if (ISelf.Override.BackColor.HasValue)
                    return ISelf.Override.BackColor.Value;
                else if (UseVisualStyleBackColor)
                    return Color.FromName("0");
                else
                    return Color.Transparent; 
            }
            set {
                ISelf.Override.BackColor = value;
                ISelf.Override.OnAddClass();
                Invalidate();
            }
        }
        public virtual event PaintEventHandler Paint
        {
            add { ISelf.Override.Paint += value; }
            remove { ISelf.Override.Paint -= value; }
        }
        #endregion
        public virtual AccessibleObject AccessibilityObject { get; }

        public virtual string AccessibleDefaultActionDescription { get; set; }
        public virtual string AccessibleDescription { get; set; }
        public virtual string AccessibleName { get; set; }
        public virtual AccessibleRole AccessibleRole { get; set; }
        public virtual bool AllowDrop { get; set; }
        public virtual AnchorStyles Anchor { get; set; }
        public virtual Point AutoScrollOffset { get; set; }
        public virtual bool AutoSize { get; set; }
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
        public virtual Font Font { get => _Font; 
            set { _Font = value; UpdateForeStyle();} 
        }
        private Color _ForeColor;
        public virtual Color ForeColor { get => _ForeColor; 
            set { _ForeColor = value; UpdateForeStyle(); } 
        }

        public virtual bool HasChildren { get; }

        public virtual int Height { get { return Widget.HeightRequest; } set { Widget.HeightRequest = value; } }
        public virtual ImeMode ImeMode { get; set; }

        public virtual bool InvokeRequired { get; }

        public virtual bool IsAccessible { get; set; }

        public virtual bool IsDisposed { get; }

        public virtual bool IsHandleCreated { get => true; }

        public virtual bool IsMirrored { get; }

        public virtual LayoutEngine LayoutEngine { get; }

        public virtual int Left { get; set; }

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
        public virtual Control Parent { get; set; }
        public virtual Size PreferredSize { get; }
        public virtual string ProductName { get; }
        public virtual string ProductVersion { get; }
        public virtual bool RecreatingHandle { get; }
        public virtual Drawing.Region Region { get; set; }
        public virtual int Right { get; }

        public virtual RightToLeft RightToLeft { get; set; }
        //public override ISite Site { get => base.Site; set => base.Site = value; }
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
        public virtual int Top { get; set; }
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
        //public virtual event PaintEventHandler Paint;
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

        CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
        public virtual event EventHandler Validated;
        public virtual event CancelEventHandler Validating;
        public virtual event EventHandler VisibleChanged;
        //public event EventHandler Disposed;
        public virtual event EventHandler Load;

        public virtual IAsyncResult BeginInvoke(Delegate method, params object[] args)
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
            Control control = this.Parent;
            while (control != null)
            {
                if (control is Form)
                    break;
                else
                    control = this.Parent;
            }
            return control as Form;
        }

        public virtual bool Focus()
        {
            if (this.Widget != null)
            {
                this.Widget.IsFocus = true;
                return this.Widget.IsFocus;
            }
            else { 
                return false; 
            }
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
            Invalidate(true);
        }

        public virtual void Invalidate(bool invalidateChildren)
        {
            if (this.Widget != null && this.Widget.IsVisible)
            {
                Widget.Window.InvalidateRect(Widget.Allocation, invalidateChildren);
                System.Threading.Thread.Sleep(10);
            }
        }

        public virtual void Invalidate(Rectangle rc)
        {
            Invalidate(rc, true);
        }

        public virtual void Invalidate(Rectangle rc, bool invalidateChildren)
        {
            if (this.Widget != null)
            {
                Widget.Window.InvalidateRect(new Gdk.Rectangle(rc.X, rc.Y, rc.Width, rc.Height), invalidateChildren);
                System.Threading.Thread.Sleep(10);
            }
        }

        public virtual void Invalidate(Drawing.Region region)
        {
            Invalidate(region, true);
        }

        public virtual void Invalidate(Drawing.Region region, bool invalidateChildren)
        {
            if (this.Widget != null)
            {
                Widget.Window.InvalidateRect(Widget.Allocation, invalidateChildren);
            }
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
            if (Widget != null)
            {
                Widget.Window.GetPosition(out int x, out int y);
                if (p.X > x && p.Y > y)
                    return new Point(p.X - x, p.Y - y);
            }
            return new Point(p.X, p.Y);
        }

        public virtual Point PointToScreen(Point p)
        {
            if (Widget != null)
            {
                Widget.Window.GetPosition(out int x, out int y);
                if (p.X < x && p.Y < y)
                    return new Point(p.X + x, p.Y + y);
            }
            return new Point(p.X, p.Y);
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
            if (Widget != null)
            {
                Widget.Window.GetPosition(out int x, out int y);
                if (r.X > x && r.Y > y)
                    return new Rectangle(r.X - x, r.Y - y, r.Width, r.Height);
            }
            return new Rectangle(r.X, r.Y, r.Width, r.Height);
        }

        public virtual Rectangle RectangleToScreen(Rectangle r)
        {
            if (Widget != null)
            {
                Widget.Window.GetPosition(out int x, out int y);
                if (r.X < x && r.Y < y)
                    return new Rectangle(r.X + x, r.Y + y, r.Width, r.Height);
            }
            return new Rectangle(r.X, r.Y, r.Width, r.Height);
        }

        public virtual void Refresh()
        {
            if (this.Widget != null && this.Widget.IsVisible)
            {
                Widget.QueueDraw();
                System.Threading.Thread.Sleep(10);
            }
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
            if (this.Widget != null)
                this.Widget.SetStateFlags(StateFlags.Selected, true);
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
            SetBounds(x, y, width, height, BoundsSpecified.All);
        }

        public virtual void SetBounds(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (this.Widget != null)
            {
                Gdk.Rectangle rect = this.Widget.Clip;
                if (specified == BoundsSpecified.X)
                    rect.X = x;
                else if (specified == BoundsSpecified.Y)
                    rect.Y = y;
                else if (specified == BoundsSpecified.Width)
                    rect.Width = width;
                else if (specified == BoundsSpecified.Height)
                    rect.Height = height;
                else if (specified == BoundsSpecified.Size)
                {
                    rect.Width = width;
                    rect.Height = height;
                }
                else if (specified == BoundsSpecified.Location)
                {
                    rect.X = x;
                    rect.Y = y;
                }
                else
                {
                    rect.X = x;
                    rect.Y = y;
                    rect.Width = width;
                    rect.Height = height;
                }
                this.Widget.SetClip(rect);
            }
        }
        public virtual Rectangle ClientRectangle { get { if (Widget == null) { return new Rectangle(); } else { Widget.GetAllocatedSize(out Gdk.Rectangle allocation, out int baseline); return new Rectangle(allocation.X, allocation.Y, allocation.Width, allocation.Height); } } }

        public virtual Size ClientSize { get { Widget.GetSizeRequest(out int width,out int height); return new Size(width, height); } set { if (Widget != null) { Widget.SetSizeRequest(value.Width, value.Height); } } }

        public virtual IntPtr Handle { get => this.Widget == null ? IntPtr.Zero : this.Widget.Handle; }

        public virtual Padding Margin { get; set; }
        public virtual Size MaximumSize { get; set; }
        public virtual Size MinimumSize { get; set; }
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
            if (this.Widget != null)
            {
                Widget.ShowAll();
            }
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
            if (this.Widget != null)
            {
                this.Widget.Window.ProcessUpdates(true);
                this.Widget.QueueDraw();
            }
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
                    this.Widget.Destroy();
                    this.GtkControl = null;
                }
            }
            catch { }
            base.Dispose(disposing);
        }


        protected virtual CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = new CreateParams();
                createParams.ExStyle |= 32;
                return createParams;
            }
        }
        protected virtual void OnKeyDown(KeyEventArgs e)
        {

        }
        protected virtual void OnKeyUp(KeyEventArgs e)
        {

        }
        protected virtual void OnVisibleChanged(EventArgs e)
        {

        }
        protected virtual void OnSizeChanged(EventArgs e)
        {

        }
        protected virtual void Select(bool directed, bool forward)
        {

        }
        protected virtual void OnGotFocus(EventArgs e)
        {

        }
        protected virtual void WndProc(ref Message m)
        {
            //Console.WriteLine($"HWnd:{m.HWnd},WParam:{m.WParam},LParam:{m.LParam},Msg:{m.Msg}");
        }
    }
}
