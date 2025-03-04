/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public class NumericUpDown : Control
{
    public readonly NumericUpDownBase self = new();
    public override object GtkControl => self;
    public NumericUpDown()
    {
        self.ValueChanged += Self_ValueChanged;
    }

    private void Self_ValueChanged(object? sender, EventArgs e)
    {
        if (ValueChanged != null && self.IsVisible)
            ValueChanged?.Invoke(this, e);
    }

    public event EventHandler? ValueChanged;
    public int DecimalPlaces { get => Convert.ToInt32(self.Digits); set => self.Digits = Convert.ToUInt32(value); }
    public decimal Increment
    {
        get => Convert.ToDecimal(self.Adjustment.StepIncrement);
        set => self.Adjustment.StepIncrement = Convert.ToDouble(value);
    }
    public decimal Maximum { get => Convert.ToDecimal(self.Adjustment.Upper); set => self.Adjustment.Upper = Convert.ToDouble(value); }

    public decimal Minimum { get => Convert.ToDecimal(self.Adjustment.Lower); set => self.Adjustment.Lower = Convert.ToDouble(value); }
    public decimal Value { get => Convert.ToDecimal(self.Value);
        set => self.Value = Convert.ToDouble(value);
    }
}