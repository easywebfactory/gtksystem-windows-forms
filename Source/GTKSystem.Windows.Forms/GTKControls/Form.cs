/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;


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

        }
        private void Init()
        {
            _body.Valign = Gtk.Align.Fill;
            _body.Halign = Gtk.Align.Fill;
            _body.Expand = true;
            _body.Hexpand = true;
            _body.Vexpand = true;

            self.ScrollArea.Child = _body;
            _ObjectCollection = new ObjectCollection(this, _body);

            self.Mapped += Self_Mapped;
            self.ResizeChecked += Form_ResizeChecked;
            self.ButtonReleaseEvent += Body_ButtonReleaseEvent;

            self.Shown += Control_Shown;
            self.DeleteEvent += Control_DeleteEvent;
        }
        private void Self_Mapped(object sender, EventArgs e)
        {
            //Console.WriteLine("Self_Mapped");
            if (this.MaximizeBox == false && this.MinimizeBox == false)
            {
                self.TypeHint = Gdk.WindowTypeHint.Dialog;
            }
            else if (this.MaximizeBox == false && this.MinimizeBox == true)
            {
                self.Resizable = false;
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

            if (Load != null)
                Load(this, e);
        }

        public override ISite Site { get; set; }
        private void Control_DeleteEvent(object o, DeleteEventArgs args)
        {
            if (FormClosing != null)
                FormClosing(this, new FormClosingEventArgs(CloseReason.UserClosing, false));
            if (FormClosed != null)
                FormClosed(this, new FormClosedEventArgs(CloseReason.UserClosing));
        }

        private void Control_Shown(object sender, EventArgs e)
        {
            if (Shown != null)
                Shown(this, e);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void MenuPositionFuncNative(IntPtr menu, out int x, out int y, out bool push_in, IntPtr user_data);
        static MenuPositionFuncNative StatusIconPositionMenuFunc = null;
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void d_gtk_menu_popup(IntPtr menu, IntPtr parent_menu_shell, IntPtr parent_menu_item, MenuPositionFuncNative func, IntPtr data, uint button, uint activate_time);
        private static d_gtk_menu_popup gtk_menu_popup = FuncLoader.LoadFunction<d_gtk_menu_popup>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_menu_popup"));
        public void PresentMenu(Gtk.Menu menu, uint button, uint activate_time)
        {
            gtk_menu_popup(menu == null ? IntPtr.Zero : menu.Handle, IntPtr.Zero, IntPtr.Zero, StatusIconPositionMenuFunc, self.Handle, button, activate_time);
        }
        private void Body_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
        {
            if (base.ContextMenuStrip != null)
            {
                base.ContextMenuStrip.self.ShowAll();
                if (args.Event.Button == 3)
                    PresentMenu(base.ContextMenuStrip.self, args.Event.Button, args.Event.Time);
            }
        }
        int resizeWidth= 0;
        int resizeHeight= 0;
        private void Form_ResizeChecked(object sender, EventArgs e)
        {
            if (self.Resizable == true)
            {
                if (_body.IsMapped && resizeWidth != self.AllocatedWidth && resizeHeight != self.AllocatedHeight)
                {
                    resizeWidth = self.AllocatedWidth;
                    resizeHeight = self.AllocatedHeight;
                    _body.WidthRequest = self.AllocatedWidth - (AutoScroll ? 18 : 0); //留出滚动条位置
                    _body.HeightRequest = self.AllocatedHeight - (AutoScroll ? 18 : 0);
                    int widthIncrement = self.AllocatedWidth - self.DefaultSize.Width;
                    int heightIncrement = self.AllocatedHeight - self.DefaultSize.Height;
                    ResizeControls(widthIncrement, heightIncrement, _body, false, null);
                }
            }
            if (SizeChanged != null)
                SizeChanged(this, e);

        }

        private void ResizeControls(int widthIncrement, int heightIncrement, Gtk.Container parent, bool isPaned, Gtk.Paned gtkPaned)
        {
            foreach (Gtk.Widget control in parent.Children)
            {
                if (control != null)
                {
                    object dock = control.Data["Dock"];
                    if (dock != null)
                    {
                        string dockStyle = dock.ToString();
                        if (gtkPaned != null)
                        {
                            if (gtkPaned.Orientation == Gtk.Orientation.Vertical)
                                heightIncrement = gtkPaned.Child1.AllocatedHeight - gtkPaned.Child1.HeightRequest;
                            else
                                widthIncrement = gtkPaned.Child1.AllocatedWidth - gtkPaned.Child1.WidthRequest;
                        }

                        Gtk.Widget sizeParent = getSizeParent(control);
                        int width = sizeParent.WidthRequest;
                        int height = sizeParent.HeightRequest;
                        if (dockStyle == DockStyle.Top.ToString())
                        {
                            control.Valign = Gtk.Align.Start;
                            control.Hexpand = true;
                            if (control.WidthRequest > -1 && width > -1)
                                control.WidthRequest = width;
                        }
                        else if (dockStyle == DockStyle.Bottom.ToString())
                        {
                            control.Valign = Gtk.Align.End;
                            control.Halign = Gtk.Align.Fill;
                            control.Hexpand = true;
                            control.MarginTop = heightIncrement;
                            if (control.WidthRequest > -1 && width > -1)
                                control.WidthRequest = width;
                        }
                        else if (dockStyle == DockStyle.Left.ToString())
                        {
                            control.Halign = Gtk.Align.Start;
                            control.Vexpand = true;
                            if (control.HeightRequest > -1 && height > -1)
                                control.HeightRequest = height;
                        }
                        else if (dockStyle == DockStyle.Right.ToString())
                        {
                            control.Halign = Gtk.Align.End;
                            control.Vexpand = true;
                            if (control.HeightRequest > -1 && height > -1)
                                control.HeightRequest = height;
                            control.MarginStart = widthIncrement;
                        }
                        else if (dockStyle == DockStyle.Fill.ToString())
                        {
                            control.Hexpand = true;
                            control.Vexpand = true;
                            if (control.HeightRequest > -1 && height > -1)
                                control.HeightRequest = height;
                            if (control.WidthRequest > -1 && width > -1)
                                control.WidthRequest = width;
                        }
                        if (control is Gtk.MenuBar menuba)
                        {
                            //菜单不用处理
                        }
                        else if (control is Gtk.TreeView)
                        {
                            //树目录不用处理
                        }
                        else if (control is Gtk.Paned paned)
                        {
                            ResizeControls(widthIncrement, heightIncrement, paned, true, paned);
                        }
                        else if (control is Gtk.Container container)
                        {
                            ResizeControls(widthIncrement, heightIncrement, container, isPaned, gtkPaned);
                        }
                    }
                    else
                    {
                        if (control is Gtk.MenuBar)
                        {

                        }
                        else if (control is Gtk.TreeView)
                        {
                        }
                        else if (control is Gtk.Paned paned)
                        {
                            ResizeControls(widthIncrement, heightIncrement, paned, true, paned);
                        }
                        else if (control is Gtk.Container container)
                        {
                            ResizeControls(widthIncrement, heightIncrement, container, isPaned, gtkPaned);
                        }
                    }
                }
            }
        }

        private Gtk.Widget getSizeParent(Gtk.Widget control)
        {
            while (control.Parent != null)
            {
                if (control.Parent.WidthRequest > -1)
                    return control.Parent;
                else
                    return getSizeParent(control.Parent);
            }
            return control.Parent;
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
            }

            _body.WidthRequest = this.Width;
            _body.HeightRequest = this.Height;

            if (AutoScroll == true)
            {
                self.ScrollArea.HscrollbarPolicy = PolicyType.Always;
                self.ScrollArea.VscrollbarPolicy = PolicyType.Always;
            }
            else
            {
                self.ScrollArea.HscrollbarPolicy = PolicyType.External;
                self.ScrollArea.VscrollbarPolicy = PolicyType.External;
            }
            self.Resizable = this.FormBorderStyle == FormBorderStyle.Sizable || this.FormBorderStyle == FormBorderStyle.SizableToolWindow;
            
            if (this.WindowState == FormWindowState.Maximized)
            {
                self.Maximize();
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                self.Iconify();
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
                return new Size(self.WidthRequest, self.HeightRequest);
            }
            set
            {
                self.SetDefaultSize(value.Width, value.Height);
                base.Width = value.Width;
                base.Height = value.Height;
            }
        }
        public override bool AutoScroll { 
            get => base.AutoScroll; 
            set {
                base.AutoScroll = value;
                if (value == true)
                    self.StyleContext.AddClass("ScrollForm");
                else
                    self.StyleContext.RemoveClass("ScrollForm");
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
                    self.BorderWidth = 0;
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
        public FormWindowState WindowState { get; set; } = FormWindowState.Normal;
        public DialogResult DialogResult { get; set; }
        public void Close() {
            if (self != null)
            {
                self.CloseWindow();
                self.Dispose();
            }
        }
        public override void Hide()
        {
            if (self != null)
            {
                self.Hide();
            }
        }

        public override ObjectCollection Controls { get { return _ObjectCollection; } }

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
            return true;
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

