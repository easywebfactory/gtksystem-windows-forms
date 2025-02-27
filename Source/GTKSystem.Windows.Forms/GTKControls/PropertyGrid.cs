﻿/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Microsoft.Win32;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms.ComponentModel;
using System.Windows.Forms.Design;
namespace System.Windows.Forms;

/// <summary>
/// 注：此控件无法使用，正在开发中...
/// </summary>
[Designer($"System.Windows.Forms.Design.PropertyGridDesigner, {AssemblyRef.systemDesign}")]
public partial class PropertyGrid : ContainerControl, IComPropertyBrowser//, IPropertyNotifySink.Interface
{
    private static readonly object propertyValueChangedEvent = new();
    private static readonly object comComponentNameChangedEvent = new();
    private static readonly object propertyTabChangedEvent = new();
    private static readonly object selectedGridItemChangedEvent = new();
    private static readonly object propertySortChangedEvent = new();
    private static readonly object selectedObjectsChangedEvent = new();
    private object[]? _selectedObjects;
    private readonly List<TabInfo> _tabs = [];

    [DefaultValue(true)]
    [Localizable(true)]
    public virtual bool HelpVisible { get; set; }
    [DefaultValue(typeof(Color), "ControlDark")]
    public Color HelpBorderColor { get; set; }
    [DefaultValue(typeof(Color), "ControlText")]
    public Color HelpForeColor { get; set; }
    [DefaultValue(typeof(Color), "Control")]
    public Color HelpBackColor { get; set; }
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override Color ForeColor { get; set; }
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool AutoScroll { get; set; }
    public override Color BackColor { get; set; }
#nullable disable
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override Image BackgroundImage { get; set; }
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override ImageLayout BackgroundImageLayout { get; set; }
    [DefaultValue(typeof(Color), "Highlight")]
    public Color SelectedItemWithFocusBackColor { get; set; }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public AttributeCollection BrowsableAttributes { get; set; }
    [DefaultValue(typeof(Color), "ControlText")]
    public Color CategoryForeColor { get; set; }
    public Color CommandsBackColor { get; set; }
    public Color CommandsForeColor { get; set; }
    public Color CommandsLinkColor { get; set; }
    public Color CommandsActiveLinkColor { get; set; }
    public Color CommandsDisabledLinkColor { get; set; }
    [DefaultValue(typeof(Color), "ControlDark")]
    public Color CommandsBorderColor { get; set; }
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public virtual bool CommandsVisible { get; set; }
    [DefaultValue(true)]
    public virtual bool CommandsVisibleIfAvailable { get; set; }
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public virtual bool CanShowCommands { get; set; }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public Point ContextMenuDefaultLocation { get; set; }
    [DefaultValue(typeof(Color), "HighlightText")]
    public Color SelectedItemWithFocusForeColor { get; set; }
    [DefaultValue(typeof(Color), "Control")]
    public Color CategorySplitterColor { get; set; }
    [DefaultValue(false)]
    public bool UseCompatibleTextRendering { get; set; }
    [DefaultValue(typeof(Color), "ControlDark")]
    public Color ViewBorderColor { get; set; }
    [DefaultValue(typeof(Color), "WindowText")]
    public Color ViewForeColor { get; set; }
    [DefaultValue(typeof(Color), "Window")]
    public Color ViewBackColor { get; set; }
    [DefaultValue(true)]
    public virtual bool ToolbarVisible { get; set; }
    [DefaultValue(false)]
    public bool LargeButtons { get; set; }
    [DefaultValue(typeof(Color), "GrayText")]
    public Color DisabledItemForeColor { get; set; }
    public override ISite Site { get; set; }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public GridItem SelectedGridItem { get; set; }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override string Text { get; set; }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object[] SelectedObjects { 
        get => _selectedObjects is null ? _selectedObjects=[] : (object[])_selectedObjects.Clone();
        set { _selectedObjects = value; }
    }
    [DefaultValue(null)]
    //[TypeConverter(typeof(SelectedObjectConverter))]
    public object SelectedObject
    {
        get => _selectedObjects is null || _selectedObjects.Length == 0 ? null : _selectedObjects[0];
        set => SelectedObjects = value is null ? [] : [value];
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public PropertyTabCollection PropertyTabs { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    internal void AddTab(Type tabType, PropertyTabScope scope, object? @object = null, bool setupToolbar = true)
    {
    }
    internal void ClearTabs(PropertyTabScope tabScope)
    {
        //if (tabScope < PropertyTabScope.Document)
        //{
        //    throw new ArgumentException(SR.PropertyGridTabScope, nameof(tabScope));
        //}

        //RemoveTabs(tabScope, true);
    }
    internal void RemoveTabs(PropertyTabScope classification, bool setupToolbar)
    {

    }
    internal void RemoveTab(int tabIndex, bool setupToolbar)
    {
    }
    internal void RemoveTab(Type tabType)
    {
    }
    [DefaultValue(PropertySort.CategorizedAlphabetical)]
    public PropertySort PropertySort { get; set; }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Padding Padding { get; set; }
    [DefaultValue(typeof(Color), "InactiveBorder")]
    public Color LineColor { get; set; }
    [DefaultValue(true)]
    public bool CanShowVisualStyleGlyphs { get; set; }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public PropertyTab SelectedTab { get; set; }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new ControlCollection Controls { get; set; }
    public bool InPropertySet => throw new NotImplementedException();
    protected bool DrawFlatToolbar { get; set; }
    //protected ToolStripRenderer ToolStripRenderer { get; set; }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected virtual Bitmap SortByPropertyImage { get; set; }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected virtual Bitmap SortByCategoryImage { get; set; }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected virtual Bitmap ShowPropertyPageImage { get; set; }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected virtual Type DefaultTabType { get; set; }
    protected new virtual Size DefaultSize { get; set; }
    protected internal new virtual bool ShowFocusCues { get; set; }
    public event ComponentRenameEventHandler? ComComponentNameChanged;
    public event EventHandler? SelectedObjectsChanged
    {
        add => Events.AddHandler(selectedObjectsChangedEvent, value);
        remove => Events.RemoveHandler(selectedObjectsChangedEvent, value);
    }
    //[Browsable(false)]
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public event EventHandler? BackgroundImageChanged
    //{ 
    //}
    //[Browsable(false)]
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public event EventHandler? BackgroundImageLayoutChanged
    //{ 
    //}
    //[Browsable(false)]
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public event EventHandler? ForeColorChanged
    //{ 
    //}
    // [Browsable(false)]
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public event EventHandler? PaddingChanged
    //{ 
    //}
    // [Browsable(false)]
    //public event EventHandler? TextChanged
    //{ 
    //}
    // [Browsable(false)]
    //[EditorBrowsable(EditorBrowsableState.Advanced)]
    //public event KeyPressEventHandler? KeyPress
    //{ 
    //}
    // [Browsable(false)]
    //[EditorBrowsable(EditorBrowsableState.Advanced)]
    //public event KeyEventHandler? KeyUp
    //{ 
    //}
    // [Browsable(false)]
    //[EditorBrowsable(EditorBrowsableState.Advanced)]
    //public new event MouseEventHandler? MouseDown
    //{
    //    add => base.MouseDown += value;
    //    remove => base.MouseDown -= value;
    //}
    //[Browsable(false)]
    //[EditorBrowsable(EditorBrowsableState.Advanced)]
    //public new event KeyEventHandler? KeyDown
    //{
    //    add => base.KeyDown += value;
    //    remove => base.KeyDown -= value;
    //}
    //[Browsable(false)]
    //[EditorBrowsable(EditorBrowsableState.Advanced)]
    //public new event MouseEventHandler? MouseMove
    //{
    //    add => base.MouseMove += value;
    //    remove => base.MouseMove -= value;
    //}
    public event SelectedGridItemChangedEventHandler? SelectedGridItemChanged
    {
        add => Events.AddHandler(selectedGridItemChangedEvent, value);
        remove => Events.RemoveHandler(selectedGridItemChangedEvent, value);
    }
    public event EventHandler? PropertySortChanged
    {
        add => Events.AddHandler(propertySortChangedEvent, value);
        remove => Events.RemoveHandler(propertySortChangedEvent, value);
    }
    // [Browsable(false)]
    //[EditorBrowsable(EditorBrowsableState.Advanced)]
    //public new event MouseEventHandler? MouseUp
    //{
    //    add => base.MouseUp += value;
    //    remove => base.MouseUp -= value;
    //}
    public event PropertyValueChangedEventHandler? PropertyValueChanged
    {
        add => Events.AddHandler(propertyValueChangedEvent, value);
        remove => Events.RemoveHandler(propertyValueChangedEvent, value);
    }
    public event PropertyTabChangedEventHandler? PropertyTabChanged
    {
        add => Events.AddHandler(propertyTabChangedEvent, value);
        remove => Events.RemoveHandler(propertyTabChangedEvent, value);
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public new event EventHandler? MouseLeave
    {
        add => base.MouseLeave += value;
        remove => base.MouseLeave -= value;
    }
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public new event EventHandler? MouseEnter
    {
        add => base.MouseEnter += value;
        remove => base.MouseEnter -= value;
    }
    public void CollapseAllGridItems()
    {
    }

    public override void Refresh()
    {
    }
    public void RefreshTabs(PropertyTabScope tabScope)
    {
    }
    public void ResetSelectedProperty()
    {
    }
    void IComPropertyBrowser.SaveState(RegistryKey key)
    {
        throw new NotImplementedException();
    }
    // protected override AccessibleObject CreateAccessibilityInstance()
    //{ 
    //}
    // protected virtual PropertyTab CreatePropertyTab(Type tabType)
    //{ 
    //}
    // protected override void Dispose(bool disposing)
    //{ 
    //}
    // protected void OnComComponentNameChanged(ComponentRenameEventArgs e)
    //{ 
    //}
    // protected override void OnEnabledChanged(EventArgs e)
    //{ 
    //}
    // protected override void OnFontChanged(EventArgs e)
    //{ 
    //}
    // protected override void OnGotFocus(EventArgs e)
    //{ 
    //}
    // protected override void OnHandleCreated(EventArgs e)
    //{ 
    //}
    // protected override void OnHandleDestroyed(EventArgs e)
    //{ 
    //}
    // protected override void OnMouseDown(MouseEventArgs me)
    //{ 
    //}
    // protected override void OnMouseMove(MouseEventArgs me)
    //{ 
    //}
    // protected override void OnMouseUp(MouseEventArgs me)
    //{ 
    //}
    // protected void OnNotifyPropertyValueUIItemsChanged(object? sender, EventArgs e)
    //{ 
    //}
    // protected override void OnPaint(PaintEventArgs pevent)
    //{ 
    //}
    // protected virtual void OnPropertySortChanged(EventArgs e)
    //{ 
    //}
    // protected virtual void OnPropertyTabChanged(PropertyTabChangedEventArgs e)
    //{ 
    //}
    // protected virtual void OnPropertyValueChanged(PropertyValueChangedEventArgs e)
    //{ 
    //}
    // protected override void OnResize(EventArgs e)
    //{ 
    //}
    // protected virtual void OnSelectedGridItemChanged(SelectedGridItemChangedEventArgs e)
    //{ 
    //}
    // protected virtual void OnSelectedObjectsChanged(EventArgs e)
    //{ 
    //}
    // protected override void OnSystemColorsChanged(EventArgs e)
    //{ 
    //}
    // protected override void OnVisibleChanged(EventArgs e)
    //{ 
    //}
    // protected override bool ProcessDialogKey(Keys keyData)
    //{ 
    //    return false;
    //}
    // [EditorBrowsable(EditorBrowsableState.Never)]
    //protected override void ScaleCore(float dx, float dy)
    //{ 
    //}
    // protected void ShowEventsButton(bool value)
    //{ 
    //}
    // protected override void WndProc(ref Message m)
    //{ 
    //}

    void IComPropertyBrowser.DropDownDone()
    {
        throw new NotImplementedException();
    }

    bool IComPropertyBrowser.EnsurePendingChangesCommitted()
    {
        throw new NotImplementedException();
    }

    void IComPropertyBrowser.HandleF4()
    {
        throw new NotImplementedException();
    }

    void IComPropertyBrowser.LoadState(RegistryKey key)
    {
        throw new NotImplementedException();
    }
}