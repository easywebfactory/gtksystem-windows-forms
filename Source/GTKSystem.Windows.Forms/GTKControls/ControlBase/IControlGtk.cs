using System.Windows.Forms.Interfaces;

namespace System.Windows.Forms;

public interface IControlGtk : IWidget
{
    IGtkControlOverride Override { get; set; }
}