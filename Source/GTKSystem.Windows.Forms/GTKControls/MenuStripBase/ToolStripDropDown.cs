using System.Drawing;
using System.Text;

namespace System.Windows.Forms
{
    public class ToolStripDropDown : ToolStripItem
    {
        public StripDropDown self = new StripDropDown();
        public override IToolMenuItem Widget { get => self; }
        public ToolStripDropDown() : base()
        {

        }

        public Size ImageScalingSize { get; set; }
    }
}
