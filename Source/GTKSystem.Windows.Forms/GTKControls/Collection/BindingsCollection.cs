using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DefaultEvent("CollectionChanged")]
    public class BindingsCollection : BaseCollection
    {
        private List<Binding> _list = new List<Binding>();
        private CollectionChangeEventHandler? _onCollectionChanging;
        private CollectionChangeEventHandler? _onCollectionChanged;

        internal BindingsCollection()
        {
        }

        public override int Count => _list.Count;

        protected override ArrayList List => ArrayList.Adapter(_list);

        public Binding this[int index] => _list[index]!;

        protected internal void Add(Binding binding)
        {
            CollectionChangeEventArgs eventArgs = new CollectionChangeEventArgs(CollectionChangeAction.Add, binding);
            OnCollectionChanging(eventArgs);
            AddCore(binding);
            OnCollectionChanged(eventArgs);
        }

        protected virtual void AddCore(Binding dataBinding)
        {
           // ArgumentNullException.ThrowIfNull(dataBinding);
            _list.Add(dataBinding);
        }
        public event CollectionChangeEventHandler? CollectionChanging
        {
            add => _onCollectionChanging += value;
            remove => _onCollectionChanging -= value;
        }

        public event CollectionChangeEventHandler? CollectionChanged
        {
            add => _onCollectionChanged += value;
            remove => _onCollectionChanged -= value;
        }

        protected internal void Clear()
        {
            CollectionChangeEventArgs eventArgs = new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null);
            OnCollectionChanging(eventArgs);
            ClearCore();
            OnCollectionChanged(eventArgs);
        }
        protected virtual void ClearCore() => _list.Clear();

        protected virtual void OnCollectionChanging(CollectionChangeEventArgs e)
        {
            _onCollectionChanging?.Invoke(this, e);
        }

        protected virtual void OnCollectionChanged(CollectionChangeEventArgs ccevent)
        {
            _onCollectionChanged?.Invoke(this, ccevent);
        }

        protected internal void Remove(Binding binding)
        {
            CollectionChangeEventArgs eventArgs = new CollectionChangeEventArgs(CollectionChangeAction.Remove, binding);
            OnCollectionChanging(eventArgs);
            RemoveCore(binding);
            OnCollectionChanged(eventArgs);
        }

        protected internal void RemoveAt(int index) => Remove(this[index]);

        protected virtual void RemoveCore(Binding dataBinding) => _list.Remove(dataBinding);

        protected internal bool ShouldSerializeMyAll() => Count > 0;
    }
}