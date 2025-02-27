/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 */

using Gtk;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using Container = Gtk.Container;
using Icon = System.Drawing.Icon;

namespace System.Windows.Forms;

[DesignerCategory("Form")]
[DefaultEvent(nameof(Load)),
 InitializationEvent(nameof(Load))]
public class Form : ContainerControl, IWin32Window
{
    public new Gtk.Application Application { get; } = Forms.Application.Init();
    public FormBase self = new();
    public override object GtkControl => self;
    private readonly Overlay contanter = new();
    private ObjectCollection? objectCollection;
    public override event EventHandler? SizeChanged;

    public Form()
    {
        Init();
    }
    public Form(string title) : this()
    {
        self.Title = title;
    }
    internal void Init()
    {
        SetScrolledWindow(self);
        contanter.Valign = Align.Fill;
        contanter.Halign = Align.Fill;
        contanter.Hexpand = true;
        contanter.Vexpand = true;
        contanter.MarginBottom = 0;
        contanter.MarginEnd = 0;
        contanter.Add(new Fixed { Halign = Align.Fill, Valign = Align.Fill });
        self.scrollView.Child = contanter;
        objectCollection = new ObjectCollection(this, contanter);

        self.ResizeChecked += Self_ResizeChecked;
        self.Shown += Control_Shown;
        self.CloseWindowEvent += Self_CloseWindowEvent;
    }

    private void Self_ResizeChecked(object? sender, EventArgs e)
    {
        SizeChanged?.Invoke(this, EventArgs.Empty);
    }

    private bool Self_CloseWindowEvent(object? sender, EventArgs e)
    {
        var closing = new FormClosingEventArgs(CloseReason.UserClosing, false);
        FormClosing?.Invoke(this, closing);

        if (closing.Cancel == false)
        {
            FormClosed?.Invoke(this, new FormClosedEventArgs(CloseReason.UserClosing));
        }
        return closing.Cancel == false;
    }
    private bool isControlShown;
    private void Control_Shown(object? sender, EventArgs e)
    {
        if (isControlShown == false)
        {
            isControlShown = true;
            if (self.Titlebar is HeaderBar titlebar)
            {
                titlebar.DecorationLayout = "menu:close";
                if (formBorderStyle == FormBorderStyle.FixedToolWindow || formBorderStyle == FormBorderStyle.SizableToolWindow)
                {
                }
                else
                {
                    if (MaximizeBox)
                    {
                        var maximize = new Gtk.Button("window-maximize-symbolic", IconSize.SmallToolbar) { Name = "maximize", Visible = true, Relief = ReliefStyle.None, Valign = Align.Center, Halign = Align.Center };
                        maximize.StyleContext.AddClass("maximize");
                        maximize.StyleContext.AddClass("titlebutton");
                        maximize.Clicked += Maximize_Clicked;
                        titlebar.PackEnd(maximize);
                    }
                    if (MinimizeBox)
                    {
                        var minimize = new Gtk.Button("window-minimize-symbolic", IconSize.SmallToolbar) { Name = "minimize", Visible = true, Relief = ReliefStyle.None, Valign = Align.Center, Halign = Align.Center };
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

    private void Close_Clicked(object? sender, EventArgs e)
    {
        self.CloseWindow();
    }

    private void Maximize_Clicked(object? sender, EventArgs e)
    {
        var maximize = sender as Gtk.Button;
        if (maximize?.Name == "restore")
        {
            self.Unmaximize();
            maximize.Image = Gtk.Image.NewFromIconName("window-maximize-symbolic", IconSize.SmallToolbar);
            maximize.Name = "maximize";
        }
        else
        {
            self.Maximize();
            if (maximize != null)
            {
                maximize.Image = Gtk.Image.NewFromIconName("window-restore-symbolic", IconSize.SmallToolbar);
                maximize.Name = "restore";
            }
        }
    }

    private void Minimize_Clicked(object? sender, EventArgs e)
    {
        self.Iconify();
    }

    public override event ScrollEventHandler? Scroll
    {
        add => self.Scroll += value;
        remove => self.Scroll += value;
    }

    private void OnLoadHandler()
    {
        var e = EventArgs.Empty;
        OnLoad(e);
    }

    private void OnShownHandler()
    {
        var e = EventArgs.Empty;
        OnShown(e);
    }

    protected internal virtual void OnShown(EventArgs e)
    {
        Shown?.Invoke(this, e);
        if (!bindingContextSet)
        {
            OnBindingContextChanged(e);
        }
        foreach (Control control in Controls)
        {
            control.OnLoad(e);
        }
    }

    public override void Show()
    {
        Show(null);
    }
    public void Show(IWin32Window? owner)
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

        if (owner is Form parent)
        {
            Parent = parent;
            self.SetPosition(WindowPosition.CenterOnParent);
            self.Activate();
        }

        if (self.IsVisible == false)
        {
            if (AutoScroll)
            {
                self.scrollView.HscrollbarPolicy = PolicyType.Automatic;
                self.scrollView.VscrollbarPolicy = PolicyType.Automatic;
            }
            else
            {
                self.scrollView.HscrollbarPolicy = PolicyType.Never;
                self.scrollView.VscrollbarPolicy = PolicyType.Never;
            }

            FormBorderStyle = FormBorderStyle;
            if (MaximizeBox == false && MinimizeBox == false)
            {
                self.TypeHint = Gdk.WindowTypeHint.Dialog;
            }
            else if (MaximizeBox == false && MinimizeBox)
            {
                self.Resizable = false;
            }
            self.Resize(self.DefaultWidth, self.DefaultHeight);

            if (WindowState == FormWindowState.Maximized)
            {
                self.Maximize();
            }
            else if (WindowState == FormWindowState.Minimized)
            {
                self.Iconify();
            }
            if (self.IsMapped == false)
            {
                try
                {
                    if (ShowIcon)
                    {
                        if (Icon != null)
                        {
                            if (Icon.Pixbuf != null)
                                self.Icon = Icon.Pixbuf;
                            else if (Icon.PixbufData != null)
                                self.Icon = new Gdk.Pixbuf(Icon.PixbufData);
                            else if (Icon.FileName != null && IO.File.Exists(Icon.FileName))
                                self.SetIconFromFile(Icon.FileName);
                            else if (Icon.FileName != null && IO.File.Exists("Resources\\" + Icon.FileName))
                                self.SetIconFromFile("Resources\\" + Icon.FileName);
                        }
                        var titlebar = (HeaderBar)self.Titlebar;
                        var flag = new Gtk.Image(self.Icon);
                        flag.Visible = true;
                        titlebar.PackStart(flag);
                    }
                    else
                    {
                        self.Icon = new Gdk.Pixbuf(GetType().Assembly, "System.Windows.Forms.Resources.System.view-more.png");
                    }

                }
                catch (Exception ex) 
                {
                    Trace.Write(ex);
                }
            }
        }
        self.ShowAll();
    }

    public DialogResult ShowDialog()
    {
        return ShowDialog(null);
    }
    public DialogResult ShowDialog(IWin32Window? owner)
    {
        if (owner == this)
        {
            throw new ArgumentException(@"OwnsSelfOrOwner", nameof(ShowDialog));
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
        self.Run();

        return DialogResult;
    }

    public event EventHandler? Shown;
    public event FormClosingEventHandler? FormClosing;
    public event FormClosedEventHandler? FormClosed;
    public override event EventHandler? Load;
    public override string? Text
    {
        get => self.Title;
        set => self.Title = value;
    }
    public override Size ClientSize
    {
        get => new(self.AllocatedWidth, self.AllocatedHeight);
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
        get => formBorderStyle;
        set
        {
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
    private FormWindowState windowState = FormWindowState.Normal;
    public FormWindowState WindowState
    {
        get => windowState;
        set
        {
            windowState = value;
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
    public void Close()
    {
        self?.CloseWindow();
    }
    public override void Hide()
    {
        self?.Hide();
    }

    public new ObjectCollection Controls => objectCollection!;

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
    public double Opacity
    {
        get => self.Opacity;
        set => self.Opacity = value;
    }
    public bool ShowIcon { get; set; } = true;
    public bool ShowInTaskbar
    {
        get => self.SkipTaskbarHint == false;
        set => self.SkipTaskbarHint = value == false;
    }
    public Icon? Icon { get; set; }
    public override void SuspendLayout()
    {
        created = false;
    }
    public override void ResumeLayout(bool resume)
    {
        created = resume == false;
    }

    public override void PerformLayout()
    {
        created = true;
    }
    public bool Activate()
    {
        return self.Activate();
    }
    public MenuStrip? MainMenuStrip { get; set; }

    public override IntPtr Handle => self.Handle;

    public class ObjectCollection : ControlCollection
    {
        Container? owner;
        public ObjectCollection(Control? control, Container? owner) : base(control, owner)
        {
            this.owner = owner;
        }

    }

    public class MdiLayout
    {
    }
}

public class BindingContext : ContextBoundObject
{
    internal class HashKey
    {
        private readonly WeakReference wRef;

        private readonly int dataSourceHashCode;

        private readonly string dataMember;

        internal HashKey(object? dataSource, string? dataMember)
        {
            if (dataSource == null)
            {
                throw new ArgumentNullException("dataSource");
            }
            if (dataMember == null)
            {
                dataMember = "";
            }
            wRef = new WeakReference(dataSource, false);
            dataSourceHashCode = dataSource.GetHashCode();
            this.dataMember = dataMember.ToLower(CultureInfo.InvariantCulture);
        }

        public override bool Equals(object? target)
        {
            if (!(target is HashKey))
            {
                return false;
            }
            var hashKey = (HashKey)target;
            if (wRef.Target != hashKey.wRef.Target)
            {
                return false;
            }
            return dataMember == hashKey.dataMember;
        }

        public override int GetHashCode()
        {
            return dataSourceHashCode * dataMember.GetHashCode();
        }
    }


    public static void UpdateBinding(BindingContext? newBindingContext, Binding binding)
    {
        var bindingManagerBase = binding.BindingManagerBase;
        bindingManagerBase?.Bindings.Remove(binding);
        if (newBindingContext != null)
        {
            if (binding.BindToObject?.BindingManagerBase is PropertyManager)
            {
                CheckPropertyBindingCycles(newBindingContext, binding);
            }
            var bindToObject = binding.BindToObject;
            if (bindToObject != null)
            {
                var bindingManagerBase1 = newBindingContext.EnsureListManager(bindToObject.DataSource, bindToObject.BindingMemberInfo.BindingPath);
                if (bindingManagerBase1 != null)
                {
                    bindingManagerBase1.Bindings.Add(binding);
                }
            }
        }
    }

    internal BindingManagerBase EnsureListManager(object? dataSource, string? dataMember)
    {
        BindingManagerBase? relatedCurrencyManager = null;
        if (dataMember == null)
        {
            dataMember = "";
        }
        if (dataSource is ICurrencyManagerProvider)
        {
            relatedCurrencyManager = (dataSource as ICurrencyManagerProvider)?.GetRelatedCurrencyManager(dataMember);
            if (relatedCurrencyManager != null)
            {
                return relatedCurrencyManager;
            }
        }
        var key = GetKey(dataSource, dataMember);
        var item = listManagers[key] as WeakReference;
        if (item != null)
        {
            relatedCurrencyManager = (BindingManagerBase)item.Target;
        }
        if (relatedCurrencyManager != null)
        {
            return relatedCurrencyManager;
        }
        if (dataMember.Length != 0)
        {
            var num = dataMember.LastIndexOf(".", StringComparison.Ordinal);
            var str = num == -1 ? "" : dataMember.Substring(0, num);
            var str1 = dataMember.Substring(num + 1);
            var bindingManagerBase = EnsureListManager(dataSource, str);
            var propertyDescriptor = bindingManagerBase.GetItemProperties()?.Find(str1, true);
            if (propertyDescriptor == null)
            {
                throw new ArgumentException("RelatedListManagerChild");
            }
            if (!typeof(IList).IsAssignableFrom(propertyDescriptor.PropertyType))
            {
                relatedCurrencyManager = new RelatedPropertyManager(bindingManagerBase, str1);
            }
            else
            {
                relatedCurrencyManager = new RelatedCurrencyManager(bindingManagerBase, str1);
            }
        }
        else if (dataSource is IList || dataSource is IListSource)
        {
            relatedCurrencyManager = new CurrencyManager(dataSource);
        }
        else
        {
            relatedCurrencyManager = new PropertyManager(dataSource);
        }
        if (item != null)
        {
            item.Target = relatedCurrencyManager;
        }
        else
        {
            listManagers.Add(key, new WeakReference(relatedCurrencyManager, false));
        }
        ScrubWeakRefs();
        return relatedCurrencyManager;
    }

    private void ScrubWeakRefs()
    {
        ArrayList? arrayLists = null;
        foreach (DictionaryEntry listManager in listManagers)
        {
            if (((WeakReference)listManager.Value).Target != null)
            {
                continue;
            }
            if (arrayLists == null)
            {
                arrayLists = new ArrayList();
            }
            arrayLists.Add(listManager.Key);
        }
        if (arrayLists != null)
        {
            foreach (var arrayList in arrayLists)
            {
                listManagers.Remove(arrayList);
            }
        }
    }

    private readonly Hashtable listManagers = new();

    public bool Contains(object? dataSource, string? dataMember)
    {
        return listManagers.ContainsKey(GetKey(dataSource, dataMember));
    }

    internal HashKey GetKey(object? dataSource, string? dataMember)
    {
        return new HashKey(dataSource, dataMember);
    }


    private static void CheckPropertyBindingCycles(BindingContext? newBindingContext, Binding propBinding)
    {
        if (newBindingContext == null || propBinding == null)
        {
            return;
        }
        if (newBindingContext.Contains(propBinding.BindableComponent, ""))
        {
            var bindingManagerBase = newBindingContext.EnsureListManager(propBinding.BindableComponent, "");
            for (var i = 0; i < bindingManagerBase.Bindings.Count; i++)
            {
                var item = bindingManagerBase.Bindings[i];
                if (item.DataSource == propBinding.BindableComponent)
                {
                    if (propBinding.BindToObject?.BindingMemberInfo.BindingMember.Equals(item.PropertyName)??false)
                    {
                        throw new ArgumentException(@"DataBindingCycle", "propBinding");
                    }
                }
                else if (propBinding.BindToObject?.BindingManagerBase is PropertyManager)
                {
                    CheckPropertyBindingCycles(newBindingContext, item);
                }
            }
        }
    }

}