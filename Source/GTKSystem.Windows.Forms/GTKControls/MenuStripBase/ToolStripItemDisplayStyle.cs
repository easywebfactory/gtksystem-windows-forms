namespace System.Windows.Forms;

//
// 摘要:
//     Specifies what to render (image or text) for this System.Windows.Forms.ToolStripItem.
public enum ToolStripItemDisplayStyle
{
    //
    // 摘要:
    //     Specifies that neither image nor text is to be rendered for this System.Windows.Forms.ToolStripItem.
    None = 0,
    //
    // 摘要:
    //     Specifies that only text is to be rendered for this System.Windows.Forms.ToolStripItem.
    Text = 1,
    //
    // 摘要:
    //     Specifies that only an image is to be rendered for this System.Windows.Forms.ToolStripItem.
    Image = 2,
    //
    // 摘要:
    //     Specifies that both an image and text are to be rendered for this System.Windows.Forms.ToolStripItem.
    ImageAndText = 3
}