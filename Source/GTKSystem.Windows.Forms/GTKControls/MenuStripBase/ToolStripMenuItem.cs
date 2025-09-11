using Gtk;


namespace System.Windows.Forms
{
    public class ToolStripMenuItem : ToolStripItem
    {
        public StripMenuItem self = new StripMenuItem();
        public override IToolMenuItem Widget { get => self; }
        public override Gtk.MenuItem MenuItem { get => self; set { } }
        public Gtk.CheckButton checkButton => self.checkButton;
        public ToolStripMenuItem() : base()
        {
            checkButton.StyleContext.AddClass("MenuItemButton");
            self.Realized += Self_Realized;
            DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
        }
        private void Self_Realized(object? sender, EventArgs e)
        {
            SetIconImage(this.Image);
        }
        public override event EventHandler CheckedChanged;
        public override event EventHandler CheckStateChanged;
        public override string Name { get => self.Name; set => self.Name = value; }
        public override string Text { get => self.Text; set { self.Text = value; } }
        public override ToolStripItemDisplayStyle DisplayStyle
        {
            get => base.DisplayStyle;
            set => base.DisplayStyle = value;
        }
        public override Drawing.Image Image
        {
            get => base.Image;
            set
            {
                base.Image = value;
                SetIconImage(value);
            }
        }
        private void SetIconImage(Drawing.Image image)
        {
            if (self.IsRealized && image?.Pixbuf != null)
            {
                int x = 20;
                if (self.Parent is Gtk.Toolbar || self.Parent is Gtk.MenuBar)
                    x = Math.Max(20, self.AllocatedHeight / 2);
                Gdk.Pixbuf pixbuf = image.Pixbuf.ScaleSimple(x, x, Gdk.InterpType.Bilinear);
                self.Image = pixbuf;
            }
        }
        public override CheckState CheckState
        {
            get => self.CheckState;
            set => self.CheckState = value;
        }
        public override bool Checked
        {
            get => base.Checked;
            set
            {
                base.Checked = value;
                self.Checked = value;
            }
        }

        public override TextImageRelation TextImageRelation
        {
            get => base.TextImageRelation;
            set
            {
                base.TextImageRelation = value;
                if (value == TextImageRelation.ImageAboveText)
                {
                    checkButton.Halign = Align.Fill;
                    checkButton.ImagePosition = PositionType.Top;
                }
                else if (value == TextImageRelation.TextAboveImage)
                {
                    checkButton.Halign = Align.Fill;
                    checkButton.ImagePosition = PositionType.Bottom;
                }
                else if (value == TextImageRelation.ImageBeforeText)
                {
                    checkButton.Halign = Align.Start;
                    checkButton.ImagePosition = PositionType.Left;
                }
                else if (value == TextImageRelation.TextBeforeImage)
                {
                    checkButton.Halign = Align.Start;
                    checkButton.ImagePosition = PositionType.Right;
                }
                else
                {
                    checkButton.Halign = Align.Start;
                    checkButton.ImagePosition = PositionType.Left;
                }
            }
        }
    }
}
