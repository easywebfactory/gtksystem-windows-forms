using Gtk;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms
{
	[ListBindable(false)]
	public class TableLayoutControlCollection : ControlCollection
	{
		public TableLayoutPanel Container
		{
			[CompilerGenerated]
			get
			{
				throw null;
            }
		}
        public TableLayoutControlCollection(Control owner, Gtk.Container ownerContainer) : base(owner, ownerContainer)
        {
 
        }
        public TableLayoutControlCollection(TableLayoutPanel container) : base(container)
        {
			
		}

		public virtual void Add(Control control, int column, int row)
		{
			throw null;
		}
	}
}
