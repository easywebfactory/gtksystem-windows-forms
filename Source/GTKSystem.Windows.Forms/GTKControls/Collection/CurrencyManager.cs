using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	public class CurrencyManager : BindingManagerBase
	{
		protected int listposition;

		protected Type finalType;

		internal bool AllowAdd
		{
			get
			{
				throw null;
			}
		}

		internal bool AllowEdit
		{
			get
			{
				throw null;
			}
		}

		internal bool AllowRemove
		{
			get
			{
				throw null;
			}
		}

		public override int Count
		{
			get
			{
				throw null;
			}
		}

		public override object Current
		{
			get
			{
				throw null;
			}
		}

		internal override Type BindType
		{
			get
			{
				throw null;
			}
		}

		internal override object DataSource
		{
			get
			{
				throw null;
			}
		}

		internal override bool IsBinding
		{
			get
			{
				throw null;
			}
		}

		internal bool ShouldBind
		{
			get
			{
				throw null;
			}
		}

		public IList List
		{
			get
			{
				throw null;
			}
		}

		public override int Position
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		internal object this[int index]
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		public event ItemChangedEventHandler ItemChanged
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		public event ListChangedEventHandler ListChanged
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		public event EventHandler MetaDataChanged
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		internal CurrencyManager(object dataSource)
		{
			throw null;
		}

		private protected override void SetDataSource(object dataSource)
		{
			throw null;
		}

		public override void AddNew()
		{
			throw null;
		}

		public override void CancelCurrentEdit()
		{
			throw null;
		}

		protected void CheckEmpty()
		{
			throw null;
		}

		public override void RemoveAt(int index)
		{
			throw null;
		}

		public override void EndCurrentEdit()
		{
			throw null;
		}

		internal void SetSort(PropertyDescriptor property, ListSortDirection sortDirection)
		{
			throw null;
		}

		internal PropertyDescriptor GetSortProperty()
		{
			throw null;
		}

		internal ListSortDirection GetSortDirection()
		{
			throw null;
		}

		internal int Find(PropertyDescriptor property, object key, bool keepIndex)
		{
			throw null;
		}

		internal override string GetListName()
		{
			throw null;
		}

		protected internal override string GetListName(ArrayList listAccessors)
		{
			throw null;
		}

		internal override PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			throw null;
		}

		public override PropertyDescriptorCollection GetItemProperties()
		{
			throw null;
		}

		protected internal override void OnCurrentChanged(EventArgs e)
		{
			throw null;
		}

		protected internal override void OnCurrentItemChanged(EventArgs e)
		{
			throw null;
		}

		protected virtual void OnItemChanged(ItemChangedEventArgs e)
		{
			throw null;
		}

		protected internal void OnMetaDataChanged(EventArgs e)
		{
			throw null;
		}

		protected virtual void OnPositionChanged(EventArgs e)
		{
			throw null;
		}

		public void Refresh()
		{
			throw null;
		}

		internal void Release()
		{
			throw null;
		}

		public override void ResumeBinding()
		{
			throw null;
		}

		public override void SuspendBinding()
		{
			throw null;
		}

		internal void UnwireEvents(IList list)
		{
			throw null;
		}

		protected override void UpdateIsBinding()
		{
			throw null;
		}

		internal void WireEvents(IList list)
		{
			throw null;
		}
	}
}
