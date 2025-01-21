/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */

using GLib;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms.Layout;


namespace System.Windows.Forms
{
    public class WidgetToolStrip<T> : ToolStripItem, IDropTarget, ISupportOleDropSource, IArrangedElement, IComponent, IDisposable, IKeyboardToolTip
    {
        public override string unique_key { get; protected set; }
        public override Gtk.MenuItem MenuItem { get => base.MenuItem; set { base.MenuItem = value; } }
        public override Gtk.Widget Widget => MenuItem;
        internal Gtk.Box itemBox = new Gtk.Box(Gtk.Orientation.Horizontal, 0);
        internal Gtk.Label label = new Gtk.Label();
        internal Gtk.Button button = new Gtk.Button();
        internal Gtk.Entry entry = new Gtk.Entry();
        internal Gtk.ComboBoxText comboBox = new Gtk.ComboBoxText() { HasFrame = false };
        // internal Gtk.ProgressBar progressBar = new Gtk.ProgressBar();
        internal Gtk.LevelBar progressBar = new Gtk.LevelBar();
        internal Gtk.Viewport flagBox = new Gtk.Viewport();
        Gtk.CssProvider provider = new Gtk.CssProvider();
        public string StripType { get; set; }
        public WidgetToolStrip() : this(null, "", null, null, "", null)
        {
        }

        public WidgetToolStrip(string stripType, params object[] args) : this(stripType, "", null, null, "", args)
        {

        }
        public WidgetToolStrip(string text, System.Drawing.Image image, EventHandler onClick) : this(null, text, image, onClick, "")
        {
        }
        public WidgetToolStrip(string text, System.Drawing.Image image, EventHandler onClick, string name) : this(null, text, image, onClick, "")
        {
        }

        protected WidgetToolStrip(string stripType, string text, System.Drawing.Image image, EventHandler onClick, string name, params object[] args) : base(text, image, onClick, name)
        {
            this.unique_key = Guid.NewGuid().ToString().ToLower();
            MenuItem = (Gtk.MenuItem)Activator.CreateInstance(typeof(T), args);
            MenuItem.StyleContext.AddProvider(provider, 900);
            if (stripType == "ToolStripSeparator")
            {
                Created = true;
            }
            else
            {
                this.StripType = stripType;
                this.MenuItem.Realized += ToolStripItem_Realized;
                this.MenuItem.Activated += MenuItem_Activated;
                this.MenuItem.ButtonReleaseEvent += MenuItem_ButtonReleaseEvent;
                this.MenuItem.Valign = Gtk.Align.Center;
                this.MenuItem.Halign = Gtk.Align.Fill;
                this.MenuItem.Vexpand = false;
                this.MenuItem.Hexpand = true;
                itemBox.Valign = Gtk.Align.Center;
                itemBox.Halign = Gtk.Align.Start;
                flagBox.BorderWidth = 0;
                flagBox.ShadowType = Gtk.ShadowType.None;
                flagBox.Hexpand = false;
                flagBox.Vexpand = false;
                if (stripType == "ToolStripDropDownItem")
                {
                    button.Image = Gtk.Image.NewFromIconName("pan-down", Gtk.IconSize.Button);
                    button.ImagePosition = Gtk.PositionType.Right;
                    button.Relief = Gtk.ReliefStyle.None;
                    button.AlwaysShowImage = true;
                    button.Halign = Gtk.Align.Start;
                    button.Valign = Gtk.Align.Center;
                    button.Hexpand = false;
                    button.Vexpand = false;
                    itemBox.PackStart(flagBox, false, false, 0);
                    itemBox.PackStart(button, false, false, 0);
                    this.MenuItem.Add(itemBox);
                }
                else if (stripType == "ToolStripTextBox")
                {
                    entry.HasFrame = false;
                    entry.MaxWidthChars = 1;
                    entry.WidthChars = 0;
                    entry.Valign = Gtk.Align.Fill;
                    entry.Halign = Gtk.Align.Fill;
                    entry.IsFocus = true;
                    this.MenuItem.Add(entry);
                }
                else if (stripType == "ToolStripComboBox")
                {
                    this.MenuItem.Add(comboBox);
                }
                else if (stripType == "ToolStripProgressBar")
                {
                    progressBar.Halign = Gtk.Align.Fill;
                    progressBar.Valign = Gtk.Align.Fill;
                    progressBar.Visible = true;
                    this.MenuItem.Add(progressBar);
                }
                else
                {
                    itemBox.PackStart(flagBox, false, false, 0);
                    itemBox.PackStart(label, false, false, 0);
                    this.MenuItem.Add(itemBox);
                }

            }
            Created = true;

        }

        private void MenuItem_ButtonReleaseEvent(object o, Gtk.ButtonReleaseEventArgs args)
        {
            if (DropDownItemClicked != null)
                DropDownItemClicked(this, new ToolStripItemClickedEventArgs(this));

            if (Click != null)
                Click(this, args);

        }

        private void MenuItem_Activated(object sender, EventArgs e)
        {
            if (this.CheckState != CheckState.Unchecked)
            {
                if (flagBox.Child is Gtk.CheckButton checkbutton)
                {
                    checkbutton.Active = !checkbutton.Active;
                }
            }

            if (((Gtk.MenuItem)sender).Child is Gtk.ComboBoxText combo)
            {
                combo.Popup();
            }
            else if (((Gtk.MenuItem)sender).Child is Gtk.Entry ey)
            {
                ey.IsFocus = true;
            }
        }
        public override bool Checked { get; set; }
        public override CheckState CheckState { get; set; } = CheckState.Unchecked;
        public override System.Drawing.Image Image
        {
            get => base.Image;
            set
            {
                base.Image = value;
            }
        }
        public ToolStripItemAlignment Alignment { get => MenuItem.Halign == Gtk.Align.End ? ToolStripItemAlignment.Right : ToolStripItemAlignment.Left; set { MenuItem.Hexpand = true; MenuItem.Halign = Gtk.Align.End; MenuItem.RightJustified = value == ToolStripItemAlignment.Right; } }
        internal Gtk.RadioButton groupradio = new Gtk.RadioButton("");
        private void ToolStripItem_Realized(object sender, EventArgs e)
        {
            SetStyle((Gtk.MenuItem)sender);
            Gtk.MenuItem menuItem = (Gtk.MenuItem)sender;
            if (menuItem.Parent is Gtk.Menu)
            {
                flagBox.Vexpand = true;
                flagBox.Hexpand = true;
                flagBox.WidthRequest = 30;
                menuItem.StyleContext.AddClass("MenuItem");
            }
            if (this.CheckState == CheckState.Checked)
            {
                Gtk.CheckButton checkbutton = new Gtk.CheckButton();
                checkbutton.StyleContext.AddClass("MenuCheck");
                checkbutton.BorderWidth = 0;
                checkbutton.Margin = 0;
                checkbutton.Halign = Gtk.Align.Center;
                checkbutton.Valign = Gtk.Align.Center;
                checkbutton.Active = this.Checked;
                checkbutton.Toggled += Checkbutton_Toggled;
                flagBox.Child = checkbutton;
            }
            else if (this.CheckState == CheckState.Indeterminate)
            {
                Gtk.RadioButton checkbutton = new Gtk.RadioButton(((WidgetToolStrip<Gtk.MenuItem>)this.Parent).groupradio);
                checkbutton.StyleContext.AddClass("MenuCheck");
                checkbutton.BorderWidth = 0;
                checkbutton.Margin = 0;
                checkbutton.Halign = Gtk.Align.Center;
                checkbutton.Valign = Gtk.Align.Center;
                checkbutton.Active = this.Checked;
                checkbutton.Toggled += Checkbutton_Toggled;
                flagBox.Child = checkbutton;
            }
            else
            {
                try
                {
                    if (DisplayStyle == ToolStripItemDisplayStyle.Text)
                    {
                        label.NoShowAll = false;
                        label.Visible = true;
                    }
                    else if (DisplayStyle == ToolStripItemDisplayStyle.Image)
                    {
                        label.Visible = false;
                        label.NoShowAll = true;
                        if (Image != null && Image.PixbufData != null)
                        {
                            Gtk.Image ico1 = new Gtk.Image(new Gdk.Pixbuf(Image.PixbufData).ScaleSimple(20, 20, Gdk.InterpType.Tiles));
                            ico1.Halign = Gtk.Align.Center;
                            ico1.Valign = Gtk.Align.Center;
                            flagBox.Child = ico1;
                        }
                    }
                    else
                    {

                        if (Image != null && Image.PixbufData != null)
                        {
                            Gtk.Image ico1 = new Gtk.Image(new Gdk.Pixbuf(Image.PixbufData).ScaleSimple(20, 20, Gdk.InterpType.Tiles));
                            ico1.Halign = Gtk.Align.Center;
                            ico1.Valign = Gtk.Align.Center;
                            flagBox.Child = ico1;
                        }
                    }
                }
                catch { }
            }
            menuItem.ShowAll();

        }

        private void Checkbutton_Toggled(object sender, EventArgs e)
        {
            if (CheckedChanged != null)
                CheckedChanged(this, e);
        }

        internal void UpdateStyle()
        {
            if (this.Widget.IsMapped)
                SetStyle(this.Widget);
        }
        protected virtual void SetStyle(Gtk.Widget widget)
        {

            StringBuilder style = new StringBuilder();
            if (this.BackColor.Name != "0")
            {
                string color = $"rgba({this.BackColor.R},{this.BackColor.G},{this.BackColor.B},{this.BackColor.A})";
                style.AppendFormat("background-color:{0};background:{0};", color);
            }
            if (this.ForeColor.Name != "0")
            {
                string color = $"rgba({this.ForeColor.R},{this.ForeColor.G},{this.ForeColor.B},{this.ForeColor.A})";
                style.AppendFormat("color:{0};", color);
            }

            if (this.Font != null)
            {
                Font font = this.Font;
                if (font.Unit == GraphicsUnit.Pixel)
                    style.AppendFormat("font-size:{0}px;", font.Size);
                else if (font.Unit == GraphicsUnit.Inch)
                    style.AppendFormat("font-size:{0}in;", font.Size);
                else if (font.Unit == GraphicsUnit.Point)
                    style.AppendFormat("font-size:{0}pt;", font.Size);
                else if (font.Unit == GraphicsUnit.Millimeter)
                    style.AppendFormat("font-size:{0}mm;", font.Size);
                else if (font.Unit == GraphicsUnit.Document)
                    style.AppendFormat("font-size:{0}cm;", font.Size);
                else if (font.Unit == GraphicsUnit.Display)
                    style.AppendFormat("font-size:{0}pc;", font.Size);
                else
                    style.AppendFormat("font-size:{0}pt;", font.Size);

                if (string.IsNullOrWhiteSpace(font.FontFamily?.Name) == false)
                {
                    style.AppendFormat("font-family:\"{0}\";", font.FontFamily.Name);
                }
                if ((font.Style & FontStyle.Bold) != 0)
                {
                    style.Append("font-weight:bold;");
                }
                if ((font.Style & FontStyle.Italic) != 0)
                {
                    style.Append("font-style:italic;");
                }
                if ((font.Style & FontStyle.Underline) != 0)
                {
                    style.Append("text-decoration:underline;");
                }
                if ((font.Style & FontStyle.Strikeout) != 0)
                {
                    style.Append("text-decoration:line-through;");
                }
            }

            string stylename = $"s{unique_key}";
            StringBuilder css = new StringBuilder();
            css.AppendLine($".{stylename}{{{style.ToString()}}}");
            if (widget is Gtk.TextView)
            {
                css.AppendLine($".{stylename} text{{{style.ToString()}}}");
                css.AppendLine($".{stylename} .view{{{style.ToString()}}}");
            }
            css.AppendLine(" menu menuitem .MenuCheck{padding:0px;margin:0px;} menu .MenuItem{padding:0px;margin-left:-23px;}");

            if (provider.LoadFromData(css.ToString()))
            {
                widget.StyleContext.RemoveClass(stylename);
                widget.StyleContext.AddClass(stylename);
            }
        }

        public override string Text
        {
            get
            {
                if (this.StripType == "ToolStripTextBox")
                {
                    return this.entry.Text;
                }
                else if (this.StripType == "ToolStripDropDownItem")
                {
                    return this.button.Label;
                }
                else if (this.StripType == "ToolStripComboBox")
                {
                    return this.comboBox.ActiveText;
                }
                return this.label.Text;
            }
            set { this.label.Text = value; this.button.Label = value; }
        }
        public override Color ImageTransparentColor { get; set; }
        public override ToolStripItemDisplayStyle DisplayStyle { get; set; }
        public override bool AutoToolTip { get; set; }
        public override System.Drawing.Image BackgroundImage { get; set; }
        public override ImageLayout BackgroundImageLayout { get; set; }
        public override string ToolTipText { get { return this.Widget.TooltipText; } set { this.Widget.TooltipText = value; } }
        public override ContentAlignment ImageAlign { get; set; }
        public override int ImageIndex { get; set; }
        public override string ImageKey { get; set; }
        public override ToolStripItemImageScaling ImageScaling { get; set; }
        public override TextImageRelation TextImageRelation { get; set; }
        public override ToolStripTextDirection TextDirection { get; set; }
        public override ContentAlignment TextAlign { get; set; }
        public override bool RightToLeftAutoMirrorImage { get; set; }
        public override bool Pressed { get; }
        public override ToolStripItemPlacement Placement { get; }
        public override ToolStripItemOverflow Overflow { get; set; }
        public override ToolStripItem OwnerItem { get; }
        public override ToolStrip Owner { get; set; }
        public override int MergeIndex { get; set; }
        public override MergeAction MergeAction { get; set; }
        public override bool Enabled { get { return this.Widget.Sensitive; } set { this.Widget.Sensitive = value; } }
        private Font _Font;
        public override Font Font
        {
            get
            {
                if (_Font == null)
                {
                    var fontdes = Widget.PangoContext.FontDescription;
                    int size = Convert.ToInt32(fontdes.Size / Pango.Scale.PangoScale);
                    return new Drawing.Font(new Drawing.FontFamily(fontdes.Family), size);
                }
                else
                    return _Font;
            }
            set { _Font = value; UpdateStyle(); }
        }
        private Color _ForeColor;
        public override Color ForeColor { get => _ForeColor; set { _ForeColor = value; UpdateStyle(); } }
        private Color _BackColor;
        public override Color BackColor { get => _BackColor; set { _BackColor = value; UpdateStyle(); } }
        public override bool HasChildren { get; }

        public override int Height { get { return this.Widget.HeightRequest; } set { this.Widget.HeightRequest = value; } }
        public override ImeMode ImeMode { get; set; }

        public override int Left
        {
            get;
            set;
        }

        //public override Padding Margin { get; set; }
        //public override Size MaximumSize { get; set; }
        //public override Size MinimumSize { get; set; }
        public override Padding Padding { get; set; }
        public override ToolStripItem Parent { get; set; }
        public override System.Drawing.Region Region { get; set; }
        public override int Right { get; }

        public override RightToLeft RightToLeft { get; set; }
        public override ISite Site { get; set; }
        public override Size Size
        {
            get
            {
                return new Size(this.Widget.WidthRequest, this.Widget.HeightRequest);
            }
            set
            {
                this.Widget.SetSizeRequest(value.Width, value.Height);
            }
        }

        public override object Tag { get; set; }
        public override int Top
        {
            get;
            set;
        }

        public override bool UseWaitCursor { get; set; }
        public override int Width { get { return this.Widget.WidthRequest; } set { this.Widget.WidthRequest = value; } }
        public virtual bool Visible { get { return this.Widget.Visible; } set { this.Widget.Visible = value; this.Widget.NoShowAll = value == false; } }

        public override event EventHandler Click;
        public override event EventHandler CheckedChanged;
        public override event EventHandler CheckStateChanged;
        public override event ToolStripItemClickedEventHandler DropDownItemClicked;
    }
}
