
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms
{
	[ListBindable(false)]
	public class TableLayoutControlCollection : ControlCollection
	{
		public TableLayoutPanel Container
		{
			[CompilerGenerated]
			get;
			protected set;
		}
        public TableLayoutControlCollection(Control owner, Gtk.Container ownerContainer) : base(owner, ownerContainer)
        {
            Container = owner as TableLayoutPanel;
        }
        public TableLayoutControlCollection(TableLayoutPanel container) : base(container)
        {
            Container = container;
        }

        public Dictionary<string, Control> GridControls = new Dictionary<string, Control>();
        public virtual void Add(Control control, int column, int row)
		{
            GridControls.Add($"{column},{row}", control);
            base.AddControl(control);
        }
    }
}
