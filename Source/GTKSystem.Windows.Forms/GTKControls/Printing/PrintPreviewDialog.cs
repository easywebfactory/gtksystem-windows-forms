// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;

namespace System.Windows.Forms
{

    [DesignTimeVisible(true)]
    [DefaultProperty(nameof(Document))]
    [ToolboxItemFilter("System.Windows.Forms.Control.TopLevel")]
    [ToolboxItem(true)]
    public partial class PrintPreviewDialog : Form
    {
        private readonly PrintPreviewControl _previewControl;
        public PrintPreviewDialog()
        {

            _previewControl = new PrintPreviewControl();
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