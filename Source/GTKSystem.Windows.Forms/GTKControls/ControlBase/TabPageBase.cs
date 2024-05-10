using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TabPageBase : Gtk.Layout, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal TabPageBase() : base(new Gtk.Adjustment(IntPtr.Zero), new Gtk.Adjustment(IntPtr.Zero))
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("TabPage");
            this.BorderWidth = 0;
        }
        protected override void OnShown()
        {
            Override.OnAddClass();
            base.OnShown();
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnDrawnBackground(cr, rec);
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
