
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    /// <summary>
    ///  A non selectable ToolStrip item
    /// </summary>
    public class ToolStripItem : Gtk.MenuItem
    {
        public virtual bool Checked { get; set; }
        public virtual CheckState CheckState { get; set; }

        internal Gtk.Image DefaultImage { get; set; } = new Gtk.Image("image-missing", Gtk.IconSize.SmallToolbar);
        private System.Drawing.Image _Image;
        public virtual System.Drawing.Image Image
        {
            get { return _Image; }
            set
            {
                _Image = value;
                if (value != null)
                {
                    AlwaysShowImage = true;
                    IcoImage = new Gtk.Image(new Gdk.Pixbuf(value.PixbufData).ScaleSimple(17, 17, Gdk.InterpType.Tiles));
                }
            }
        }

       // public Gtk.MenuItem Control => this;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr d_gtk_image_menu_item_get_image(IntPtr raw);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void d_gtk_image_menu_item_set_image(IntPtr raw, IntPtr image);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool d_gtk_image_menu_item_get_always_show_image(IntPtr raw);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void d_gtk_image_menu_item_set_always_show_image(IntPtr raw, bool always_show);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr d_gtk_image_menu_item_get_type();

        private static d_gtk_image_menu_item_get_image gtk_image_menu_item_get_image = FuncLoader.LoadFunction<d_gtk_image_menu_item_get_image>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_image_menu_item_get_image"));

        private static d_gtk_image_menu_item_set_image gtk_image_menu_item_set_image = FuncLoader.LoadFunction<d_gtk_image_menu_item_set_image>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_image_menu_item_set_image"));

        private static d_gtk_image_menu_item_get_always_show_image gtk_image_menu_item_get_always_show_image = FuncLoader.LoadFunction<d_gtk_image_menu_item_get_always_show_image>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_image_menu_item_get_always_show_image"));

        private static d_gtk_image_menu_item_set_always_show_image gtk_image_menu_item_set_always_show_image = FuncLoader.LoadFunction<d_gtk_image_menu_item_set_always_show_image>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_image_menu_item_set_always_show_image"));

        private static d_gtk_image_menu_item_get_type gtk_image_menu_item_get_type = FuncLoader.LoadFunction<d_gtk_image_menu_item_get_type>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_image_menu_item_get_type"));


        [GLib.Property("Image")]
        internal Gtk.Widget IcoImage
        {
            get
            {
                return GLib.Object.GetObject(gtk_image_menu_item_get_image(base.Handle)) as Gtk.Widget;
            }
            set
            {
                gtk_image_menu_item_set_image(base.Handle, value?.Handle ?? IntPtr.Zero);
            }
        }

        [GLib.Property("always-show-image")]
        internal bool AlwaysShowImage
        {
            get
            {
                return gtk_image_menu_item_get_always_show_image(base.Handle);
            }
            set
            {
                gtk_image_menu_item_set_always_show_image(base.Handle, value);
            }
        }

        public new static GLib.GType GType
        {
            get
            {
                IntPtr val = gtk_image_menu_item_get_type();
                return new GLib.GType(val);
            }
        }
        public ToolStripItem()
        {
            dropDownItems = new ToolStripItemCollection(this);
            base.Activated += ToolStripItem_Activated;
            
        }

        private void ToolStripItem_Activated(object sender, EventArgs e)
        {
            if (Checked == true)
            {
                AlwaysShowImage = AlwaysShowImage == false;
            }
        }

        public ToolStripItem(IntPtr raw) : base(raw)
        {
            dropDownItems = new ToolStripItemCollection(this);
        }
        protected ToolStripItem(string text, Image image, EventHandler onClick) : this(text, image, onClick, "")
        {
        }
        protected ToolStripItem(string text, Image image, EventHandler onClick, string name) : this()
        {
            this.Name = name;
            this.Text = text;
            if (image != null && image.PixbufData != null)
                IcoImage = new Gtk.Image(new Gdk.Pixbuf(image.PixbufData));

            Click += onClick;
        }

        public virtual ToolStripItemCollection Items
        {
            get
            {
                return dropDownItems;
            }
        }
        private ToolStripItemCollection dropDownItems;
        public ToolStripItemCollection DropDownItems
        {
            get
            {
                return dropDownItems;
            }
        }
        public virtual string Text { get { return base.Label; } set { base.Label = value; } }

        public Color ImageTransparentColor { get; set; }
        public virtual ToolStripItemDisplayStyle DisplayStyle { get; set; }
        //public virtual Size Size { get; set; }
        public virtual bool AutoToolTip { get; set; }

        public virtual Image BackgroundImage { get; set; }

        public virtual ImageLayout BackgroundImageLayout { get; set; }

        //public virtual bool Enabled { get; set; }
        public virtual string ToolTipText { get { return base.TooltipText; } set { base.TooltipText = value; } }
        public ContentAlignment ImageAlign { get; set; }
        public int ImageIndex { get; set; }
        public string ImageKey { get; set; }
        public ToolStripItemImageScaling ImageScaling { get; set; }

        public TextImageRelation TextImageRelation { get; set; }

        public virtual ToolStripTextDirection TextDirection { get; set; }

        public virtual ContentAlignment TextAlign { get; set; }

       // public virtual bool Selected { get; }

        public bool RightToLeftAutoMirrorImage { get; set; }

        public bool Pressed { get; }
        public ToolStripItemPlacement Placement { get; }
        public ToolStripItemOverflow Overflow { get; set; }
        public ToolStripItem OwnerItem { get; }

        public ToolStrip Owner { get; set; }

        public int MergeIndex { get; set; }
        public MergeAction MergeAction { get; set; }



        public virtual bool Enabled { get { return this.Sensitive; } set { this.Sensitive = value; } }

      //  public virtual bool Focused { get { return this.IsFocus; } }

        public virtual Font Font { get; set; }

        public virtual Color ForeColor { get; set; }

        public virtual bool HasChildren { get; }

        public virtual int Height { get { return this.HeightRequest; } set { this.HeightRequest = value; } }
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
        public new Control Parent { get; set; }
        public virtual Region Region { get; set; }
        public virtual int Right { get; }

        public virtual RightToLeft RightToLeft { get; set; }
        public virtual ISite Site { get; set; }
        public virtual Size Size
        {
            get
            {
                return new Size(this.WidthRequest, this.HeightRequest);
            }
            set
            {
                this.SetSizeRequest(value.Width, value.Height);
            }
        }

        public virtual object Tag { get; set; }
        public virtual int Top
        {
            get;
            set;
        }

        public virtual bool UseWaitCursor { get; set; }
        public virtual int Width { get { return this.WidthRequest; } set { this.WidthRequest = value; } }


        public event EventHandler Click
        {
            add { base.Activated += (object sender, EventArgs args) => { value.Invoke(this, args); }; }
            remove { base.Activated -= (object sender, EventArgs args) => { value.Invoke(this, args); }; }
        }
        public event EventHandler CheckedChanged
        {
            add { base.Activated += (object sender, EventArgs args) => { if (Checked) { value.Invoke(this, args); } }; }
            remove { base.Activated -= (object sender, EventArgs args) => { if (Checked) { value.Invoke(this, args); } }; }
        }
        public event EventHandler CheckStateChanged
        {
            add { base.Activated += (object sender, EventArgs args) => { if (Checked) { value.Invoke(this, args); } }; }
            remove { base.Activated -= (object sender, EventArgs args) => { if (Checked) { value.Invoke(this, args); } }; }
        }
        public event ToolStripItemClickedEventHandler DropDownItemClicked
        {
            add { base.Activated += (object sender, EventArgs args) => { value.Invoke(this, new ToolStripItemClickedEventArgs(this)); }; }
            remove { base.Activated -= (object sender, EventArgs args) => { value.Invoke(this, new ToolStripItemClickedEventArgs(this)); }; }
        }
    }

}


