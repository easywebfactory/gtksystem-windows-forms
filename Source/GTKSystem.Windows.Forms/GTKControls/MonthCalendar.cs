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
    public partial class MonthCalendar : Control
    {
        public readonly MonthCalendarBase self = new MonthCalendarBase();
        public override object GtkControl => self;
        public MonthCalendar():base()
        {
            self.DaySelected += MonthCalendar_DaySelected;
        }

        private void MonthCalendar_DaySelected(object sender, EventArgs e)
        {
            if (DateChanged != null && self.IsVisible)
                DateChanged(this, new DateRangeEventArgs(SelectionRange.Start, SelectionRange.End));

            if (DateSelected != null && self.IsVisible)
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
            get { return self.Date; }
            set { self.Date = value; }
        }
        public bool ShowWeekNumbers { get; set; }
        public event DateRangeEventHandler DateChanged;

        public event DateRangeEventHandler DateSelected;

    }
}
