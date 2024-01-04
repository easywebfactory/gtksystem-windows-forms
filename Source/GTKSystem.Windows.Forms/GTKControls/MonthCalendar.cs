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
    public partial class MonthCalendar : WidgetControl<Gtk.Calendar>
    {
        public MonthCalendar():base()
        {
            Widget.StyleContext.AddClass("MonthCalendar");
            Widget.StyleContext.AddClass("BorderRadiusStyle");
            base.Control.DaySelected += MonthCalendar_DaySelected;
        }

        private void MonthCalendar_DaySelected(object sender, EventArgs e)
        {
            if (DateChanged != null)
                DateChanged(this, new DateRangeEventArgs(SelectionRange.Start, SelectionRange.End));

            if (DateSelected != null)
                DateSelected(this, new DateRangeEventArgs(SelectionRange.Start, SelectionRange.End));
        }
      
   
        public Day FirstDayOfWeek
        {
            get;set;
        }
        public DateTime MaxDate
        {
            get;set;
        }
        public DateTime MinDate
        {
            get; set;
        }

        public bool ShowToday { get; set; }
        public bool ShowTodayCircle { get; set; }
        public SelectionRange SelectionRange { get; set; }
        public DateTime TodayDate
        {
            get { return base.Control.Date; }
            set { base.Control.Date = value; }
        }
        public bool ShowWeekNumbers { get; set; }
        public event DateRangeEventHandler DateChanged;

        public event DateRangeEventHandler DateSelected;

    }
}
