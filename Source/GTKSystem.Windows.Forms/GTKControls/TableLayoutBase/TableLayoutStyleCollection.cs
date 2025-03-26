using System.Collections;

namespace System.Windows.Forms;

public abstract class TableLayoutStyleCollection : ArrayList
{
    internal IArrangedElement? Owner
    {
        get;
        private set;
    }

    public new TableLayoutStyle this[int index]
    {
        get => (TableLayoutStyle)base[index];
        set => base[index] = value;
    }
    internal TableLayoutStyleCollection(IArrangedElement? owner)
    {
        Owner = owner;
    }

    public int Add(TableLayoutStyle style)
    {
        return base.Add(style);
    }

    internal void EnsureOwnership(IArrangedElement? owner)
    {
        Owner = owner;
    }
}