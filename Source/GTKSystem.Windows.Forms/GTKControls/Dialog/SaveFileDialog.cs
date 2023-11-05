using Cairo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Forms
{
    public sealed class SaveFileDialog : FileDialog
    {
        public SaveFileDialog()
        {
            base.Action=Gtk.FileChooserAction.Save;
        }
    }
}
