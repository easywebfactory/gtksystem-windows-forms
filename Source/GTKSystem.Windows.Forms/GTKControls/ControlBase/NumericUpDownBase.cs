using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class NumericUpDownBase : Gtk.SpinButton, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal NumericUpDownBase() : base(0, 100, 1)
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("NumericUpDown");
            //self.Override.AddClass("BackgroundTransparent");
            this.Value = 0;
            this.Orientation = Gtk.Orientation.Horizontal;
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
