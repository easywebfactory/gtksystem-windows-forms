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
        public DataGridView DataGridView { get; set; }
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
        /// <summary>
        /// 此属性无法还原行位置，建议结合列排序使用，利用排序定行位
        /// </summary>
        public bool Visible
        {
            get => _visible;
            set
            {
                if (value != _visible)
                {
                    _visible = value;
                    if (DataGridView != null && DataGridView.self.IsVisible)
                    {
                        //通过增删行实现隐藏，无法还原行位置
                        if (_visible == false)
                        {
                            Gtk.TreeIter iter = this.TreeIter;
                            if (Gtk.TreeIter.Zero.Equals(this.TreeIter) == false && this.TreeIter.UserData != null)
                                DataGridView.Store.Remove(ref iter);
                            this.TreeIter = Gtk.TreeIter.Zero;
                        }
                        else if (Gtk.TreeIter.Zero.Equals(this.TreeIter))
                        {
                            DataGridView.Rows.AddRowsStore(this);
                        }
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