using Gtk;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace System.Windows.Forms
{
    public class ControlCollection : ArrayList
    {
        Gtk.Container __owner;
        Type __itemType;
        CheckedListBox checkedListBox;
        public ControlCollection(Gtk.Container owner)
        {
            __owner = owner;
        }
        public ControlCollection(CheckedListBox owner, Type itemType)
        {
            checkedListBox = owner;
            __owner = owner.Container;
            __itemType = itemType;
        }

        public override int Add(object item)
        {
            if (item is IControl)
            {
                IControl widget = (IControl)item;
                __owner.Add(widget.Widget);
            }
            else
                __owner.Add((Widget)item);

            return base.Add(item);
        }

        public virtual void Add(Type itemType, object item)
        {
            //重载处理
        }

        public virtual void AddRange(object[] items)
        {
            foreach (object item in items)
            {
                if (item is String)
                    Add(__itemType, item);
                else
                    Add(item);
            }
        }
        public virtual void AddRange(Gtk.Widget[] items)
        {
            foreach (Gtk.Widget item in items)
                __owner.Add(item);
        }
        public override void Clear()
        {
            foreach (Widget wid in __owner.Children)
                __owner.Remove(wid);

            base.Clear();
        }

        public override void Insert(int index, object item) { __owner.Add((Widget)item); base.Insert(index, item); }

        public override void Remove(object value) { __owner.Remove((Widget)value); base.Remove(value); }
        public override void RemoveAt(int index) { __owner.Remove(__owner.Children[index]); base.RemoveAt(index); }
    }
    public class ControlCollection1 : IList, ICollection, IEnumerable
    {
        Gtk.Container __owner;
        Type __itemType;
        CheckedListBox checkedListBox;
        public ControlCollection1(Gtk.Container owner)
        {
            __owner = owner;
        }
        public ControlCollection1(CheckedListBox owner, Type itemType)
        {
            checkedListBox = owner;
            __owner = owner.Container;
            __itemType = itemType;
        }
        public object this[int index] { get { return __owner.Children[index]; } set { __owner.Children[index] = (Widget)value; } }
        public int Count { get { return __owner.Children.Length; } }

        public bool IsReadOnly { get; }

        public bool IsSynchronized => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();

        public bool IsFixedSize => throw new NotImplementedException();

        public virtual int Add(object item)
        {
           // Console.WriteLine(((Widget)item).Name);
            if (item is IControl)
            {
                IControl widget = (IControl)item;
                __owner.Add(widget.Widget);
            }
            else
                __owner.Add((Widget)item);

            return Count;
        }

        public virtual void Add(Type itemType, object item)
        {
            if (itemType == typeof(CheckBox) && checkedListBox != null)
            {
                CheckBox box = new CheckBox();
                box.Control.Label = item.ToString();
                box.Control.Toggled += Control_Toggled;
                ArrayList all = __owner.AllChildren as ArrayList;
                box.Control.Name = all.Count.ToString();
                __owner.Add(box.Widget);
            }
        }

        private void Control_Toggled(object sender, EventArgs e)
        {
             checkedListBox.SendEvent(sender,e);
        }

        public virtual void AddRange(object[] items)
        {
            foreach (object item in items)
            {
                if (item is String)
                    Add(__itemType, item);
                else
                    Add(item);
            }
        }
        public virtual void AddRange(Gtk.Widget[] items)
        {
            foreach (Gtk.Widget item in items)
                __owner.Add(item);
        }
        public virtual void Clear()
        {
            foreach (Widget wid in __owner.Children)
                __owner.Remove(wid);
        }

        public virtual bool Contains(object value)
        {
            return false;
        }

        public virtual void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerator GetEnumerator() { return __owner.GetEnumerator(); }

        public virtual int IndexOf(object value) { return Array.IndexOf(__owner.Children, value); }

        public virtual void Insert(int index, object item) { __owner.Add((Widget)item); }

        public virtual void Remove(object value) { __owner.Remove((Widget)value); }
        public virtual void RemoveAt(int index) { __owner.Remove(__owner.Children[index]); }
    }
}