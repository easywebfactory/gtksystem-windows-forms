namespace System.Windows.Forms
{
    public interface IToolMenuItem
    {
        Gtk.Widget ToolItem { get; }
        Gtk.Widget MenuItem { get; }
        bool Checked { get; set; }
        CheckState CheckState { get; set; }
        ToolStripItemDisplayStyle DisplayStyle { get; set; }
        Gdk.Pixbuf Image { get; set; }
        string Name { get; set; }
        string Text { get; set; }
        void ShowAll();
    }

    public class StripMenuItem : Gtk.MenuItem, IToolMenuItem
    {
        public StripMenuItem()
        {
            base.Visible = true;
        }
        public Gtk.Widget ToolItem { get; }
        public Gtk.Widget MenuItem { get => this; }
        public string Text { get => base.Label; set => base.Label = value; }
        public ToolStripItemDisplayStyle DisplayStyle { get; set; }
        public bool Checked { get; set; }
        public CheckState CheckState { get; set; }
        public Gdk.Pixbuf Image { get; set; }
    }
    public class StripToolButton : Gtk.ToolButton, IToolMenuItem
    {
        public StripToolButton() : base(null, "") { }
        public Gtk.Widget ToolItem { get => this; }
        public Gtk.Widget MenuItem { get; }
        public string Text { get => base.Label; set => base.Label = value; }
        public ToolStripItemDisplayStyle DisplayStyle { get; set; }
        public bool Checked { get; set; }
        public CheckState CheckState { get; set; }
        public Gdk.Pixbuf Image { get; set; }
    }
    public class StripMenuToolButton : Gtk.MenuToolButton, IToolMenuItem
    {
        public StripMenuToolButton() : base(null, "") { }
        public Gtk.Widget ToolItem { get => this; }
        public Gtk.Widget MenuItem { get; }
        public string Text { get => base.Label; set => base.Label = value; }
        public ToolStripItemDisplayStyle DisplayStyle { get; set; }
        public bool Checked { get; set; }
        public CheckState CheckState { get; set; }
        public Gdk.Pixbuf Image { get; set; }
    }
    public class StripToolItem : Gtk.ToolItem, IToolMenuItem
    {
        public StripToolItem() : base() {  }
        public Gtk.Widget ToolItem { get => this; }
        public Gtk.Widget MenuItem { get; }
        public string Text { get; set; }
        public ToolStripItemDisplayStyle DisplayStyle { get; set; }
        public bool Checked { get; set; }
        public CheckState CheckState { get; set; }
        public Gdk.Pixbuf Image { get; set; }
    }
    public class StripSeparator : IToolMenuItem
    {
        public Gtk.SeparatorToolItem toolItem = null;
        public Gtk.SeparatorMenuItem menuItem = null;
        public StripSeparator() : base() { }
        public Gtk.Widget ToolItem
        {
            get
            {
                if (toolItem == null)
                    toolItem = new Gtk.SeparatorToolItem() { Visible = true, BorderWidth = 1 };
                return toolItem;
            }
        }
        public Gtk.Widget MenuItem
        {
            get
            {
                if (menuItem == null)
                    menuItem = new Gtk.SeparatorMenuItem() { Visible = true, BorderWidth = 1 };
                return menuItem;
            }
        }
        public string Name { get; set; }
        public string Text { get; set; }
        public ToolStripItemDisplayStyle DisplayStyle { get; set; }
        public bool Checked { get; set; }
        public CheckState CheckState { get; set; }
        public Gdk.Pixbuf Image { get; set; }
        public void ShowAll()
        {
            toolItem?.ShowAll();
            menuItem?.ShowAll();
        }
    }
    public class StripDropDown : Gtk.Menu, IToolMenuItem
    {
        public StripDropDown() { }
        public Gtk.Widget ToolItem { get; }
        public Gtk.Widget MenuItem { get => this; }
        public string Text { get; set; }
        public ToolStripItemDisplayStyle DisplayStyle { get; set; }
        public bool Checked { get; set; }
        public CheckState CheckState { get; set; }
        public Gdk.Pixbuf Image { get; set; }
    }
}
