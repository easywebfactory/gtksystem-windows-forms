/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using GTKSystem.Windows.Forms.Utility;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Linq;
using static System.Windows.Forms.ListViewItem;



namespace System.Windows.Forms
{

    [DefaultEvent("SelectedIndexChanged")]
	public class ListView : ContainerControl
    {
        public readonly ListViewBase self = new ListViewBase();
        public override object GtkControl => self;
        private ListViewItemCollection _items;
		private ListViewGroupCollection _groups;
		private ColumnHeaderCollection _columns;
        internal Gtk.Box flowBoxContainer = new Gtk.Box(Gtk.Orientation.Vertical, 0);
        internal Gtk.StackSwitcher header = new Gtk.StackSwitcher();
        public ListView():base()
        {
			_items = new ListViewItemCollection(this);
			_groups = new ListViewGroupCollection(this);
			_columns = new ColumnHeaderCollection(this);
            self.Realized += Control_Realized;

            header.StyleContext.AddClass("ListViewHeader");
            header.Spacing = 0;
            header.Homogeneous = false;
            header.Halign = Gtk.Align.Fill;
            header.Valign = Gtk.Align.Fill;
            header.HeightRequest = 35;
            header.NoShowAll = true;
            header.Visible = false;
            header.Hide();
            flowBoxContainer.PackStart(header, false, false, 0);
            self.Add(flowBoxContainer);
            this.BorderStyle = BorderStyle.Fixed3D;
        }
        private bool ControlRealized = false;
        private void Control_Realized(object sender, EventArgs e)
        {
            if (ControlRealized == false)
            {
                ControlRealized = true;
                if (this.View == View.Details)
                {
                    header.NoShowAll = false;
                    header.Visible = true;
                    header.ShowAll();
                    foreach (ColumnHeader col in Columns)
                    {
                        LabelBase label = new LabelBase(col.Text) { WidthRequest = col.Width, MaxWidthChars = 0, Valign = Gtk.Align.End, Ellipsize = Pango.EllipsizeMode.End };
                        label.TooltipText = col.Text;
                        label.Markup = col.Text;
                        label.Data.Add("ColumnIndex", col.DisplayIndex);
                        label.Override.DrawnBackground += Override_DrawnBackground;
                        var columbt = new Gtk.Button(label) { WidthRequest = col.Width, Halign = Gtk.Align.Fill, Valign = Gtk.Align.Fill };
                        columbt.ActionTargetValue = new GLib.Variant(col.DisplayIndex);
                        columbt.Data.Add("ColumnIndex", col.DisplayIndex);
                        columbt.Clicked += Columbt_Clicked;
                        header.Add(columbt);
                    }
                }
                var group = Items.GroupBy(g => g.Group);
                foreach (var g in group)
                {
                    NativeGroupAdd(g.Key, -1);
                    foreach (ListViewItem item in g)
                    {
                        NativeAdd(item, -1);
                    }
                }

                MultiSelect = MultiSelect == true;
                self.ShowAll();
            }
        }

        private void Columbt_Clicked(object sender, EventArgs e)
        {
            Gtk.Button btn = (Gtk.Button)sender;
            
            if (this.HeaderStyle == ColumnHeaderStyle.Clickable)
            {
                int actioncolumn = (int)btn.ActionTargetValue;
                if (this.AllowColumnReorder == true)
                {
                    if (SortingColumnIndex == actioncolumn)
                    {
                        if (Sorting == SortOrder.Ascending)
                            Sorting = SortOrder.Descending;
                        else if (Sorting == SortOrder.Descending)
                            Sorting = SortOrder.None;
                        else
                            Sorting = SortOrder.Ascending;
                    }
                    else
                    {
                        Sorting = SortOrder.Ascending;
                    }
                    
                    foreach (var box in flowBoxContainer.AllChildren)
                    {
                        if (box is Gtk.Box group)
                        {
                            foreach (var flow in group.Children)
                            {
                                if (flow is Gtk.FlowBox _flow)
                                {
                                    _flow.InvalidateSort();
                                }
                            }
                        }
                    }

                    if (ColumnReordered != null)
                        ColumnReordered(this, new ColumnReorderedEventArgs(SortingColumnIndex, actioncolumn, Columns[actioncolumn]));
                    
                    SortingColumnIndex = actioncolumn;
                }
                if (ColumnClick != null)
                    ColumnClick(this, new ColumnClickEventArgs((int)btn.ActionTargetValue));
            }
        }

        private void Override_DrawnBackground(object o, DrawnArgs args)
        {
            Cairo.Rectangle rec = args.Cr.ClipExtents();
            if (rec.Width > 10 && SortingColumnIndex > -1)
            {
                LabelBase ws = (LabelBase)o;
                if (ws.Data["ColumnIndex"].ToString() == SortingColumnIndex.ToString())
                {
                    if (Sorting != SortOrder.None)
                    {
                        Cairo.Context ctx = args.Cr;
                        ctx.Save();
                        ctx.ResetClip();
                        ctx.Rectangle(rec.X, rec.Y - 4, rec.Width, rec.Height + 4);
                        ctx.Clip();
                        ctx.Translate((int)rec.Width / 2 - 16, rec.Y - 5);
                        ctx.Rotate(0.5 * Math.PI);
                        Gdk.RGBA color = ws.StyleContext.GetColor(StateFlags.Normal);
                        ctx.SetSourceRGBA(color.Red, color.Green, color.Blue, color.Alpha);
                        //Serif,Impact,Sylfaen,Arial Black,Cambria,Candara,Arial
                        ctx.SelectFontFace("Serif", Cairo.FontSlant.Normal, Cairo.FontWeight.Bold);
                        ctx.SetFontSize(13);
                        if (Sorting == SortOrder.Descending)
                            ctx.ShowText("<");
                        else if (Sorting == SortOrder.Ascending)
                            ctx.ShowText(">");
                        ctx.Stroke();
                        ctx.Restore();
                    }
                }
            }
        }
        private int SortingColumnIndex = -1;
        public bool Sorted { get; set; }
        public System.Windows.Forms.SortOrder Sorting { get; set; }
        public System.Windows.Forms.ListViewAlignment Alignment { get; set; }
        public bool AllowColumnReorder { get; set; }
		public bool GridLines { get; set; } = true;
		public ImageList GroupImageList { get; set; }
        public System.Windows.Forms.ColumnHeaderStyle HeaderStyle { get; set; } = ColumnHeaderStyle.Clickable;

        public bool HideSelection { get; set; }
		public bool HoverSelection { get; set; }
		public bool LabelEdit { get; set; }
		public bool LabelWrap { get; set; }
		public ImageList LargeImageList { get; set; }
        private bool _MultiSelect;
        public virtual bool MultiSelect
        {
            get
            {
                return _MultiSelect;
            }
            set
            {
                _MultiSelect = value;
                foreach (var box in flowBoxContainer.AllChildren)
                {
                    if (box is Gtk.Box group)
                    {
                        foreach (var flow in group.Children)
                        {
                            if (flow is Gtk.FlowBox _flow)
                            {
                                if (value == true)
                                {
                                    _flow.SelectionMode = Gtk.SelectionMode.Multiple;
                                }
                                else
                                {
                                    _flow.SelectionMode = Gtk.SelectionMode.Single;
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool OwnerDraw { get; set; }
		public bool Scrollable { get; set; }
        public bool ShowGroups { get; set; } = true;
		public bool ShowItemToolTips { get; set; }

		public ImageList SmallImageList { get; set; }

        public ImageList StateImageList { get; set; }

		public bool UseCompatibleStateImageBehavior { get; set; }
		public System.Windows.Forms.View View { get; set; }
        private int _fontSize;
        protected int FontSize
        {
            get
            {
                if (_fontSize < 5)
                {
                    if (Font?.Size != null && Font.Size > 5)
                    {
                        if (Font.Unit == GraphicsUnit.Point)
                            _fontSize = (int)Font.Size * 96 / 72;
                        else
                            _fontSize = (int)Font.Size;
                    }
                }
                return _fontSize;
            }
        }
        protected void NativeAdd(ListViewItem item, int position)
        {
            Gtk.FlowBoxChild boxitem = new Gtk.FlowBoxChild();
            boxitem.TooltipText = item.Text;
            boxitem.Data.Add("ItemId", item.Index);
            boxitem.Halign = Gtk.Align.Start;
            boxitem.Valign = Gtk.Align.Start;
            boxitem.HeightRequest = FontSize + 12; 
            boxitem.BorderWidth = 0;
            boxitem.Margin = 0;

            BoxBase hBox = new BoxBase(Gtk.Orientation.Horizontal, 4);
            if (item.BackColor.HasValue)
                hBox.Override.BackColor = item.BackColor.Value;
            hBox.Valign = Gtk.Align.Fill;
            hBox.Halign = Gtk.Align.Start;
            hBox.BorderWidth = 0;
            hBox.Homogeneous = false;

            foreach (ColumnHeader col in Columns)
            {
                if (col.DisplayIndex == 0)
                    boxitem.Data.Add(0, item.Text);
                else if (item.SubItems != null && item.SubItems.Count > col.DisplayIndex - 1)
                {
                    boxitem.Data.Add(col.DisplayIndex, item.SubItems[col.DisplayIndex - 1].Text);
                }else
                    boxitem.Data.Add(col.DisplayIndex, string.Empty);
            }
            boxitem.Add(hBox);

            Gtk.FlowBox flowBox = DefaultGroup.FlowBox; 
            if (this.ShowGroups == true && this.View != View.List && this.View != View.Tile)
            {
                flowBox = item.Group.FlowBox;
            }
            flowBox.Halign = Gtk.Align.Fill;
            flowBox.Valign = Gtk.Align.Fill;
            flowBox.Name = item.Group.Name;
            flowBox.ColumnSpacing = 0;
            if (position == -1)
                flowBox.Add(boxitem);
            else
                flowBox.Insert(boxitem, position);

            if (this.CheckBoxes == true)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.self.Halign = Gtk.Align.Start;
                checkBox.self.Valign = Gtk.Align.Center;
                checkBox.Width = 25;
                checkBox.self.BorderWidth = 0;
                checkBox.Checked = item.Checked;
                checkBox.CheckedChanged += (object sender, EventArgs e) =>
                {
                    CheckBox box = sender as CheckBox;
                    Gtk.FlowBoxChild boxitem = box.self.Parent.Parent as Gtk.FlowBoxChild;
                    ListViewItem thisitem = this.Items[Convert.ToInt32(boxitem.Data["ItemId"])];
                    thisitem.Checked = box.self.Active;
                    if (ItemCheck != null)
                    {
                        ItemCheck(sender, new ItemCheckEventArgs(boxitem.Index, box.self.Active ? CheckState.Checked : CheckState.Unchecked, box.self.Active ? CheckState.Unchecked : CheckState.Checked));
                    }
                    if (ItemChecked != null)
                    {
                        ItemChecked(sender, new ItemCheckedEventArgs(thisitem));
                    }
                };
                hBox.PackStart(checkBox.self, false, true, 0);
            }

            if (this.View == View.Details)
            {
                header.Visible = true;
                flowBox.MinChildrenPerLine = 1;
                flowBox.MaxChildrenPerLine = 1;

                Gtk.Layout fistcell = new Gtk.Layout(new Adjustment(IntPtr.Zero), new Adjustment(IntPtr.Zero));
                fistcell.Halign = Gtk.Align.Start;
                fistcell.Valign = Gtk.Align.Fill;
                fistcell.Vexpand = true;
                fistcell.BorderWidth = 0;
                fistcell.WidthRequest = Columns.Count > 0 ? Columns[0].Width - 30 : -1;
                fistcell.HeightRequest = boxitem.HeightRequest;

                int x_position = 0;
                int padding = 2;
                if (this.SmallImageList != null)
                {
                    int imgsize = FontSize + 2;
                    this.SmallImageList.ImageSize = new Size(imgsize, imgsize);
                    if (!string.IsNullOrWhiteSpace(item.ImageKey))
                    {
                        Drawing.Image img = this.SmallImageList.Images[item.ImageKey];
                        fistcell.Put(new Gtk.Image(img.Pixbuf) { Halign = Gtk.Align.Start, Valign = Gtk.Align.Fill }, x_position, padding + 2);
                        x_position += imgsize + 5;
                    }
                    else if (item.ImageIndex > -1)
                    {
                        Drawing.Image img = this.SmallImageList.Images[item.ImageIndex];
                        fistcell.Put(new Gtk.Image(img.Pixbuf) { Halign = Gtk.Align.Start, Valign = Gtk.Align.Fill }, x_position, padding + 2);
                        x_position += imgsize + 5;
                    }
                }
                Gtk.Label lab = new Gtk.Label();
                Pango.AttrList attributes = new Pango.AttrList();
                if (item.ForeColor.HasValue)
                {
                    Pango.AttrForeground fg = new Pango.AttrForeground(Convert.ToUInt16(item.ForeColor.Value.R * 257), Convert.ToUInt16(item.ForeColor.Value.G * 257), Convert.ToUInt16(item.ForeColor.Value.B * 257));
                    attributes.Insert(fg);
                }
                if (item.BackColor.HasValue)
                {
                    Pango.AttrBackground fg = new Pango.AttrBackground(Convert.ToUInt16(item.BackColor.Value.R * 257), Convert.ToUInt16(item.BackColor.Value.G * 257), Convert.ToUInt16(item.BackColor.Value.B * 257));
                    attributes.Insert(fg);
                }
                lab.Attributes = attributes;
                lab.Halign = Gtk.Align.Start;
                lab.Valign = Gtk.Align.Fill;
                lab.Ellipsize = Pango.EllipsizeMode.End;
                lab.Text = item.Text;
                lab.Xalign = 0;
                lab.Xpad = 0;
                lab.Ypad = 0;
                lab.WidthRequest = fistcell.WidthRequest - x_position;
                fistcell.Put(lab, x_position, padding);
                hBox.PackStart(fistcell, false, true, 0);

                int index = 0;
                foreach (ColumnHeader col in Columns)
                {
                    if (index > 0)
                    {
                        Gtk.Layout sublayout = new Gtk.Layout(new Adjustment(IntPtr.Zero), new Adjustment(IntPtr.Zero));
                        sublayout.Halign = Gtk.Align.Start;
                        sublayout.Valign = Gtk.Align.Fill;
                        sublayout.WidthRequest = col.Width;
                        if (item.SubItems != null && item.SubItems.Count > index - 1)
                        {
                            ListViewSubItem subitem = item.SubItems[index - 1];
                            Gtk.Label sublabel = new Gtk.Label();
                            Pango.AttrList subattributes = new Pango.AttrList();
                            if (subitem.ForeColor.HasValue)
                            {
                                Pango.AttrForeground fg = new Pango.AttrForeground(Convert.ToUInt16(subitem.ForeColor.Value.R * 257), Convert.ToUInt16(subitem.ForeColor.Value.G * 257), Convert.ToUInt16(subitem.ForeColor.Value.B * 257));
                                subattributes.Insert(fg);
                                sublabel.Attributes = subattributes;
                            }
                            if (subitem.BackColor.HasValue)
                            {
                                Pango.AttrBackground fg = new Pango.AttrBackground(Convert.ToUInt16(subitem.BackColor.Value.R * 257), Convert.ToUInt16(subitem.BackColor.Value.G * 257), Convert.ToUInt16(subitem.BackColor.Value.B * 257));
                                subattributes.Insert(fg);
                            }
                            sublabel.Attributes = subattributes;
                            sublabel.WidthRequest = col.Width + 2;
                            sublabel.MaxWidthChars = 0;

                            sublabel.Halign = Gtk.Align.Fill;
                            sublabel.Valign = Gtk.Align.Fill;
                            sublabel.Ellipsize = Pango.EllipsizeMode.End;
                            sublabel.Text = subitem.Text;
                            sublayout.Add(sublabel);
                        }
                        hBox.PackStart(sublayout, false, true, 0);
                    }
                    index++;
                }
            }
            else if (this.View == View.SmallIcon)
            {
                header.Visible = false;

                flowBox.Orientation = Gtk.Orientation.Horizontal;
                flowBox.MinChildrenPerLine = 1;
                flowBox.MaxChildrenPerLine = 999;
                if (this.SmallImageList != null)
                {
                    this.SmallImageList.ImageSize = new Size(16, 16);
                    if (!string.IsNullOrWhiteSpace(item.ImageKey))
                    {
                        Drawing.Image img = this.SmallImageList.Images[item.ImageKey];
                        hBox.PackStart(new Gtk.Image(img.Pixbuf), false, false, 0);
                    }
                    else if (item.ImageIndex > -1)
                    {
                        Drawing.Image img = this.SmallImageList.Images[item.ImageIndex];
                        hBox.PackStart(new Gtk.Image(img.Pixbuf), false, false, 0);
                    }

                }
                Gtk.Label lab = new Gtk.Label();
                Pango.AttrList attributes = new Pango.AttrList();
                if (item.ForeColor.HasValue)
                {
                    Pango.AttrForeground fg = new Pango.AttrForeground(Convert.ToUInt16(item.ForeColor.Value.R * 257), Convert.ToUInt16(item.ForeColor.Value.G * 257), Convert.ToUInt16(item.ForeColor.Value.B * 257));
                    attributes.Insert(fg);

                }
                if (item.BackColor.HasValue)
                {
                    Pango.AttrBackground fg = new Pango.AttrBackground(Convert.ToUInt16(item.BackColor.Value.R * 257), Convert.ToUInt16(item.BackColor.Value.G * 257), Convert.ToUInt16(item.BackColor.Value.B * 257));
                    attributes.Insert(fg);
                }
                lab.Attributes = attributes;
                lab.MaxWidthChars = 100;
                lab.Halign = Gtk.Align.Start;
                lab.Valign = Gtk.Align.Center;
                lab.Ellipsize = Pango.EllipsizeMode.End;
                lab.Text = item.Text;
                hBox.PackStart(lab, false, false, 0);
            }
            else if (this.View == View.LargeIcon)
            {
                flowBox.MinChildrenPerLine = 1;
                flowBox.MaxChildrenPerLine = 999;
       
                Gtk.Box vBox = new Gtk.Box(Gtk.Orientation.Vertical, 0);
                if (this.LargeImageList != null)
                {
                    this.SmallImageList.ImageSize = new Size(100, 100);
                    if (!string.IsNullOrWhiteSpace(item.ImageKey))
                    {
                        Drawing.Image img = this.LargeImageList.Images[item.ImageKey];
                        int width = img.Pixbuf.Width;
                        int height = img.Pixbuf.Height;

                        vBox.Add(new Gtk.Image(new Gdk.Pixbuf(img.PixbufData)) { WidthRequest = Math.Min(50, width), HeightRequest = Math.Min(50, height) });
                    }
                    else if (item.ImageIndex > -1)
                    {
                        Drawing.Image img = this.LargeImageList.Images[item.ImageIndex];
                        int width = img.Pixbuf.Width;
                        int height = img.Pixbuf.Height;

                        vBox.Add(new Gtk.Image(img.Pixbuf) { WidthRequest = Math.Min(50, width), HeightRequest = Math.Min(50, height) });
                    }
                }
                Gtk.Label lab = new Gtk.Label();
                Pango.AttrList attributes = new Pango.AttrList();
                if (item.ForeColor.HasValue)
                {
                    Pango.AttrForeground fg = new Pango.AttrForeground(Convert.ToUInt16(item.ForeColor.Value.R * 257), Convert.ToUInt16(item.ForeColor.Value.G * 257), Convert.ToUInt16(item.ForeColor.Value.B * 257));
                    attributes.Insert(fg);

                }
                if (item.BackColor.HasValue)
                {
                    Pango.AttrBackground fg = new Pango.AttrBackground(Convert.ToUInt16(item.BackColor.Value.R * 257), Convert.ToUInt16(item.BackColor.Value.G * 257), Convert.ToUInt16(item.BackColor.Value.B * 257));
                    attributes.Insert(fg);
                }
                lab.MaxWidthChars = 16;
                lab.Halign = Gtk.Align.Center;
                lab.Valign = Gtk.Align.Center;
                lab.Ellipsize = Pango.EllipsizeMode.End;
                lab.Text = item.Text;
                vBox.Add(lab);
                hBox.PackStart(vBox, false, false, 0);
            }
            else
            {
                header.Visible = false;
                boxitem.Halign = Gtk.Align.Fill;

                Gtk.Label lab = new Gtk.Label();
                Pango.AttrList attributes = new Pango.AttrList();
                if (item.ForeColor.HasValue)
                {
                    Pango.AttrForeground fg = new Pango.AttrForeground(Convert.ToUInt16(item.ForeColor.Value.R * 257), Convert.ToUInt16(item.ForeColor.Value.G * 257), Convert.ToUInt16(item.ForeColor.Value.B * 257));
                    attributes.Insert(fg);

                }
                if (item.BackColor.HasValue)
                {
                    Pango.AttrBackground fg = new Pango.AttrBackground(Convert.ToUInt16(item.BackColor.Value.R * 257), Convert.ToUInt16(item.BackColor.Value.G * 257), Convert.ToUInt16(item.BackColor.Value.B * 257));
                    attributes.Insert(fg);
                }
                lab.Halign = Gtk.Align.Start;
                lab.Valign = Gtk.Align.Fill;
                lab.Text = item.Text;
                hBox.PackStart(lab, false, false, 0);
            }
        }

        protected void NativeGroupAdd(ListViewGroup group, int position)
        {
            if (group == null)
                return;

            Gtk.FlowBox _flow = DefaultGroup.FlowBox;
            Gtk.Box hBox = new Gtk.Box(Gtk.Orientation.Vertical, 0);
            hBox.Valign = Gtk.Align.Start;
            hBox.Halign = Gtk.Align.Fill;
            hBox.Expand = true;
            if (ShowGroups == true && this.View != View.List && this.View != View.Tile)
            {
                _flow = group.FlowBox;
                if (_flow.Parent != null)
                    return;

                Gtk.Box groupbox=new Box(Gtk.Orientation.Horizontal, 0);
                groupbox.StyleContext.AddClass("GroupTitle");
                groupbox.MarginStart = 3;
                var title = new Gtk.Label(group.Header) { Xalign = 0, Halign = Gtk.Align.Start, Valign = Gtk.Align.Center, Ellipsize = Pango.EllipsizeMode.End };
                groupbox.PackStart(title, false, false, 0);
                Gtk.Viewport groupline = new Gtk.Viewport();
                groupline.StyleContext.AddClass("GroupLine");
                groupline.HeightRequest = 1;
                groupline.Halign = Gtk.Align.Fill;
                groupline.Valign = Gtk.Align.Center;
                groupline.Vexpand = false;
                groupbox.PackEnd(groupline, true, true, 0);
                hBox.PackStart(groupbox, false, false, 0);
                if (!string.IsNullOrEmpty(group.Subtitle))
                {
                    var subtitle = new Gtk.Label(group.Subtitle) { Xalign = 0, Halign = Gtk.Align.Fill, Valign = Gtk.Align.Start, Ellipsize = Pango.EllipsizeMode.End };
                    subtitle.MarginStart = 3;
                    subtitle.StyleContext.AddClass("GroupSubTitle");
                    hBox.PackStart(subtitle, false, false, 0);
                }
            }
            if (_flow.Parent != null)
                return;

            _flow.MaxChildrenPerLine = 100u;
            _flow.MinChildrenPerLine = 0u;
            _flow.ColumnSpacing = 0;
            _flow.Halign = Gtk.Align.Fill;
            _flow.Valign = Gtk.Align.Fill;
            _flow.Orientation = Gtk.Orientation.Horizontal;
            _flow.SelectionMode = Gtk.SelectionMode.Single;
            _flow.SortFunc = new Gtk.FlowBoxSortFunc((fbc1, fbc2) =>
            {
                if (this.AllowColumnReorder && SortingColumnIndex > -1)
                {
                    if (this.Sorting == SortOrder.Ascending)
                        return fbc2.Data[SortingColumnIndex].ToString().CompareTo(fbc1.Data[SortingColumnIndex].ToString());
                    else if (this.Sorting == SortOrder.Descending)
                        return fbc1.Data[SortingColumnIndex].ToString().CompareTo(fbc2.Data[SortingColumnIndex].ToString());
                    else
                        return fbc2.Index.CompareTo(fbc1.Index);
                }
                else
                    return 0;
            });
            _flow.ChildActivated += _flow_ChildActivated;
            hBox.PackStart(_flow, true, true, 0);

            if (ShowGroups == true && this.View != View.List && this.View != View.Tile)
            {
                if (!string.IsNullOrEmpty(group.Footer))
                {
                    var footer = new Gtk.Label(group.Footer) { Xalign = 0, Halign = Gtk.Align.Fill, Valign = Gtk.Align.Start, Ellipsize = Pango.EllipsizeMode.End };
                    footer.MarginStart = 3;
                    footer.StyleContext.AddClass("GroupSubTitle");
                    hBox.PackEnd(footer, false, false, 0);
                }
            }
            flowBoxContainer.PackStart(hBox, true, true, 0);
            if (position > -1)
            {
                flowBoxContainer.ReorderChild(hBox, position + 1);
            }

        }
        private void _flow_ChildActivated(object o, Gtk.ChildActivatedArgs args)
        {
            ListViewItem item = this.Items[Convert.ToInt32(args.Child.Data["ItemId"])];
            if (args.Child.IsSelected && item.Selected)
            {
                item.Selected = false;
                if (args.Child.Parent is Gtk.FlowBox flow)
                    flow.UnselectChild(args.Child);
            }
            else
            {
                item.Selected = true;
                if (SelectedIndexChanged != null)
                    SelectedIndexChanged(this, args);
            }
            if (ItemActivate != null)
                ItemActivate(this, args);

            if (ItemSelectionChanged != null)
                ItemSelectionChanged(this, new ListViewItemSelectionChangedEventArgs(item, item.Index, item.Selected));

            if (Click != null)
                Click(this, args);
        }
        public void Sort()
        {
            foreach (Gtk.Box vbox in flowBoxContainer.AllChildren)
            {
                foreach (var flow in vbox.AllChildren)
                {
                    if (flow is Gtk.FlowBox _flow)
                    {
                        _flow.InvalidateSort();
                    }
                }
            }
        }
        private bool IsCacheUpdate;
        public void BeginUpdate()
        {
            IsCacheUpdate = true;
        }
        public void EndUpdate()
        {
            IsCacheUpdate = false;
            self.Window.ProcessUpdates(true);
            self.ShowAll();
        }
        public class CheckedIndexCollection : List<int>
		{
			 
			public CheckedIndexCollection(ListView owner)
			{
				 
			}

		}

		[ListBindable(false)]
		public class CheckedListViewItemCollection : List<ListViewItem>
		{
			
			public virtual ListViewItem this[string key]
			{
				get
				{
					return this.Find(w => w.Name == key);
				}
			}
			 
			public CheckedListViewItemCollection(ListView owner)
			{
				
			}

		}

		[ListBindable(false)]
		public class ColumnHeaderCollection : List<ColumnHeader>
        {
			public ColumnHeaderCollection(ListView owner)
            {
                owner.self.Realized += Self_Realized;
            }

            private void Self_Realized(object sender, EventArgs e)
            {
                this.Sort(new Comparison<ColumnHeader>((a, b) => a.DisplayIndex.CompareTo(b.DisplayIndex)));
            }

            public virtual ColumnHeader this[string key]
			{
				get
				{
					return base.Find(o => o.Name == key);
				}
			}

			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}


			public virtual void RemoveByKey(string key)
			{
				base.Remove(base.Find(o=> o.Name == key));
			}

			public virtual int IndexOfKey(string key)
			{
				return base.FindIndex(o=> o.Name == key);
			}

			public virtual ColumnHeader Add(string text, int width, HorizontalAlignment textAlign)
			{
                return Add("", text, width, textAlign, "");
            }

			public virtual ColumnHeader Add(string text)
			{
                return Add("", text, 60, HorizontalAlignment.Left, "");
            }

			public virtual ColumnHeader Add(string text, int width)
			{
                return Add("", text, width, HorizontalAlignment.Left, "");
            }

			public virtual ColumnHeader Add(string key, string text)
			{
                return Add(key, text, 60, HorizontalAlignment.Left, "");
            }

			public virtual ColumnHeader Add(string key, string text, int width)
			{
				return Add(key, text, width, HorizontalAlignment.Left, "");
            }

			public virtual ColumnHeader Add(string key, string text, int width, HorizontalAlignment textAlign, string imageKey)
			{
                ColumnHeader header = new ColumnHeader();
                header.Name = key;
                header.Text = text;
                header.Width = width;
                header.TextAlign = textAlign;
                header.ImageKey = imageKey;
                header.ImageIndex = -1;
                header._index = base.Count;
                header.DisplayIndex = header._index;
                base.Add(header);
                return header;
            }

			public virtual ColumnHeader Add(string key, string text, int width, HorizontalAlignment textAlign, int imageIndex)
			{
                ColumnHeader header = new ColumnHeader();
                header.Name = key;
                header.Text = text;
                header.Width = width;
                header.TextAlign = textAlign;
				header.ImageIndex = imageIndex;
                header._index = base.Count;
                header.DisplayIndex = header._index;
                base.Add(header);
                return header;
            }

			public virtual void AddRange(ColumnHeader[] values)
			{
                int idx = 0;
                foreach (ColumnHeader value in values)
                {
                    value._index = idx++;
                    if (value.DisplayIndex == 0)
                        value.DisplayIndex = value._index;
                }

                base.AddRange(values);
			}

			public virtual bool ContainsKey(string key)
			{
				return base.Contains(base.Find(o=>o.Name==key));
			}


			public void Insert(int index, string text, int width, HorizontalAlignment textAlign)
			{
                Insert(index, "", text, width, textAlign, null);
            }

			public void Insert(int index, string text)
			{
                Insert(index, "", text, 0, HorizontalAlignment.Center, null);
            }

			public void Insert(int index, string text, int width)
			{
                Insert(index, "", text, width, HorizontalAlignment.Center, null);
            }

			public void Insert(int index, string key, string text)
			{
                Insert(index, key, text, 0, HorizontalAlignment.Center, null);

            }

			public void Insert(int index, string key, string text, int width)
			{
				Insert(index, key, text, width, HorizontalAlignment.Center, null);
            }

			public void Insert(int index, string key, string text, int width, HorizontalAlignment textAlign, string imageKey)
			{
                ColumnHeader header = new ColumnHeader();
                header._index = index;
                header.DisplayIndex = index;
                header.Name = key;
                header.Text = text;
                header.Width = width;
                header.TextAlign = textAlign;
                header.ImageKey = imageKey;
                base.Insert(index, header);
            }

			public void Insert(int index, string key, string text, int width, HorizontalAlignment textAlign, int imageIndex)
			{
                ColumnHeader header = new ColumnHeader();
				header._index = index;
                header.DisplayIndex = index;
                header.Name = key;
                header.Text = text;
                header.Width = width;
                header.TextAlign = textAlign;
                header.ImageIndex = imageIndex;
				base.Insert(index, header);
            }
		}
        internal readonly  ListViewGroup DefaultGroup = ListViewGroup.GetDefaultListViewGroup();
        [ListBindable(false)]
		public class ListViewItemCollection : List<ListViewItem>, IList
		{
            ListView _owner;
			public virtual ListViewItem this[string key]
			{
				get
				{
					return base.Find(w => w.Name == key);
				}
			}

            public ListViewItemCollection(ListView owner)
			{
				_owner = owner;
            }

            public new void Add(ListViewItem item)
            {
                AddCore(item, -1);
            }
			public virtual ListViewItem Add(string text)
			{
				return Add("", text, -1);
            }

			public virtual ListViewItem Add(string text, int imageIndex)
			{
                return Add("", text, imageIndex);
            }

			public virtual ListViewItem Add(string text, string imageKey)
            {
                SortedSet<ListViewItem> dd = new SortedSet<ListViewItem>();

                return Add("", text, imageKey);
            }

			public virtual ListViewItem Add(string key, string text, string imageKey)
			{
				ListViewItem item = new ListViewItem(text,imageKey);
                item.Name = key;
				item.Text = text;
                AddCore(item, -1);
                return item;
            }

			public virtual ListViewItem Add(string key, string text, int imageIndex)
			{
                ListViewItem item = new ListViewItem(text,imageIndex);
                item.Name = key;
                AddCore(item, -1);
                return item;
            }
            private void AddCore(ListViewItem item, int position)
			{
                item.Index = Count;
                if (item.Group == null)
                    item.Group = _owner.DefaultGroup;

                base.Add(item);

                if (_owner.self.IsRealized)
                {
                    if (_owner.Groups.Exists(g => g.SerialGuid == item.Group.SerialGuid) == false)
                    {
                        int index = -1; 
                        if (_owner.AllowColumnReorder)
                        {
                            if (_owner.Sorting == SortOrder.Descending)
                            {
                                index = _owner.Groups.OrderByDescending(o => o.Header).ToList().FindIndex(g => g.Name == item.Group.Name);
                            }
                            else if (_owner.Sorting == SortOrder.Ascending)
                            {
                                index = _owner.Groups.OrderBy(o => o.Header).ToList().FindIndex(g => g.Name == item.Group.Name);
                            }
                        }
                        _owner.NativeGroupAdd(item.Group, index);
                    }
                    _owner.NativeAdd(item, position);
                    if (_owner.IsCacheUpdate == false)
                        _owner.self.ShowAll();
                }
            }
            public void AddRange(ListViewItem[] items)
            {
                foreach (ListViewItem item in items)
                {
                    AddCore(item, -1);
                }   
            }

			public void AddRange(ListViewItemCollection items)
			{
				 foreach(ListViewItem item in items)
                    AddCore(item,-1);
			}

			public virtual bool ContainsKey(string key)
			{
				return base.FindIndex(w => w.Name == key) > -1;
            }

			public void CopyTo(Array dest, int index)
			{
				throw null;
			}

			public ListViewItem[] Find(string key, bool searchAllSubItems)
			{
				if(searchAllSubItems)
					return base.FindAll(w => w.Name == key && w.SubItems.ContainsKey(key)).ToArray();
				else
                    return base.FindAll(w => w.Name == key).ToArray();
            }

			public virtual int IndexOfKey(string key)
			{
				return base.FindIndex(w => w.Name == key);
			}

			public ListViewItem Insert(int index, string text)
			{
                return Insert(index, "", text, -1);
            }

			public ListViewItem Insert(int index, string text, int imageIndex)
			{
                return Insert(index, "", text, imageIndex);
            }

			public ListViewItem Insert(int index, string text, string imageKey)
			{
				return Insert(index, "", text, imageKey);
            }

			public virtual ListViewItem Insert(int index, string key, string text, string imageKey)
			{
                ListViewItem item = new ListViewItem(text, imageKey);
                item.Name = key;
                base.Insert(index, item);
                return item;
            }

			public virtual ListViewItem Insert(int index, string key, string text, int imageIndex)
			{
                ListViewItem item = new ListViewItem(text, imageIndex);
                item.Name = key;
				base.Insert(index, item);
				return item;
			}

			public virtual void RemoveByKey(string key)
			{
				base.Remove(base.Find(w => w.Name == key));
			}

		}

		[ListBindable(false)]
		public class SelectedIndexCollection : List<int>
		{
			
		}

		[ListBindable(false)]
		public class SelectedListViewItemCollection : List<ListViewItem>
        {
		
		}


		public ItemActivation Activation { get; set; }

        public bool CheckBoxes { get; set; }

        public CheckedIndexCollection CheckedIndices
		{
			get
			{
                CheckedIndexCollection selecteditems = new CheckedIndexCollection(this);
                foreach (ListViewItem item in CheckedItems)
                {
                    selecteditems.Add(item.Index);
                }
                return selecteditems;
            }
		}

		public CheckedListViewItemCollection CheckedItems
		{
			get
			{
                CheckedListViewItemCollection selecteditems = new CheckedListViewItemCollection(this);
                foreach(ListViewItem item in this.Items)
                {
                    if (item.Checked)
                    {
                        selecteditems.Add(item);
                    }
                }
                return selecteditems;
            }
		}
		public ColumnHeaderCollection Columns
		{
			get
			{
				return _columns;
			}
		}

		public bool FullRowSelect { get; set; }

        public ListViewGroupCollection Groups
		{
			get
			{
                return _groups;
            }
		}

		public ListViewItemCollection Items
		{
			get
			{
                return _items;
            }
		}

		public IComparer ListViewItemSorter
		{
			get;
			set;
		}

		public SelectedIndexCollection SelectedIndices
		{
			get
			{
                SelectedIndexCollection selecteditems = new SelectedIndexCollection();
                foreach(ListViewItem item in SelectedItems)
                {
                    selecteditems.Add(item.Index);
                }
               
                return selecteditems;
            }
		}

		public SelectedListViewItemCollection SelectedItems
		{
			get
			{
				SelectedListViewItemCollection selecteditems = new SelectedListViewItemCollection();
                foreach (ListViewItem item in this.Items)
                {
                    if (item.Selected)
                    {
                        selecteditems.Add(item);
                    }
                }
                return selecteditems;
            }
		}

		public void Clear()
		{
            foreach (Gtk.Box vbox in flowBoxContainer.AllChildren)
            {
                foreach (var flow in vbox.AllChildren)
                {
                    if (flow is Gtk.FlowBox _flow)
                    {
                        foreach (Gtk.FlowBoxChild child in _flow.Children)
                            _flow.Remove(child);
                    }
                }
                flowBoxContainer.Remove(vbox);
            }
            Items.Clear();
		}


		public ListViewItem FindItemWithText(string text)
		{
            return Items.Find(w => w.Text == text);
        }

		public ListViewItem FindItemWithText(string text, bool includeSubItemsInSearch, int startIndex)
		{
            int idx = Items.FindIndex(startIndex, w => w.Text == text);
            return idx == -1 ? null : Items[idx];
        }

		public ListViewItem FindItemWithText(string text, bool includeSubItemsInSearch, int startIndex, bool isPrefixSearch)
		{
            int idx = Items.FindIndex(startIndex, w => w.Text == text);
            return idx == -1 ? null : Items[idx];
        }

		public ListViewItem GetItemAt(int x, int y)
		{
			throw null;
		}

		public Drawing.Rectangle GetItemRect(int index)
		{
			throw null;
		}

        public event ColumnClickEventHandler ColumnClick;
        public event ColumnReorderedEventHandler ColumnReordered;
        public event ItemCheckEventHandler ItemCheck;
        public event ItemCheckedEventHandler ItemChecked;
        public event ListViewItemSelectionChangedEventHandler ItemSelectionChanged;
		public event EventHandler SelectedIndexChanged;
		public override event EventHandler Click;
		public event EventHandler ItemActivate;
    }
}
