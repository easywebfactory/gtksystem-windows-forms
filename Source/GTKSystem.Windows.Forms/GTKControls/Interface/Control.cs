using Gtk;
using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Forms
{
    public partial class Control: IEnumerable<Gtk.Widget>
    {
        private IEnumerable<Gtk.Widget> Object;
        public Gtk.Widget Widget { get; set; }
        public Control(Gtk.Widget obj)
        {
            Widget = obj;
            Object = new Gtk.Widget[] { obj };
        }
 
        public IEnumerator<Widget> GetEnumerator()
        {
            return Object.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
