using System.Drawing;

namespace System.Windows.Forms;

public interface IControlGtk : IWidget
{
    IGtkControlOverride Override { get; set; }
}