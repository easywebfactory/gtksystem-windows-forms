using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	[DefaultProperty("Value")]
	public class ToolStripProgressBar : WidgetToolStrip<Gtk.MenuItem>
    {
        public ToolStripProgressBar() : base("ToolStripProgressBar", null)
        {
            base.MenuItem.Realized += Control_Realized;
        }

        private void Control_Realized(object sender, EventArgs e)
        {
            progressBar.MaxValue = Maximum;
            progressBar.MinValue = Minimum;
        }

        public ToolStripProgressBar(string name) : this()
        {

        }
        public ProgressBar ProgressBar
		{
			get
			{
				return new ProgressBar() { Value = this.Value, Maximum = this.Maximum, Minimum = this.Minimum, Step = this.Step };
            }
		}

		public int MarqueeAnimationSpeed { get; set; } = 100;
        public int Maximum { get; set; } = 100;
        public int Minimum { get; set; } = 0;

        private int _step;
        [DefaultValue(10)]
        public int Step { get; set; }
        public ProgressBarStyle Style { get; set; }
        public int Value
        {
            get { return (int)Math.Round(progressBar.Value, 0); }
            set
            {
                progressBar.Value = value;
            }
        }

        public override Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
                progressBar.SetSizeRequest(value.Width, value.Height);
            }
        }
        public void Increment(int value)
		{

        }

		public void PerformStep()
		{
			
		}

    }
}
