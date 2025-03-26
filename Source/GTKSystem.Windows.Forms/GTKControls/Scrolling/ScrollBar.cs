using System.ComponentModel;

namespace System.Windows.Forms;

[DefaultProperty(nameof(Value))]
[DefaultEvent(nameof(Scroll))]
public abstract class ScrollBar : Control
{
    public abstract Gtk.Adjustment Adjustment {  get; }
    public ScrollBar()
    {
        Adjustment.ValueChanged += Control_ValueChanged;
    }
    double _oldValue;
    private void Control_ValueChanged(object? sender, EventArgs e)
    {
        var newValue = Adjustment.Value;
        OnValueChanged(e);
        var args = new ScrollEventArgs(newValue >= _oldValue ? ScrollEventType.SmallIncrement : ScrollEventType.SmallDecrement, (int)_oldValue, (int)newValue);
        OnScroll(args);
        _oldValue = newValue;
    }

    protected virtual void OnScroll(ScrollEventArgs e)
    {
        Scroll?.Invoke(this, e);
    }

    protected virtual void OnValueChanged(EventArgs e)
    {
        ValueChanged?.Invoke(this, e);
    }

    public int SmallChange { get => (int)Adjustment.StepIncrement; set => Adjustment.StepIncrement = value; }
    public int LargeChange { get => (int)Adjustment.PageIncrement; set => Adjustment.PageIncrement = value; }
    public int Maximum { get => (int)Adjustment.Upper; set => Adjustment.Upper = value; }
    public int Minimum { get => (int)Adjustment.Lower; set => Adjustment.Lower = value; }
    public int Value { get => (int)Adjustment.Value; set => Adjustment.Value = value;
    }
    public event ScrollEventHandler? Scroll;
    public event EventHandler? ValueChanged;
}