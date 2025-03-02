using System.Runtime.CompilerServices;

namespace System.Windows.Forms;

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
        Column = column;
        Row = row;
    }

    public readonly override bool Equals(object? other)
    {
        if(other is TableLayoutPanelCellPosition cell)
        {
            return Equals(cell);
        }
        return false;
    }

    public readonly bool Equals(TableLayoutPanelCellPosition other)
    {
        return Column == other.Column && Row == other.Row;
    }

    public static bool operator ==(TableLayoutPanelCellPosition p1, TableLayoutPanelCellPosition p2)
    {
        return p1.Equals(p2);
    }

    public static bool operator !=(TableLayoutPanelCellPosition p1, TableLayoutPanelCellPosition p2)
    {
        return p1.Equals(p2) == false;
    }

    public readonly override string ToString()
    {
        return $"[{Column},{Row}]";
    }

    public readonly override int GetHashCode()
    {
        return Column.GetHashCode() & Row.GetHashCode();
    }
}