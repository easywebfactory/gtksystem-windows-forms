﻿using Gtk;
using Pango;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
        private bool OwnerContainerRealized = false;
        private void OwnerContainer_Realized(object sender, EventArgs e)
        {
            if (OwnerContainerRealized == false)
            {
                OwnerContainerRealized = true;
                PerformLayout();
            }
        }

        public ControlCollection(CheckedListBox owner, Type itemType)
        {
            __ownerControl = owner.self;
            checkedListBox = owner;
            __owner = owner;
            __itemType = itemType;
        }
        public virtual void PerformLayout()
        {
            foreach (object item in this)
            {
                AddToWidget(item);
            }
            __ownerControl.ShowAll();
        }
        internal Drawing.Point Offset = new Drawing.Point(0,0);
        private void AddToWidget(object item)
        {
            if (item is StatusStrip statusbar)
            {
                if (__owner is Form form)
                {
                    statusbar.self.Halign = Gtk.Align.Fill;
                    statusbar.self.Valign = Gtk.Align.Fill;
                    statusbar.self.Expand = true;
                    form.self.StatusBar.NoShowAll = false;
                    form.self.StatusBar.Visible = true;
                    form.self.StatusBar.HeightRequest = 35;
                    form.self.StatusBar.Child = statusbar.self;
                    form.self.StatusBar.ShowAll();
                }
            }
            else if (item is Control control)
            {
                if (fixedContainer != null)
                    fixedContainer.Put(control.Widget, control.Left + Offset.X, control.Top + Offset.Y);
                else if (layoutContainer != null)
                    layoutContainer.Put(control.Widget, control.Left + Offset.X, control.Top + Offset.Y);
                else
                    __ownerControl.Add(control.Widget);
            }
            else if (item is Gtk.Widget widget)
            {
                if (fixedContainer != null)
                    fixedContainer.Put(widget, widget.Allocation.X + Offset.X, widget.Allocation.Y + Offset.Y);
                else if (layoutContainer != null)
                    layoutContainer.Put(widget, widget.Allocation.X + Offset.X, widget.Allocation.Y + Offset.Y);
                else
                    __ownerControl.Add(widget);
            }
        }
        public override int Add(object item)
        {
            if (item is Control control)
            {
                control.Parent = __owner;
            }
            if (__ownerControl.IsRealized)
            {
                AddToWidget(item);
                __ownerControl.ShowAll();
            }
            return base.Add(item);
        }

        public int AddWidget(Gtk.Widget item, Control control)
        {
            control.Parent = __owner;
            if (__ownerControl.IsRealized)
            {
                AddToWidget(item);
                __ownerControl.ShowAll();
            }
            return base.Add(item);
        }
        public virtual void Add(Type itemType, object item)
        {
            //重载处理
            base.Add(item);
            if (__ownerControl.IsRealized)
            {
                __ownerControl.ShowAll();
            }
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

        public override void Remove(object value) { base.Remove(((Control)value).Widget); __ownerControl.Remove(((Control)value).Widget); }
        public override void RemoveAt(int index) { base.RemoveAt(index); __ownerControl.Remove(__ownerControl.Children[index]); }
    }
}