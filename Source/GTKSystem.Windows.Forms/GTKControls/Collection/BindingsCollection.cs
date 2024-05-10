using Gtk;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	[DefaultEvent("CollectionChanged")]
	public class BindingsCollection : BaseCollection
    {
        private ArrayList _array = new ArrayList();
        private Control _owner;
        public BindingsCollection(Control owner)
        {
            _owner = owner;
        }
        public override int Count
        {
            get
            {
               return _array.Count;
            }
        }

        protected override ArrayList List
        {
            get
            {
                return _array;
            }
        }

        public Binding this[int index]
        {
            get
            {
                return (Binding)_array[index];
            }
        }

        internal BindingsCollection()
        {
            
        }

        protected internal void Add(Binding binding)
        {
            AddCore(binding);
        }

        protected virtual void AddCore(Binding dataBinding)
        {
            _array.Add(dataBinding);
        }

        protected internal void Clear()
        {
            ClearCore();
        }

        protected virtual void ClearCore()
        {
            _array.Clear();
        }

        protected virtual void OnCollectionChanging(CollectionChangeEventArgs e)
        {
            if(CollectionChanging!=null)
                CollectionChanging(_owner, e);
        }

        protected virtual void OnCollectionChanged(CollectionChangeEventArgs ccevent)
        {
            if (CollectionChanged != null)
                CollectionChanged(_owner, ccevent);
        }

        protected internal void Remove(Binding binding)
        {
            RemoveCore(binding);
        }

        protected internal void RemoveAt(int index)
        {
            _array.RemoveAt(index);
        }

        protected virtual void RemoveCore(Binding dataBinding)
        {
            _array.Remove(dataBinding);
        }

        protected internal bool ShouldSerializeMyAll()
        {
            return true;
        }
        public override IEnumerator GetEnumerator()
        {
            return _array.GetEnumerator();
        }
        public event CollectionChangeEventHandler CollectionChanging;

		public event CollectionChangeEventHandler CollectionChanged;

	}
}
