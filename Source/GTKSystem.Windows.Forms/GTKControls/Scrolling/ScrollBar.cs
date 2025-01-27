using System.ComponentModel;

namespace System.Windows.Forms
{
    [DefaultProperty(nameof(Value))]
    [DefaultEvent(nameof(Scroll))]
    public abstract partial class ScrollBar : Control
    {
        public virtual Gtk.Adjustment Adjustment {  get; }
        public ScrollBar() : base()
        {
            Adjustment.ValueChanged += Control_ValueChanged;
        }
        double _oldValue = 0;
        private void Control_ValueChanged(object sender, EventArgs e)
        {
            double newValue = Adjustment.Value;
            ValueChanged?.Invoke(this, e);
            var args = new ScrollEventArgs(newValue >= _oldValue ? ScrollEventType.SmallIncrement : ScrollEventType.SmallDecrement, (int)_oldValue, (int)newValue);
            Scroll?.Invoke(this, args);
            _oldValue = newValue;
        }
        public int SmallChange { get => (int)Adjustment.StepIncrement; set => Adjustment.StepIncrement = value; }
        public int LargeChange { get => (int)Adjustment.PageIncrement; set => Adjustment.PageIncrement = value; }
        public int Maximum { get => (int)Adjustment.Upper; set => Adjustment.Upper = value; }
        public int Minimum { get => (int)Adjustment.Lower; set => Adjustment.Lower = value; }
        public int Value { get => (int)Adjustment.Value; set { Adjustment.Value = value; } }
        public event ScrollEventHandler Scroll;
        public event EventHandler ValueChanged;
    }
}
