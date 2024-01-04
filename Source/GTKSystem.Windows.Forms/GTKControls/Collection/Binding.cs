using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms.GtkRender;
using System.Xml;

namespace System.Windows.Forms
{
	public class Binding
	{
		public object DataSource
		{
			[CompilerGenerated]
			get;
            internal set;
        }

		public BindingMemberInfo BindingMemberInfo
		{
			[CompilerGenerated]
			get;
            internal set;
        }

		[DefaultValue(null)]
		public IBindableComponent BindableComponent
		{
			[CompilerGenerated]
			get;
            internal set;
        }

		[DefaultValue(null)]
		public Control Control
		{
			get;
            internal set;
        }

		public bool IsBinding
		{
			[CompilerGenerated]
			get;
            internal set;
        }

		public BindingManagerBase BindingManagerBase
		{
			get;
			internal set;
		}

        [DefaultValue("")]
		public string PropertyName
		{
			[CompilerGenerated]
			get;
            internal set;
        }
        [DefaultValue("")]
        public string DataMember
        {
            [CompilerGenerated]
            get;
            internal set;
        }
        [DefaultValue(false)]
		public bool FormattingEnabled
		{
            get; set;
        }

		[DefaultValue(null)]
		public IFormatProvider FormatInfo
		{
            get; set;
        }

		public string FormatString
		{
            get; set;
        }

		public object NullValue
		{
            get; set;
        }

		public object DataSourceNullValue
		{
            get; set;
        }

		public ControlUpdateMode ControlUpdateMode
		{
            get; set;
        }

		public DataSourceUpdateMode DataSourceUpdateMode
		{
			get;set;
		}

		public event BindingCompleteEventHandler BindingComplete;
		public event ConvertEventHandler Parse;

		public event ConvertEventHandler Format;
		public Binding(string propertyName, object dataSource, string dataMember) : this(propertyName, dataSource, dataMember, true, DataSourceUpdateMode.OnPropertyChanged, null, null, null)
        {
        }

		public Binding(string propertyName, object dataSource, string dataMember, bool formattingEnabled) : this(propertyName, dataSource, dataMember, formattingEnabled, DataSourceUpdateMode.OnPropertyChanged, null, null, null)
        {
        }

		public Binding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode) : this(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, null, null, null)
        {
        }

		public Binding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object nullValue) : this(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, nullValue, null, null)
        {
			
		}

		public Binding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object nullValue, string formatString) : this(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, nullValue, formatString, null)
        {
			
		}

		public Binding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object nullValue, string formatString, IFormatProvider formatInfo)
		{
            PropertyName = propertyName;
            DataSource = dataSource;
            DataMember = dataMember;
			FormattingEnabled = formattingEnabled;
			DataSourceUpdateMode = dataSourceUpdateMode;
			NullValue = nullValue;
			FormatString = formatString;
			FormatInfo = formatInfo;
        }
		public void ReadValue()
		{
			
		}

		public void WriteValue()
		{
			
		}
	}
}
