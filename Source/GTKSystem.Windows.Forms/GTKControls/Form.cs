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
        private Gtk.Overlay contanter = new Gtk.Overlay();
        private ObjectCollection _ObjectCollection;
        public override event EventHandler SizeChanged;

        public Form() : base()
        {
            Init();
        }
        public Form(string title) : this()
        {
            self.Title = title;
        }
        private void Init()
        {
            this.SetScrolledWindow(self);
            contanter.Valign = Gtk.Align.Fill;
            contanter.Halign = Gtk.Align.Fill;
            contanter.Hexpand = true;
            contanter.Vexpand = true;
            contanter.MarginBottom = 0;
            contanter.MarginEnd = 0;
            contanter.Add(new Gtk.Fixed() { Halign = Align.Fill, Valign = Align.Fill });
            self.ScrollView.Child = contanter;
            _ObjectCollection = new ObjectCollection(this, contanter);
            self.ResizeChecked += Self_ResizeChecked;
            self.Shown += Control_Shown;
            self.CloseWindowEvent += Self_CloseWindowEvent;
        }

        private void Self_ResizeChecked(object sender, EventArgs e)
        {
            if (SizeChanged != null)
                SizeChanged(this, EventArgs.Empty);
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
            }
            return closing.Cancel == false;
        }
        private bool Is_Control_Shown = false;
        private void Control_Shown(object sender, EventArgs e)
        {
            if (Is_Control_Shown == false)
            {
                Is_Control_Shown = true;
                if (self.Titlebar is Gtk.HeaderBar titlebar)
                {
                    titlebar.DecorationLayout = "menu:close";
                    if (formBorderStyle == FormBorderStyle.FixedToolWindow || formBorderStyle == FormBorderStyle.SizableToolWindow)
                    {
                    }
                    else
                    {
                        if (MaximizeBox == true)
                        {
                            Gtk.Button maximize = new Gtk.Button("window-maximize-symbolic", IconSize.SmallToolbar) { Name = "maximize", Visible = true, Relief = ReliefStyle.None, Valign = Align.Center, Halign = Align.Center };
                            maximize.StyleContext.AddClass("maximize");
                            maximize.StyleContext.AddClass("titlebutton");
                            maximize.Clicked += Maximize_Clicked;
                            titlebar.PackEnd(maximize);
                        }
                        if (MinimizeBox == true)
                        {
                            Gtk.Button minimize = new Gtk.Button("window-minimize-symbolic", IconSize.SmallToolbar) { Name = "minimize", Visible = true, Relief = ReliefStyle.None, Valign = Align.Center, Halign = Align.Center };
                            minimize.StyleContext.AddClass("minimize");
                            minimize.StyleContext.AddClass("titlebutton");
                            minimize.Clicked += Minimize_Clicked;
                            titlebar.PackEnd(minimize);
                        }
                    }
                }
                OnLoadHandler();
            }
            OnShownHandler();
        }

        private void Close_Clicked(object sender, EventArgs e)
        {
            self.CloseWindow();
        }

        private void Maximize_Clicked(object sender, EventArgs e)
        {
            Gtk.Button maximize = (Gtk.Button)sender;
            if (maximize.Name == "restore")
            {
                self.Unmaximize();
                maximize.Image = Gtk.Image.NewFromIconName("window-maximize-symbolic", IconSize.SmallToolbar);
                maximize.Name = "maximize";
            }
            else
            {
                self.Maximize();
                maximize.Image = Gtk.Image.NewFromIconName("window-restore-symbolic", IconSize.SmallToolbar);
                maximize.Name = "restore";
            }
        }

        private void Minimize_Clicked(object sender, EventArgs e)
        {
            self.Iconify();
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
                throw new InvalidOperationException("OwnsSelfOrOwner");
            }

            if (base.Visible)
            {
                throw new InvalidOperationException("ShowDialogOnVisible");
            }

            if (!base.Enabled)
            {
                throw new InvalidOperationException("ShowDialogOnDisabled");
            }

            if (owner != null && owner is Form parent)
            {
                this.Parent = parent;
                self.SetPosition(WindowPosition.CenterOnParent);
                self.Activate();
            }

            if (self.IsVisible == false)
            {
                if (AutoScroll == true)
                {
                    self.ScrollView.HscrollbarPolicy = PolicyType.Automatic;
                    self.ScrollView.VscrollbarPolicy = PolicyType.Automatic;
                }
                else
                {
                    self.ScrollView.HscrollbarPolicy = PolicyType.Never;
                    self.ScrollView.VscrollbarPolicy = PolicyType.Never;
                }

                this.FormBorderStyle = this.FormBorderStyle;
                if (this.MaximizeBox == false && this.MinimizeBox == false)
                {
                    self.TypeHint = Gdk.WindowTypeHint.Dialog;
                }
                else if (this.MaximizeBox == false && this.MinimizeBox == true)
                {
                    self.Resizable = false;
                }
                self.Resize(self.DefaultWidth, self.DefaultHeight);

                if (this.WindowState == FormWindowState.Maximized)
                {
                    self.Maximize();
                }
                else if (this.WindowState == FormWindowState.Minimized)
                {
                    self.Iconify();
                }
                if (self.IsMapped == false)
                {
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
                                else if (this.Icon.FileName != null && System.IO.File.Exists(this.Icon.FileName))
                                    self.SetIconFromFile(this.Icon.FileName);
                                else if (this.Icon.FileName != null && System.IO.File.Exists("Resources\\" + this.Icon.FileName))
                                    self.SetIconFromFile("Resources\\" + this.Icon.FileName);
                            }
                            Gtk.HeaderBar titlebar = (Gtk.HeaderBar)self.Titlebar;
                            Gtk.Image flag = new Gtk.Image(self.Icon);
                            flag.Visible = true;
                            titlebar.PackStart(flag);
                        }
                        else
                        {
                            self.Icon = new Gdk.Pixbuf(this.GetType().Assembly, "GTKSystem.Windows.Forms.Resources.System.view-more.png");
                        }

                    }
                    catch
                    {

                    }
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
            if (owner == this)
            {
                throw new ArgumentException("OwnsSelfOrOwner", "showDialog");
            }

            if (base.Visible)
            {
                throw new InvalidOperationException("ShowDialogOnVisible");
            }

            if (!base.Enabled)
            {
                throw new InvalidOperationException("ShowDialogOnDisabled");
            }
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
        public FormBorderStyle formBorderStyle = FormBorderStyle.Sizable;
        public FormBorderStyle FormBorderStyle
        {
            get { return formBorderStyle; }
            set {
                formBorderStyle = value;
                self.Resizable = value == FormBorderStyle.Sizable || value == FormBorderStyle.SizableToolWindow;
                if (value == FormBorderStyle.None)
                {
                    self.Decorated = false; //删除工具栏
                }
                else if (value == FormBorderStyle.FixedToolWindow)
                {
                    self.Decorated = true;
                    self.TypeHint = Gdk.WindowTypeHint.Dialog;
                }
                else if (value == FormBorderStyle.SizableToolWindow)
                {
                    self.Decorated = true;
                    self.TypeHint = Gdk.WindowTypeHint.Dialog;
                }
                else
                {
                    self.Decorated = true;
                    self.TypeHint = Gdk.WindowTypeHint.Normal;
                }
            }
        }
        public FormStartPosition StartPosition { get; set; }
        private FormWindowState _WindowState = FormWindowState.Normal;
        public FormWindowState WindowState {
            get { 
                return _WindowState;
            } 
            set
            {
                _WindowState = value;
                if (self.IsMapped)
                {
                    if (value == FormWindowState.Maximized)
                    {
                        self.Maximize();
                    }
                    else if (value == FormWindowState.Minimized)
                    {
                        self.Iconify();
                    }
                }
            } 
        }
        public DialogResult DialogResult { get; set; }
        public void Close() {
            if (self != null)
            {
                self.CloseWindow();
            }
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
        public bool MaximizeBox { get; set; } = true;
        public bool MinimizeBox { get; set; } = true;
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
            _Created = resume == false;
        }

        public override void PerformLayout()
        {
            _Created = true;
        }
        public bool Activate()
        {
            return self.Activate();
        }
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

    public class BindingContext : ContextBoundObject
    {
    }
}

