/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms.Design;
using System.Windows.Forms.Interfaces;
using Cairo;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using Graphics = System.Drawing.Graphics;
using Image = System.Drawing.Image;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using Region = System.Drawing.Region;
using Size = System.Drawing.Size;

namespace System.Windows.Forms;

[DefaultEvent("Click")]
[DefaultProperty("Text")]
[Designer(typeof(ControlDesigner))]
[ToolboxItemFilter("System.Windows.Forms")]
public partial class Control : Component, IControl, ISynchronizeInvoke, ISupportInitialize, IArrangedElement, IBindableComponent
{
    public Gtk.Application Application { get; } = Forms.Application.Init();
    public string? UniqueKey { get; protected set; }

    public virtual IWidget Widget => (IWidget)GtkControl!;
    public virtual IControlGtk? Self => (IControlGtk)GtkControl!;

    public virtual object? GtkControl { get; set; }

    public static event EventHandler? BeforeInit;
    private Cursor? cursor;
    private ImeMode imeMode;
    private ContextMenuStrip? _contextMenuStrip;
    private Padding margin;
    private bool _capture;
    private bool causesValidation;
    private bool _handleCreated;
    private AccessibleObject? accessibilityObject;
    private BindingContext? bindingContext;

    public Control()
    {
        BeforeInit?.Invoke(this, EventArgs.Empty);
        Init();
    }

    private ControlStyles controlStyle;

    protected bool GetStyle(ControlStyles flag)
    {
        return (controlStyle & flag) == flag;
    }

    private void Init()
    {
        Disposed += Control_Disposed;
        Controls = new ControlCollection(this);
        DataBindings = new ControlBindingsCollection(this);
        Enabled = true;
        UniqueKey = Guid.NewGuid().ToString().ToLower();
        if (Widget != null)
        {
            if (Widget is Window)
            {
                Widget.Halign = Align.Fill;
                Widget.Valign = Align.Fill;
            }
            else
            {
                Widget.Halign = Align.Start;
                Widget.Valign = Align.Start;
                Widget.Expand = false;
            }
            Widget.Data["Control"] = this;
            Widget.StyleContext.AddClass("DefaultThemeStyle");
            Widget.ButtonPressEvent += Widget_ButtonPressEvent;
            Widget.ButtonReleaseEvent += Widget_ButtonReleaseEvent;
            Widget.EnterNotifyEvent += Widget_EnterNotifyEvent;
            Widget.MotionNotifyEvent += Widget_MotionNotifyEvent;
            Widget.LeaveNotifyEvent += Widget_LeaveNotifyEvent;
            Widget.ScrollEvent += Widget_ScrollEvent;
            Widget.FocusInEvent += Widget_FocusInEvent;
            Widget.FocusOutEvent += Widget_FocusOutEvent;
            Widget.KeyPressEvent += Widget_KeyPressEvent;
            Widget.KeyReleaseEvent += Widget_KeyReleaseEvent;
            Widget.Realized += Widget_Realized;
            Widget.ConfigureEvent += Widget_ConfigureEvent;
            Self.Override.PaintGraphics += Override_PaintGraphics;
            Widget.SizeAllocated += Widget_SizeAllocated;

            SetStyle(ControlStyles.ContainerControl, false);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.FixedWidth, false);
            SetStyle(ControlStyles.FixedHeight, false);
            SetStyle(ControlStyles.StandardClick, false);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.UserMouse, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.StandardDoubleClick, false);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.CacheText, true);
            SetStyle(ControlStyles.EnableNotifyMessage, false);
            SetStyle(ControlStyles.DoubleBuffer, false);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UseTextForAccessibility, true);
        }
    }

    private void Control_Disposed(object? sender, EventArgs e)
    {
        if (!IsDisposed)
        {
            Disposed?.Invoke(this, e);
        }
    }

    private int sizeWidth;
    private int sizeHeight;
    private int locationX;
    private int locationY;
    private void Widget_SizeAllocated(object? o, SizeAllocatedArgs args)
    {
        if (args.Allocation.Width != sizeWidth || args.Allocation.Height != sizeHeight)
        {
            sizeWidth = args.Allocation.Width;
            sizeHeight = args.Allocation.Height;
            SizeChanged?.Invoke(this, EventArgs.Empty);
        }
        if (args.Allocation.X != locationX || args.Allocation.Y != locationY)
        {
            locationX = args.Allocation.X;
            locationY = args.Allocation.Y;
            LocationChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    private void Widget_ConfigureEvent(object? o, ConfigureEventArgs args)
    {
        Move?.Invoke(this, args);
    }
    #region events
    private bool widgetRealized;
    private void Widget_Realized(object? sender, EventArgs e)
    {
        if (widgetRealized == false)
        {
            widgetRealized = true;
            InitStyle((Widget)sender!);
            OnLoad(e);
            foreach (Control control in Controls)
            {
                control.OnLoad(e);
            }
        }
    }

    protected internal void OnBindingContextChanged(EventArgs e)
    {
        BindingContextChanged?.Invoke(this, e);
    }

    private bool _loaded;

    protected internal virtual void OnLoad(EventArgs e)
    {
        if (_loaded)
        {
            return;
        }
        _loaded = true;
        Load?.Invoke(this, e);
        if (!bindingContextSet)
        {
            OnBindingContextChanged(e);
        }
    }

    private void Widget_ButtonPressEvent(object? o, ButtonPressEventArgs args)
    {
        //Console.WriteLine($"Widget_ButtonPressEvent1:{args.Event.XRoot},{args.Event.YRoot};{args.Event.X},{args.Event.Y}");
        var result = MouseButtons.None;
        if (args.Event.Button == 1)
            result = MouseButtons.Left;
        else if (args.Event.Button == 2)
            result = MouseButtons.Middle;
        else if (args.Event.Button == 3)
            result = MouseButtons.Right;

        var owidget = o as Widget;
        if (owidget != null)
        {
            owidget.Window.GetOrigin(out var x, out var y); //避免事件穿透错误
            MouseDown?.Invoke(this, new MouseEventArgs(result, 1, (int)args.Event.XRoot - x, (int)args.Event.YRoot - y, 0));
            if (args.Event.Type == Gdk.EventType.TwoButtonPress || args.Event.Type == Gdk.EventType.DoubleButtonPress)
            {
                MouseDoubleClick?.Invoke(this, new MouseEventArgs(result, 2, (int)args.Event.XRoot - x, (int)args.Event.YRoot - y, 0));
                DoubleClick?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Click?.Invoke(this, EventArgs.Empty);
                MouseClick?.Invoke(this, new MouseEventArgs(result, 1, (int)args.Event.XRoot - x, (int)args.Event.YRoot - y, 0));
            }
        }

    }
    private void Widget_ButtonReleaseEvent(object? o, ButtonReleaseEventArgs args)
    {
        if (MouseUp != null)
        {
            var result = MouseButtons.None;
            if (args.Event.Button == 1)
                result = MouseButtons.Left;
            else if (args.Event.Button == 2)
                result = MouseButtons.Middle;
            else if (args.Event.Button == 3)
                result = MouseButtons.Right;
            var owidget = (Widget)o!;
            owidget.Window.GetOrigin(out var x, out var y);
            MouseUp?.Invoke(this, new MouseEventArgs(result, 1, (int)args.Event.XRoot - x, (int)args.Event.YRoot - y, 0));
        }

        if (ContextMenuStrip != null)
        {
            if (args.Event.Button == 3)
            {
                ContextMenuStrip.Widget.ShowAll();
                ((Menu)ContextMenuStrip.Widget).PopupAtPointer(args.Event);
            }
        }
    }

    private void Widget_EnterNotifyEvent(object? o, EnterNotifyEventArgs args)
    {
        if (Cursor != null)
        {
            if (Cursor.CursorType == Gdk.CursorType.CursorIsPixmap)
            {
                if (o != null)
                {
                    Widget.Window.Cursor = new Gdk.Cursor(((Widget)o).Display, Cursor.CursorPixbuf, Cursor.CursorsXy.X,
                        Cursor.CursorsXy.Y);
                }
            }
            else
            {
                if (o != null)
                {
                    Widget.Window.Cursor = new Gdk.Cursor(((Widget)o).Display, Cursor.CursorType);
                }
            }
        }

        Enter?.Invoke(this, args);
        MouseEnter?.Invoke(this, args);

        MouseHover?.Invoke(this, args);
    }
    private void Widget_MotionNotifyEvent(object? o, MotionNotifyEventArgs args)
    {
        MouseMove?.Invoke(this, new MouseEventArgs(MouseButtons.None, 1, (int)args.Event.X, (int)args.Event.Y, 0));
    }
    private void Widget_LeaveNotifyEvent(object? o, LeaveNotifyEventArgs args)
    {
        if (Cursor != null)
        {
            Widget.Window.Cursor = null;
        }

        Leave?.Invoke(this, args);
        MouseLeave?.Invoke(this, args);


    }
    private void Widget_ScrollEvent(object? o, Gtk.ScrollEventArgs args)
    {
        MouseWheel?.Invoke(this, new MouseEventArgs(MouseButtons.None, 0, (int)args.Event.X, (int)args.Event.Y, (int)args.Event.DeltaY));
    }
    private void Widget_FocusInEvent(object? o, FocusInEventArgs args)
    {
        GotFocus?.Invoke(this, args);
    }
    private void Widget_FocusOutEvent(object? o, FocusOutEventArgs args)
    {
        LostFocus?.Invoke(this, args);

        Validating?.Invoke(this, cancelEventArgs);
        if (Validated != null && cancelEventArgs.Cancel == false)
            Validated?.Invoke(this, cancelEventArgs);
    }
    private void Widget_KeyPressEvent(object? o, Gtk.KeyPressEventArgs args)
    {
        if (KeyDown != null)
        {
            if (args.Event is Gdk.EventKey eventkey)
            {
                var keys = (Keys)eventkey.HardwareKeycode;
                if (eventkey.State.HasFlag(Gdk.ModifierType.Mod1Mask))
                    keys |= Keys.Alt;
                if (eventkey.State.HasFlag(Gdk.ModifierType.ControlMask))
                    keys |= Keys.Control;
                if (eventkey.State.HasFlag(Gdk.ModifierType.ShiftMask))
                    keys |= Keys.Shift;
                if (eventkey.State.HasFlag(Gdk.ModifierType.LockMask))
                    keys |= Keys.CapsLock;

                KeyDown?.Invoke(this, new KeyEventArgs(keys));
            }
        }
    }
    private void Widget_KeyReleaseEvent(object? o, KeyReleaseEventArgs args)
    {
        if (KeyUp != null)
        {
            var keys = (Keys)args.Event.HardwareKeycode;
            KeyUp?.Invoke(this, new KeyEventArgs(keys));
        }
        if (KeyPress != null)
        {
            var keys = (Keys)args.Event.HardwareKeycode;
            KeyPress?.Invoke(this, new KeyPressEventArgs(Convert.ToChar(keys)));
        }
    }

    #endregion

    //===================
    protected virtual void InitStyle(Widget widget)
    {
        SetStyle(widget);
    }
    protected virtual void UpdateStyle()
    {
        if (Widget is { IsMapped: true })
        {
            var widget = Widget as Widget;
            if (widget != null) SetStyle(widget);
        }
    }
    protected virtual void UpdateBackgroundStyle()
    {
        if (Widget is { IsMapped: true })
            Self.Override.OnAddClass();
    }
    protected virtual void SetStyle(Widget widget)
    {
        var style = new StringBuilder();
        if (widget is Gtk.Image) { }
        else
        {
            if (Image is { PixbufData: not null })
            {
                var imguri = $"Resources/{widget.WidgetPath.IterGetName(0)}${widget.Name}_img.png";
                if (!File.Exists(imguri))
                {
                    var imagepixbuf = new Gdk.Pixbuf(Image.PixbufData);
                    imagepixbuf.Save(imguri, "png");
                }
                style.AppendFormat("background:url(\"{0}\")", imguri);
                style.Append(" no-repeat");
                if (ImageAlign == ContentAlignment.TopLeft)
                {
                    style.Append(" top left");
                }
                else if (ImageAlign == ContentAlignment.TopCenter)
                {
                    style.Append(" top center");
                }
                else if (ImageAlign == ContentAlignment.TopRight)
                {
                    style.Append(" top right");
                }
                else if (ImageAlign == ContentAlignment.MiddleLeft)
                {
                    style.Append(" center left");
                }
                else if (ImageAlign == ContentAlignment.MiddleCenter)
                {
                    style.Append(" center center");
                }
                else if (ImageAlign == ContentAlignment.MiddleRight)
                {
                    style.Append(" center right");
                }
                else if (ImageAlign == ContentAlignment.BottomLeft)
                {
                    style.Append(" bottom left");
                }
                else if (ImageAlign == ContentAlignment.BottomCenter)
                {
                    style.Append(" bottom center");
                }
                else if (ImageAlign == ContentAlignment.BottomRight)
                {
                    style.Append(" bottom right");
                }
                else
                {
                    style.Append(" center center");
                }

                if (BackgroundImage is { PixbufData: not null })
                {
                    var bgimguri = $"Resources/{widget.WidgetPath.IterGetName(0)}${widget.Name}_bg.png";
                    if (!File.Exists(bgimguri))
                    {
                        var bgpixbuf = new Gdk.Pixbuf(BackgroundImage.PixbufData);
                        bgpixbuf.Save(bgimguri, "png");
                    }

                    style.AppendFormat(",url(\"Resources/{0}_bg.png\") repeat", widget.Name);
                }
                style.Append(";");
                style.Append("background-origin: padding-box;");
                style.Append("background-clip: padding-box;");
            }
            else if (BackgroundImage is { PixbufData: not null })
            {
                var bgpixbuf = new Gdk.Pixbuf(BackgroundImage.PixbufData);
                var bgimguri = $"Resources/{widget.WidgetPath.IterGetName(0)}${widget.Name}_bg.png";
                if (!File.Exists(bgimguri))
                {
                    bgpixbuf.Save(bgimguri, "png");
                }
                style.AppendFormat("background-image:url(\"{0}\");", bgimguri);
                if (BackgroundImageLayout == ImageLayout.Tile)
                {
                    style.Append("background-repeat:repeat;");
                }
                else if (BackgroundImageLayout == ImageLayout.Zoom)
                {
                    style.Append("background-repeat:no-repeat;");
                    style.Append("background-size: contain;");
                    style.Append("background-position:center;");
                }
                else if (BackgroundImageLayout == ImageLayout.Stretch)
                {
                    style.Append("background-repeat:no-repeat;");
                    style.Append("background-size: cover;");
                    style.Append("background-position:center;");
                }
                else if (BackgroundImageLayout == ImageLayout.Center)
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
                if (BackColor.Name != "0")
                {
                    var backColor = BackColor;
                    var color = $"rgba({backColor.R},{backColor.G},{backColor.B},{backColor.A})";
                    style.AppendFormat("background-color:{0};", color);
                }
            }
            else if (BackColor.Name != "0")
            {
                var backColor = BackColor;
                var color = $"rgba({backColor.R},{backColor.G},{backColor.B},{backColor.A})";
                style.AppendFormat("background-color:{0};background:{0};", color);
            }

            if (ForeColor.Name != "0")
            {
                var foreColor = ForeColor;
                var color = $"rgba({foreColor.R},{foreColor.G},{foreColor.B},{foreColor.A})";
                style.AppendFormat("color:{0};", color);
            }
            if (Font != null)
            {
                var font = Font;
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
                var styleClassName = $"s{UniqueKey}";
                var css = new StringBuilder();
                css.AppendLine($".{styleClassName}{{{style}}}");
                if (widget is TextView)
                {
                    css.AppendLine($".{styleClassName} text{{{style}}}");
                    css.AppendLine($".{styleClassName} .view{{{style}}}");
                }
                var provider = new CssProvider();
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
        controlStyle = value ? controlStyle | styles : controlStyle & ~styles;
    }

    #region 背景
    public virtual Image? Image { get; set; }
    public virtual ContentAlignment ImageAlign { get; set; }

    public virtual bool UseVisualStyleBackColor { get; set; } = true;
    public virtual Color VisualStyleBackColor { get; }

    public virtual ImageLayout BackgroundImageLayout
    {
        get => Self == null ? ImageLayout.None : Self.Override.BackgroundImageLayout;
        set
        {
            if (Self != null)
            {
                var layout = Self.Override.BackgroundImageLayout;
                Self.Override.BackgroundImageLayout = value;
                if (layout != value)
                {
                    BackgroundImageLayoutChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    public virtual Image? BackgroundImage
    {
        get => Self == null ? null : Self.Override.BackgroundImage;
        set
        {
            if (Self != null)
            {
                var overrideBackgroundImage = Self.Override.BackgroundImage;
                Self.Override.BackgroundImage = value;
                Refresh();
                if (overrideBackgroundImage != value)
                {
                    BackgroundImageChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
    public virtual Color BackColor
    {
        get
        {
            if (Self.Override.BackColor.HasValue)
                return Self.Override.BackColor.Value;
            //else if (UseVisualStyleBackColor)
            //    return Color.FromName("0");
            //else
            //    return Color.Transparent; 
            return Color.FromName("0");
        }
        set
        {
            var overrideBackColor = Self.Override.BackColor;
            Self.Override.BackColor = value;
            if (overrideBackColor != value)
            {
                BackColorChanged?.Invoke(this, EventArgs.Empty);
            }
            Self.Override.OnAddClass();
            UpdateStyle();
            Refresh();
        }
    }
    public virtual event PaintEventHandler? Paint
    {
        add => Self.Override.Paint += value;
        remove => Self.Override.Paint -= value;
    }
    #endregion

    public virtual AccessibleObject? AccessibilityObject
    {
        get
        {
            _handleCreated = true;
            return accessibilityObject;
        }
        set => accessibilityObject = value;
    }

    public virtual string? AccessibleDefaultActionDescription { get; set; }
    public virtual string? AccessibleDescription { get; set; }
    public virtual string? AccessibleName { get; set; }
    public virtual AccessibleRole AccessibleRole { get; set; }
    public virtual bool AllowDrop { get; set; }
    private AnchorStyles _anchor;
    public virtual AnchorStyles Anchor
    {
        get => _anchor;
        set
        {
            _anchor = value;
            if (Widget is Widget widget) SetAnchorStyles(widget, _anchor);

            AnchorChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    private void SetAnchorStyles(Widget widget, AnchorStyles anchorStyles)
    {
        if (anchorStyles.HasFlag(AnchorStyles.Left) && anchorStyles.HasFlag(AnchorStyles.Right))
        {
            widget.Halign = Align.Fill;
        }
        else if (anchorStyles.HasFlag(AnchorStyles.Left))
        {
            widget.Halign = Align.Start;
        }
        else if (anchorStyles.HasFlag(AnchorStyles.Right))
        {
            widget.Halign = Align.End;
        }
        else
        {
            widget.Halign = Align.Start;
        }

        if (anchorStyles.HasFlag(AnchorStyles.Top) && anchorStyles.HasFlag(AnchorStyles.Bottom))
        {
            widget.Valign = Align.Fill;
        }
        else if (anchorStyles.HasFlag(AnchorStyles.Top))
        {
            widget.Valign = Align.Start;
        }
        else if (anchorStyles.HasFlag(AnchorStyles.Bottom))
        {
            widget.Valign = Align.End;
        }
        else
        {
            widget.Valign = Align.Start;
        }
    }
    public virtual Point AutoScrollOffset { get; set; }
    private bool _autoSize;
    public virtual bool AutoSize
    {
        get => _autoSize;
        set
        {
            var autoSize = _autoSize;
            _autoSize = value;
            if (autoSize != value)
            {
                AutoSizeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    internal bool bindingContextSet;

    public virtual BindingContext? BindingContext
    {
        get => bindingContext;
        set
        {
            if (bindingContext != value)
            {
                var e = EventArgs.Empty;
                OnBindingContextChanged(e);
            }
            bindingContext = value;
            bindingContextSet = true;
        }
    }

    public virtual Rectangle Bounds
    {
        get => new(Widget.Clip.X, Widget.Clip.Y, Widget.Clip.Width, Widget.Clip.Height);
        set
        {
            var r = new Rectangle(Widget.Clip.X, Widget.Clip.Y, Widget.Clip.Width,
                Widget.Clip.Height);
            SetBounds(value.X, value.Y, value.Width, value.Height);
            if (r != value)
            {
                Layout?.Invoke(this, new LayoutEventArgs(this, nameof(Bounds)));
                Resize?.Invoke(this, EventArgs.Empty);
                SizeChanged?.Invoke(this, EventArgs.Empty);
                ClientSizeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public virtual bool CanFocus => Widget.CanFocus;

    public virtual bool CanSelect { get; }

    public virtual bool Capture
    {
        get => _capture;
        set
        {
            _capture = value;
            _handleCreated = true;
            Handle = IntPtr.Zero;
        }
    }

    public virtual bool CausesValidation
    {
        get => causesValidation;
        set
        {
            var validation = causesValidation;
            causesValidation = value;
            if (validation != value)
            {
                CausesValidationChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public virtual string? CompanyName { get; }

    public virtual bool ContainsFocus { get; }

    public virtual ContextMenuStrip? ContextMenuStrip
    {
        get => _contextMenuStrip;
        set
        {
            var menuStrip = _contextMenuStrip;
            _contextMenuStrip = value;
            if (menuStrip != value)
            {
                ContextMenuStripChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public virtual ControlCollection? Controls { get; set; }

    public virtual bool Created => created;
    internal bool created;

    public virtual Cursor? Cursor
    {
        get => cursor;
        set
        {
            var c = cursor;
            cursor = value;
            if (c != value)
            {
                CursorChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public virtual ControlBindingsCollection? DataBindings { get; set; }

    public virtual int DeviceDpi { get; }

    public virtual Rectangle DisplayRectangle { get; }

    public virtual bool Disposing { get; }
    private DockStyle _dock;
    public virtual DockStyle Dock
    {
        get => _dock;
        set
        {
            var dockStyle = _dock;
            _dock = value;
            var widget = Widget;
            if (value == DockStyle.Fill)
            {
                widget.Halign = Align.Fill;
                widget.Valign = Align.Fill;
            }
            else if (value == DockStyle.Left)
            {
                widget.Halign = Align.Start;
                widget.Valign = Align.Fill;
            }
            else if (value == DockStyle.Top)
            {
                widget.Halign = Align.Fill;
                widget.Valign = Align.Start;
            }
            else if (value == DockStyle.Right)
            {
                widget.Halign = Align.End;
                widget.Valign = Align.Fill;
            }
            else if (value == DockStyle.Bottom)
            {
                widget.Halign = Align.Fill;
                widget.Valign = Align.End;
            }
            else if (value == DockStyle.None)
            {
                widget.Halign = Align.Start;
                widget.Valign = Align.Start;
            }
            if (dockStyle != value)
                DockChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public virtual bool Enabled
    {
        get => Widget.Sensitive;
        set
        {
            var sensitive = false;
            if (Widget != null)
            {
                sensitive = Widget.Sensitive;
                Widget.Sensitive = value;
            }

            if (sensitive != value)
            {
                EnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public virtual bool Focused => Widget.IsFocus;
    private Font? font;
    public virtual Font? Font
    {
        get
        {
            if (font == null)
            {
                var fontdes = Widget.PangoContext.FontDescription;
                var size = Convert.ToInt32(fontdes.Size / Pango.Scale.PangoScale);
                return new Font(new FontFamily(fontdes.Family), size);
            }

            return font;
        }
        set
        {
            var font = this.font;
            this.font = value; UpdateStyle();
            if (font != value)
            {
                FontChanged?.Invoke(this, EventArgs.Empty);
                Layout?.Invoke(this, new LayoutEventArgs(this, nameof(Font)));
            }
        }
    }
    private Color foreColor;
    public virtual Color ForeColor
    {
        get => foreColor;
        set
        {
            var foreColor = this.foreColor;
            this.foreColor = value; UpdateStyle();
            if (foreColor != value)
            {
                ForeColorChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public virtual bool HasChildren { get; }

    public virtual ImeMode ImeMode
    {
        get => imeMode;
        set
        {
            var mode = imeMode;
            imeMode = value;
            if (mode != value)
            {
                ImeModeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public virtual bool InvokeRequired { get; }

    public virtual bool IsAccessible { get; set; }

    public virtual bool IsDisposed { get; internal set; }

    public virtual bool IsHandleCreated
    {
        get => _handleCreated;
        set => _handleCreated = value;
    }

    public virtual bool IsMirrored { get; }

    public virtual LayoutEngine? LayoutEngine { get; }
    public virtual string? Text { get; set; }

    public virtual int Top
    {
        get => Widget.MarginTop;
        set
        {
            var widget = Widget;
            var marginTop = widget?.MarginTop ?? 0;
            if (widget != null)
            {
                widget.MarginTop = value;
            }
            if (marginTop != value)
            {
                LocationChanged?.Invoke(this, EventArgs.Empty);
            }

            DockChanged?.Invoke(this, EventArgs.Empty);
            AnchorChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public virtual int Left
    {
        get => Widget.MarginStart;
        set
        {
            var widget = Widget;
            var marginStart = widget?.MarginStart ?? 0;
            if (widget != null)
            {
                widget.MarginStart = value;
            }

            if (marginStart != value)
            {
                Move?.Invoke(this, EventArgs.Empty);
                LocationChanged?.Invoke(this, EventArgs.Empty);
            }

            DockChanged?.Invoke(this, EventArgs.Empty);
            AnchorChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public virtual int Right => Widget.MarginEnd;

    public virtual int Bottom => Widget.MarginBottom;
    internal bool LockLocation = false;//由于代码有顺序执行，特殊锁定
    public virtual Point Location
    {
        get => new(Left, Top);
        set
        {
            if (LockLocation == false)
            {
                Left = value.X;
                Top = value.Y;
            }
        }
    }
    public virtual string? Name
    {
        get => Widget.Name;
        set
        {
            var widget = Widget;
            if (widget != null)
            {
                widget.Name = value;
            }
        }
    }
    public virtual Padding Padding { get; set; }
    public virtual Control? Parent { get; set; }
    public virtual Size PreferredSize { get; }
    public virtual string? ProductName { get; }
    public virtual string? ProductVersion { get; }
    public virtual bool RecreatingHandle { get; }
    public virtual Region? Region { get; set; }

    public virtual RightToLeft RightToLeft { get; set; }
    private Size _size;
    public virtual Size Size
    {
        get => _size;
        set
        {
            _size = value;
            if (AutoSize == false)
            {
                if (Widget is Gtk.Button)
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

    public virtual int TabIndex { get; set; }
    public virtual bool TabStop { get; set; }
    public object? Tag { get; set; }

    public virtual int Height
    {
        get
        {
            if (Widget.IsMapped == false && Widget is Window wnd)
            {
                return wnd.HeightRequest == -1 ? wnd.DefaultHeight : wnd.HeightRequest;
            }
            return Widget.HeightRequest == -1 ? Widget.AllocatedHeight : Widget.HeightRequest;
        }
        set
        {
            Widget.HeightRequest = Math.Max(-1, value);
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
            if (Widget.IsMapped == false && Widget is Window wnd)
            {
                return wnd.WidthRequest == -1 ? wnd.DefaultWidth : wnd.WidthRequest;
            }
            return Widget.WidthRequest == -1 ? Widget.AllocatedWidth : Widget.WidthRequest;
        }
        set => Widget.WidthRequest = value;
    }

    public virtual Control? TopLevelControl { get; }
    public virtual bool UseWaitCursor { get; set; }

    public virtual bool Visible
    {
        get => Widget.Visible;
        set
        {
            var widget = Widget;
            var visible = widget?.Visible ?? true;
            if (widget != null)
            {
                widget.Visible = value;
                widget.NoShowAll = value == false;
            }

            if (visible != value)
            {
                VisibleChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    protected virtual Size DefaultSize { get; }
    protected virtual Padding DefaultPadding { get; }
    protected virtual Size DefaultMinimumSize { get; }
    protected virtual Padding DefaultMargin { get; }
    protected virtual Cursor? DefaultCursor { get; }
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


    public virtual IWindowTarget? WindowTarget { get; set; }
    public virtual event EventHandler? AutoSizeChanged;
    public virtual event EventHandler? BackColorChanged;
    public virtual event EventHandler? BackgroundImageChanged;
    public virtual event EventHandler? BackgroundImageLayoutChanged;
    public virtual event EventHandler? BindingContextChanged;
    public virtual event EventHandler? CausesValidationChanged;
    public virtual event UiCuesEventHandler? ChangeUiCues;
    public virtual event EventHandler? Click;
    public virtual event EventHandler? ClientSizeChanged;
    public virtual event EventHandler? ContextMenuStripChanged;
    public virtual event ControlEventHandler? ControlAdded;
    public virtual event ControlEventHandler? ControlRemoved;
    public virtual event EventHandler? CursorChanged;
    public virtual event EventHandler? DockChanged;
    public virtual event EventHandler? AnchorChanged;
    public virtual event EventHandler? DoubleClick;
    public virtual event EventHandler? DpiChangedAfterParent;
    public virtual event EventHandler? DpiChangedBeforeParent;
    public virtual event DragEventHandler? DragDrop;
    public virtual event DragEventHandler? DragEnter;
    public virtual event EventHandler? DragLeave;
    public virtual event DragEventHandler? DragOver;
    public virtual event EventHandler? EnabledChanged;
    public virtual event EventHandler? Enter;
    public virtual event EventHandler? FontChanged;
    public virtual event EventHandler? ForeColorChanged;
    public virtual event GiveFeedbackEventHandler? GiveFeedback;
    public virtual event EventHandler? GotFocus;
    public virtual event EventHandler? HandleCreated;
    public virtual event EventHandler? HandleDestroyed;
    public virtual event HelpEventHandler? HelpRequested;
    public virtual event EventHandler? ImeModeChanged;
    public virtual event InvalidateEventHandler? Invalidated;
    public virtual event KeyEventHandler? KeyDown;
    public virtual event KeyPressEventHandler? KeyPress;
    public virtual event KeyEventHandler? KeyUp;
    public virtual event LayoutEventHandler? Layout;
    public virtual event EventHandler? Leave;
    public virtual event EventHandler? LocationChanged;
    public virtual event EventHandler? LostFocus;
    public virtual event EventHandler? MarginChanged;
    public virtual event EventHandler? MouseCaptureChanged;
    public virtual event MouseEventHandler? MouseClick;
    public virtual event MouseEventHandler? MouseDoubleClick;
    public virtual event MouseEventHandler? MouseDown;
    public virtual event EventHandler? MouseEnter;
    public virtual event EventHandler? MouseHover;
    public virtual event EventHandler? MouseLeave;
    public virtual event MouseEventHandler? MouseMove;
    public virtual event MouseEventHandler? MouseUp;
    public virtual event MouseEventHandler? MouseWheel;
    public virtual event EventHandler? Move;
    public virtual event EventHandler? PaddingChanged;
    //public virtual event PaintEventHandler? Paint;
    public virtual event EventHandler? ParentChanged;
    public virtual event PreviewKeyDownEventHandler? PreviewKeyDown;
    public virtual event QueryAccessibilityHelpEventHandler? QueryAccessibilityHelp;
    public virtual event QueryContinueDragEventHandler? QueryContinueDrag;
    public virtual event EventHandler? RegionChanged;
    public virtual event EventHandler? Resize;
    public virtual event EventHandler? RightToLeftChanged;
    public virtual event EventHandler? SizeChanged;
    public virtual event EventHandler? StyleChanged;
    public virtual event EventHandler? SystemColorsChanged;
    public virtual event EventHandler? TabIndexChanged;
    public virtual event EventHandler? TabStopChanged;
    public virtual event EventHandler? TextChanged;
    public virtual event EventHandler? PropertyChanged;

    readonly CancelEventArgs cancelEventArgs = new(false);
    public virtual event EventHandler? Validated;
    public virtual event CancelEventHandler? Validating;
    public virtual event EventHandler? VisibleChanged;
    //public event EventHandler? Disposed;
    public virtual event EventHandler? Load;
    public virtual IAsyncResult BeginInvoke(Delegate method, params object[] args)
    {
        var task = Task.Factory.StartNew(state =>
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
        var task = Task.Factory.StartNew(method);
        return task;
    }
    public virtual object EndInvoke(IAsyncResult asyncResult)
    {
        if (asyncResult is Task task)
        {
            if (task.IsCompleted == false && task is { IsCanceled: false, IsFaulted: false })
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
        _handleCreated = true;
        created = true;
    }

    ImageSurface? image;
    Surface? surface;
    Context? context;
    public virtual Graphics? CreateGraphics()
    {
        try
        {
            if (image == null)
                image = new ImageSurface(Format.Argb32, Widget.AllocatedWidth, Widget.AllocatedHeight);

            surface?.Dispose();
            surface = image.CreateSimilar(Content.ColorAlpha, Widget.AllocatedWidth, Widget.AllocatedHeight);
            context?.Dispose();
            context = new Context(surface);

            return new Graphics(Widget, context, Widget.Allocation);
        }
        catch (Exception ex)
        {
            Console.WriteLine("画版创建失败：" + ex.Message);
            throw;
        }
    }

    private void Override_PaintGraphics(Context cr, Rectangle rec)
    {
        if (surface != null)
        {
            cr.Save();
            cr.SetSourceSurface(surface, 0, 0);
            cr.Paint();
            cr.Restore();
        }
    }

    public virtual DragDropEffects DoDragDrop(object? data, DragDropEffects allowedEffects)
    {
        return DragDropEffects.None;
    }

    public virtual void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds)
    {
    }

    public virtual Form FindForm()
    {
        if (Widget.Toplevel?.Data.ContainsKey("Control") ?? false)
        {
            return Widget.Toplevel.Data["Control"] as Form;
        }

        var control = Parent;
        while (control != null)
        {
            if (control is Form)
                break;
            control = Parent;
        }
        return control as Form;
    }

    public virtual bool Focus()
    {
        if (Widget != null)
        {
            Widget.IsFocus = true;
            return Widget.IsFocus;
        }

        return false;
    }

    public virtual Control GetChildAtPoint(Point pt)
    {
        _handleCreated = true;
        return null;
    }

    public virtual Control GetChildAtPoint(Point pt, GetChildAtPointSkip skipValue)
    {
        _handleCreated = true;
        return null;
    }

    public virtual IContainerControl GetContainerControl()
    {
        return this as IContainerControl;
    }

    public virtual Control? GetNextControl(Control ctl, bool forward)
    {
        Control? prev = null;
        Control? next = null;
        var finded = false;

        foreach (var obj in Controls)
        {
            if (obj is Control control)
            {
                if (finded)
                    next = control;

                if (control.Widget.Handle == ctl.Widget.Handle)
                {
                    finded = true;
                }
                if (finded)
                {
                    if (forward == false && prev != null)
                    {
                        return prev;
                    }

                    if (forward && next != null)
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
        if (Widget != null)
        {
            if (Self != null)
                Self.Override.OnAddClass();
            Widget.Window.InvalidateRect(new Gdk.Rectangle(rc.X, rc.Y, rc.Width, rc.Height), invalidateChildren);
            if (invalidateChildren && Widget is Gtk.Container container)
            {
                foreach (var child in container.Children)
                    child.Window.InvalidateRect(child.Allocation, invalidateChildren);
            }
            Refresh();
        }
    }

    public virtual void Invalidate(Region region)
    {
        Invalidate(region, true);
    }

    public virtual void Invalidate(Region region, bool invalidateChildren)
    {
        if (Widget != null)
        {
            Self?.Override.OnAddClass();
            Widget.Window.InvalidateRect(Widget.Allocation, invalidateChildren);
        }
    }

    public virtual object? Invoke(Delegate method)
    {
        return Invoke(method, null);
    }

    public virtual object? Invoke(Delegate method, params object[] args)
    {
        object? result = null;
        if (!_handleCreated)
        {
            throw new InvalidOperationException();
        }
        GLib.Idle.Add(() =>
        {
            result = method.DynamicInvoke(args);
            return false;
        });
        return result;
    }
    public virtual void Invoke(Action method)
    {
        GLib.Idle.Add(() =>
        {
            method.Invoke();
            return false;
        });
    }
    public virtual TEntry Invoke<TEntry>(Func<TEntry> method)
    {
        var result = default(TEntry);
        GLib.Idle.Add(() =>
        {
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
        var x = 0;
        var y = 0;
        _handleCreated = true;
        if (Widget != null)
        {
            Widget.Window?.GetOrigin(out x, out y);
            if (p.X > x && p.Y > y)
                return new Point(p.X - x, p.Y - y);
        }
        return new Point(p.X, p.Y);
    }

    public virtual Point PointToScreen(Point p)
    {
        var x = 0;
        var y = 0;
        _handleCreated = true;
        if (Widget != null)
        {
            Widget.Window?.GetOrigin(out x, out y);
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
        var x = 0;
        var y = 0;
        _handleCreated = true;
        if (Widget != null)
        {
            Widget.Window?.GetPosition(out x, out y);
            if (r.X > x && r.Y > y)
                return new Rectangle(r.X - x, r.Y - y, r.Width, r.Height);
        }
        return new Rectangle(r.X, r.Y, r.Width, r.Height);
    }

    public virtual Rectangle RectangleToScreen(Rectangle r)
    {
        var x = 0;
        var y = 0;
        _handleCreated = true;
        if (Widget != null)
        {
            Widget.Window?.GetPosition(out x, out y);
            if (r.X < x && r.Y < y)
                return new Rectangle(r.X + x, r.Y + y, r.Width, r.Height);
        }
        return new Rectangle(r.X, r.Y, r.Width, r.Height);
    }

    public virtual void Refresh()
    {
        if (Widget is { IsVisible: true })
        {
            if (Self != null)
                Self.Override.OnAddClass();
            Widget.QueueDraw();
            if (Widget is Gtk.Container container)
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
        created = true;
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
        Widget?.SetStateFlags(StateFlags.Selected, true);
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
        if (Widget != null)
        {
            var rect = Widget.Clip;
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
            Widget.SetClip(rect);
        }
    }
    public virtual Rectangle ClientRectangle { get { Widget.GetAllocatedSize(out var allocation, out _); return new Rectangle(allocation.X, allocation.Y, allocation.Width, allocation.Height); } }

    public virtual Size ClientSize
    {
        get => new(Widget.AllocatedWidth, Widget.AllocatedHeight);
        set
        {
            var size = new Size(Widget.AllocatedWidth, Widget.AllocatedHeight);
            Widget.SetSizeRequest(value.Width, value.Height);
            if (size != value)
            {
                Layout?.Invoke(this, new LayoutEventArgs(this, nameof(ClientSize)));
                Resize?.Invoke(this, EventArgs.Empty);
                SizeChanged?.Invoke(this, EventArgs.Empty);
            }
            ClientSizeChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public virtual IntPtr Handle
    {
        get
        {
            OnHandleCreated(EventArgs.Empty);
            return Widget.Handle;
        }
        set => OnHandleCreated(EventArgs.Empty);
    }


    protected virtual void OnHandleCreated(EventArgs eventArgs)
    {
        if (!_handleCreated)
        {
            _handleCreated = true;
            HandleCreated?.Invoke(this, eventArgs);
        }
    }

    public virtual Padding Margin
    {
        get => margin;
        set
        {
            var padding = margin;
            margin = value;
            if (padding != value)
            {
                MarginChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public virtual Size MaximumSize { get; set; }
    public virtual Size MinimumSize { get; set; }
    private BorderStyle _BorderStyle;
    public virtual BorderStyle BorderStyle
    {
        get => _BorderStyle;
        set
        {
            _BorderStyle = value;
            if (value == BorderStyle.FixedSingle)
            {
                Widget.StyleContext.RemoveClass("BorderFixed3D");
                Widget.StyleContext.RemoveClass("BorderNone");
                Widget.StyleContext.AddClass("BorderFixedSingle");
            }
            else if (value == BorderStyle.Fixed3D)
            {
                Widget.StyleContext.RemoveClass("BorderFixedSingle");
                Widget.StyleContext.RemoveClass("BorderNone");
                Widget.StyleContext.AddClass("BorderFixed3D");
            }
            else
            {
                Widget.StyleContext.RemoveClass("BorderFixedSingle");
                Widget.StyleContext.RemoveClass("BorderFixed3D");
                Widget.StyleContext.AddClass("BorderNone");
            }
        }
    }

    public virtual void Hide()
    {
        if (GtkControl is Misc con)
        {
            con.Hide();
        }
    }

    public virtual void Show()
    {
        Widget?.ShowAll();
    }
    protected virtual void OnPaint(PaintEventArgs e)
    {
    }
    protected virtual void OnParentChanged(EventArgs e)
    {
    }

    public virtual void SuspendLayout()
    {
        created = false;
    }

    public virtual void PerformLayout()
    {
        created = true;
    }

    public virtual void PerformLayout(Control affectedControl, string affectedProperty)
    {
        created = true;
    }

    public virtual void Update()
    {
        if (Widget != null)
        {
            Widget.Window?.ProcessUpdates(true);
            Widget.QueueDraw();
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

    public new virtual event EventHandler? Disposed;

    public new virtual void Dispose()
    {
        Dispose(true);
        base.Dispose();
        _handleCreated = false;
        Disposed?.Invoke(this, EventArgs.Empty);
    }

    protected override void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }
        IsDisposed = true;
        try
        {
            image?.Dispose();
            surface?.Dispose();
            context?.Dispose();

            if (Widget != null)
            {
                Widget.Destroy();
                GtkControl = null;
            }
        }
        catch (Exception ex) { Trace.WriteLine(ex); }
        base.Dispose(disposing);
        HandleDestroyed?.Invoke(this, EventArgs.Empty);
    }

    protected virtual CreateParams? CreateParams
    {
        get
        {
            var createParams = new CreateParams();
            createParams.ExStyle |= 32;
            return createParams;
        }
    }

    public bool ParticipatesInLayout => false;

    PropertyStore IArrangedElement.Properties => throw new NotImplementedException();

    IArrangedElement IArrangedElement.Container => throw new NotImplementedException();

    private ArrangedElementCollection? arrangedElementCollection;
    public ArrangedElementCollection? Children => arrangedElementCollection;

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
    }

    void IArrangedElement.PerformLayout(IArrangedElement affectedElement, string propertyName)
    {
    }
}