//using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public class HScrollBar : Control
    {
        public readonly Gtk.HScrollbar self; 
        public override object GtkControl => self;
        private Gtk.Adjustment adjustment = new Gtk.Adjustment(10, 0, 100, 1, 1, 0);
        public HScrollBar() : base()
        {
            var vbar = new Gtk.Scrollbar(Gtk.Orientation.Horizontal, adjustment);
            self = new Gtk.HScrollbar(adjustment);
            self.Realized += Control_Realized;
        }

        private void Control_Realized(object sender, EventArgs e)
        {
            adjustment.Lower = Minimum;
            adjustment.Upper = Maximum;
            adjustment.Value = 0;// Value;
            adjustment.ValueChanged += Control_ValueChanged;
            _oldValue = (int)adjustment.Value;
        }

        private int _oldValue;
        private void Control_ValueChanged(object sender, EventArgs e)
        {
            var ea = new ScrollEventArgs(Value > _oldValue ? ScrollEventType.SmallIncrement : ScrollEventType.SmallDecrement, _oldValue, Value);
            Scroll?.Invoke(this, ea);
            _oldValue = Value;
        }

        public int LargeChange { get; set; } = 5;
        public int Maximum { get; set; } = 100;
        public int Minimum { get; set; } = 0;
        public int Value { get => (int)adjustment.Value; set { adjustment.Value = value; } }
        //public System.Windows.Forms.Orientation Orientation { get; set; }
        //public int TickFrequency { get; set; }
        //public System.Windows.Forms.TickStyle TickStyle { get; set; }
        public event EventHandler<ScrollEventArgs> Scroll;
    }
}
