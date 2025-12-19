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
            self.Override.sender = this;
            self.Entry.HasFrame = false;
            self.Entry.WidthChars = 0;

            __itemsData = new ObjectCollection(this);
            self.Realized += Self_Realized;
            self.Changed += Self_Changed;
            self.GrabNotify += Self_GrabNotify;
            self.MoveActive += Self_MoveActive;
        }

        public event EventHandler DropDown;
        public event EventHandler DropDownClosed;
        private void Self_MoveActive(object o, MoveActiveArgs args)
        {
            self.QueueDraw();
        }

        private void Self_GrabNotify(object o, GrabNotifyArgs args)
        {
            if (args.WasGrabbed)
            {
                DropDownClosed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                DropDown?.Invoke(this, EventArgs.Empty);
            }
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

        private bool Is_Self_Realized;
        private void Self_Realized(object sender, EventArgs e)
        {
            if (!Is_Self_Realized)
            {
                Is_Self_Realized = true;
                OnSetDataSource();
                if (DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    Gtk.Box box =(Gtk.Box)self.Entry.Parent;
                    var ws = box.Children[1] as Gtk.ToggleButton;
                    self.Entry.IsEditable = false;
                    self.Entry.CanFocus = false;
                    self.Entry.NoShowAll = true;
                    self.Entry.WidthRequest = 1;
                    self.Entry.Opacity = 0;
                    self.Entry.Visible = false;
                    ws.WidthRequest = self.WidthRequest;
                    ws.DrawIndicator = true;
                    ws.Drawn += Ws_Drawn;

                    var window = Gtk.Window.ListToplevels().Where(w => w.WindowType == WindowType.Popup);
                    foreach (Gtk.Window w in window)
                    {
                        if (w.AttachedTo is ComboBoxBase cbb && cbb.WidgetPath.Equals(self.WidgetPath))
                        {
                            w.SizeAllocated += W_SizeAllocated;
                            w.Shown += W_Shown;
                            break;
                        }
                    }
                }
            }
        }
        private void Ws_Drawn(object o, DrawnArgs args)
        {
            var ws = o as Gtk.ToggleButton;
            string text = self.ActiveText;
            Pango.Layout layout = ws.CreatePangoLayout(text);
            args.Cr.Save();
            args.Cr.Translate(10, 5);
            args.Cr.Rectangle(0, 0, ws.AllocatedWidth - 35, ws.AllocatedHeight - 5);
            args.Cr.Clip();
            Pango.CairoHelper.ShowLayout(args.Cr, layout);
            args.Cr.Restore();
        }
        private void W_SizeAllocated(object o, SizeAllocatedArgs args)
        {
            Gtk.Window w = (Gtk.Window)o;
            if (w.Window != null && w.Window.IsVisible)
            {
                self.Window.GetOrigin(out int x, out int y);
                int wx = x - 5;
                int wy = y + self.AllocatedHeight - 4;
                w.Window.GetOrigin(out int wx1, out int wy1);
                if (wx != wx1)
                {
                    w.Window.Move(wx, wy);
                    GLib.Timeout.Add(50, () =>
                    {
                        w.Window.GetOrigin(out int x1, out int y1);
                        if (wx == x1 && wy == y1)
                        {
                            w.Window.Opacity = 1;
                            w.Window.Show();
                            return false;
                        }
                        w.Window.Move(wx, wy);
                        return true;
                    });
                }
            }
        }
        private void W_Shown(object sender, EventArgs e)
        {
            Gtk.Window w= (Gtk.Window)sender;
            w.Window.Opacity = 0;
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
