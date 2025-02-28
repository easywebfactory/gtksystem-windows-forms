/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;

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
            var ws = ((Gtk.Box)self.Children[0].Parent).Children[1] as Gtk.ToggleButton;
            ws.Toggled += Ws_Toggled;
            if (DropDownStyle == ComboBoxStyle.DropDownList)
            {
                self.Entry.IsEditable = false;
                self.Entry.CanFocus = false;
                self.Entry.NoShowAll = true;
                self.Entry.WidthRequest = 1;
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
        public event EventHandler DropDown;
        private void Ws_Toggled(object sender, EventArgs e)
        {
            if (DropDown != null)
            {
                DropDown(this, e);
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
            if (this.ForeColor.Name == "0")
            {
                Gdk.RGBA color = ws.StyleContext.GetColor(StateFlags.Normal);
                args.Cr.SetSourceRGBA(color.Red, color.Green, color.Blue, color.Alpha);
            }
            else
            {
                args.Cr.SetSourceRGBA(this.ForeColor.R / 255, this.ForeColor.G / 255, this.ForeColor.B / 255, this.ForeColor.A / 255);
            }

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
                    self.StyleContext.RemoveClass("DropDownList");
                }
                else if (value == ComboBoxStyle.DropDownList)
                {
                    self.StyleContext.AddClass("DropDownList");
                    self.Entry.IsEditable = false;
                    self.Entry.CanFocus = false;
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
        public override object SelectedValue { get { return self.ActiveId; } set => self.ActiveId = value?.ToString(); }
        public ObjectCollection Items { get { return __itemsData; } }
        public override string GetItemText(object item)
        {
            if (item is ObjectCollection.Entry entry)
            {
                Type type = entry.Item.GetType();
                if (entry.Item is DataRow dr)
                    return dr[DisplayMember]?.ToString();
                else if (type.IsValueType && type.IsPrimitive)
                    return type.GetProperty(DisplayMember).GetValue(entry)?.ToString();
                else
                    return item?.ToString();
            }
            return item?.ToString();
        }
        public string NativeGetItemText(int index)
        {
            self.Model.GetIter(out TreeIter iter, new TreePath(new int[] { index }));
            object val = self.Model.GetValue(iter, 1);
            return val?.ToString();
        }
        public void NativeAdd(int index, string value, string text)
        {
            if (_sorted == false && index > -1)
            {
                self.Insert(index, value, text);
            }
            else
            {
                self.Append(value, text);
            }
        }
        private bool _sorted;
        public bool Sorted { get=> _sorted; set=> _sorted = value; }
        public object _DataSource;
        public override object DataSource
        {
            get => _DataSource;
            set {
                _DataSource = value;
                if (self.IsRealized)
                {
                    OnSetDataSource();
                }
            }
        }
        private void OnSetDataSource()
        {
            if (_DataSource != null)
            {
                if (_DataSource is DataTable dtable)
                {
                    LoadDataTableSource(dtable);
                }
                else if (_DataSource is DataView dview)
                {
                    LoadDataTableSource(dview.Table);
                }
                else if (_DataSource is IList list)
                {
                    LoadListSource(list);
                }
            }
        }
        private void LoadDataTableSource(DataTable dtable)
        {
            __itemsData.Clear();
            if(dtable.Columns.Contains(ValueMember)&& dtable.Columns.Contains(DisplayMember))
            {
                foreach (DataRow row in dtable.Rows)
                    __itemsData.Add(row[ValueMember].ToString(), row[DisplayMember].ToString(), row);
            }
            else if (dtable.Columns.Contains(DisplayMember))
            {
                foreach (DataRow row in dtable.Rows)
                    __itemsData.Add("", row[DisplayMember].ToString(), row);
            }
            else
            {
                throw new Exception("DisplayMember属性未赋值或字段名不存在");
            }
        }
        private void LoadListSource(IList list)
        {
            __itemsData.Clear();
            if (list.Count > 0)
            {
                Type type = list[0].GetType();
                PropertyInfo valproperty = type.GetProperty(ValueMember);
                PropertyInfo disproperty = type.GetProperty(DisplayMember);
                foreach (var entry in list)
                    __itemsData.Add(valproperty?.GetValue(entry)?.ToString(), disproperty?.GetValue(entry)?.ToString(), entry);
            }
        }
    }

}
