using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace System.Windows.Forms
{
	[DefaultProperty("Value")]
	public class ToolStripProgressBar : WidgetToolStrip<Gtk.MenuItem>
    {
        public ToolStripProgressBar() : base("ToolStripProgressBar", null)
        {

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
        public int Step { get { return _step; } set { _step = value; progressBar.PulseStep = value / Maximum; } }

        public ProgressBarStyle Style { get; set; }

        public int Value { 
            get { return (int)Math.Round(progressBar.Fraction * Maximum, 0); } 
            set { 
                progressBar.Fraction = (value*1.000001 / Maximum);
                progressBar.ShowText = true; progressBar.Text = $"{Math.Round((value * 100.001 / Maximum),3)}%";
            }
        }

        public void Increment(int value)
		{
            progressBar.Fraction = (Value + value) / Maximum;
        }

		public void PerformStep()
		{
			
		}
	}
}
