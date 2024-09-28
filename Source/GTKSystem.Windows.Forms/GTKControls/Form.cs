/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GLib;
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace System.Windows.Forms
{
    [DesignerCategory("Form")]
    [DefaultEvent(nameof(Load)),
    InitializationEvent(nameof(Load))]
    public partial class Form: ScrollableControl, IWin32Window
    {
        private Gtk.Application app = Application.Init();
        public FormBase self = new FormBase();
        public override object GtkControl { get => self; }
        private Gtk.Fixed _body = new Gtk.Fixed();
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

        public Form(string title, Window parent) : base()
        {
            self.Title = title;
            Init();
        }
        private void Init()
        {
            this.SetScrolledWindow((IScrollableBoxBase)self);
            _body.Valign = Gtk.Align.Fill;
            _body.Halign = Gtk.Align.Fill;
            _body.Expand = true;
            _body.Hexpand = true;
            _body.Vexpand = true;
            self.ScrollView.Child = _body;
            _ObjectCollection = new ObjectCollection(this, _body);
            self.ResizeChecked += Form_ResizeChecked;
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
            }
            return closing.Cancel == false;
        }
        private void Control_Shown(object sender, EventArgs e)
        {
            if (Shown != null)
                Shown(this, e);
        }

        int resizeWidth= 0;
        int resizeHeight= 0;
        private void Form_ResizeChecked(object sender, EventArgs e)
        {
            if (self.Resizable == true && self.IsMapped)
            {
                if (self.ContentArea.AllocatedWidth != resizeWidth || self.ContentArea.AllocatedHeight != resizeHeight)
                {
                    try
                    {
                        resizeWidth = self.ContentArea.AllocatedWidth;
                        resizeHeight = self.ContentArea.AllocatedHeight;
                        int widthIncrement = resizeWidth - self.DefaultWidth;
                        int heightIncrement = resizeHeight - self.DefaultHeight;

                        _body.WidthRequest = resizeWidth; //留出滚动条位置 - (AutoScroll ? self.ScrollArrowVlength : 0)
                        _body.HeightRequest = resizeHeight - self.StatusBarView.AllocatedHeight;

                        Gtk.Application.Invoke(new EventHandler((o, e) =>
                        {
                            self.ResizeControls(widthIncrement, heightIncrement, _body);
                        }));

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("form resize:" + ex.Message);
                    }
                    if (SizeChanged != null)
                        SizeChanged(this, e);
                }
            }
        }
        public override event ScrollEventHandler Scroll
        {
            add { self.Scroll += value; }
            remove { self.Scroll += value; }
        }
        private void OnLoad()
        {
            if (Load != null)
                Load(this, new EventArgs());
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
                //self.ParentWindow = parent.self.Window;
                //self.WindowPosition = Gtk.WindowPosition.CenterOnParent;
            }

            if (self.IsVisible == false)
            {
                if (AutoScroll == true)
                {
                    self.ScrollView.HscrollbarPolicy = PolicyType.Always;
                    self.ScrollView.VscrollbarPolicy = PolicyType.Always;
                }
                else
                {
                    self.ScrollView.HscrollbarPolicy = PolicyType.External;
                    self.ScrollView.VscrollbarPolicy = PolicyType.External;
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
                    }
                    else
                    {
                        System.IO.Stream sm = typeof(System.Windows.Forms.Form).Assembly.GetManifestResourceStream("GTKSystem.Windows.Forms.Resources.System.view-more.png");
                        self.Icon = new Gdk.Pixbuf(sm);
                    }
                }
                catch
                {

                }

                OnLoad();
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

        public bool ActivateControl(object active)
        {
            if (active is Gtk.Widget wg)
            {
                wg.SetStateFlags(StateFlags.Active, false);
                return true;
            }
            return false;
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

        private bool _topMost = false;
        public bool TopMost
        {
            get => _topMost;
            set
            {
                if (_topMost != value)
                {
                    var gtkWindow = Widget.Toplevel as Gtk.Window;
                    _topMost = value;
                    gtkWindow.KeepAbove = _topMost;
                }
            }
        }
    }

    public class BindingContext : ContextBoundObject
    {
    }
}

