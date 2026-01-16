/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using Pango;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

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
        internal Gtk.ScrolledWindow scrolledWindow = new Gtk.ScrolledWindow();
        internal Gtk.Box flowBoxContainer = new Gtk.Box(Gtk.Orientation.Vertical, 0);
        internal Gtk.Box header = new Gtk.Box(Gtk.Orientation.Horizontal, 0);
        internal Gtk.Layout headerView;
        private int __headerheight = 30;
        public ListView() : base()
        {
            self.Override.sender = this;
            _items = new ListViewItemCollection(this);
            _groups = new ListViewGroupCollection(this);
            _columns = new ColumnHeaderCollection(this);
            self.Realized += Control_Realized;

            header.StyleContext.AddClass("ListViewHeader");
            header.Spacing = 0;
            header.BorderWidth = 0;
            header.Halign = Gtk.Align.Fill;
            header.Valign = Gtk.Align.Fill;
            header.HeightRequest = 1;
            header.Homogeneous = false;
            flowBoxContainer.Halign = Gtk.Align.Fill;
            flowBoxContainer.Valign = Gtk.Align.Start;
            scrolledWindow.Halign = Gtk.Align.Fill;
            scrolledWindow.Valign = Gtk.Align.Fill;
            scrolledWindow.Add(flowBoxContainer);
            scrolledWindow.Hadjustment.ValueChanged += Hadjustment_ValueChanged;
            headerView = new Gtk.Layout(null, null);
            headerView.Halign = Gtk.Align.Fill;
            headerView.Valign = Gtk.Align.Start;
            headerView.Hexpand = true;
            headerView.Vexpand = false;
            headerView.HeightRequest = 1;// __headerheight;
            headerView.Width = 100000;
            headerView.Add(header);
            self.box.PackStart(headerView, false, true, 0);
            self.box.PackStart(scrolledWindow, true, true, 0);
            this.BorderStyle = BorderStyle.Fixed3D;
        }
        private void Hadjustment_ValueChanged(object sender, EventArgs e)
        {
            headerView.Hadjustment.Value = scrolledWindow.Hadjustment.Value;
        }

        private bool Is_Control_Realized = false;
        private void Control_Realized(object sender, EventArgs e)
        {
            if (!Is_Control_Realized)
            {
                Is_Control_Realized = true;
                if (this.View == View.Details)
                {
                    header.NoShowAll = false;
                    header.Visible = true;
                    header.ShowAll();
                    headerView.HeightRequest = __headerheight;
                }
                foreach (ColumnHeader cheader in this.Columns)
                {
                    NativeHeaderAdd(cheader);
                }
                foreach (ListViewGroup g in Groups)
                {
                    NativeGroupAdd(g, -1);
                }
                foreach (ListViewItem item in Items)
                {
                    NativeAdd(item, -1);
                }

                MultiSelect = _MultiSelect == true;
                self.ShowAll();
            }
        }
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
        private bool _MultiSelect = true;
        public virtual bool MultiSelect
        {
            get
            {
                return _MultiSelect;
            }
            set
            {
                _MultiSelect = value;
                foreach (var group in GetAllGroups())
                {
                    if (value == true)
                    {
                        group.FlowBox.SelectionMode = Gtk.SelectionMode.Multiple;
                    }
                    else
                    {
                        group.FlowBox.SelectionMode = Gtk.SelectionMode.Single;
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
        internal int FontSize
        {
            get
            {
                if (_fontSize < 5)
                {
                    if (Font != null && Font.Size > 5)
                    {
                        if (Font.Unit == GraphicsUnit.Point)
                            _fontSize = (int)Font.Size * 96 / 72;
                        else
                            _fontSize = (int)Font.Size;
                    }
                    else
                    {
                        _fontSize = 12;
                    }
                }
                return _fontSize;
            }
        }

        public void Clear()
        {
            Items.Clear();
            Groups.Clear();
        }
        internal void NativeItemsClear()
        {
            foreach (var group in GetAllGroups())
            {
                foreach (Gtk.FlowBoxChild child in group.FlowBox.Children)
                    group.FlowBox.Remove(child);
            }
        }
        internal void NativeGroupsClear()
        {
            foreach (var group in flowBoxContainer.Children)
            {
                flowBoxContainer.Remove(group);
                group.Dispose();
            }
            _defaultGroup?.FlowBox?.Destroy();
            _defaultGroup=null; 
        }
        internal void NativeHeaderClear()
        {
            Clear();
            foreach (var col in header.Children)
            {
                header.Remove(col);
                col.Destroy();
            }
            header.Hide();
        }
        internal void NativeHeaderRemove(ColumnHeader column)
        {
            Clear();
            if (column.DisplayIndex < header.Children.Length)
            {
                var headercol = header.Children[column.DisplayIndex];
                header.Remove(headercol);
                headercol.Destroy();
            }
        }
        internal void NativeHeaderAdd(ColumnHeader col)
        {
            if (self.IsRealized)
            {
                LabelBase label = new LabelBase(col.Text) { Xalign = 0, Xpad = 5, WidthRequest = col.Width, MaxWidthChars = 0, Halign = Gtk.Align.Start, Valign = Gtk.Align.End, Ellipsize = Pango.EllipsizeMode.End, Wrap = false, LineWrap = false };
                label.TooltipText = col.Text;
                label.Markup = col.Text;
                label.Name = $"column_t_{col.Index}";
                label.Drawn += Label_Drawn;
                var columbt = new Gtk.Button(label) { MarginStart = 0, WidthRequest = col.Width, HeightRequest = __headerheight, Halign = Gtk.Align.Start, Valign = Gtk.Align.Fill };
                columbt.Name = $"column_b_{col.Index}";
                columbt.ActionTargetValue = new GLib.Variant(col.Index);
                columbt.Clicked += Columbt_Clicked;
                header.PackStart(columbt, false, false, 0);

                header.ReorderChild(columbt, col.Index);

                if (this.View == View.Details)
                {
                    headerView.HeightRequest = __headerheight;
                    headerView.ShowAll();
                }
                else
                {
                    headerView.HeightRequest = 1;
                }
            }
        }

        private void Label_Drawn(object o, DrawnArgs args)
        {
            Cairo.Rectangle rec = args.Cr.ClipExtents();
            if (rec.Width > 10 && SortColumn != null)
            {
                LabelBase ws = (LabelBase)o;
                if (ws.Name == $"column_t_{SortColumn.Index}")
                {
                    if (Sorting != SortOrder.None)
                    {
                        Cairo.Context ctx = args.Cr;
                        ctx.Save();
                        ctx.ResetClip();
                        ctx.Rectangle(rec.X, rec.Y - 4, rec.Width, rec.Height);
                        ctx.Clip();
                        ctx.Translate((int)rec.Width / 2 - 6, rec.Y - 5);
                        ctx.Rotate(0.5 * Math.PI);
                        Gdk.RGBA color = ws.StyleContext.GetColor(StateFlags.Normal);
                        ctx.SetSourceRGB(color.Red, color.Green, color.Blue);
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

        private void Columbt_Clicked(object sender, EventArgs e)
        {
            Gtk.Button btn = (Gtk.Button)sender;
            if (this.HeaderStyle == ColumnHeaderStyle.Clickable)
            {
                int actioncolumn = (int)btn.ActionTargetValue;
                if (SelectedColumnIndex != actioncolumn)
                    Sorting = SortOrder.None;
                SelectedColumnIndex = actioncolumn;
                SortColumn = this.Columns[actioncolumn];
                if (Sorting == SortOrder.Ascending)
                    Sorting = SortOrder.Descending;
                else if (Sorting == SortOrder.Descending)
                    Sorting = SortOrder.None;
                else
                    Sorting = SortOrder.Ascending;

                this.Sort();

                if (ColumnClick != null)
                    ColumnClick(this, new ColumnClickEventArgs((int)btn.ActionTargetValue));
            }
        }
        private int SelectedColumnIndex = -1;
        public ColumnHeader SortColumn { set; get; }
        public void Sort()
        {
            if (SortColumn != null)
            {
                foreach (var group in GetAllGroups())
                {
                    group.FlowBox.InvalidateSort();
                }
            }
        }
        internal void NativeUpdateText(ListViewItem item, string text)
        {
            if (item._flowBoxChild != null && item._flowBoxChild.Parent is Gtk.FlowBox flowBox)
            {
                Gtk.Box box = item._flowBoxChild.Child as Gtk.Box;
                if (this.View == View.Details)
                {
                    Gtk.Viewport viewport = box.Children[0] as Gtk.Viewport;
                    Gtk.Layout layout = viewport.Child as Gtk.Layout;
                    foreach (var lab in layout.Children)
                    {
                        if (lab is Gtk.Label label)
                        {
                            label.Text = text;
                            break;
                        }
                    }
                }
                else if (this.View == View.SmallIcon)
                {
                    foreach (var lab in box.Children)
                    {
                        if (lab is Gtk.Label label)
                        {
                            label.Text = text;
                            break;
                        }
                    }
                }
                else if (this.View == View.LargeIcon)
                {
                    Gtk.Box vbox = box.Children[0] as Gtk.Box;
                    foreach (var lab in vbox.Children)
                    {
                        if (lab is Gtk.Label label)
                        {
                            label.Text = text;
                            break;
                        }
                    }
                }
                else if (this.View == View.List)
                {
                    foreach (var lab in box.Children)
                    {
                        if (lab is Gtk.Label label)
                        {
                            label.Text = text;
                            break;
                        }
                    }
                }
            }
        }
        internal void NativeSelectItem(ListViewItem item, bool isselected)
        {
            if (item._flowBoxChild != null && item._flowBoxChild.Parent is Gtk.FlowBox flowBox)
            {
                if (isselected == true)
                    flowBox.SelectChild(item._flowBoxChild);
                else
                    flowBox.UnselectChild(item._flowBoxChild);
            }
        }
        internal void NativeCheckItem(ListViewItem item, bool ischecked)
        {
            if (item._flowBoxChild != null && item._flowBoxChild.IsRealized && item._flowBoxChild.Parent is Gtk.FlowBox flowBox)
            {
                Gtk.Box box = item._flowBoxChild.Child as Gtk.Box;
                if (this.View == View.Details)
                {
                    Gtk.Viewport viewport = box.Children[0] as Gtk.Viewport;
                    Gtk.Layout layout = viewport.Child as Gtk.Layout;
                    foreach (var chk in layout.Children)
                    {
                        if (chk is Gtk.CheckButton chkbutton)
                        {
                            chkbutton.Active = ischecked;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var chk in box.Children)
                    {
                        if (chk is Gtk.CheckButton chkbutton)
                        {
                            chkbutton.Active = ischecked;
                            break;
                        }
                    }
                }
            }
        }
        internal void NativeAdd(ListViewItem item, int position)
        {
            if (self.IsRealized)
            {
                int padding = 5;
               
                Gtk.FlowBoxChild flowitem = new Gtk.FlowBoxChild();
                flowitem.TooltipText = item.Text;
                flowitem.Data.Add("ItemId", item.Index);
                flowitem.Halign = Gtk.Align.Start;
                flowitem.Valign = Gtk.Align.Start;
                flowitem.BorderWidth = 0;
                flowitem.Margin = 0;
                flowitem.HeightRequest = FontSize + 10;
                item._flowBoxChild = flowitem;
                BoxBase hBox = new BoxBase(Gtk.Orientation.Horizontal, 0);
                if (item.BackColor.HasValue)
                {
                    hBox.Override.BackColor = Drawing.Color.FromArgb(180, item.BackColor.Value);
                }
                hBox.Valign = Gtk.Align.Fill;
                hBox.Halign = Gtk.Align.Fill;
                hBox.BorderWidth = 0;
                hBox.Spacing = 0;
                hBox.Homogeneous = false;

                foreach (ColumnHeader col in Columns)
                {
                    if (item.SubItems != null && item.SubItems.Count > col.Index)
                    {
                        flowitem.Data.Add(col.Index, item.SubItems[col.Index].Text);
                    }
                    else
                        flowitem.Data.Add(col.Index, string.Empty);
                }

                flowitem.Add(hBox);

                Gtk.FlowBox flowBox = DefaultGroup.FlowBox;
                if (this.ShowGroups == true && this.View != View.List && this.View != View.Tile)
                {
                    if (Groups.Exists(g => g.Name == item.Group.Name))
                        flowBox = item.Group.FlowBox;
                    else
                        NativeGroupAdd(DefaultGroup, -1);
                }
                else
                {
                    NativeGroupAdd(DefaultGroup, -1);
                }

                if (position == -1)
                    flowBox.Add(flowitem);
                else
                    flowBox.Insert(flowitem, position);

                Gtk.Overlay fistcell = new Gtk.Overlay();
                fistcell.Halign = Gtk.Align.Fill;
                fistcell.Valign = Gtk.Align.Fill;
                fistcell.BorderWidth = 1;
                if (Columns.Count > 0)
                    fistcell.WidthRequest = Columns[0].Width;
                else
                    fistcell.WidthRequest = Width - padding - padding;
                CheckBox checkBox = new CheckBox();
                Gtk.CheckButton checkboxself = checkBox.self;
                checkboxself.Halign = Gtk.Align.Start;
                checkboxself.Valign = Gtk.Align.Center;
                checkboxself.BorderWidth = 0;
                checkboxself.Relief = ReliefStyle.None;
                checkboxself.StyleContext.AddClass("ButtonNone");
                checkboxself.Label = item.Text;
                checkboxself.Active = item.Checked;
                checkboxself.DrawIndicator = this.CheckBoxes;
                checkboxself.WidgetEvent += Checkboxself_WidgetEvent;
                if (checkboxself.Child is Gtk.Label label)
                {
                    label.Halign = Gtk.Align.Start;
                    label.Valign = Gtk.Align.Start;
                    if (item.ForeColor.HasValue)
                    {
                        label.Attributes = new Pango.AttrList();
                        Pango.AttrForeground fg = new Pango.AttrForeground(Convert.ToUInt16(item.ForeColor.Value.R * 257), Convert.ToUInt16(item.ForeColor.Value.G * 257), Convert.ToUInt16(item.ForeColor.Value.B * 257));
                        label.Attributes.Insert(fg);
                    }
                }

                if (fistcell.WidthRequest == -1)
                    fistcell.Add(checkboxself);
                else
                    fistcell.AddOverlay(checkboxself);

                checkboxself.Xalign = 0.0f;
                checkboxself.Yalign = 0.5f;
                Gtk.Viewport viewport = new Viewport();
                viewport.BorderWidth = 0;
                viewport.Add(fistcell);
                hBox.PackStart(viewport, true, true, 0);

                if (this.View == View.Details)
                {
                    header.Visible = true;
                    if (this.SmallImageList != null)
                    {
                        int width = this.SmallImageList.ImageSize.Width;
                        int height = this.SmallImageList.ImageSize.Height;
                        if (height > flowitem.HeightRequest)
                            flowitem.HeightRequest = height;

                        checkboxself.AlwaysShowImage = true;
                        checkboxself.ImagePosition = PositionType.Left;
                        if (!string.IsNullOrWhiteSpace(item.ImageKey))
                        {
                            Drawing.Image img = this.SmallImageList.GetBitmap(item.ImageKey);
                            if (img != null)
                            {
                                checkboxself.Image = new Gtk.Image(img.Pixbuf) { WidthRequest = width, HeightRequest = height, Halign = Align.Start };
                            }
                        }
                        else if (item.ImageIndex > -1)
                        {
                            Drawing.Image img = this.SmallImageList.GetBitmap(item.ImageIndex);
                            if (img != null)
                            {
                                checkboxself.Image = new Gtk.Image(img.Pixbuf) { WidthRequest = width, HeightRequest = height, Halign = Align.Start };
                            }
                        }
                    }
                    int index = 0;
                    foreach (ColumnHeader col in Columns)
                    {
                        if (index > 0)
                        {
                            Gtk.Overlay sublayout = new Gtk.Overlay();
                            sublayout.Halign = Gtk.Align.Fill;
                            sublayout.Valign = Gtk.Align.Fill;
                            sublayout.WidthRequest = col.Width;
                            if (item.SubItems != null && item.SubItems.Count > index)
                            {
                                ListViewItem.ListViewSubItem subitem = item.SubItems[index];
                                Gtk.Label sublabel = new Gtk.Label();
                                subitem._label = sublabel;

                                if (subitem.ForeColor.HasValue)
                                {
                                    sublabel.Attributes = new Pango.AttrList();
                                    Pango.AttrForeground fg = new Pango.AttrForeground(Convert.ToUInt16(subitem.ForeColor.Value.R * 257), Convert.ToUInt16(subitem.ForeColor.Value.G * 257), Convert.ToUInt16(subitem.ForeColor.Value.B * 257));
                                    sublabel.Attributes.Insert(fg);
                                }
                                sublabel.WidthChars = 0;
                                sublabel.MaxWidthChars = 0;
                                sublabel.Halign = Gtk.Align.Fill;
                                sublabel.Valign = Gtk.Align.Center;
                                sublabel.MarginStart = padding;
                                sublabel.Xalign = 0.0f;
                                sublabel.Yalign = 0.5f;
                                sublabel.Ellipsize = Pango.EllipsizeMode.End;
                                sublabel.Text = subitem.Text;
                                sublayout.AddOverlay(sublabel);
                            }
                            Gtk.Viewport sviewport = new Viewport();
                            sviewport.BorderWidth = 0;
                            sviewport.Add(sublayout);
                            hBox.PackStart(sviewport, true, true, 0);
                        }
                        index++;
                    }
                }
                else
                {
                    header.Visible = false;
                    if (this.View == View.LargeIcon)
                    {
                        if (this.LargeImageList != null)
                        {
                            int width = this.LargeImageList.ImageSize.Width;
                            int height = this.LargeImageList.ImageSize.Height;
                            flowitem.WidthRequest = width + 40;
                            flowitem.HeightRequest = height + FontSize + 20;

                            checkboxself.AlwaysShowImage = true;
                            checkboxself.ImagePosition = PositionType.Top;
                            if (!string.IsNullOrWhiteSpace(item.ImageKey))
                            {
                                Drawing.Image img = this.LargeImageList.GetBitmap(item.ImageKey);
                                if (img != null)
                                {
                                    checkboxself.Image = new Gtk.Image(img.Pixbuf) { WidthRequest = width, HeightRequest = height, Halign = Align.Start };
                                }
                            }
                            else if (item.ImageIndex > -1)
                            {
                                Drawing.Image img = this.LargeImageList.GetBitmap(item.ImageIndex);
                                if (img != null)
                                {
                                    checkboxself.Image = new Gtk.Image(img.Pixbuf) { WidthRequest = width, HeightRequest = height, Halign = Align.Start };
                                }
                            }
                        }
                    }
                    else
                    {
                        if (this.SmallImageList != null)
                        {
                            int width = this.SmallImageList.ImageSize.Width;
                            int height = this.SmallImageList.ImageSize.Height;
                            if (height > flowitem.HeightRequest)
                                flowitem.HeightRequest = height;

                            checkboxself.AlwaysShowImage = true;
                            checkboxself.ImagePosition = PositionType.Left;
                            if (!string.IsNullOrWhiteSpace(item.ImageKey))
                            {
                                Drawing.Image img = this.SmallImageList.GetBitmap(item.ImageKey);
                                if (img != null)
                                {
                                    checkboxself.Image = new Gtk.Image(img.Pixbuf) { WidthRequest = width, HeightRequest = height, Halign = Align.Start };
                                }
                            }
                            else if (item.ImageIndex > -1)
                            {
                                Drawing.Image img = this.SmallImageList.GetBitmap(item.ImageIndex);
                                if (img != null)
                                {
                                    checkboxself.Image = new Gtk.Image(img.Pixbuf) { WidthRequest = width, HeightRequest = height, Halign = Align.Start };
                                }
                            }
                        }
                    }
                }
                if (IsCacheUpdate == false)
                    self.ShowAll();
            }
        }
        private void Checkboxself_WidgetEvent(object o, WidgetEventArgs args)
        {
            if (args.Event.Type is Gdk.EventType.ButtonRelease && args.Event is Gdk.EventButton event2 && event2.Button == 1)
            {
                Gtk.CheckButton checkButton = (Gtk.CheckButton)o;
                checkButton.Active = !checkButton.Active;
                if (event2.X < 25)
                {
                    OnCheckedChanged(checkButton.Data["Control"]);
                    args.RetVal = true;
                }
            }
        }
        private void OnCheckedChanged(object? sender)
        {
            CheckBox box = sender as CheckBox;
            Gtk.FlowBoxChild checkitem = box.self.Parent.Parent.Parent.Parent as Gtk.FlowBoxChild;
            ListViewItem thisitem = this.Items.Find(m => m._flowBoxChild.Handle == checkitem.Handle);
            if (thisitem != null)
            {
                thisitem.Checked = box.self.Active;
                if (ItemCheck != null)
                {
                    ItemCheck(sender, new ItemCheckEventArgs(checkitem.Index, box.self.Active ? CheckState.Checked : CheckState.Unchecked, box.self.Active ? CheckState.Unchecked : CheckState.Checked));
                }
                if (ItemChecked != null && box.self.Active == true)
                {
                    ItemChecked(sender, new ItemCheckedEventArgs(thisitem));
                }
            }
        }
        internal void NativeGroupAdd(ListViewGroup group, int position)
        {
            if (self.IsRealized)
            {
                if (group == null)
                    return;

                if (position == -1 && Groups.Exists(g => g.SerialGuid == group.SerialGuid) == false)
                {
                    if (Sorting == SortOrder.Descending)
                    {
                        position = Groups.OrderByDescending(o => o.Header).ToList().FindIndex(g => g.Name == group.Name);
                    }
                    else if (Sorting == SortOrder.Ascending)
                    {
                        position = Groups.OrderBy(o => o.Header).ToList().FindIndex(g => g.Name == group.Name);
                    }
                }

                Gtk.FlowBox _flow = DefaultGroup.FlowBox;
                Gtk.Box hBox = new Gtk.Box(Gtk.Orientation.Vertical, 0);
                hBox.Valign = Gtk.Align.Start;
                hBox.Halign = Gtk.Align.Fill;
                if (ShowGroups == true && this.View != View.List && this.View != View.Tile)
                {
                    _flow = group.FlowBox;
                    if (_flow.Parent != null)
                        return;

                    Gtk.Box groupbox = new Box(Gtk.Orientation.Horizontal, 0);
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
                    group._groupbox = groupbox;
                    if (Groups.Count == 0)
                    {
                        if (group.Name == ListViewGroup.defaultListViewGroupKey)
                        {
                            groupbox.NoShowAll = true;
                            groupbox.Visible = false;
                        }
                        if (this.View == View.Details)
                            _flow.StyleContext.AddClass("GridBorder");
                    }
                    else
                    {
                        if (DefaultGroup._groupbox != null && DefaultGroup._groupbox is Gtk.Box defbox)
                        {
                            defbox.NoShowAll = false;
                            defbox.Visible = true;
                        }
                        if (this.View == View.Details)
                            _flow.StyleContext.AddClass("GridBorder");
                    }

                    hBox.PackStart(groupbox, false, false, 0);
                    if (!string.IsNullOrEmpty(group.Subtitle))
                    {
                        var subtitle = new Gtk.Label(group.Subtitle) { Xalign = 0, Halign = Gtk.Align.Fill, Valign = Gtk.Align.Start, Ellipsize = Pango.EllipsizeMode.End };
                        subtitle.MarginStart = 3;
                        subtitle.MarginBottom = 2;
                        subtitle.StyleContext.AddClass("GroupSubTitle");
                        hBox.PackStart(subtitle, false, false, 0);
                    }
                }
                if (_flow.Parent != null)
                    return;

                _flow.Name = group.Name;
                _flow.ColumnSpacing = 0;
                _flow.RowSpacing = 0;
                _flow.BorderWidth = 0;
                _flow.Homogeneous = false;
                _flow.Orientation = Gtk.Orientation.Horizontal;
                _flow.MaxChildrenPerLine = 50u;
                _flow.MinChildrenPerLine = 0u;
                _flow.Halign = Gtk.Align.Fill;
                _flow.Valign = Gtk.Align.Start;
                _flow.SelectionMode = _MultiSelect == false ? Gtk.SelectionMode.Single : Gtk.SelectionMode.Multiple;
                _flow.ActivateOnSingleClick = _MultiSelect == false;
                _flow.SortFunc = new Gtk.FlowBoxSortFunc((fbc1, fbc2) =>
                {
                    if (SortColumn != null)
                    {
                        if (this.Sorting == SortOrder.Ascending)
                            return fbc2.Data[SortColumn.Index].ToString().CompareTo(fbc1.Data[SortColumn.Index].ToString());
                        else if (this.Sorting == SortOrder.Descending)
                            return fbc1.Data[SortColumn.Index].ToString().CompareTo(fbc2.Data[SortColumn.Index].ToString());
                        else
                            return fbc2.Index.CompareTo(fbc1.Index);
                    }
                    else
                        return 0;
                });
                _flow.SelectedChildrenChanged += _flow_SelectedChildrenChanged;
                _flow.ButtonReleaseEvent += _flow_ButtonReleaseEvent;
                hBox.PackStart(_flow, false, true, 0);

                if (ShowGroups == true && this.View != View.List && this.View != View.Tile)
                {
                    if (!string.IsNullOrEmpty(group.Footer))
                    {
                        var footer = new Gtk.Label(group.Footer) { Xalign = 0, Halign = Gtk.Align.Fill, Valign = Gtk.Align.Start, Ellipsize = Pango.EllipsizeMode.End };
                        footer.MarginStart = 3;
                        footer.MarginTop = 2;
                        footer.StyleContext.AddClass("GroupSubTitle");
                        hBox.PackEnd(footer, false, false, 0);
                    }
                }
                flowBoxContainer.PackStart(hBox, false, true, 0);
                if (position > -1)
                {
                    flowBoxContainer.ReorderChild(hBox, position + 1);
                }
                if (self.IsRealized && Groups.Count > 0)
                {
                    hBox.ShowAll();
                    DefaultGroup?._groupbox?.ShowAll();
                }
            }
        }

        private void _flow_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
        {
            Gtk.FlowBox widget = (Gtk.FlowBox)o;
            if (_MultiSelect == false || ModifierKeys.HasFlag(Keys.Control) == false)
            {
                foreach (var group in GetAllGroups())
                {
                    if (group.FlowBox.Handle != widget.Handle)
                        group.FlowBox.UnselectAll();
                }
            }
            if (ItemActivate != null)
                ItemActivate(this, EventArgs.Empty);
        }
        private void _flow_SelectedChildrenChanged(object sender, EventArgs e)
        {
            Gtk.FlowBox widget = (Gtk.FlowBox)sender;
            List<IntPtr> selecteds = new List<IntPtr>();
            foreach (var group in GetAllGroups())
            {
                foreach (FlowBoxChild o in group.FlowBox.SelectedChildren)
                {
                    selecteds.Add(o.Handle);
                }
            }
            foreach (ListViewItem item in Items)
            {
                if (selecteds.Contains(item._flowBoxChild.Handle))
                {
                    if (item._selected == false)
                    {
                        item._selected = true;
                        if (ItemSelectionChanged != null)
                            ItemSelectionChanged(this, new ListViewItemSelectionChangedEventArgs(item, item.Index, item.Selected));
                    }
                }
                else if (item._selected == true)
                {
                    item._selected = false;
                    if (ItemSelectionChanged != null)
                        ItemSelectionChanged(this, new ListViewItemSelectionChangedEventArgs(item, item.Index, item.Selected));
                }
            }
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);
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
        private ListViewGroup _defaultGroup;
        internal ListViewGroup DefaultGroup
        {
            get
            {
                if (_defaultGroup == null)
                {
                    _defaultGroup = ListViewGroup.CreateDefaultListViewGroup();
                }
                return _defaultGroup;
            }
        }
        private IEnumerable<ListViewGroup> GetAllGroups()
        {
            if (_defaultGroup == null)
                return Groups;
            else
                return Groups.Union(new ListViewGroup[] { _defaultGroup });
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
            ListView _owner;
            public ColumnHeaderCollection(ListView owner)
            {
                _owner = owner;
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
                ColumnHeader column = base.Find(o => o.Name == key);
                base.Remove(column);
                _owner.NativeHeaderRemove(column);
            }
            public new void Clear()
            {
                base.Clear();
                _owner.NativeHeaderClear();
            } 
            public virtual int IndexOfKey(string key)
            {
                return base.FindIndex(o => o.Name == key);
            }
            public new virtual int Add(ColumnHeader header)
            {
                header._index = Count;
                if (header.DisplayIndex == 0)
                    header.DisplayIndex = header._index;
                int index= ((IList)this).Add(header);
                _owner.NativeHeaderAdd(header);
                return index;
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
                this.Add(header);
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
                this.Add(header);
                return header;
            }

            public virtual void AddRange(ColumnHeader[] values)
            {
                foreach (ColumnHeader value in values)
                {
                    this.Add(value);
                }
            }

            public virtual bool ContainsKey(string key)
            {
                return base.Contains(base.Find(o => o.Name == key));
            }

            public new void Insert(int index, ColumnHeader header)
            {
                header._index = index;
                if (header.DisplayIndex == 0)
                    header.DisplayIndex = header._index;

                base.Insert(index, header);
                int idx = 0;
                foreach(var item in this)
                {
                    header._index = idx;
                    if (header.DisplayIndex == 0)
                        header.DisplayIndex = header._index;
                    idx++;
                }
                _owner.NativeHeaderAdd(header);
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
                Insert(index, header);
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
                Insert(index, header);
            }
        }

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
                return Add("", text, imageKey);
            }

            public virtual ListViewItem Add(string key, string text, string imageKey)
            {
                ListViewItem item = new ListViewItem(text, imageKey);
                item.Name = key;
                item.Text = text;
                AddCore(item, -1);
                return item;
            }

            public virtual ListViewItem Add(string key, string text, int imageIndex)
            {
                ListViewItem item = new ListViewItem(text, imageIndex);
                item.Name = key;
                AddCore(item, -1);
                return item;
            }
            private void AddCore(ListViewItem item, int position)
            {
                item._listView = _owner;
                item.Index = Count;
                if (item.Group == null)
                    item.Group = _owner.DefaultGroup;

                base.Add(item);
                _owner.NativeAdd(item, position);
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
                foreach (ListViewItem item in items)
                    AddCore(item, -1);
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
                if (searchAllSubItems)
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
            public new void RemoveAt(int index)
            {
                if (index < Count && index >= 0)
                {
                    ListViewItem item = this[index];
                    Remove(item);
                }
            }
            public new void Remove(ListViewItem item)
            {
                if (item != null)
                {
                    if (item._flowBoxChild?.Parent is Gtk.FlowBox flow)
                    {
                        flow.Remove(item._flowBoxChild);
                        item._flowBoxChild.Destroy();
                    }
                    base.Remove(item);
                }
            }
            public virtual void RemoveByKey(string key)
            {
                ListViewItem item = base.Find(w => w.Name == key);
                if (item != null)
                    Remove(item);
            }
            public new void Clear()
            {
                if (_owner != null)
                    _owner.NativeItemsClear();

                foreach (var item in this)
                {
                    item.SubItems.Clear();
                }
                base.Clear();
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
                foreach (ListViewItem item in this.Items)
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
                foreach (ListViewItem item in SelectedItems)
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
            foreach (Gtk.Box vbox in flowBoxContainer.Children)
            {
                foreach (var flow in vbox.Children)
                {
                    if (flow is Gtk.FlowBox _flow)
                    {
                        int top = _flow.Allocation.Top + __headerheight - (int)scrolledWindow.Vadjustment.Value;
                        FlowBoxChild child = _flow.GetChildAtPos(x + (int)scrolledWindow.Hadjustment.Value, y - top);
                        if (child != null)
                        {
                            return this.Items.Find(m => m.Index == Convert.ToInt32(child.Data["ItemId"]));
                        }
                    }
                }
            }
            return null;
        }
        internal void GetSubItemAt(int x, int y, out int iItem, out int iSubItem)
        {
            iItem = -1;
            iSubItem = -1;
            int position = -(int)headerView.Hadjustment.Value;
            for (int i = 0; i < Columns.Count; i++)
            {
                if (position < x && position + Columns[i].Width > x)
                {
                    iSubItem = i;
                    break;
                }
                position += Columns[i].Width;
            }
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
        public event EventHandler ItemActivate;
    }
}
