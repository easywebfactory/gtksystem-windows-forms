using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
 

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public interface IControlGtk : IDisposable
    {
        GtkControlOverride Override { get; set; }
    }
}
