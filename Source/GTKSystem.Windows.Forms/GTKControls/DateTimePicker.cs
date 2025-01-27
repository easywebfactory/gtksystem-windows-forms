/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class DateTimePicker : MaskedTextBox
    {
        Gtk.Popover popver;
        Gtk.Calendar calendar = new Gtk.Calendar();
        public DateTimePicker() : base("DateTimePicker")
        {
            base.Mask = Gtk.Windows.Forms.Properties.Resources.DateTimePicker_DateTimePicker_Mask;

            self.SecondaryIconActivatable = true;
            self.SecondaryIconStock= "open-menu";
            self.SecondaryIconPixbuf = new Gdk.Pixbuf(this.GetType().Assembly, "GTKSystem.Windows.Forms.Resources.System.MonthCalendar.ico");
            self.IconRelease += DateTimePicker_IconRelease;
            self.Shown += Self_Shown;

            popver = new Gtk.Popover(self);
            popver.BorderWidth = 5;
            popver.Modal = true;
            popver.Position = PositionType.Bottom;

            calendar.Halign = Gtk.Align.Fill;
            calendar.Valign = Gtk.Align.Fill;
            calendar.DaySelected += Calendar_DaySelected;
            calendar.PrevYear += Calendar_PrevYear;
            calendar.NextYear += Calendar_NextYear;
            calendar.PrevMonth += Calendar_PrevMonth;
            calendar.NextMonth += Calendar_NextMonth;

            Gtk.Box popbody=new Gtk.Box(Gtk.Orientation.Vertical, 6);
            popbody.Add(calendar);
            Gtk.Button todaybtn = new Gtk.Button() { Label = Gtk.Windows.Forms.Properties.Resources.DateTimePicker_DateTimePicker_Choose_Today +DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") };
            todaybtn.Clicked += Todaybtn_Clicked;
            popbody.Add(todaybtn);
            popver.Add(popbody);
            self.Changed += Self_Changed;
        }
        private void Self_Changed(object sender, EventArgs e)
        {
            if (ValueChanged != null && self.IsMapped)
                ValueChanged(this, e);
        }

        private void Calendar_NextMonth(object sender, EventArgs e)
        {
            if (calendar.GetDate() > MaxDate)
            {
                calendar.Date = MaxDate.Date.AddDays(-1);
            }
        }

        private void Calendar_PrevMonth(object sender, EventArgs e)
        {
            if (calendar.GetDate() < MinDate)
            {
                calendar.Date = MinDate.Date.AddDays(1);
            }
        }

        private void Calendar_NextYear(object sender, EventArgs e)
        {
            if (calendar.GetDate() > MaxDate)
            {
                calendar.Date = MaxDate.Date.AddDays(-1);
            }
        }

        private void Calendar_PrevYear(object sender, EventArgs e)
        {
            if (calendar.GetDate() < MinDate)
            {
                calendar.Date = MinDate.Date.AddDays(1);
            }
        }

        private void Todaybtn_Clicked(object sender, EventArgs e)
        {
            this.Value = DateTime.Now;
            popver.Hide();
        }

        private void Self_Shown(object sender, EventArgs e)
        {
            this.Value = DateTime.Now;
        }

        private void DateTimePicker_IconRelease(object o, Gtk.IconReleaseArgs args)
        {
            DateTime current = Value;
            if (current >= MaxDate)
            {
                calendar.Date = MaxDate.Date.AddDays(-1);
            }
            else if(current <= MinDate)
            {
                calendar.Date = MinDate.AddDays(1);
            }
            else
            {
                calendar.Date = Value;
            }
            popver.ShowAll();
        }

        private void Calendar_DaySelected(object sender, EventArgs e)
        {
            Gtk.Calendar calendar = sender as Gtk.Calendar;
            DateTime dt = calendar.GetDate();
            if (dt > MaxDate || dt < MinDate)
            {
                calendar.Date = Value;
                MessageBox.Show(
                    string.Format(Gtk.Windows.Forms.Properties.Resources.DateTimePicker_Calendar_DaySelected_Choose, MaxDate.ToString("yyyy/MM/dd HH:mm:ss"),
                        MinDate.ToString("yyyy/MM/dd HH:mm:ss")), Gtk.Windows.Forms.Properties.Resources.DateTimePicker_Calendar_DaySelected_Date_restrictions);
            }
            else
            {
                DateTime current = Value;
                this.Value = dt.AddHours(current.Hour).AddMinutes(current.Minute).AddSeconds(current.Second);
            }
        }

        public DateTime MaxDate { get; set; } = DateTime.MaxValue;
        public DateTime MinDate { get; set; } = DateTime.MinValue;
        public DateTime Value
        {
            get
            {
                DateTime result = DateTime.Now;
                CultureInfo provider = CultureInfo.InvariantCulture;
                if (string.IsNullOrWhiteSpace(Text))
                { }
                else if (Format == DateTimePickerFormat.Custom && DateTime.TryParseExact(Text, CustomFormat, provider, Globalization.DateTimeStyles.AllowWhiteSpaces, out result))
                { }
                else if (DateTime.TryParse(Text, provider, Globalization.DateTimeStyles.AllowWhiteSpaces, out result))
                { }
                else
                { 
                    result = DateTime.Now;
                }
                if (result > MaxDate)
                {
                    return MaxDate;
                }
                if (result < MinDate)
                {
                    return MinDate;
                }
                return result;
            }
            set { 
                if(value > MaxDate)
                {
                    value = MaxDate.AddDays(-1);
                }
                if (value < MinDate)
                {
                    value = MinDate.AddDays(1);
                }
                if (Format==DateTimePickerFormat.Long)
                    base.Text = value.ToString(Gtk.Windows.Forms.Properties.Resources.DateTimePicker_Value_yyyy年MM月dd日);
                else if (Format == DateTimePickerFormat.Short)
                    base.Text = value.ToString("yyyy/MM/dd");
                else if (Format == DateTimePickerFormat.Time)
                    base.Text = value.ToString("HH:mm:ss");
                else if (Format == DateTimePickerFormat.Custom)
                    base.Text = value.ToString(CustomFormat);
                else
                    base.Text = value.ToString(Gtk.Windows.Forms.Properties.Resources.DateTimePicker_Value_yyyy年MM月dd日); 
            }
        }
        private string _CustomFormat= Gtk.Windows.Forms.Properties.Resources.DateTimePicker_Value_yyyy年MM月dd日;
        public string CustomFormat { get => _CustomFormat; 
            set { _CustomFormat = value; 
                base.Mask = Regex.Replace(value,"[ymdhs]","_",RegexOptions.IgnoreCase); 
            }
        }
        public DateTimePickerFormat _Format;
        public DateTimePickerFormat Format { get => _Format;
            set {
                _Format = value;
                if (Format == DateTimePickerFormat.Long)
                    base.Mask = Gtk.Windows.Forms.Properties.Resources.DateTimePicker_DateTimePicker_Mask;
                else if (Format == DateTimePickerFormat.Short)
                    base.Mask = "____/__/__";
                else if (Format == DateTimePickerFormat.Time)
                    base.Mask = "__:__:__";
                else if (Format == DateTimePickerFormat.Custom)
                    base.Mask = Regex.Replace(CustomFormat, "[ymdhs]", "_", RegexOptions.IgnoreCase);
                else
                    base.Mask = Gtk.Windows.Forms.Properties.Resources.DateTimePicker_DateTimePicker_Mask;
            } 
        }
        public Font CalendarFont { get; set; }
        public Color CalendarForeColor { get; set; }
        public Color CalendarMonthBackground { get; set; }
        public Color CalendarTitleBackColor { get; set; }
        public Color CalendarTitleForeColor { get; set; }
        public Color CalendarTrailingForeColor { get; set; }

        public event EventHandler ValueChanged;
    }
}
