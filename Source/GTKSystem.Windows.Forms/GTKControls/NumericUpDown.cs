/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class NumericUpDown : Control
    {
        public readonly NumericUpDownBase self = new NumericUpDownBase();
        public override object GtkControl => self;
        public NumericUpDown() : base()
        {
            self.ValueChanged += Self_ValueChanged;
        }

        private void Self_ValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null && self.IsVisible)
                ValueChanged(this, e);
        }

        public event EventHandler ValueChanged;
        public int DecimalPlaces { get => Convert.ToInt32(self.Digits); set => self.Digits = Convert.ToUInt32(value); }
        public decimal Increment
        {
            get => Convert.ToDecimal(self.Adjustment.StepIncrement);
            set => self.Adjustment.StepIncrement = Convert.ToDouble(value);
        }
        public decimal Maximum { get => Convert.ToDecimal(self.Adjustment.Upper); set => self.Adjustment.Upper = Convert.ToDouble(value); }

        public decimal Minimum { get => Convert.ToDecimal(self.Adjustment.Lower); set => self.Adjustment.Lower = Convert.ToDouble(value); }
        public decimal Value { get { return Convert.ToDecimal(self.Value); } set { self.Value = Convert.ToDouble(value); } }
    }
}
