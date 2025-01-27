
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	public class TableLayoutColumnStyleCollection : TableLayoutStyleCollection
	{

		public new ColumnStyle this[int index]
		{
			get
			{
			 return	(ColumnStyle)base[index];
			}
			set
			{
				base[index] = value;

            }
		}

		internal TableLayoutColumnStyleCollection(IArrangedElement Owner):base(Owner)
		{
			
		}

		internal TableLayoutColumnStyleCollection() : base(null)
        {
			
		}

		public int Add(ColumnStyle columnStyle)
		{
			return base.Add(columnStyle);
		}

		public void Insert(int index, ColumnStyle columnStyle)
		{
            base.Insert(index, columnStyle);
        }

		public void Remove(ColumnStyle columnStyle)
		{
			base.Remove(columnStyle);
		}

		public bool Contains(ColumnStyle columnStyle)
		{
			return base.Contains(columnStyle);
		}

		public int IndexOf(ColumnStyle columnStyle)
		{
			return base.IndexOf(columnStyle);
		}
	}
}
