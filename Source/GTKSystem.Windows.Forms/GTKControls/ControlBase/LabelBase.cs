using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class LabelBase : Gtk.Label, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal LabelBase() : base()
        {
 
            //this.ModifyBg(StateType.Normal, new Gdk.Color(222, 0, 0));
            //this.ModifyBg(StateType.Insensitive, new Gdk.Color(0, 222, 0));

            Pango.AttrList attributes = new Pango.AttrList();

            Pango.AttrBackground attrBackground = new Pango.AttrBackground(Convert.ToUInt16(65535), Convert.ToUInt16(35535), Convert.ToUInt16(35535)){ StartIndex=10, EndIndex=30 };
            Pango.AttrForeground attrForeground = new Pango.AttrForeground(Convert.ToUInt16(0.9), Convert.ToUInt16(0.5), Convert.ToUInt16(0.9));
            attributes.Insert(attrForeground);
            attributes.Insert(attrBackground);
            attributes.Insert(new Pango.AttrUnderline(Pango.Underline.Low));
            //this.Attributes = attributes;

            
          
            this.Override = new GtkControlOverride(this);

            this.Override.AddClass("Label");
            //self.Override.AddClass("BackgroundTransparent");
            this.Xalign = 0.08f;
            this.Yalign = 0.08f;

        }

        internal LabelBase(string text) : base(text)
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("Label");
            //self.Override.AddClass("BackgroundTransparent");
            this.Xalign = 0.08f;
            this.Yalign = 0.08f;
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
