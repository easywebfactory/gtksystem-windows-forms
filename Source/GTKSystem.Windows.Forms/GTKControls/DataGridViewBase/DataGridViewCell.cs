using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace System.Windows.Forms
{
    public abstract class DataGridViewCell
    {
        internal DataGridViewRow OwningRowInternal { get; set; }
        public DataGridView DataGridView { get => OwningRowInternal.DataGridView; }
        protected DataGridViewCell() { }
        public object Value { get; set; }
        public string ToolTipText { get; set; }
        public object Tag { get; set; }
        public DataGridViewCellStyle Style { get; set; }

        public Size Size { get; }
        private bool _Selected;
        public virtual bool Selected { get => _Selected; set { _Selected = value; DataGridView?.GridView.QueueDraw(); } }
        public int RowIndex { get => OwningRowInternal == null ? -1 : OwningRowInternal.Index; }

        public virtual bool Resizable { get; }

        public virtual bool ReadOnly { get; set; }

        public Size PreferredSize { get; }

        public DataGridViewRow OwningRow { get => OwningRowInternal; }

        public DataGridViewColumn OwningColumn { get; }

        public bool IsInEditMode { get; }
        internal DataGridViewCellStyle RowStyle { get => OwningRowInternal.DefaultCellStyle; }
        public DataGridViewCellStyle InheritedStyle { get; }

        public AccessibleObject AccessibilityObject { get; }
        public int ColumnIndex { get; internal set; }

        public Rectangle ContentBounds { get; }
        public virtual ContextMenuStrip ContextMenuStrip { get; set; }

        public virtual object DefaultNewRowValue { get; }

        public virtual bool Displayed { get; }

        public object EditedFormattedValue { get; }

        public virtual Type EditType { get; }

        public Rectangle ErrorIconBounds { get; }

        public string ErrorText { get; set; }

        public object FormattedValue { get; }

        public virtual Type FormattedValueType { get; }

        public virtual bool Frozen { get; }

        public virtual bool Visible { get; }

        public bool HasStyle { get; }

        public DataGridViewElementStates InheritedState { get; }
        private Type _valueType;
        public virtual Type ValueType { get { return _valueType == null ? Value?.GetType() : _valueType; } set { _valueType = value; } }
        public void OnCellPainting(DataGridViewCellPaintingEventArgs args)
        {
            OwningRowInternal?.DataGridView?.OnCellPainting(this, args);
        }
    }
    public class DataGridViewTextBoxCell : DataGridViewCell
    {

    }
    public class DataGridViewCheckBoxCell : DataGridViewCell
    {
    }
    public class DataGridViewRadioCell : DataGridViewCell
    {
    }
    public class DataGridViewComboBoxCell : DataGridViewCell
    {
    }
    public class DataGridViewButtonCell : DataGridViewCell
    {
    }
    public class DataGridViewImageCell : DataGridViewCell
    {
    }
    public class DataGridViewLinkCell : DataGridViewCell
    {
    }
}
