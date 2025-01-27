using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms
{
    [DefaultEvent("CollectionChanged")]
    public class ControlBindingsCollection : BindingsCollection
    {
        private Control _owner;
        public ControlBindingsCollection(Control owner) : base()
        {
            _owner = owner;
        }
        private void Control_Shown(object sender, EventArgs e)
        {
            foreach(Binding bin in this)
                bin.ReadValue();
        }

        public IBindableComponent BindableComponent
        {
            get
            {
                throw null;
            }
        }

        public Control Control
        {
            get => _owner;
        }

        public Binding this[string propertyName]
        {
            get
            {
                foreach (Binding binding in this)
                {
                    if (binding.PropertyName == propertyName)
                        return binding;
                }
                return null;
            }
        }

        public DataSourceUpdateMode DefaultDataSourceUpdateMode
        {
            [CompilerGenerated]
            get;
            [CompilerGenerated]
            set;
        }

        public ControlBindingsCollection(IBindableComponent control)
        {
        }
        public new void Add(Binding binding)
        {
            binding.Control = this.Control;
            if (binding.DataSource is INotifyPropertyChanged notify)
            {
                notify.PropertyChanged += (object sender, PropertyChangedEventArgs e) => {
                    binding.ReadValue();
                };
            }

            base.Add(binding);
        }

        public Binding Add(string propertyName, object dataSource, string dataMember)
        {
            return Add(propertyName, dataSource, dataMember, true, DataSourceUpdateMode.OnPropertyChanged, null, null, null);
        }

        public Binding Add(string propertyName, object dataSource, string dataMember, bool formattingEnabled)
        {
            return Add(propertyName, dataSource, dataMember, formattingEnabled, DataSourceUpdateMode.OnPropertyChanged, null, null, null);
        }

        public Binding Add(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode updateMode)
        {
            return Add(propertyName, dataSource, dataMember, formattingEnabled, updateMode, null, null, null);
        }

        public Binding Add(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode updateMode, object nullValue)
        {
            return Add(propertyName, dataSource, dataMember, formattingEnabled, updateMode, nullValue, null, null);
        }

        public Binding Add(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode updateMode, object nullValue, string formatString)
        {
            return Add(propertyName, dataSource, dataMember, formattingEnabled, updateMode, nullValue, formatString, null);
        }

        public Binding Add(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode updateMode, object nullValue, string formatString, IFormatProvider formatInfo)
        {
            Binding binding = new Binding(propertyName, dataSource, dataMember, formattingEnabled, updateMode, nullValue, formatString, formatInfo);
            this.Add(binding);
            return binding;
        }
        private void Binding_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Binding binding = (Binding)sender;
            binding.ReadValue();
        }
    }
}
