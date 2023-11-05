// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Atk;
using GLib;
//using Gtk;
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
        public bool Checked { get; set; }
        public CheckState CheckState { get; set; }

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

        [Obsolete]
        [Property("image")]
        public Gtk.Widget Image
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
        [Obsolete]
        [Property("always-show-image")]
        public bool AlwaysShowImage
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
        [Obsolete]
        public new static GType GType
        {
            get
            {
                IntPtr val = gtk_image_menu_item_get_type();
                return new GType(val);
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
                Image = new Gtk.Image(new Gdk.Pixbuf(image.PixbufData));

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

        // public virtual Image Image { get; set; }
        public Color ImageTransparentColor { get; set; }
        public virtual ToolStripItemDisplayStyle DisplayStyle { get; set; }
        public virtual Size Size { get; set; }
        public virtual bool AutoToolTip { get; set; }

        public virtual Image BackgroundImage { get; set; }

        public virtual ImageLayout BackgroundImageLayout { get; set; }

        public virtual bool Enabled { get; set; }

        

        public virtual string ToolTipText { get { return base.TooltipText; } set { base.TooltipText = value; } }

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


