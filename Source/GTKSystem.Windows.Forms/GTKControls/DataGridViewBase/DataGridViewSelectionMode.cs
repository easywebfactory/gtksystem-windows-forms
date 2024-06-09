using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Forms
{
    //
    // 摘要:
    //     Describes how cells of a DataGridView control can be selected.
    public enum DataGridViewSelectionMode
    {
        //
        // 摘要:
        //     One or more individual cells can be selected.
        CellSelect = 0,
        //
        // 摘要:
        //     The entire row will be selected by clicking its row's header or a cell contained
        //     in that row.
        FullRowSelect = 1,
        //
        // 摘要:
        //     The entire column will be selected by clicking the column's header or a cell
        //     contained in that column.
        FullColumnSelect = 2,
        //
        // 摘要:
        //     The row will be selected by clicking in the row's header cell. An individual
        //     cell can be selected by clicking that cell.
        RowHeaderSelect = 3,
        //
        // 摘要:
        //     The column will be selected by clicking in the column's header cell. An individual
        //     cell can be selected by clicking that cell.
        ColumnHeaderSelect = 4
    }
}
