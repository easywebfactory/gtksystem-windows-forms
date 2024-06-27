using Gtk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ListBoxBase : ScrollableBoxBase
    {
        public Gtk.ListBox ListBox = new Gtk.ListBox();
        internal ListBoxBase() : base()
        {
            this.Override.AddClass("ListBox");
            ListBox.BorderWidth = 1;
            ListBox.MarginTop = 3;
            ListBox.MarginStart = 1;
            ListBox.MarginEnd = 1;
            ListBox.MarginBottom = 1;
            ListBox.Hexpand = true;
            ListBox.Vexpand = true;
            base.AutoScroll= true;
            base.Add(ListBox);
        }
    }
}
