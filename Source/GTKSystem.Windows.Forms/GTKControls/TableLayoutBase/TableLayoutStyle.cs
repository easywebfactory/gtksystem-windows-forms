using System.ComponentModel;

namespace System.Windows.Forms;

public abstract class TableLayoutStyle
{
    [DefaultValue(SizeType.AutoSize)]
    public SizeType SizeType
    {
        get;
        set;
    }

    internal float Size
    {
        get;
        set;
    }

    internal IArrangedElement? Owner
    {
        get;
        set;
    }

    internal void SetSize(float size)
    {
        Size = size;
    }
}