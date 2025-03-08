using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Forms
{
    public enum HighDpiMode
    {
        DpiUnaware = 0,
        SystemAware = 1,
        PerMonitor = 2,
        PerMonitorV2 = 3,
        DpiUnawareGdiScaled = 4
    }
}
