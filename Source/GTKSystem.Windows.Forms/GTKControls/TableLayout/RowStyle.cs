namespace System.Windows.Forms
{
	public class RowStyle : TableLayoutStyle
	{
		public float Height
		{
			get;
			set;
		}

		public RowStyle()
		{
			
		}

		public RowStyle(SizeType sizeType)
		{
            this.SizeType = sizeType;
        }

		public RowStyle(SizeType sizeType, float height)
		{
            this.SizeType = sizeType;
            this.Height = height;
        }
	}
}
