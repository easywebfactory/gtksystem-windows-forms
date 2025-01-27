using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

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

        internal bool IsBindable =>
            BindableComponent != null
            && !string.IsNullOrEmpty(PropertyName)
            && DataSource != null;

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
            BindingMemberInfo = new BindingMemberInfo(dataMember);
            FormattingEnabled = formattingEnabled;
			DataSourceUpdateMode = dataSourceUpdateMode;
			NullValue = nullValue;
			FormatString = formatString;
			FormatInfo = formatInfo;
        }

        public void WriteValue()
		{
			object _dataSource = DataSource ?? this.DataSourceNullValue;
            if (this.Control != null && _dataSource != null)
            {
                WriteValueCore(_dataSource);
            }
        }
        private void WriteValueCore(object data)
        {
            Type type = this.Control.GetType();
            object val = type.GetProperty(PropertyName).GetValue(this.Control) ?? this.NullValue;
            if (val != null)
            {
                PropertyInfo propertyInfo = data.GetType().GetProperty(DataMember);
                object corVal = Convert.ChangeType(val, propertyInfo.PropertyType);
                if (Parse != null)
                {
                    Parse(this, new ConvertEventArgs(corVal, propertyInfo.PropertyType));
                }
                propertyInfo.SetValue(data, corVal);
            }
        }

        public void ReadValue()
        {
            object _dataSource = DataSource ?? this.DataSourceNullValue;
			if (this.Control != null && _dataSource != null)
			{
                if (_dataSource is IEnumerable list)
                {
                    foreach (object data in list)
                    {
                        ReadValueCore(data);
                    }
                }
                else
                {
                    ReadValueCore(_dataSource);
                }
            }
        }
		private void ReadValueCore(object data)
		{
            object val = data.GetType().GetProperty(DataMember).GetValue(data) ?? this.NullValue;
            if (FormattingEnabled)
            {
                if (!string.IsNullOrEmpty(FormatString))
                {
                    Type oriType = val.GetType();
                    val = string.Format(FormatString, val);
                    if (Format != null)
                        Format(this, new ConvertEventArgs(val, oriType));
                }
            }
            if (val != null)
            {
                Type type = this.Control.GetType();
                PropertyInfo propertyInfo = type.GetProperty(PropertyName);
                if (val.GetType().Equals(propertyInfo.PropertyType))
                {
                    propertyInfo.SetValue(data, val);
                }
                else
                {
                    object corVal = Convert.ChangeType(val, propertyInfo.PropertyType);
                    if (Parse != null)
                    {
                        Parse(this, new ConvertEventArgs(corVal, propertyInfo.PropertyType));
                    }
                    propertyInfo.SetValue(this.Control, corVal);
                }
            }
        }
    }
}
