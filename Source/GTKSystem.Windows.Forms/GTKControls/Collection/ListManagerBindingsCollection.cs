using System.ComponentModel;

namespace System.Windows.Forms;

[DefaultEvent("CollectionChanged")]
internal class ListManagerBindingsCollection : BindingsCollection
{
    private readonly BindingManagerBase? _bindingManagerBase;

    internal ListManagerBindingsCollection(BindingManagerBase? bindingManagerBase)
    {
        _bindingManagerBase = bindingManagerBase;
    }

    protected override void AddCore(Binding dataBinding)
    {
        if (dataBinding == null)
        {
            throw new ArgumentNullException("dataBinding");
        }
        if (dataBinding.BindingManagerBase == _bindingManagerBase)
        {
            throw new ArgumentException(nameof(dataBinding));
        }
        if (dataBinding.BindingManagerBase != null)
        {
            throw new ArgumentException(nameof(dataBinding));
        }
        dataBinding.SetListManager(_bindingManagerBase);
        base.AddCore(dataBinding);
    }

    protected override void ClearCore()
    {
        var count = Count;
        for (var i = 0; i < count; i++)
        {
            base[i].SetListManager(null);
        }
        base.ClearCore();
    }

    protected override void RemoveCore(Binding dataBinding)
    {
        if (dataBinding.BindingManagerBase != _bindingManagerBase)
        {
            throw new ArgumentException(nameof(dataBinding));
        }
        dataBinding.SetListManager(null);
        base.RemoveCore(dataBinding);
    }
}