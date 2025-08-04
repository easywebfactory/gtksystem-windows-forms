using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Drawing;

namespace System.Windows.Forms
{

    public class ToolStripLabel : ToolStripItem
    {
        public StripToolButton self = new StripToolButton();
        public override IToolMenuItem Widget { get => self; }
        public ToolStripLabel() : this("", null, null, "")
        {

        }

        public ToolStripLabel(string text) : this(text, null, null, "")
        {

        }

        public ToolStripLabel(string text, Image image) : this(text, image, null, "")
        {
        }

        public ToolStripLabel(string text, Image image, EventHandler onClick) : this(text, image, onClick, "")
        {

        }

        public ToolStripLabel(string text, Image image, EventHandler onClick, string name) : base(text, image, onClick, name)
        {
            DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            self.Realized += Self_Realized;
        }

        private void Self_Realized(object? sender, EventArgs e)
        {
            //Console.WriteLine(DisplayStyle);
            SetIcon(DisplayStyle);
        }

        public override string Name { get => self.Name; set => self.Name = value; }
        public override string Text { get => self.Label; set => self.Label = value; }
        public override ToolStripItemDisplayStyle DisplayStyle
        {
            get => base.DisplayStyle;
            set
            {
                base.DisplayStyle = value;
                SetIcon(value);
            }
        }
        private void SetIcon(ToolStripItemDisplayStyle displayStyle)
        {
            if (displayStyle == ToolStripItemDisplayStyle.Text)
            {
                self.IsImportant = false;
            }
            else if (displayStyle == ToolStripItemDisplayStyle.Image)
            {
                self.IsImportant = false;
                if (this.Image?.Pixbuf != null)
                {
                    if (self.Parent is ToolStripBase toolbar)
                    {
                        self.IconWidget = new Gtk.Image(new Gdk.Pixbuf(this.Image.PixbufData, toolbar.ImageScalingSize.Width, toolbar.ImageScalingSize.Height)) { Visible = true };
                    }
                }
            }
            else if (displayStyle == ToolStripItemDisplayStyle.ImageAndText)
            {
                self.IsImportant = true;
                if (this.Image?.Pixbuf != null)
                {
                    if (self.Parent is ToolStripBase toolbar)
                    {
                        self.IconWidget = new Gtk.Image(new Gdk.Pixbuf(this.Image.PixbufData, toolbar.ImageScalingSize.Width, toolbar.ImageScalingSize.Height)) { Visible = true };
                    }
                }

            }
            else
            {
                self.IsImportant = false;
            }
        }

    }

}


