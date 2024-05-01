using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class PanelBase: Gtk.Viewport, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal PanelBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("Panel");
            this.MarginStart = 0;
            this.MarginTop = 0;
            this.ShadowType = Gtk.ShadowType.In;
            this.BorderWidth = 0;
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
