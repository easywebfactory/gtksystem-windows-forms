namespace System.Windows.Forms;

public sealed class CheckBoxBase : Gtk.CheckButton, IControlGtk
{
    public IGtkControlOverride Override { get; set; }
        public CheckBoxBase() : base()
        {
            Override = new GtkControlOverride(this);
            Override.AddClass("CheckBox");
            Valign = Gtk.Align.Start;
            Halign = Gtk.Align.Start;
        }
        protected override void OnShown()
        {
            Override.OnAddClass();
            base.OnShown();
        }
    }
