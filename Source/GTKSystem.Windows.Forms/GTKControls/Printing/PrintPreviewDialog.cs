// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Gtk;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;

namespace System.Windows.Forms;

[DesignTimeVisible(true)]
//[DefaultProperty(nameof(Document))]
[ToolboxItemFilter("System.Windows.Forms.Control.TopLevel")]
[ToolboxItem(true)]
public class PrintPreviewDialog : ScrollableControl
{
    public ViewportBase self = new();
    public override object GtkControl => self;
    private PrintPreviewControl? _previewControl;
    private Form? previewForm;
    private Box? box;

    private void Printbutton_ButtonPressEvent(object? o, ButtonPressEventArgs args)
    {
        var printDialog = new PrintDialog();
        printDialog.Document = _previewControl?.Document;
        printDialog.AllowPrintToFile = false;
        printDialog.PrintToFile = false;
        if (printDialog.Document != null)
        {
            printDialog.Document.DocumentName = "PrintDoument";
        }

        printDialog.ShowDialog(previewForm);
    }
    private void Init()
    {
        previewForm = new Form();
        previewForm.AutoScroll = true;
        previewForm.self.Resizable = true;
        _previewControl = new PrintPreviewControl();
        //document size(794,1123)px
        _previewControl.self.MarginBottom = 20;
        _previewControl.Zoom = 1;
        _previewControl.AutoZoom = true;
        _previewControl.Document = Document;
        var formwidth = _previewControl.Width + 100;
        var formheight = _previewControl.Height + 100;
        previewForm.ClientSize = new Size(formwidth, formheight);
        box = new Box(Gtk.Orientation.Vertical, 15);
        var header = new Box(Gtk.Orientation.Horizontal, 20);
        header.PackStart(new Gtk.Label("打印预览"), false, false, 0);
        var printbutton = new Gtk.Button("打印") { WidthRequest = 200 };
        printbutton.ButtonReleaseEvent += Printbutton_ButtonReleaseEvent;
        header.PackEnd(printbutton, false, false, 0);
        box.PackStart(header, false, true, 0);
        var widget = _previewControl.Widget as Widget;
        if (widget != null) box.PackStart(widget, false, true, 0);
        box.MarginTop = 20;
        box.MarginStart = (previewForm.Width - _previewControl.Width) / 2;
        previewForm.Controls.Add(box);
        previewForm.SizeChanged += PreviewForm_SizeChanged;
    }

    private void Printbutton_ButtonReleaseEvent(object? o, ButtonReleaseEventArgs args)
    {
        var printDialog = new PrintDialog();
        printDialog.Document = _previewControl?.Document;
        printDialog.AllowPrintToFile = false;
        printDialog.PrintToFile = false;
        if (printDialog.Document != null)
        {
            printDialog.Document.DocumentName = "PrintDoument";
        }

        printDialog.ShowDialog(previewForm);
    }

    private void PreviewForm_SizeChanged(object? sender, EventArgs e)
    {
        if (box != null)
        {
            box.MarginStart = (previewForm?.Width ?? 0 - _previewControl?.Width ?? 0) / 2;
        }
    }

    public override void Show()
    {
        Show(null);
    }
    public void Show(IWin32Window? owner)
    {
        if (ReferenceEquals(owner, this))
        {
            throw new InvalidOperationException("OwnsSelfOrOwner");
        }
        Init();
        previewForm?.Show(owner);
    }
    public DialogResult ShowDialog(IWin32Window? owner)
    {
        Init();
        return previewForm?.ShowDialog(owner)??DialogResult.None;
    }
    public DialogResult ShowDialog()
    {
        return ShowDialog(null);
    }
    public Drawing.Icon? Icon
    {
        get;
        set;
    }
    public IButtonControl? AcceptButton
    {
        get;
        set;
    }


    public bool AutoScale
    {
        get;
        set;
    }

    public IButtonControl? CancelButton
    {
        get;
        set;
    }



    public bool ControlBox
    {
        get;
        set;
    }
    public bool HelpButton
    {
        get;
        set;
    }

    public bool IsMdiContainer
    {
        get;
        set;
    }

    public bool KeyPreview
    {
        get;
        set;
    }


#pragma warning disable CS0067 // Event is never used
    public event EventHandler? MaximumSizeChanged;

    public event EventHandler? MinimumSizeChanged;
#pragma warning restore CS0067 // Event is never used

    public FormStartPosition StartPosition
    {
        get;
        set;
    }
    public bool TopMost
    {
        get;
        set;
    }

    public bool UseAntiAlias
    {
        get => PrintPreviewControl?.UseAntiAlias??false;
        set
        {
            if (PrintPreviewControl != null)
            {
                PrintPreviewControl.UseAntiAlias = value;
            }
        }
    }

    public PrintDocument? Document
    {
        get;
        set;
    }

    public PrintPreviewControl? PrintPreviewControl => _previewControl;

    public SizeGripStyle SizeGripStyle
    {
        get;
        set;
    }

}