namespace System.Windows.Forms;

public class ColumnStyle : TableLayoutStyle
{
    private float width;

    public float Width
    {
        get => width;
        set
        {
            if (value < 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            width = value;
        }
    }

    public ColumnStyle()
    {
			
    }

    public ColumnStyle(SizeType sizeType)
    {
        SizeType = sizeType;
    }

    public ColumnStyle(SizeType sizeType, float width)
    {
        SizeType = sizeType;
        if (width < 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(width));
        }
        Width = width;
    }
}