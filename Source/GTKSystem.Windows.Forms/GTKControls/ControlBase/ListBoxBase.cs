using Gtk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ListBoxBase : ScrollableBoxBase
    {
        public Gtk.ListBox ListBox = new Gtk.ListBox();
        public ListBoxBase() : base()
        {
            this.Override.AddClass("ListBox");
            ListBox.BorderWidth = 1;
            ListBox.Margin = 0;
            ListBox.Hexpand = true;
            ListBox.Vexpand = true;
            base.AutoScroll= true;
            base.Add(ListBox);
        }
    }
}
