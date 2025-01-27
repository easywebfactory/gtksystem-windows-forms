/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms.ComponentModel.Com2Interop;
using System.Windows.Forms.Design;
namespace System.Windows.Forms
{
    /// <summary>
    /// Note: This control cannot be used and is under development...
    /// </summary>
    [Designer($"System.Windows.Forms.Design.PropertyGridDesigner, {AssemblyRef.SystemDesign}")]
    public partial class PropertyGrid : ContainerControl, IComPropertyBrowser//, IPropertyNotifySink.Interface
    {
        private static readonly object s_propertyValueChangedEvent = new();
        private static readonly object s_comComponentNameChangedEvent = new();
        private static readonly object s_propertyTabChangedEvent = new();
        private static readonly object s_selectedGridItemChangedEvent = new();
        private static readonly object s_propertySortChangedEvent = new();
        private static readonly object s_selectedObjectsChangedEvent = new();
        private object[]? _selectedObjects;
        private readonly List<TabInfo> _tabs = new List<TabInfo>();
        public PropertyGrid()
        {
        }
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
        public virtual bool CommandsVisible { get; }
        [DefaultValue(true)]
        public virtual bool CommandsVisibleIfAvailable { get; set; }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual bool CanShowCommands { get; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Point ContextMenuDefaultLocation { get; }
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
            get => _selectedObjects is null ? Array.Empty<object>() : (object[])_selectedObjects.Clone();
            set { } 
        }
        [DefaultValue(null)]
        //[TypeConverter(typeof(SelectedObjectConverter))]
        public object SelectedObject
        {
            get => _selectedObjects is null || _selectedObjects.Length == 0 ? null : _selectedObjects[0];
            set => SelectedObjects = value is null ? Array.Empty<object>() : (new object[] { value });
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public PropertyTabCollection PropertyTabs { get; }
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
        public Padding Padding { get; set; }
        [DefaultValue(typeof(Color), "InactiveBorder")]
        public Color LineColor { get; set; }
        [DefaultValue(true)]
        public bool CanShowVisualStyleGlyphs { get; set; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public PropertyTab SelectedTab { get; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ControlCollection Controls { get; }
        public bool InPropertySet => throw new NotImplementedException();
        protected bool DrawFlatToolbar { get; set; }
        //protected ToolStripRenderer ToolStripRenderer { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual Bitmap SortByPropertyImage { get; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual Bitmap SortByCategoryImage { get; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual Bitmap ShowPropertyPageImage { get; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual Type DefaultTabType { get; }
        protected override Size DefaultSize { get; }
        protected internal override bool ShowFocusCues { get; }
        public event ComponentRenameEventHandler ComComponentNameChanged;
        public event EventHandler SelectedObjectsChanged
        {
            add => Events.AddHandler(s_selectedObjectsChangedEvent, value);
            remove => Events.RemoveHandler(s_selectedObjectsChangedEvent, value);
        }
        //[Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public event EventHandler BackgroundImageChanged
        //{ 
        //}
        //[Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public event EventHandler BackgroundImageLayoutChanged
        //{ 
        //}
        //[Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public event EventHandler ForeColorChanged
        //{ 
        //}
        // [Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public event EventHandler PaddingChanged
        //{ 
        //}
        // [Browsable(false)]
        //public event EventHandler TextChanged
        //{ 
        //}
        // [Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        //public event KeyPressEventHandler KeyPress
        //{ 
        //}
        // [Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        //public event KeyEventHandler KeyUp
        //{ 
        //}
        // [Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        //public new event MouseEventHandler MouseDown
        //{
        //    add => base.MouseDown += value;
        //    remove => base.MouseDown -= value;
        //}
        //[Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        //public new event KeyEventHandler KeyDown
        //{
        //    add => base.KeyDown += value;
        //    remove => base.KeyDown -= value;
        //}
        //[Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        //public new event MouseEventHandler MouseMove
        //{
        //    add => base.MouseMove += value;
        //    remove => base.MouseMove -= value;
        //}
        public event SelectedGridItemChangedEventHandler SelectedGridItemChanged
        {
            add => Events.AddHandler(s_selectedGridItemChangedEvent, value);
            remove => Events.RemoveHandler(s_selectedGridItemChangedEvent, value);
        }
        public event EventHandler PropertySortChanged
        {
            add => Events.AddHandler(s_propertySortChangedEvent, value);
            remove => Events.RemoveHandler(s_propertySortChangedEvent, value);
        }
        // [Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        //public new event MouseEventHandler MouseUp
        //{
        //    add => base.MouseUp += value;
        //    remove => base.MouseUp -= value;
        //}
        public event PropertyValueChangedEventHandler PropertyValueChanged
        {
            add => Events.AddHandler(s_propertyValueChangedEvent, value);
            remove => Events.RemoveHandler(s_propertyValueChangedEvent, value);
        }
        public event PropertyTabChangedEventHandler PropertyTabChanged
        {
            add => Events.AddHandler(s_propertyTabChangedEvent, value);
            remove => Events.RemoveHandler(s_propertyTabChangedEvent, value);
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public new event EventHandler MouseLeave
        {
            add => base.MouseLeave += value;
            remove => base.MouseLeave -= value;
        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public new event EventHandler MouseEnter
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
        public void SaveState(RegistryKey key)
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
        // protected void OnNotifyPropertyValueUIItemsChanged(object sender, EventArgs e)
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
}