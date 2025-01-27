namespace System.Windows.Forms
{
    //
    // summary:
    //     Specifies the possible alignments with which the items of a System.Windows.Forms.ToolStrip
    //     can be displayed.
    public enum ToolStripLayoutStyle
    {
        //
        // summary:
        //     Specifies that items are laid out automatically.
        StackWithOverflow = 0,
        //
        // summary:
        //     Specifies that items are laid out horizontally and overflow as necessary.
        HorizontalStackWithOverflow = 1,
        //
        // summary:
        //     Specifies that items are laid out vertically, are centered within the control,
        //     and overflow as necessary.
        VerticalStackWithOverflow = 2,
        //
        // summary:
        //     Specifies that items flow horizontally or vertically as necessary.
        Flow = 3,
        //
        // summary:
        //     Specifies that items are laid out flush left.
        Table = 4
    }
}