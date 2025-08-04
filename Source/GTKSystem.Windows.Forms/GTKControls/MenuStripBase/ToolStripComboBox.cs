/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{

    public class ToolStripComboBox : ToolStripItem
    {
        public StripToolItem self = new StripToolItem();
        public override IToolMenuItem Widget { get => self; }

        internal Gtk.ComboBoxText comboBox = Gtk.ComboBoxText.NewWithEntry();
        private ObjectCollection __itemsData;
        public ToolStripComboBox() : base()
        {
            comboBox.HasFrame = false;
            comboBox.Entry.HasFrame = false;
            comboBox.Entry.MaxWidthChars = 1;
            comboBox.Entry.WidthChars = 0;
            self.Add(comboBox);
            __itemsData = new ObjectCollection(this.comboBox);
            comboBox.Changed += ComboBox_Changed;
        }

        private void ComboBox_Changed(object sender, EventArgs e)
        {
            if(SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);
            if (SelectedValueChanged != null)
                SelectedValueChanged(this, e);
        }
        public override string Text { get => this.comboBox.Entry.Text; set => this.comboBox.Entry.Text = value; }
        public bool FormattingEnabled { get; set; }

        public object SelectedItem { get { return __itemsData[SelectedIndex]; } }
        public int SelectedIndex { get { return comboBox.Active; } }
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
