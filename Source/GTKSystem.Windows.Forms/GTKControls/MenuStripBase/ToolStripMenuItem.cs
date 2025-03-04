namespace System.Windows.Forms;

public class ToolStripMenuItem : WidgetToolStrip<Gtk.MenuItem>
{
    public ToolStripMenuItem():base("ToolStripMenuItem")
    {
        DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
    }

    public override bool Checked {
        get
        {
            if (flagBox.Child is Gtk.CheckButton checkbutton)
            {
                return checkbutton.Active;
            }

            if (flagBox.Child is Gtk.RadioButton radiobutton)
            {
                return radiobutton.Active;
            }
            return base.Checked; 
        }
        set { 
            base.Checked = value;
            if (flagBox.Child is Gtk.CheckButton checkbutton)
            {
                checkbutton.Active = value;
            }
            else if (flagBox.Child is Gtk.RadioButton radiobutton)
            {
                radiobutton.Active = value;
            }
        } }
}