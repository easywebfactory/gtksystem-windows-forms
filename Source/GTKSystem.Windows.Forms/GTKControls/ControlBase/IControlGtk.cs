using System;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public interface IControlGtk : IDisposable
    {
        GtkControlOverride Override { get; set; }
    }
}
