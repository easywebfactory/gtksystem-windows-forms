// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;

namespace System.Windows.Forms
{
    /// <summary>
    ///  This event is fired by owner drawn <see cref="Control"/> objects, such as <see cref="ListBox"/> and
    ///  <see cref="ComboBox"/>. It contains all the information needed for the user to paint the given item,
    ///  including the item index, the <see cref="Rectangle"/> in which the drawing should be done, and the
    ///  <see cref="Graphics"/> object with which the drawing should be done.
    /// </summary>
    public class DrawItemEventArgs : EventArgs, IDisposable, IDeviceContext
    {

        /// <summary>
        ///  The backColor to paint each menu item with.
        /// </summary>
        private readonly Color _backColor;

        /// <summary>
        ///  The foreColor to paint each menu item with.
        /// </summary>
        private readonly Color _foreColor;

        /// <summary>
        ///  Creates a new DrawItemEventArgs with the given parameters.
        /// </summary>
        public DrawItemEventArgs(Graphics graphics, Font? font, Rectangle rect, int index, DrawItemState state)
            : this(graphics, font, rect, index, state, SystemColors.WindowText, SystemColors.Window)
        { }

        /// <summary>
        ///  Creates a new DrawItemEventArgs with the given parameters, including the foreColor and backColor
        ///  of the control.
        /// </summary>
        public DrawItemEventArgs(
            Graphics graphics,
            Font? font,
            Rectangle rect,
            int index,
            DrawItemState state,
            Color foreColor,
            Color backColor)
        {
            this.graphics=graphics ?? throw new ArgumentNullException(nameof(graphics));
            this.Bounds = rect;
            Font = font;
            Index = index;
            State = state;
            _foreColor = foreColor;
            _backColor = backColor;
        }
        private Graphics graphics;
        /// <summary>
        ///  Gets the <see cref="Drawing.Graphics"/> object used to paint.
        /// </summary>
        public Graphics Graphics { 
            get {
                return graphics;
            } 
        }

        /// <summary>
        ///  A suggested font, usually the parent control's Font property.
        /// </summary>
        public Font? Font { get; }

        /// <summary>
        ///  The rectangle outlining the area in which the painting should be  done.
        /// </summary>
        public Rectangle Bounds { get; }

        /// <summary>
        ///  The index of the item that should be painted.
        /// </summary>
        public int Index { get; }

        /// <summary>
        ///  Miscellaneous state information, such as whether the item is
        ///  "selected", "focused", or some other such information.  ComboBoxes
        ///  have one special piece of information which indicates if the item
        ///  being painted is the editable portion of the ComboBox.
        /// </summary>
        public DrawItemState State { get; }

        /// <summary>
        ///  A suggested color drawing: either SystemColors.WindowText or SystemColors.HighlightText,
        ///  depending on whether this item is selected.
        /// </summary>
        public Color ForeColor
            => (State & DrawItemState.Selected) == DrawItemState.Selected ? SystemColors.HighlightText : _foreColor;

        public Color BackColor
            => (State & DrawItemState.Selected) == DrawItemState.Selected ? SystemColors.Highlight : _backColor;

        public void Dispose()
        {
             
        }

        /// <summary>
        ///  Fills the <see cref="Bounds"/> with the <see cref="BackColor"/>.
        /// </summary>
        public virtual void DrawBackground()
        {
            //using var backBrush = BackColor.GetCachedSolidBrushScope();
            //GraphicsInternal.FillRectangle(backBrush, Bounds);
        }

        /// <summary>
        ///  Draws a handy focus rect in the given rectangle.
        /// </summary>
        public virtual void DrawFocusRectangle()
        {
            if ((State & DrawItemState.Focus) == DrawItemState.Focus
                && (State & DrawItemState.NoFocusRect) != DrawItemState.NoFocusRect)
            {
               // ControlPaint.DrawFocusRectangle(GraphicsInternal, Bounds, ForeColor, BackColor);
            }
        }

        public nint GetHdc()
        {
           return IntPtr.Zero;
        }

        public void ReleaseHdc()
        {
            
        }
    }
}
