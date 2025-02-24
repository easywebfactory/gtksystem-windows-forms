/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using Microsoft.Win32;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms.ComponentModel.Com2Interop;
using System.Windows.Forms.Design;
using System.Windows.Forms.PropertyGridInternal;
namespace System.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [Designer($"System.Windows.Forms.Design.PropertyGridDesigner, {AssemblyRef.SystemDesign}")]
    public partial class PropertyGrid : ContainerControl, IComPropertyBrowser
    {
        public readonly PropertyGridBase self = new PropertyGridBase();
        public override object GtkControl => self;
        private static readonly object s_propertyValueChangedEvent = new();
        private static readonly object s_comComponentNameChangedEvent = new();
        private static readonly object s_propertyTabChangedEvent = new();
        private static readonly object s_selectedGridItemChangedEvent = new();
        private static readonly object s_propertySortChangedEvent = new();
        private static readonly object s_selectedObjectsChangedEvent = new();
        private object[] _selectedObjects;
        private readonly List<TabInfo> _tabs = new List<TabInfo>();

        private PropertyGridView _propertyView;
        public PropertyGrid()
        {
            _propertyView = new PropertyGridView(this);
            self.child1.Add(_propertyView.tree);
            _propertyView.PropertyValueChanged += Self_PropertyValueChanged;
            _propertyView.SelectedGridItemChanged += Self_SelectedGridItemChanged;
        }

        private void Self_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            Events[s_selectedGridItemChangedEvent]?.DynamicInvoke(this, e);
        }

        private void Self_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Events[s_propertyValueChangedEvent]?.DynamicInvoke(this, e);
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object[] SelectedObjects
        {
            get => _selectedObjects is null ? Array.Empty<object>() : (object[])_selectedObjects.Clone();
            set
            {
                _selectedObjects = value is null ? Array.Empty<object>() : (object[])value.Clone();
                //只支持一个对象
                _propertyView.LoadPropertyInfo(_selectedObjects[0]);
            }
        }
        [DefaultValue(null)]
        public object SelectedObject
        {
            get => _selectedObjects is null || _selectedObjects.Length == 0 ? null : _selectedObjects[0];
            set => SelectedObjects = value is null ? Array.Empty<object>() : (new object[] { value });
        }

        [Browsable(true)]
        [Description("控件的大小（以像素为单位）。")]
        public override Size Size { get => base.Size; set { base.Size = value; self.Position = value.Height - 60; } }

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
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public PropertyTabCollection PropertyTabs { get; }
     
        [DefaultValue(PropertySort.CategorizedAlphabetical)]
        public PropertySort PropertySort { get; set; }
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public Padding Padding { get; set; }
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
        public override ControlCollection Controls { get; }
        public bool InPropertySet => true;
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
        void SaveState(RegistryKey key)
        {
            throw new NotImplementedException();
        }
        void IComPropertyBrowser.SaveState(RegistryKey key)
        {
            throw new NotImplementedException();
        }

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