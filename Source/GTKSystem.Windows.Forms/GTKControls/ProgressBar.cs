/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    [DefaultProperty("Value")]
	public class ProgressBar : Control
    {
        private ProgressBarBase self = new ProgressBarBase();
        public override object GtkControl => self;
        public ProgressBar():base()
        {
            self.Override.sender = this;
            self.MaxValue = 100;
            self.MinValue = 0;
        }
        public ProgressBarStyle Style { get; set; }
        [DefaultValue(100)]
		public int MarqueeAnimationSpeed { get; set; } = 100;
        [DefaultValue(100)]
		public int Maximum { get => (int)self.MaxValue; set { self.MaxValue = value; } }
        [DefaultValue(0)]
		public int Minimum { get => (int)self.MinValue; set { self.MinValue = value; } }
        public new Padding Padding { get; set; }
        [DefaultValue(10)]
        public int Step { get; set; }
        [DefaultValue(0)]
        public int Value { get => (int)self.Value; set => self.Value = value; }
        public void Increment(int value)
        {
            self.Value = Math.Max(Minimum, Math.Min(Maximum, self.Value + value));

        }
 
        public void PerformStep()
		{
	
		}
	}
}
