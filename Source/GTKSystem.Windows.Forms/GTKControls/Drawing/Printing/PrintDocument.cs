// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using Gtk;
using System.ComponentModel;
using System.Windows.Forms;

namespace System.Drawing.Printing
{

    [DefaultProperty("DocumentName"), DefaultEvent("PrintPage")]
    public class PrintDocument : Component
    {
        private string _documentName = "document";

        private PrintEventHandler _beginPrintHandler;
        private PrintEventHandler _endPrintHandler;
        private PrintPageEventHandler _printPageHandler;
        private QueryPageSettingsEventHandler _queryHandler;

        private PrinterSettings _printerSettings = new PrinterSettings();
        private PageSettings _defaultPageSettings;

        //private PrintController _printController;

        private bool _originAtMargins;
        private bool _userSetPageSettings;

        public PrintDocument() 
        { 
            _defaultPageSettings = new PageSettings(_printerSettings);
            _pageSetup = new PageSetup();
        }
        private PageSetup _pageSetup;
        public PageSetup PageSetup { 
            get=> _pageSetup; 
            set {
                _pageSetup = value;
                PageSettings pageSettings = DefaultPageSettings;
                pageSettings.Landscape = value.Orientation == Gtk.PageOrientation.Landscape || value.Orientation == Gtk.PageOrientation.ReverseLandscape;
                pageSettings.Margins = new Margins((int)value.GetLeftMargin(Unit.Points), (int)value.GetTopMargin(Unit.Points), (int)value.GetRightMargin(Unit.Points), (int)value.GetBottomMargin(Unit.Points));
                Enum.TryParse(value.PaperSize.DisplayName, out PaperKind paperKind);
                pageSettings.PaperSize = new System.Drawing.Printing.PaperSize(paperKind, value.PaperSize.Name, (int)value.PaperSize.GetWidth(Unit.Points), (int)value.PaperSize.GetHeight(Unit.Points));
                _userSetPageSettings = true;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PageSettings DefaultPageSettings
        {
            get { return _defaultPageSettings; }
            set
            {
                _defaultPageSettings = value ?? new PageSettings();
                _userSetPageSettings = true;
            }
        }

        [DefaultValue("document")]
        public string DocumentName
        {
            get => _documentName;
            set => _documentName = value ?? "";
        }

        [DefaultValue(false)]
        public bool OriginAtMargins
        {
            get => _originAtMargins;
            set => _originAtMargins = value;
        }

        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public PrintController PrintController
        //{
        //    get => _printController ??= new StandardPrintController();
        //    set => _printController = value;
        //}

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PrinterSettings PrinterSettings
        {
            get => _printerSettings;
            set
            {
                value ??= new PrinterSettings();
                _printerSettings = value;

                // Reset the PageSettings that match the PrinterSettings only if we have created the defaultPageSettings.
                if (!_userSetPageSettings)
                {
                    _defaultPageSettings = _printerSettings.DefaultPageSettings;
                }
            }
        }

        public event PrintEventHandler BeginPrint
        {
            add => _beginPrintHandler += value;
            remove => _beginPrintHandler -= value;
        }

        public event PrintEventHandler EndPrint
        {
            add => _endPrintHandler += value;
            remove => _endPrintHandler -= value;
        }

        public event PrintPageEventHandler PrintPage
        {
            add => _printPageHandler += value;
            remove => _printPageHandler -= value;
        }

        public event QueryPageSettingsEventHandler QueryPageSettings
        {
            add => _queryHandler += value;
            remove => _queryHandler -= value;
        }

        protected internal virtual void OnBeginPrint(PrintEventArgs e) => _beginPrintHandler?.Invoke(this, e);

        protected internal virtual void OnEndPrint(PrintEventArgs e) => _endPrintHandler?.Invoke(this, e);

        protected internal virtual void OnPrintPage(PrintPageEventArgs e) => _printPageHandler?.Invoke(this, e);

        protected internal virtual void OnQueryPageSettings(QueryPageSettingsEventArgs e) => _queryHandler?.Invoke(this, e);

        public void Print()
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = this;
            printDialog.AllowPrintToFile = false;
            printDialog.PrintToFile = false;
            printDialog.Document.DocumentName = "PrintDoument";
            printDialog.RunPrint(null, true);
        }

        public override string ToString() => $"[PrintDocument {DocumentName}]";
    }
}