﻿using Gtk;
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
        Gtk.Fixed fixedContainer;
        Gtk.Layout layoutContainer;
        public ControlCollection(Control owner)
        {
            __ownerControl = owner.GtkControl as Gtk.Container;
            __owner = owner;
            if (owner.GtkControl is Gtk.Fixed gtkfixed)
                fixedContainer = gtkfixed;
            if (owner.GtkControl is Gtk.Layout gtklayout)
                layoutContainer = gtklayout;

            __ownerControl.Realized += OwnerContainer_Realized;
        }
        public ControlCollection(Control owner, Gtk.Container ownerContainer)
        {
            __ownerControl = ownerContainer;
            __owner = owner;
            if (ownerContainer is Gtk.Fixed gtkfixed)
                fixedContainer = gtkfixed;
            if (ownerContainer is Gtk.Layout gtklayout)
                layoutContainer = gtklayout;

            __ownerControl.Realized += OwnerContainer_Realized;
        }

        private void OwnerContainer_Realized(object sender, EventArgs e)
        {
            PerformLayout();
        }

        public ControlCollection(CheckedListBox owner, Type itemType)
        {
            __ownerControl = owner.Container;
            checkedListBox = owner;
            __owner = owner;
            __itemType = itemType;
        }
        public virtual void PerformLayout()
        {
            foreach (object item in this)
            {
                if (item is Control control)
                {
                    if (fixedContainer != null)
                        fixedContainer.Put(control.Widget, control.Left, control.Top);
                    else if (layoutContainer != null)
                        layoutContainer.Put(control.Widget, control.Left, control.Top);
                    else
                        __ownerControl.Add(control.Widget);
                }
                else if (item is Gtk.Widget widget)
                {
                    if (fixedContainer != null)
                        fixedContainer.Put(widget, widget.Allocation.X, widget.Allocation.Y);
                    else if (layoutContainer != null)
                        layoutContainer.Put(widget, widget.Allocation.X, widget.Allocation.Y);
                    else
                        __ownerControl.Add(widget);
                }
            }
            __ownerControl.ShowAll();
        }
        public override int Add(object item)
        {
            Control widget = (Control)item;
            widget.Parent = __owner;
            //__ownerControl.Add(widget.Widget);
            //if (fixedContainer != null)
            //    fixedContainer.Put(widget.Widget, widget.Left, widget.Top);
            //else if (layoutContainer != null)
            //    layoutContainer.Put(widget.Widget, widget.Left, widget.Top);
            //else
            //    __ownerControl.Add(widget.Widget);
            return base.Add(item);
        }

        public int AddControl(object item)
        {
            return base.Add(item);
        }
        public int AddWidget(Gtk.Widget item)
        {
           // __ownerControl.Add(item);
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