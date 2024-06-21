using Gtk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ListBoxBase : ScrollableBoxBase
    {
        public Gtk.ListBox ListBox = new Gtk.ListBox();
        internal ListBoxBase() : base()
        {
            this.Override.AddClass("ListBox");
            ListBox.BorderWidth = 0;
            base.AutoScroll= true;
            base.Add(ListBox);
        }
    }
}
