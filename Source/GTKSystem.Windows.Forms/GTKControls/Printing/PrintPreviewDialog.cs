// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;

namespace System.Windows.Forms
{

    [DesignTimeVisible(true)]
    //[DefaultProperty(nameof(Document))]
    [ToolboxItemFilter("System.Windows.Forms.Control.TopLevel")]
    [ToolboxItem(true)]
    public partial class PrintPreviewDialog : ScrollableControl
    {
        public ViewportBase self = new ViewportBase();
        public override object GtkControl { get => self; }
        private PrintPreviewControl _previewControl;
        private Form previewForm;
        private Gtk.Box box;
        public PrintPreviewDialog():base()
        {
            
        }
        private void Printbutton_ButtonPressEvent(object o, Gtk.ButtonPressEventArgs args)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = _previewControl.Document;
            printDialog.AllowPrintToFile = false;
            printDialog.PrintToFile = false;
            printDialog.Document.DocumentName = "PrintDoument";
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
            int formwidth = _previewControl.Width + 100;
            int formheight = _previewControl.Height + 100;
            previewForm.ClientSize = new Size(formwidth, formheight);
            box = new Gtk.Box(Gtk.Orientation.Vertical, 15);
            Gtk.Box header = new Gtk.Box(Gtk.Orientation.Horizontal, 20);
            header.PackStart(new Gtk.Label("打印预览"), false, false, 0);
            Gtk.Button printbutton = new Gtk.Button("打印") { WidthRequest = 200 };
            printbutton.ButtonReleaseEvent += Printbutton_ButtonReleaseEvent;
            header.PackEnd(printbutton, false, false, 0);
            box.PackStart(header, false, true, 0);
            box.PackStart(_previewControl.Widget, false, true, 0);
            box.MarginTop = 20;
            box.MarginStart = (previewForm.Width - _previewControl.Width) / 2;
            previewForm.Controls.Add(box);
            previewForm.SizeChanged += PreviewForm_SizeChanged;
        }

        private void Printbutton_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = _previewControl.Document;
            printDialog.AllowPrintToFile = false;
            printDialog.PrintToFile = false;
            printDialog.Document.DocumentName = "PrintDoument";
            printDialog.ShowDialog(previewForm);
        }

        private void PreviewForm_SizeChanged(object sender, EventArgs e)
        {
            box.MarginStart = (previewForm.Width - _previewControl.Width) / 2;
        }

        public override void Show()
        {
            this.Show(null);
        }
        public void Show(IWin32Window owner)
        {
            if (owner == this)
            {
                throw new InvalidOperationException("OwnsSelfOrOwner");
            }
            Init();
            previewForm.Show(owner);
        }
        public DialogResult ShowDialog(IWin32Window owner)
        {
            Init();
            return previewForm.ShowDialog(owner);
        }
        public DialogResult ShowDialog()
        {
            return ShowDialog(null);
        }
        public System.Drawing.Icon Icon
        {
            get;
            set;
        }
        public IButtonControl AcceptButton
        {
            get;
            set;
        }
         
         
        public bool AutoScale
        {
            get;
            set;
        }

        public IButtonControl CancelButton
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

        
        public event EventHandler MaximumSizeChanged;
         
        public event EventHandler MinimumSizeChanged;

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
            get => PrintPreviewControl.UseAntiAlias;
            set => PrintPreviewControl.UseAntiAlias = value;
        }

        public PrintDocument Document
        {
            get;
            set;
        }

        public PrintPreviewControl PrintPreviewControl => _previewControl;

        public SizeGripStyle SizeGripStyle
        {
            get;
            set;
        }
 
    }
}