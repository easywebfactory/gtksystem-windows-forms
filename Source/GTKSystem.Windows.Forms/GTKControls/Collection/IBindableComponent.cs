using System.ComponentModel;

namespace System.Windows.Forms;

public interface IBindableComponent : IComponent
{
    ControlBindingsCollection? DataBindings { get; }

    BindingContext? BindingContext { get; set; }
}