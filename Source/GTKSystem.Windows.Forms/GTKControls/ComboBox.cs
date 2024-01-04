/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{

    [DesignerCategory("Component")]
    public partial class ComboBox: WidgetControl<Gtk.ComboBox>
    {
        private ObjectCollection __itemsData;
        public ComboBox():base()
        {
            Widget.StyleContext.AddClass("ComboBox");
            Widget.StyleContext.AddClass("BorderRadiusStyle");
            __itemsData = new ObjectCollection(this);
        }
        public ComboBox(Gtk.ITreeModel model):base(model)
        {
            Widget.StyleContext.AddClass("ComboBox");
            Widget.StyleContext.AddClass("BorderRadiusStyle");
            __itemsData = new ObjectCollection(this);
        }
        

        public bool FormattingEnabled { get; set; }

        public object SelectedItem { get { return __itemsData[SelectedIndex]; } }
        public int SelectedIndex { get { return base.Control.Active; } }
        public ObjectCollection Items { get { return __itemsData; } }

        public event EventHandler SelectedIndexChanged
        {
            add { base.Control.Changed += (object sender, EventArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
            remove { base.Control.Changed -= (object sender, EventArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
        }
        public event EventHandler SelectedValueChanged
        {
            add { base.Control.Changed += (object sender, EventArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
            remove { base.Control.Changed -= (object sender, EventArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
        }

        [ListBindable(false)]
        public class ObjectCollection : Gtk.TreeStore,IList
        {
            Gtk.ComboBox __owner;
            public ObjectCollection(ComboBox owner):base(typeof(string))
            {
                __owner = owner.Control;
                __owner.Model = this;
                var textCell = new Gtk.CellRendererText();
                __owner.PackStart(textCell, true);
                __owner.AddAttribute(textCell, "text", 0);
            }

            public object this[int index] { get {

                    if (base.GetIter(out Gtk.TreeIter iter, new Gtk.TreePath(new int[] { index })))
                        return base.GetValue(iter, 0);
                    else
                        return null;
                }
                set {
                    if (base.GetIter(out Gtk.TreeIter iter, new Gtk.TreePath(new int[] { index })))
                        base.SetValue(iter, 0, Convert.ToString(value));
                    else
                    {
                        throw new Exception("索引值超出范围");
                    }
                }
            }

            public bool IsFixedSize => throw new NotImplementedException();

            public bool IsReadOnly => throw new NotImplementedException();

            public int Count => throw new NotImplementedException();

            public bool IsSynchronized => throw new NotImplementedException();

            public object SyncRoot => throw new NotImplementedException();

            public int Add(object item)
            {
                this.AppendValues(Convert.ToString(item));
                return 1;
            }
            public void AddRange(object[] items)
            {
                foreach(object item in items )
                    this.AppendValues(Convert.ToString(item));
            }

            public bool Contains(object value)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            public IEnumerator GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public int IndexOf(object value)
            {
                throw new NotImplementedException();
            }

            public void Insert(int index, object value)
            {
                throw new NotImplementedException();
            }

            public void Remove(object value)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }
        }
    }

}
