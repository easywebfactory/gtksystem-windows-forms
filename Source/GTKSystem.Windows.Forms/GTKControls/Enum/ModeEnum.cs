namespace System.Windows.Forms
{
    public enum PictureBoxSizeMode
    {
        //
        // summary:
        //     The image is placed in the upper-left corner of the System.Windows.Forms.PictureBox.
        //     The image is clipped if it is larger than the System.Windows.Forms.PictureBox
        //     it is contained in.
        Normal = 0,
        //
        // summary:
        //     The image within the System.Windows.Forms.PictureBox is stretched or shrunk to
        //     fit the size of the System.Windows.Forms.PictureBox.
        StretchImage = 1,
        //
        // summary:
        //     The System.Windows.Forms.PictureBox is sized equal to the size of the image that
        //     it contains.
        AutoSize = 2,
        //
        // summary:
        //     The image is displayed in the center if the System.Windows.Forms.PictureBox is
        //     larger than the image. If the image is larger than the System.Windows.Forms.PictureBox,
        //     the picture is placed in the center of the System.Windows.Forms.PictureBox and
        //     the outside edges are clipped.
        CenterImage = 3,
        //
        // summary:
        //     The size of the image is increased or decreased maintaining the size ratio.
        Zoom = 4
    }
    public enum MaskFormat
    {
        //
        // summary:
        //     Return only text input by the user.
        ExcludePromptAndLiterals = 0,
        //
        // summary:
        //     Return text input by the user as well as any instances of the prompt character.
        IncludePrompt = 1,
        //
        // summary:
        //     Return text input by the user as well as any literal characters defined in the
        //     mask.
        IncludeLiterals = 2,
        //
        // summary:
        //     Return text input by the user as well as any literal characters defined in the
        //     mask and any instances of the prompt character.
        IncludePromptAndLiterals = 3
    }
    //
    // summary:
    //     Specifies how tabs in a tab control are sized.
    public enum TabSizeMode
    {
        //
        // summary:
        //     The width of each tab is sized to accommodate what is displayed on the tab, and
        //     the size of tabs in a row are not adjusted to fill the entire width of the container
        //     control.
        Normal = 0,
        //
        // summary:
        //     The width of each tab is sized so that each row of tabs fills the entire width
        //     of the container control. This is only applicable to tab controls with more than
        //     one row.
        FillToRight = 1,
        //
        // summary:
        //     All tabs in a control are the same width.
        Fixed = 2
    }

    public enum HighDpiMode
    {
        DpiUnaware = 0,
        SystemAware = 1,
        PerMonitor = 2,
        PerMonitorV2 = 3,
        DpiUnawareGdiScaled = 4
    }


    public enum DataGridViewAutoSizeColumnMode
    {
        //
        // summary:
        //     The sizing behavior of the column is inherited from the System.Windows.Forms.DataGridView.AutoSizeColumnsMode
        //     property.
        NotSet = 0,
        //
        // summary:
        //     The column width does not automatically adjust.
        None = 1,
        //
        // summary:
        //     The column width adjusts to fit the contents of the column header cell.
        ColumnHeader = 2,
        //
        // summary:
        //     The column width adjusts to fit the contents of all cells in the column, excluding
        //     the header cell.
        AllCellsExceptHeader = 4,
        //
        // summary:
        //     The column width adjusts to fit the contents of all cells in the column, including
        //     the header cell.
        AllCells = 6,
        //
        // summary:
        //     The column width adjusts to fit the contents of all cells in the column that
        //     are in rows currently displayed onscreen, excluding the header cell.
        DisplayedCellsExceptHeader = 8,
        //
        // summary:
        //     The column width adjusts to fit the contents of all cells in the column that
        //     are in rows currently displayed onscreen, including the header cell.
        DisplayedCells = 10,
        //
        // summary:
        //     The column width adjusts so that the widths of all columns exactly fills the
        //     display area of the control, requiring horizontal scrolling only to keep column
        //     widths above the System.Windows.Forms.DataGridViewColumn.MinimumWidth property
        //     values. Relative column widths are determined by the relative System.Windows.Forms.DataGridViewColumn.FillWeight
        //     property values.
        Fill = 16
    }

    public enum DataGridViewColumnSortMode
    {
        //
        // summary:
        //     The column can only be sorted programmatically, but it is not intended for sorting,
        //     so the column header will not include space for a sorting glyph.
        NotSortable = 0,
        //
        // summary:
        //     The user can sort the column by clicking the column header (or pressing F3 on
        //     a cell) unless the column headers are used for selection. A sorting glyph will
        //     be displayed automatically.
        Automatic = 1,
        //
        // summary:
        //     The column can only be sorted programmatically, and the column header will include
        //     space for a sorting glyph.
        Programmatic = 2
    }

}

namespace System.Drawing.Drawing2D
{
    //
    // summary:
    //     The System.Drawing.Drawing2D.InterpolationMode enumeration specifies the algorithm
    //     that is used when images are scaled or rotated.
    public enum InterpolationMode
    {
        //
        // summary:
        //     Equivalent to the System.Drawing.Drawing2D.QualityMode.Invalid element of the
        //     System.Drawing.Drawing2D.QualityMode enumeration.
        Invalid = -1,
        //
        // summary:
        //     Specifies default mode.
        Default = 0,
        //
        // summary:
        //     Specifies low quality interpolation.
        Low = 1,
        //
        // summary:
        //     Specifies high quality interpolation.
        High = 2,
        //
        // summary:
        //     Specifies bilinear interpolation. No prefiltering is done. This mode is not suitable
        //     for shrinking an image below 50 percent of its original size.
        Bilinear = 3,
        //
        // summary:
        //     Specifies bicubic interpolation. No prefiltering is done. This mode is not suitable
        //     for shrinking an image below 25 percent of its original size.
        Bicubic = 4,
        //
        // summary:
        //     Specifies nearest-neighbor interpolation.
        NearestNeighbor = 5,
        //
        // summary:
        //     Specifies high-quality, bilinear interpolation. Prefiltering is performed to
        //     ensure high-quality shrinking.
        HighQualityBilinear = 6,
        //
        // summary:
        //     Specifies high-quality, bicubic interpolation. Prefiltering is performed to ensure
        //     high-quality shrinking. This mode produces the highest quality transformed images.
        HighQualityBicubic = 7
    }
}
