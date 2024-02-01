
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{

	public class ToolStripStatusLabel : ToolStripLabel
	{


        public ToolStripStatusLabel()
		{
			
		}

		public ToolStripStatusLabel(string text)
		{
			
		}

		public ToolStripStatusLabel(Image image)
		{
			
		}

		public ToolStripStatusLabel(string text, Image image)
		{
			
		}

		public ToolStripStatusLabel(string text, Image image, EventHandler onClick)
		{
			
		}

		public ToolStripStatusLabel(string text, Image image, EventHandler onClick, string name):base(text,image,onClick,name)
		{
        }

        [DefaultValue(Border3DStyle.Flat)]
        public Border3DStyle BorderStyle { get; set; }

        [DefaultValue(ToolStripStatusLabelBorderSides.None)]
        public ToolStripStatusLabelBorderSides BorderSides { get; set; }

        [DefaultValue(false)]
        public bool Spring { get; set; }
    }
}
