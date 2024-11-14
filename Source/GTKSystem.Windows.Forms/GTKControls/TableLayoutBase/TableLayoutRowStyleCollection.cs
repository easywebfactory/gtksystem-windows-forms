
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	public class TableLayoutRowStyleCollection : TableLayoutStyleCollection
	{

		public new RowStyle this[int index]
		{
			get { return (RowStyle)base[index]; }
			set { base[index] = value; }
		}

		internal TableLayoutRowStyleCollection(IArrangedElement Owner):base(Owner)
		{
			
		}

		internal TableLayoutRowStyleCollection() : base(null)
        {
			
		}

		public int Add(RowStyle rowStyle)
		{
			return base.Add(rowStyle);
		}

		public void Insert(int index, RowStyle rowStyle)
		{
			base.Insert(index, rowStyle);
		}

		public void Remove(RowStyle rowStyle)
		{
			 base.Remove(rowStyle);
		}

		public bool Contains(RowStyle rowStyle)
		{
            return base.Contains(rowStyle);
        }

		public int IndexOf(RowStyle rowStyle)
		{
			return base.IndexOf(rowStyle);
		}
	}
}
