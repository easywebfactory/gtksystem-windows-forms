using Gtk;

namespace System.Windows.Forms
{
    public class DataGridViewRow
    {
        public int Index { get; internal set; }
        public TreeIter TreeIter { get; internal set; }
        public DataGridView DataGridView { get; set; }
        private DataGridViewCellCollection _cell;
        private DataGridViewRowCollection _children;
        public DataGridViewRow()
        {
            _cell = new DataGridViewCellCollection(this);
        }
        public DataGridViewCellCollection Cells { get { return _cell; } }
        public DataGridViewRowCollection Children { get => _children ?? new DataGridViewRowCollection(DataGridView, this); }
        public object DataBoundItem { get; }
        public DataGridViewCellStyle DefaultCellStyle { get; set; }

    public bool Displayed => default;

    public int DividerHeight { get; set; }

    public string? ErrorText { get; set; }

    public bool Frozen { get; set; }

    public DataGridViewRowHeaderCell? HeaderCell { get; set; }

    public int Height { get; set; } = 28;

    public DataGridViewCellStyle? InheritedStyle => default;

    public bool IsNewRow => default;

    public int MinimumHeight { get; set; }

    public bool ReadOnly { get; set; }

    public DataGridViewTriState Resizable { get; set; }

    public bool Selected { get => DataGridView?.NativeRowGetSelected(Index)??default; set => DataGridView?.NativeRowSetSelected(Index, value); }
    //public bool Selected { get; set; }

    public DataGridViewElementStates State => DataGridViewElementStates.None;

    public ContextMenuStrip? ContextMenuStrip { get; set; }

    public bool Visible { get; set; }

    public AccessibleObject? AccessibilityObject => default;

    public int GetHeight(int index)
    {
        return Height;
    }
}