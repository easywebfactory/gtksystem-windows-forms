using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms;

[DefaultEvent("CollectionChanged")]
public class BindingsCollection : BaseCollection
{
    private readonly List<Binding> _list = [];
    private CollectionChangeEventHandler? _collectionChanging;
    private CollectionChangeEventHandler? _collectionChanged;

    internal BindingsCollection()
    {
    }

    public override int Count => _list.Count;

    protected override ArrayList List => ArrayList.Adapter(_list);

    public Binding this[int index] => _list[index]!;

    protected internal void Add(Binding binding)
    {
        var eventArgs = new CollectionChangeEventArgs(CollectionChangeAction.Add, binding);
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
        add => _collectionChanging += value;
        remove => _collectionChanging -= value;
    }

    public event CollectionChangeEventHandler? CollectionChanged
    {
        add => _collectionChanged += value;
        remove => _collectionChanged -= value;
    }

    protected internal void Clear()
    {
        var eventArgs = new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null);
        OnCollectionChanging(eventArgs);
        ClearCore();
        OnCollectionChanged(eventArgs);
    }
    protected virtual void ClearCore() => _list.Clear();

    protected virtual void OnCollectionChanging(CollectionChangeEventArgs e)
    {
        _collectionChanging?.Invoke(this, e);
    }

    protected virtual void OnCollectionChanged(CollectionChangeEventArgs e)
    {
        _collectionChanged?.Invoke(this, e);
    }

    protected internal void Remove(Binding binding)
    {
        var eventArgs = new CollectionChangeEventArgs(CollectionChangeAction.Remove, binding);
        OnCollectionChanging(eventArgs);
        RemoveCore(binding);
        OnCollectionChanged(eventArgs);
    }

    protected internal void RemoveAt(int index) => Remove(this[index]);

    protected virtual void RemoveCore(Binding dataBinding) => _list.Remove(dataBinding);

    protected internal bool ShouldSerializeMyAll() => Count > 0;
}