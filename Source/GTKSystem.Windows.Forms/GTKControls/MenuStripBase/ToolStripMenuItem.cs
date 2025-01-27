namespace System.Windows.Forms
{
    public class ToolStripMenuItem : WidgetToolStrip<Gtk.MenuItem>
    {
        public ToolStripMenuItem():base("ToolStripMenuItem")
        {
            DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
        }

        public override CheckState CheckState { 
            get => base.CheckState;
            set
            {
                base.CheckState = value;
            }
        }
        public override bool Checked {
            get
            {
                if (this.flagBox.Child is Gtk.CheckButton checkbutton)
                {
                   return checkbutton.Active;
                }
                else if (this.flagBox.Child is Gtk.RadioButton radiobutton)
                {
                    return radiobutton.Active;
                }
                return base.Checked; 
            }
            set { 
                base.Checked = value;
                if (this.flagBox.Child is Gtk.CheckButton checkbutton)
                {
                    checkbutton.Active = value;
                }
                else if (this.flagBox.Child is Gtk.RadioButton radiobutton)
                {
                    radiobutton.Active = value;
                }
            } }
    }
}
