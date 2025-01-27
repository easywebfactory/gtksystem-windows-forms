// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Drawing.Printing
{

    public unsafe class PageSettings : ICloneable
    {
        private PrinterSettings _printerSettings;
        private PaperSize _paperSize;
        private PaperSource _paperSource;
        private PrinterResolution _printerResolution;
        private Margins _margins = new Margins();

        public PageSettings() : this(new PrinterSettings())
        {
        }

        public PageSettings(PrinterSettings printerSettings)
        {
            _printerSettings = printerSettings;
        }

        public Rectangle Bounds
        {
            get;
            internal set;
        }

        public bool Color
        {
            get;
            set;
        }

        public float HardMarginX
        {
            get;
            internal set;
        }

        public float HardMarginY
        {
            get;
            internal set;
        }

        public bool Landscape
        {
            get;
            set;
        }

        public Margins Margins
        {
            get => _margins;
            set => _margins = value;
        }

        public PaperSize PaperSize
        {
            get;
            set;
        }

        public PaperSource PaperSource
        {
            get;
            set;
        }

        public RectangleF PrintableArea
        {
            get;
            internal set;
        }

        public PrinterResolution PrinterResolution
        {
            get;
            set;
        }

        public PrinterSettings PrinterSettings
        {
            get => _printerSettings;
            set => _printerSettings = value ?? new PrinterSettings();
        }

        public object Clone()
        {
            PageSettings result = (PageSettings)MemberwiseClone();
            result._margins = (Margins)_margins.Clone();
            return result;
        }

        public void CopyToHdevmode(IntPtr hdevmode)
        {
            
        }

        public void SetHdevmode(IntPtr hdevmode)
        {
        
        }

        public override string ToString() =>
            $"[{nameof(PageSettings)}: Color={Color}, Landscape={Landscape}, Margins={Margins}, PaperSize={PaperSize}, PaperSource={PaperSource}, PrinterResolution={PrinterResolution}]";
    }
}
