/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    [Designer(typeof(ControlDesigner))]
    [ToolboxItemFilter("System.Windows.Forms")]
    public partial class Control : Component, IControl, ISynchronizeInvoke, IComponent, IDisposable, ISupportInitialize, IArrangedElement
    {
        private Gtk.Application app = Application.Init();
        public string unique_key { get; protected set; }

        public virtual Gtk.Widget Widget { get => GtkControl as Gtk.Widget; }
        public virtual Gtk.Container GtkContainer { get => GtkControl as Gtk.Container; }
        public virtual IControlGtk ISelf { get => GtkControl as IControlGtk; }
        public virtual object GtkControl { get; set; }
        public Control()
        {
            this.unique_key = Guid.NewGuid().ToString().ToLower();
            Gtk.Widget widget = this.Widget;
            if (widget != null)
            {
                if (widget is Gtk.Window win)
                {
                    widget.Halign = Align.Fill;
                    widget.Valign = Align.Fill;
                }
                else
                {
                    widget.Halign = Align.Start;
                    widget.Valign = Align.Start;
                    widget.Expand = false;
                }
                widget.Data["Control"] = this;
                widget.StyleContext.AddClass("DefaultThemeStyle");
                widget.ButtonPressEvent += Widget_ButtonPressEvent;
                widget.ButtonReleaseEvent += Widget_ButtonReleaseEvent;
                widget.EnterNotifyEvent += Widget_EnterNotifyEvent;
                widget.MotionNotifyEvent += Widget_MotionNotifyEvent;
                widget.LeaveNotifyEvent += Widget_LeaveNotifyEvent;
                widget.ScrollEvent += Widget_ScrollEvent;
                widget.FocusInEvent += Widget_FocusInEvent;
                widget.FocusOutEvent += Widget_FocusOutEvent;
                widget.KeyPressEvent += Widget_KeyPressEvent;
                widget.KeyReleaseEvent += Widget_KeyReleaseEvent;
                widget.Realized += Widget_Realized;
                widget.ConfigureEvent += Widget_ConfigureEvent;
                ISelf.Override.PaintGraphics += Override_PaintGraphics;
                widget.SizeAllocated += Widget_SizeAllocated;
            }
        }
        private int size_width = 0;
        private int size_height = 0;
        private int location_x = 0;
        private int location_y = 0;
        private void Widget_SizeAllocated(object o, SizeAllocatedArgs args)
        {
            if (args.Allocation.Width != size_width || args.Allocation.Height != size_height)
            {
                size_width = args.Allocation.Width;
                size_height = args.Allocation.Height;
                if (SizeChanged != null)
                    SizeChanged(this, EventArgs.Empty);
            }
            if (args.Allocation.X != location_x || args.Allocation.Y != location_y)
            {
                location_x = args.Allocation.X;
                location_y = args.Allocation.Y;
                if (LocationChanged != null)
                    LocationChanged(this, EventArgs.Empty);
            }
        }
        private void Widget_ConfigureEvent(object o, ConfigureEventArgs args)
        {
            if (Move != null)
                Move(this, args);
        }
        #region events
        private bool WidgetRealized = false;
        private void Widget_Realized(object sender, EventArgs e)
        {
            if (WidgetRealized == false)
            {
                WidgetRealized = true;
                InitStyle((Gtk.Widget)sender);
                if (Load != null)
                    Load(this, e);
            }
        }

        private void Widget_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            //Console.WriteLine($"Widget_ButtonPressEvent1:{args.Event.XRoot},{args.Event.YRoot};{args.Event.X},{args.Event.Y}");
            MouseButtons result = MouseButtons.None;
            if (args.Event.Button == 1)
                result = MouseButtons.Left;
            else if (args.Event.Button == 2)
                result = MouseButtons.Middle;
            else if (args.Event.Button == 3)
                result = MouseButtons.Right;

            Gtk.Widget owidget = (Gtk.Widget)o;
            owidget.Window.GetOrigin(out int x, out int y);//避免事件穿透错误
            if (MouseDown != null)
            {
                MouseDown(this, new MouseEventArgs(result, 1, (int)args.Event.XRoot - x, (int)args.Event.YRoot - y, 0));
            }
            if (args.Event.Type == Gdk.EventType.TwoButtonPress || args.Event.Type == Gdk.EventType.DoubleButtonPress)
            {
                if (MouseDoubleClick != null)
                    MouseDoubleClick(this, new MouseEventArgs(result, 2, (int)args.Event.XRoot - x, (int)args.Event.YRoot - y, 0));
                if (DoubleClick != null)
                    DoubleClick(this, EventArgs.Empty);
            }
            else
            {
                if (Click != null)
                    Click(this, EventArgs.Empty);
                if (MouseClick != null)
                    MouseClick(this, new MouseEventArgs(result, 1, (int)args.Event.XRoot - x, (int)args.Event.YRoot - y, 0));
            }
            
        }
        private void Widget_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
        {
            if (MouseUp != null)
            {
                MouseButtons result = MouseButtons.None;
                if (args.Event.Button == 1)
                    result = MouseButtons.Left;
                else if (args.Event.Button == 2)
                    result = MouseButtons.Middle;
                else if (args.Event.Button == 3)
                    result = MouseButtons.Right;
                Gtk.Widget owidget = (Gtk.Widget)o;
                owidget.Window.GetOrigin(out int x, out int y);
                MouseUp(this, new MouseEventArgs(result, 1, (int)args.Event.XRoot - x, (int)args.Event.YRoot - y, 0));
            }

            if (ContextMenuStrip != null)
            {
                if (args.Event.Button == 3)
                {
                    ContextMenuStrip.Widget.ShowAll();
                    ((Gtk.Menu)ContextMenuStrip.Widget).PopupAtPointer(args.Event);
                }
            }
        }

        private void Widget_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
        {
            if (Cursor != null)
            {
                if (Cursor.CursorType == Gdk.CursorType.CursorIsPixmap)
                {
                    this.Widget.Window.Cursor = new Gdk.Cursor(((Gtk.Widget)o).Display, Cursor.CursorPixbuf, Cursor.CursorsXY.X, Cursor.CursorsXY.Y);
                }
                else
                {
                    this.Widget.Window.Cursor = new Gdk.Cursor(((Gtk.Widget)o).Display, Cursor.CursorType);
                }
            }
            if (Enter != null)
                Enter(this, args);
            if (MouseEnter != null)
                MouseEnter(this, args);

            if (MouseHover != null)
                MouseHover(this, args);
        }
        private void Widget_MotionNotifyEvent(object o, MotionNotifyEventArgs args)
        {
            if (MouseMove != null)
                MouseMove(this, new MouseEventArgs(MouseButtons.None, 1, (int)args.Event.X, (int)args.Event.Y, 0));
        }
        private void Widget_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
        {
            if (Cursor != null)
            {
                this.Widget.Window.Cursor = null;
            }
            if (Leave != null)
                Leave(this, args);
            if (MouseLeave != null)
                MouseLeave(this, args);


        }
        private void Widget_ScrollEvent(object o, Gtk.ScrollEventArgs args)
        {
            if (MouseWheel != null)
                MouseWheel(this, new MouseEventArgs(MouseButtons.None, 0, (int)args.Event.X, (int)args.Event.Y, (int)args.Event.DeltaY));
        }
        private void Widget_FocusInEvent(object o, FocusInEventArgs args)
        {
            if (GotFocus != null)
                GotFocus(this, args);
        }
        private void Widget_FocusOutEvent(object o, FocusOutEventArgs args)
        {
            if (LostFocus != null)
                LostFocus(this, args);

            if (Validating != null)
                Validating(this, cancelEventArgs);
            if (Validated != null && cancelEventArgs.Cancel == false)
                Validated(this, cancelEventArgs);
        }
        private void Widget_KeyPressEvent(object o, Gtk.KeyPressEventArgs args)
        {
            if (KeyDown != null)
            {
                if (args.Event is Gdk.EventKey eventkey)
                {
                    Keys keys = (Keys)eventkey.HardwareKeycode;
                    if(eventkey.State.HasFlag(Gdk.ModifierType.Mod1Mask))
                        keys |= Keys.Alt;
                    if (eventkey.State.HasFlag(Gdk.ModifierType.ControlMask))
                        keys |= Keys.Control;
                    if (eventkey.State.HasFlag(Gdk.ModifierType.ShiftMask))
                        keys |= Keys.Shift;
                    if (eventkey.State.HasFlag(Gdk.ModifierType.LockMask))
                        keys |= Keys.CapsLock;

                    KeyDown(this, new KeyEventArgs(keys));
                }
            }
        }
        private void Widget_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
        {
            if (KeyUp != null)
            {
                Keys keys = (Keys)args.Event.HardwareKeycode;
                KeyUp(this, new KeyEventArgs(keys));
            }
            if (KeyPress != null)
            {
                Keys keys = (Keys)args.Event.HardwareKeycode;
                KeyPress(this, new KeyPressEventArgs(Convert.ToChar(keys)));
            }
        }

        #endregion

        //===================
        protected virtual void InitStyle(Gtk.Widget widget)
        {
             SetStyle(widget);
        }
        protected virtual void UpdateStyle()
        {
            if (this.Widget != null && this.Widget.IsMapped)
                SetStyle(this.Widget);
        }
        protected virtual void UpdateBackgroundStyle()
        {
            if (this.Widget != null && this.Widget.IsMapped)
                ISelf.Override.OnAddClass();
        }
        protected virtual void SetStyle(Gtk.Widget widget)
        {
            StringBuilder style = new StringBuilder();
            if (widget is Gtk.Image) { }
            else
            {
                if (this.Image != null && this.Image.PixbufData != null)
                {
                    string imguri = $"Resources/{widget.WidgetPath.IterGetName(0)}${widget.Name}_img.png";
                    if (!File.Exists(imguri))
                    {
                        Gdk.Pixbuf imagepixbuf = new Gdk.Pixbuf(this.Image.PixbufData);
                        imagepixbuf.Save(imguri, "png");
                    }
                    style.AppendFormat("background:url(\"{0}\")", imguri);
                    style.Append(" no-repeat");
                    if (this.ImageAlign == ContentAlignment.TopLeft)
                    {
                        style.Append(" top left");
                    }
                    else if (this.ImageAlign == ContentAlignment.TopCenter)
                    {
                        style.Append(" top center");
                    }
                    else if (this.ImageAlign == ContentAlignment.TopRight)
                    {
                        style.Append(" top right");
                    }
                    else if (this.ImageAlign == ContentAlignment.MiddleLeft)
                    {
                        style.Append(" center left");
                    }
                    else if (this.ImageAlign == ContentAlignment.MiddleCenter)
                    {
                        style.Append(" center center");
                    }
                    else if (this.ImageAlign == ContentAlignment.MiddleRight)
                    {
                        style.Append(" center right");
                    }
                    else if (this.ImageAlign == ContentAlignment.BottomLeft)
                    {
                        style.Append(" bottom left");
                    }
                    else if (this.ImageAlign == ContentAlignment.BottomCenter)
                    {
                        style.Append(" bottom center");
                    }
                    else if (this.ImageAlign == ContentAlignment.BottomRight)
                    {
                        style.Append(" bottom right");
                    }
                    else
                    {
                        style.Append(" center center");
                    }

                    if (this.BackgroundImage != null && this.BackgroundImage.PixbufData != null)
                    {
                        string bgimguri = $"Resources/{widget.WidgetPath.IterGetName(0)}${widget.Name}_bg.png";
                        if (!File.Exists(bgimguri))
                        {
                            Gdk.Pixbuf bgpixbuf = new Gdk.Pixbuf(this.BackgroundImage.PixbufData);
                            bgpixbuf.Save(bgimguri, "png");
                        }

                        style.AppendFormat(",url(\"Resources/{0}_bg.png\") repeat", widget.Name);
                    }
                    style.Append(";");
                    style.Append("background-origin: padding-box;");
                    style.Append("background-clip: padding-box;");
                }
                else if (this.BackgroundImage != null && this.BackgroundImage.PixbufData != null)
                {
                    Gdk.Pixbuf bgpixbuf = new Gdk.Pixbuf(this.BackgroundImage.PixbufData);
                    string bgimguri = $"Resources/{widget.WidgetPath.IterGetName(0)}${widget.Name}_bg.png";
                    if (!File.Exists(bgimguri))
                    {
                        bgpixbuf.Save(bgimguri, "png");
                    }
                    style.AppendFormat("background-image:url(\"{0}\");", bgimguri);
                    if (this.BackgroundImageLayout == ImageLayout.Tile)
                    {
                        style.Append("background-repeat:repeat;");
                    }
                    else if (this.BackgroundImageLayout == ImageLayout.Zoom)
                    {
                        style.Append("background-repeat:no-repeat;");
                        style.Append("background-size: contain;");
                        style.Append("background-position:center;");
                    }
                    else if (this.BackgroundImageLayout == ImageLayout.Stretch)
                    {
                        style.Append("background-repeat:no-repeat;");
                        style.Append("background-size: cover;");
                        style.Append("background-position:center;");
                    }
                    else if (this.BackgroundImageLayout == ImageLayout.Center)
                    {
                        style.Append("background-repeat:no-repeat;");
                        if (widget.HeightRequest < bgpixbuf.Height)
                            style.Append("background-position:top,center;");
                        else
                            style.Append("background-position:center,center;");

                    }
                    else
                    {
                        style.Append("background-repeat:no-repeat;");
                    }
                    style.Append("background-origin: padding-box;");
                    style.Append("background-clip: padding-box;");
                    if (this.BackColor.Name != "0")
                    {
                        Color backColor = this.BackColor;
                        string color = $"rgba({backColor.R},{backColor.G},{backColor.B},{backColor.A})";
                        style.AppendFormat("background-color:{0};", color);
                    }
                }
                else if (this.BackColor.Name != "0")
                {
                    Color backColor = this.BackColor;
                    string color = $"rgba({backColor.R},{backColor.G},{backColor.B},{backColor.A})";
                    style.AppendFormat("background-color:{0};background:{0};", color);
                }
                
                if (this.ForeColor.Name != "0")
                {
                    Color foreColor = this.ForeColor;
                    string color = $"rgba({foreColor.R},{foreColor.G},{foreColor.B},{foreColor.A})";
                    style.AppendFormat("color:{0};", color);
                }
                if (this.Font != null)
                {
                    Font font = this.Font;
                    if (font.Unit == GraphicsUnit.Pixel)
                        style.AppendFormat("font-size:{0}px;", font.Size);
                    else if (font.Unit == GraphicsUnit.Inch)
                        style.AppendFormat("font-size:{0}in;", font.Size);
                    else if (font.Unit == GraphicsUnit.Point)
                        style.AppendFormat("font-size:{0}pt;", font.Size);
                    else if (font.Unit == GraphicsUnit.Millimeter)
                        style.AppendFormat("font-size:{0}mm;", font.Size);
                    else if (font.Unit == GraphicsUnit.Document)
                        style.AppendFormat("font-size:{0}cm;", font.Size);
                    else if (font.Unit == GraphicsUnit.Display)
                        style.AppendFormat("font-size:{0}pc;", font.Size);
                    else
                        style.AppendFormat("font-size:{0}pt;", font.Size);

                    if (string.IsNullOrWhiteSpace(font.FontFamily?.Name) == false)
                    {
                        style.AppendFormat("font-family:\"{0}\";", font.FontFamily.Name);
                    }
                    if ((font.Style & FontStyle.Bold) != 0)
                    {
                        style.Append("font-weight:bold;");
                    }
                    if ((font.Style & FontStyle.Italic) != 0)
                    {
                        style.Append("font-style:italic;");
                    }
                    if ((font.Style & FontStyle.Underline) != 0)
                    {
                        style.Append("text-decoration:underline;");
                    }
                    if ((font.Style & FontStyle.Strikeout) != 0)
                    {
                        style.Append("text-decoration:line-through;");
                    }
                }
                if (style.Length > 10)
                {
                    string styleClassName = $"s{unique_key}";
                    StringBuilder css = new StringBuilder();
                    css.AppendLine($".{styleClassName}{{{style.ToString()}}}");
                    if (widget is Gtk.TextView)
                    {
                        css.AppendLine($".{styleClassName} text{{{style.ToString()}}}");
                        css.AppendLine($".{styleClassName} .view{{{style.ToString()}}}");
                    }
                    CssProvider provider = new CssProvider();
                    if (provider.LoadFromData(css.ToString()))
                    {
                        if (widget.StyleContext.HasClass(styleClassName))
                            widget.StyleContext.RemoveProvider(provider);
                        widget.StyleContext.AddProvider(provider, 900);
                        widget.StyleContext.AddClass(styleClassName);
                    }
                }
            }
        }
        protected virtual void SetStyle(ControlStyles styles, bool value)
        {
        }

        #region 背景
        public virtual System.Drawing.Image Image { get; set; }
        public virtual System.Drawing.ContentAlignment ImageAlign { get; set; }

        public virtual bool UseVisualStyleBackColor { get; set; } = true;
        public virtual Color VisualStyleBackColor { get; }
        public virtual ImageLayout BackgroundImageLayout { get => ISelf == null ? ImageLayout.None : ISelf.Override.BackgroundImageLayout; set { if (ISelf != null) { ISelf.Override.BackgroundImageLayout = value; } } }
        public virtual Drawing.Image BackgroundImage { get => ISelf == null ? null : ISelf.Override.BackgroundImage; set { if (ISelf != null) { ISelf.Override.BackgroundImage = value; Refresh(); } } }
        public virtual Color BackColor
        {
            get
            {
                if (ISelf.Override.BackColor.HasValue)
                    return ISelf.Override.BackColor.Value;
                //else if (UseVisualStyleBackColor)
                //    return Color.FromName("0");
                //else
                //    return Color.Transparent; 
                else
                    return Color.FromName("0");
            }
            set {
                ISelf.Override.BackColor = value;
                ISelf.Override.OnAddClass();
                UpdateStyle();
                Refresh();
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
        private AnchorStyles _anchor;
        public virtual AnchorStyles Anchor { 
            get=> _anchor; 
            set {
                _anchor = value;
                SetAnchorStyles(Widget, _anchor);
                if (AnchorChanged != null)
                    AnchorChanged(this, EventArgs.Empty);
            } 
        }
        private void SetAnchorStyles(Gtk.Widget widget, AnchorStyles anchorStyles)
        {
            if (anchorStyles.HasFlag(AnchorStyles.Left) && anchorStyles.HasFlag(AnchorStyles.Right))
            {
                widget.Halign = Gtk.Align.Fill;
            }
            else if (anchorStyles.HasFlag(AnchorStyles.Left))
            {
                widget.Halign = Gtk.Align.Start;
            }
            else if (anchorStyles.HasFlag(AnchorStyles.Right))
            {
                widget.Halign = Gtk.Align.End;
            }
            else
            {
                widget.Halign = Gtk.Align.Start;
            }

            if (anchorStyles.HasFlag(AnchorStyles.Top) && anchorStyles.HasFlag(AnchorStyles.Bottom))
            {
                widget.Valign = Gtk.Align.Fill;
            }
            else if (anchorStyles.HasFlag(AnchorStyles.Top))
            {
                widget.Valign = Gtk.Align.Start;
            }
            else if (anchorStyles.HasFlag(AnchorStyles.Bottom))
            {
                widget.Valign = Gtk.Align.End;
            }
            else
            {
                widget.Valign = Gtk.Align.Start;
            }
        }
        public virtual Point AutoScrollOffset { get; set; }
        private bool _autoSize;
        public virtual bool AutoSize { 
            get => _autoSize; 
            set { _autoSize = value;
                if (_autoSize == true) { this.Widget.WidthRequest = -1; this.Widget.HeightRequest = -1; } else { Size = _size; } 
            } 
        }
        public virtual BindingContext BindingContext { get; set; }
        public virtual Rectangle Bounds { get=> new Rectangle(Widget.Clip.X, this.Widget.Clip.Y, this.Widget.Clip.Width, this.Widget.Clip.Height); set { SetBounds(value.X, value.Y, value.Width, value.Height); } }

        public virtual bool CanFocus { get { return this.Widget.CanFocus; } }

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
        private DockStyle _dock;
        public virtual DockStyle Dock
        {
            get
            {
                return _dock;
            }
            set
            {
                _dock = value;
                if (value == DockStyle.Fill)
                {
                    this.Widget.Halign = Align.Fill;
                    this.Widget.Valign = Align.Fill;
                }
                else if (value == DockStyle.Left)
                {
                    this.Widget.Halign = Align.Start;
                    this.Widget.Valign = Align.Fill;
                }
                else if (value == DockStyle.Top)
                {
                    this.Widget.Halign = Align.Fill;
                    this.Widget.Valign = Align.Start;
                }
                else if (value == DockStyle.Right)
                {
                    this.Widget.Halign = Align.End;
                    this.Widget.Valign = Align.Fill;
                }
                else if (value == DockStyle.Bottom)
                {
                    this.Widget.Halign = Align.Fill;
                    this.Widget.Valign = Align.End;
                }
                else if (value == DockStyle.None)
                {
                    this.Widget.Halign = Align.Start;
                    this.Widget.Valign = Align.Start;
                }
                if (DockChanged != null)
                    DockChanged(this, EventArgs.Empty);
            }
        }
        public virtual bool Enabled { get { return this.Widget.Sensitive; } set { this.Widget.Sensitive = value; } }

        public virtual bool Focused { get { return this.Widget.IsFocus; } }
        private Font _Font;
        public virtual Font Font {
            get
            {
                if (_Font == null)
                {
                    var fontdes = this.Widget.PangoContext.FontDescription;
                    int size = Convert.ToInt32(fontdes.Size / Pango.Scale.PangoScale);
                    return new Drawing.Font(new Drawing.FontFamily(fontdes.Family), size);
                }
                else
                    return _Font;
            }
            set { _Font = value; UpdateStyle(); }
        }
        private Color _ForeColor;
        public virtual Color ForeColor { 
            get { return _ForeColor; }
            set { _ForeColor = value; UpdateStyle(); } 
        }

        public virtual bool HasChildren { get; }

        public virtual ImeMode ImeMode { get; set; }

        public virtual bool InvokeRequired { get; }

        public virtual bool IsAccessible { get; set; }

        public virtual bool IsDisposed { get; }

        public virtual bool IsHandleCreated { get => true; }

        public virtual bool IsMirrored { get; }

        public virtual LayoutEngine LayoutEngine { get; }
        public virtual int Top
        {
            get => this.Widget.MarginTop;
            set
            {
                this.Widget.MarginTop = value;
                if (DockChanged != null)
                    DockChanged(this, EventArgs.Empty);
                if (AnchorChanged != null)
                    AnchorChanged(this, EventArgs.Empty);
            }
        }
        public virtual int Left
        {
            get => this.Widget.MarginStart;
            set { 
                this.Widget.MarginStart = value;
                if (DockChanged != null)
                    DockChanged(this, EventArgs.Empty);
                if (AnchorChanged != null)
                    AnchorChanged(this, EventArgs.Empty);
            }
        }
        public virtual int Right {
            get => this.Widget.MarginEnd;
        }
        public virtual int Bottom
        {
            get => this.Widget.MarginBottom;
        }
        internal bool LockLocation = false;//由于代码有顺序执行，特殊锁定
        public virtual Point Location
        {
            get
            {
                return new Point(Left, Top);
            }
            set
            {
                if (LockLocation == false)
                {
                    Left = value.X;
                    Top = value.Y;
                }
            }
        }
        public virtual string Name { get { return this.Widget.Name; } set { this.Widget.Name = value; } }
        public virtual Padding Padding { get; set; }
        public virtual Control Parent { get; set; }
        public virtual Size PreferredSize { get; }
        public virtual string ProductName { get; }
        public virtual string ProductVersion { get; }
        public virtual bool RecreatingHandle { get; }
        public virtual Drawing.Region Region { get; set; }

        public virtual RightToLeft RightToLeft { get; set; }
        private Size _size;
        public virtual Size Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                if (AutoSize == false)
                {
                    if (this.Widget is Gtk.Button)
                    {
                        Width = value.Width > 6 ? value.Width - 6 : value.Width;
                        Height = value.Height > 6 ? value.Height - 6 : value.Height;
                    }
                    else
                    {
                        Width = value.Width;
                        Height = value.Height;
                    }
                }
            }
        }
        public virtual int Height
        {
            get
            {
                if (this.Widget.IsMapped == false && this.Widget is Gtk.Window wnd)
                {
                    return wnd.HeightRequest == -1 ? wnd.DefaultHeight : wnd.HeightRequest;
                }
                return this.Widget.HeightRequest == -1 ? this.Widget.AllocatedHeight : this.Widget.HeightRequest;
            }
            set
            {
                this.Widget.HeightRequest = Math.Max(-1, value);
                if (DockChanged != null)
                    DockChanged(this, EventArgs.Empty);
                if (AnchorChanged != null)
                    AnchorChanged(this, EventArgs.Empty);
            }
        }
        public virtual int Width
        {
            get
            {
                if (this.Widget.IsMapped == false && this.Widget is Gtk.Window wnd)
                {
                    return wnd.WidthRequest == -1 ? wnd.DefaultWidth : wnd.WidthRequest;
                }
                return this.Widget.WidthRequest == -1 ? this.Widget.AllocatedWidth : this.Widget.WidthRequest;
            }
            set {
                this.Widget.WidthRequest = Math.Max(-1, value);
                if (DockChanged != null)
                    DockChanged(this, EventArgs.Empty);
                if (AnchorChanged != null)
                    AnchorChanged(this, EventArgs.Empty);
            }
        }
        public virtual int TabIndex { get; set; }
        public virtual bool TabStop { get; set; }
        public virtual object Tag { get; set; }
        public virtual string Text { get; set; }
        public virtual Control TopLevelControl { get; }
        public virtual bool UseWaitCursor { get; set; }
        public virtual bool Visible { get { return this.Widget.Visible; } set { this.Widget.Visible = value; this.Widget.NoShowAll = value == false; } }

        protected virtual Size DefaultSize { get; }
        protected virtual Padding DefaultPadding { get; }
        protected virtual Size DefaultMinimumSize { get; }
        protected virtual Padding DefaultMargin { get; }
        protected virtual Cursor DefaultCursor { get; }
        protected override bool CanRaiseEvents { get; }
        protected virtual bool DoubleBuffered { get; set; }
        protected int FontHeight { get; set; }
        protected virtual Size DefaultMaximumSize { get; }
        protected virtual ImeMode ImeModeBase { get; set; }
        protected virtual ImeMode DefaultImeMode { get; }
        protected virtual bool CanEnableIme { get; }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual bool ScaleChildren { get; }
        protected bool ResizeRedraw { get; set; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected internal virtual bool ShowFocusCues { get; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected internal virtual bool ShowKeyboardCues { get; }


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
        public virtual event EventHandler AnchorChanged;
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
        Cairo.ImageSurface image;
        Cairo.Surface surface;
        Cairo.Context context;
        public virtual Graphics CreateGraphics()
        {
            try
            {
                if (image == null)
                    image = new Cairo.ImageSurface(Cairo.Format.Argb32, this.Widget.AllocatedWidth, this.Widget.AllocatedHeight);

                surface?.Dispose();
                surface = image.CreateSimilar(Cairo.Content.ColorAlpha, this.Widget.AllocatedWidth, this.Widget.AllocatedHeight);
                context?.Dispose();
                context = new Cairo.Context(surface);

                return new Drawing.Graphics(this.Widget, context, this.Widget.Allocation);
            }
            catch(Exception ex) 
            {
                Console.WriteLine("画版创建失败：" + ex.Message);
                throw;
            }
        }

        private void Override_PaintGraphics(Cairo.Context cr, Rectangle rec)
        {
            if (surface != null)
            {
                cr.Save();
                cr.SetSourceSurface(surface, 0, 0);
                cr.Paint();
                cr.Restore();
            }
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
            if (this.Widget.Toplevel.Data.ContainsKey("Control"))
            {
                return this.Widget.Toplevel.Data["Control"] as Form;
            }
            else
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
            return this as IContainerControl;
        }

        public virtual Control GetNextControl(Control ctl, bool forward)
        {
            Control prev = null;
            Control next= null;
            bool finded = false;
 
            foreach(var obj in this.Controls) 
            {
                if (obj is Control control)
                {
                    if (finded == true)
                        next = control;

                    if (control.Widget.Handle == ctl.Widget.Handle)
                    {
                        finded = true;
                    } 
                    if (finded == true)
                    {
                        if (forward == false && prev != null)
                        {
                            return prev;
                        }
                        else if (forward == true && next != null)
                        {
                            return next;
                        }
                    }
                    prev = control;
                }
            }

            return null;
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
            Invalidate(new Rectangle(Widget.Allocation.X, Widget.Allocation.Y, Widget.Allocation.Width, Widget.Allocation.Height), invalidateChildren);
        }

        public virtual void Invalidate(Rectangle rc)
        {
            Invalidate(rc, true);
        }

        public virtual void Invalidate(Rectangle rc, bool invalidateChildren)
        {
            if (this.Widget != null)
            {
                if (ISelf != null)
                    ISelf.Override.OnAddClass();
                this.Widget.Window.InvalidateRect(new Gdk.Rectangle(rc.X, rc.Y, rc.Width, rc.Height), invalidateChildren);
                if (invalidateChildren == true && this.Widget is Gtk.Container container)
                {
                    foreach (var child in container.Children)
                        child.Window.InvalidateRect(child.Allocation, invalidateChildren);
                }
                Refresh();
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
                if (ISelf != null)
                    ISelf.Override.OnAddClass();
                this.Widget.Window.InvalidateRect(Widget.Allocation, invalidateChildren);
            }
        }

        public virtual object Invoke(Delegate method)
        {
            return Invoke(method, null);
        }

        public virtual object Invoke(Delegate method, params object[] args)
        {
            object result = null;
            GLib.Idle.Add(() => {
                result = method.DynamicInvoke(args);
                return false;
            });
            return result;
        }
        public virtual void Invoke(Action method)
        {
            GLib.Idle.Add(() => {
                method.Invoke();
                return false;
            });
        }
        public virtual ENTRY Invoke<ENTRY>(Func<ENTRY> method)
        {
            ENTRY result = default(ENTRY);
            GLib.Idle.Add(() => {
                result = method.Invoke();
                return false;
            });
            return result;
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
                this.Widget.Window.GetOrigin(out int x, out int y);
                if (p.X > x && p.Y > y)
                    return new Point(p.X - x, p.Y - y);
            }
            return new Point(p.X, p.Y);
        }

        public virtual Point PointToScreen(Point p)
        {
            if (Widget != null)
            {
                this.Widget.Window.GetOrigin(out int x, out int y);
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
                this.Widget.Window.GetPosition(out int x, out int y);
                if (r.X > x && r.Y > y)
                    return new Rectangle(r.X - x, r.Y - y, r.Width, r.Height);
            }
            return new Rectangle(r.X, r.Y, r.Width, r.Height);
        }

        public virtual Rectangle RectangleToScreen(Rectangle r)
        {
            if (Widget != null)
            {
                this.Widget.Window.GetPosition(out int x, out int y);
                if (r.X < x && r.Y < y)
                    return new Rectangle(r.X + x, r.Y + y, r.Width, r.Height);
            }
            return new Rectangle(r.X, r.Y, r.Width, r.Height);
        }

        public virtual void Refresh()
        {
            if (this.Widget != null && this.Widget.IsVisible)
            {
                if (ISelf != null)
                    ISelf.Override.OnAddClass();
                this.Widget.QueueDraw();
                if (this.Widget is Gtk.Container container)
                {
                    foreach (var child in container.Children)
                        child.QueueDraw();
                }
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
            ResumeLayout(false);
        }

        public virtual void ResumeLayout(bool performLayout)
        {
            _Created = true; 
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
        public virtual Rectangle ClientRectangle { get { this.Widget.GetAllocatedSize(out Gdk.Rectangle allocation, out int baseline); return new Rectangle(allocation.X, allocation.Y, allocation.Width, allocation.Height); } }

        public virtual Size ClientSize { get { return new Size(Widget.AllocatedWidth, this.Widget.AllocatedHeight); } set { this.Widget.SetSizeRequest(value.Width, value.Height); } }

        public virtual IntPtr Handle { get => this.Widget == null ? IntPtr.Zero : this.Widget.Handle; }
        public virtual Padding Margin { get; set; }
        public virtual Size MaximumSize { get; set; }
        public virtual Size MinimumSize { get; set; }
        private BorderStyle _BorderStyle;
        public virtual BorderStyle BorderStyle
        {
            get { return _BorderStyle; }
            set {
                _BorderStyle = value;
                if(value==BorderStyle.FixedSingle)
                {
                    this.Widget.StyleContext.RemoveClass("BorderFixed3D");
                    this.Widget.StyleContext.RemoveClass("BorderNone");
                    this.Widget.StyleContext.AddClass("BorderFixedSingle");
                }
                else if (value == BorderStyle.Fixed3D)
                {
                    this.Widget.StyleContext.RemoveClass("BorderFixedSingle");
                    this.Widget.StyleContext.RemoveClass("BorderNone");
                    this.Widget.StyleContext.AddClass("BorderFixed3D");
                }
                else
                {
                    this.Widget.StyleContext.RemoveClass("BorderFixedSingle");
                    this.Widget.StyleContext.RemoveClass("BorderFixed3D");
                    this.Widget.StyleContext.AddClass("BorderNone");
                }
            }
        }

        public virtual void Hide()
        {
            if (this.GtkControl is Misc con)
            {
                con.Hide();
            }
        }

        public virtual void Show()
        {
            if (this.Widget != null)
            {
                this.Widget.ShowAll();
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
                if (image != null)
                    image.Dispose();
                if(surface != null) 
                    surface.Dispose();
                if(context != null) 
                    context.Dispose();

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

        public bool ParticipatesInLayout => false;

        PropertyStore IArrangedElement.Properties => throw new NotImplementedException();

        IArrangedElement IArrangedElement.Container => throw new NotImplementedException();

        private ArrangedElementCollection arrangedElementCollection;
        public ArrangedElementCollection Children => arrangedElementCollection;

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

        public void SetBounds(Rectangle bounds, BoundsSpecified specified)
        {
            throw new NotImplementedException();
        }

        void IArrangedElement.PerformLayout(IArrangedElement affectedElement, string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
