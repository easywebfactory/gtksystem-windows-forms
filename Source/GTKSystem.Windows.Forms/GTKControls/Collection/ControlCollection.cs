using Gtk;
using Pango;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace System.Windows.Forms
{
    public class ControlCollection : ArrayList
    {
        Gtk.Container __ownerControl;
        Control __owner;
        Type __itemType;
        CheckedListBox checkedListBox;
        public ControlCollection(Control owner)
        {
            __ownerControl = owner.GtkControl as Gtk.Container;
            __owner = owner;
        }
        public ControlCollection(Control owner, Gtk.Container ownerContainer)
        {
            __ownerControl = ownerContainer;
            __owner = owner;
        }
        public ControlCollection(CheckedListBox owner, Type itemType)
        {
            __ownerControl = owner.Container;
            checkedListBox = owner;
            __owner = owner;
            __itemType = itemType;
        }

        public override int Add(object item)
        {
            Control widget = (Control)item;
            widget.Parent = __owner;
            __ownerControl.Add(widget.Widget);

            return base.Add(item);
        }
        public int AddControl(object item)
        {
            return base.Add(item);
        }
        public int AddWidget(Gtk.Widget item)
        {
            __ownerControl.Add(item);
            return base.Add(item);
        }
        public virtual void Add(Type itemType, object item)
        {
            //重载处理
            base.Add(item);
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

        public override void Clear()
        {
            foreach (Widget wid in __ownerControl.Children)
                __ownerControl.Remove(wid);

            base.Clear();
        }

        public override void Insert(int index, object item) { __ownerControl.Add((Widget)item); base.Insert(index, item); }

        public override void Remove(object value) { __ownerControl.Remove((Widget)value); base.Remove(value); }
        public override void RemoveAt(int index) { __ownerControl.Remove(__ownerControl.Children[index]); base.RemoveAt(index); }
    }
}