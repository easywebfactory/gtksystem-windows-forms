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
using System.Text;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    [Designer(typeof(ControlDesigner))]
    [ToolboxItemFilter("System.Windows.Forms")]
    public partial class Control : Component, IControl, ISynchronizeInvoke, IComponent, IDisposable, ISupportInitialize, IArrangedElement, IBindableComponent
    {
        public string unique_key { get; protected set; }

        public virtual Gtk.Widget Widget { get => GtkControl as Gtk.Widget; }
        public virtual Gtk.Container GtkContainer { get => GtkControl as Gtk.Container; }
        public virtual IControlGtk ISelf { get => GtkControl as IControlGtk; }
        public virtual object GtkControl { get; set; }
        private EventHandlerList handlerList = new EventHandlerList();
        private static Dictionary<string, string> fontLanguages = new Dictionary<string, string>();
        internal static Dictionary<Gdk.Key, int> keyboardMap = new Dictionary<Gdk.Key, int>();
        static Control()
        {
            if (fontLanguages.Count == 0)
            {
                fontLanguages.Add("宋体", "SimSun");
                fontLanguages.Add("黑体", "SimHei");
                fontLanguages.Add("微软雅黑", "Microsoft Yahei");
                fontLanguages.Add("微软正黑", "Microsoft JhengHei");
                fontLanguages.Add("微軟正黑體", "Microsoft JhengHei");
                fontLanguages.Add("楷体", "KaiTi");
                fontLanguages.Add("新宋体", "NSimSun");
                fontLanguages.Add("仿宋", "FangSong");
                fontLanguages.Add("標楷體", "BiauKai");
                fontLanguages.Add("新細明體", "PMingLiU");
                fontLanguages.Add("細明體", "MingLiU");
                //macos
                fontLanguages.Add("苹方", "PingFang SC");
                fontLanguages.Add("华文黑体", "STHeiti");
                fontLanguages.Add("华文楷体", "STKaiti");
                fontLanguages.Add("华文宋体", "STSong");
                fontLanguages.Add("华文仿宋", "STFangsong");
                fontLanguages.Add("华文中宋", "STZhongsong");
                fontLanguages.Add("华文琥珀", "STHupo");
                fontLanguages.Add("华文新魏", "STXinwei");
                fontLanguages.Add("华文隶书", "STLiti");
                fontLanguages.Add("华文行楷", "STXingkai");
                //open
                fontLanguages.Add("思源黑体", "Source Han Sans CN");
                fontLanguages.Add("思源宋体", "Source Han Serif SC");
                fontLanguages.Add("文泉驿微米黑", "WenQuanYi Micro Hei");
            }
            if (keyboardMap.Count == 0)
            {
                keyboardMap = new Dictionary<Gdk.Key, int>{
                { Gdk.Key.BackSpace,8},
                { Gdk.Key.Tab,9},
                { Gdk.Key.ISO_Left_Tab,9},
                { Gdk.Key.KP_Tab,9},
                { Gdk.Key.Return,13},
                { Gdk.Key.KP_Enter,13},
                { Gdk.Key.Shift_L,16},
                { Gdk.Key.Shift_R,161},
                { Gdk.Key.Shift_Lock,10},
                { Gdk.Key.Control_L,17},
                { Gdk.Key.Control_R,163},
                { Gdk.Key.Alt_L,18},
                { Gdk.Key.Alt_R,165},
                { Gdk.Key.Pause,19},
                { Gdk.Key.Caps_Lock,20},
                { Gdk.Key.VoidSymbol,20},
                { Gdk.Key.Escape,27},
                { Gdk.Key.space,32},
                { Gdk.Key.Prior,33},
                { Gdk.Key.KP_Prior,33},
                { Gdk.Key.Next,34},
                { Gdk.Key.KP_Page_Down,34},
                { Gdk.Key.End,35},
                { Gdk.Key.KP_End,35},
                { Gdk.Key.Home,36},
                { Gdk.Key.KP_Home,36},
                { Gdk.Key.Left,37},
                { Gdk.Key.KP_Left,37},
                { Gdk.Key.KP_Up,38},
                { Gdk.Key.Right,39},
                { Gdk.Key.KP_Right,39},
                { Gdk.Key.Down,40},
                { Gdk.Key.KP_Down,40},
                { Gdk.Key.Insert,45},
                { Gdk.Key.Delete,46},
                { Gdk.Key.KP_Delete,46},
                { Gdk.Key.Key_0,48},
                { Gdk.Key.Key_1,49},
                { Gdk.Key.Key_2,50},
                { Gdk.Key.Key_3,51},
                { Gdk.Key.Key_4,52},
                { Gdk.Key.Key_5,53},
                { Gdk.Key.Key_6,54},
                { Gdk.Key.Key_7,55},
                { Gdk.Key.Key_8,56},
                { Gdk.Key.Key_9,57},
                { Gdk.Key.parenright,48},
                { Gdk.Key.exclam,49},
                { Gdk.Key.at,50},
                { Gdk.Key.numbersign,51},
                { Gdk.Key.dollar,52},
                { Gdk.Key.percent,53},
                { Gdk.Key.asciicircum,54},
                { Gdk.Key.ampersand,55},
                { Gdk.Key.asterisk,56},
                { Gdk.Key.parenleft,57},
                { Gdk.Key.A,65},
                { Gdk.Key.B,66},
                { Gdk.Key.C,67},
                { Gdk.Key.D,68},
                { Gdk.Key.E,69},
                { Gdk.Key.F,70},
                { Gdk.Key.G,71},
                { Gdk.Key.H,72},
                { Gdk.Key.I,73},
                { Gdk.Key.J,74},
                { Gdk.Key.K,75},
                { Gdk.Key.L,76},
                { Gdk.Key.M,77},
                { Gdk.Key.N,78},
                { Gdk.Key.O,79},
                { Gdk.Key.P,80},
                { Gdk.Key.Q,81},
                { Gdk.Key.R,82},
                { Gdk.Key.S,83},
                { Gdk.Key.T,84},
                { Gdk.Key.U,85},
                { Gdk.Key.V,86},
                { Gdk.Key.W,87},
                { Gdk.Key.X,88},
                { Gdk.Key.Y,89},
                { Gdk.Key.Z,90},
                { Gdk.Key.a,65},
                { Gdk.Key.b,66},
                { Gdk.Key.c,67},
                { Gdk.Key.d,68},
                { Gdk.Key.e,69},
                { Gdk.Key.f,70},
                { Gdk.Key.g,71},
                { Gdk.Key.h,72},
                { Gdk.Key.i,73},
                { Gdk.Key.j,74},
                { Gdk.Key.k,75},
                { Gdk.Key.l,76},
                { Gdk.Key.m,77},
                { Gdk.Key.n,78},
                { Gdk.Key.o,79},
                { Gdk.Key.p,80},
                { Gdk.Key.q,81},
                { Gdk.Key.r,82},
                { Gdk.Key.s,83},
                { Gdk.Key.t,84},
                { Gdk.Key.u,85},
                { Gdk.Key.v,86},
                { Gdk.Key.w,87},
                { Gdk.Key.x,88},
                { Gdk.Key.y,89},
                { Gdk.Key.z,90},
                { Gdk.Key.Meta_L,91},
                { Gdk.Key.Meta_R,92},
                { Gdk.Key.approximate,93},
                { Gdk.Key.KP_0,96},
                { Gdk.Key.KP_1,97},
                { Gdk.Key.KP_2,98},
                { Gdk.Key.KP_3,99},
                { Gdk.Key.KP_4,100},
                { Gdk.Key.KP_5,101},
                { Gdk.Key.KP_6,102},
                { Gdk.Key.KP_7,103},
                { Gdk.Key.KP_8,104},
                { Gdk.Key.KP_9,105},
                { Gdk.Key.KP_Multiply,106},
                { Gdk.Key.KP_Add,107},
                { Gdk.Key.KP_Subtract,109},
                { Gdk.Key.KP_Decimal,110},
                { Gdk.Key.KP_Divide,111},
                { Gdk.Key.F1,112},
                { Gdk.Key.F2,113},
                { Gdk.Key.F3,114},
                { Gdk.Key.F4,115},
                { Gdk.Key.F5,116},
                { Gdk.Key.F6,117},
                { Gdk.Key.F7,118},
                { Gdk.Key.F8,119},
                { Gdk.Key.F9,120},
                { Gdk.Key.F10,121},
                { Gdk.Key.F11,122},
                { Gdk.Key.F12,123},
                { Gdk.Key.Num_Lock,144},
                { Gdk.Key.Scroll_Lock,145},
                { Gdk.Key.period,190},
                { Gdk.Key.greater,190},
                { Gdk.Key.slash,191},
                { Gdk.Key.question,191},
                { Gdk.Key.quoteleft,192},
                { Gdk.Key.asciitilde,192},
                { Gdk.Key.braceleft,219},
                { Gdk.Key.bracketleft,219},
                { Gdk.Key.backslash,220},
                { Gdk.Key.bar,220},
                { Gdk.Key.braceright,221},
                { Gdk.Key.bracketright,221},
                { Gdk.Key.apostrophe,222},
                { Gdk.Key.quotedbl,222},
                { Gdk.Key.semicolon,186},
                { Gdk.Key.colon,186},
                { Gdk.Key.equal,187},
                { Gdk.Key.plus,187},
                { Gdk.Key.comma,188},
                { Gdk.Key.less,188},
                { Gdk.Key.minus,189},
                { Gdk.Key.underscore,189},
                };
            }
        }
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
                widget.Realized += Widget_Realized;
                ISelf.Override.PaintGraphics += Override_PaintGraphics;
                widget.SizeAllocated += Widget_SizeAllocated;
                widget.ParentSet += Widget_ParentSet;
                widget.WidgetEvent += Widget_WidgetEvent;
            }
        }
        #region events
        private bool WidgetRealized = false;
        private void Widget_Realized(object sender, EventArgs e)
        {
            if (WidgetRealized == false)
            {
                WidgetRealized = true;
                InitStyle((Gtk.Widget)sender);
                OnLoad(EventArgs.Empty);
                Load?.Invoke(this, e);
                SetDockAnchor(this);
            }
        }

        private bool Is_DoubleButtonPress = false;
        private void Widget_WidgetEvent(object o, WidgetEventArgs args)
        {
            Gtk.Widget owidget = o as Gtk.Widget;
            if (owidget.IsRealized)
            {
                if (args.Event.Type == Gdk.EventType.MotionNotify)
                {
                    int clicks = 0;
                    Gdk.EventMotion eventmotion = args.Event as Gdk.EventMotion;
                    MouseButtons button = MouseButtons.None;
                    if (eventmotion.State.HasFlag(Gdk.ModifierType.Button1Mask))
                    {
                        button |= MouseButtons.Left;
                        clicks += 1;
                    }
                    if (eventmotion.State.HasFlag(Gdk.ModifierType.Button2Mask))
                    {
                        button |= MouseButtons.Middle;
                        clicks += 1;
                    }
                    if (eventmotion.State.HasFlag(Gdk.ModifierType.Button3Mask))
                    {
                        button |= MouseButtons.Right;
                        clicks += 1;
                    }
                    owidget.Window.GetOrigin(out int x, out int y);
                    MouseEventArgs mouseArgs = new MouseEventArgs(button, clicks, (int)eventmotion.XRoot - x, (int)eventmotion.YRoot - y, 1);
                    OnMouseMove(mouseArgs);
                    MouseMove?.Invoke(this, mouseArgs);
                }
                else if (args.Event.Type == Gdk.EventType.KeyPress)
                {
                    Gdk.EventKey eventkey = args.Event as Gdk.EventKey;
                    Keys keys = (Keys)eventkey.HardwareKeycode;
                    if (keyboardMap.TryGetValue(eventkey.Key, out int keycode))
                    {
                        keys = (Keys)keycode;
                        if (eventkey.Key.Equals(Gdk.Key.VoidSymbol) && eventkey.HardwareKeycode == 20)
                            keys = Keys.CapsLock;
                    }
                    if (eventkey.State.HasFlag(Gdk.ModifierType.Mod1Mask))
                        keys |= Keys.Alt;
                    if (eventkey.State.HasFlag(Gdk.ModifierType.ControlMask))
                        keys |= Keys.Control;
                    if (eventkey.State.HasFlag(Gdk.ModifierType.ShiftMask))
                        keys |= Keys.Shift;
                    if (eventkey.State.HasFlag(Gdk.ModifierType.LockMask))
                        keys |= Keys.CapsLock;
                    KeyEventArgs keyargs = new KeyEventArgs(keys);
                    OnKeyDown(keyargs);
                    KeyDown?.Invoke(this, keyargs);
                    args.RetVal = keyargs.SuppressKeyPress;
                }
                else if (args.Event.Type == Gdk.EventType.KeyRelease)
                {
                    Gdk.EventKey eventkey = args.Event as Gdk.EventKey;
                    Keys keys = (Keys)eventkey.HardwareKeycode;
                    if (keyboardMap.TryGetValue(eventkey.Key, out int keycode))
                    {
                        keys = (Keys)keycode;
                        if (eventkey.Key.Equals(Gdk.Key.VoidSymbol) && eventkey.HardwareKeycode == 20)
                            keys = Keys.CapsLock;
                    }
                    if (eventkey.State.HasFlag(Gdk.ModifierType.Mod1Mask))
                        keys |= Keys.Alt;
                    if (eventkey.State.HasFlag(Gdk.ModifierType.ControlMask))
                        keys |= Keys.Control;
                    if (eventkey.State.HasFlag(Gdk.ModifierType.ShiftMask))
                        keys |= Keys.Shift;
                    if (eventkey.State.HasFlag(Gdk.ModifierType.LockMask))
                        keys |= Keys.CapsLock;
                    KeyEventArgs keyargs = new KeyEventArgs(keys);
                    OnKeyUp(keyargs);
                    KeyUp?.Invoke(this, keyargs);
                    args.RetVal = keyargs.SuppressKeyPress;
                    if (!keyargs.SuppressKeyPress)
                        KeyPress?.Invoke(this, new KeyPressEventArgs(Convert.ToChar(keycode)));
                }
                else if (args.Event.Type == Gdk.EventType.ButtonPress)
                {
                    Gdk.EventButton eventbutton = args.Event as Gdk.EventButton;
                    MouseButtons button = MouseButtons.None;
                    if (eventbutton.Button == 1)
                        button = MouseButtons.Left;
                    else if (eventbutton.Button == 2)
                        button = MouseButtons.Middle;
                    else if (eventbutton.Button == 3)
                        button = MouseButtons.Right;
                    owidget.Window.GetOrigin(out int x, out int y);//避免事件穿透错误
                    MouseEventArgs mouseArgs = new MouseEventArgs(button, 1, (int)eventbutton.XRoot - x, (int)eventbutton.YRoot - y, 0);
                    OnMouseDown(mouseArgs);
                    MouseDown?.Invoke(this, mouseArgs);
                }
                else if (args.Event.Type == Gdk.EventType.TwoButtonPress || args.Event.Type == Gdk.EventType.DoubleButtonPress)
                {
                    Gdk.EventButton eventbutton = args.Event as Gdk.EventButton;
                    if (eventbutton.Button == 1)
                    {
                        Is_DoubleButtonPress = true;
                        MouseButtons button = MouseButtons.None;
                        if (eventbutton.Button == 1)
                            button = MouseButtons.Left;
                        else if (eventbutton.Button == 2)
                            button = MouseButtons.Middle;
                        else if (eventbutton.Button == 3)
                            button = MouseButtons.Right;
                        owidget.Window.GetOrigin(out int x, out int y);//避免事件穿透错误

                        MouseEventArgs mouseArgs2 = new MouseEventArgs(button, 2, (int)eventbutton.XRoot - x, (int)eventbutton.YRoot - y, 0);
                        OnMouseDoubleClick(mouseArgs2);
                        MouseDoubleClick?.Invoke(this, mouseArgs2);
                        DoubleClick?.Invoke(this, EventArgs.Empty);
                    }
                }
                else if (args.Event.Type == Gdk.EventType.ButtonRelease)
                {
                    Gdk.EventButton eventbutton = args.Event as Gdk.EventButton;
                    MouseButtons button = MouseButtons.None;
                    if (eventbutton.Button == 1)
                        button = MouseButtons.Left;
                    else if (eventbutton.Button == 2)
                        button = MouseButtons.Middle;
                    else if (eventbutton.Button == 3)
                        button = MouseButtons.Right;
                    owidget.Window.GetOrigin(out int x, out int y);
                    if (eventbutton.Button == 1 && Is_DoubleButtonPress == false)
                    {
                        OnClick(EventArgs.Empty);
                        MouseEventArgs mouseArgs3 = new MouseEventArgs(button, 1, (int)eventbutton.XRoot - x, (int)eventbutton.YRoot - y, 0);
                        OnMouseClick(mouseArgs3);
                        Click?.Invoke(this, EventArgs.Empty);
                        MouseClick?.Invoke(this, mouseArgs3);
                    }
                    MouseEventArgs mouseArgs = new MouseEventArgs(button, 1, (int)eventbutton.XRoot - x, (int)eventbutton.YRoot - y, 0);
                    OnMouseUp(mouseArgs);
                    MouseUp?.Invoke(this, mouseArgs);
                    if (eventbutton.Button == 3)
                    {
                        if (ContextMenuStrip != null)
                        {
                            ContextMenuStrip.Widget.ShowAll();
                            ((Gtk.Menu)ContextMenuStrip.Widget).PopupAtPointer(args.Event);
                            args.RetVal = true;
                        }
                    }
                    Is_DoubleButtonPress = false;
                }
                else if (args.Event.Type == Gdk.EventType.Configure)
                {
                    OnMove(EventArgs.Empty);
                    Move?.Invoke(this, EventArgs.Empty);
                }
                else if (args.Event.Type == Gdk.EventType.EnterNotify)
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
                    OnMouseEnter(EventArgs.Empty);
                    OnMouseHover(EventArgs.Empty);
                    Enter?.Invoke(this, EventArgs.Empty);
                    MouseEnter?.Invoke(this, EventArgs.Empty);
                    MouseHover?.Invoke(this, EventArgs.Empty);
                }
                else if (args.Event.Type == Gdk.EventType.LeaveNotify)
                {
                    if (Cursor != null && this.Widget.Window != null)
                    {
                        this.Widget.Window.Cursor = null;
                    }
                    OnMouseLeave(EventArgs.Empty);
                    Leave?.Invoke(this, EventArgs.Empty);
                    MouseLeave?.Invoke(this, EventArgs.Empty);
                }
                else if (args.Event.Type == Gdk.EventType.FocusChange)
                {
                    Gdk.EventFocus eventfocus = args.Event as Gdk.EventFocus;
                    if (eventfocus.In)
                    {
                        GotFocus?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
                        LostFocus?.Invoke(this, EventArgs.Empty);
                        OnValidating(cancelEventArgs);
                        Validating?.Invoke(this, cancelEventArgs);
                        if (Validated != null && cancelEventArgs.Cancel == false)
                        {
                            OnValidated(cancelEventArgs);
                            Validated(this, cancelEventArgs);
                        }
                    }
                }
                else if (args.Event.Type == Gdk.EventType.Scroll)
                {
                    int clicks = 0;
                    Gdk.EventScroll eventscroll = args.Event as Gdk.EventScroll;
                    MouseButtons button = MouseButtons.None;
                    if (eventscroll.State.HasFlag(Gdk.ModifierType.Button1Mask))
                    {
                        button |= MouseButtons.Left;
                        clicks += 1;
                    }
                    if (eventscroll.State.HasFlag(Gdk.ModifierType.Button2Mask))
                    {
                        button |= MouseButtons.Middle;
                        clicks += 1;
                    }
                    if (eventscroll.State.HasFlag(Gdk.ModifierType.Button3Mask))
                    {
                        button |= MouseButtons.Right;
                        clicks += 1;
                    }
                    int delta = (int)eventscroll.DeltaY;
                    if (eventscroll.Direction == Gdk.ScrollDirection.Left || eventscroll.Direction == Gdk.ScrollDirection.Right)
                    {
                        delta = (int)eventscroll.DeltaX;
                    }
                    MouseEventArgs mouseArgs = new MouseEventArgs(button, clicks, (int)eventscroll.X, (int)eventscroll.Y, delta);
                    OnMouseWheel(mouseArgs);
                    MouseWheel?.Invoke(this, mouseArgs);
                }
            }
        }

        private void Widget_ParentSet(object o, ParentSetArgs args)
        {
            OnParentChanged(EventArgs.Empty);
            ParentChanged?.Invoke(this, args);
        }

        private int size_width = 0;
        private int size_height = 0;
        private int location_x = 0;
        private int location_y = 0;
        private void Widget_SizeAllocated(object o, SizeAllocatedArgs args)
        {
            OnResize(EventArgs.Empty);
            Resize?.Invoke(this, EventArgs.Empty);
            if (args.Allocation.Width != size_width || args.Allocation.Height != size_height)
            {
                size_width = args.Allocation.Width;
                size_height = args.Allocation.Height;
                OnSizeChanged(EventArgs.Empty);
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

        #endregion

        //===================
        protected virtual void InitStyle(Gtk.Widget widget)
        {
            SetStyle(widget);
        }
        protected virtual void UpdateStyle()
        {
            GLib.Timeout.Add(1, () =>
            {
                if (this.Widget != null && this.Widget.IsMapped)
                    SetStyle(this.Widget);
                return false;
            });
        }
        CssProvider provider = new CssProvider();
        protected virtual void SetStyle(Gtk.Widget widget)
        {
            StringBuilder style = new StringBuilder();
            if (widget is Gtk.Image) { }
            else
            {
                if (this._image != null && this._image.PixbufData != null)
                {
                    string imgdir = Path.Combine("Resources", widget.WidgetPath.IterGetName(0)).Replace("\\", "/");
                    string imguri = $"{imgdir}/{widget.Name}_image.png";
                    if (!File.Exists(imguri))
                    {
                        if (!Directory.Exists(imgdir))
                            Directory.CreateDirectory(imgdir);
                        Gdk.Pixbuf imagepixbuf = new Gdk.Pixbuf(this._image.PixbufData);
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

                    if (this._backgroundImage != null && this._backgroundImage.PixbufData != null)
                    {
                        string bgimguri = $"{imgdir}/{widget.Name}_bgimage.png";
                        if (!File.Exists(bgimguri))
                        {
                            Gdk.Pixbuf bgpixbuf = new Gdk.Pixbuf(this._backgroundImage.PixbufData);
                            bgpixbuf.Save(bgimguri, "png");
                        }
                        style.AppendFormat(",url(\"{0}\") repeat", bgimguri);
                    }
                    style.Append(";");
                    style.Append("background-origin: padding-box;");
                    style.Append("background-clip: padding-box;");
                }
                else if (this._backgroundImage != null && this._backgroundImage.PixbufData != null)
                {
                    Gdk.Pixbuf bgpixbuf = new Gdk.Pixbuf(this._backgroundImage.PixbufData);
                    string bgimgdir = Path.Combine("Resources", widget.WidgetPath.IterGetName(0)).Replace("\\", "/");
                    string bgimguri = $"{bgimgdir}/{widget.Name}_bgimage.png";
                    if (!File.Exists(bgimguri))
                    { 
                        if (!Directory.Exists(bgimgdir))
                            Directory.CreateDirectory(bgimgdir);
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
                        if (fontLanguages.TryGetValue(font.FontFamily.Name, out string enname))
                            style.AppendFormat("font-family:\"{0}\";", enname);
                        else
                            style.AppendFormat("font-family:\"{0}\";", font.FontFamily.Name);
                    }
                    if (font.Bold)
                    {
                        style.Append("font-weight:bold;");
                    }
                    if (font.Italic)
                    {
                        style.Append("font-style:italic;");
                    }
                    if (font.Underline)
                    {
                        style.Append("text-decoration:underline;");
                    }
                    if (font.Strikeout)
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
                    if (provider.LoadFromData(css.ToString()))
                    {
                        if (widget.StyleContext.HasClass(styleClassName))
                        {
                            widget.StyleContext.RemoveClass(styleClassName);
                            widget.StyleContext.RemoveProvider(provider);
                        }
                        widget.StyleContext.AddProvider(provider, StyleProviderPriority.User);
                        widget.StyleContext.AddClass(styleClassName);
                    }
                }
            }
        }
        protected virtual void SetStyle(ControlStyles styles, bool value)
        {
        }

        public static Point MousePosition
        {
            get
            {
                Gdk.Display.Default.GetPointer(out int x, out int y, out Gdk.ModifierType mod);
                return new Point(x, y);
            }
        }
        public static MouseButtons MouseButtons
        {
            get
            {
                Gdk.Display.Default.GetPointer(out int x, out int y, out Gdk.ModifierType mod);
                MouseButtons result = MouseButtons.None;
                if (mod.HasFlag(Gdk.ModifierType.Button1Mask))
                    result |= MouseButtons.Left;
                else if (mod.HasFlag(Gdk.ModifierType.Button2Mask))
                    result |= MouseButtons.Middle;
                else if (mod.HasFlag(Gdk.ModifierType.Button3Mask))
                    result |= MouseButtons.Right;
                return result;
            }
        }
        public static Keys ModifierKeys
        {
            get
            {
                Gdk.Display.Default.GetPointer(out int x, out int y, out Gdk.ModifierType mod);
                Keys result = Keys.None;
                if (mod.HasFlag(Gdk.ModifierType.ControlMask))
                    result |= Keys.Control;
                else if (mod.HasFlag(Gdk.ModifierType.Mod1Mask))
                    result |= Keys.Alt;
                else if (mod.HasFlag(Gdk.ModifierType.ShiftMask))
                    result |= Keys.Shift;
                return result;
            }
        }

        #region 背景
        private Drawing.Image _image;
        public virtual System.Drawing.Image Image { get => _image; set { _image = value; UpdateStyle(); } }
        public virtual System.Drawing.ContentAlignment ImageAlign { get; set; }

        public virtual bool UseVisualStyleBackColor { get; set; } = true;
        public virtual Color VisualStyleBackColor { get; }
        private ImageLayout _backgroundImageLayout;
        public virtual ImageLayout BackgroundImageLayout { get => _backgroundImageLayout; set { _backgroundImageLayout = value; if (ISelf != null) { ISelf.Override.BackgroundImageLayout = value; } } }
        private Drawing.Image _backgroundImage;
        public virtual Drawing.Image BackgroundImage { get => _backgroundImage; set { _backgroundImage = value; if (ISelf != null) { ISelf.Override.BackgroundImage = value;  } UpdateStyle(); } }
        public virtual Color BackColor
        {
            get
            {
                if (ISelf.Override.BackColor.HasValue)
                    return ISelf.Override.BackColor.Value;
                else
                    return Color.FromName("0");
            }
            set
            {
                ISelf.Override.BackColor = value;
                ISelf.Override.OnAddClass();
                UpdateStyle();
                Refresh();
            }
        }
        #endregion
        public virtual AccessibleObject AccessibilityObject { get; }

        public virtual string AccessibleDefaultActionDescription { get; set; }
        public virtual string AccessibleDescription { get; set; }
        public virtual string AccessibleName { get; set; }
        public virtual AccessibleRole AccessibleRole { get; set; }
        public virtual bool AllowDrop { get; set; }
        private AnchorStyles _anchor;
        public virtual AnchorStyles Anchor
        {
            get => _anchor;
            set
            {
                _anchor = value;
                SetAnchorStyles(Widget, _anchor);
                if (AnchorChanged != null)
                    AnchorChanged(this, EventArgs.Empty);

                SetDockAnchor(this);
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
        public virtual bool AutoSize
        {
            get => _autoSize;
            set
            {
                _autoSize = value;
                if (_autoSize == true) { this.Widget.WidthRequest = -1; this.Widget.HeightRequest = -1; } else { Size = _size; }
            }
        }
        public virtual BindingContext BindingContext { get; set; }
        public virtual Rectangle Bounds { get => new Rectangle(Widget.Clip.X, this.Widget.Clip.Y, this.Widget.Clip.Width, this.Widget.Clip.Height); set { SetBounds(value.X, value.Y, value.Width, value.Height); } }

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
                SetDockStyles(this.Widget, _dock);
                
                if (DockChanged != null)
                    DockChanged(this, EventArgs.Empty);

                SetDockAnchor(this);
            }
        }
        private void SetDockStyles(Gtk.Widget widget, DockStyle dockStyle)
        {
            if (dockStyle == DockStyle.Fill)
            {
                widget.Halign = Align.Fill;
                widget.Valign = Align.Fill;
            }
            else if (dockStyle == DockStyle.Left)
            {
                widget.Halign = Align.Start;
                widget.Valign = Align.Fill;
            }
            else if (dockStyle == DockStyle.Top)
            {
                widget.Halign = Align.Fill;
                widget.Valign = Align.Start;
            }
            else if (dockStyle == DockStyle.Right)
            {
                widget.Halign = Align.End;
                widget.Valign = Align.Fill;
            }
            else if (dockStyle == DockStyle.Bottom)
            {
                widget.Halign = Align.Fill;
                widget.Valign = Align.End;
            }
            else
            {
                widget.Halign = Align.Start;
                widget.Valign = Align.Start;
            }
        }
        private void SetDockAnchor(Control control)
        {
            Gtk.Widget widget = control.Widget;
            if (widget.IsRealized)
            {
                if (widget.Parent is Gtk.Overlay lay)
                {
                    int framewidth = lay.AllocatedWidth;
                    int frameheight = lay.AllocatedHeight;
                    if (lay.Parent is IControlGtk)
                    {
                        if (lay.Parent.WidthRequest > -1)
                            framewidth = lay.Parent.WidthRequest;
                        if (lay.Parent.HeightRequest > -1)
                            frameheight = lay.Parent.HeightRequest;
                    }
                    else if (lay.Parent.Parent is IControlGtk)
                    {
                        if (lay.Parent.Parent.WidthRequest > -1)
                            framewidth = lay.Parent.Parent.WidthRequest;
                        if (lay.Parent.Parent.HeightRequest > -1)
                            frameheight = lay.Parent.Parent.HeightRequest;
                    }

                    if (control.Dock == DockStyle.Fill)
                    {
                        widget.WidthRequest = -1;
                        widget.HeightRequest = -1;
                        if (lay.HeightRequest <= frameheight)
                            lay.HeightRequest = -1;
                        if (lay.WidthRequest <= framewidth)
                            lay.WidthRequest = -1;
                    }
                    else if (control.Dock == DockStyle.Left)
                    {
                        widget.HeightRequest = -1;
                        if (lay.HeightRequest <= frameheight)
                            lay.HeightRequest = -1;
                    }
                    else if (control.Dock == DockStyle.Right)
                    {
                        widget.HeightRequest = -1;
                        if (lay.HeightRequest <= frameheight)
                            lay.HeightRequest = -1;
                    }
                    else if (control.Dock == DockStyle.Top)
                    {
                        widget.WidthRequest = -1;
                        if (lay.WidthRequest <= framewidth)
                            lay.WidthRequest = -1;
                    }
                    else if (control.Dock == DockStyle.Bottom)
                    {
                        widget.WidthRequest = -1;
                        if (lay.WidthRequest <= framewidth)
                            lay.WidthRequest = -1;
                    }

                    if (widget.Halign == Gtk.Align.End)
                    {
                        if (widget.WidthRequest > 0)
                            widget.MarginEnd = Math.Max(0, framewidth - widget.MarginStart - widget.WidthRequest);
                        else
                            widget.MarginEnd = 0;
                    }
                    else if (widget.Halign == Gtk.Align.Fill)
                    {
                        if (control.Dock == DockStyle.Fill)
                            widget.MarginEnd = 0;
                        else if (widget.WidthRequest > 0)
                            widget.MarginEnd = Math.Max(0, framewidth - widget.MarginStart - widget.WidthRequest);
                        else
                            widget.MarginEnd = 0;
                    }
                    if (widget.Valign == Gtk.Align.End)
                    {
                        if (widget.HeightRequest > 0)
                            widget.MarginBottom = Math.Max(0, frameheight - widget.MarginTop - widget.HeightRequest);
                        else
                            widget.MarginBottom = 0;
                    }
                    else if (widget.Valign == Gtk.Align.Fill)
                    {
                        if (control.Dock == DockStyle.Fill)
                            widget.MarginBottom = 0;
                        else if (widget.HeightRequest > 0)
                            widget.MarginBottom = Math.Max(0, frameheight - widget.MarginTop - widget.HeightRequest);
                        else
                            widget.MarginBottom = 0;
                    }
                }
            }
        }
        public virtual bool Enabled { get { return this.Widget.Sensitive; } set { this.Widget.Sensitive = value; } }

        public virtual bool Focused { get { return this.Widget.IsFocus; } }
        private Font _Font;
        public virtual Font Font
        {
            get
            {
                return _Font;
            }
            set { 
                _Font = value;
                UpdateStyle();
            }
        }
        private Color _ForeColor;
        public virtual Color ForeColor
        {
            get { return _ForeColor; }
            set { _ForeColor = value; UpdateStyle(); }
        }

        public virtual bool HasChildren { get; }

        public virtual ImeMode ImeMode { get; set; }

        public virtual bool InvokeRequired { get; }

        public virtual bool IsAccessible { get; set; }

        public virtual bool IsDisposed { get; }

        public virtual bool IsHandleCreated { get => this.Widget.Handle != IntPtr.Zero; }

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
            set
            {
                this.Widget.MarginStart = value;
                if (DockChanged != null)
                    DockChanged(this, EventArgs.Empty);
                if (AnchorChanged != null)
                    AnchorChanged(this, EventArgs.Empty);
            }
        }
        public virtual int Right
        {
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
                return new Size(Width, Height);
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
                SetDockAnchor(this);
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
            set
            {
                this.Widget.WidthRequest = Math.Max(-1, value);
                if (DockChanged != null)
                    DockChanged(this, EventArgs.Empty);
                if (AnchorChanged != null)
                    AnchorChanged(this, EventArgs.Empty);

                SetDockAnchor(this);
            }
        }
        public virtual int TabIndex { get; set; }
        public virtual bool TabStop { get; set; }
        public virtual object Tag { get; set; }
        public virtual string Text { get; set; }
        public virtual Control TopLevelControl { get; }
        public virtual bool UseWaitCursor { get; set; }
        public virtual bool Visible { get { return this.Widget.Visible; } set { this.Widget.NoShowAll = value == false; if (value == true) { this.Widget.ShowAll(); } else { this.Widget.Visible = value; } } }

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

        public virtual event EventHandler Validated;
        public virtual event CancelEventHandler Validating;
        public virtual event EventHandler VisibleChanged;
        //public event EventHandler Disposed;
        public virtual event EventHandler Load;
        public virtual IAsyncResult BeginInvoke(Delegate method, params object[] args)
        {
            System.Threading.Tasks.Task<object> task = System.Threading.Tasks.Task.Factory.StartNew(state =>
            {
               return method.DynamicInvoke((object[])state);
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
            if (asyncResult is Task<object> task1)
            {
                try
                {
                    if (task1.IsCompleted == true)
                    {
                        return task1.Result;
                    }
                    else if (task1.IsCanceled == true)
                    {
                        return null;
                    }
                    else if (task1.IsFaulted == true)
                    {
                        return task1.Exception;
                    }
                    else
                    {
                        return task1.GetAwaiter().GetResult();
                    }
                }
                finally
                {
                    task1.Dispose();
                }
            }
            else if (asyncResult is System.Threading.Tasks.Task task2)
            {
                try
                {
                    if (task2.IsCompleted == true)
                    {
                        task2.Dispose();
                        return null;
                    }
                    else if (task2.IsCanceled == true)
                    {
                        task2.Dispose();
                        return null;
                    }
                    else if (task2.IsFaulted == true)
                    {
                        task2.Dispose();
                        return task2.Exception;
                    }
                    else
                    {
                        task2.Dispose();
                        return null;
                    }
                }
                finally
                {
                    task2.Dispose();
                }
            }

            return null;
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
        Cairo.Surface surface;
        Cairo.Context context;
        public virtual Drawing.Graphics CreateGraphics()
        {
            try
            {
                surface?.Dispose();
                surface = this.Widget.Window.CreateSimilarSurface(Cairo.Content.ColorAlpha, this.Widget.AllocatedWidth, this.Widget.AllocatedHeight);
                context?.Dispose();
                context = new Cairo.Context(surface);
                return new Drawing.Graphics(this.Widget, context, this.Widget.Allocation);
            }
            catch (Exception ex)
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
        private object paintEventHandler_paint = new object();
        public virtual event PaintEventHandler Paint
        {
            add { handlerList.AddHandler(paintEventHandler_paint, value); ISelf.Override.Paint += Override_Paint; }
            remove { handlerList.RemoveHandler(paintEventHandler_paint, value); ISelf.Override.Paint -= Override_Paint; }
        }
        private void Override_Paint(object sender, PaintEventArgs e)
        {
            OnPaint(e);
            handlerList[paintEventHandler_paint]?.DynamicInvoke(sender, e);
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
            else
            {
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
            Control next = null;
            bool finded = false;

            foreach (var obj in this.Controls)
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
                this.Widget.Window?.InvalidateRect(new Gdk.Rectangle(rc.X, rc.Y, rc.Width, rc.Height), invalidateChildren);
                if (invalidateChildren == true && this.Widget is Gtk.Container container)
                {
                    foreach (var child in container.Children)
                        child.Window?.InvalidateRect(child.Allocation, invalidateChildren);
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
                this.Widget.Window?.InvalidateRect(Widget.Allocation, invalidateChildren);
            }
        }

        public virtual object Invoke(Delegate method)
        {
            return Invoke(method, null);
        }

        public virtual object Invoke(Delegate method, params object[] args)
        {
            object result = null;
            Gdk.Threads.AddIdle(0, () => {
                result = method?.DynamicInvoke(args);
                return false;
            });
            return result;
        }
        public virtual void Invoke(Action method)
        {
            Gdk.Threads.AddIdle(0, () => {
                method?.Invoke();
                return false;
            });
        }
        public virtual ENTRY Invoke<ENTRY>(Func<ENTRY> method)
        {
            ENTRY result = default(ENTRY);
            Gdk.Threads.AddIdle(0, () => {
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
            if (Widget != null && this.Widget.Window != null)
            {
                this.Widget.Window.GetOrigin(out int x, out int y);
                if (p.X > x && p.Y > y)
                    return new Point(p.X - x, p.Y - y);
            }
            return new Point(p.X, p.Y);
        }

        public virtual Point PointToScreen(Point p)
        {
            if (Widget != null && this.Widget.Window != null)
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
            if (Widget != null && this.Widget.Window != null)
            {
                this.Widget.Window.GetPosition(out int x, out int y);
                if (r.X > x && r.Y > y)
                    return new Rectangle(r.X - x, r.Y - y, r.Width, r.Height);
            }
            return new Rectangle(r.X, r.Y, r.Width, r.Height);
        }

        public virtual Rectangle RectangleToScreen(Rectangle r)
        {
            if (Widget != null && this.Widget.Window != null)
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
                this.Widget.QueueDraw();
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
            set
            {
                _BorderStyle = value;
                if (value == BorderStyle.FixedSingle)
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
            this.Widget?.Hide();
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
            if (this.Widget != null && this.Widget.Window != null)
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
        protected override void Dispose(bool disposing)
        {
            try
            {
                this.Widget?.Dispose();
                this.GtkControl = null;
                surface?.Dispose();
                context?.Dispose();
                Image?.Dispose();
                BackgroundImage?.Dispose();
            }
            catch { }
            try
            {
                if (Controls != null)
                {
                    foreach (Control c in Controls)
                        c?.Dispose();
                    Controls.Clear();
                }
            }
            catch { }
            GC.SuppressFinalize(this);
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
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnLoad(EventArgs e)
        {

        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMove(EventArgs e)
        {

        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnValidating(CancelEventArgs e)
        {

        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnValidated(CancelEventArgs e)
        {

        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnResize(EventArgs e)
        {
            
        }
        public virtual void PerformClick()
        {
            OnClick(EventArgs.Empty);
            Click?.Invoke(this, new EventArgs());
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnClick(EventArgs e)
        {

        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseDoubleClick(MouseEventArgs e)
        {

        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseClick(MouseEventArgs e)
        {

        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseDown(MouseEventArgs e)
        {

        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseUp(MouseEventArgs e)
        {

        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseEnter(EventArgs e)
        {

        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseHover(EventArgs e)
        {

        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseLeave(EventArgs e)
        {

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

        public void SetBounds(Rectangle bounds, BoundsSpecified specified)
        {
            throw new NotImplementedException();
        }

        void IArrangedElement.PerformLayout(IArrangedElement affectedElement, string propertyName)
        {
            throw new NotImplementedException();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseMove(MouseEventArgs e)
        {
            
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseWheel(MouseEventArgs e)
        {
            
        }
    }
}
