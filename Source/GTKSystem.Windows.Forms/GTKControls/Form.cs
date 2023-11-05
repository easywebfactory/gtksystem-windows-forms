//基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
//使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows和linux运行
//开发联系438865652@qq.com，https://www.cnblogs.com/easywebfactory
using Gtk;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;


namespace System.Windows.Forms
{
    [DesignerCategory("Form"),
    DefaultEvent(nameof(Load)),
    InitializationEvent(nameof(Load))]
    public partial class Form : WidgetControl<Gtk.Dialog>, IWin32Window
    {
        private Gtk.Fixed _body = null;
        private ObjectCollection _ObjectCollection;
        private Gtk.Menu contextMenu = new Gtk.Menu();
        public override event EventHandler SizeChanged;
        public Form() : base("title", new Gtk.Window(WindowType.Toplevel), DialogFlags.Modal)
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
            _body = new Fixed();
            _body.Valign = Gtk.Align.Fill;
            _body.Halign = Gtk.Align.Fill;
            _ObjectCollection = new ObjectCollection(_body);
            base.Control.WindowPosition = Gtk.WindowPosition.Center;
            base.Control.BorderWidth = 1;
            base.Control.SetDefaultSize(100, 100);

            base.Control.Realized += Control_Realized;
            
            base.Control.ResizeChecked += Form_ResizeChecked;
            base.Control.ButtonReleaseEvent += Body_ButtonReleaseEvent;

            base.Control.Shown += Control_Shown;
            base.Control.DeleteEvent += Control_DeleteEvent;
            base.Control.Response += Control_Response;
        }

        private void Control_Response(object o, ResponseArgs args)
        {
            base.Dispose();
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
                    Gtk.Box box = base.Control.ContentArea;
                    ResizeChildren(box);
                }
            }
            if (SizeChanged != null)
                SizeChanged(sender, e);
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
                            //int width = parent.AllocatedWidth - control.MarginStart - ((int)control.BorderWidth);
                            //int height = parent.AllocatedHeight - ((int)control.BorderWidth);
                            //if (parent.GetType().Name == "Window")
                            //{
                            //    width = parent.AllocatedWidth - 1;
                            //    height = parent.AllocatedHeight - 1;
                            //}
                            if (parent.GetType().Name == "Dialog")
                            {
                                width = parent.AllocatedWidth - 1;
                                height = parent.AllocatedHeight - 1;
                            }
                            width = width - control.MarginStart - control.MarginStart - 1;
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

        public void Show()
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
                this.Control.Parent = ((Form)owner).Control;
            }

            if (this.AutoScroll == true)
            {
                Gtk.ScrolledWindow scrollwindow = new Gtk.ScrolledWindow();
                _body.WidthRequest = this.Width;
                _body.HeightRequest = this.Height;
                scrollwindow.Child = _body;
                base.Control.ContentArea.PackStart(scrollwindow, true, true, 0);
            }
            else
            {
                Gtk.Layout laybody = new Gtk.Layout(new Gtk.Adjustment(IntPtr.Zero), new Gtk.Adjustment(IntPtr.Zero));
                laybody.Add(_body);
                base.Control.ContentArea.PackStart(laybody, true, true, 0);
            }
            base.Control.ShowAll();
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

            if (owner != null)
            {
               // ownerWindow = ((Form)owner).Control;
            }

            this.Show(null);
            int irun = base.Control.Run();
           // int irun = -9;
            Gtk.ResponseType resp = Enum.Parse<Gtk.ResponseType>(irun.ToString());
            if (resp == Gtk.ResponseType.Yes)
                return DialogResult.Yes;
            else if (resp == Gtk.ResponseType.No)
                return DialogResult.No;
            else if (resp == Gtk.ResponseType.Ok)
                return DialogResult.OK;
            else if (resp == Gtk.ResponseType.Cancel)
                return DialogResult.Cancel;
            else if (resp == Gtk.ResponseType.Reject)
                return DialogResult.Abort;
            else if (resp == Gtk.ResponseType.Help)
                return DialogResult.Retry;
            else if (resp == Gtk.ResponseType.Close)
                return DialogResult.Ignore;
            else if (resp == Gtk.ResponseType.None)
                return DialogResult.None;
            else if (resp == Gtk.ResponseType.DeleteEvent)
                return DialogResult.None;
            else
                return DialogResult.None;
        }

        public event EventHandler Shown;
        public event FormClosingEventHandler FormClosing;
        public event FormClosedEventHandler FormClosed;
        public override event EventHandler Load;
        public override string Text { get { return base.Control.Title; } set { base.Control.Title = value; } }
        public Size ClientSize
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
        public Rectangle ClientRectangle { get; }

        public SizeF AutoScaleDimensions { get; set; }
        public AutoScaleMode AutoScaleMode { get; set; }
        public FormBorderStyle FormBorderStyle
        {
            get { return base.Control.Resizable == true ? FormBorderStyle.Sizable : FormBorderStyle.None; }
            set { base.Control.Resizable = value == FormBorderStyle.Sizable; }
        }

        public new ObjectCollection Controls { get { return _ObjectCollection; } }

        public object ActiveControl { get; set; }

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

        public bool ActivateControl(Control active)
        {
            return false;
        }

        public bool AutoScroll { get; set; }

        public MenuStrip MainMenuStrip { get; set; }

        public IntPtr Handle => base.Control.OwnedHandle;

        public class ObjectCollection : ControlCollection
        {
            Gtk.Container __owner;
            public ObjectCollection(Gtk.Container owner) : base(owner)
            {
                __owner = owner;
            }
            public static Form ActiveForm { get; }
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            [EditorBrowsable(EditorBrowsableState.Never)]

            public bool AutoScale { get; set; }
            [SettingsBindable(true)]
            public Point Location { get; set; }
            [DefaultValue(false)]

            public bool KeyPreview { get; set; }
            [Browsable(false)]
            [EditorBrowsable(EditorBrowsableState.Advanced)]
            public bool IsRestrictedWindow { get; }
            [DefaultValue(false)]


            public bool IsMdiContainer { get; set; }
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]


            public bool IsMdiChild { get; }

            //[AmbientValue(null)]
            //[Localizable(true)]
            //public Icon Icon { get; set; }

            [DefaultValue(false)]
            public bool HelpButton { get; set; }

            //[Browsable(false)]
            //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

            // public DialogResult DialogResult { get; set; }

            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

            public Point DesktopLocation { get; set; }
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

            public Form ActiveMdiChild { get; }
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]


            public Rectangle DesktopBounds { get; set; }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            [Localizable(true)]
            public Size ClientSize { get; set; }

            [DefaultValue(null)]

            public IButtonControl CancelButton { get; set; }

            [DefaultValue(FormBorderStyle.Sizable)]


            public FormBorderStyle FormBorderStyle { get; set; }

            public Color BackColor { get; set; }
            [Browsable(true)]
            [EditorBrowsable(EditorBrowsableState.Always)]
            public AutoValidate AutoValidate { get; set; }
            [Browsable(true)]
            [DefaultValue(AutoSizeMode.GrowOnly)]
            [Localizable(true)]


            public AutoSizeMode AutoSizeMode { get; set; }
            [Browsable(true)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            [EditorBrowsable(EditorBrowsableState.Always)]
            public bool AutoSize { get; set; }
            [Localizable(true)]
            public bool AutoScroll { get; set; }
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            [EditorBrowsable(EditorBrowsableState.Never)]
            [Localizable(true)]
            public virtual Size AutoScaleBaseSize { get; set; }
            [DefaultValue(typeof(Size), "0, 0")]
            [Localizable(true)]
            [RefreshProperties(RefreshProperties.Repaint)]


            public Size MaximumSize { get; set; }
            [DefaultValue(true)]


            public bool ControlBox { get; set; }
            [DefaultValue(null)]


            [TypeConverter(typeof(ReferenceConverter))]
            public MenuStrip MainMenuStrip { get; set; }
            [Localizable(true)]
            [RefreshProperties(RefreshProperties.Repaint)]


            public Size MinimumSize { get; set; }
            [DefaultValue(FormWindowState.Normal)]


            public FormWindowState WindowState { get; set; }


            public Color TransparencyKey { get; set; }
            [DefaultValue(false)]


            public bool TopMost { get; set; }
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            [EditorBrowsable(EditorBrowsableState.Advanced)]
            public bool TopLevel { get; set; }
            [SettingsBindable(true)]
            public string Text { get; set; }
            [Browsable(false)]
            [DefaultValue(true)]

            [EditorBrowsable(EditorBrowsableState.Never)]


            public bool TabStop { get; set; }
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public int TabIndex { get; set; }

            [DefaultValue(FormStartPosition.WindowsDefaultLocation)]
            [Localizable(true)]


            public FormStartPosition StartPosition { get; set; }
            [DefaultValue(SizeGripStyle.Auto)]


            public SizeGripStyle SizeGripStyle { get; set; }
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            [Localizable(false)]
            public Size Size { get; set; }
            [Browsable(false)]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public Padding Margin { get; set; }
            [DefaultValue(true)]


            public bool ShowIcon { get; set; }
            [DefaultValue(false)]
            [Localizable(true)]
            public virtual bool RightToLeftLayout { get; set; }
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public Rectangle RestoreBounds { get; }
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public Form Owner { get; set; }
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public Form[] OwnedForms { get; }
            [DefaultValue(1)]
            [TypeConverter(typeof(OpacityConverter))]
            public double Opacity { get; set; }
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public bool Modal { get; }
            [DefaultValue(true)]


            public bool MinimizeBox { get; set; }
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]


            public Form MdiParent { get; set; }
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]


            public Form[] MdiChildren { get; }
            [DefaultValue(true)]


            public bool MaximizeBox { get; set; }
            [DefaultValue(true)]


            public bool ShowInTaskbar { get; set; }
            [DefaultValue(null)]

            public IButtonControl AcceptButton { get; set; }
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

            public bool AllowTransparency { get; set; }

            public event FormClosingEventHandler FormClosing;
            [Browsable(false)]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public event EventHandler TabIndexChanged;


            public event EventHandler MinimumSizeChanged;
            [Browsable(false)]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public event EventHandler MarginChanged;


            public event EventHandler ResizeEnd;


            public event EventHandler MaximumSizeChanged;
            [Browsable(true)]
            [EditorBrowsable(EditorBrowsableState.Always)]


            public event EventHandler AutoSizeChanged;
            [Browsable(true)]
            [EditorBrowsable(EditorBrowsableState.Always)]
            public event EventHandler AutoValidateChanged;


            public event EventHandler MdiChildActivate;


            public event FormClosedEventHandler FormClosed;


            public event EventHandler Deactivate;


            public event EventHandler Load;


            public event EventHandler MaximizedBoundsChanged;
            [Browsable(false)]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public event EventHandler TabStopChanged;
            [Browsable(true)]
            [EditorBrowsable(EditorBrowsableState.Always)]


            public event CancelEventHandler HelpButtonClicked;
            [Browsable(false)]
            [EditorBrowsable(EditorBrowsableState.Never)]


            public event CancelEventHandler Closing;
            [Browsable(false)]
            [EditorBrowsable(EditorBrowsableState.Never)]


            public event EventHandler Closed;


            public event EventHandler ResizeBegin;


            public event DpiChangedEventHandler DpiChanged;


            public event EventHandler Shown;

            public event EventHandler Activated;


            public event InputLanguageChangingEventHandler InputLanguageChanging;


            public event InputLanguageChangedEventHandler InputLanguageChanged;
            [Browsable(false)]
            public event EventHandler MenuStart;

            public event EventHandler RightToLeftLayoutChanged;
            [Browsable(false)]
            public event EventHandler MenuComplete;

            [EditorBrowsable(EditorBrowsableState.Never)]

            public static SizeF GetAutoScaleSize(Font font) { return new SizeF(); }
            public void Activate() { }
            public void AddOwnedForm(Form ownedForm) { }
            public void Close() { }
            public void LayoutMdi(MdiLayout value) { }
            public void RemoveOwnedForm(Form ownedForm) { }
            public void SetDesktopBounds(int x, int y, int width, int height) { }
            public void SetDesktopLocation(int x, int y) { }
            public void Show(IWin32Window owner) { }
            public DialogResult ShowDialog(IWin32Window owner) { return new DialogResult(); }
            public DialogResult ShowDialog() { return new DialogResult(); }

            [Browsable(true)]
            [EditorBrowsable(EditorBrowsableState.Always)]
            public bool ValidateChildren() { return true; }
            [Browsable(true)]
            [EditorBrowsable(EditorBrowsableState.Always)]
            public bool ValidateChildren(ValidationConstraints validationConstraints) { return true; }


            [Browsable(false)]
            [EditorBrowsable(EditorBrowsableState.Advanced)]
            public SizeF CurrentAutoScaleDimensions { get; }

            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public Control ActiveControl { get; set; }

            [Browsable(false)]
            public BindingContext BindingContext { get; set; }


            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            [EditorBrowsable(EditorBrowsableState.Advanced)]
            public AutoScaleMode AutoScaleMode { get; set; }

            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            [EditorBrowsable(EditorBrowsableState.Advanced)]
            [Localizable(true)]
            public SizeF AutoScaleDimensions { get; set; }

            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public Form ParentForm { get; }

            public void PerformAutoScale() { }

            public bool Validate() { return true; }

            public bool Validate(bool checkAutoValidate) { return true; }

        }

        public class MdiLayout
        {
        }


    }

    public class BindingContext : ContextBoundObject
    {
    }
}

