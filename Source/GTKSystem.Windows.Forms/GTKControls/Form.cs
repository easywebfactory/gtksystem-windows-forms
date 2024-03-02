/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */

using Gtk;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;


namespace System.Windows.Forms
{
    [DesignerCategory("Form")]
    [DefaultEvent(nameof(Load)),
    InitializationEvent(nameof(Load))]
    public partial class Form : WidgetContainerControl<Gtk.Window>, IWin32Window
    {
        private Gtk.Fixed _body = new Gtk.Fixed();
        private Gtk.ScrolledWindow scrollwindow = new Gtk.ScrolledWindow();
        private Gtk.Layout windowbody = new Gtk.Layout(new Gtk.Adjustment(IntPtr.Zero), new Gtk.Adjustment(IntPtr.Zero));

        private ObjectCollection _ObjectCollection;

        public override event EventHandler SizeChanged;
        public Form() : base(WindowType.Toplevel)
        {
            Init();
        }
        public Form(string title) : base()
        {
            base.Control.Title = title;
            Init();
        }

        private void Init()
        {
            this.Control.StyleContext.AddClass("Form");
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
            base.Control.WindowPosition = Gtk.WindowPosition.Center;
            base.Control.BorderWidth = 1;
            base.Control.SetDefaultSize(100, 100);
            base.Control.Realized += Control_Realized;
            base.Control.ResizeChecked += Form_ResizeChecked;
            base.Control.ButtonReleaseEvent += Body_ButtonReleaseEvent;

            base.Control.Shown += Control_Shown;
            base.Control.DeleteEvent += Control_DeleteEvent;

            WindowBackgroundImage.MarginStart = 0;
            WindowBackgroundImage.MarginTop = 0;
            WindowBackgroundImage.Valign = Gtk.Align.Fill;
            WindowBackgroundImage.Halign = Gtk.Align.Fill;
            WindowBackgroundImage.Expand = true;
            WindowBackgroundImage.Hexpand = true;
            WindowBackgroundImage.Vexpand = true;
            WindowBackgroundImage.Drawn += Bg_Drawn;

            windowbody.Put(WindowBackgroundImage, 0, 0);
            windowbody.Put(scrollwindow, 0, 0);
            this.Load += Form_Load;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            
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
            if(this.MaximizeBox==false && this.MinimizeBox == false)
            {
                this.Control.TypeHint = Gdk.WindowTypeHint.Dialog;
            }
            else if (this.MaximizeBox == false && this.MinimizeBox == true)
            {
                this.Control.Resizable = false;
            }
            else if (this.MaximizeBox == true && this.MinimizeBox == false)
            {
                this.Control.Resizable = true;
                this.Control.SkipTaskbarHint = true;
            }
            try
            {
                if (this.ShowIcon)
                {
                    if (this.Icon != null)
                    {
                        if (this.Icon.Pixbuf != null)
                            this.Control.Icon = this.Icon.Pixbuf;
                        else if (this.Icon.PixbufData != null)
                            this.Control.Icon = new Gdk.Pixbuf(this.Icon.PixbufData);
                        else if (this.Icon.FileName != null && System.IO.File.Exists(this.Icon.FileName))
                            this.Control.SetIconFromFile(this.Icon.FileName);
                        else if (this.Icon.FileName != null && System.IO.File.Exists("Resources\\" + this.Icon.FileName))
                            this.Control.SetIconFromFile("Resources\\" + this.Icon.FileName);
                    }
                }
                else
                {
                    System.IO.Stream sm = typeof(System.Windows.Forms.Form).Assembly.GetManifestResourceStream("GTKSystem.Windows.Forms.Resources.System.view-more.png");
                    this.Control.Icon = new Gdk.Pixbuf(sm);
                }
            }
            catch
            {

            }
            this.Control.SkipTaskbarHint = this.ShowInTaskbar;
 
            if (Load != null)
                Load(this, e);
        }
        private Gtk.Image WindowBackgroundImage = new Gtk.Image();
        private Gdk.Pixbuf backgroundPixbuf;
        private void Bg_Drawn(object o, DrawnArgs args)
        {
            Gdk.Rectangle rec = ((Gtk.Image)o).Allocation;
            if (this.BackColor.Name != "Control" && this.BackColor.Name != "0")
            {
                DrawBackgroundColor(args.Cr, this.BackColor, rec);
            }
            if (BackgroundImage != null)
            {
                if (backgroundPixbuf == null)
                {
                    Gdk.Pixbuf imagePixbuf = new Gdk.Pixbuf(IntPtr.Zero);
                    ScaleImage(rec.Width, rec.Height, ref imagePixbuf, BackgroundImage.PixbufData, PictureBoxSizeMode.AutoSize, BackgroundImageLayout == ImageLayout.None ? ImageLayout.Tile : BackgroundImageLayout);
                    backgroundPixbuf = imagePixbuf.ScaleSimple(imagePixbuf.Width, imagePixbuf.Height, Gdk.InterpType.Tiles);
                }
                args.Cr.Scale(rec.Width * 1.00001 / backgroundPixbuf.Width, rec.Height * 1.00001 / backgroundPixbuf.Height);
                DrawBackgroundImage(args.Cr, backgroundPixbuf, rec);
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void MenuPositionFuncNative(IntPtr menu, out int x, out int y, out bool push_in, IntPtr user_data);
        static MenuPositionFuncNative StatusIconPositionMenuFunc = null;
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void d_gtk_menu_popup(IntPtr menu, IntPtr parent_menu_shell, IntPtr parent_menu_item, MenuPositionFuncNative func, IntPtr data, uint button, uint activate_time);
        private static d_gtk_menu_popup gtk_menu_popup = FuncLoader.LoadFunction<d_gtk_menu_popup>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_menu_popup"));
        public void PresentMenu(Gtk.Menu menu, uint button, uint activate_time)
        {
            gtk_menu_popup(menu == null ? IntPtr.Zero : menu.Handle, IntPtr.Zero, IntPtr.Zero, StatusIconPositionMenuFunc, base.Control.Handle, button, activate_time);
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
                WindowBackgroundImage.WidthRequest = window.AllocatedWidth;
                WindowBackgroundImage.HeightRequest = window.AllocatedHeight;
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
            foreach (Gtk.Widget control in parent.AllChildren)
            {
                if(control is Gtk.ScrolledWindow)
                {

                }
                else if (control != null)
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
                        int width = (parent.WidthRequest > 0 ? parent.WidthRequest : parent.AllocatedWidth) - 3;
                        int height = (parent.HeightRequest > 0 ? parent.HeightRequest : parent.AllocatedHeight) - 3;
 
                        if (dockStyle == DockStyle.Top.ToString())
                        {
                            control.Valign = Gtk.Align.Start;
                            control.Hexpand = true;
                            control.WidthRequest = width;
                        }
                        else if (dockStyle == DockStyle.Bottom.ToString())
                        {
                            control.Valign = Gtk.Align.End;
                            control.Halign = Gtk.Align.Fill;
                            control.Hexpand = true;
                            control.MarginTop = heightIncrement;
                            control.WidthRequest = width;
                        }
                        else if (dockStyle == DockStyle.Left.ToString())
                        {
                            control.Halign = Gtk.Align.Start;
                            control.Vexpand = true;
                            control.HeightRequest = height;
                        }
                        else if (dockStyle == DockStyle.Right.ToString())
                        {
                            control.Halign = Gtk.Align.End;
                            control.Vexpand = true;
                            control.HeightRequest = height;
                            control.MarginStart = widthIncrement;
                        }
                        else if (dockStyle == DockStyle.Fill.ToString())
                        {
                            control.Hexpand = true;
                            control.Vexpand = true;
                            control.HeightRequest = height;
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
            WindowBackgroundImage.WidthRequest = this.Width;
            WindowBackgroundImage.HeightRequest = this.Height;
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
            this.Control.Add(windowbody);
            base.Control.Resizable = this.FormBorderStyle == FormBorderStyle.Sizable || this.FormBorderStyle == FormBorderStyle.SizableToolWindow;
            base.Control.Resizable = true;
            base.Control.ShowAll();
            if (this.WindowState == FormWindowState.Maximized)
            {
                base.Control.Maximize();
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                base.Control.KeepBelow = true;
            }
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
            WindowBackgroundImage.WidthRequest = this.Width;
            WindowBackgroundImage.HeightRequest = this.Height;

            int irun = -9;
            if (owner != null)
            {
                Gtk.Window ownerWindow = ((Form)owner).Control;
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
        public override string Text { get { return base.Control.Title; } set { base.Control.Title = value; } }
        public override Size ClientSize
        {
            get
            {
                return new Size(base.Control.WidthRequest, base.Control.HeightRequest);
            }
            set
            {
                base.Control.SetDefaultSize(value.Width, value.Height);
                base.Width = value.Width;
                base.Height = value.Height;
            }
        }
        //public override Rectangle ClientRectangle { get; }

        public SizeF AutoScaleDimensions { get; set; }
        public AutoScaleMode AutoScaleMode { get; set; }
        public FormBorderStyle FormBorderStyle
        {
            get { return base.Control.Resizable == true ? FormBorderStyle.Sizable : FormBorderStyle.None; }
            set { 
                base.Control.Resizable = value == FormBorderStyle.Sizable; 
                if (value == FormBorderStyle.None)
                {            
                    this.Control.Titlebar =new Gtk.Fixed() { HeightRequest = 0 }; 
                }
                else if (value == FormBorderStyle.FixedToolWindow)
                {
                    this.Control.TypeHint = Gdk.WindowTypeHint.Dialog;
                }
                else if (value == FormBorderStyle.SizableToolWindow)
                {
                    this.Control.TypeHint = Gdk.WindowTypeHint.Dialog;
                }
                else
                {
                    this.Control.TypeHint = Gdk.WindowTypeHint.Normal;
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
            this.Control.Close(); 
        }
        public override void Hide()
        {
            if (dialogWindow != null)
            {
                dialogWindow.Hide();
            }
            this.Control.Hide();
        }

        public override ObjectCollection Controls { get { return _ObjectCollection; } }

        public bool MaximizeBox { get; set; }
        public bool MinimizeBox { get; set; }
        public double Opacity { get { return base.Control.Opacity; } set { base.Control.Opacity = value; } }
        public bool ShowIcon { get; set; } = true;
        public bool ShowInTaskbar { get; set; } = true;
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

        public override IntPtr Handle => base.Control.OwnedHandle;

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
    public class FormSite : ISite
    {
        private Form form;
        public FormSite(Form form)
        {
            this.form = form;
        }
        public IComponent Component => this.form;

        public IContainer Container => throw new NotImplementedException();

        public bool DesignMode => false;

        public string Name { get => form.Text; set { } }

        public object GetService(Type serviceType)
        {
            return Activator.CreateInstance(serviceType);
        }
    }
}

