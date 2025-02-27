// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms;

internal abstract class ArrangedElement : Component, IArrangedElement
{
    private Rectangle _bounds = Rectangle.Empty;
    private IArrangedElement? _parent;
    private BitVector32 _state;
    private readonly PropertyStore _propertyStore = new();  // Contains all properties that are not always set.

    private static readonly int stateVisible = BitVector32.CreateMask();

    internal ArrangedElement()
    {
        Init();
    }

    private void Init()
    {
        Padding = DefaultPadding;
        Margin = DefaultMargin;
        _state[stateVisible] = true;
    }

    public Rectangle Bounds => _bounds;

    ArrangedElementCollection IArrangedElement.Children => GetChildren();

    IArrangedElement? IArrangedElement.Container => GetContainer();

    protected virtual Padding DefaultMargin => Padding.Empty;

    protected virtual Padding DefaultPadding => Padding.Empty;

    public virtual Rectangle DisplayRectangle
    {
        get
        {
            var displayRectangle = Bounds;
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
        get => _parent;
        set => _parent = value;
    }

    public virtual bool ParticipatesInLayout => Visible;

    PropertyStore IArrangedElement.Properties => Properties;

    private PropertyStore Properties => _propertyStore;

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
        OnLayout(new LayoutEventArgs(container, propertyName??string.Empty));
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
            var oldBounds = _bounds;

            _bounds = bounds;
            OnBoundsChanged(oldBounds, bounds);
        }
    }
}