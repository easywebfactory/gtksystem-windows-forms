// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    [DefaultProperty(nameof(Document))]
    public partial class PrintPreviewControl : Control
    {
        private const int ScrollSmallChange = 5;
        private const double DefaultZoom = .3;

        // Spacing per page, in mm
        private const int Border = 10;

        private static readonly object s_startPageChangedEvent = new object();

        private PrintDocument _document;
        private int _startPage;  // 0-based
        private int _rows = 1;
        private int _columns = 1;
        private bool _autoZoom = true;
        private Size _virtualSize = new Size(1, 1);
        private Point _position = new Point(0, 0);

        private bool _scrollLayoutPending;

        // The following are all computed by ComputeLayout
        private bool _layoutOk;

        // 100ths of an inch, not pixels
        private Size _imageSize = Size.Empty;
        private Point _screenDPI = Point.Empty;
        private double _zoom = DefaultZoom;
        private bool _pageInfoCalcPending;
        private bool _exceptionPrinting;

        public PrintPreviewControl()
        {
            ResetBackColor();
            ResetForeColor();
            Size = new Size(100, 100);
            SetStyle(ControlStyles.ResizeRedraw, false);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            TabStop = false;
        }

        [DefaultValue(false)]
        public bool UseAntiAlias
        {
            get; set;
        }

        [DefaultValue(true)]
        public bool AutoZoom
        {
            get => _autoZoom;
            set
            {
                if (_autoZoom != value)
                {
                    _autoZoom = value;
                   // InvalidateLayout();
                }
            }
        }

        [DefaultValue(DefaultZoom)]
        public double Zoom
        {
            get => _zoom;
            set
            {
                _autoZoom = false;
                _zoom = value;
                //InvalidateLayout();
            }
        }

        [DefaultValue(null)]
        public PrintDocument Document
        {
            get => _document;
            set
            {
                _document = value;
                InvalidatePreview();
            }
        }

        [DefaultValue(1)]
        public int Rows
        {
            get => _rows;
            set
            {
                _rows = value;
               // InvalidateLayout();
            }
        }

        [DefaultValue(1)]
        public int Columns
        {
            get => _columns;
            set
            {
                _columns = value;
                //InvalidateLayout();
            }
        }

        [DefaultValue(0)]
        public int StartPage
        {
            get
            {
                int value = _startPage;
                value = Math.Max(value, 0);

                return value;
            }
            set
            {
                int oldValue = StartPage;
                _startPage = value;
                if (oldValue != _startPage)
                {
                   // InvalidateLayout();
                    OnStartPageChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler StartPageChanged
        {
            add => Events.AddHandler(s_startPageChangedEvent, value);
            remove => Events.RemoveHandler(s_startPageChangedEvent, value);
        }

        protected virtual void OnStartPageChanged(EventArgs e)
        {
            if (Events[s_startPageChangedEvent] is EventHandler eh)
            {
                eh(this, e);
            }
        }

        [Localizable(true)]
        [AmbientValue(RightToLeft.Inherit)]
        public override RightToLeft RightToLeft
        {
            get => base.RightToLeft;
            set
            {
                base.RightToLeft = value;
                InvalidatePreview();
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        public override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler TextChanged
        {
            add => base.TextChanged += value;
            remove => base.TextChanged -= value;
        }

        [DefaultValue(false)]
        public new bool TabStop
        {
            get => base.TabStop;
            set => base.TabStop = value;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void ResetBackColor() => BackColor = SystemColors.AppWorkspace;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void ResetForeColor() => ForeColor = Color.White;
         
        public void InvalidatePreview()
        {
            if (!IsHandleCreated)
            {
                return;
            }
            _exceptionPrinting = false;
        }
    }
}
