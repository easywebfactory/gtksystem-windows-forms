/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Atk;
using Cairo;
using Gdk;
using GLib;
using Gtk;
using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Forms.PropertyGridInternal;

internal sealed partial class PropertyGridView 
{
    public Gtk.TreeView tree = new Gtk.TreeView();
    public Gtk.TreeStore store = new Gtk.TreeStore(typeof(GridEntry));
    public PropertyGrid OwnerGrid { get; private set; }
    private object _selectedObject;
    public PropertyGridView(PropertyGrid propertyGrid)
    {
        OwnerGrid = propertyGrid;
        tree.ShowExpanders = false;
        tree.EnableTreeLines = false;
        tree.EnableGridLines = TreeViewGridLines.Horizontal;
        IconTheme iconTheme = new IconTheme();
        Gtk.TreeViewColumn column1 = new Gtk.TreeViewColumn();
        column1.FixedWidth = 30;
        column1.Resizable = false;
        column1.Clickable = false;
        column1.SortIndicator = false;
        column1.Reorderable = false;
        Gtk.Button button1 = column1.Button as Gtk.Button;
        if (button1.Child is Gtk.Box box1)
        {
            Gtk.Image img = new Gtk.Image(this.GetType().Assembly, "GTKSystem.Windows.Forms.Resources.System.PBCategory.ico");
            img.Visible = true;
            box1.PackStart(img, false, true, 1);
            box1.ReorderChild(img, 0);
        }

        CellRendererExpander expander = new CellRendererExpander(this);
        expander.CellBackgroundRgba = new RGBA() { Alpha = 0.6, Red = 0.9, Green = 0.9, Blue = 0.9 };
        column1.PackStart(expander, false);
        column1.AddAttribute(expander, "icon", 0);

        Gtk.TreeViewColumn column2 = new Gtk.TreeViewColumn();
        column2.MinWidth = 20;
        column2.FixedWidth = 150;
        column2.Resizable = true;
        column2.Clickable = false;
        column2.Title = " ";
        column2.SortIndicator = true;
        column2.SortOrder = SortType.Ascending;
        column2.Reorderable = false;
        column2.Button.Name = "key";
        Gtk.Button button2 = column2.Button as Gtk.Button;
        if (button2.Child is Gtk.Box box)
        {
            Gtk.Image img = new Gtk.Image(this.GetType().Assembly, "GTKSystem.Windows.Forms.Resources.System.PBAlpha.ico");
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

        Gtk.TreeViewColumn column3 = new Gtk.TreeViewColumn();
        column3.MinWidth = 10;
        //column3.FixedWidth = 50;
        column3.Resizable = false;
        column3.Clickable = false;
        column3.Title = " ";
        column3.SortIndicator = false;
        column3.Reorderable = false;
        column3.Button.Name = "value";
        Gtk.CellRendererText values = new CellRendererProperty(this);
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
            GridEntry val1 = model.GetValue(iter1, 0) as GridEntry;
            GridEntry val2 = model.GetValue(iter2, 0) as GridEntry;
            return val1.Value.ToString().CompareTo(val2.Value.ToString());
        }));

        tree.Model = store;
    }
    private GridEntry oldSelectedPropertyItem;
    private void Selection_Changed(object sender, EventArgs e)
    {
        if (tree.Selection.GetSelected(out ITreeModel model, out Gtk.TreeIter oiter))
        {
            GridEntry propertyItem = store.GetValue(oiter, 0) as GridEntry;
            if (SelectedGridItemChanged != null)
                SelectedGridItemChanged(sender, new SelectedGridItemChangedEventArgs(oldSelectedPropertyItem, propertyItem));
            oldSelectedPropertyItem = propertyItem;
        }
    }
    private void Tree_RowActivated(object o, RowActivatedArgs args)
    {
        Console.WriteLine(args.Path);
        if (store.GetIter(out TreeIter iter, args.Path))
        {
            if (store.GetValue(iter, 0) is GridEntry ov)
            {
                OwnerGrid.self.ShowDescription(ov.Label, ov.Description);
            }
        }
    }
    public void LoadPropertyInfo(object entryobj)
    {
        _selectedObject = entryobj;
        List<GridEntry> categorys = new List<GridEntry>();
        PropertyInfo[] propertyInfos = entryobj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var propertyInfo in propertyInfos)
        {
            var attri = propertyInfo.GetCustomAttributes(typeof(BrowsableAttribute));
            if (attri != null && attri.Count() > 0)
            {
                //BrowsableAttribute
                BrowsableAttribute attribute = (BrowsableAttribute)attri.FirstOrDefault();
                if (attribute.Browsable == false)
                {
                    continue;
                }
            }
            string category = "【未分类组】";
            string description = "";

            var attri2 = propertyInfo.GetCustomAttributes(typeof(CategoryAttribute));
            if (attri2 != null && attri2.Count() > 0)
            {
                CategoryAttribute attribute = (CategoryAttribute)attri2.FirstOrDefault();
                category = attribute.Category;
            }
            var attri3 = propertyInfo.GetCustomAttributes(typeof(DescriptionAttribute));
            if (attri3 != null && attri3.Count() > 0)
            {
                DescriptionAttribute attribute = (DescriptionAttribute)attri3.FirstOrDefault();
                description = attribute.Description;
            }

            GridEntry propertyItem = new GridEntry(null, GridItemType.Category, 0, category, category, description);
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
                GridEntry value0 = new GridEntry(null, GridItemType.Category, 0, g.Key, g.Key, g.Key) { Level = 0, Editable = false };
                Gtk.TreeIter category1 = store.AppendValues(value0);
                foreach (var m in g)
                {
                    GetPropertyInfo(category1, value0, m.PropertyInfo, entryobj);
                }
            }
        }
        tree.ExpandAll();
    }
    private void GetPropertyInfos(Gtk.TreeIter? parent, GridEntry parentitem, object obj)
    {
        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
        foreach (PropertyInfo property in properties)
        {
            GetPropertyInfo(parent, parentitem, property, obj);
        }
    }
    private bool GetPropertyInfo(Gtk.TreeIter? parent, GridEntry parentitem, PropertyInfo property, object obj)
    {
        if (property.CanRead == true)
        {
            var attri = property.GetCustomAttributes(typeof(BrowsableAttribute));
            if (attri != null && attri.Count() > 0)
            {
                //BrowsableAttribute
                BrowsableAttribute attribute = (BrowsableAttribute)attri.FirstOrDefault();
                if (attribute.Browsable == false)
                {
                    return false;
                }
            }
            string description = "";
            var attri3 = property.GetCustomAttributes(typeof(DescriptionAttribute));
            if (attri3 != null && attri3.Count() > 0)
            {
                DescriptionAttribute attribute = (DescriptionAttribute)attri3.FirstOrDefault();
                description = attribute.Description;
            }
            if (property.PropertyType.IsEnum)
            {
                object val = property.GetValue(obj);
                GridEntry value1 = new GridEntry(parentitem, GridItemType.Property, 1, property.Name, val, description) { Editable = property.CanWrite, ValueType = property.PropertyType };
                StoreValue(parent, value1);
            }
            else if (property.PropertyType.IsValueType)
            {
                object val = property.GetValue(obj);
                if (property.PropertyType.IsPrimitive == false)
                { //只有基元类型可以编辑
                    GridEntry value1 = new GridEntry(parentitem, GridItemType.Property, 1, property.Name, val, description) { ValueType = property.PropertyType };
                    Gtk.TreeIter node1 = StoreValue(parent, value1);
                    GetPropertyInfos(node1, value1, val);
                }
                else
                {
                    GridEntry value1 = new GridEntry(parentitem, GridItemType.Property, 1, property.Name, val, description) { Editable = property.CanWrite, ValueType = property.PropertyType };
                    Gtk.TreeIter node1 = StoreValue(parent, value1);
                }
            }
            else if (property.PropertyType.IsArray)
            {
                object val = property.GetValue(obj);
                GridEntry value1 = new GridEntry(parentitem, GridItemType.Property, 1, property.Name, val, description) { Editable = false, ValueType = property.PropertyType };
                Gtk.TreeIter node1 = StoreValue(parent, value1);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    private Gtk.TreeIter StoreValue(Gtk.TreeIter? parent, object value)
    {
        if (parent == null)
            return store.AppendValues(value);
        else
            return store.AppendValues(parent.Value, value);
    }
    public event PropertyValueChangedEventHandler PropertyValueChanged;
    public event SelectedGridItemChangedEventHandler SelectedGridItemChanged;
    public class CellRendererExpander : Gtk.CellRendererToggle
    {
        Pixbuf gonextpixbuf = new Pixbuf(typeof(CellRendererExpander).Assembly, "GTKSystem.Windows.Forms.Resources.System.go-next-symbolic.png");
        Pixbuf godownixbuf = new Pixbuf(typeof(CellRendererExpander).Assembly, "GTKSystem.Windows.Forms.Resources.System.go-down-symbolic.png");
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
        protected override bool OnActivate(Event evnt, Widget widget, string path, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, CellRendererState flags)
        {
            bool re = base.OnActivate(evnt, widget, path, background_area, cell_area, flags);
            Gtk.TreeView tree = widget as Gtk.TreeView;
            if (this.IsExpanded)
                tree.CollapseRow(new TreePath(path));
            else
                tree.ExpandToPath(new TreePath(path));

            return re;
        }
        protected override void OnRender(Context cr, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, CellRendererState flags)
        {
            if (this.IsExpander)
            {
                cr.Save();
                if (this.IsExpanded)
                    Gdk.CairoHelper.SetSourcePixbuf(cr, godownixbuf, background_area.Left + 10, background_area.Top + 5);
                else
                    Gdk.CairoHelper.SetSourcePixbuf(cr, gonextpixbuf, background_area.Left + 10, background_area.Top + 5);
                cr.Paint();
                cr.Restore();
            }
        }
    }
    public class CellRendererProperty : Gtk.CellRendererCombo
    {
        PropertyGridView owner;
        Gtk.ListStore model = new Gtk.ListStore(typeof(string));
        public CellRendererProperty(PropertyGridView owner)
        {
            this.owner = owner;
            this.TextColumn = 0;
            this.Height = 20;
            this.SetFixedSize(50, 20);
            this.Model = model;
            this.EditingStarted += CellRendererText_EditingStarted;
            this.Edited += CellRendererText_Edited;
        }

        private void CellRendererText_EditingStarted(object o, EditingStartedArgs args)
        {
            model.Clear();
            if (_value.ValueType != null)
            {
                if (_value.ValueType.IsEnum)
                {
                    string[] keys = Enum.GetNames(_value.ValueType);
                    foreach (string key in keys)
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
            TreePath path = new TreePath(args.Path);
            var model = ((Gtk.TreeView)owner.tree).Model;
            model.GetIter(out TreeIter iter, path);
            object cell = model.GetValue(iter, 0);
            if (cell is GridEntry val)
            {
                object oldvalue = val.Value;
                try
                {
                    if (path.Depth <= 2)
                    {
                        object obj = owner._selectedObject;
                        PropertyInfo info = obj.GetType().GetProperty(val.Label);
                        var value = info.GetValue(obj);
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
                            object obj = owner._selectedObject;
                            PropertyInfo info = obj.GetType().GetProperty(val.Label);
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
                            if (model.IterParent(out TreeIter piter, iter))
                            {   //取父对象
                                object pcell = model.GetValue(piter, 0);
                                if (pcell is GridEntry pval)
                                {
                                    object obj = owner._selectedObject;
                                    Type otype = obj.GetType();
                                    PropertyInfo pinfo = otype.GetProperty(pval.Label);
                                    var pvalue = pinfo.GetValue(obj);
                                    //从obj取当前对象，赋值
                                    PropertyInfo info = pvalue.GetType().GetProperty(val.Label);
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
        private GridEntry _value;
        [Property("otext")]
        public GridEntry OText
        {
            set
            {
                _value = value;
                if (_value != null)
                {
                    base.Text = value.Label;
                    base.Editable = false;
                    base.Weight = value.Level == 0 ? 900 : 200;
                }

            }
            get { return _value; }
        }
        [Property("ovalue")]
        public GridEntry OValue
        {
            set
            {
                _value = value;
                if (_value != null)
                {
                    base.Text = value.Value?.ToString();
                    base.Editable = value.Editable;
                    base.Weight = value.Level == 0 ? 900 : 200;
                }
            }
            get { return _value; }
        }
        protected override void OnRender(Context cr, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, CellRendererState flags)
        {
            if (this.IsExpander)
            {
                if (_value.Level == 0)
                {
                    cell_area.X = 30;
                    background_area.X = 30;

                    cr.Save();
                    cr.SetSourceColor(new Cairo.Color() { A = 0.5, R = 0.9, G = 0.9, B = 0.9 });
                    cr.Paint();
                    cr.Restore();
                }
            }
            base.OnRender(cr, widget, background_area, cell_area, flags);
        }
    }
}
