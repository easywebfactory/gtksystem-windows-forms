﻿/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */

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
        }

        private ComboBoxStyle _DropDownStyle;
        public ComboBoxStyle DropDownStyle { 
            get=> _DropDownStyle; 
            set {
                _DropDownStyle = value; 
                if(value==ComboBoxStyle.DropDown) {
                    self.StyleContext.AddClass("DropDown");
                }
                else if (value == ComboBoxStyle.DropDownList)
                {
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
