/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Atk;
using GLib;
using Gtk;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;


namespace System.Windows.Forms
{
    [DesignerCategory("Form"),
    DefaultEvent(nameof(Load)),
    InitializationEvent(nameof(Load))]
    public partial class Form : WidgetContainerControl<Gtk.Window>, IWin32Window
    {
        private Gtk.Fixed _body = null;
        private ObjectCollection _ObjectCollection;
        private Gtk.Menu contextMenu = new Gtk.Menu();
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
            _body = new Fixed();
            _body.Valign = Gtk.Align.Fill;
            _body.Halign = Gtk.Align.Fill;
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
            WindowBackgroundImage.WidthRequest = this.Width;
            WindowBackgroundImage.HeightRequest = this.Height;
            WindowBackgroundImage.Drawn += Bg_Drawn;
            _body.Add(WindowBackgroundImage);
        }

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
            if (Load != null)
                Load(this, e);
        }
        private Gtk.Image WindowBackgroundImage = new Gtk.Image();
        private Gdk.Pixbuf backgroundPixbuf;
        private void Bg_Drawn(object o, DrawnArgs args)
        {
            Gdk.Rectangle rec = Widget.Allocation;
            if (this.BackColor.Name != "Control" && this.BackColor.Name != "0")
            {
                DrawBackgroundColor(args.Cr, Widget, this.BackColor, rec);
            }
            if (BackgroundImage != null)
            {
                byte[] _BackgroundImageBytes = new byte[BackgroundImage.PixbufData.Length];
                BackgroundImage.PixbufData.CopyTo(_BackgroundImageBytes, 0);

                if (backgroundPixbuf == null)
                {
                    Gdk.Pixbuf imagePixbuf = new Gdk.Pixbuf(IntPtr.Zero);
                    ScaleImage(ref imagePixbuf, _BackgroundImageBytes, PictureBoxSizeMode.AutoSize, BackgroundImageLayout == ImageLayout.None ? ImageLayout.Tile : BackgroundImageLayout);
                    backgroundPixbuf = imagePixbuf.ScaleSimple(imagePixbuf.Width - 8, imagePixbuf.Height - 6, Gdk.InterpType.Tiles);
                }
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
        int width = 0;
        int height = 0;
        private void Form_ResizeChecked(object sender, EventArgs e)
        {
            if (base.Control.IsRealized)
            {
                if (base.Control.Allocation.Width != width || base.Control.Allocation.Height != height)
                {
                    width = base.Control.Allocation.Width;
                    height = base.Control.Allocation.Height;
                    WindowBackgroundImage.WidthRequest = width;
                    WindowBackgroundImage.HeightRequest = height;
                    ResizeChildren(base.Control);
                }
            }
            if (SizeChanged != null)
                SizeChanged(this, e);
        }

        private void ResizeChildren(Gtk.Container container)
        {
            foreach (var o in container.AllChildren)
            {
                if (o is Gtk.Container control)
                {
                    object dock = control.Data["Dock"];
                    if (dock != null)
                    {
                        string dockStyle = dock.ToString();
                        int widthIncrement = base.Control.AllocatedWidth - base.Control.DefaultWidth;
                        int heightIncrement = base.Control.AllocatedHeight - base.Control.DefaultHeight;
                        if (GetParentWidget(container, out Gtk.Widget parent))
                        {
                            int width = parent.WidthRequest - control.MarginStart - ((int)control.BorderWidth);
                            int height = parent.HeightRequest - ((int)control.BorderWidth);
                            if (parent.GetType().Name == "Window")
                            {
                                width = parent.AllocatedWidth - 2;
                                height = parent.AllocatedHeight - 1;
                            }
                            if (parent.GetType().Name == "Dialog")
                            {
                                width = parent.AllocatedWidth - 1;
                                height = parent.AllocatedHeight - 1;
                            }
                            width = width - control.MarginStart - control.MarginStart - 2;
                            height = width - control.MarginTop - control.MarginTop - 1;

                            if (dockStyle == DockStyle.Top.ToString())
                            {
                                control.WidthRequest = width;
                            }
                            else if (dockStyle == DockStyle.Bottom.ToString())
                            {
                                control.WidthRequest = width;

                                if ((int)control.Data["InitMarginTop"] + heightIncrement > 0)
                                    control.MarginTop = (int)control.Data["InitMarginTop"] + heightIncrement;
                            }
                            else if (dockStyle == DockStyle.Left.ToString())
                            {
                                control.HeightRequest = height;
                            }
                            else if (dockStyle == DockStyle.Right.ToString())
                            {
                                control.HeightRequest = height;
                                if ((int)control.Data["InitMarginStart"] + widthIncrement > 0)
                                    control.MarginStart = (int)control.Data["InitMarginStart"] + widthIncrement;
                            }
                            else if (dockStyle == DockStyle.Fill.ToString())
                            {
                                control.HeightRequest = height;
                                control.WidthRequest = width;
                            }
                        }
                    }
                    if (o is Gtk.ScrolledWindow)
                    {

                    }
                    else
                    {
                        ResizeChildren(control);
                    }
                }
            }
        }
        private bool GetParentWidget(Gtk.Widget container, out Gtk.Widget parent)
        {
            parent = container;
            while (parent.Parent != null)
            {
                parent = parent.Parent;
                if (parent.WidthRequest > -1)
                {
                    return true;
                }
            }
            return true;
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
                this.Parent= ((Form)owner);
            }

            if (this.AutoScroll == true)
            {
                Gtk.ScrolledWindow scrollwindow = new Gtk.ScrolledWindow();
                _body.WidthRequest = this.Width;
                _body.HeightRequest = this.Height;
                scrollwindow.Child = _body;
                base.Control.Add(scrollwindow);
            }
            else
            {
                Gtk.Layout laybody = new Gtk.Layout(new Gtk.Adjustment(IntPtr.Zero), new Gtk.Adjustment(IntPtr.Zero));
                laybody.Add(_body);
                base.Control.Add(laybody);
            }

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
            int irun = -9;
            if (owner != null)
            {
                Gtk.Window ownerWindow = ((Form)owner).Control;
                dialogWindow = new Dialog(this.Text, ownerWindow, DialogFlags.DestroyWithParent);
                dialogWindow.SetPosition(Gtk.WindowPosition.CenterOnParent);
                dialogWindow.DefaultHeight = this.Height;
                dialogWindow.DefaultWidth = this.Width;
                dialogWindow.Response += Dia_Response;
                if (this.AutoScroll == true)
                {
                    Gtk.ScrolledWindow scrollwindow = new Gtk.ScrolledWindow();
                    _body.WidthRequest = this.Width;
                    _body.HeightRequest = this.Height;
                    scrollwindow.Child = _body;
                    dialogWindow.ContentArea.PackStart(scrollwindow,true,true,1);
                }
                else
                {
                    Gtk.Layout laybody = new Gtk.Layout(new Gtk.Adjustment(IntPtr.Zero), new Gtk.Adjustment(IntPtr.Zero));
                    laybody.Add(_body);
                    dialogWindow.ContentArea.PackStart(laybody, true, true, 1);
                }
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
            }
            else
            {
                dialogWindow = new Dialog();
                dialogWindow.SetPosition(Gtk.WindowPosition.Center);
                dialogWindow.DefaultHeight = this.Height;
                dialogWindow.DefaultWidth = this.Width;
                dialogWindow.Response += Dia_Response;
                if (this.AutoScroll == true)
                {
                    Gtk.ScrolledWindow scrollwindow = new Gtk.ScrolledWindow();
                    _body.WidthRequest = this.Width;
                    _body.HeightRequest = this.Height;
                    scrollwindow.Child = _body;
                    dialogWindow.ContentArea.PackStart(scrollwindow, true, true, 1);
                }
                else
                {
                    Gtk.Layout laybody = new Gtk.Layout(new Gtk.Adjustment(IntPtr.Zero), new Gtk.Adjustment(IntPtr.Zero));
                    laybody.Add(_body);
                    dialogWindow.ContentArea.PackStart(laybody, true, true, 1);
                }
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

            }
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
                base.Control.Data["InitWidth"] = value.Width;
                base.Control.Data["InitHeight"] = value.Height;
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
            set { base.Control.Resizable = value == FormBorderStyle.Sizable; }
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
}

