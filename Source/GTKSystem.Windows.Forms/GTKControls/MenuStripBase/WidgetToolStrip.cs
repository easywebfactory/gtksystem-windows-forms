/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Gtk;
using Image = System.Drawing.Image;
using Region = System.Drawing.Region;

namespace System.Windows.Forms;

public class WidgetToolStrip<T> : ToolStripItem
{
    public override string? UniqueKey { get; protected set; }
    public override Widget? Widget => MenuItem;
    internal Box itemBox = new(Gtk.Orientation.Horizontal, 0);
    internal Gtk.Label label = new();
    internal Gtk.Button button = new();
    internal Entry entry = new();
    internal ComboBoxText comboBox = new() { HasFrame = false };
    // internal Gtk.ProgressBar progressBar = new Gtk.ProgressBar();
    internal LevelBar progressBar = new();
    internal Viewport flagBox = new();
    readonly CssProvider provider = new();
    public string? StripType { get; set; }
    public WidgetToolStrip() : this(null, "", null, null, "", [])
    {
    }

    public WidgetToolStrip(string? stripType, params object[] args) : this(stripType, "", null, null, "", args)
    {

    }
    public WidgetToolStrip(string? text, Image? image, EventHandler? onClick) : this(null, text, image, onClick, "")
    {
    }
    public WidgetToolStrip(string? text, Image? image, EventHandler? onClick, string name) : this(null, text, image, onClick, "")
    {
    }

    protected WidgetToolStrip(string? stripType, string? text, Image? image, EventHandler? onClick, string? name, params object[] args) : base(text, image, onClick, name)
    {
        UniqueKey = Guid.NewGuid().ToString().ToLower();
        MenuItem = (MenuItem)Activator.CreateInstance(typeof(T), args);
        MenuItem.StyleContext.AddProvider(provider, 900);
        if (stripType == "ToolStripSeparator")
        {
            Created = true;
        }
        else
        {
            StripType = stripType;
            MenuItem.Realized += ToolStripItem_Realized;
            MenuItem.Activated += MenuItem_Activated;
            MenuItem.ButtonReleaseEvent += MenuItem_ButtonReleaseEvent;
            MenuItem.Valign = Align.Center;
            MenuItem.Halign = Align.Fill;
            MenuItem.Vexpand = false;
            MenuItem.Hexpand = true;
            itemBox.Valign = Align.Center;
            itemBox.Halign = Align.Start;
            flagBox.BorderWidth = 0;
            flagBox.ShadowType = ShadowType.None;
            flagBox.Hexpand = false;
            flagBox.Vexpand = false;
            if (stripType == "ToolStripDropDownItem")
            {
                button.Image = Gtk.Image.NewFromIconName("pan-down", IconSize.Button);
                button.ImagePosition = PositionType.Right;
                button.Relief = ReliefStyle.None;
                button.AlwaysShowImage = true;
                button.Halign = Align.Start;
                button.Valign = Align.Center;
                button.Hexpand = false;
                button.Vexpand = false;
                itemBox.PackStart(flagBox, false, false, 0);
                itemBox.PackStart(button, false, false, 0);
                MenuItem.Add(itemBox);
            }
            else if (stripType == "ToolStripTextBox")
            {
                entry.HasFrame = false;
                entry.MaxWidthChars = 1;
                entry.WidthChars = 0;
                entry.Valign = Align.Fill;
                entry.Halign = Align.Fill;
                entry.IsFocus = true;
                MenuItem.Add(entry);
            }
            else if (stripType == "ToolStripComboBox")
            {
                MenuItem.Add(comboBox);
            }
            else if (stripType == "ToolStripProgressBar")
            {
                progressBar.Halign = Align.Fill;
                progressBar.Valign = Align.Fill;
                progressBar.Visible = true;
                MenuItem.Add(progressBar);
            }
            else
            {
                itemBox.PackStart(flagBox, false, false, 0);
                itemBox.PackStart(label, false, false, 0);
                MenuItem.Add(itemBox);
            }

        }
        Created = true;

    }

    private void MenuItem_ButtonReleaseEvent(object? o, ButtonReleaseEventArgs args)
    {
        DropDownItemClicked?.Invoke(this, new ToolStripItemClickedEventArgs(this));

        Click?.Invoke(this, args);

    }

    private void MenuItem_Activated(object? sender, EventArgs e)
    {
        if (CheckState != CheckState.Unchecked)
        {
            if (flagBox.Child is CheckButton checkbutton)
            {
                checkbutton.Active = !checkbutton.Active;
            }
        }

        if ((sender as MenuItem)?.Child is ComboBoxText combo)
        {
            combo.Popup();
        }
        else if ((sender as MenuItem)?.Child is Entry ey)
        {
            ey.IsFocus = true;
        }
    }
    public override bool Checked { get; set; }
    public override CheckState CheckState { get; set; } = CheckState.Unchecked;

    [Obsolete]
    public ToolStripItemAlignment Alignment
    {
        get => MenuItem?.Halign == Align.End ? ToolStripItemAlignment.Right : ToolStripItemAlignment.Left; set
        {
            if (MenuItem != null)
            {
                MenuItem.Hexpand = true;
                MenuItem.Halign = Align.End;
                MenuItem.RightJustified = value == ToolStripItemAlignment.Right;
            }
        }
    }
    internal Gtk.RadioButton groupradio = new("");
    private void ToolStripItem_Realized(object? sender, EventArgs e)
    {
        SetStyle(sender as MenuItem);
        var menuItem = sender as MenuItem;
        if (menuItem?.Parent is Menu)
        {
            flagBox.Vexpand = true;
            flagBox.Hexpand = true;
            flagBox.WidthRequest = 30;
            menuItem.StyleContext.AddClass("MenuItem");
        }
        if (CheckState == CheckState.Checked)
        {
            var checkbutton = new CheckButton();
            checkbutton.StyleContext.AddClass("MenuCheck");
            checkbutton.BorderWidth = 0;
            checkbutton.Margin = 0;
            checkbutton.Halign = Align.Center;
            checkbutton.Valign = Align.Center;
            checkbutton.Active = Checked;
            checkbutton.Toggled += Checkbutton_Toggled;
            flagBox.Child = checkbutton;
        }
        else if (CheckState == CheckState.Indeterminate)
        {
            if (Parent is WidgetToolStrip<MenuItem> widgetToolStrip)
            {
                var checkbutton = new Gtk.RadioButton(widgetToolStrip.groupradio);
                checkbutton.StyleContext.AddClass("MenuCheck");
                checkbutton.BorderWidth = 0;
                checkbutton.Margin = 0;
                checkbutton.Halign = Align.Center;
                checkbutton.Valign = Align.Center;
                checkbutton.Active = Checked;
                checkbutton.Toggled += Checkbutton_Toggled;
                flagBox.Child = checkbutton;
            }
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
                    if (Image is { PixbufData: not null })
                    {
                        var ico1 = new Gtk.Image(new Gdk.Pixbuf(Image.PixbufData).ScaleSimple(20, 20, Gdk.InterpType.Tiles));
                        ico1.Halign = Align.Center;
                        ico1.Valign = Align.Center;
                        flagBox.Child = ico1;
                    }
                }
                else
                {

                    if (Image is { PixbufData: not null })
                    {
                        var ico1 = new Gtk.Image(new Gdk.Pixbuf(Image.PixbufData).ScaleSimple(20, 20, Gdk.InterpType.Tiles));
                        ico1.Halign = Align.Center;
                        ico1.Valign = Align.Center;
                        flagBox.Child = ico1;
                    }
                }
            }
            catch (Exception ex) { Trace.Write(ex); }
        }
        menuItem?.ShowAll();

    }

    private void Checkbutton_Toggled(object? sender, EventArgs e)
    {
        CheckedChanged?.Invoke(this, e);
    }

    internal void UpdateStyle()
    {
        if (Widget is { IsMapped: true })
            SetStyle(Widget);
    }
    protected virtual void SetStyle(Widget? widget)
    {

        var style = new StringBuilder();
        if (BackColor.Name != "0")
        {
            var color = $"rgba({BackColor.R},{BackColor.G},{BackColor.B},{BackColor.A})";
            style.AppendFormat("background-color:{0};background:{0};", color);
        }
        if (ForeColor.Name != "0")
        {
            var color = $"rgba({ForeColor.R},{ForeColor.G},{ForeColor.B},{ForeColor.A})";
            style.AppendFormat("color:{0};", color);
        }

        if (Font != null)
        {
            var fontValue = Font;
            if (fontValue.Unit == GraphicsUnit.Pixel)
                style.AppendFormat("font-size:{0}px;", fontValue.Size);
            else if (fontValue.Unit == GraphicsUnit.Inch)
                style.AppendFormat("font-size:{0}in;", fontValue.Size);
            else if (fontValue.Unit == GraphicsUnit.Point)
                style.AppendFormat("font-size:{0}pt;", fontValue.Size);
            else if (fontValue.Unit == GraphicsUnit.Millimeter)
                style.AppendFormat("font-size:{0}mm;", fontValue.Size);
            else if (fontValue.Unit == GraphicsUnit.Document)
                style.AppendFormat("font-size:{0}cm;", fontValue.Size);
            else if (fontValue.Unit == GraphicsUnit.Display)
                style.AppendFormat("font-size:{0}pc;", fontValue.Size);
            else
                style.AppendFormat("font-size:{0}pt;", fontValue.Size);

            if (string.IsNullOrWhiteSpace(fontValue.FontFamily?.Name) == false)
            {
                style.AppendFormat("font-family:\"{0}\";", fontValue.FontFamily?.Name);
            }
            if ((fontValue.Style & FontStyle.Bold) != 0)
            {
                style.Append("font-weight:bold;");
            }
            if ((fontValue.Style & FontStyle.Italic) != 0)
            {
                style.Append("font-style:italic;");
            }
            if ((fontValue.Style & FontStyle.Underline) != 0)
            {
                style.Append("text-decoration:underline;");
            }
            if ((fontValue.Style & FontStyle.Strikeout) != 0)
            {
                style.Append("text-decoration:line-through;");
            }
        }

        var stylename = $"s{UniqueKey}";
        var css = new StringBuilder();
        css.AppendLine($".{stylename}{{{style}}}");
        if (widget is TextView)
        {
            css.AppendLine($".{stylename} text{{{style}}}");
            css.AppendLine($".{stylename} .view{{{style}}}");
        }
        css.AppendLine(" menu menuitem .MenuCheck{padding:0px;margin:0px;} menu .MenuItem{padding:0px;margin-left:-23px;}");

        if (provider.LoadFromData(css.ToString()) && widget != null)
        {
            widget.StyleContext.RemoveClass(stylename);
            widget.StyleContext.AddClass(stylename);
        }
    }

    public override string? Text
    {
        get
        {
            if (StripType == "ToolStripTextBox")
            {
                return entry.Text;
            }

            if (StripType == "ToolStripDropDownItem")
            {
                return button.Label;
            }
            if (StripType == "ToolStripComboBox")
            {
                return comboBox.ActiveText;
            }

            return label.Text;
        }
        set { label.Text = value; button.Label = value; }
    }
    public override Color ImageTransparentColor { get; set; }
    public override ToolStripItemDisplayStyle DisplayStyle { get; set; }
    public override bool AutoToolTip { get; set; }
    public override Image? BackgroundImage { get; set; }
    public override ImageLayout BackgroundImageLayout { get; set; }
    public override string? ToolTipText
    {
        get => Widget?.TooltipText;
        set
        {
            if (Widget != null)
            {
                Widget.TooltipText = value;
            }
        }
    }

    public override ContentAlignment ImageAlign { get; set; }
    public override int ImageIndex { get; set; }
    public override string? ImageKey { get; set; }
    public override ToolStripItemImageScaling ImageScaling { get; set; }
    public override TextImageRelation TextImageRelation { get; set; }
    public override ToolStripTextDirection TextDirection { get; set; }
    public override ContentAlignment TextAlign { get; set; }
    public override bool RightToLeftAutoMirrorImage { get; set; }
    public override bool Pressed { get; protected set; }
    public override ToolStripItemPlacement Placement { get; protected set; }
    public override ToolStripItemOverflow Overflow { get; set; }
    public override ToolStripItem? OwnerItem { get; protected set; }
    public override ToolStrip? Owner { get; set; }
    public override int MergeIndex { get; set; }
    public override MergeAction MergeAction { get; set; }

    public override bool Enabled
    {
        get => Widget?.Sensitive ?? false;
        set
        {
            if (Widget != null)
            {
                Widget.Sensitive = value;
            }
        }
    }
    private Font? font;
    public override Font? Font
    {
        get
        {
            if (font == null && Widget != null)
            {
                var fontdes = Widget.PangoContext.FontDescription;
                var size = Convert.ToInt32(fontdes.Size / Pango.Scale.PangoScale);
                return new Font(new FontFamily(fontdes.Family), size);
            }

            return font;
        }
        set { font = value; UpdateStyle(); }
    }
    private Color foreColor;
    public override Color ForeColor { get => foreColor; set { foreColor = value; UpdateStyle(); } }
    private Color backColor;
    public override Color BackColor { get => backColor; set { backColor = value; UpdateStyle(); } }
    public override bool HasChildren { get; protected set; }

    public override int Height
    {
        get => Widget?.HeightRequest??0;
        set
        {
            if (Widget != null)
            {
                Widget.HeightRequest = value;
            }
        }
    }

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
    public override ToolStripItem? Parent { get; set; }
    public override Region? Region { get; set; }
    public override int Right { get; protected set; }

    public override RightToLeft RightToLeft { get; set; }
    public override ISite? Site { get; set; }
    public override Size Size
    {
        get => new(Widget?.WidthRequest ?? 0, Widget?.HeightRequest ?? 0);
        set => Widget?.SetSizeRequest(value.Width, value.Height);
    }

    public override object? Tag { get; set; }
    public override int Top
    {
        get;
        set;
    }

    public override bool UseWaitCursor { get; set; }
    public override int Width
    {
        get => Widget?.WidthRequest ?? 0;
        set
        {
            if (Widget != null)
            {
                Widget.WidthRequest = value;
            }
        }
    }

    public virtual bool Visible
    {
        get => Widget?.Visible??false;
        set {
            if (Widget != null)
            {
                Widget.Visible = value;
                Widget.NoShowAll = value == false;
            }
        }
    }

    public override event EventHandler? Click;
    public override event EventHandler? CheckedChanged;
    public override event EventHandler? CheckStateChanged;
    public override event ToolStripItemClickedEventHandler? DropDownItemClicked;
}