/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;
using Pango;
using System.ComponentModel;

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
            base.Add(column);
            GridView.AppendColumn(column);
            if (GridView.IsRealized)
            {
                Invalidate();
            }
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
            int count = Count;
            if (__owner.UseModelFilter)
            {
                if (count > __owner.Store.NColumns)
                {
                    object[] columnTypes = new object[count];
                    __owner.Store.Clear();
                    __owner.Store = new Gtk.TreeStore(Array.ConvertAll(columnTypes, o => typeof(DataGridViewCell)));

                    Gtk.TreeModelFilter modelFilter = new TreeModelFilter(__owner.Store, null);
                    modelFilter.VisibleFunc = new TreeModelFilterVisibleFunc((model, iter) =>
                    {
                        if (model.GetValue(iter, 0) is DataGridViewCell cell)
                            return cell.OwningRowInternal.Visible;
                        else
                            return true;
                    });

                    __owner.GridView.Model = modelFilter;
                    foreach (DataGridViewColumn col in this)
                    {
                        col.SortColumnId = -1;
                    }
                }
                else if (__owner.GridView.Model == null)
                {
                    Gtk.TreeModelFilter modelFilter = new TreeModelFilter(__owner.Store, null);
                    modelFilter.VisibleFunc = new TreeModelFilterVisibleFunc((model, iter) =>
                    {
                        if (model.GetValue(iter, 0) is DataGridViewCell cell)
                            return cell.OwningRowInternal.Visible;
                        else
                            return true;
                    });
                    __owner.GridView.Model = modelFilter;
                    foreach (DataGridViewColumn col in this)
                    {
                        col.SortColumnId = -1;
                    }
                }
            }
            else
            {
                if (count > __owner.Store.NColumns)
                {
                    object[] columnTypes = new object[count];
                    __owner.Store.Clear();
                    __owner.Store = new Gtk.TreeStore(Array.ConvertAll(columnTypes, o => typeof(DataGridViewCell)));
                    __owner.GridView.Model = __owner.Store;
                    __owner.Store.DefaultSortFunc = new Gtk.TreeIterCompareFunc((Gtk.ITreeModel m, Gtk.TreeIter t1, Gtk.TreeIter t2) => { return 0; });
                    for (int i = 0; i < __owner.Store.NColumns && i < count; i++)
                    {
                        if (this[i].SortMode != DataGridViewColumnSortMode.NotSortable)
                        {
                            __owner.Store.SetSortFunc(i, new Gtk.TreeIterCompareFunc((Gtk.ITreeModel m, Gtk.TreeIter t1, Gtk.TreeIter t2) =>
                            {
                                ((Gtk.TreeStore)m).GetSortColumnId(out int sortid, out Gtk.SortType order);
                                DataGridViewCell v1 = m.GetValue(t1, sortid) as DataGridViewCell;
                                DataGridViewCell v2 = m.GetValue(t2, sortid) as DataGridViewCell;
                                if (v1?.Value == null || v2?.Value == null)
                                    return 0;
                                else if (v1.ValueType.IsValueType && long.TryParse(v1.Value.ToString(), out long rv1) && long.TryParse(v2.Value.ToString(), out long rv2))
                                    return rv1.CompareTo(rv2);
                                else if (v1.ValueType.Name == "DateTime")
                                    return ((DateTime)v1.Value).CompareTo((DateTime)v2.Value);
                                else
                                    return v1.Value.ToString().CompareTo(v2.Value.ToString());
                            }));
                        }
                    }
                }
                else if (__owner.GridView.Model == null)
                {
                    __owner.GridView.Model = __owner.Store;
                    __owner.Store.DefaultSortFunc = new Gtk.TreeIterCompareFunc((Gtk.ITreeModel m, Gtk.TreeIter t1, Gtk.TreeIter t2) => { return 0; });
                    for (int i = 0; i < __owner.Store.NColumns && i < count; i++)
                    {
                        if (this[i].SortMode != DataGridViewColumnSortMode.NotSortable)
                        {
                            __owner.Store.SetSortFunc(i, new Gtk.TreeIterCompareFunc((Gtk.ITreeModel m, Gtk.TreeIter t1, Gtk.TreeIter t2) =>
                            {
                                ((Gtk.TreeStore)m).GetSortColumnId(out int sortid, out Gtk.SortType order);
                                DataGridViewCell v1 = m.GetValue(t1, sortid) as DataGridViewCell;
                                DataGridViewCell v2 = m.GetValue(t2, sortid) as DataGridViewCell;
                                if (v1?.Value == null || v2?.Value == null)
                                    return 0;
                                else if (v1.ValueType.IsValueType && long.TryParse(v1.Value.ToString(), out long rv1) && long.TryParse(v2.Value.ToString(), out long rv2))
                                    return rv1.CompareTo(rv2);
                                else if (v1.ValueType.Name == "DateTime")
                                    return ((DateTime)v1.Value).CompareTo((DateTime)v2.Value);
                                else
                                    return v1.Value.ToString().CompareTo(v2.Value.ToString());
                            }));
                        }
                    }
                }
            }
            foreach (DataGridViewColumn col in this)
            {
                col.UpdateDefaultStyle();
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
