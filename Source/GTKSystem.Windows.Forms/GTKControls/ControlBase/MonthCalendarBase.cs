namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class MonthCalendarBase : Gtk.Calendar, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public MonthCalendarBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("MonthCalendar");
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
        }
    }
}
