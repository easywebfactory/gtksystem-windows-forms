/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 */

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using GTKSystem.Windows.Forms.Resources;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Form")]
    [DefaultEvent(nameof(Load)),
    InitializationEvent(nameof(Load))]
    public partial class Form : ContainerControl, IWin32Window
    {
        private Gtk.Application app = Application.Init();
        public FormBase self = new FormBase();
        public override object GtkControl { get => self; }
        protected override IScrollableBoxBase scrollbase { get => self; set => base.scrollbase = value; }
        private ControlCollection _ObjectCollection;
        public Form() : base()
        {
            self.Override.sender = this;
            Init();
        }
        public Form(string title) : this()
        {
            self.Title = title;
        }
        private void Init()
        {
            _ObjectCollection = new ControlCollection(this, self.contaner);
            self.Shown += Control_Shown;
            self.CloseWindowEvent += Self_CloseWindowEvent;
        }
        private bool Self_CloseWindowEvent(object sender, EventArgs e)
        {
            FormClosingEventArgs closing = new FormClosingEventArgs(CloseReason.UserClosing, false);
            if (FormClosing != null)
                FormClosing(this, closing);

            if (closing.Cancel == false)
            {
                if (FormClosed != null)
                    FormClosed(this, new FormClosedEventArgs(CloseReason.UserClosing));

                this.Dispose();
            }

            return closing.Cancel;
        }
        private bool Is_Control_Shown = false;
        private void Control_Shown(object sender, EventArgs e)
        {
            if (Is_Control_Shown == false)
            {
                Is_Control_Shown = true;
                OnLoadHandler();
            }
            OnShownHandler();
        }
        public override event ScrollEventHandler Scroll
        {
            add { self.Scroll += value; }
            remove { self.Scroll += value; }
        }
        private void OnLoadHandler()
        {
            if (Load != null)
                Load(this, EventArgs.Empty);
        }
        private void OnShownHandler()
        {
            if (Shown != null)
                Shown(this, EventArgs.Empty);
        }
        public override void Show()
        {
            this.Show(null);
        }
        public System.Windows.Forms.Form? Owner
        {
            get => this.Parent as Form;
            set
            {
                if (this.Parent != null && this.Parent is Form parent)
                    parent.self.Group.RemoveWindow(self);
                this.Parent = value;
                if (value != null)
                {
                    self.TransientFor = value.self;
                    self.DestroyWithParent = true;
                    value.self.Group.AddWindow(self);
                }
            }
        }
        public void Show(IWin32Window owner)
        {
            if (owner == this)
            {
                throw new InvalidOperationException("Owns Self Or Owner");
            }
            if (_Created == false && self.IsRealized == false)
            {
                _Created = true;
                if (owner != null && owner is Form parent)
                {
                    Owner = parent;
                }
                if (StartPosition == FormStartPosition.CenterScreen)
                    self.WindowPosition = WindowPosition.Center;
                else if (StartPosition == FormStartPosition.Manual)
                    self.WindowPosition = WindowPosition.Mouse;
                else if (this.Parent != null && StartPosition == FormStartPosition.CenterParent)
                    self.WindowPosition = WindowPosition.CenterOnParent;
                else
                    self.WindowPosition = WindowPosition.Center;

                if (this.MaximizeBox == false)
                {
                    self.Resizable = false;
                }
                self.Resize(self.DefaultWidth, self.DefaultHeight);
                try
                {
                    if (this.ShowIcon)
                    {
                        if (this.Icon != null)
                        {
                            if (this.Icon.Pixbuf != null)
                                self.Icon = this.Icon.Pixbuf;
                            else if (this.Icon.PixbufData != null)
                                self.Icon = new Gdk.Pixbuf(this.Icon.PixbufData);
                        }
                        else
                        {
                            if (System.IO.File.Exists("Resources/icon.png"))
                                self.Icon = new Gdk.Pixbuf("Resources/icon.png");
                            else if (System.IO.File.Exists("Resources/icon.ico"))
                                self.Icon = new Gdk.Pixbuf("Resources/icon.ico");
                        }
                    }
                }
                catch
                {
                    self.Icon = new Gdk.Pixbuf(AssemblyResources.CurrentAssembly, AssemblyResources.ToSystemUri("image-missing16.png"));
                }
            }
            else
            {
                foreach (var item in this.self.Group.ListWindows())
                {
                    item.Deiconify();
                    item.ShowAll();
                }
            }
            self.ShowAll();
        }
        public DialogResult ShowDialog()
        {
            return ShowDialog(null);
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            Show(owner);
            self.TypeHint = Gdk.WindowTypeHint.Dialog;
            int irun = self.Run();
            return this.DialogResult;
        }

        public event EventHandler Shown;
        public event FormClosingEventHandler FormClosing;
        public event FormClosedEventHandler FormClosed;
        public override event EventHandler Load;
        public override string Text { get { return self.Title; } set { self.Title = value; } }
        public override Size ClientSize
        {
            get
            {
                if (self.IsRealized)
                    return new Size(self.ContentArea.AllocatedWidth, self.ContentArea.AllocatedHeight);
                else
                    return new Size(self.DefaultWidth, self.DefaultHeight);
            }
            set
            {
                self.SetDefaultSize(value.Width, value.Height);
            }
        }
        private Point _location = new Point();
        public override Point Location { get => _location; set { Left = value.X; Top = value.Y; } }
        public override int Left { get => _location.X; set { _location.X = value; base.Left = value; self.Move(_location.X, _location.Y); } }
        public override int Top { get => _location.Y; set { _location.Y = value; base.Top = value; self.Move(_location.X, _location.Y); } }
        public SizeF AutoScaleDimensions { get; set; }
        public AutoScaleMode AutoScaleMode { get; set; }
        private FormBorderStyle formBorderStyle = FormBorderStyle.Sizable;
        public FormBorderStyle FormBorderStyle
        {
            get { return formBorderStyle; }
            set
            {
                formBorderStyle = value;
                self.Resizable = value == FormBorderStyle.Sizable || value == FormBorderStyle.SizableToolWindow || value == FormBorderStyle.None;
                if (value == FormBorderStyle.None)
                {
                    self.Decorated = false;
                }
                else
                {
                    self.Decorated = true;
                }
            }
        }
        public FormStartPosition StartPosition { get; set; }
        private FormWindowState _WindowState = FormWindowState.Normal;
        public FormWindowState WindowState
        {
            get
            {
                if (self.Window != null)
                {
                    if (self.Window.State.HasFlag(Gdk.WindowState.Iconified))
                        _WindowState = FormWindowState.Minimized;
                    else if (self.Window.State.HasFlag(Gdk.WindowState.Maximized) || self.Window.State.HasFlag(Gdk.WindowState.Fullscreen))
                        _WindowState = FormWindowState.Maximized;
                    else
                        _WindowState = FormWindowState.Normal;
                }
                return _WindowState;
            }
            set
            {
                _WindowState = value;
                if (value == FormWindowState.Maximized)
                {
                    self.Maximize();
                }
                else if (value == FormWindowState.Minimized)
                {
                    self.Iconify();
                }
                else if (self.IsMapped)
                {
                    self.Deiconify();
                    self.Present();
                }
            }
        }
        public DialogResult DialogResult { get; set; }
        public void Close()
        {
            if (self != null)
            {
                if (self.CloseWindow())
                {
                    this.Dispose(true);
                }
            }
        }
        public override void Hide()
        {
            if (self != null)
            {
                self.Hide();
            }
        }

        public override ControlCollection Controls { get { return _ObjectCollection; } }
        public override Padding Padding
        {
            get => base.Padding;
            set
            {
                base.Padding = value;
                self.contaner.MarginStart = value.Left;
                self.contaner.MarginTop = value.Top;
                self.contaner.MarginEnd = value.Right;
                self.contaner.MarginBottom = value.Bottom;
            }
        }
        public bool MaximizeBox { get => self.MaximizeBox; set => self.MaximizeBox = value && _ControlBox; }
        public bool MinimizeBox { get => self.MinimizeBox; set => self.MinimizeBox = value && _ControlBox; }
        private bool _ControlBox = true;
        public bool ControlBox { get => _ControlBox; set { _ControlBox = value; self.MinimizeBox = value; self.MaximizeBox = value; self.Deletable = value; } }
        public double Opacity { get { return self.Opacity; } set { self.Opacity = value; } }
        public bool ShowIcon { get; set; } = true;
        public bool ShowInTaskbar { get { return self.SkipTaskbarHint == false; } set { self.SkipTaskbarHint = value == false; } }
        public System.Drawing.Icon Icon { get; set; }
        public override void SuspendLayout()
        {
            _Created = false;
        }
        public override void ResumeLayout(bool resume)
        {
        }

        public override void PerformLayout()
        {
        }
        public bool Activate()
        {
            return self.Activate();
        }
        public bool TopMost
        {
            get { return self.IsActive; }
            set
            {
                self.KeepAbove = value;
                if (self.Window != null)
                    if (self.IsVisible == false || self.IsActive == false || self.Window.State.HasFlag(Gdk.WindowState.Iconified))
                        self.Present();
                self.Activate();
            }
        }
        public bool TopLevel
        {
            get { return self.IsToplevel; }
            set { }
        }
        public bool KeyPreview { get; set; }
        public MenuStrip MainMenuStrip { get; set; }

        public override IntPtr Handle => self.Handle;
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            self = null;
        }
        #region mdiform
        internal Gtk.ScrolledWindow scrolledWindow;
        internal Gtk.Overlay MdiContainer;
        internal Gtk.Overlay MdiContainer2;
        private System.Windows.Forms.Form? _ActiveMdiChild;
        public System.Windows.Forms.Form? ActiveMdiChild { get => _ActiveMdiChild?.self == null ? null : _ActiveMdiChild; internal set => _ActiveMdiChild = value; }
        public bool IsMdiChild { get => _MdiParent != null; }
        public bool IsMdiContainer { get; set; }
        private System.Windows.Forms.Form? _MdiParent;
        public System.Windows.Forms.Form? MdiParent
        {
            get => _MdiParent;
            set
            {

                if (value == null)
                {
                    if (_MdiParent != null)
                    {
                        self.CloseWindow();
                    }
                    _MdiParent = value;
                }
                else
                {
                    if (value.IsMdiContainer == false)
                        throw new Exception("MdiParent is not MdiContainer");

                    _MdiParent = value;

                    if (_MdiParent.MdiContainer == null)
                    {
                        _MdiParent.MdiContainer2 = new Gtk.Overlay() { NoShowAll=true, WidthRequest=0,HeightRequest=0 };
                        _MdiParent.Controls.Add(_MdiParent.MdiContainer2);
                        _MdiParent.MdiContainer = new Gtk.Overlay();
                        _MdiParent.scrolledWindow = new Gtk.ScrolledWindow();
                        _MdiParent.scrolledWindow.StyleContext.AddClass("MDIPanel");
                        _MdiParent.scrolledWindow.Add(_MdiParent.MdiContainer);
                        _MdiParent.Controls.Add(_MdiParent.scrolledWindow);

                        if (_MdiParent.scrolledWindow.Parent is Gtk.Overlay olay)
                        {
                            int left = 0; int top = 0; int right = 0; int bottom = 0;
                            foreach (var w in olay.Children)
                            {
                                if (w.Halign == Align.Fill && w.Valign == Align.Start)
                                {
                                    top = Math.Max(top, w.HeightRequest + w.MarginTop);
                                }
                                else if (w.Halign == Align.Fill && w.Valign == Align.End)
                                {
                                    bottom += w.HeightRequest;
                                }

                                if (w.Valign == Align.Fill && w.Halign == Align.Start)
                                {
                                    left = Math.Max(left, w.WidthRequest + w.MarginStart);
                                }
                                else if (w.Valign == Align.Fill && w.Halign == Align.End)
                                {
                                    right += w.WidthRequest;
                                }
                            }
                            _MdiParent.scrolledWindow.MarginTop = top;
                            _MdiParent.scrolledWindow.MarginStart = left;
                            _MdiParent.scrolledWindow.MarginEnd = right;
                            _MdiParent.scrolledWindow.MarginBottom = bottom;
                        }
                    }
                    this.self.CanFocus = true;
                    this.self.BorderWidth = 2;
                    this.self.StyleContext.AddClass("MDIForm");
                    this.Size = new Size(self.DefaultWidth, self.DefaultHeight);
                    this.self.Halign = Align.Start;
                    this.self.Valign = Align.Start;
                    this.self.WidgetEventAfter += Self_WidgetEventAfter;
                    _MdiParent.MdiContainer.WidgetEventAfter += Contaner_WidgetEventAfter;
                    _MdiParent.MdiContainer.AddOverlay(self);
                }
            }
        }

        private void Contaner_WidgetEventAfter(object o, WidgetEventAfterArgs args)
        {
            if (args.Event.Type == Gdk.EventType.ButtonRelease)
            {
                Gtk.Overlay lay = o as Gtk.Overlay;
                UpdateMDILayout(lay);
            }
        }
        private void UpdateMDILayout(Gtk.Overlay lay)
        {
            if (lay != null)
            {
                lay.SetSizeRequest(-1, -1);
                foreach (var widget in lay.Children)
                {
                    if (widget.AllocatedWidth > 1 && widget.AllocatedWidth + widget.MarginStart > lay.WidthRequest)
                        lay.WidthRequest = widget.AllocatedWidth + widget.MarginStart;

                    if (widget.AllocatedHeight > 1 && widget.AllocatedHeight + widget.MarginTop > lay.HeightRequest)
                        lay.HeightRequest = widget.AllocatedHeight + widget.MarginTop;
                }
            }
        }

        private double prevmarginstart = 0;
        private double prevmargintop = 0;
        private double prevwidth = 0;
        private double prevheight = 0;
        private void Self_WidgetEventAfter(object o, WidgetEventAfterArgs args)
        {
            Gtk.Dialog owidget = o as Gtk.Dialog;
            if (args.Event is Gdk.EventMotion motion && motion.State.HasFlag(Gdk.ModifierType.Button1Mask))
            {
                owidget.Window.GetOrigin(out int x, out int y);
                if (motion.XRoot > x && motion.YRoot < y + 50)
                {
                    owidget.MarginStart = Math.Max(0, (int)(prevmarginstart + motion.XRoot));
                    owidget.MarginTop = Math.Max(0, (int)(prevmargintop + motion.YRoot));
                }
                if (motion.XRoot > x + owidget.AllocatedWidth - 10 || motion.YRoot > y + owidget.AllocatedHeight - 10)
                {
                    owidget.WidthRequest = Math.Max(0, (int)(prevwidth + motion.XRoot));
                    owidget.HeightRequest = Math.Max(0, (int)(prevheight + motion.YRoot));
                }

            }
            else if (args.Event.Type == Gdk.EventType.ButtonPress && args.Event is Gdk.EventButton btn)
            {
                prevmarginstart = owidget.MarginStart - btn.XRoot;
                prevmargintop = owidget.MarginTop - btn.YRoot;

                prevwidth = owidget.AllocatedWidth - btn.XRoot;
                prevheight = owidget.AllocatedHeight - btn.YRoot;
            }
            else if (args.Event.Type == Gdk.EventType.ButtonRelease)
            {
                if (owidget.Parent is Gtk.Overlay lay)
                {
                    lay.ReorderOverlay(owidget, lay.Children.Length);
                    if(_MdiParent != null)
                        _MdiParent.ActiveMdiChild = this;
                }
            }
        }

        public System.Windows.Forms.Form[] MdiChildren
        {
            get
            {
                List<System.Windows.Forms.Form> forms = new List<Form>();
                if (this.IsMdiContainer && MdiContainer != null)
                {
                    foreach (var ofm in MdiContainer.Children)
                    {
                        if (ofm.Data.ContainsKey("Control") && ofm.Data["Control"] is Form fm)
                            forms.Add(fm);
                    }
                }
                return forms.ToArray();
            }
        }
        protected void ActivateMdiChild(System.Windows.Forms.Form? form)
        {
            form.Activate();
            ActiveMdiChild = form;
        }
        public void LayoutMdi(System.Windows.Forms.MdiLayout value)
        {
            if (MdiContainer != null)
            {
                if (value == MdiLayout.TileVertical)
                {
                    int x = 0, y = 0;
                    int width = scrolledWindow.Parent.AllocatedWidth - scrolledWindow.MarginStart - scrolledWindow.MarginEnd;
                    int height = scrolledWindow.Parent.AllocatedHeight - scrolledWindow.MarginTop - scrolledWindow.MarginBottom;
                    double cellsize = width * height / MdiContainer.Children.Length;
                    int cellheight = (int)(Math.Sqrt(cellsize) / width * height);
                    int cellwidth = (int)(cellsize / cellheight);
                    cellwidth = (int)(width / Math.Ceiling(width * 1f / cellwidth));
                    Gtk.Widget last = null;
                    foreach (var fm in MdiContainer.Children)
                    {
                        last = fm;
                        fm.MarginStart = x;
                        fm.MarginTop = y;
                        fm.HeightRequest = cellheight;
                        fm.WidthRequest = cellwidth;
                        y += cellheight;
                        if (y + 50 >= height)
                        {
                            x += cellwidth;
                            y = 0;
                        }

                    }
                    if (height > y && last != null)
                        last.HeightRequest = last.HeightRequest + height - y;

                    MdiContainer.QueueResize();
                    MdiContainer.QueueDraw();
                }
                else if (value == MdiLayout.TileHorizontal)
                {
                    int x = 0, y = 0;
                    int width = scrolledWindow.Parent.AllocatedWidth - scrolledWindow.MarginStart - scrolledWindow.MarginEnd;
                    int height = scrolledWindow.Parent.AllocatedHeight - scrolledWindow.MarginTop - scrolledWindow.MarginBottom;
                    double cellsize = width * height / MdiContainer.Children.Length;
                    int cellheight = (int)(Math.Sqrt(cellsize) / width * height);
                    int cellwidth = (int)(cellsize / cellheight);
                    cellwidth = (int)(width / Math.Ceiling(width * 1f / cellwidth));
                    Gtk.Widget last = null;
                    foreach (var fm in MdiContainer.Children)
                    {
                        last = fm;
                        fm.MarginStart = x;
                        fm.MarginTop = y;
                        fm.HeightRequest = cellheight;
                        fm.WidthRequest = cellwidth;
                        x += cellwidth;
                        if (x + 50 >= width)
                        {
                            x = 0;
                            y += cellheight;
                        }
                    }
                    if (width > x && last != null)
                        last.WidthRequest = last.WidthRequest + width - x;

                    MdiContainer.QueueResize();
                    MdiContainer.QueueDraw();
                }
                else if (value == MdiLayout.Cascade || value == MdiLayout.ArrangeIcons)
                {
                    if (this.IsMdiContainer && MdiContainer != null)
                    {
                        int x = 0, y = 0;
                        foreach (var fm in MdiContainer.Children)
                        {
                            fm.MarginStart = x;
                            fm.MarginTop = y;
                            x += 30;
                            y += 30;

                        }
                        MdiContainer.QueueResize();
                        MdiContainer.QueueDraw();
                    }
                }
                GLib.Timeout.Add(500, () =>
                {
                    UpdateMDILayout(MdiContainer);
                    return false;
                });

            }
        }
        #endregion
    }
}

