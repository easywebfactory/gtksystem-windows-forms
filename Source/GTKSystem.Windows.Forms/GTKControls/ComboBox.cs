/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms
{

    [DesignerCategory("Component")]
    public partial class ComboBox: Control
    {
        public readonly ComboBoxBase self = new ComboBoxBase();
        public override object GtkControl => self;
        private ObjectCollection __itemsData;
        public ComboBox():base()
        {
            self.Entry.HasFrame = false;
            self.Entry.WidthChars = 0;
            __itemsData = new ObjectCollection(self);
        }
        public bool FormattingEnabled { get; set; }
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
        public object SelectedItem { get { return __itemsData[SelectedIndex]; } }
        public int SelectedIndex { get { return self.Active; } }
        public ObjectCollection Items { get { return __itemsData; } }

        public event EventHandler SelectedIndexChanged
        {
            add { self.Changed += (object sender, EventArgs e) => { if (self.IsRealized) { value.Invoke(this, e); } }; }
            remove { self.Changed -= (object sender, EventArgs e) => { if (self.IsRealized) { value.Invoke(this, e); } }; }
        }
        public event EventHandler SelectedValueChanged
        {
            add { self.Changed += (object sender, EventArgs e) => { if (self.IsRealized) { value.Invoke(this, e); } }; }
            remove { self.Changed -= (object sender, EventArgs e) => { if (self.IsRealized) { value.Invoke(this, e); } }; }
        }
        [ListBindable(false)]
        public class ObjectCollection : IList, ICollection, IEnumerable, IDisposable
        {
            Gtk.ComboBoxText __owner;
            Gtk.ListStore __store;
            public ObjectCollection(Gtk.ComboBoxText owner) : base()
            {
                __owner = owner;
                __store = owner.Model as Gtk.ListStore;
            }

            public object this[int index]
            {
                get
                {
                    if (__store.GetIter(out Gtk.TreeIter iter, new Gtk.TreePath(new int[] { index })))
                        return __store.GetValue(iter, 0);
                    else
                        return null;
                }
                set
                {
                    if (__store.GetIter(out Gtk.TreeIter iter, new Gtk.TreePath(new int[] { index })))
                        __store.SetValue(iter, 0, Convert.ToString(value));
                    else
                    {
                        throw new Exception("索引值超出范围");
                    }
                }
            }

            public bool IsFixedSize => throw new NotImplementedException();

            public bool IsReadOnly => throw new NotImplementedException();

            public int Count
            {
                get
                {
                    var iEnumerator = __store.GetEnumerator();
                    int count = 0;
                    while (iEnumerator.MoveNext())
                        count++;
                    return count;
                }
            }
            public bool IsSynchronized => throw new NotImplementedException();

            public object SyncRoot => throw new NotImplementedException();

            public int Add(object value)
            {
                __owner.AppendText(Convert.ToString(value));
                return 1;
            }
            public void AddRange(object[] items)
            {
                foreach (object item in items)
                    Add(Convert.ToString(item));
            }
            public void Clear()
            {
                __store.Clear();
                __owner.Clear();
            }

            public bool Contains(object value)
            {
                var enumrator = __store.GetEnumerator();
                while (enumrator.MoveNext()) {
                    var vals = enumrator.Current as object[];
                    if (vals[0].Equals(value))
                        return true;
                }
                return false;
            }

            public void CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                __store.Clear();
                __owner.Dispose();
            }

            public IEnumerator GetEnumerator()
            {
                return __store.GetEnumerator();
            }

            public int IndexOf(object value)
            {
                int idx = -1;
                var enumrator = __store.GetEnumerator();
                while (enumrator.MoveNext())
                {
                    idx++;
                    var vals = enumrator.Current as object[];
                    if (vals[0].Equals(value))
                        break;
                }
                return idx;
            }

            public void Insert(int index, object value)
            {
                __owner.InsertText(index, value.ToString());
            }

            public void Remove(object value)
            {
                int position = this.IndexOf(value);
                if (position > -1)
                    __owner.Remove(position);
            }

            public void RemoveAt(int index)
            {
                __owner.Remove(index);
            }
        }
    }

}
