// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace System.Windows.Forms
{
    public class DataGridViewCellPaintingEventArgs : HandledEventArgs
    {
        private readonly DataGridView _dataGridView;

        public DataGridViewCellPaintingEventArgs(DataGridView dataGridView,
                                                 Graphics graphics,
                                                 Rectangle clipBounds,
                                                 Rectangle cellBounds,
                                                 int rowIndex,
                                                 int columnIndex,
                                                 DataGridViewElementStates cellState,
                                                 object value,
                                                 object formattedValue,
                                                 string errorText,
                                                 DataGridViewCellStyle cellStyle,
                                                 DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                                 DataGridViewPaintParts paintParts)
        {
            //if ((paintParts & ~DataGridViewPaintParts.All) != 0)
            //{
            //    throw new ArgumentException("string.Format(SR.DataGridView_InvalidDataGridViewPaintPartsCombination, nameof(paintParts)), nameof(paintParts)");
            //}

            //_dataGridView = dataGridView ?? throw new ArgumentNullException(nameof(dataGridView));
            Graphics = graphics ?? throw new ArgumentNullException(nameof(graphics));
            ClipBounds = clipBounds;
            CellBounds = cellBounds;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            State = cellState;
            Value = value;
            FormattedValue = formattedValue;
            ErrorText = errorText;
            CellStyle = cellStyle ?? throw new ArgumentNullException(nameof(cellStyle));
            AdvancedBorderStyle = advancedBorderStyle;
            PaintParts = paintParts;
        }

        internal DataGridViewCellPaintingEventArgs(DataGridView dataGridView)
        {
            Debug.Assert(dataGridView != null);
            _dataGridView = dataGridView;
        }

        public Graphics Graphics { get; private set; }

        public DataGridViewAdvancedBorderStyle AdvancedBorderStyle { get; private set; }

        public Rectangle CellBounds { get; private set; }

        public Rectangle ClipBounds { get; private set; }

        public int RowIndex { get; private set; }

        public int ColumnIndex { get; private set; }

        public DataGridViewElementStates State { get; private set; }

        public object Value { get; private set; }

        public object FormattedValue { get; private set; }

        public string ErrorText { get; private set; }

        public DataGridViewCellStyle CellStyle { get; private set; }

        public DataGridViewPaintParts PaintParts { get; private set; }

        public void Paint(Rectangle clipBounds, DataGridViewPaintParts paintParts)
        {

        }

        public void PaintBackground(Rectangle clipBounds, bool cellsPaintSelectionBackground)
        {

        }

        public void PaintContent(Rectangle clipBounds)
        {

        }

        internal void SetProperties(Graphics graphics,
                                    Rectangle clipBounds,
                                    Rectangle cellBounds,
                                    int rowIndex,
                                    int columnIndex,
                                    DataGridViewElementStates cellState,
                                    object value,
                                    object formattedValue,
                                    string errorText,
                                    DataGridViewCellStyle cellStyle,
                                    DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                    DataGridViewPaintParts paintParts)
        {
            Debug.Assert(graphics != null);

            Graphics = graphics;
            ClipBounds = clipBounds;
            CellBounds = cellBounds;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            State = cellState;
            Value = value;
            FormattedValue = formattedValue;
            ErrorText = errorText;
            CellStyle = cellStyle;
            AdvancedBorderStyle = advancedBorderStyle;
            PaintParts = paintParts;
            Handled = false;
        }
    }
}
