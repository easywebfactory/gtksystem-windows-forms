/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */

using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms;

public class ToolStripComboBox : WidgetToolStrip<Gtk.MenuItem>
{
    private readonly ObjectCollection itemsData;
    public ToolStripComboBox() : base("ToolStripComboBox",[])
    {
        itemsData = new ObjectCollection(comboBox);
        comboBox.Changed += ComboBox_Changed;
    }

    private void ComboBox_Changed(object? sender, EventArgs e)
    {
        SelectedIndexChanged?.Invoke(this, e);
        SelectedValueChanged?.Invoke(this, e);
    }

    public override Size Size { get => base.Size; set { comboBox.WidthRequest = value.Width; comboBox.HeightRequest = value.Height; base.Size = value; } }

    public bool FormattingEnabled { get; set; }

    public object SelectedItem => itemsData[SelectedIndex];
    public int SelectedIndex => comboBox.Active;
    public new ObjectCollection Items => itemsData;

    public event EventHandler? SelectedIndexChanged;
    public event EventHandler? SelectedValueChanged;

    [ListBindable(false)]
    public class ObjectCollection : ArrayList
    {
        readonly Gtk.ComboBoxText owner;
        public ObjectCollection(Gtk.ComboBoxText owner)
        {
            this.owner = owner;
        }
        public override int Add(object? value)
        {
            owner.AppendText(value?.ToString());
            return base.Add(value);
        }
        public override void Clear()
        {
            owner.Clear();
            base.Clear();
        }
        public override void AddRange(ICollection c)
        {
            foreach (var item in c)
                owner.AppendText(item.ToString());
            base.AddRange(c);
        }
    }
}