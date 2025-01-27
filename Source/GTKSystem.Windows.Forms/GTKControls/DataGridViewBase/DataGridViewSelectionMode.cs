namespace System.Windows.Forms
{
    //
    // summary:
    //     Describes how cells of a DataGridView control can be selected.
    public enum DataGridViewSelectionMode
    {
        //
        // summary:
        //     One or more individual cells can be selected.
        CellSelect = 0,
        //
        // summary:
        //     The entire row will be selected by clicking its row's header or a cell contained
        //     in that row.
        FullRowSelect = 1,
        //
        // summary:
        //     The entire column will be selected by clicking the column's header or a cell
        //     contained in that column.
        FullColumnSelect = 2,
        //
        // summary:
        //     The row will be selected by clicking in the row's header cell. An individual
        //     cell can be selected by clicking that cell.
        RowHeaderSelect = 3,
        //
        // summary:
        //     The column will be selected by clicking in the column's header cell. An individual
        //     cell can be selected by clicking that cell.
        ColumnHeaderSelect = 4
    }
}
