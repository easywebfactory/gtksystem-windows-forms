/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public class MonthCalendar : Control
{
    public readonly MonthCalendarBase self = new();
    public override object GtkControl => self;
    public MonthCalendar()
    {
        self.DaySelected += MonthCalendar_DaySelected;
    }

    private void MonthCalendar_DaySelected(object? sender, EventArgs e)
    {
        if (DateChanged != null && self.IsVisible)
            OnDateChanged(new DateRangeEventArgs(SelectionRange.Start, SelectionRange.End));

        if (DateSelected != null && self.IsVisible)
            OnDateSelected(new DateRangeEventArgs(SelectionRange.Start, SelectionRange.End));
    }

    protected virtual void OnDateSelected(DateRangeEventArgs e)
    {
        DateSelected?.Invoke(this, e);
    }

    protected virtual void OnDateChanged(DateRangeEventArgs e)
    {
        DateChanged?.Invoke(this, e);
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
    public SelectionRange SelectionRange { get; set; } = new();
    public DateTime TodayDate
    {
        get => self.Date;
        set => self.Date = value;
    }
    public bool ShowWeekNumbers { get; set; }
    public event DateRangeEventHandler? DateChanged;

    public event DateRangeEventHandler? DateSelected;

}