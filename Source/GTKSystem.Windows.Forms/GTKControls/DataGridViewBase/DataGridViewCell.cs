using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms;

public abstract class DataGridViewCell
{
    internal DataGridViewRow? OwningRowInternal { get; set; }
    public DataGridView? DataGridView { get; set; }
    public object? Value { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? ToolTipText { get; set; }
    [Bindable(true)]
    [DefaultValue(null)]
    [Localizable(false)]
    [TypeConverter(typeof(StringConverter))]
    public object? Tag { get; set; }
    [Browsable(true)]
    public DataGridViewCellStyle? Style { get; set; }

    public Size Size => default;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual bool Selected { get; set; }

    public int RowIndex
    {
        get;
        set;
    }

    public virtual bool Resizable => default;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual bool ReadOnly { get; set; }

    public Size PreferredSize => default;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public DataGridViewRow? OwningRow => default;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public DataGridViewColumn? OwningColumn => default;

    public bool IsInEditMode => default;

    public DataGridViewCellStyle? InheritedStyle => default;

    public AccessibleObject? AccessibilityObject => default;
    public int ColumnIndex
    {
        get;
        set;
    }

    public Rectangle ContentBounds => default;

    [DefaultValue(null)]
    public virtual ContextMenuStrip? ContextMenuStrip { get; set; }

    public virtual object? DefaultNewRowValue => default;

    public virtual bool Displayed => default;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public object? EditedFormattedValue => default;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public virtual Type? EditType => default;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public Rectangle ErrorIconBounds => default;

    public string? ErrorText { get; set; }

    public object? FormattedValue => default;

    public virtual Type? FormattedValueType => default;

    public virtual bool Frozen => default;

    public virtual bool Visible => default;

    public bool HasStyle => default;

    public DataGridViewElementStates InheritedState => default;

    public virtual Type? ValueType { get; set; }
    public DataGridViewCellStyle? RowStyle { get; set; }
}
public class DataGridViewTextBoxCell : DataGridViewCell
{
    //public DataGridViewTextBoxCell(DataGridViewRow dataGridViewRow) {
         
    //}
}
public class DataGridViewCheckBoxCell : DataGridViewCell;
public class DataGridViewComboBoxCell : DataGridViewCell;
public class DataGridViewButtonCell : DataGridViewCell;
public class DataGridViewImageCell : DataGridViewCell;
public class DataGridViewLinkCell : DataGridViewCell;