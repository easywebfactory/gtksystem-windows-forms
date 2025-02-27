
using Gtk;
using System.ComponentModel;
using System.Drawing.Printing;

namespace System.Windows.Forms;

[DefaultProperty(nameof(Document))]
public sealed class PrintDialog : CommonDialog
{
    private PrinterSettings? _printerSettings;
    private PrintDocument? _printDocument;


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
        
    public PrintDocument? Document
    {
        get => _printDocument;
        set
        {
            _printDocument = value;
            _printerSettings = _printDocument is null ? new PrinterSettings() : _printDocument.PrinterSettings;
        }
    }

    private PageSettings? PageSettings => Document is null
        ? PrinterSettings!.DefaultPageSettings
        : Document.DefaultPageSettings;

                                
    [DefaultValue(null)]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        
    public PrinterSettings? PrinterSettings
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
        
    public bool UseExDialog { get; set; }


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

    protected override bool RunDialog(IWin32Window? owner)
    {
        return RunPrint(owner, true);
    }
    public bool RunPrint(IWin32Window? owner, bool showDialog)
    {
        try
        {
            var printOperation = new PrintOperation();
            printOperation.DefaultPageSetup = Document?.PageSetup;
            printOperation.Unit = Unit.Points;
            printOperation.ShowProgress = true;
            printOperation.UseFullPage = true;
            printOperation.RequestPageSetup += PrintOperation_RequestPageSetup;
            printOperation.DrawPage += PrintOperation_DrawPage;
            printOperation.BeginPrint += PrintOperation_BeginPrint;
            printOperation.EndPrint += PrintOperation_EndPrint;
            printOperation.Preview += PrintOperation_Preview;
            var window = Window.ListToplevels().LastOrDefault(o => o is FormBase && o.IsActive);
            if (PrintToFile && AllowPrintToFile)
            {
                var exportFileName = _printerSettings?.PrintFileName;
                if (string.IsNullOrWhiteSpace(exportFileName))
                    exportFileName = $"print_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
                else if (Path.GetExtension(exportFileName).ToLower() != ".pdf")
                    exportFileName += ".pdf";

                printOperation.ExportFilename = exportFileName;
                var result = printOperation.Run(PrintOperationAction.Export, owner == null ? window! : ((Form)owner).self);
                return result != PrintOperationResult.Cancel && result != PrintOperationResult.Error;
            }
            else
            {
                var result = printOperation.Run(PrintOperationAction.PrintDialog, owner == null ? window! : ((Form)owner).self);
                return result != PrintOperationResult.Cancel && result != PrintOperationResult.Error;
            }
        }catch(Exception ex)
        {
            var messageDialog = new MessageDialog(owner == null ? null : ((Form)owner).self, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "");
            messageDialog.WindowPosition = owner == null ? WindowPosition.Center : WindowPosition.CenterOnParent;
            if (ex.Message.ToLower().Contains("doc"))
                messageDialog.Text = "文件正在使用，无法覆盖写入";
            else
                messageDialog.Text = ex.Message;
            messageDialog.Response += MessageDialog_Response;
            messageDialog.ShowAll();
            return false;
        }
    }

    private void PrintOperation_Preview(object? o, PreviewArgs args)
    {
        args.Preview.RenderPage(0);
    }

    private void MessageDialog_Response(object? o, ResponseArgs args)
    {
        var messageDialog = o as MessageDialog;
        messageDialog?.Destroy();
    }

    private void PrintOperation_RequestPageSetup(object? o, RequestPageSetupArgs args)
    {
        _printDocument?.OnQueryPageSettings(new QueryPageSettingsEventArgs(PageSettings));
    }

    private void PrintOperation_EndPrint(object? o, EndPrintArgs args)
    {
        _printDocument?.OnEndPrint(new PrintEventArgs());
        //Console.WriteLine($"PrintOperation_EndPrint:{args.Context.Width},{args.Context.Height}");
    }

    private void PrintOperation_BeginPrint(object? o, BeginPrintArgs args)
    {
        _printDocument?.OnBeginPrint(new PrintEventArgs());
        var oper = o as PrintOperation;
        oper?.RenderPage(0);
        //Console.WriteLine($"PrintOperation_BeginPrint:{args.Context.Width},{args.Context.Height}");
    }

    private void PrintOperation_DrawPage(object? o, DrawPageArgs args)
    {
        var oper = o as PrintOperation;
        var setup = args.Context.PageSetup;
        using var cr = args.Context.CairoContext;
        //document size(794,1123)px
        var pxscale = 0.75;
        var top = (int)Math.Round(setup.GetTopMargin(oper!.Unit) / pxscale, 0);
        var left = (int)Math.Round(setup.GetLeftMargin(oper.Unit) / pxscale, 0);
        var right = (int)Math.Round(setup.GetRightMargin(oper.Unit) / pxscale, 0);
        var bottom = (int)Math.Round(setup.GetBottomMargin(oper.Unit) / pxscale, 0);
        var width = (int)Math.Round(setup.GetPaperWidth(oper.Unit) / pxscale, 0); //page<paper
        var height = (int)Math.Round(setup.GetPaperHeight(oper.Unit) / pxscale, 0);

        cr.Scale(pxscale, pxscale);
        cr.Translate(0, 0);
        _printDocument?.OnPrintPage(new PrintPageEventArgs(new Drawing.Graphics(cr, new Gdk.Rectangle(0, 0, width, height)), new Drawing.Rectangle(left, top, width - left - right, height - top - bottom), new Drawing.Rectangle(0, 0, width, height), PageSettings));
    }
}