using System.Runtime.CompilerServices;

namespace System.Windows.Forms
{

	public struct TableLayoutPanelCellPosition : IEquatable<TableLayoutPanelCellPosition>
	{
		public int Row
		{
			[CompilerGenerated]
			readonly get;
			[CompilerGenerated]
			set;
		}

		public int Column
		{
			[CompilerGenerated]
			readonly get;
			[CompilerGenerated]
			set;
		}

		public TableLayoutPanelCellPosition(int column, int row)
		{
			this.Column = column;
			this.Row = row;
		}

		public override readonly bool Equals(object other)
		{
			if(other is TableLayoutPanelCellPosition cell)
			{
                return Equals(cell);
            }
			return false;
		}

		public readonly bool Equals(TableLayoutPanelCellPosition other)
		{
             return this.Column == other.Column && this.Row == other.Row;
        }

		public static bool operator ==(TableLayoutPanelCellPosition p1, TableLayoutPanelCellPosition p2)
		{
			return p1.Equals(p2);
		}

		public static bool operator !=(TableLayoutPanelCellPosition p1, TableLayoutPanelCellPosition p2)
		{
            return p1.Equals(p2) == false;
        }

		public override readonly string ToString()
		{
			return $"[{this.Column},{this.Row}]";
		}

		public override readonly int GetHashCode()
		{
			return this.Column.GetHashCode() & this.Row.GetHashCode();
		}
	}
}
