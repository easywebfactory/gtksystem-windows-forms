using System.Drawing;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class RichTextBoxBase1 : Gtk.TextView, IControlGtk
    {
        public readonly ViewportBase ContentView = new ViewportBase();
        internal Gtk.ScrolledWindow ScrolledWindow = new Gtk.ScrolledWindow();
        internal Gtk.TextView TextView { get => this; }
        public GtkControlOverride Override { get; set; }
        internal RichTextBoxBase1() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("RichTextBox");

            ScrolledWindow.Child = this;
            ContentView.Child = ScrolledWindow;
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
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnDrawnBackground(cr, rec);
            Override.OnPaint(cr, rec);
            base.OnDrawn(cr);
            base.StyleContext.RemoveClass("forestyle");
            return true;

            //return base.OnDrawn(cr);

        }
    }

    public sealed class RichTextBoxBase : Gtk.Viewport, IControlGtk
    {
        internal Gtk.TextView TextView = new Gtk.TextView();
        public GtkControlOverride Override { get; set; }
        internal RichTextBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("RichTextBox");
            this.TextView.BorderWidth = 1;
            this.TextView.WrapMode = Gtk.WrapMode.Char;
            this.TextView.Halign = Gtk.Align.Fill;
            this.TextView.Valign = Gtk.Align.Fill;
            this.TextView.Hexpand = true;
            this.TextView.Vexpand = true;
            Gtk.ScrolledWindow scrolledWindow = new Gtk.ScrolledWindow();
            scrolledWindow.Child = this.TextView;
            this.Child = scrolledWindow;
        }
        public void AddClass(string cssClass)
        {
            this.Override.AddClass(cssClass);
        }
        protected override void OnShown()
        {
            if (Override.BackgroundImage != null || Override.Image != null)
            {
                if (Override.BackColor.HasValue == false)
                    Override.BackColor = Color.White;
            }
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
