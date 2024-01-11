namespace System.Windows.Forms
{
	public class ColumnStyle : TableLayoutStyle
	{
		public float Width
		{
            get;
            set;
        }

		public ColumnStyle()
		{
			
		}
		public ColumnStyle(SizeType sizeType)
		{
            this.SizeType = sizeType;
		}

		public ColumnStyle(SizeType sizeType, float width)
		{
            this.SizeType = sizeType;
			this.Width = width;
        }
	}
}
