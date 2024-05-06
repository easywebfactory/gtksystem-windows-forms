using Cairo;
using Gtk;
using System;
using System.Drawing.Drawing2D;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class FormBase : Gtk.Dialog, IControlGtk
    {
        public readonly Gtk.ScrolledWindow ScrollArea = new Gtk.ScrolledWindow();
        public GtkControlOverride Override { get; set; }
        public FormBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("Form");
            this.WindowPosition = Gtk.WindowPosition.Center;
            this.BorderWidth = 0;
            this.SetDefaultSize(100, 100);
            this.TypeHint = Gdk.WindowTypeHint.Normal;
            this.Response += FormBase_Response;
            ScrollArea.BorderWidth = 0;
            ScrollArea.Valign = Gtk.Align.Fill;
            ScrollArea.Halign = Gtk.Align.Fill;
            ScrollArea.HscrollbarPolicy = PolicyType.Always;
            ScrollArea.VscrollbarPolicy = PolicyType.Always;
            this.ContentArea.PackStart(ScrollArea, true, true, 0);

            //this.ContentArea.PackEnd(new Gtk.Button("ddddddd"), false, true, 0);
            // this.Decorated = false; //删除工具栏
        }

        public FormBase(string title, Gtk.Window parent, DialogFlags flags, params object[] button_data) : base(title, parent, flags, button_data)
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("Form");
            this.WindowPosition = Gtk.WindowPosition.Center;
            this.BorderWidth = 0;
            this.SetDefaultSize(100, 100);
            this.TypeHint = Gdk.WindowTypeHint.Normal;
            this.Response += FormBase_Response;
        }
        private void FormBase_Response(object o, ResponseArgs args)
        {
            //Console.WriteLine(args.ResponseId);
            if (args.ResponseId == ResponseType.DeleteEvent)
                if (this.IsActive)
                    this.OnClose();
        }

        public void CloseWindow()
        {
            this.OnClose();
        }
    }
}
