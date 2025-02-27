/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using System.ComponentModel;
using System.Windows.Forms.GtkRender;

namespace System.Windows.Forms
{
    public class DataGridViewColumnCollection : List<DataGridViewColumn>
    {
        public event CollectionChangeEventHandler CollectionChanged;
        private DataGridView __owner;
        private Gtk.TreeView GridView;
        public DataGridViewColumnCollection(DataGridView dataGridView)
        {
            __owner = dataGridView;
            GridView = dataGridView.GridView;
        }

        public virtual DataGridViewColumn this[string columnName] { get { return base.Find(m => m.Name == columnName); } }

        protected DataGridView DataGridView { get { return __owner; } }
        
        public void Add(string columnName, string headerText)
        {
            DataGridViewColumn column = new DataGridViewColumn() { Name = columnName, HeaderText = headerText };
            Add(column);
        }
        public new void Add(DataGridViewColumn column)
        {
            column.DataGridView = __owner;
            column.Index = Count;
            column.DisplayIndex = column.Index;
            column.SetGridViewDefaultStyle(__owner.DefaultCellStyle);

            DataGridViewCellStyle _cellStyle = column.DefaultCellStyle;
            if (__owner.ColumnHeadersDefaultCellStyle != null)
                _cellStyle = __owner.ColumnHeadersDefaultCellStyle;

            if (_cellStyle != null)
            {
                Gtk.Button header = ((Gtk.Button)column.Button);
                string style = "";
                if (_cellStyle.BackColor.Name != "0")
                {
                    string backcolor = $"rgba({_cellStyle.BackColor.R},{_cellStyle.BackColor.G},{_cellStyle.BackColor.B},{_cellStyle.BackColor.A})";
                    style += $".columnheaderbackcolor{{background-color:{backcolor};}} ";
                    header.StyleContext.AddClass("columnheaderbackcolor");
                }
                if (_cellStyle.ForeColor.Name != "0")
                {
                    string forecolor = $"rgba({_cellStyle.ForeColor.R},{_cellStyle.ForeColor.G},{_cellStyle.ForeColor.B},{_cellStyle.ForeColor.A})";
                    style += $".columnheaderforecolor{{color:{forecolor};}} ";
                    header.StyleContext.AddClass("columnheaderforecolor");
                }
                if (style.Length > 9)
                {
                    Gtk.CssProvider css = new Gtk.CssProvider();
                    css.LoadFromData(style);
                    header.StyleContext.AddProvider(css, 800);
                }
                switch (_cellStyle.Alignment)
                {
                    case DataGridViewContentAlignment.TopLeft:
                    case DataGridViewContentAlignment.MiddleLeft:
                    case DataGridViewContentAlignment.BottomLeft:
                        column.Alignment = 0f;
                        break;

                    case DataGridViewContentAlignment.TopCenter:
                    case DataGridViewContentAlignment.MiddleCenter:
                    case DataGridViewContentAlignment.BottomCenter:
                        column.Alignment = 0.5f;
                        break;

                    case DataGridViewContentAlignment.TopRight:
                    case DataGridViewContentAlignment.MiddleRight:
                    case DataGridViewContentAlignment.BottomRight:
                        column.Alignment = 1.0f;
                        break;

                    default:
                        column.Alignment = 0f;
                        break;
                }
            }

            base.Add(column);
            if (__owner.self.IsRealized)
            {
                Invalidate();
            }
            GridView.AppendColumn(column);
            OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, column));
        }
        public new void AddRange(IEnumerable<DataGridViewColumn> columns)
        {
            foreach (DataGridViewColumn column in columns)
            {
                Add(column);
            }
        }
        public new bool Remove(DataGridViewColumn column)
        {
            __owner.GridView.RemoveColumn(column);
            bool ir= base.Remove(column);
            OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, column));
            return ir;
        }
        public new void RemoveAt(int index)
        {
            this.Remove(this[index]);
        }
        public new void Clear()
        {
            base.Clear();
            foreach (var wik in GridView.Columns)
                GridView.RemoveColumn(wik);

            OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, __owner));
        }
        public void Invalidate()
        {
            if (Count > __owner.Store.NColumns)
            {
                object[] columnTypes = new object[Count];
                __owner.Store.Clear();
                __owner.Store = new Gtk.TreeStore(Array.ConvertAll(columnTypes, o => typeof(DataGridViewCell)));
                __owner.GridView.Model = __owner.Store;
            }
            else if (__owner.GridView.Model == null)
            {
                __owner.GridView.Model = __owner.Store;
            }
            __owner.Store.DefaultSortFunc = new Gtk.TreeIterCompareFunc((Gtk.ITreeModel m, Gtk.TreeIter t1, Gtk.TreeIter t2) => { return 0; });
            for (int i=0;i < __owner.Store.NColumns; i++)
            {
                __owner.Store.SetSortFunc(i, new Gtk.TreeIterCompareFunc((Gtk.ITreeModel m, Gtk.TreeIter t1, Gtk.TreeIter t2) =>
                {
                    ((Gtk.TreeStore)m).GetSortColumnId(out int sortid, out Gtk.SortType order);
                    DataGridViewCell v1 = m.GetValue(t1, sortid) as DataGridViewCell;
                    DataGridViewCell v2 = m.GetValue(t2, sortid) as DataGridViewCell;
                    if (v1?.Value == null || v2?.Value == null)
                        return 0;
                    else if(int.TryParse(v1.Value.ToString(), out int rv1) && int.TryParse(v2.Value.ToString(), out int rv2))
                        return (rv2 - rv1);
                    else if (DateTime.TryParse(v1.Value.ToString(), out DateTime rd1) && DateTime.TryParse(v2.Value.ToString(), out DateTime rd2))
                        return (int)((rd2 - rd1).TotalSeconds);
                    else
                        return v2.Value.ToString().CompareTo(v1.Value.ToString());
                }));
            }
        }
        public int GetColumnCount(DataGridViewElementStates includeFilter)
        {
            return FindAll(m => m.State == includeFilter).Count;
        }

        public int GetColumnsWidth(DataGridViewElementStates includeFilter)
        {
            DataGridViewColumn co = Find(m => m.State == includeFilter);
            return co == null ? __owner.RowHeadersWidth : co.Width;
        }

        public DataGridViewColumn GetFirstColumn(DataGridViewElementStates includeFilter)
        {
            return Find(m => m.State == includeFilter);
        }

        public DataGridViewColumn GetFirstColumn(DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
        {
            return Find(m => m.State == includeFilter && m.State == excludeFilter);
        }

        public DataGridViewColumn GetLastColumn(DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
        {
            return FindLast(m => m.State == includeFilter && m.State == excludeFilter);
        }

        public DataGridViewColumn GetNextColumn(DataGridViewColumn dataGridViewColumnStart, DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
        {
            int ix = FindIndex(m => m.Name == dataGridViewColumnStart.Name && m.State == includeFilter && m.State == excludeFilter);
            return ix < Count ? base[ix] : null;
        }
        protected virtual void OnCollectionChanged(CollectionChangeEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(__owner, e);
        }
    }
}
