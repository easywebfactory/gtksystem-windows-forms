using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Forms
{
    //
    // 摘要:
    //     Specifies how tabs in a tab control are sized.
    public enum TabSizeMode
    {
        //
        // 摘要:
        //     The width of each tab is sized to accommodate what is displayed on the tab, and
        //     the size of tabs in a row are not adjusted to fill the entire width of the container
        //     control.
        Normal = 0,
        //
        // 摘要:
        //     The width of each tab is sized so that each row of tabs fills the entire width
        //     of the container control. This is only applicable to tab controls with more than
        //     one row.
        FillToRight = 1,
        //
        // 摘要:
        //     All tabs in a control are the same width.
        Fixed = 2
    }
}
