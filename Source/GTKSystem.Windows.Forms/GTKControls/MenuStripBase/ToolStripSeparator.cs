using System.Drawing;

namespace System.Windows.Forms
{

    public class ToolStripSeparator : ToolStripItem
    {
        public StripSeparator self = new StripSeparator();
        public override IToolMenuItem Widget { get => self; }
        public ToolStripSeparator() : base()
        {
         
        }
    }
}
