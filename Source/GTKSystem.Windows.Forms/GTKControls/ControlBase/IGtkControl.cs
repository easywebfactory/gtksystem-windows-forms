using System.Windows.Forms.Interfaces;

namespace System.Windows.Forms;

public interface IGtkControl : IWidget
{
    IGtkControlOverride Override { get; set; }
}