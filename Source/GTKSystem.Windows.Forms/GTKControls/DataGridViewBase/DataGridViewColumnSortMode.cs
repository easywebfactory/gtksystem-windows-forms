using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Forms
{
    public enum DataGridViewColumnSortMode
    {
        //
        // 摘要:
        //     The column can only be sorted programmatically, but it is not intended for sorting,
        //     so the column header will not include space for a sorting glyph.
        NotSortable = 0,
        //
        // 摘要:
        //     The user can sort the column by clicking the column header (or pressing F3 on
        //     a cell) unless the column headers are used for selection. A sorting glyph will
        //     be displayed automatically.
        Automatic = 1,
        //
        // 摘要:
        //     The column can only be sorted programmatically, and the column header will include
        //     space for a sorting glyph.
        Programmatic = 2
    }
}
