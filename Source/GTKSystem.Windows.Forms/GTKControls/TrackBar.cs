/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;
using Pango;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using static System.Formats.Asn1.AsnWriter;

namespace System.Windows.Forms
{

	public class TrackBar : WidgetControl<Gtk.Viewport>
	{
		Gtk.Adjustment adjustment = new Gtk.Adjustment(10, 0, 100, 1, 1, 0);
        Gtk.Scale scale;
		public TrackBar():base()
		{
            base.Control.Realized += Control_Realized;
        }

        private void Control_Realized(object sender, EventArgs e)
        {
            scale = new Gtk.Scale(Orientation== Orientation.Horizontal ? Gtk.Orientation.Horizontal : Gtk.Orientation.Vertical, adjustment);
            scale.ShowFillLevel = true;
            scale.DrawValue = false;
            scale.RoundDigits = 1;
            scale.Visible = true;
            scale.Inverted = Orientation == Orientation.Vertical;
            adjustment.Lower = Minimum;
            adjustment.Upper = Maximum;
            adjustment.Value = Value;
            adjustment.ValueChanged += Control_ValueChanged;
            this.Control.Add(scale);
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            Value = (int)adjustment.Value;
            if (Scroll != null)
                Scroll(this, e);
        }

        public int LargeChange { get; set; } = 5;
        public int Maximum { get; set; }
        public int Minimum { get; set; }
        public int Value { get=> (int)adjustment.Value; set=> adjustment.Value = value; }
        public System.Windows.Forms.Orientation Orientation { get; set; }
        public int TickFrequency { get; set; }
        public System.Windows.Forms.TickStyle TickStyle { get; set; }
        public event EventHandler Scroll;
    }
}
