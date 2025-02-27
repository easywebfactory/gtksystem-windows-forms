
using System.ComponentModel;

namespace System.Windows.Forms
{
    public abstract class GridItem
    {
        [Localizable(false)]
        [Bindable(true)]
        [DefaultValue(null)]
        [TypeConverter(typeof(StringConverter))]
        public object Tag { get; set; }

        public abstract GridItemCollection GridItems { get; }

        public abstract GridItemType GridItemType { get; }

        public abstract string Label { get; }

        public abstract GridItem Parent { get; }

        public abstract PropertyDescriptor PropertyDescriptor { get; }

        public abstract object Value { get; }

        public virtual bool Expandable => false;

        public virtual bool Expanded
        {
            get;
            set;
        }

        public abstract bool Select();
    }
}
