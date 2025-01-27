using System.ComponentModel;
 
namespace System.Windows.Forms
{
	[LookupBindingProperties("DataSource", "DisplayMember", "ValueMember", "SelectedValue")]
	public abstract class ListControl : ScrollableControl
    {
        protected new EventHandlerList Events = new EventHandlerList();
        [DefaultValue(null)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[AttributeProvider(typeof(IListSource))]
		public virtual object DataSource
		{
			get;set;
		}

		[DefaultValue("")]
        public virtual string DisplayMember
        {
            get; set;
        }

        [Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DefaultValue(null)]
        public virtual IFormatProvider FormatInfo
        {
            get; set;
        }

        [DefaultValue("")]
		[MergableProperty(false)]
        public virtual string FormatString
        {
            get; set;
        }

        [DefaultValue(false)]
        public virtual bool FormattingEnabled
        {
            get; set;
        }

		[DefaultValue("")]
        public virtual string ValueMember
        {
            get; set;
        }

        public abstract int SelectedIndex { get; set; }

		[DefaultValue(null)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Bindable(true)]
        public virtual object SelectedValue
        {
            get; set;
        }

		public event EventHandler DataSourceChanged
        {
            add { Events.AddHandler("DataSourceChanged", value); }
            remove { Events.RemoveHandler("DataSourceChanged", value); }
        }

        public event EventHandler DisplayMemberChanged;

		public event ListControlConvertEventHandler Format;

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler FormatInfoChanged;

		public event EventHandler FormatStringChanged;

        public event EventHandler FormattingEnabledChanged;

        public event EventHandler ValueMemberChanged;
        public event EventHandler SelectedItemChanged
        {
            add { Events.AddHandler("SelectedItemChanged", value); }
            remove { Events.RemoveHandler("SelectedItemChanged", value); }
        }
        public event EventHandler SelectedValueChanged
        {
            add { Events.AddHandler("SelectedValueChanged", value); }
            remove { Events.RemoveHandler("SelectedValueChanged", value); }
        }
        public event EventHandler SelectedIndexChanged
        {
            add { Events.AddHandler("SelectedIndexChanged", value); }
            remove { Events.RemoveHandler("SelectedIndexChanged", value); }
        }
        public virtual string GetItemText(object item)
		{
            return item?.ToString();
        }

	}
}
