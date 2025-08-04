using Gtk;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DefaultProperty("Value")]
	public class ToolStripProgressBar : ToolStripItem
    {
        public StripToolItem self = new StripToolItem();
        public override IToolMenuItem Widget { get => self; }
        internal Gtk.LevelBar progressBar = new Gtk.LevelBar();
        public ToolStripProgressBar() : base()
        {
            progressBar.MaxValue = 100;
            progressBar.MinValue = 0;
            progressBar.Orientation = Gtk.Orientation.Horizontal;
            progressBar.Valign = Align.Center;
            self.BorderWidth = 0;
            self.Add(progressBar);
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
        public int Maximum { get => (int)progressBar.MaxValue; set => progressBar.MaxValue = value; }
        public int Minimum { get => (int)progressBar.MinValue; set => progressBar.MinValue = value; }

        private int _step;
        [DefaultValue(10)]
        public int Step { get; set; }
        private ProgressBarStyle style;
        public ProgressBarStyle Style { get => style; 
            set {
                style = value;
                if (style == ProgressBarStyle.Continuous)
                    progressBar.Mode = LevelBarMode.Continuous;
                else
                    progressBar.Mode = LevelBarMode.Discrete;
            }
        }
        public int Value
        {
            get { return (int)progressBar.Value; }
            set
            {
                progressBar.MaxValue = Maximum;
                progressBar.MinValue = Minimum;
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
                progressBar.SetSizeRequest(value.Width, value.Height);
                base.Size = value;
            }
        }
        public void Increment(int value)
		{
            progressBar.Value = Math.Min(Maximum, Math.Max(Minimum, progressBar.Value + value));
        }

		public void PerformStep()
		{
			
		}

    }
}
