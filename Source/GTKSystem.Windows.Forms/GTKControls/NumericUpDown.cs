/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class NumericUpDown : WidgetControl<Gtk.SpinButton>
    {
       
        public NumericUpDown() : base(0, 100, 1)
        {
            Widget.StyleContext.AddClass("NumericUpDown");
            Widget.StyleContext.AddClass("BorderRadiusStyle");
            base.Control.Value = 0;
            base.Control.Orientation = Gtk.Orientation.Horizontal;
        }

        public event EventHandler ValueChanged
        {
            add { base.Control.ValueChanged += (object sender, EventArgs e) => { value.Invoke(this, e); }; }
            remove { base.Control.ValueChanged -= (object sender, EventArgs e) => { value.Invoke(this, e); }; }
        }

        public int DecimalPlaces { get => Convert.ToInt32(base.Control.Digits); set => base.Control.Digits = Convert.ToUInt32(value); }
        public decimal Increment
        {
            get => Convert.ToDecimal(base.Control.Adjustment.StepIncrement);
            set => base.Control.Adjustment.StepIncrement = Convert.ToDouble(value);
        }
        public decimal Maximum { get => Convert.ToDecimal(base.Control.Adjustment.Upper); set => base.Control.Adjustment.Upper = Convert.ToDouble(value); }

        public decimal Minimum { get => Convert.ToDecimal(base.Control.Adjustment.Lower); set => base.Control.Adjustment.Lower = Convert.ToDouble(value); }
        public decimal Value { get { return Convert.ToDecimal(base.Control.Value); } set { base.Control.Value = Convert.ToDouble(value); } }
    }
}
