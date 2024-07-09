
using Gtk;
using System.ComponentModel;
using System.Drawing.Printing;

namespace System.Windows.Forms
{
    [DefaultProperty(nameof(Document))]
    public sealed class PrintDialog : CommonDialog
    {
        private PrinterSettings _printerSettings;
        private PrintDocument _printDocument;


        public PrintDialog() => Reset();

        
        [DefaultValue(false)]
        
        public bool AllowCurrentPage { get; set; }

                                
        [DefaultValue(false)]
        
        public bool AllowSomePages { get; set; }

                                
        [DefaultValue(true)]
        
        public bool AllowPrintToFile { get; set; }

                                
        [DefaultValue(false)]
        
        public bool AllowSelection { get; set; }

                                        
        [DefaultValue(null)]
        
        public PrintDocument Document
        {
            get => _printDocument;
            set
            {
                _printDocument = value;
                _printerSettings = _printDocument is null ? new PrinterSettings() : _printDocument.PrinterSettings;
            }
        }

        private PageSettings PageSettings => Document is null
            ? PrinterSettings.DefaultPageSettings
            : Document.DefaultPageSettings;

                                
        [DefaultValue(null)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        
        public PrinterSettings PrinterSettings
        {
            get => _printerSettings ??= new PrinterSettings();
            set
            {
                if (value != PrinterSettings)
                {
                    _printerSettings = value;
                    _printDocument = null;
                }
            }
        }

                                
        [DefaultValue(false)]
        
        public bool PrintToFile { get; set; }

                                
        [DefaultValue(false)]
        
        public bool ShowHelp { get; set; }

                                
        [DefaultValue(true)]
        
        public bool ShowNetwork { get; set; }                                                                                [DefaultValue(false)]
        
        public bool UseEXDialog { get; set; }


        public override void Reset()
        {
            AllowCurrentPage = false;
            AllowSomePages = false;
            AllowPrintToFile = true;
            AllowSelection = false;
            _printDocument = null;
            PrintToFile = false;
            _printerSettings = null;
            ShowHelp = false;
            ShowNetwork = true;
        }

        protected override bool RunDialog(IWin32Window owner)
        {
            Gtk.PrintOperation printOperation = new Gtk.PrintOperation();
            printOperation.DefaultPageSetup = Document.PageSetup;
            printOperation.Unit = Unit.Mm;
            printOperation.ShowProgress = true;
            printOperation.DrawPage += PrintOperation_DrawPage;
            printOperation.BeginPrint += PrintOperation_BeginPrint;
            printOperation.EndPrint += PrintOperation_EndPrint;
            printOperation.Done += PrintOperation_Done;

            Gtk.PrintOperationResult result = printOperation.Run(PrintOperationAction.PrintDialog, owner == null ? null : ((Form)owner).self);
            return true;
        }

        private void PrintOperation_Done(object o, DoneArgs args)
        {
            Console.WriteLine("PrintOperation_Done");
        }

        private void PrintOperation_EndPrint(object o, EndPrintArgs args)
        {
            Console.WriteLine($"PrintOperation_EndPrint:{args.Context.Width},{args.Context.Height}");
        }

        private void PrintOperation_BeginPrint(object o, BeginPrintArgs args)
        {
            Gtk.PrintOperation oper = (Gtk.PrintOperation)o;
            oper.RenderPage(0);
            //Console.WriteLine($"PrintOperation_BeginPrint:{args.Context.Width},{args.Context.Height}");
        }

        private void PrintOperation_DrawPage(object o, DrawPageArgs args)
        {
            Gtk.PrintOperation oper = (Gtk.PrintOperation)o;
            PageSetup setup = args.Context.PageSetup;
 
            //Console.WriteLine($"PrintOperation_DrawPage:left{setup.GetLeftMargin(oper.Unit)},top:{setup.GetTopMargin(oper.Unit)},right:{setup.GetRightMargin(oper.Unit)},bottom:{setup.GetBottomMargin(oper.Unit)}");
            //Console.WriteLine($"PrintOperation_DrawPage:PageWidth{setup.GetPageWidth(oper.Unit)},PaperWidth:{setup.GetPaperWidth(oper.Unit)}");
            //Console.WriteLine($"PrintOperation_DrawPage:PageNr{args.PageNr},{args.Context.Width},{args.Context.Height}");
            using (var cr = args.Context.CairoContext)
            {
                cr.Save();
                int top = (int)setup.GetTopMargin(oper.Unit);
                int left = (int)setup.GetLeftMargin(oper.Unit);
                int right = (int)setup.GetRightMargin(oper.Unit);
                int bottom = (int)setup.GetBottomMargin(oper.Unit);
                int width = (int)setup.GetPageWidth(oper.Unit); //page<paper
                int height = (int)setup.GetPageHeight(oper.Unit);

                Document?.OnPrintPage(new PrintPageEventArgs(new Drawing.Graphics(cr, new Gdk.Rectangle(left, top, width - left - right, height - top - bottom)), new Drawing.Rectangle(0, 0, width, height), new Drawing.Rectangle(left, top, width - left - right, height - top - bottom), PageSettings));
            }
        }
    }
}