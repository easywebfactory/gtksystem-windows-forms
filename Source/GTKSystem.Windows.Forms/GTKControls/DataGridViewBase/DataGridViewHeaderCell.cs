using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	public class DataGridViewHeaderCell : DataGridViewCell
	{
		protected ButtonState ButtonState
		{
			get;
		}

		[Browsable(false)]
		public override bool Displayed
		{
			get;
		}

		internal Bitmap FlipXPThemesBitmap
		{
            get;
            set;
        }

		public override Type FormattedValueType
		{
			get;
		}

		[Browsable(false)]
		public override bool Frozen
		{
			get;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool ReadOnly
		{
            get;
            set;
        }

		[Browsable(false)]
		public override bool Resizable
		{
			get;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool Selected
		{
            get;
            set;
        }

		public override Type ValueType
		{
			get;
			set;
		}

		[Browsable(false)]
		public override bool Visible
		{
			get
			{
				return true;
			}
		}

		public DataGridViewHeaderCell()
		{

		}
	}
}
