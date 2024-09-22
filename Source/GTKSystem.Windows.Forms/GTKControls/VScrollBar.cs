using System.ComponentModel;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public class VScrollBar : Control
    {
        public readonly Gtk.VScrollbar self;
        public override object GtkControl => self;
        private Gtk.Adjustment adjustment = new Gtk.Adjustment(10, 0, 100, 1, 1, 0);
        public VScrollBar() : base()
        {
            self = new Gtk.VScrollbar(adjustment);
            self.Realized += Control_Realized;
        }

        private int _oldValue;
        private void Control_Realized(object sender, EventArgs e)
        {
            adjustment.Lower = Minimum;
            adjustment.Upper = Maximum;
            adjustment.Value = 0;// Value;
            adjustment.ValueChanged += Control_ValueChanged;
            _oldValue = (int)adjustment.Value;
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            var ea = new ScrollEventArgs(Value>_oldValue? ScrollEventType.SmallIncrement: ScrollEventType.SmallDecrement, _oldValue, Value);
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
