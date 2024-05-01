using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class RadioButtonBase : Gtk.RadioButton, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal RadioButtonBase() : base(new Gtk.RadioButton("baseradio"))
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("RadioButton");
            //self.Override.AddClass("BackgroundTransparent");
        }
        internal RadioButtonBase(Gtk.RadioButton radio_group_member) : base(radio_group_member)
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("RadioButton");
            //self.Override.AddClass("BackgroundTransparent");
        }
        public void AddClass(string cssClass)
        {
            this.Override.AddClass(cssClass);
        }
        protected override void OnShown()
        {
            Override.OnAddClass();
            base.OnShown();
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = this.Allocation;
            Override.OnDrawnBackground(cr, rec);
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
