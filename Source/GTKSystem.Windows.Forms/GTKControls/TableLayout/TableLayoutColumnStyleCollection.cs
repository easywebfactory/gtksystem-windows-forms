using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	public class TableLayoutColumnStyleCollection : TableLayoutStyleCollection
	{
		internal override string PropertyName
		{
			get
			{
				throw null;
			}
		}

		public new ColumnStyle this[int index]
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

		internal TableLayoutColumnStyleCollection(IArrangedElement Owner):base(Owner)
		{
			throw null;
		}

		internal TableLayoutColumnStyleCollection() : base(null)
        {
			throw null;
		}

		public int Add(ColumnStyle columnStyle)
		{
			throw null;
		}

		public void Insert(int index, ColumnStyle columnStyle)
		{
			throw null;
		}

		public void Remove(ColumnStyle columnStyle)
		{
			throw null;
		}

		public bool Contains(ColumnStyle columnStyle)
		{
			throw null;
		}

		public int IndexOf(ColumnStyle columnStyle)
		{
			throw null;
		}
	}
}
