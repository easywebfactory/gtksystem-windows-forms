/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 */

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Form")]
    [DefaultEvent(nameof(Load)),
    InitializationEvent(nameof(Load))]
    public partial class Form: ContainerControl, IWin32Window
    {
        private Gtk.Application app = Application.Init();
        public FormBase self = new FormBase();
        public override object GtkControl { get => self; }
        protected override IScrollableBoxBase scrollbase { get => self; set => base.scrollbase = value; }
        private Gtk.Overlay contanter = new Gtk.Overlay();
        private ObjectCollection _ObjectCollection;
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
            contanter.Valign = Gtk.Align.Fill;
            contanter.Halign = Gtk.Align.Fill;
            contanter.Hexpand = true;
            contanter.Vexpand = true;
            contanter.MarginBottom = 0;
            contanter.MarginEnd = 0;
            Gtk.Viewport viewport = new Gtk.Viewport() { Halign = Align.Fill, Valign = Align.Fill, BorderWidth = 0 };
            viewport.StyleContext.AddClass("Form");
            contanter.Add(viewport);
            self.ScrollView.Child = contanter;
            _ObjectCollection = new ObjectCollection(this, contanter);
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
                    this.Parent = parent;
                    self.TransientFor = parent.self;
                    self.DestroyWithParent = true;
                    parent.self.Group.AddWindow(self);
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
                    self.Icon = new Gdk.Pixbuf(this.GetType().Assembly, "GTKSystem.Windows.Forms.Resources.System.image-missing16.png");
                }
            }
            else
            {
                foreach (var item in this.self.Group.ListWindows())
                {
                    item.Visible = true;
                    item.Window.Show();
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
                return new Size(self.AllocatedWidth, self.AllocatedHeight);
            }
            set
            {
                self.WidthRequest = -1;
                self.HeightRequest = -1;
                self.SetDefaultSize(value.Width, value.Height);
            }
        }
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
        public FormWindowState WindowState {
            get { 
                if (self.Window.State.HasFlag(Gdk.WindowState.Fullscreen))
                    return FormWindowState.Maximized;
                else if (self.Window.State.HasFlag(Gdk.WindowState.Maximized))
                    return FormWindowState.Maximized;
                else if (self.Window.State.HasFlag(Gdk.WindowState.Iconified))
                    return FormWindowState.Minimized;
                else if (self.Window.State.HasFlag(Gdk.WindowState.Withdrawn))
                    return FormWindowState.Minimized;
                else
                    return FormWindowState.Normal;
            }
            set
            {
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
                    if (self.Window.State.HasFlag(Gdk.WindowState.Fullscreen))
                        self.Unfullscreen();
                    else if (self.Window.State.HasFlag(Gdk.WindowState.Maximized))
                        self.Unmaximize();
                    else
                        self.Deiconify();
                }
            }
        }
        public DialogResult DialogResult { get; set; }
        public void Close()
        {
            if (self != null)
            {
                self.CloseWindow();
            }
            this.Dispose();
        }
        public override void Hide()
        {
            if (self != null)
            {
                self.Hide();
            }
        }

        public new ObjectCollection Controls { get { return _ObjectCollection; } }
        public override Padding Padding
        {
            get => base.Padding;
            set
            {
                base.Padding = value;
                contanter.MarginStart = value.Left;
                contanter.MarginTop = value.Top;
                contanter.MarginEnd = value.Right;
                contanter.MarginBottom = value.Bottom;
            }
        }
        public bool MaximizeBox { get => self.MaximizeBox; set => self.MaximizeBox = value; }
        public bool MinimizeBox { get => self.MinimizeBox; set => self.MinimizeBox = value; }
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
        public bool TopMost { get { return self.IsActive; } set { self.KeepAbove = value; } }
        public MenuStrip MainMenuStrip { get; set; }

        public override IntPtr Handle => self.Handle;

        public class ObjectCollection : ControlCollection
        {
            Gtk.Container __owner;
            public ObjectCollection(Control control, Gtk.Container owner) : base(control, owner)
            {
                __owner = owner;
            }

        }

        public class MdiLayout
        {
        }
    }
}

