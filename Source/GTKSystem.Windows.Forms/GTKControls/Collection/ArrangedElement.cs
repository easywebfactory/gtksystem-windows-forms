// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
    internal abstract class ArrangedElement : Component, IArrangedElement
    {
        private Rectangle _bounds = Rectangle.Empty;
        private IArrangedElement? _parent;
        private BitVector32 _state;
        private readonly PropertyStore _propertyStore = new();  // Contains all properties that are not always set.

        private static readonly int s_stateVisible = BitVector32.CreateMask();

        internal ArrangedElement()
        {
            Padding = DefaultPadding;
            Margin = DefaultMargin;
            _state[s_stateVisible] = true;
        }

        public Rectangle Bounds
        {
            get
            {
                return _bounds;
            }
        }

        ArrangedElementCollection IArrangedElement.Children
        {
            get { return GetChildren(); }
        }

        IArrangedElement? IArrangedElement.Container
        {
            get { return GetContainer(); }
        }

        protected virtual Padding DefaultMargin
        {
            get { return Padding.Empty; }
        }

        protected virtual Padding DefaultPadding
        {
            get { return Padding.Empty; }
        }

        public virtual Rectangle DisplayRectangle
        {
            get
            {
                Rectangle displayRectangle = Bounds;
                return displayRectangle;
            }
        }

        public abstract LayoutEngine LayoutEngine
        {
            get;
        }

        public Padding Margin
        {
            get;
            set;
        }

        public virtual Padding Padding
        {
            get;
            set;
        }

        public virtual IArrangedElement? Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        public virtual bool ParticipatesInLayout
        {
            get
            {
                return Visible;
            }
        }

        PropertyStore IArrangedElement.Properties
        {
            get
            {
                return Properties;
            }
        }

        private PropertyStore Properties
        {
            get
            {
                return _propertyStore;
            }
        }

        public virtual bool Visible
        {
            get;
            set;
        }

        public IntPtr Handle => throw new NotImplementedException();

        protected abstract IArrangedElement? GetContainer();

        protected abstract ArrangedElementCollection GetChildren();

        public virtual Size GetPreferredSize(Size constrainingSize)
        {
            return constrainingSize;
        }

        public virtual void PerformLayout(IArrangedElement container, string? propertyName)
        {
            OnLayout(new LayoutEventArgs(container, propertyName));
        }

        protected virtual void OnLayout(LayoutEventArgs e)
        {

        }

        protected virtual void OnBoundsChanged(Rectangle oldBounds, Rectangle newBounds)
        {

        }

        public void SetBounds(Rectangle bounds, BoundsSpecified specified)
        {
            // in this case the parent is telling us to refresh our bounds - don't
            // call PerformLayout
            SetBoundsCore(bounds, specified);
        }

        protected virtual void SetBoundsCore(Rectangle bounds, BoundsSpecified specified)
        {
            if (bounds != _bounds)
            {
                Rectangle oldBounds = _bounds;

                _bounds = bounds;
                OnBoundsChanged(oldBounds, bounds);
            }
        }
    }
}
