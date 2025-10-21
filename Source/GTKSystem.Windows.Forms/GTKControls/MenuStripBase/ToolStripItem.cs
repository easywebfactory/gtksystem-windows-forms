using Gtk;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{

    public class ToolStripItem : Component, IDropTarget, ISupportOleDropSource, IArrangedElement, IComponent, IDisposable, IKeyboardToolTip
    {
        public virtual string unique_key { get; protected set; }
        public virtual IToolMenuItem Widget { get; protected set; }
        public virtual Gtk.MenuItem MenuItem { get; set; }
        public Gtk.Widget GetWidget()
        {
            return this.Widget.ToolItem ?? this.Widget.MenuItem;
        }
        public virtual bool Created { get; set; }
        public virtual bool Checked { get; set; }
        public virtual CheckState CheckState { get; set; }
        public virtual System.Drawing.Image Image { get; set; }
        private static Dictionary<string, string> fontLanguages = new Dictionary<string, string>();
        static ToolStripItem()
        {
            if (fontLanguages.Count == 0)
            {
                fontLanguages.Add("宋体", "SimSun");
                fontLanguages.Add("黑体", "SimHei");
                fontLanguages.Add("微软雅黑", "Microsoft Yahei");
                fontLanguages.Add("微软正黑", "Microsoft JhengHei");
                fontLanguages.Add("微軟正黑體", "Microsoft JhengHei");
                fontLanguages.Add("楷体", "KaiTi");
                fontLanguages.Add("新宋体", "NSimSun");
                fontLanguages.Add("仿宋", "FangSong");
                fontLanguages.Add("標楷體", "BiauKai");
                fontLanguages.Add("新細明體", "PMingLiU");
                fontLanguages.Add("細明體", "MingLiU");
                //macos
                fontLanguages.Add("苹方", "PingFang SC");
                fontLanguages.Add("华文黑体", "STHeiti");
                fontLanguages.Add("华文楷体", "STKaiti");
                fontLanguages.Add("华文宋体", "STSong");
                fontLanguages.Add("华文仿宋", "STFangsong");
                fontLanguages.Add("华文中宋", "STZhongsong");
                fontLanguages.Add("华文琥珀", "STHupo");
                fontLanguages.Add("华文新魏", "STXinwei");
                fontLanguages.Add("华文隶书", "STLiti");
                fontLanguages.Add("华文行楷", "STXingkai");
                //open
                fontLanguages.Add("思源黑体", "Source Han Sans CN");
                fontLanguages.Add("思源宋体", "Source Han Serif SC");
                fontLanguages.Add("文泉驿微米黑", "WenQuanYi Micro Hei");
            }
        }
        public ToolStripItem()
        {
            dropDownItems = new ToolStripItemCollection(this);
            this.unique_key = Guid.NewGuid().ToString().ToLower();
            Gtk.Widget _widget = this.Widget.ToolItem ?? this.Widget.MenuItem;
            _widget.ButtonPressEvent += _widget_ButtonPressEvent;
            _widget.ButtonReleaseEvent += _widget_ButtonReleaseEvent;
            if (_widget is Gtk.ToolButton widget)
            {
                widget.Clicked += Widget_Clicked;
            }
            else if (_widget is Gtk.MenuToolButton widget2)
            {
                widget2.Clicked += Widget_Clicked;
            }
            else if (_widget is Gtk.MenuItem widget3)
            {
                widget3.Selected += Widget3_Selected;
                widget3.Deselected += Widget3_Deselected;
                widget3.Activated += Widget3_Activated;
            }
        }
        private void Widget3_Activated(object? sender, EventArgs e)
        {
            DropDownOpened?.Invoke(this, EventArgs.Empty);

            if (Checked == true)
            {
                if(CheckState == CheckState.Checked)
                    CheckState = CheckState.Unchecked;
                else
                    CheckState = CheckState.Checked;

                CheckedChanged?.Invoke(this, EventArgs.Empty);
                CheckStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Widget3_Deselected(object? sender, EventArgs e)
        {
            DropDownClosed?.Invoke(this, EventArgs.Empty);
        }

        private void Widget3_Selected(object? sender, EventArgs e)
        {
            DropDownOpening?.Invoke(this, EventArgs.Empty);
        }
        private bool isDoubleClick = false;
        private void _widget_ButtonReleaseEvent(object o, Gtk.ButtonReleaseEventArgs args)
        {
            if (!isDoubleClick)
            {
                Gtk.Widget widget = (Gtk.Widget)o;
                MouseButtons button = MouseButtons.None;
                if (args.Event.State.HasFlag(Gdk.ModifierType.Button1Mask))
                    button = MouseButtons.Left;
                else if (args.Event.State.HasFlag(Gdk.ModifierType.Button2Mask))
                    button = MouseButtons.Middle;
                else if (args.Event.State.HasFlag(Gdk.ModifierType.Button3Mask))
                    button = MouseButtons.Right;

                widget.Window.GetOrigin(out int x1, out int y1);
                Click?.Invoke(this, new EventArgs());
                MouseEventArgs mouseArgs3 = new MouseEventArgs(button, 1, (int)args.Event.XRoot - x1, (int)args.Event.XRoot - y1, 0);
                MouseUp?.Invoke(this, mouseArgs3);
                MouseClick?.Invoke(this, mouseArgs3);
                DropDownItemClicked?.Invoke(this, new ToolStripItemClickedEventArgs(this));
            }
        }
        private void _widget_ButtonPressEvent(object o, Gtk.ButtonPressEventArgs args)
        {
            Gtk.Widget widget = (Gtk.Widget)o;
            MouseButtons button = MouseButtons.None;
            if (args.Event.State.HasFlag(Gdk.ModifierType.Button1Mask))
                button = MouseButtons.Left;
            else if (args.Event.State.HasFlag(Gdk.ModifierType.Button2Mask))
                button = MouseButtons.Middle;
            else if (args.Event.State.HasFlag(Gdk.ModifierType.Button3Mask))
                button = MouseButtons.Right;

            widget.Window.GetOrigin(out int x1, out int y1);
            MouseEventArgs mouseArgs = new MouseEventArgs(button, 1, (int)args.Event.XRoot - x1, (int)args.Event.XRoot - y1, 0);
            MouseDown?.Invoke(this, mouseArgs);
            isDoubleClick = false;
            if (args.Event.Type == Gdk.EventType.TwoButtonPress || args.Event.Type == Gdk.EventType.DoubleButtonPress)
            {
                isDoubleClick = true;
                DoubleClick?.Invoke(this, new EventArgs());
                ButtonDoubleClick?.Invoke(this, new EventArgs());
            }
        }

        private void Widget_Clicked(object? sender, EventArgs e)
        {
            ButtonClick?.Invoke(this, new EventArgs());
        }
        protected ToolStripItem(string text, Drawing.Image image, EventHandler onClick) : this(text, image, onClick, "")
        {
        }

        protected ToolStripItem(string text, Drawing.Image image, EventHandler onClick, string name) : this()
        {
            this.Name = name;
            this.Text = text;
            if (image != null && image.PixbufData != null)
                this.Image = image;

            if (onClick != null)
                Click += onClick;
        }
        public virtual void CreateControl() { }
        public virtual ToolStripItemCollection Items
        {
            get
            {
                return dropDownItems;
            }
        }
        private ToolStripItemCollection dropDownItems;

        public virtual ToolStripItemCollection DropDownItems
        {
            get
            {
                return dropDownItems;
            }
        }
        public virtual string Name { get; set; }
        public virtual string Text { get; set; }
        public virtual Color ImageTransparentColor { get; set; }
        public virtual ToolStripItemDisplayStyle DisplayStyle { get; set; }
        public virtual bool AutoToolTip { get; set; }

        public virtual Drawing.Image BackgroundImage { get; set; }

        public virtual ImageLayout BackgroundImageLayout { get; set; }
        public virtual string ToolTipText { get; set; }
        public virtual ContentAlignment ImageAlign { get; set; }
        public virtual int ImageIndex { get; set; }
        public virtual string ImageKey { get; set; }
        public virtual ToolStripItemImageScaling ImageScaling { get; set; }
        private TextImageRelation textImageRelation;
        public virtual TextImageRelation TextImageRelation
        {
            get => textImageRelation;
            set
            {
                textImageRelation = value;
                Gtk.Widget widget = GetWidget();
                if (widget?.Parent is Gtk.Toolbar toolbar)
                {
                    if (value == TextImageRelation.ImageAboveText || value == TextImageRelation.TextAboveImage)
                    {
                        toolbar.ToolbarStyle = Gtk.ToolbarStyle.Both;
                        toolbar.IconSize = Gtk.IconSize.Dialog;
                    }
                    else if (value == TextImageRelation.ImageBeforeText || value == TextImageRelation.TextBeforeImage)
                    {
                        toolbar.ToolbarStyle = Gtk.ToolbarStyle.BothHoriz;
                        toolbar.IconSize = Gtk.IconSize.SmallToolbar;
                    }
                    else
                    {
                        toolbar.ToolbarStyle = Gtk.ToolbarStyle.BothHoriz;
                        toolbar.IconSize = Gtk.IconSize.SmallToolbar;
                    }
                }
                else if (widget?.Parent is Gtk.MenuBar menubar)
                {
                    if (value == TextImageRelation.ImageAboveText)
                    {
                        widget.Halign = Gtk.Align.Center;
                        menubar.ChildPackDirection = Gtk.PackDirection.Ttb;
                    }
                    else if(value == TextImageRelation.TextAboveImage)
                    {
                        widget.Halign = Gtk.Align.Center;
                        menubar.ChildPackDirection = Gtk.PackDirection.Ttb;
                    }
                    else if (value == TextImageRelation.ImageBeforeText || value == TextImageRelation.TextBeforeImage)
                    {
                        widget.Halign = Gtk.Align.Start;
                        menubar.ChildPackDirection = Gtk.PackDirection.Ltr;
                    }
                    else
                    {
                        widget.Halign = Gtk.Align.Start;
                        menubar.ChildPackDirection = Gtk.PackDirection.Ltr;
                    }
                }
            }
        }
        public virtual ToolStripTextDirection TextDirection { get; set; }

        public virtual ContentAlignment TextAlign { get; set; }

        // public virtual bool Selected { get; }

        public virtual bool RightToLeftAutoMirrorImage { get; set; }

        public virtual bool Pressed { get; }
        public virtual ToolStripItemPlacement Placement { get; }
        public virtual ToolStripItemOverflow Overflow { get; set; }
        public virtual ToolStripItem OwnerItem { get; }

        public virtual ToolStrip Owner { get; set; }

        public virtual int MergeIndex { get; set; }
        public virtual MergeAction MergeAction { get; set; }

        public virtual bool Enabled { get { return GetWidget().Sensitive; } set { GetWidget().Sensitive = value; } }
        public virtual bool Visible { get { return GetWidget().Visible; }
            set { 
                Gtk.Widget widget = GetWidget();
                widget.NoShowAll = value == false;
                if (value == true) { widget.ShowAll(); } else { widget.Visible = value; }
            }
        }
        //  public virtual bool Focused { get { return this.IsFocus; } }

        private Font _Font;
        public virtual Font Font
        {
            get
            {
                return _Font;
            }
            set
            {
                _Font = value;
                if (_Font != null)
                {
                    Gtk.Widget widget = GetWidget();
                    Pango.FontDescription fdesc = widget.PangoContext.FontDescription;
                    string fontfamily = _Font.Name;
                    if (string.IsNullOrWhiteSpace(fontfamily) == false)
                    {
                        if (fontLanguages.TryGetValue(fontfamily, out string enname))
                        {
                            fdesc.Family = enname;
                            _Font = new Font(enname, value.Size, value.Style, value.Unit, value.GdiCharSet);
                        }
                        else if (widget.PangoContext.Families.Any(o => o.Name == fontfamily))
                        {
                            fdesc.Family = fontfamily;
                        }
                        //else
                        //{
                        //    Console.WriteLine($"\"{_Font.Name}\" font name is not supported, only English names are supported. Please confirm that the font name is correct or replace it with an English name");
                        //    _Font = new Font(fdesc.Family, value.Size, value.Style, value.Unit, value.GdiCharSet);
                        //}
                    }
                    if (_Font.Unit == GraphicsUnit.Point)
                        fdesc.Size = Convert.ToInt32(_Font.Size * Pango.Scale.PangoScale * 96 / 72);
                    else
                        fdesc.Size = Convert.ToInt32(_Font.Size * Pango.Scale.PangoScale);
                    if (_Font.Bold)
                        fdesc.Weight = Pango.Weight.Bold;
                    if (_Font.Italic)
                        fdesc.Style = Pango.Style.Italic;

                    widget.OverrideFont(fdesc);
                }
            }
        }

        private Color _ForeColor;
        public virtual Color ForeColor
        {
            get { return _ForeColor; }
            set { 
                _ForeColor = value;
                Gtk.Widget widget = GetWidget();
                widget.OverrideColor(Gtk.StateFlags.Normal, new Gdk.RGBA() { Alpha = (1.0 * _ForeColor.A / 0xff), Red = (1.0 * _ForeColor.R / 0xff), Green = (1.0 * _ForeColor.G / 0xff), Blue = (1.0 * _ForeColor.B / 0xff) });
            }
        }
        private Color _BackColor;
        public virtual Color BackColor { 
            get => _BackColor;
            set {
                _BackColor = value;
                Gtk.Widget widget = GetWidget();
                widget.OverrideBackgroundColor(Gtk.StateFlags.Normal, new Gdk.RGBA() { Alpha= (1.0 * _BackColor.A / 0xff), Red = (1.0 * _BackColor.R / 0xff), Green = (1.0 * _BackColor.G / 0xff), Blue = (1.0 * _BackColor.B / 0xff) });
                widget.OverrideBackgroundColor(Gtk.StateFlags.Prelight, new Gdk.RGBA() { Alpha = (1.0 * _BackColor.A / 0xff), Red = (0.95 * _BackColor.R / 0xff), Green = (0.95 * _BackColor.G / 0xff), Blue = (0.95 * _BackColor.B / 0xff) });
            }
        }
        public virtual bool HasChildren { get; }
        public virtual bool AutoSize { get; set; }
        public virtual int Height { get; set; }
        public virtual ImeMode ImeMode { get; set; }

        public virtual int Left
        {
            get;
            set;
        }

        //public override Padding Margin { get; set; }
        //public override Size MaximumSize { get; set; }
        //public override Size MinimumSize { get; set; }
        public virtual Padding Padding { get; set; }
        public virtual ToolStripItem Parent { get; set; }
        public virtual System.Drawing.Region Region { get; set; }
        public virtual int Right { get; }

        public virtual RightToLeft RightToLeft { get; set; }
        private ToolStripItemAlignment _Alignment;
        public virtual ToolStripItemAlignment Alignment {
            get => _Alignment;
            set {
                _Alignment = value;
                Gtk.Widget widget = GetWidget();
                if (value == ToolStripItemAlignment.Right)
                {
                    if (widget is Gtk.MenuItem item)
                        item.RightJustified = true;
                 
                }
            }
        }
        //public virtual ISite Site { get; set; }
        private Size _size;
        public virtual Size Size { get=> _size; 
            set { _size = value;
                Gtk.Widget widget = GetWidget();
                //widget.SetSizeRequest(value.Width, value.Height);
                widget.SetSizeRequest(value.Width, -1);
            } 
        }

        public virtual object Tag { get; set; }
        public virtual int Top
        {
            get;
            set;
        }
        public virtual void ResumeLayout()
        {

        }

        public virtual void ResumeLayout(bool performLayout)
        {

        }
        public virtual void SuspendLayout()
        {

        }

        public virtual void PerformLayout()
        {

        }

        public virtual void PerformLayout(Control affectedControl, string affectedProperty)
        {

        }

        public void SetBounds(Rectangle bounds, BoundsSpecified specified)
        {
            throw new NotImplementedException();
        }

        public Size GetPreferredSize(Size proposedSize)
        {
            throw new NotImplementedException();
        }

        void IArrangedElement.PerformLayout(IArrangedElement affectedElement, string propertyName)
        {
            throw new NotImplementedException();
        }

        public virtual bool UseWaitCursor { get; set; }
        public virtual int Width { get; set; }

        public Rectangle Bounds => throw new NotImplementedException();

        public Rectangle DisplayRectangle => throw new NotImplementedException();

        public bool ParticipatesInLayout => throw new NotImplementedException();

        PropertyStore IArrangedElement.Properties => throw new NotImplementedException();

        IArrangedElement IArrangedElement.Container => throw new NotImplementedException();

        public ArrangedElementCollection Children => throw new NotImplementedException();
        public virtual void PerformClick()
        {
            OnClick(EventArgs.Empty);
            Click?.Invoke(this, new EventArgs());
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnClick(EventArgs e)
        {

        }
        public virtual event EventHandler Click;
        public virtual event EventHandler CheckedChanged;
        public virtual event EventHandler CheckStateChanged;
        public virtual event ToolStripItemClickedEventHandler DropDownItemClicked;
        public virtual event EventHandler DropDownOpening;
        public virtual event EventHandler DropDownOpened;
        public virtual event EventHandler DropDownClosed;
        public virtual event EventHandler ButtonClick;
        public virtual event EventHandler ButtonDoubleClick;

        public virtual event EventHandler ContextMenuStripChanged;
        public virtual event EventHandler DockChanged;
        public virtual event EventHandler AnchorChanged;
        public virtual event EventHandler DoubleClick;
        public virtual event EventHandler EnabledChanged;
        public virtual event EventHandler Enter;
        public virtual event KeyEventHandler KeyDown;
        public virtual event KeyPressEventHandler KeyPress;
        public virtual event KeyEventHandler KeyUp;
        public virtual event LayoutEventHandler Layout;
        public virtual event EventHandler Leave;
        public virtual event EventHandler LocationChanged;
        public virtual event EventHandler LostFocus;
        public virtual event MouseEventHandler MouseClick;
        public virtual event MouseEventHandler MouseDoubleClick;
        public virtual event MouseEventHandler MouseDown;
        public virtual event EventHandler MouseEnter;
        public virtual event EventHandler MouseHover;
        public virtual event EventHandler MouseLeave;
        public virtual event MouseEventHandler MouseMove;
        public virtual event MouseEventHandler MouseUp;
        public virtual event MouseEventHandler MouseWheel;
        public virtual event EventHandler Move;
        public virtual event PaintEventHandler Paint;
        public virtual event EventHandler RegionChanged;
        public virtual event EventHandler Resize;
        public virtual event EventHandler SizeChanged;
        public virtual event EventHandler TextChanged;
        public virtual event EventHandler VisibleChanged;
        protected override void Dispose(bool disposing)
        {
            try
            {
                GetWidget()?.Dispose();
                Image?.Dispose();
                BackgroundImage?.Dispose();
                GC.SuppressFinalize(this);
            }
            catch (Exception ex) { }
            base.Dispose(disposing);
        }
    }

}


