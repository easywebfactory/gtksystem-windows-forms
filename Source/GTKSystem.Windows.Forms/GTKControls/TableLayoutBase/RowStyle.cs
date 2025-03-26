namespace System.Windows.Forms;

public class RowStyle : TableLayoutStyle
{
    private float height;

    public float Height
    {
        get => height;
        set
        {
            if (value < 0.0f)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }
            height = value;
        }
    }

    public RowStyle()
    {
			
    }

    public RowStyle(SizeType sizeType)
    {
        SizeType = sizeType;
    }

    public RowStyle(SizeType sizeType, float height)
    {
        SizeType = sizeType;
        if (height < 0.0f)
        {
            throw new ArgumentOutOfRangeException(nameof(height));
        }
        Height = height;
    }
}