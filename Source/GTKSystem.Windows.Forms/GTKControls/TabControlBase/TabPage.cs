using Gtk;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace System.Windows.Forms
{
    public class TabPage : WidgetControl<Gtk.Layout>
    {
        private ControlCollection _controls;
        public TabPage() : base(new Gtk.Adjustment(IntPtr.Zero), new Gtk.Adjustment(IntPtr.Zero))
        {
            Widget.StyleContext.AddClass("TabPage");
            Control.BorderWidth = 1;
            _controls = new ControlCollection(this.Control);

            Widget.Data["Dock"] = DockStyle.Fill;
        }

        public TabPage(string text)
        {
            _TabLabel.Text = text;
            _controls = new ControlCollection(this.Control);
        }

        public override Point Location
        {
            get
            {
                return new Point(Widget.MarginStart, Widget.MarginTop);
            }
            set
            {
                Widget.MarginStart = 0;
                Widget.MarginTop = 0;
                Widget.Data["InitMarginStart"] = 0;
                Widget.Data["InitMarginTop"] = 0;
            }
        }
        public new DockStyle Dock
        {
            get
            {
                return DockStyle.Fill;
            }
            set { Widget.Data["Dock"] = DockStyle.Fill; }
        }
        public override string Text { get { return _TabLabel.Text; } set { _TabLabel.Text = value; } }

        private Gtk.Label _TabLabel = new Gtk.Label();
        public Gtk.Label TabLabel { get { return _TabLabel; } }

        public new ControlCollection Controls => _controls;

        public int ImageIndex { get; set; }
        public string ImageKey { get; set; }
        public List<object> ImageList { get; set; }
    }
}
