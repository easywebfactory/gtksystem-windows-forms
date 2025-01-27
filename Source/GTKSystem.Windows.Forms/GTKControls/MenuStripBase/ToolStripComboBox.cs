/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.Collections;
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
            this.comboBox.Changed += ComboBox_Changed;
        }

        private void ComboBox_Changed(object sender, EventArgs e)
        {
            if(SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);
            if (SelectedValueChanged != null)
                SelectedValueChanged(this, e);
        }

        public override Size Size { get => base.Size; set { this.comboBox.WidthRequest = value.Width; this.comboBox.HeightRequest = value.Height; base.Size = value; } }

        public bool FormattingEnabled { get; set; }

        public object SelectedItem { get { return __itemsData[SelectedIndex]; } }
        public int SelectedIndex { get { return base.comboBox.Active; } }
        public new ObjectCollection Items { get { return __itemsData; } }

        public event EventHandler SelectedIndexChanged;
        public event EventHandler SelectedValueChanged;

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
