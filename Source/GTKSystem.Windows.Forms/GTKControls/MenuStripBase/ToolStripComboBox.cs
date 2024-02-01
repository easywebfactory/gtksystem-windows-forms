/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{

    public class ToolStripComboBox : WidgetToolStrip<Gtk.MenuItem>
    {
        private ObjectCollection __itemsData;
        public ToolStripComboBox() : base("ToolStripComboBox",null)
        {
            __itemsData = new ObjectCollection(this.comboBox);
        }
        public override Size Size { get => base.Size; set { this.comboBox.WidthRequest = value.Width; this.comboBox.HeightRequest = value.Height; base.Size = value; } }

        public bool FormattingEnabled { get; set; }

        public object SelectedItem { get { return __itemsData[SelectedIndex]; } }
        public int SelectedIndex { get { return base.comboBox.Active; } }
        public new ObjectCollection Items { get { return __itemsData; } }

        public event EventHandler SelectedIndexChanged
        {
            add { base.comboBox.Changed += (object sender, EventArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
            remove { base.comboBox.Changed -= (object sender, EventArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
        }
        public event EventHandler SelectedValueChanged
        {
            add { base.comboBox.Changed += (object sender, EventArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
            remove { base.comboBox.Changed -= (object sender, EventArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
        }

        [ListBindable(false)]
        public class ObjectCollection : ArrayList
        {
            Gtk.ComboBoxText __owner;
            public ObjectCollection(Gtk.ComboBoxText owner)
            {
                __owner = owner;
            }
            public override int Add(object value)
            {
                __owner.AppendText(value?.ToString());
                return base.Add(value);
            }
            public override void Clear()
            {
                __owner.Clear();
                base.Clear();
            }
            public override void AddRange(ICollection c)
            {
                foreach (var item in c)
                    __owner.AppendText(item.ToString());
                base.AddRange(c);
            }
        }
    }

}
