/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GLib;
using Gtk;
using System.Collections;
using System.ComponentModel;

using System.Drawing;
using Size = System.Drawing.Size;

namespace System.Windows.Forms;

[DefaultEvent("SelectedIndexChanged")]
public class ListView : ContainerControl
{
    public readonly ListViewBase self = new();
    public override object GtkControl => self;
    private readonly ListViewItemCollection _items;
    private readonly ListViewGroupCollection _groups;
    private readonly ColumnHeaderCollection _columns;
    internal ScrolledWindow scrolledWindow = new();
    internal Box flowBoxContainer = new(Gtk.Orientation.Vertical, 0);
    internal Box header = new(Gtk.Orientation.Horizontal, 0);
    internal Gtk.Layout headerView;
    private readonly int headerheight = 30;
    public ListView()
    {
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
            // header.WidthRequest = 500;
            header.Homogeneous = false;
            flowBoxContainer.Halign = Gtk.Align.Fill;
            flowBoxContainer.Valign = Gtk.Align.Start;
            scrolledWindow.Halign = Gtk.Align.Fill;
            scrolledWindow.Valign = Gtk.Align.Fill;
            scrolledWindow.Add(flowBoxContainer);
            scrolledWindow.OverlayScrolling = false;
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

    public override void Dispose()
    {
        base.Dispose();
        IsDisposed = true;
        foreach (var column in Columns)
        {
            column.ImageList = null;
            column.Index = -1;
        }
    }

    private void Hadjustment_ValueChanged(object? sender, EventArgs e)
    {
        headerView.Hadjustment.Value = scrolledWindow.Hadjustment.Value;
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
                    headerView.HeightRequest=__headerheight;
                }
                foreach (ColumnHeader header in this.Columns)
                {
                    NativeHeaderAdd(header);
                }
                foreach (ListViewGroup g in Groups)
                {
                    NativeGroupAdd(g, -1);
                }
                foreach (ListViewItem item in Items)
                {
                    NativeAdd(item, -1);
                }

                MultiSelect = MultiSelect == true;
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
    public ImageList? LargeImageList { get; set; }
    private bool multiSelect = true;
    public virtual bool MultiSelect
    {
        get => multiSelect;
        set
        {
            multiSelect = value;
            foreach (var group in GetAllGroups())
            {
                if (value)
                {
                    group.flowBox.SelectionMode = Gtk.SelectionMode.Multiple;
                }
                else
                {
                    group.flowBox.SelectionMode = Gtk.SelectionMode.Single;
                }
            }
        }
    }

    public bool OwnerDraw { get; set; }
    public bool Scrollable { get; set; }
    public bool ShowGroups { get; set; } = true;
    public bool ShowItemToolTips { get; set; }

    public ImageList? SmallImageList
    {
        get => smallImageList;
        set => smallImageList = value;
    }

    public ImageList? StateImageList { get; set; }

    public bool UseCompatibleStateImageBehavior { get; set; }
    public View View { get; set; }
    private int _fontSize;
    protected int FontSize
    {
        get
        {
            if (_fontSize < 5)
            {
                if (Font?.Size is > 5)
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
                label.Data.Add("ColumnIndex", col.Index);
                label.Override.DrawnBackground += Override_DrawnBackground;
                var columbt = new Gtk.Button(label) { MarginStart = 0, WidthRequest = col.Width, HeightRequest = __headerheight, Halign = Gtk.Align.Start, Valign = Gtk.Align.Fill };
                columbt.Name = col.Name;
                columbt.ActionTargetValue = new GLib.Variant(col.Index);
                columbt.Data.Add("ColumnIndex", col.Index);
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
        private int SortingColumnIndex = -1;
        private void Columbt_Clicked(object sender, EventArgs e)
        {
            //Console.WriteLine(((Gtk.Widget)sender).AllocatedWidth);
            Gtk.Button btn = (Gtk.Button)sender;

            if (this.HeaderStyle == ColumnHeaderStyle.Clickable)
            {
                int actioncolumn = (int)btn.ActionTargetValue;
                if (this.Sorted == true)
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
                    this.Sort();

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
            }
            else if (View == View.SmallIcon)
            {
                if (box != null)
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
            else if (View == View.LargeIcon)
            {
                var vbox = box?.Children[0] as Box;
                if (vbox != null)
                {
                    foreach (var lab in vbox.Children)
                    {
                        if (lab is Gtk.Label label)
                        {
                            label.Text = text;
                            break;
                        }
                    }
                }
            }
            else if (View == View.List)
            {
                if (box != null)
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
    }
    internal void NativeSelectItem(ListViewItem item, bool isselected)
    {
        if (item._flowBoxChild is { Parent: FlowBox flowBox })
        {
            if (isselected)
                flowBox.SelectChild(item._flowBoxChild);
            else
                flowBox.UnselectChild(item._flowBoxChild);
        }
    }
    internal void NativeCheckItem(ListViewItem item, bool ischecked)
    {
        if (item._flowBoxChild is { Parent: FlowBox })
        {
            var box = item._flowBoxChild.Child as Box;
            if (View == View.Details)
            {
                var viewport = box?.Children[0] as Viewport;
                var layout = viewport?.Child as Gtk.Layout;
                if (layout != null)
                {
                    foreach (var chk in layout.Children)
                    {
                        if (chk is CheckButton chkbutton)
                        {
                            chkbutton.Active = ischecked;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (box != null)
                {
                    foreach (var chk in box.Children)
                    {
                        if (chk is CheckButton chkbutton)
                        {
                            chkbutton.Active = ischecked;
                            break;
                        }
                    }
                }
            }
        }
    }
    internal void NativeAdd(ListViewItem? item, int position)
    {
        if (self.IsRealized)
        {
            var boxitem = new FlowBoxChild();
            boxitem.TooltipText = item?.Text;
            if (item != null)
            {
                boxitem.Data.Add("ItemId", item.Index);
                boxitem.Halign = Align.Start;
                boxitem.Valign = Align.Start;
                boxitem.BorderWidth = 0;
                boxitem.Margin = 0;
                boxitem.HeightRequest = FontSize + 10;
                item._flowBoxChild = boxitem;
                var hBox = new BoxBase(Gtk.Orientation.Horizontal, 0);
                if (item.BackColor.HasValue)
                {
                    hBox.Override.BackColor = Color.FromArgb(180, item.BackColor.Value);
                }

                hBox.Valign = Align.Fill;
                hBox.Halign = Align.Start;
                hBox.BorderWidth = 0;
                hBox.Homogeneous = false;

                foreach (var col in Columns)
                {
                    if (item.SubItems != null && item.SubItems.Count > col.Index)
                    {
                        boxitem.Data.Add(col.Index, item.SubItems[col.Index].Text);
                    }
                    else
                        boxitem.Data.Add(col.Index, string.Empty);
                }

                boxitem.Add(hBox);

                var flowBox = DefaultGroup.flowBox;
                if (ShowGroups && View != View.List && View != View.Tile)
                {
                    if (Groups.Exists(g => g.Name == item.Group?.Name))
                        flowBox = item.Group?.flowBox;
                    else
                        NativeGroupAdd(DefaultGroup, -1);
                }
                else
                {
                    NativeGroupAdd(DefaultGroup, -1);
                }

                if (position == -1)
                    flowBox?.Add(boxitem);
                else
                    flowBox?.Insert(boxitem, position);

                if (View == View.Details)
                {
                    hBox.Spacing = 0;
                    header.Visible = true;
                    flowBox!.MinChildrenPerLine = 1;
                    flowBox.MaxChildrenPerLine = 1;
                    var fistcell = new Gtk.Layout(new Adjustment(IntPtr.Zero), new Adjustment(IntPtr.Zero));
                    fistcell.Halign = Align.Start;
                    fistcell.Valign = Align.Fill;
                    fistcell.Vexpand = true;
                    fistcell.BorderWidth = 0;
                    fistcell.WidthRequest = Columns.Count > 0 ? Columns[0].Width : Width;
                    fistcell.HeightRequest = boxitem.HeightRequest;

                    var xPosition = 5;
                    var padding = 2;
                    if (CheckBoxes)
                    {
                        var checkBox = new CheckBox();
                        checkBox.self.Halign = Align.Start;
                        checkBox.self.Valign = Align.Center;
                        checkBox.Width = 20;
                        checkBox.self.BorderWidth = 0;
                        checkBox.Checked = item.Checked;
                        checkBox.CheckedChanged += (sender, _) =>
                        {
                            var box = sender as CheckBox;
                            var checkitem = box?.self.Parent.Parent.Parent.Parent as FlowBoxChild;
                            var thisitem = Items.Find(m => m.Index == Convert.ToInt32(checkitem?.Data["ItemId"]));
                            if (thisitem != null)
                            {
                                if (box != null)
                                {
                                    thisitem.Checked = box.self.Active;
                                    if (checkitem != null)
                                    {
                                        ItemCheck?.Invoke(sender,
                                            new ItemCheckEventArgs(checkitem.Index,
                                                box.self.Active ? CheckState.Checked : CheckState.Unchecked,
                                                box.self.Active ? CheckState.Unchecked : CheckState.Checked));
                                    }
                                }

                                ItemChecked?.Invoke(sender, new ItemCheckedEventArgs(thisitem));
                            }
                        };

                        fistcell.Put(checkBox.self, xPosition, padding);
                        xPosition += 20;
                    }

                    if (SmallImageList != null)
                    {
                        var imgsize = FontSize + 2;
                        SmallImageList.ImageSize = new Size(imgsize, imgsize);
                        if (!string.IsNullOrWhiteSpace(item.ImageKey))
                        {
                            Drawing.Image img = SmallImageList.GetBitmap(item.ImageKey);
                            if (img != null)
                                fistcell.Put(new Gtk.Image(img.Pixbuf) { Halign = Align.Start, Valign = Align.Fill },
                                    xPosition, padding + 2);
                            xPosition += imgsize + 5;
                        }
                        else if (item.ImageIndex > -1)
                        {
                            Drawing.Image img = SmallImageList.GetBitmap(item.ImageIndex);
                            if (img != null)
                                fistcell.Put(new Gtk.Image(img.Pixbuf) { Halign = Align.Start, Valign = Align.Fill },
                                    xPosition, padding + 2);
                            xPosition += imgsize + 5;
                        }
                    }

                    var lab = new Gtk.Label();
                    var attributes = new Pango.AttrList();
                    if (item.ForeColor.HasValue)
                    {
                        var fg = new Pango.AttrForeground(Convert.ToUInt16(item.ForeColor.Value.R * 257),
                            Convert.ToUInt16(item.ForeColor.Value.G * 257),
                            Convert.ToUInt16(item.ForeColor.Value.B * 257));
                        attributes.Insert(fg);
                    }

                    lab.Attributes = attributes;
                    lab.Halign = Align.Start;
                    lab.Valign = Align.Fill;
                    lab.Ellipsize = Pango.EllipsizeMode.End;
                    lab.Text = item.Text;
                    lab.Xalign = 0;
                    lab.Xpad = 0;
                    lab.Ypad = 0;
                    lab.WidthRequest = fistcell.WidthRequest - xPosition;
                    fistcell.Put(lab, xPosition, padding);
                    var viewport = new Viewport();
                    viewport.WidthRequest = Columns.Count > 0 ? Columns[0].Width : Width;
                    viewport.BorderWidth = 0;
                    viewport.Add(fistcell);
                    hBox.PackStart(viewport, false, true, 0);

                    var index = 0;
                    foreach (var col in Columns)
                    {
                        if (index > 0)
                        {
                            var sublayout = new Gtk.Layout(new Adjustment(IntPtr.Zero), new Adjustment(IntPtr.Zero));
                            sublayout.Halign = Align.Start;
                            sublayout.Valign = Align.Fill;
                            sublayout.WidthRequest = col.Width;
                            if (item.SubItems != null && item.SubItems.Count > index)
                            {
                                ListViewSubItem subitem = item.SubItems[index];
                                Gtk.Label sublabel = new Gtk.Label();
                                subitem._label = sublabel;
                                var subattributes = new Pango.AttrList();
                                if (subitem.ForeColor.HasValue)
                                {
                                    var fg = new Pango.AttrForeground(Convert.ToUInt16(subitem.ForeColor.Value.R * 257),
                                        Convert.ToUInt16(subitem.ForeColor.Value.G * 257),
                                        Convert.ToUInt16(subitem.ForeColor.Value.B * 257));
                                    subattributes.Insert(fg);
                                    sublabel.Attributes = subattributes;
                                }

                                //if (subitem.BackColor.HasValue)
                                //{
                                //    Pango.AttrBackground fg = new Pango.AttrBackground(Convert.ToUInt16(subitem.BackColor.Value.R * 257), Convert.ToUInt16(subitem.BackColor.Value.G * 257), Convert.ToUInt16(subitem.BackColor.Value.B * 257));
                                //    subattributes.Insert(fg);
                                //}
                                sublabel.Attributes = subattributes;
                                sublabel.WidthRequest = col.Width + 2;
                                sublabel.MaxWidthChars = 0;

                                sublabel.Halign = Align.Fill;
                                sublabel.Valign = Align.Fill;
                                sublabel.Ellipsize = Pango.EllipsizeMode.End;
                                sublabel.Text = subitem.Text;
                                sublayout.Add(sublabel);
                            }

                            var sviewport = new Viewport();
                            sviewport.WidthRequest = col.Width;
                            sviewport.BorderWidth = 0;
                            sviewport.Add(sublayout);
                            hBox.PackStart(sviewport, false, true, 0);
                        }

                        index++;
                    }
                }
                else
                {
                    if (CheckBoxes)
                    {
                        var checkBox = new CheckBox();
                        checkBox.self.Halign = Align.Start;
                        checkBox.self.Valign = Align.Center;
                        checkBox.Width = 20;
                        checkBox.self.BorderWidth = 0;
                        checkBox.Checked = item.Checked;
                        checkBox.CheckedChanged += (sender, _) =>
                        {
                            var box = sender as CheckBox;
                            var checkitem = box?.self.Parent.Parent as FlowBoxChild;
                            var thisitem = Items.Find(m => m.Index == Convert.ToInt32(checkitem?.Data["ItemId"]));
                            if (thisitem != null)
                            {
                                thisitem.Checked = box?.self.Active??false;
                                ItemCheck?.Invoke(sender,
                                    new ItemCheckEventArgs(checkitem?.Index??-1,
                                        box?.self.Active??false ? CheckState.Checked : CheckState.Unchecked,
                                        box?.self.Active ?? false ? CheckState.Unchecked : CheckState.Checked));
                                ItemChecked?.Invoke(sender, new ItemCheckedEventArgs(thisitem));
                            }
                        };
                        hBox.PackStart(checkBox.self, false, true, 5);
                    }

                    if (View == View.SmallIcon)
                    {
                        header.Visible = false;
                        if (flowBox != null)
                        {
                            flowBox.MinChildrenPerLine = 1;
                            flowBox.MaxChildrenPerLine = 999;
                        }

                        if (SmallImageList != null)
                        {
                            SmallImageList.ImageSize = new Size(16, 16);
                            if (!string.IsNullOrWhiteSpace(item.ImageKey))
                            {
                                Drawing.Image img = SmallImageList.GetBitmap(item.ImageKey);
                                if (img != null)
                                    hBox.PackStart(new Gtk.Image(img.Pixbuf), false, false, 0);
                            }
                            else if (item.ImageIndex > -1)
                            {
                                Drawing.Image img = SmallImageList.GetBitmap(item.ImageIndex);
                                if (img != null)
                                    hBox.PackStart(new Gtk.Image(img.Pixbuf), false, false, 0);
                            }
                        }

                        var lab = new Gtk.Label();
                        var attributes = new Pango.AttrList();
                        if (item.ForeColor.HasValue)
                        {
                            var fg = new Pango.AttrForeground(Convert.ToUInt16(item.ForeColor.Value.R * 257),
                                Convert.ToUInt16(item.ForeColor.Value.G * 257),
                                Convert.ToUInt16(item.ForeColor.Value.B * 257));
                            attributes.Insert(fg);
                        }

                        }
                        lab.Xpad = 5;
                        lab.Attributes = attributes;
                        lab.MaxWidthChars = 100;
                        lab.Halign = Align.Start;
                        lab.Valign = Align.Center;
                        lab.Ellipsize = Pango.EllipsizeMode.End;
                        lab.Text = item.Text;
                        hBox.PackStart(lab, false, false, 0);
                    }
                    else if (View == View.LargeIcon)
                    {
                        if (flowBox != null)
                        {
                            flowBox.MinChildrenPerLine = 1;
                            flowBox.MaxChildrenPerLine = 999;
                        }

                        var vBox = new Box(Gtk.Orientation.Vertical, 5);
                        if (LargeImageList != null)
                        {
                            if (SmallImageList != null)
                            {
                                SmallImageList.ImageSize = new Size(100, 100);
                            }

                            if (!string.IsNullOrWhiteSpace(item.ImageKey))
                            {
                                Drawing.Image img = LargeImageList.GetBitmap(item.ImageKey);
                                if (img != null)
                                {
                                    var width = img.Pixbuf?.Width;
                                    var height = img.Pixbuf?.Height;
                                    vBox.Add(new Gtk.Image(new Gdk.Pixbuf(img.PixbufData))
                                        { WidthRequest = Math.Min(50, width??0), HeightRequest = Math.Min(50, height??0) });
                                }
                            }
                            else if (item.ImageIndex > -1)
                            {
                                Drawing.Image img = LargeImageList.GetBitmap(item.ImageIndex);
                                if (img != null)
                                {
                                    var width = img.Pixbuf?.Width;
                                    var height = img.Pixbuf?.Height;
                                    vBox.Add(new Gtk.Image(img.Pixbuf)
                                        { WidthRequest = Math.Min(50, width?? 0), HeightRequest = Math.Min(50, height?? 0) });
                                }
                            }
                        }

                        var lab = new Gtk.Label();
                        var attributes = new Pango.AttrList();
                        if (item.ForeColor.HasValue)
                        {
                            var fg = new Pango.AttrForeground(Convert.ToUInt16(item.ForeColor.Value.R * 257),
                                Convert.ToUInt16(item.ForeColor.Value.G * 257),
                                Convert.ToUInt16(item.ForeColor.Value.B * 257));
                            attributes.Insert(fg);
                        }

                        }
                        lab.Xpad = 5;
                        lab.MaxWidthChars = 16;
                        lab.Halign = Align.Center;
                        lab.Valign = Align.Center;
                        lab.Ellipsize = Pango.EllipsizeMode.End;
                        lab.Text = item.Text;
                        vBox.Add(lab);
                        hBox.PackStart(vBox, false, false, 0);
                    }
                    else
                    {
                        header.Visible = false;
                        boxitem.Halign = Align.Fill;

                        var lab = new Gtk.Label();
                        var attributes = new Pango.AttrList();
                        if (item.ForeColor.HasValue)
                        {
                            var fg = new Pango.AttrForeground(Convert.ToUInt16(item.ForeColor.Value.R * 257),
                                Convert.ToUInt16(item.ForeColor.Value.G * 257),
                                Convert.ToUInt16(item.ForeColor.Value.B * 257));
                            attributes.Insert(fg);
                        }

                        lab.Halign = Align.Start;
                        lab.Valign = Align.Fill;
                        lab.Text = item.Text;
                        hBox.PackStart(lab, false, false, 0);
                    }
                }
            }

            if (isCacheUpdate == false)
                self.ShowAll();
        }
    }

    internal void NativeGroupAdd(ListViewGroup? group, int position)
    {
        if (self.IsRealized)
        {
            if (group == null)
                return;

            if (position == -1 && Groups.Exists(g => g.serialGuid == group.serialGuid) == false)
            {
                if (AllowColumnReorder)
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
            }

            var _flow = DefaultGroup.flowBox;
            var hBox = new Box(Gtk.Orientation.Vertical, 0);
            hBox.Valign = Align.Start;
            hBox.Halign = Align.Fill;
            if (ShowGroups && View != View.List && View != View.Tile)
            {
                _flow = group.flowBox;
                if (_flow.Parent != null)
                    return;

                var groupbox = new Box(Gtk.Orientation.Horizontal, 0);
                groupbox.StyleContext.AddClass("GroupTitle");
                groupbox.MarginStart = 3;
                var title = new Gtk.Label(group.Header) { Xalign = 0, Halign = Align.Start, Valign = Align.Center, Ellipsize = Pango.EllipsizeMode.End };
                groupbox.PackStart(title, false, false, 0);
                var groupline = new Viewport();
                groupline.StyleContext.AddClass("GroupLine");
                groupline.HeightRequest = 1;
                groupline.Halign = Align.Fill;
                groupline.Valign = Align.Center;
                groupline.Vexpand = false;
                groupbox.PackEnd(groupline, true, true, 0);
                group.Groupbox = groupbox;
                if (Groups.Count == 0)
                {
                    if (group.Name == ListViewGroup.defaultListViewGroupKey)
                    {
                        groupbox.NoShowAll = true;
                        groupbox.Visible = false;
                        _flow.StyleContext.AddClass("GridBorder");
                    }
                }
                else
                {
                    var box = DefaultGroup.Groupbox;
                    if (box is { } defbox)
                    {
                        defbox.NoShowAll = false;
                        defbox.Visible = true;
                        DefaultGroup.flowBox.StyleContext.RemoveClass("GridBorder");
                    }
                }

                hBox.PackStart(groupbox, false, false, 0);
                if (!string.IsNullOrEmpty(group.Subtitle))
                {
                    var subtitle = new Gtk.Label(group.Subtitle) { Xalign = 0, Halign = Align.Fill, Valign = Align.Start, Ellipsize = Pango.EllipsizeMode.End };
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
                _flow.MaxChildrenPerLine = 500u;
                _flow.MinChildrenPerLine = 0u;
                _flow.Halign = Gtk.Align.Fill;
                _flow.Valign = Gtk.Align.Start;
                _flow.SelectionMode = MultiSelect == false ? Gtk.SelectionMode.Single : Gtk.SelectionMode.Multiple;
                _flow.ActivateOnSingleClick = MultiSelect == false;
                _flow.SortFunc = new Gtk.FlowBoxSortFunc((fbc1, fbc2) =>
                {
                    if (SortingColumnIndex > -1)
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
                _flow.SelectedChildrenChanged += _flow_SelectedChildrenChanged;
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

    private void _flow_SelectedChildrenChanged(object? sender, EventArgs e)
    {
        if (MultiSelect)
        {
            ItemActivate?.Invoke(this, e);

            var selecteds = new List<int>();
            foreach (var group in GetAllGroups())
            {
                foreach (var o in group.flowBox.SelectedChildren)
                {
                    if (o.IsSelected)
                    {
                        var id = Convert.ToInt32(o.Data["ItemId"]);
                        selecteds.Add(id);
                    }
                }
            }
            foreach (var item in Items)
            {
                if (selecteds.Contains(item.Index))
                {
                    if (item.Selected == false)
                    {
                        item.Selected = true;
                        ItemSelectionChanged?.Invoke(this, new ListViewItemSelectionChangedEventArgs(item, item.Index, item.Selected));
                    }
                }
                else if (item.Selected)
                {
                    item.Selected = false;
                    ItemSelectionChanged?.Invoke(this, new ListViewItemSelectionChangedEventArgs(item, item.Index, item.Selected));
                }
            }
        }

        SelectedIndexChanged?.Invoke(this, e);
    }
    private void _flow_ChildActivated(object? o, ChildActivatedArgs args)
    {
        var widget = o as FlowBox;
        var item = Items.Find(m => m.Index == Convert.ToInt32(args.Child.Data["ItemId"]));
        if (item != null)
        {
            item._selected = args.Child.IsSelected;
            foreach (var group in GetAllGroups())
            {
                if (group.flowBox.Equals(widget) == false)
                {
                    group.flowBox.UnselectAll();
                }
            }

            ItemActivate?.Invoke(this, args);
            ItemSelectionChanged?.Invoke(this, new ListViewItemSelectionChangedEventArgs(item, item.Index, item.Selected));
        }
    }
    public void Sort()
    {
        foreach (var group in GetAllGroups())
        {
            group.flowBox.InvalidateSort();
        }
    }
    private bool isCacheUpdate;
    public void BeginUpdate()
    {
        isCacheUpdate = true;
    }
    public void EndUpdate()
    {
        isCacheUpdate = false;
        self.Window?.ProcessUpdates(true);
        self.ShowAll();
    }
    private ListViewGroup? _defaultGroup;
    private ImageList? smallImageList;

    internal ListViewGroup DefaultGroup
    {
        get
        {
            if (_defaultGroup == null)
            {
                _defaultGroup = ListViewGroup.CreateDefaultListViewGroup();
            }
            return _defaultGroup!;
        }
    }
    private IEnumerable<ListViewGroup> GetAllGroups()
    {
        if (_defaultGroup == null)
            return Groups;
        return Groups.Union([_defaultGroup]);
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
                return Find(w => w.Name == key);
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

        public bool IsReadOnly => false;

        public new virtual bool Remove(ColumnHeader item)
        {
            var index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                item.displayIndex = -1;
                for (var i = 0; i < owner.Columns.Count; i++)
                {
                    if (owner.Columns[i].displayIndex >= item.displayIndex)
                    {
                        owner.Columns[i].displayIndex--;
                    }
                }
                return true;
            }

            return false;
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

        public new virtual void Add(ColumnHeader item)
        {
            item._listView = owner;
            base.Add(item);
            item.displayIndex = Count - 1;
            item.ImageList = owner.smallImageList;
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
            return Contains(Find(o => o.Name == key));
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
        readonly ListView _owner;
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
        public virtual ListViewItem Add(string? text)
        {
            return Add("", text, -1);
        }

        public virtual ListViewItem Add(string? text, int imageIndex)
        {
            return Add("", text, imageIndex);
        }

        public virtual ListViewItem Add(string? text, string? imageKey)
        {
            return Add("", text, imageKey);
        }

        public virtual ListViewItem Add(string key, string? text, string? imageKey)
        {
            var item = new ListViewItem(text, imageKey)
            {
                Name = key,
                Text = text??string.Empty
            };
            AddCore(item, -1);
            return item;
        }

        public virtual ListViewItem Add(string key, string? text, int imageIndex)
        {
            var item = new ListViewItem(text, imageIndex)
            {
                Name = key
            };
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
            foreach (var item in items)
            {
                AddCore(item, -1);
            }
        }

        public void AddRange(ListViewItemCollection items)
        {
            foreach (var item in items)
                AddCore(item, -1);
        }

        public virtual bool ContainsKey(string key)
        {
            return FindIndex(w => w.Name == key) > -1;
        }

        public void CopyTo(Array dest, int index)
        {
            throw new NotImplementedException();
        }

        public ListViewItem[] Find(string key, bool searchAllSubItems)
        {
            if (searchAllSubItems)
                return FindAll(w => w.Name == key && (w.SubItems?.ContainsKey(key)??false)).ToArray();
            return FindAll(w => w.Name == key).ToArray();
        }

        public virtual int IndexOfKey(string key)
        {
            return FindIndex(w => w.Name == key);
        }

        public ListViewItem Insert(int index, string? text)
        {
            return Insert(index, "", text, -1);
        }

        public ListViewItem Insert(int index, string? text, int imageIndex)
        {
            return Insert(index, "", text, imageIndex);
        }

        public ListViewItem Insert(int index, string? text, string? imageKey)
        {
            return Insert(index, "", text, imageKey);
        }

        public virtual ListViewItem Insert(int index, string key, string? text, string? imageKey)
        {
            var item = new ListViewItem(text, imageKey)
            {
                Name = key
            };
            base.Insert(index, item);
            return item;
        }

        public virtual ListViewItem Insert(int index, string key, string? text, int imageIndex)
        {
            var item = new ListViewItem(text, imageIndex)
            {
                Name = key
            };
            base.Insert(index, item);
            return item;
        }

            public virtual void RemoveByKey(string key)
            {
                base.Remove(base.Find(w => w.Name == key));
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
            var selecteditems = new CheckedIndexCollection(this);
            foreach (var item in CheckedItems)
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
            var selecteditems = new CheckedListViewItemCollection(this);
            foreach (var item in Items)
            {
                if (item.Checked)
                {
                    selecteditems.Add(item);
                }
            }
            return selecteditems;
        }
    }
    public ColumnHeaderCollection Columns => _columns;

    public bool FullRowSelect { get; set; }

    public ListViewGroupCollection Groups => _groups;

    public ListViewItemCollection Items => _items;

    public IComparer? ListViewItemSorter
    {
        get;
        set;
    }

    public SelectedIndexCollection SelectedIndices
    {
        get
        {
            var selecteditems = new SelectedIndexCollection();
            foreach (var item in SelectedItems)
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
            var selecteditems = new SelectedListViewItemCollection();
            foreach (var item in Items)
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

    public ListViewItem? FindItemWithText(string text, bool includeSubItemsInSearch, int startIndex)
    {
        var idx = Items.FindIndex(startIndex, w => w.Text == text);
        return idx == -1 ? null : Items[idx];
    }

        public ListViewItem FindItemWithText(string text, bool includeSubItemsInSearch, int startIndex, bool isPrefixSearch)
        {
            int idx = Items.FindIndex(startIndex, w => w.Text == text);
            return idx == -1 ? null : Items[idx];
        }
        // public override event MouseEventHandler MouseDown;
        public ListViewItem GetItemAt(int x, int y)
        {
            foreach (Gtk.Box vbox in flowBoxContainer.Children)
            {
                if (vbox.Allocation.Top < y && vbox.Allocation.Top + vbox.AllocatedHeight > y)
                {
                    foreach (var flow in vbox.Children)
                    {
                        if (flow is Gtk.FlowBox _flow)
                        {
                            int top2 = _flow.Allocation.Top - __headerheight;
                            FlowBoxChild child = _flow.GetChildAtPos(x, y - top2);
                            if (child != null)
                            {
                                return this.Items.Find(m => m.Index == Convert.ToInt32(child.Data["ItemId"]));
                            }
                        }
                    }
                }
            }
            return null;
        }

    public Rectangle GetItemRect(int index)
    {
        throw new NotImplementedException();
    }

    public event ColumnClickEventHandler? ColumnClick;
    public event ColumnReorderedEventHandler? ColumnReordered;
    public event ItemCheckEventHandler? ItemCheck;
    public event ItemCheckedEventHandler? ItemChecked;
    public event ListViewItemSelectionChangedEventHandler? ItemSelectionChanged;
    public event EventHandler? SelectedIndexChanged;
    //public override event EventHandler? Click;
    public event EventHandler? ItemActivate;
}