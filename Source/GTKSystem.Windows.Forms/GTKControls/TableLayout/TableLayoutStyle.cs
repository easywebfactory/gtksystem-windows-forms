using System.ComponentModel;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	public abstract class TableLayoutStyle
	{
		[DefaultValue(SizeType.AutoSize)]
		public SizeType SizeType
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		internal float Size
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		internal IArrangedElement? Owner
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		internal void SetSize(float size)
		{
			throw null;
		}

		protected TableLayoutStyle()
		{
			throw null;
		}
	}
}
