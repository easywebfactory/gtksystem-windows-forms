// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;

namespace System.Windows.Forms
{
    [DefaultProperty(nameof(Document))]
    public partial class PrintPreviewControl : Control
    {
        public readonly ViewportBase self = new ViewportBase();
        public override object GtkControl => self;
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
        private double _zoom = DefaultZoom;
        private Gtk.ScrolledWindow scroll;
        private ViewportBase paper;
        private const double pxscale = 0.75;
        public PrintPreviewControl()
        {
            Size = new Size(600, 500);
            self.StyleContext.AddClass("PrintPreviewBack");
            
            paper = new ViewportBase();
            paper.Valign = Align.Center;
            paper.Halign = Align.Center;
            paper.StyleContext.AddClass("Paper");
            paper.Drawn += paper_Drawn;
            scroll = new Gtk.ScrolledWindow();
            scroll.Child = paper;
            self.Add(scroll);
            self.Realized += Self_Shown;

            ResetBackColor();
            ResetForeColor();
        }

        private void Self_Shown(object sender, EventArgs e)
        {
            if (AutoZoom == false)
            {
                paper.WidthRequest = (int)Math.Round(_document.PageSetup.GetPaperWidth(Unit.Points) * _zoom / 0.75, 0);
                paper.HeightRequest = (int)Math.Round(_document.PageSetup.GetPaperHeight(Unit.Points) * _zoom / 0.75, 0);
            }
        }
        private void paper_Drawn(object o, Gtk.DrawnArgs args)
        {
            if (_document != null)
            {
                PageSetup setup = _document.PageSetup;
                _document.OnBeginPrint(new PrintEventArgs());
                var cr = args.Cr;
                int top = (int)Math.Round(setup.GetTopMargin(Unit.Points) / pxscale, 0);
                int left = (int)Math.Round(setup.GetLeftMargin(Unit.Points) / pxscale, 0);
                int right = (int)Math.Round(setup.GetRightMargin(Unit.Points) / pxscale, 0);
                int bottom = (int)Math.Round(setup.GetBottomMargin(Unit.Points) / pxscale, 0);
                int width = (int)Math.Round(setup.GetPaperWidth(Unit.Points) / pxscale, 0); //page<paper
                int height = (int)Math.Round(setup.GetPaperHeight(Unit.Points) / pxscale, 0);

                if (AutoZoom == false)
                {

                    paper.WidthRequest = (int)Math.Round(width * _zoom, 0);
                    paper.HeightRequest = (int)Math.Round(height * _zoom, 0);
                    self.WidthRequest = paper.WidthRequest + 100;
                    self.HeightRequest = paper.HeightRequest + 100;
                    cr.Scale(_zoom, _zoom);
                }
                cr.Translate(0, 0);
                cr.Rectangle(0, 0, width, height);
                cr.SetSourceRGBA(ForeColor.R / 255, ForeColor.G / 255, ForeColor.B / 255, ForeColor.A / 255);
                cr.Fill();
                _document.OnPrintPage(new PrintPageEventArgs(new Drawing.Graphics((Gtk.Widget)o, cr, new Gdk.Rectangle(0, 0, width, height)), new Drawing.Rectangle(left, top, width - left - right, height - top - bottom), new Drawing.Rectangle(0, 0, width, height), _document.DefaultPageSettings));
                _document.OnEndPrint(new PrintEventArgs());
            }
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
            }
        }

        [DefaultValue(null)]
        public PrintDocument Document
        {
            get => _document;
            set
            {
                _document = value;
                paper.WidthRequest = (int)Math.Round(_document.PageSetup.GetPaperWidth(Unit.Points) / pxscale, 0);
                paper.HeightRequest = (int)Math.Round(_document.PageSetup.GetPaperHeight(Unit.Points) / pxscale, 0);
                self.WidthRequest = paper.WidthRequest + 100;
                self.HeightRequest = paper.HeightRequest + 100;
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
            ((EventHandler)Events[s_startPageChangedEvent])?.Invoke(this, e);
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
            paper.QueueDraw();
        }
    }
}
