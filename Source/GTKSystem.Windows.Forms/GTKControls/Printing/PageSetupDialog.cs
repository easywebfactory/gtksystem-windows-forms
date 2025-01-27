// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;

namespace System.Windows.Forms
{
    [DefaultProperty(nameof(Document))]
    public sealed class PageSetupDialog : CommonDialog
    {
        private PrintDocument _printDocument;
        private PageSettings _pageSettings;
        private PrinterSettings _printerSettings;

        private Margins _minMargins;

        public PageSetupDialog() => Reset();

        [DefaultValue(true)]
        public bool AllowMargins { get; set; }

        [DefaultValue(true)]
        public bool AllowOrientation { get; set; }

        [DefaultValue(true)]
        public bool AllowPaper { get; set; }

        [DefaultValue(true)]
        public bool AllowPrinter { get; set; }

        [DefaultValue(null)]
        public PrintDocument Document
        {
            get => _printDocument;
            set
            {
                _printDocument = value;
                if (_printDocument != null)
                {
                    _pageSettings = _printDocument.DefaultPageSettings;
                    _printerSettings = _printDocument.PrinterSettings;
                }
            }
        }

        [DefaultValue(false)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool EnableMetric { get; set; }

        public Margins MinMargins
        {
            get => _minMargins;
            set => _minMargins = value ?? new Margins(0, 0, 0, 0);
        }

        [DefaultValue(null)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PageSettings PageSettings
        {
            get => _pageSettings;
            set
            {
                _pageSettings = value;
                _printDocument = null;
            }
        }

        [DefaultValue(null)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PrinterSettings PrinterSettings
        {
            get => _printerSettings;
            set
            {
                _printerSettings = value;
                _printDocument = null;
            }
        }

        [DefaultValue(false)]
        public bool ShowHelp { get; set; }

        [DefaultValue(true)]
        public bool ShowNetwork { get; set; }

        public override void Reset()
        {
            AllowMargins = true;
            AllowOrientation = true;
            AllowPaper = true;
            AllowPrinter = true;
            MinMargins = null; // turns into Margin with all zeros
            _pageSettings = null;
            _printDocument = null;
            _printerSettings = null;
            ShowHelp = false;
            ShowNetwork = true;
        }

        protected override bool RunDialog(IWin32Window owner)
        {
            try
            {
                Gtk.Window window = Gtk.Window.ListToplevels().LastOrDefault(o => o is FormBase && o.IsActive);
                Gtk.PrintSettings printSettings = new Gtk.PrintSettings();
                PageSetup pageSetup = Gtk.Print.RunPageSetupDialog(owner == null ? window : ((Form)owner).self, _printDocument.PageSetup ?? new Gtk.PageSetup(), printSettings);
                _printDocument.PageSetup = pageSetup;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
