using System.Drawing;

namespace System.Windows.Forms;

public interface IDataGridViewCell
{
    DataGridView DataGridView { get; set; }
    object Value { get; set; }
    string ToolTipText { get; set; }
    object Tag { get; set; }
    DataGridViewCellStyle Style { get; set; }
    Size Size { get; }
    bool Selected { get; set; }
    int RowIndex { get; }
    bool Resizable { get; }
    bool ReadOnly { get; set; }
    Size PreferredSize { get; }
    DataGridViewRow OwningRow { get; }
    DataGridViewColumn OwningColumn { get; }
    bool IsInEditMode { get; }
    DataGridViewCellStyle InheritedStyle { get; }
    AccessibleObject AccessibilityObject { get; }
    int ColumnIndex { get; }
    Rectangle ContentBounds { get; }
    ContextMenuStrip ContextMenuStrip { get; set; }
    object DefaultNewRowValue { get; }
    bool Displayed { get; }
    object EditedFormattedValue { get; }
    Type EditType { get; }
    Rectangle ErrorIconBounds { get; }
    string ErrorText { get; set; }
    object FormattedValue { get; }
    Type FormattedValueType { get; }
    bool Frozen { get; }
    bool Visible { get; }
    bool HasStyle { get; }
    DataGridViewElementStates InheritedState { get; }
    Type ValueType { get; set; }
}