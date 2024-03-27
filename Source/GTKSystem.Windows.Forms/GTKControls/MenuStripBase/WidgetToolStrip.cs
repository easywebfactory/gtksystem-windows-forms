/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */

using Gtk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;


namespace System.Windows.Forms
{
    public class WidgetToolStrip<T> : ToolStripItem, IDropTarget, ISupportOleDropSource, IArrangedElement, IComponent, IDisposable, IKeyboardToolTip
    {
        public override string unique_key { get; protected set; }
        private Gtk.Widget _widget;
        public override Gtk.Widget Widget
        {
            get { return _widget; }
            set { _widget = value; base.Widget = _widget; }
        }

        private T _control;

        public T Control
        {
            get { return _control; }
        }
        private Gtk.MenuItem _menuItem;
        public override Gtk.MenuItem MenuItem { get => _menuItem; set{ _menuItem = value; base.MenuItem = value; } }

        internal Gtk.Box itemBox = new Gtk.Box(Gtk.Orientation.Horizontal, 0);
        internal Gtk.Image checkedico = Gtk.Image.NewFromIconName("object-select-symbolic", Gtk.IconSize.Menu);
        internal Gtk.Viewport icoViewport = new Gtk.Viewport() { BorderWidth = 0 };
        internal Gtk.Label label = new Gtk.Label();
        internal Gtk.Button button = new Gtk.Button();
        internal Gtk.Entry entry = new Gtk.Entry();
        internal Gtk.ComboBoxText comboBox = new Gtk.ComboBoxText();
       // internal Gtk.ProgressBar progressBar = new Gtk.ProgressBar();
        internal Gtk.LevelBar progressBar = new Gtk.LevelBar();
        public string StripType { get; set; }
        public WidgetToolStrip() : this(null, "", null, null, "",null)
        {
        }
        public WidgetToolStrip(string stripType) : base("", null, null, "")
        {
            object widget = Activator.CreateInstance(typeof(T));
            Widget = widget as Gtk.Widget;
            this.StripType = stripType;
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

        protected WidgetToolStrip(string stripType, string text, System.Drawing.Image image, EventHandler onClick, string name,params object[] args) : base(text, image, onClick, name)
        {
            object widget = Activator.CreateInstance(typeof(T), args);
            CreateControl(widget, stripType, text, image, onClick, name);
        }

        public override void CreateControl(object widget, string stripType, string text, System.Drawing.Image image, EventHandler onClick, string name, params object[] args)
        {
            this.StripType = stripType;
            _control = (T)widget;
            _widget = widget as Gtk.Widget;
            if (widget is Gtk.SeparatorMenuItem)
            {

            }
            else
            {
                _menuItem = widget as Gtk.MenuItem;
                
                _menuItem.Activated += _menuItem_Activated;
                _menuItem.Realized += ToolStripItem_Realized;
                _menuItem.Valign = Gtk.Align.Center;
                _menuItem.Halign = Gtk.Align.Fill;
                _menuItem.Vexpand = false;
                _menuItem.Hexpand = false;

                itemBox.Valign = Gtk.Align.Start;
                itemBox.Halign = Gtk.Align.Start;

                icoViewport.Vexpand = false;
                icoViewport.Hexpand = false;
                icoViewport.Valign = Gtk.Align.Center;
                icoViewport.Halign = Gtk.Align.Center;

                if (stripType == "ToolStripDropDownItem")
                {
                    button.Image = Gtk.Image.NewFromIconName("pan-down", IconSize.Button);
                    button.ImagePosition = PositionType.Right;
                    button.Relief = ReliefStyle.None;
                    button.AlwaysShowImage = true;
                    button.Halign = Gtk.Align.Start;
                    button.Valign = Gtk.Align.Center;
                    button.Hexpand = false;
                    button.Vexpand = false;
                    itemBox.PackStart(icoViewport, false, false, 0);
                    itemBox.PackStart(button, false, false, 1);
                    _menuItem.Add(itemBox);
                }
                else if(stripType == "ToolStripTextBox")
                {
                    entry.HasFrame = false;
                    entry.MaxWidthChars = 1;
                    entry.WidthChars = 0;
                    entry.Valign = Gtk.Align.Fill;
                    entry.Halign = Gtk.Align.Fill;
                    entry.IsFocus = true;
                    _menuItem.Add(entry);
                }
                else if (stripType == "ToolStripComboBox")
                {
                    comboBox.StyleContext.AddClass("ComboBox");
                    _menuItem.Add(comboBox);
                }
                else if (stripType == "ToolStripProgressBar")
                {
                    progressBar.Halign = Gtk.Align.Fill;
                    progressBar.Valign = Gtk.Align.Fill;
                    progressBar.Visible = true;
                    _menuItem.Add(progressBar);
                }
                else
                {
                    itemBox.PackStart(icoViewport, false, false, 0);
                    itemBox.PackStart(label, false, false, 1);
                    _menuItem.Add(itemBox);
                }

            }
        }

        private void _menuItem_Activated(object sender, EventArgs e)
        {
            if(((Gtk.MenuItem)sender).Child is Gtk.ComboBoxText combo)
            {
                combo.Popup();
            }
            else if (((Gtk.MenuItem)sender).Child is Gtk.Entry ey)
            {
                ey.IsFocus = true;
            }
        }
        public override bool Checked { get; set; }
        public override CheckState CheckState { get; set; }
        public override System.Drawing.Image Image {
            get => base.Image;
            set {
                base.Image = value;
            } 
        }


        private void ToolStripItem_Realized(object sender, EventArgs e)
        {
            UpdateStyle();
            if (this.Widget is Gtk.CheckMenuItem checkMenuItem)
            {
                if (this.CheckState == CheckState.Unchecked)
                    _widget.StyleContext.AddClass("ToolStripMenuItemNoChecked");
                if (this.Checked)
                {
                    checkMenuItem.Active = true;
                }
            }
            try
            {
                if (DisplayStyle == ToolStripItemDisplayStyle.Text)
                {
                }
                else if (DisplayStyle == ToolStripItemDisplayStyle.Image)
                {
                    label.Visible = false;
                    label.NoShowAll = true;
                    button.Label = string.Empty;
                    if (Image != null && Image.PixbufData != null)
                    {
                        Gtk.Image ico1 = new Gtk.Image(new Gdk.Pixbuf(Image.PixbufData).ScaleSimple(20, 20, Gdk.InterpType.Tiles));
                        icoViewport.Child = ico1;
                    }
                }
                else
                {
                    if (Image != null && Image.PixbufData != null)
                    {
                        Gtk.Image ico1 = new Gtk.Image(new Gdk.Pixbuf(Image.PixbufData).ScaleSimple(20, 20, Gdk.InterpType.Tiles));
                        icoViewport.Child = ico1;
                    }
                }
            }
            catch { }
            _widget.ShowAll();
        }
        internal void UpdateStyle()
        {
            SetStyle(_widget);
        }
        protected virtual void SetStyle(Gtk.Widget widget)
        {
            string stylename = $"s{unique_key}";
            StringBuilder style = new StringBuilder();
            if (this.BackColor.Name != "Control" && this.BackColor.Name != "0")
            {
                string color = $"rgba({this.BackColor.R},{this.BackColor.G},{this.BackColor.B},{this.BackColor.A})";
                style.AppendFormat("background-color:{0};background:{0};", color);
            }
            if (this.ForeColor.Name != "Control" && this.ForeColor.Name != "0")
            {
                string color = $"rgba({this.ForeColor.R},{this.ForeColor.G},{this.ForeColor.B},{this.ForeColor.A})";
                style.AppendFormat("color:{0};", color);
            }

            if (this.Font != null)
            {
                Pango.AttrList attributes = new Pango.AttrList();
                float textSize = this.Font.Size;
                if (this.Font.Unit == GraphicsUnit.Point)
                    textSize = this.Font.Size / 72 * 96;
                else if (this.Font.Unit == GraphicsUnit.Inch)
                    textSize = this.Font.Size * 96;

                style.AppendFormat("font-size:{0}px;", (int)textSize);
                if (string.IsNullOrWhiteSpace(Font.FontFamily.Name) == false)
                {
                    style.AppendFormat("font-family:\"{0}\";", Font.FontFamily.Name);
                    attributes.Insert(new Pango.AttrFontDesc(new Pango.FontDescription() { Family = Font.FontFamily.Name, Size = (int)(textSize * Pango.Scale.PangoScale * 0.7) }));
                }

                string[] fontstyle = Font.Style.ToString().ToLower().Split(new char[] { ',', ' ' });
                foreach (string sty in fontstyle)
                {
                    if (sty == "bold")
                    {
                        style.Append("font-weight:bold;");
                        attributes.Insert(new Pango.AttrWeight(Pango.Weight.Bold));
                    }
                    else if (sty == "italic")
                    {
                        style.Append("font-style:italic;");
                        attributes.Insert(new Pango.AttrStyle(Pango.Style.Italic));
                    }
                    else if (sty == "underline")
                    {
                        style.Append("text-decoration:underline;");
                        attributes.Insert(new Pango.AttrUnderline(Pango.Underline.Low));
                    }
                    else if (sty == "strikeout")
                    {
                        style.Append("text-decoration:line-through;");
                        attributes.Insert(new Pango.AttrStrikethrough(true));
                    }
                }
                if (widget is Gtk.Label gtklabel)
                {
                    gtklabel.Attributes = attributes;
                }
            }

            StringBuilder css = new StringBuilder();
            css.AppendLine($".{stylename}{{{style.ToString()}}}");
            if (widget is Gtk.TextView)
            {
                css.AppendLine($".{stylename} text{{{style.ToString()}}}");
                css.AppendLine($".{stylename} .view{{{style.ToString()}}}");
            }
            CssProvider provider = new CssProvider();
            if (provider.LoadFromData(css.ToString()))
            {
                widget.StyleContext.AddProvider(provider, 900);
                widget.StyleContext.RemoveClass(stylename);
                widget.StyleContext.AddClass(stylename);
            }
        }
        // public override string Text { get { return this.button.Label; } set { this.button.Label = value; } }
        public override string Text { 
            get {
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
            set { this.label.Text = value; this.button.Label = value; } }
        public override Color ImageTransparentColor { get; set; }
        public override ToolStripItemDisplayStyle DisplayStyle { get; set; }
        //public override Size Size { get; set; }
        public override bool AutoToolTip { get; set; }

        public override System.Drawing.Image BackgroundImage { get; set; }

        public override ImageLayout BackgroundImageLayout { get; set; }

        //public override bool Enabled { get; set; }
        public override string ToolTipText { get { return _widget.TooltipText; } set { _widget.TooltipText = value; } }
        public override ContentAlignment ImageAlign { get; set; }
        public override int ImageIndex { get; set; }
        public override string ImageKey { get; set; }
        public override ToolStripItemImageScaling ImageScaling { get; set; }

        public override TextImageRelation TextImageRelation { get; set; }

        public override ToolStripTextDirection TextDirection { get; set; }

        public override ContentAlignment TextAlign { get; set; }

        //public override bool Selected { get; }

        public override bool RightToLeftAutoMirrorImage { get; set; }

        public override bool Pressed { get; }
        public override ToolStripItemPlacement Placement { get; }
        public override ToolStripItemOverflow Overflow { get; set; }
        public override ToolStripItem OwnerItem { get; }

        public override ToolStrip Owner { get; set; }

        public override int MergeIndex { get; set; }
        public override MergeAction MergeAction { get; set; }



        public override bool Enabled { get { return _widget.Sensitive; } set { _widget.Sensitive = value; } }

        //  public override bool Focused { get { return this.IsFocus; } }

        private Font _Font;
        public override Font Font { get => _Font; set { _Font = value; UpdateStyle(); } }
        private Color _ForeColor;
        public override Color ForeColor { get => _ForeColor; set { _ForeColor = value; UpdateStyle(); } }
        private Color _BackColor;
        public override Color BackColor { get => _BackColor; set { _BackColor = value; UpdateStyle(); } }
        public override bool HasChildren { get; }

        public override int Height { get { return _widget.HeightRequest; } set { _widget.HeightRequest = value; } }
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
                return new Size(_widget.WidthRequest, _widget.HeightRequest);
            }
            set
            {
                _widget.SetSizeRequest(value.Width, value.Height);
            }
        }

        public override object Tag { get; set; }
        public override int Top
        {
            get;
            set;
        }

        public override bool UseWaitCursor { get; set; }
        public override int Width { get { return _widget.WidthRequest; } set { _widget.WidthRequest = value; } }
        public virtual bool Visible { get { return _widget.Visible; } set { _widget.Visible = value; _widget.NoShowAll = value == false; } }

        public override event EventHandler Click
        {
            add { _menuItem.Activated += (object sender, EventArgs args) => { value.Invoke(this, args); }; }
            remove { _menuItem.Activated -= (object sender, EventArgs args) => { value.Invoke(this, args); }; }
        }
        public override event EventHandler CheckedChanged
        {
            add { _menuItem.Activated += (object sender, EventArgs args) => { if (Checked) { value.Invoke(this, args); } }; }
            remove { _menuItem.Activated -= (object sender, EventArgs args) => { if (Checked) { value.Invoke(this, args); } }; }
        }
        public override event EventHandler CheckStateChanged
        {
            add { _menuItem.Activated += (object sender, EventArgs args) => { if (Checked) { value.Invoke(this, args); } }; }
            remove { _menuItem.Activated -= (object sender, EventArgs args) => { if (Checked) { value.Invoke(this, args); } }; }
        }
        public override event ToolStripItemClickedEventHandler DropDownItemClicked
        {
            add { _menuItem.Activated += (object sender, EventArgs args) => { value.Invoke(this, new ToolStripItemClickedEventArgs(this)); }; }
            remove { _menuItem.Activated -= (object sender, EventArgs args) => { value.Invoke(this, new ToolStripItemClickedEventArgs(this)); }; }
        }
    }
}
