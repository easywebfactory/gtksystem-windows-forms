/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://github.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace System.Windows.Forms
{

    [DesignerCategory("Component")]
    public partial class ComboBox: ListControl
    {
        public readonly ComboBoxBase self = new ComboBoxBase();
        public override object GtkControl => self;
        private ObjectCollection __itemsData;
        public ComboBox():base()
        {
            self.Entry.HasFrame = false;
            self.Entry.WidthChars = 0;

            __itemsData = new ObjectCollection(this);
            self.Realized += Self_Realized;
            self.Changed += Self_Changed;
        }

        private void Self_Changed(object sender, EventArgs e)
        {
            if (self.IsVisible)
            {
                ((EventHandler)Events["SelectedIndexChanged"])?.Invoke(this, e);
                ((EventHandler)Events["SelectedValueChanged"])?.Invoke(this, e);
                ((EventHandler)Events["SelectedItemChanged"])?.Invoke(this, e);
            }
        }

        private void Self_Realized(object sender, EventArgs e)
        {
            OnSetDataSource();

            if (DropDownStyle == ComboBoxStyle.DropDownList)
            {
                self.Entry.IsEditable = false;
                self.Entry.CanFocus = false;
                self.Entry.NoShowAll = true;
                self.Entry.WidthRequest = 1;
                var ws = ((Gtk.Box)self.Children[0].Parent).Children[1] as Gtk.ToggleButton;
                ws.WidthRequest = self.WidthRequest;

                ws.Label = self.Entry.Text;
                ws.Image = Gtk.Image.NewFromIconName("pan-down", Gtk.IconSize.Button);
                ws.ImagePosition = Gtk.PositionType.Right;
                ws.AlwaysShowImage = true;
                ws.Valign = Align.Center;
                ws.Yalign = 0.5f;
                ws.Xalign = 0.95f;
                ws.Hexpand = true;
                ws.Image.Halign = Gtk.Align.End;
                ws.Image.Valign = Gtk.Align.Center;
                ws.Drawn += Ws_Drawn;
            }
        }
        private void Ws_Drawn(object o, Gtk.DrawnArgs args)
        {
            self.Entry.Visible = false;
            Gtk.ToggleButton ws = (Gtk.ToggleButton)o;
            ws.WidthRequest = -1;
            Pango.Context pangocontext = ws.PangoContext;
            string family = pangocontext.FontDescription.Family;
            args.Cr.SelectFontFace(family, Cairo.FontSlant.Normal, Cairo.FontWeight.Normal);

            args.Cr.SetSourceRGBA(this.ForeColor.R / 255, this.ForeColor.G / 255, this.ForeColor.B / 255, 0.78);

            double fontsize = pangocontext.FontDescription.Size / Pango.Scale.PangoScale * 1.5;
            args.Cr.Save();
            args.Cr.Translate(10, (ws.AllocatedHeight - fontsize) / 2 + fontsize - 2);
            args.Cr.SetFontSize(fontsize);
            Cairo.TextExtents ext = args.Cr.TextExtents(self.Entry.Text);
            string text = self.Entry.Text;
            while (text.Length > 1 && args.Cr.TextExtents(text).Width > ws.AllocatedWidth - 40)
                text = text.Substring(0, text.Length - 1);
            args.Cr.ShowText(text);
            args.Cr.Restore();
        }

        private ComboBoxStyle _DropDownStyle;
        public ComboBoxStyle DropDownStyle { 
            get=> _DropDownStyle; 
            set {
                _DropDownStyle = value;
                if (value == ComboBoxStyle.DropDown)
                {
                    self.StyleContext.AddClass("DropDown");
                }
                else if (value == ComboBoxStyle.DropDownList)
                {
                    self.StyleContext.RemoveClass("DropDown");
                    self.StyleContext.AddClass("DropDownList");
                    self.Entry.IsEditable = false;
                    self.Entry.CanFocus = false;
                }
                else
                {
                    self.StyleContext.RemoveClass("DropDown");
                    self.StyleContext.RemoveClass("DropDownList");
                }
            }
        }


        public override string Text { get => self.Entry.Text; set { self.Entry.Text = value; } }
        public object SelectedItem { 
            get { return SelectedIndex == -1 ? null : __itemsData[SelectedIndex]; }
            set { int _index = __itemsData.IndexOf(value); if (_index != -1) { SelectedIndex = _index; } } 
        }
        internal int _selectedIndex;
        public override int SelectedIndex { get { return self.Active; } set { self.Active = value; _selectedIndex = value; if (value == -1) { Text = ""; } } }
        public ObjectCollection Items { get { return __itemsData; } }
        public override string GetItemText(object item)
        {
            if (item is ObjectCollection.Entry entry)
            {
                return entry.Item?.ToString();
            }
            return item?.ToString();
        }
        public string NativeGetItemText(int index)
        {
            return __itemsData[index].ToString();
        }
        private bool _sorted;
        public bool Sorted { get=> _sorted; set=> _sorted = value; }
        public object _DataSource;
        public override object DataSource
        {
            get => _DataSource;
            set {
                _DataSource = value;
                if (self.IsVisible)
                {
                    OnSetDataSource();
                }
            }
        }
        private void OnSetDataSource()
        {
            if (_DataSource != null)
            {
                if (_DataSource is IListSource listSource)
                {
                    IEnumerator list = listSource.GetList().GetEnumerator();
                    SetDataSource(list);
                }
                else if (_DataSource is IEnumerable list1)
                {
                    SetDataSource(list1.GetEnumerator());
                }
            }
        }
        private void SetDataSource(IEnumerator enumerator)
        {
            __itemsData.Clear();
            if (enumerator != null)
            {
                if (string.IsNullOrWhiteSpace(DisplayMember))
                {
                    while (enumerator.MoveNext())
                    {
                        var o = enumerator.Current;
                        if (o is DataRowView row)
                            __itemsData.Add(row[0]);
                        else
                            __itemsData.Add(enumerator.Current);
                    }
                }
                else
                {
                    while (enumerator.MoveNext())
                    {
                        var o = enumerator.Current;
                        if(o is DataRowView row)
                            __itemsData.Add(row[DisplayMember]);
                        else
                            __itemsData.Add(o.GetType().GetProperty(DisplayMember)?.GetValue(o));
                    }
                }
            }
        }

    }

}
