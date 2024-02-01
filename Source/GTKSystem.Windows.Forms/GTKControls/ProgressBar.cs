using Gtk;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    [DefaultProperty("Value")]
	public class ProgressBar : WidgetControl<Gtk.ProgressBar>
    {

        public ProgressBar():base()
        {
			base.Control.StyleContext.AddClass("ProgressBar");
        }

        public ProgressBarStyle Style { get; set; }
        [DefaultValue(100)]
		public int MarqueeAnimationSpeed { get; set; } = 100;
        [DefaultValue(100)]
		public int Maximum { get; set; } = 100;
        [DefaultValue(0)]
		public int Minimum { get; set; } = 0;
        public new Padding Padding { get; set; }

		private int _step;
        [DefaultValue(10)]
		public int Step { get { return _step; } set { _step = value; base.Control.PulseStep = value / Maximum; } }
        [DefaultValue(0)]
		public int Value { 
			get { return (int)Math.Round(base.Control.Fraction * Maximum, 0); }
            set
            {
                base.Control.Fraction = (value * 1.000001 / Maximum);
                base.Control.ShowText = true; base.Control.Text = $"{Math.Round((value * 100.001 / Maximum), 3)}%";
            }
        }
        public void Increment(int value)
		{
            base.Control.Fraction = (Value + value) / Maximum;
        }
		  
		public void PerformStep()
		{
	
		}
		 
		public override string ToString()
		{
			throw null;
		}

	}
}
