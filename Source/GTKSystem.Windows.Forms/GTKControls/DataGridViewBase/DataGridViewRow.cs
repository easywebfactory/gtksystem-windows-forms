using Gtk;
using System.Collections.Generic;

namespace System.Windows.Forms
{
    public class DataGridViewRow
    {
        public int Index
        {
            get
            {
                if (Parent == null)
                {
                    return DataGridView.Rows.IndexOf(this);
                }
                else
                {
                    return Parent.Children.IndexOf(this);
                }
            }
        }
        public TreeIter TreeIter { get; internal set; }
        public DataGridView DataGridView { get; internal set; }
        public DataGridViewRow Parent { get; set; }
        private DataGridViewCellCollection _cell;
        private DataGridViewRowCollection _children;
        public DataGridViewRow()
        {
            _cell = new DataGridViewCellCollection(this);
        }
        public DataGridViewCellCollection Cells { get { return _cell; } }
        public DataGridViewRowCollection Children { get { if (_children == null) { _children = new DataGridViewRowCollection(DataGridView, this); } return _children; } }
        public object DataBoundItem { get; }
        public DataGridViewCellStyle DefaultCellStyle { get; set; }

        public bool Displayed { get; }

        public int DividerHeight { get; set; }

        public string ErrorText { get; set; }

        public bool Frozen { get; set; }

        public DataGridViewRowHeaderCell HeaderCell { get; set; }

        public int Height { get; set; } = 28;

        public  DataGridViewCellStyle InheritedStyle { get; }

        public bool IsNewRow { get; }

        public int MinimumHeight { get; set; }

        public bool ReadOnly { get; set; }

        public DataGridViewTriState Resizable { get; set; }

        public bool Selected { get => DataGridView.NativeRowGetSelected(this.TreeIter); set => DataGridView?.NativeRowSetSelected(this.TreeIter, value); }

        public DataGridViewElementStates State { get { return DataGridViewElementStates.None; } }

        public ContextMenuStrip ContextMenuStrip { get; set; }
        private bool _visible = true;
        public bool Visible
        {
            get => _visible;
            set
            {
                if (value != _visible)
                {
                    if (DataGridView.GridView.Model is Gtk.TreeModelFilter filter)
                    {
                        //当dataGridView.UseModelFilter = true时使用此模式，列不支持排序
                        _visible = value;
                        filter.Refilter();
                    }
                    else
                    {
                        //当dataGridView.UseModelFilter = false时使用此模式，列支持排序
                        if (DataGridView != null)
                        {
                            if (value == false)
                            {
                                Gtk.TreeIter iter = this.TreeIter;
                                if (Gtk.TreeIter.Zero.Equals(this.TreeIter) == false && this.TreeIter.UserData != null)
                                    DataGridView.Store.Remove(ref iter);
                                this.TreeIter = Gtk.TreeIter.Zero;
                            }
                            else if (Gtk.TreeIter.Zero.Equals(this.TreeIter))
                            {
                                int idx = Index + 1;
                                int count = DataGridView.Rows.Count;
                                for (int i = idx; i < count; i++)
                                {
                                    DataGridViewRow row = DataGridView.Rows[i];
                                    if (row.Visible)
                                    {
                                        TreePath path = DataGridView.Store.GetPath(row.TreeIter);
                                        if (path != null)
                                            DataGridView.Rows.InsertRowsStore(path.Indices.Last(), this);
                                        break;
                                    }
                                }
                            }
                        }
                        _visible = value;
                    }
                }
            }
        }
        public AccessibleObject AccessibilityObject { get; }

        public int GetHeight(int index)
        {
            return Height;
        }
    }
}