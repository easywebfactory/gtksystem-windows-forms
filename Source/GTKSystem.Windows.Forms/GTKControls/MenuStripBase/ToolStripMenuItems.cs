using Gtk;

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
        public Gtk.CheckButton checkButton = new Gtk.CheckButton() { Visible = true, Relief = ReliefStyle.None };
        public StripMenuItem() : base()
        {
            base.Visible = true;
            checkButton.ImagePosition = PositionType.Left;
            checkButton.Halign = Align.Start;
            checkButton.Expand = true;
            checkButton.Hexpand = true;
            checkButton.Xalign = 0f;
            checkButton.DrawIndicator = false;
            base.Add(checkButton);
        }
        public Gtk.Widget ToolItem { get; }
        public Gtk.Widget MenuItem { get => this; }
        public string Text { get => checkButton.Label; set => checkButton.Label = value; }
        public ToolStripItemDisplayStyle DisplayStyle { get; set; }
        public bool Checked { get => checkButton.DrawIndicator; set => checkButton.DrawIndicator = value; }
        public CheckState CheckState { get => checkButton.Active == true ? CheckState.Checked : CheckState.Unchecked; set => checkButton.Active = value == CheckState.Checked; }
        private Gdk.Pixbuf _Image;
        public Gdk.Pixbuf Image
        {
            get => _Image; set
            {
                _Image = value;
                if (value == null)
                {
                    checkButton.DrawIndicator = false;
                    checkButton.AlwaysShowImage = false;
                }
                else
                {
                    checkButton.DrawIndicator = false;
                    checkButton.AlwaysShowImage = true;
                    checkButton.Image = new Gtk.Image(value);
                }
            }
        }
        protected override void Dispose(bool disposing)
        {
            _Image?.Dispose();
            checkButton.Dispose();
            base.Dispose(disposing);
        }
    }
    public class StripToolButton : Gtk.ToolButton, IToolMenuItem
    {
        public StripToolButton() : base(null, "") { this.Homogeneous = false; }
        public Gtk.Widget ToolItem { get => this; }
        public Gtk.Widget MenuItem { get; }
        public string Text { get => base.Label; set => base.Label = value; }
        public ToolStripItemDisplayStyle DisplayStyle { get; set; }
        public bool Checked { get; set; }
        public CheckState CheckState { get; set; }
        private Gdk.Pixbuf _image;
        public Gdk.Pixbuf Image
        {
            get => _image;
            set
            {
                _image = value;
                this.IconWidget = new Gtk.Image(value) { Visible = true };
            }
        }
    }
    public class StripMenuToolButton : Gtk.MenuToolButton, IToolMenuItem
    {
        public StripMenuToolButton() : base(null, "") { this.Homogeneous = false; }
        public Gtk.Widget ToolItem { get => this; }
        public Gtk.Widget MenuItem { get; }
        public string Text { get => base.Label; set => base.Label = value; }
        public ToolStripItemDisplayStyle DisplayStyle { get; set; }
        public bool Checked { get; set; }
        public CheckState CheckState { get; set; }
        private Gdk.Pixbuf _image;
        public Gdk.Pixbuf Image
        {
            get => _image;
            set
            {
                _image = value;
                this.IconWidget = new Gtk.Image(value) { Visible = true };
            }
        }
    }
    public class StripToolItem : Gtk.ToolItem, IToolMenuItem
    {
        public StripToolItem() : base() { this.Homogeneous = false; }
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
