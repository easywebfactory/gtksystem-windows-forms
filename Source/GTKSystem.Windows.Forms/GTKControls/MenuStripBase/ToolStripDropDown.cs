using System.Drawing;
using Gtk;

namespace System.Windows.Forms;

public class ToolStripDropDown : ToolStripItem
{
    //public readonly ToolStripDropDownBase self = new ToolStripDropDownBase();
    public readonly Menu self = new();
    public override Widget Widget => self;

    public Size ImageScalingSize { get; set; }
}