using Cairo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace System.Windows.Forms
{
    public sealed class OpenFileDialog : FileDialog
    {
        public OpenFileDialog()
        {
            base.Action = Gtk.FileChooserAction.Open;
        }

        public bool Multiselect { get { return base.SelectMultiple; } set { base.SelectMultiple = value; } }

        public bool ReadOnlyChecked { get; set; }

        public bool ShowReadOnly { get; set; }

        public string SafeFileName { get; }


        public string[] SafeFileNames { get; }

        public Stream OpenFile() {
            if (System.IO.File.Exists(base.FileName))
                return System.IO.File.OpenRead(base.FileName);
            else
                return null;

        }

        public override void Reset() { }
    }
}
