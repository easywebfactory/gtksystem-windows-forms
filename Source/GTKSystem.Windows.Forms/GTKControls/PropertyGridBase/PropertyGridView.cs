﻿/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Cairo;
using Gdk;
using GLib;
using Gtk;
using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Forms.PropertyGridInternal;

internal sealed partial class PropertyGridView 
{
    public Gtk.TreeView tree = new();
    public TreeStore store = new(typeof(GridEntry));
    public PropertyGrid OwnerGrid { get; private set; }
    private object? _selectedObject;
    public PropertyGridView(PropertyGrid propertyGrid)
    {
        OwnerGrid = propertyGrid;
        tree.ShowExpanders = false;
        tree.EnableTreeLines = false;
        tree.EnableGridLines = TreeViewGridLines.Horizontal;
        var column1 = new TreeViewColumn();
        column1.FixedWidth = 30;
        column1.Resizable = false;
        column1.Clickable = false;
        column1.SortIndicator = false;
        column1.Reorderable = false;
        var button1 = column1.Button as Gtk.Button;
        if (button1.Child is Box box1)
        {
            var img = new Image(GetType().Assembly, "GTKSystem.Windows.Forms.Resources.System.PBCategory.ico");
            img.Visible = true;
            box1.PackStart(img, false, true, 1);
            box1.ReorderChild(img, 0);
        }

        var expander = new CellRendererExpander(this);
        expander.CellBackgroundRgba = new RGBA() { Alpha = 0.6, Red = 0.9, Green = 0.9, Blue = 0.9 };
        column1.PackStart(expander, false);
        column1.AddAttribute(expander, "icon", 0);

        var column2 = new TreeViewColumn();
        column2.MinWidth = 20;
        column2.FixedWidth = 150;
        column2.Resizable = true;
        column2.Clickable = false;
        column2.Title = " ";
        column2.SortIndicator = true;
        column2.SortOrder = SortType.Ascending;
        column2.Reorderable = false;
        column2.Button.Name = "key";
        var button2 = column2.Button as Gtk.Button;
        if (button2.Child is Box box)
        {
            var img = new Image(GetType().Assembly, "GTKSystem.Windows.Forms.Resources.System.PBAlpha.ico");
            img.Visible = true;
            box.PackStart(img, false, true, 1);
            box.ReorderChild(img, 0);
            box.SetChildPacking(box.Children[2], false, true, 0, PackType.End);
        }
        CellRendererText names = new CellRendererProperty(this);
        names.Editable = false;
        column2.SortColumnId = 0;
        column2.PackStart(names, false);
        column2.AddAttribute(names, "otext", 0);

        var column3 = new TreeViewColumn();
        column3.MinWidth = 10;
        //column3.FixedWidth = 50;
        column3.Resizable = false;
        column3.Clickable = false;
        column3.Title = " ";
        column3.SortIndicator = false;
        column3.Reorderable = false;
        column3.Button.Name = "value";
        CellRendererText values = new CellRendererProperty(this);
        values.Editable = true;
        column3.PackStart(values, false);
        column3.AddAttribute(values, "ovalue", 0);

        tree.AppendColumn(column1);
        tree.AppendColumn(column2);
        tree.AppendColumn(column3);
        tree.ExpanderColumn = column2;

        
        tree.LevelIndentation = 15;
        tree.Selection.Mode = Gtk.SelectionMode.Single;
        tree.ActivateOnSingleClick = true;
        tree.RowActivated += Tree_RowActivated;
        tree.Selection.Changed += Selection_Changed;

        store.SetSortFunc(0, new TreeIterCompareFunc((model, iter1, iter2) =>
        {
            var val1 = model.GetValue(iter1, 0) as GridEntry;
            var val2 = model.GetValue(iter2, 0) as GridEntry;
            return val1.Value.ToString().CompareTo(val2.Value.ToString());
        }));

        tree.Model = store;
    }
    private GridEntry? oldSelectedPropertyItem;
    private void Selection_Changed(object sender, EventArgs e)
    {
        if (tree.Selection.GetSelected(out _, out var oiter))
        {
            var propertyItem = store.GetValue(oiter, 0) as GridEntry;
            if (SelectedGridItemChanged != null)
                SelectedGridItemChanged(sender, new SelectedGridItemChangedEventArgs(oldSelectedPropertyItem, propertyItem));
            oldSelectedPropertyItem = propertyItem;
        }
    }
    private void Tree_RowActivated(object o, RowActivatedArgs args)
    {
        Console.WriteLine(args.Path);
        if (store.GetIter(out var iter, args.Path))
        {
            if (store.GetValue(iter, 0) is GridEntry ov)
            {
                OwnerGrid.self.ShowDescription(ov.Label, ov.Description);
            }
        }
    }
    public void LoadPropertyInfo(object? entryobj)
    {
        _selectedObject = entryobj;
        List<GridEntry> categorys = new();
        PropertyInfo?[] propertyInfos = entryobj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var propertyInfo in propertyInfos)
        {
            var attri = propertyInfo.GetCustomAttributes(typeof(BrowsableAttribute));
            if (attri != null && attri.Count() > 0)
            {
                //BrowsableAttribute
                var attribute = (BrowsableAttribute)attri.FirstOrDefault();
                if (attribute.Browsable == false)
                {
                    continue;
                }
            }
            var category = "【未分类组】";
            var description = "";

            var attri2 = propertyInfo.GetCustomAttributes(typeof(CategoryAttribute));
            if (attri2 != null && attri2.Count() > 0)
            {
                var attribute = (CategoryAttribute)attri2.FirstOrDefault();
                category = attribute.Category;
            }
            var attri3 = propertyInfo.GetCustomAttributes(typeof(DescriptionAttribute));
            if (attri3 != null && attri3.Count() > 0)
            {
                var attribute = (DescriptionAttribute)attri3.FirstOrDefault();
                description = attribute.Description;
            }

            var propertyItem = new GridEntry(null, GridItemType.Category, 0, category, category, description);
            propertyItem.PropertyInfo = propertyInfo;
            categorys.Add(propertyItem);
        }
        var gs = categorys.GroupBy(o => o.Label).OrderBy(g => g.Key);
        if (gs.Count() == 1 && gs.First().Key == "【未分类组】")
        {
            GetPropertyInfos(null, null, entryobj);
        }
        else
        {
            foreach (var g in gs)
            {
                var value0 = new GridEntry(null, GridItemType.Category, 0, g.Key, g.Key, g.Key) { Level = 0, Editable = false };
                var category1 = store.AppendValues(value0);
                foreach (var m in g)
                {
                    GetPropertyInfo(category1, value0, m.PropertyInfo, entryobj);
                }
            }
        }
        tree.ExpandAll();
    }
    private void GetPropertyInfos(TreeIter? parent, GridEntry? parentitem, object? obj)
    {
        var type = obj.GetType();
        PropertyInfo?[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
        foreach (var property in properties)
        {
            GetPropertyInfo(parent, parentitem, property, obj);
        }
    }
    private bool GetPropertyInfo(TreeIter? parent, GridEntry? parentitem, PropertyInfo? property, object? obj)
    {
        if (property.CanRead)
        {
            var attri = property.GetCustomAttributes(typeof(BrowsableAttribute));
            if (attri != null && attri.Count() > 0)
            {
                //BrowsableAttribute
                var attribute = (BrowsableAttribute)attri.FirstOrDefault();
                if (attribute.Browsable == false)
                {
                    return false;
                }
            }
            var description = "";
            var attri3 = property.GetCustomAttributes(typeof(DescriptionAttribute));
            if (attri3 != null && attri3.Count() > 0)
            {
                var attribute = (DescriptionAttribute)attri3.FirstOrDefault();
                description = attribute.Description;
            }
            if (property.PropertyType.IsEnum)
            {
                var val = property.GetValue(obj);
                var value1 = new GridEntry(parentitem, GridItemType.Property, 1, property.Name, val, description) { Editable = property.CanWrite, ValueType = property.PropertyType };
                StoreValue(parent, value1);
            }
            else if (property.PropertyType.IsValueType)
            {
                var val = property.GetValue(obj);
                if (property.PropertyType.IsPrimitive == false)
                { //只有基元类型可以编辑
                    var value1 = new GridEntry(parentitem, GridItemType.Property, 1, property.Name, val, description) { ValueType = property.PropertyType };
                    var node1 = StoreValue(parent, value1);
                    GetPropertyInfos(node1, value1, val);
                }
                else
                {
                    var value1 = new GridEntry(parentitem, GridItemType.Property, 1, property.Name, val, description) { Editable = property.CanWrite, ValueType = property.PropertyType };
                    StoreValue(parent, value1);
                }
            }
            else if (property.PropertyType.IsArray)
            {
                var val = property.GetValue(obj);
                var value1 = new GridEntry(parentitem, GridItemType.Property, 1, property.Name, val, description) { Editable = false, ValueType = property.PropertyType };
                StoreValue(parent, value1);
            }
            return true;
        }

        return false;
    }
    private TreeIter StoreValue(TreeIter? parent, object? value)
    {
        if (parent == null)
            return store.AppendValues(value);
        return store.AppendValues(parent.Value, value);
    }
    public event PropertyValueChangedEventHandler? PropertyValueChanged;
    public event SelectedGridItemChangedEventHandler? SelectedGridItemChanged;
    public class CellRendererExpander : CellRendererToggle
    {
        readonly Pixbuf gonextpixbuf = new(typeof(CellRendererExpander).Assembly, "GTKSystem.Windows.Forms.Resources.System.go-next-symbolic.png");
        readonly Pixbuf godownixbuf = new(typeof(CellRendererExpander).Assembly, "GTKSystem.Windows.Forms.Resources.System.go-down-symbolic.png");
        public CellRendererExpander(PropertyGridView owner)
        {

        }

        [Property("icon")]
        public GridEntry Icon
        {
            set
            {

            }
        }
        protected override bool OnActivate(Event evnt, Widget widget, string path, Gdk.Rectangle background_area, Gdk.Rectangle cellArea, CellRendererState flags)
        {
            var re = base.OnActivate(evnt, widget, path, background_area, cellArea, flags);
            var tree = widget as Gtk.TreeView;
            if (IsExpanded)
                tree.CollapseRow(new TreePath(path));
            else
                tree.ExpandToPath(new TreePath(path));

            return re;
        }
        protected override void OnRender(Context cr, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cellArea, CellRendererState flags)
        {
            if (IsExpander)
            {
                cr.Save();
                if (IsExpanded)
                    Gdk.CairoHelper.SetSourcePixbuf(cr, godownixbuf, background_area.Left + 10, background_area.Top + 5);
                else
                    Gdk.CairoHelper.SetSourcePixbuf(cr, gonextpixbuf, background_area.Left + 10, background_area.Top + 5);
                cr.Paint();
                cr.Restore();
            }
        }
    }
    public class CellRendererProperty : CellRendererCombo
    {
        readonly PropertyGridView owner;
        readonly ListStore model = new(typeof(string));
        public CellRendererProperty(PropertyGridView owner)
        {
            this.owner = owner;
            TextColumn = 0;
            Height = 20;
            SetFixedSize(50, 20);
            Model = model;
            EditingStarted += CellRendererText_EditingStarted;
            Edited += CellRendererText_Edited;
        }

        private void CellRendererText_EditingStarted(object o, EditingStartedArgs args)
        {
            model.Clear();
            if (_value.ValueType != null)
            {
                if (_value.ValueType.IsEnum)
                {
                    string[] keys = Enum.GetNames(_value.ValueType);
                    foreach (var key in keys)
                    {
                        model.AppendValues(key);
                    }
                }
                else if (_value.ValueType.Equals(typeof(bool)))
                {
                    model.AppendValues("False");
                    model.AppendValues("True");
                }
            }
        }

        private void CellRendererText_Edited(object o, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            var model = ((Gtk.TreeView)owner.tree).Model;
            model.GetIter(out var iter, path);
            var cell = model.GetValue(iter, 0);
            if (cell is GridEntry val)
            {
                var oldvalue = val.Value;
                try
                {
                    if (path.Depth <= 2)
                    {
                        var obj = owner._selectedObject;
                        var info = obj.GetType().GetProperty(val.Label);
                        info.GetValue(obj);
                        if (info.CanWrite)
                        {
                            if (val.ValueType.IsEnum)
                                val.value = Enum.Parse(info.PropertyType, args.NewText);
                            else
                                val.value = Convert.ChangeType(args.NewText, info.PropertyType);
                            info.SetValue(obj, val.value);
                        }
                    }
                    else
                    {
                        if (val.ValueType.IsEnum)
                        {
                            var obj = owner._selectedObject;
                            var info = obj.GetType().GetProperty(val.Label);
                            if (info.CanWrite)
                            {
                                if (val.ValueType.IsEnum)
                                    val.value = Enum.Parse(info.PropertyType, args.NewText);
                                else
                                    val.value = Convert.ChangeType(args.NewText, info.PropertyType);
                                info.SetValue(obj, val.value);
                            }
                        }
                        else
                        {
                            if (model.IterParent(out var piter, iter))
                            {   //取父对象
                                var pcell = model.GetValue(piter, 0);
                                if (pcell is GridEntry pval)
                                {
                                    var obj = owner._selectedObject;
                                    var otype = obj.GetType();
                                    var pinfo = otype.GetProperty(pval.Label);
                                    var pvalue = pinfo.GetValue(obj);
                                    //从obj取当前对象，赋值
                                    var info = pvalue.GetType().GetProperty(val.Label);
                                    if (info.CanWrite)
                                    {
                                        if (val.ValueType.IsEnum)
                                            val.value = Enum.Parse(info.PropertyType, args.NewText);
                                        else
                                            val.value = Convert.ChangeType(args.NewText, info.PropertyType);
                                        info.SetValue(pvalue, val.value);
                                        pval.value = pvalue.ToString();
                                    }
                                    //修改父对象值
                                    if (pinfo.CanWrite)
                                    {
                                        pinfo.SetValue(obj, pvalue);
                                    }

                                }
                            }
                        }

                    }
                }
                catch
                {
                }

                if (owner.PropertyValueChanged != null)
                    owner.PropertyValueChanged(o, new PropertyValueChangedEventArgs(val, oldvalue));
            }
        }
        private GridEntry? _value;
        [Property("otext")]
        public GridEntry? OText
        {
            set
            {
                _value = value;
                if (_value != null)
                {
                    Text = value.Label;
                    Editable = false;
                    Weight = value.Level == 0 ? 900 : 200;
                }

            }
            get => _value;
        }
        [Property("ovalue")]
        public GridEntry? OValue
        {
            set
            {
                _value = value;
                if (_value != null)
                {
                    Text = value.Value?.ToString();
                    Editable = value.Editable;
                    Weight = value.Level == 0 ? 900 : 200;
                }
            }
            get => _value;
        }
        protected override void OnRender(Context cr, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cellArea, CellRendererState flags)
        {
            if (IsExpander)
            {
                if (_value.Level == 0)
                {
                    cellArea.X = 30;
                    background_area.X = 30;

                    cr.Save();
                    cr.SetSourceColor(new Cairo.Color() { A = 0.5, R = 0.9, G = 0.9, B = 0.9 });
                    cr.Paint();
                    cr.Restore();
                }
            }
            base.OnRender(cr, widget, background_area, cellArea, flags);
        }
    }
}
