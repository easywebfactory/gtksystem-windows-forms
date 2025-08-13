/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKApp.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    [DefaultProperty(nameof(Text))]
    [DefaultEvent(nameof(MouseDoubleClick))]
    [Designer($"System.Windows.Forms.Design.NotifyIconDesigner, {AssemblyRef.SystemDesign}")]
    [ToolboxItemFilter("System.Windows.Forms")]
    public sealed partial class NotifyIcon : Component
    {
        internal const int MaxTextSize = 127;
        private static readonly object EVENT_MOUSEDOWN = new();
        private static readonly object EVENT_MOUSEMOVE = new();
        private static readonly object EVENT_MOUSEUP = new();
        private static readonly object EVENT_CLICK = new();
        private static readonly object EVENT_DOUBLECLICK = new();
        private static readonly object EVENT_MOUSECLICK = new();
        private static readonly object EVENT_MOUSEDOUBLECLICK = new();
        private static readonly object EVENT_BALLOONTIPSHOWN = new();
        private static readonly object EVENT_BALLOONTIPCLICKED = new();
        private static readonly object EVENT_BALLOONTIPCLOSED = new();


        private Drawing.Icon? _icon;
        private string _text = string.Empty;
        private ContextMenuStrip? _contextMenuStrip;
        private ToolTipIcon _balloonTipIcon;
        private string _balloonTipText = string.Empty;
        private string _balloonTipTitle = string.Empty;
        private object? _userData;
        private bool _doubleClick; 

        private bool _visible;

        Gtk.StatusIcon statusIcon = null;

        public NotifyIcon()
        {
            statusIcon = Gtk.StatusIcon.NewFromIconName("image-missing");
            statusIcon.ButtonPressEvent += StatusIcon_ButtonPressEvent;
            statusIcon.ButtonReleaseEvent += StatusIcon_ButtonReleaseEvent;
            statusIcon.Activate += StatusIcon_Activate;
        }

        private void StatusIcon_Activate(object sender, EventArgs e)
        {
            OnClick(EventArgs.Empty);
        }

        private void StatusIcon_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
        {
            uint button = args.Event.Button;
            MouseButtons mouseButtons = MouseButtons.None;
            if (button == 1)
                mouseButtons = MouseButtons.Left;
            else if (button == 2)
                mouseButtons = MouseButtons.Middle;
            else if (button == 3)
                mouseButtons = MouseButtons.Right;

            OnMouseUp(new MouseEventArgs(mouseButtons, 1, (int)args.Event.XRoot, (int)args.Event.YRoot, 1));
            OnMouseClick(new MouseEventArgs(mouseButtons, 1, (int)args.Event.XRoot, (int)args.Event.YRoot, 1));
            if (button == 3)
            {
                ContextMenuStrip?.self?.PopupAtPointer(args.Event);
            }
        }

        private void StatusIcon_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            uint button = args.Event.Button;
            MouseButtons mouseButtons = MouseButtons.None;
            if (button == 1)
                mouseButtons = MouseButtons.Left;
            else if (button == 2)
                mouseButtons = MouseButtons.Middle;
            else if (button == 3)
                mouseButtons = MouseButtons.Right;

            OnMouseDown(new MouseEventArgs(mouseButtons, 1, (int)args.Event.XRoot, (int)args.Event.YRoot, 1));

            if (args.Event.Type == Gdk.EventType.TwoButtonPress || args.Event.Type == Gdk.EventType.DoubleButtonPress)
            {
                MouseEventArgs mouseArgs2 = new MouseEventArgs(mouseButtons, 2, (int)args.Event.XRoot, (int)args.Event.YRoot, 0);
                OnMouseDoubleClick(mouseArgs2);
                OnDoubleClick(EventArgs.Empty);
            }
        }

        public NotifyIcon(IContainer container) : this()
        {
            container.Add(this);
        }

        [Localizable(true)]
        [DefaultValue("")]
        public string BalloonTipText
        {
            get
            {
                return _balloonTipText;
            }
            set
            {
                if (value != _balloonTipText)
                {
                    _balloonTipText = value;
                }
            }
        }

        [DefaultValue(ToolTipIcon.None)]
        public ToolTipIcon BalloonTipIcon
        {
            get
            {
                return _balloonTipIcon;
            }
            set
            {
                if (value != _balloonTipIcon)
                {
                    _balloonTipIcon = value;
                }
            }
        }

        [Localizable(true)]
        [DefaultValue("")]
        public string BalloonTipTitle
        {
            get
            {
                return _balloonTipTitle;
            }
            set
            {
                if (value != _balloonTipTitle)
                {
                    _balloonTipTitle = value;
                }
            }
        }

        public event EventHandler? BalloonTipClicked
        {
            add => Events.AddHandler(EVENT_BALLOONTIPCLICKED, value);

            remove => Events.RemoveHandler(EVENT_BALLOONTIPCLICKED, value);
        }

        public event EventHandler? BalloonTipClosed
        {
            add => Events.AddHandler(EVENT_BALLOONTIPCLOSED, value);

            remove => Events.RemoveHandler(EVENT_BALLOONTIPCLOSED, value);
        }

        public event EventHandler? BalloonTipShown
        {
            add => Events.AddHandler(EVENT_BALLOONTIPSHOWN, value);
            remove => Events.RemoveHandler(EVENT_BALLOONTIPSHOWN, value);
        }

        [DefaultValue(null)]
        public ContextMenuStrip? ContextMenuStrip
        {
            get
            {
                return _contextMenuStrip;
            }

            set
            {
                _contextMenuStrip = value;
            }
        }

        [Localizable(true)]
        [DefaultValue(null)]
        public Drawing.Icon? Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    if (statusIcon != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            _icon.Save(ms);
                            statusIcon.Pixbuf = new Gdk.Pixbuf(ms.GetBuffer());
                        }
                    }
                }
            }
        }

        [Localizable(true)]
        [DefaultValue("")]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                value ??= string.Empty;

                if (!value.Equals(_text))
                {
                    if (value.Length > MaxTextSize)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Text), value, "TrayIcon_TextTooLong");
                    }

                    _text = value;

                    if (statusIcon != null)
                    {
                        statusIcon.Title = _text;
                        statusIcon.TooltipText = _text;
                        
                    }
                }
            }
        }

        [Localizable(true)]
        [DefaultValue(false)]
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                if (_visible != value)
                {
                    if (statusIcon != null)
                        statusIcon.Visible = value;
                    _visible = value;

                }
            }
        }

        [Localizable(false)]
        [Bindable(true)]
        [DefaultValue(null)]
        [TypeConverter(typeof(StringConverter))]
        public object? Tag
        {
            get
            {
                return _userData;
            }
            set
            {
                _userData = value;
            }
        }

        public event EventHandler? Click
        {
            add => Events.AddHandler(EVENT_CLICK, value);
            remove => Events.RemoveHandler(EVENT_CLICK, value);
        }

        public event EventHandler? DoubleClick
        {
            add => Events.AddHandler(EVENT_DOUBLECLICK, value);
            remove => Events.RemoveHandler(EVENT_DOUBLECLICK, value);
        }

        public event MouseEventHandler? MouseClick
        {
            add => Events.AddHandler(EVENT_MOUSECLICK, value);
            remove => Events.RemoveHandler(EVENT_MOUSECLICK, value);
        }

        public event MouseEventHandler? MouseDoubleClick
        {
            add => Events.AddHandler(EVENT_MOUSEDOUBLECLICK, value);
            remove => Events.RemoveHandler(EVENT_MOUSEDOUBLECLICK, value);
        }
        public event MouseEventHandler? MouseDown
        {
            add => Events.AddHandler(EVENT_MOUSEDOWN, value);
            remove => Events.RemoveHandler(EVENT_MOUSEDOWN, value);
        }

        public event MouseEventHandler? MouseMove
        {
            add => Events.AddHandler(EVENT_MOUSEMOVE, value);
            remove => Events.RemoveHandler(EVENT_MOUSEMOVE, value);
        }

        public event MouseEventHandler? MouseUp
        {
            add => Events.AddHandler(EVENT_MOUSEUP, value);
            remove => Events.RemoveHandler(EVENT_MOUSEUP, value);
        }
        protected override void Dispose(bool disposing)
        {
            statusIcon?.Dispose();
            _contextMenuStrip = null;
            base.Dispose(disposing);
        }

        private void OnClick(EventArgs e)
        {
            ((EventHandler?)Events[EVENT_CLICK])?.Invoke(this, e);
        }

        private void OnDoubleClick(EventArgs e)
        {
            ((EventHandler?)Events[EVENT_DOUBLECLICK])?.Invoke(this, e);
        }

        private void OnMouseClick(MouseEventArgs mea)
        {
            ((MouseEventHandler?)Events[EVENT_MOUSECLICK])?.Invoke(this, mea);
        }

        private void OnMouseDoubleClick(MouseEventArgs mea)
        {
            ((MouseEventHandler?)Events[EVENT_MOUSEDOUBLECLICK])?.Invoke(this, mea);
        }

        private void OnMouseDown(MouseEventArgs e)
        {
            ((MouseEventHandler?)Events[EVENT_MOUSEDOWN])?.Invoke(this, e);
        }

        private void OnMouseMove(MouseEventArgs e)
        {
            ((MouseEventHandler?)Events[EVENT_MOUSEMOVE])?.Invoke(this, e);
        }

        private void OnMouseUp(MouseEventArgs e)
        {
            ((MouseEventHandler?)Events[EVENT_MOUSEUP])?.Invoke(this, e);
        }

        public void ShowBalloonTip(int timeout)
        {
            ShowBalloonTip(timeout, _balloonTipTitle, _balloonTipText, _balloonTipIcon);
        }
        int offset_bottom = 0;
        public void ShowBalloonTip(int timeout, string tipTitle, string tipText, ToolTipIcon tipIcon)
        {
            Gtk.Window balloonTip = new Gtk.Window(WindowType.Toplevel);
            balloonTip.DestroyWithParent = false;
            balloonTip.SetPosition(WindowPosition.None);
            balloonTip.TypeHint = Gdk.WindowTypeHint.Dialog;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Gtk.Window pwin = Gtk.Window.ListToplevels().FirstOrDefault(x => x.IsVisible);
                balloonTip.TransientFor = pwin == null ? Gtk.Window.ListToplevels()[0] : pwin;
            }
            balloonTip.SkipTaskbarHint = true;
            balloonTip.WidthRequest = 200;
            balloonTip.HeightRequest = 50;
            balloonTip.Decorated = true;
            balloonTip.KeepAbove = true;

            balloonTip.Title = tipTitle ?? "";
            Gtk.Label balloonTipText = new Gtk.Label(tipText) { Xpad = 20, Ypad = 20, Halign = Align.Start };
            balloonTip.Add(balloonTipText);

            if (tipIcon == ToolTipIcon.Info)
                balloonTip.Icon = new Gdk.Pixbuf(this.GetType().Assembly, "GTKSystem.Windows.Forms.Resources.System.dialog-information.png");
            else if (tipIcon == ToolTipIcon.Warning)
                balloonTip.Icon = new Gdk.Pixbuf(this.GetType().Assembly, "GTKSystem.Windows.Forms.Resources.System.dialog-warning.png");
            else if (tipIcon == ToolTipIcon.Error)
                balloonTip.Icon = new Gdk.Pixbuf(this.GetType().Assembly, "GTKSystem.Windows.Forms.Resources.System.dialog-error.png");
            else
                balloonTip.Icon = new Gdk.Pixbuf(this.GetType().Assembly, "GTKSystem.Windows.Forms.Resources.System.dialog-information.png");
            
            balloonTip.DeleteEvent += BalloonTip_DeleteEvent;
            balloonTip.Move(-1000,-1000);

            uint handler = GLib.Timeout.Add((uint)timeout, () =>
            {
                try
                {
                    if (balloonTip != null)
                    {
                        offset_bottom -= balloonTip.AllocatedHeight;
                        if (offset_bottom < 0)
                            offset_bottom = 0;
                        balloonTip.Dispose();
                        balloonTip.Destroy();
                    }
                }
                catch (Exception ex) { }
                return false;
            });
            balloonTip.Data.Add("Timeout", handler);
            balloonTip.ShowAll();
            Gdk.Monitor monitor = Gdk.Display.Default.GetMonitorAtWindow(balloonTip.Window);
            int width = monitor.Workarea.Width;
            int height = monitor.Workarea.Height;
            balloonTip.Move(width - balloonTip.AllocatedWidth - 30, height - balloonTip.AllocatedHeight - 50 - offset_bottom);
            offset_bottom += balloonTip.AllocatedHeight;
        }

        private void BalloonTip_DeleteEvent(object o, DeleteEventArgs args)
        {
            Gtk.Window balloonTip = (Gtk.Window)o;
            try
            {
                if (balloonTip != null)
                {
                    GLib.Timeout.Remove(Convert.ToUInt32(balloonTip.Data["Timeout"]));
                    offset_bottom -= balloonTip.AllocatedHeight;
                    if (offset_bottom < 0)
                        offset_bottom = 0;
                    balloonTip.Dispose();
                }
            }
            catch (Exception ex) { }
        }
    }

}
