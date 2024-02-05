/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    [DefaultProperty("Value")]
	public class ProgressBar : WidgetControl<Gtk.LevelBar>
    {
        public ProgressBar():base()
        {
			base.Control.StyleContext.AddClass("ProgressBar");
            base.Control.Realized += Control_Realized;
        }

        private void Control_Realized(object sender, EventArgs e)
        {
            base.Control.MaxValue = Maximum;
            base.Control.MinValue = Minimum;
            base.Control.Value = Value;
        }

        public ProgressBarStyle Style { get; set; }
        [DefaultValue(100)]
		public int MarqueeAnimationSpeed { get; set; } = 100;
        [DefaultValue(100)]
		public int Maximum { get; set; } = 100;
        [DefaultValue(0)]
		public int Minimum { get; set; } = 0;
        public new Padding Padding { get; set; }
        [DefaultValue(10)]
        public int Step { get; set; }
        [DefaultValue(0)]
        public int Value { get => (int)base.Control.Value; set => base.Control.Value = value; }
        public void Increment(int value)
        {

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
