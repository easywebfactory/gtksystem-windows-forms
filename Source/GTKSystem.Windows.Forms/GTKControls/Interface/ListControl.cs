using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms
{
	[LookupBindingProperties("DataSource", "DisplayMember", "ValueMember", "SelectedValue")]
	public abstract class ListControl : Control
	{
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[AttributeProvider(typeof(IListSource))]
		public object DataSource
		{
			get;set;
		}

		[DefaultValue("")]
		public string DisplayMember
        {
            get; set;
        }

        [Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DefaultValue(null)]
		public IFormatProvider FormatInfo
        {
            get; set;
        }

        [DefaultValue("")]
		[MergableProperty(false)]
		public string FormatString
        {
            get; set;
        }

        [DefaultValue(false)]
		public bool FormattingEnabled
        {
            get; set;
        }

		[DefaultValue("")]
		public string ValueMember
        {
            get; set;
        }

        public abstract int SelectedIndex { get; set; }

		[DefaultValue(null)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Bindable(true)]
		public object SelectedValue
        {
            get; set;
        }

		public event EventHandler DataSourceChanged;

		public event EventHandler DisplayMemberChanged;

		public event ListControlConvertEventHandler Format;

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler FormatInfoChanged;

		public event EventHandler FormatStringChanged;

		public event EventHandler FormattingEnabledChanged;

		public event EventHandler ValueMemberChanged;

		public event EventHandler SelectedValueChanged;

		public string GetItemText(object item)
		{
			throw null;
		}

	}
}
