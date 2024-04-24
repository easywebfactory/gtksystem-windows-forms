/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */

using GLib;
using Gtk;
using GTKSystem.Windows.Forms;
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
    public partial class Form: ScrollableControl, IWin32Window // WidgetContainerControl<Form.GtkWindow>, IWin32Window
    {
        private Gtk.Application app = Application.Init();
        private GtkWindow self = new GtkWindow(WindowType.Toplevel);
        public override Widget Widget => self;

        private Gtk.Fixed _body = new Gtk.Fixed();
        private Gtk.ScrolledWindow scrollwindow = new Gtk.ScrolledWindow();
        private Gtk.Layout windowbody = new Gtk.Layout(new Gtk.Adjustment(IntPtr.Zero), new Gtk.Adjustment(IntPtr.Zero));
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
            self.StyleContext.AddClass("Form");
            scrollwindow.Valign = Gtk.Align.Fill;
            scrollwindow.Halign = Gtk.Align.Fill;
            scrollwindow.Expand = true;
            scrollwindow.Hexpand = true;
            scrollwindow.Vexpand = true;
            scrollwindow.HscrollbarPolicy = PolicyType.Always;
            scrollwindow.VscrollbarPolicy = PolicyType.Always;
            _body.Valign = Gtk.Align.Fill;
            _body.Halign = Gtk.Align.Fill;
            _body.Expand = true;
            _body.Hexpand = true;
            _body.Vexpand = true;
            scrollwindow.Child = _body;
            windowbody.Valign = Gtk.Align.Fill;
            windowbody.Halign = Gtk.Align.Fill;
            windowbody.Expand = true;
            windowbody.Hexpand = true;
            windowbody.Vexpand = true;


            _ObjectCollection = new ObjectCollection(this, _body);
            self.WindowPosition = Gtk.WindowPosition.Center;
            self.BorderWidth = 1;
            self.SetDefaultSize(100, 100);
            self.Realized += Control_Realized;
            self.ResizeChecked += Form_ResizeChecked;
            self.ButtonReleaseEvent += Body_ButtonReleaseEvent;

            self.Shown += Control_Shown;
            self.DeleteEvent += Control_DeleteEvent;

            windowbody.Put(scrollwindow, 0, 0);
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

        private void Control_Realized(object sender, EventArgs e)
        {
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
        private Gdk.Pixbuf backgroundPixbuf;

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
                base.ContextMenuStrip.Control.ShowAll();
                if (args.Event.Button == 3)
                    PresentMenu(base.ContextMenuStrip.Control, args.Event.Button, args.Event.Time);
            }
        }

        private void Form_ResizeChecked(object sender, EventArgs e)
        {
            var window = (Gtk.Window)sender;
            if (window.IsRealized && windowbody.IsRealized)
            {
                scrollwindow.WidthRequest = window.AllocatedWidth;
                scrollwindow.HeightRequest = window.AllocatedHeight;
                _body.WidthRequest = window.AllocatedWidth;
                _body.HeightRequest = window.AllocatedHeight;
                ResizeControls(window, _body, false, null);
            }
            if (SizeChanged != null)
                SizeChanged(this, e);
        }

        private void ResizeControls(Gtk.Window window, Gtk.Container parent, bool isPaned, Gtk.Paned gtkPaned)
        {
            foreach (Gtk.Widget control in parent.Children)
            {
                if (control != null)
                {
                    object dock = control.Data["Dock"];
                    if (dock != null)
                    {
                        string dockStyle = dock.ToString();
                        int widthIncrement = window.AllocatedWidth - window.DefaultSize.Width;
                        int heightIncrement = window.AllocatedHeight - window.DefaultSize.Height;
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
                        if (control is Gtk.TreeView)
                        {
                        }
                        else if (control is Gtk.Paned paned)
                        {
                            ResizeControls(window, paned, true, paned);
                        }
                        else if (control is Gtk.Container container)
                        {
                            ResizeControls(window, container, isPaned, gtkPaned);
                        }
                    }
                    else
                    {
                        if (control is Gtk.TreeView)
                        {
                        }
                        else if (control is Gtk.Paned paned)
                        {
                            ResizeControls(window, paned, true, paned);
                        }
                        else if (control is Gtk.Container container)
                        {
                            ResizeControls(window, container, isPaned, gtkPaned);
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

            if (owner != null && owner is Form)
            {
                this.Parent = ((Form)owner);
            }


            scrollwindow.WidthRequest = this.Width;
            scrollwindow.HeightRequest = this.Height;
            _body.WidthRequest = this.Width;
            _body.HeightRequest = this.Height;

            windowbody.WidthRequest = this.Width;
            windowbody.HeightRequest = this.Height;

            if (AutoScroll == true)
            {
                scrollwindow.HscrollbarPolicy = PolicyType.Always;
                scrollwindow.VscrollbarPolicy = PolicyType.Always;
            }
            else
            {
                scrollwindow.HscrollbarPolicy = PolicyType.Never;
                scrollwindow.VscrollbarPolicy = PolicyType.Never;
            }
            self.Add(windowbody);
            self.Resizable = this.FormBorderStyle == FormBorderStyle.Sizable || this.FormBorderStyle == FormBorderStyle.SizableToolWindow;
            
            if (this.WindowState == FormWindowState.Maximized)
            {
                self.Maximize();
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                self.KeepBelow = true;
            }
            self.ShowAll();
        }

        private Gtk.Dialog dialogWindow;
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

            windowbody.WidthRequest = this.Width;
            windowbody.HeightRequest = this.Height;

            int irun = -9;
            if (owner != null)
            {
                Gtk.Window ownerWindow = ((Form)owner).Widget as Gtk.Window;
                dialogWindow = new Dialog(this.Text, ownerWindow, DialogFlags.DestroyWithParent);
                dialogWindow.SetPosition(Gtk.WindowPosition.CenterOnParent);
 
            }
            else
            {
                dialogWindow = new Dialog();
                dialogWindow.SetPosition(Gtk.WindowPosition.Center);
            }
            dialogWindow.StyleContext.AddClass("Form");
            dialogWindow.DefaultHeight = this.Height;
            dialogWindow.DefaultWidth = this.Width;
            dialogWindow.Response += Dia_Response;
            dialogWindow.ResizeChecked += Form_ResizeChecked;
            dialogWindow.Shown += Control_Shown;
            dialogWindow.DeleteEvent += Control_DeleteEvent;
            if (AutoScroll == true)
            {
                scrollwindow.HscrollbarPolicy = PolicyType.Always;
                scrollwindow.VscrollbarPolicy = PolicyType.Always;
            }
            else
            {
                scrollwindow.HscrollbarPolicy = PolicyType.Never;
                scrollwindow.VscrollbarPolicy = PolicyType.Never;
            }

            dialogWindow.ContentArea.BorderWidth = 0;
            dialogWindow.ContentArea.Spacing = 0;
            dialogWindow.ContentArea.PackStart(windowbody, true, true, 0);
            dialogWindow.Resizable = this.FormBorderStyle == FormBorderStyle.Sizable || this.FormBorderStyle == FormBorderStyle.SizableToolWindow;

            dialogWindow.ShowAll();
            if (this.WindowState == FormWindowState.Maximized)
            {
                dialogWindow.Maximize();
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                dialogWindow.KeepBelow = true;
            }

            irun = dialogWindow.Run();
            return this.DialogResult;
        }

        private void Dia_Response(object o, ResponseArgs args)
        {
            base.Dispose();
            try
            {
                ((Gtk.Dialog)o).Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
        //public override Rectangle ClientRectangle { get; }

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
                    self.Titlebar =new Gtk.Fixed() { HeightRequest = 0 }; 
                }
                else if (value == FormBorderStyle.FixedToolWindow)
                {
                    self.TypeHint = Gdk.WindowTypeHint.Dialog;
                }
                else if (value == FormBorderStyle.SizableToolWindow)
                {
                    self.TypeHint = Gdk.WindowTypeHint.Dialog;
                }
                else
                {
                    self.TypeHint = Gdk.WindowTypeHint.Normal;
                }
            }
        }
        public FormWindowState WindowState { get; set; } = FormWindowState.Normal;
        public DialogResult DialogResult { get; set; }
        public void Close() {
            if (dialogWindow != null)
            {
                dialogWindow.HideOnDelete();
            }
            self.Close(); 
        }
        public override void Hide()
        {
            if (dialogWindow != null)
            {
                dialogWindow.Hide();
            }
            self.Hide();
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

        public override IntPtr Handle => self.OwnedHandle;

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

        public override ImageLayout BackgroundImageLayout { get => self.Override.BackgroundImageLayout; set => self.Override.BackgroundImageLayout = value; }
        public override Drawing.Image BackgroundImage { get => self.Override.BackgroundImage; set => self.Override.BackgroundImage = value; }
        public override Color BackColor { get => self.Override.BackColor.HasValue ? self.Override.BackColor.Value : Color.Transparent; set => self.Override.BackColor = value; }

        public override event PaintEventHandler Paint
        {
            add { self.Override.Paint += value; }
            remove { self.Override.Paint -= value; }
        }
        public sealed class GtkWindow : Gtk.Window
        {
            internal GtkWindow(WindowType type) : base(type)
            {
                this.Override = new GtkControlOverride(this);
            }
            internal GtkControlOverride Override;
            protected override void OnShown()
            {
                Override.OnAddClass();
                base.OnShown();
            }
            protected override bool OnDrawn(Cairo.Context cr)
            {
                Gdk.Rectangle rec = this.Allocation;
                Override.OnDrawnBackground(cr, rec);
                Override.OnPaint(cr, rec);
                return base.OnDrawn(cr);
            }
        }

    }

    public class BindingContext : ContextBoundObject
    {
    }
}

