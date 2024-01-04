/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class DateTimePicker : MaskedTextBox
    {
        public DateTimePicker() : base()
        {
            Widget.StyleContext.AddClass("DateTimePicker");
            Widget.StyleContext.AddClass("BorderRadiusStyle");
            base.Mask = "____年__月__日";
            //base.Control.PrimaryIconActivatable = true;
            //base.Control.PrimaryIconStock = "gtk-index";

            base.Control.SecondaryIconActivatable = true;
            //base.Control.SecondaryIconStock= "gtk-index";
            //base.Control.SecondaryIconName = "DateTimePicker.ico";

            //base.Control.SecondaryIconPixbuf = new Gdk.Pixbuf(WindowsFormsApp1.Properties.Resources.DateTimePicker);
            base.Control.SecondaryIconName = "x-office-calendar";
            base.Control.IconRelease += DateTimePicker_IconRelease;
        }

        private void DateTimePicker_IconRelease(object o, Gtk.IconReleaseArgs args)
        {
            //Console.Write("DateTimePicker_IconRelease");
            Gtk.Popover popver = new Gtk.Popover((Gtk.Widget)o);
            popver.StyleContext.AddClass("DateTimePicker");
            popver.StyleContext.AddClass("BorderRadiusStyle");
            popver.WidthRequest = 300;
            popver.HeightRequest = 200;
            popver.Modal = true;

            Gtk.Calendar calendar = new Gtk.Calendar();
            calendar.Halign = Gtk.Align.Fill;
            calendar.Valign = Gtk.Align.Fill;
            calendar.Date= DateTime.Now;
            calendar.DaySelected += Calendar_DaySelected;
            popver.Add(calendar);
            popver.ShowAll();
        }

        private void Calendar_DaySelected(object sender, EventArgs e)
        {
            Gtk.Calendar calendar = sender as Gtk.Calendar;
            DateTime dt = calendar.GetDate();

            base.Control.DeleteSelection();
            base.Mask = "";
            this.Text = dt.ToString("yyyy年MM月dd日");
            base.Mask = "____年__月__日";
            // Console.Write(dt.ToString("yyyy年MM月dd日"));
        }

        public DateTime MaxDate
        {
            get; set;
        }
        public DateTime MinDate
        {
            get; set;
        }

        public DateTime Value
        {
            get
            {
                string val = Text.Replace("_", "0").Replace("年", "-").Replace("月", "-").Replace("日", "-");
                if (DateTime.TryParse(val, out DateTime result))
                    return result;
                else
                    return DateTime.MinValue;
            }
            set { base.Text = value.ToString("yyyy年MM月dd日"); }
        }

        public Font CalendarFont { get; set; }
        public Color CalendarForeColor { get; set; }
        public Color CalendarMonthBackground { get; set; }
        public Color CalendarTitleBackColor { get; set; }
        public Color CalendarTitleForeColor { get; set; }
        public Color CalendarTrailingForeColor { get; set; }

        public event EventHandler ValueChanged
        {
            add { base.Control.Changed += (object sender, EventArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
            remove { base.Control.Changed -= (object sender, EventArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
        }
    }
}
