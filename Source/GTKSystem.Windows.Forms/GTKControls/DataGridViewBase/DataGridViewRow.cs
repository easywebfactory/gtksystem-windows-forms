namespace System.Windows.Forms
{
    public class DataGridViewRow
    {
        public int Index { get; internal set; }
        public IntPtr Handler { get; internal set; }
        public DataGridView DataGridView { get; set; }
        private DataGridViewCellCollection _cell;
        public DataGridViewRow()
        {
            _cell = new DataGridViewCellCollection(this);
        }
        public DataGridViewCellCollection Cells { get { return _cell; } }
        public bool SetValues(params object[] values) {
            foreach (object o in values)
                _cell.Add(new DataGridViewTextBoxCell() { Value = Convert.ToString(o), ValueType = typeof(object) });
            return true;
        }
        public object DataBoundItem { get; }

        public DataGridViewCellStyle DefaultCellStyle { get; set; }

        public bool Displayed { get; }

        public int DividerHeight { get; set; }

        public string ErrorText { get; set; }

        public bool Frozen { get; set; }

        public DataGridViewRowHeaderCell HeaderCell { get; set; }

        public int Height { get; set; }

        public  DataGridViewCellStyle InheritedStyle { get; }

        public bool IsNewRow { get; }

        public int MinimumHeight { get; set; }

        public bool ReadOnly { get; set; }

        public DataGridViewTriState Resizable { get; set; }

        public bool Selected { get; set; }

        public DataGridViewElementStates State { get { return DataGridViewElementStates.None; } }

        public ContextMenuStrip ContextMenuStrip { get; set; }

        public bool Visible { get; set; }

        public AccessibleObject AccessibilityObject { get; }

        public int GetHeight(int index)
        {
            return Height;
        }
    }
}