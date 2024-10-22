using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class GroupBoxBase : Gtk.Frame, IControlGtk, IScrollableBoxBase
    {
        public GtkControlOverride Override { get; set; }
        public bool AutoScroll { get; set; }
        public bool HScroll { get; set; } = false;
        public bool VScroll { get; set; } = false;

        public GroupBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("GroupBox");
            this.LabelXalign = 0.03f;
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
        }

        public event System.Windows.Forms.ScrollEventHandler Scroll;

        protected override void OnShown()
        {
            Override.OnAddClass();
            base.OnShown();
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }

        public void AddClass(string cssClass)
        {
            this.Override.AddClass(cssClass);
        }

        public void Pack(Widget child, Align align, bool expand)
        {
            child.Valign = align;
            child.Halign = align;
            child.Expand = expand;
            base.Add(child);
        }
    }
}
