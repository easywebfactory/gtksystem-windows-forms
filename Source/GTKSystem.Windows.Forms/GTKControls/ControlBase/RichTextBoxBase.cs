using System.Drawing;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class RichTextBoxBase : ScrollableBoxBase
    {
        internal Gtk.TextView TextView = new Gtk.TextView();
        public RichTextBoxBase() : base()
        {
            this.Override.AddClass("RichTextBox");
            this.TextView.BorderWidth = 1;
            this.TextView.Margin = 0;
            this.TextView.WrapMode = Gtk.WrapMode.Char;
            this.TextView.Halign = Gtk.Align.Fill;
            this.TextView.Valign = Gtk.Align.Fill;
            this.TextView.Hexpand = true;
            this.TextView.Vexpand = true;
            this.AutoScroll = true;
            this.Add(TextView);
        }
    }
}
