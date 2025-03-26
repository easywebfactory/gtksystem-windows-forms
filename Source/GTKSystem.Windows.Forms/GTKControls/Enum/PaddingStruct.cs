using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms;

/// <summary>
///     Represents padding or margin information associated with a user interface (UI)
///     element.
/// </summary>
public struct Padding
{
    /// <summary>
    ///     Provides a System.Windows.Forms.Padding object with no padding.
    /// </summary>
    public static Padding Empty => default;

    /// <summary>
    ///     Initializes a new instance of the System.Windows.Forms.Padding class using the
    ///     supplied padding size for all edges.
    /// </summary>
    /// <param name="all">
    ///     The number of pixels to be used for padding for all edges.
    /// </param>
    public Padding(int all)
    {
        Left = all;
        Top = all;
        Right = all;
        Bottom = all;
        All = all;
        Size = new Size(0, 0);
    }

    /// <summary>
    ///     Initializes a new instance of the System.Windows.Forms.Padding class using a
    ///     separate padding size for each edge.
    /// </summary>
    /// <param name="left">
    ///     The padding size, in pixels, for the left edge.
    ///</param>
    /// <param name="top">
    ///     The padding size, in pixels, for the top edge.
    ///</param>
    /// <param name="right">
    ///     The padding size, in pixels, for the right edge.
    ///</param>
    /// <param name="bottom">
    ///     The padding size, in pixels, for the bottom edge.
    /// </param>
    public Padding(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;

        All = left;
        Size = new Size(right - left, bottom - top);
    }

    /// <summary>
    ///     Gets the combined padding for the right and left edges.
    /// </summary>
    /// <returns>
    ///     Gets the sum, in pixels, of the System.Windows.Forms.Padding.Left and System.Windows.Forms.Padding.Right
    ///     padding values.
    /// </returns>
    [Browsable(false)]
    public int Horizontal
    {
        get { return Right - Left; }
    }

    /// <summary>
    ///     Gets or sets the padding value for the top edge.
    /// </summary>
    /// <returns>
    ///     The padding, in pixels, for the top edge.
    /// </returns>
    [RefreshProperties(RefreshProperties.All)]
    public int Top { get; set; }

    /// <summary>
    ///     Gets or sets the padding value for the right edge.
    /// </summary>
    /// <returns>
    ///     The padding, in pixels, for the right edge.
    /// </returns>
    [RefreshProperties(RefreshProperties.All)]
    public int Right { get; set; }

    /// <summary>
    ///     Gets or sets the padding value for the left edge.
    /// </summary>
    /// <returns>
    ///     The padding, in pixels, for the left edge.
    /// </returns>
    [RefreshProperties(RefreshProperties.All)]
    public int Left { get; set; }

    /// <summary>
    ///     Gets or sets the padding value for the bottom edge.
    /// </summary>
    /// <returns>
    ///     The padding, in pixels, for the bottom edge.
    /// </returns>
    [RefreshProperties(RefreshProperties.All)]
    public int Bottom { get; set; }

    /// <summary>
    ///     Gets or sets the padding value for all the edges.
    /// </summary>
    /// <returns>
    ///     The padding, in pixels, for all edges if the same; otherwise, -1.
    /// </returns>
    [RefreshProperties(RefreshProperties.All)]
    public int All { get; set; }

    /// <summary>
    ///     Gets the padding information in the form of a System.Drawing.Size.
    /// </summary>
    /// <returns>
    ///     A System.Drawing.Size containing the padding information.
    /// </returns>
    [Browsable(false)]
    public Size Size { get; }

    /// <summary>
    ///     Gets the combined padding for the top and bottom edges.
    /// </summary>
    /// <returns>
    ///     Gets the sum, in pixels, of the System.Windows.Forms.Padding.Top and System.Windows.Forms.Padding.Bottom
    ///     padding values.
    /// </returns>
    [Browsable(false)]
    public int Vertical
    {
        get { return Bottom - Top; }
    }

    /// <summary>
    ///     Computes the sum of the two specified System.Windows.Forms.Padding values.
    /// </summary>
    /// <param name="p1">
    ///     A System.Windows.Forms.Padding.
    /// </param>
    /// <param name="p2">
    ///     A System.Windows.Forms.Padding.
    /// </param>
    /// <returns>
    ///     A System.Windows.Forms.Padding that contains the sum of the two specified System.Windows.Forms.Padding
    ///     values.
    /// </returns>
    public static Padding Add(Padding p1, Padding p2)
    {
        return new Padding(p1.Left + p2.Left, p1.Top + p2.Top, p1.Right + p2.Right, p1.Bottom + p2.Bottom);
    }

    /// <summary>
    ///     Subtracts one specified System.Windows.Forms.Padding value from another.
    /// </summary>
    /// <param name="p1">
    ///     A System.Windows.Forms.Padding.
    /// </param>
    /// <param name="p2">
    ///     A System.Windows.Forms.Padding.
    /// </param>
    /// <returns>
    ///     A System.Windows.Forms.Padding that contains the result of the subtraction of
    ///     one specified System.Windows.Forms.Padding value from another.
    /// </returns>
    public static Padding Subtract(Padding p1, Padding p2)
    {
        return new Padding(p1.Left - p2.Left, p1.Top - p2.Top, p1.Right - p2.Right, p1.Bottom - p2.Bottom);
    }

    /// <summary>
    ///     Determines whether the value of the specified object is equivalent to the current
    ///     System.Windows.Forms.Padding.
    /// </summary>
    /// <param name="other">
    ///     The object to compare to the current System.Windows.Forms.Padding.
    /// </param>
    /// <returns>
    ///     true if the System.Windows.Forms.Padding objects are equivalent; otherwise, false.
    /// </returns>
    public override bool Equals(object? other)
    {
        return false;
    }

    /// <summary>
    ///     Generates a hash code for the current System.Windows.Forms.Padding.
    /// </summary>
    /// <returns>
    ///     A 32-bit signed integer hash code.
    /// </returns>
    public override int GetHashCode()
    {
        return Left.GetHashCode() + Top.GetHashCode() + Right.GetHashCode() + Bottom.GetHashCode();
    }

    /// <summary>
    ///     Returns a string that represents the current System.Windows.Forms.Padding.
    /// </summary>
    /// <returns>
    ///     A System.String that represents the current System.Windows.Forms.Padding.
    /// </returns>
    public override string ToString()
    {
        return GetType().Name;
    }

    /// <summary>
    ///     Performs vector addition on the two specified System.Windows.Forms.Padding objects,
    ///     resulting in a new System.Windows.Forms.Padding.
    /// </summary>
    /// <param name="p1">
    ///     The first System.Windows.Forms.Padding to add.
    /// </param>
    /// <param name="p2">
    ///     The second System.Windows.Forms.Padding to add.
    /// </param>
    /// <returns>
    ///     A new System.Windows.Forms.Padding that results from adding p1 and p2.
    /// </returns>
    public static Padding operator +(Padding p1, Padding p2)
    {
        return new Padding(p1.Left + p2.Left, p1.Top + p2.Top, p1.Right + p2.Right, p1.Bottom + p2.Bottom);
    }

    /// <summary>
    ///     Performs vector subtraction on the two specified System.Windows.Forms.Padding
    ///     objects, resulting in a new System.Windows.Forms.Padding.
    /// </summary>
    /// <param name="p1">
    ///     The System.Windows.Forms.Padding to subtract from (the minuend).
    /// </param>
    /// <param name="p2">
    ///     The System.Windows.Forms.Padding to subtract from (the subtrahend).
    /// </param>
    /// <returns>
    ///     The System.Windows.Forms.Padding result of subtracting p2 from p1.
    /// </returns>
    public static Padding operator -(Padding p1, Padding p2)
    {
        return new Padding(p1.Left - p2.Left, p1.Top - p2.Top, p1.Right - p2.Right, p1.Bottom - p2.Bottom);
    }

    /// <summary>
    ///     Tests whether two specified System.Windows.Forms.Padding objects are equivalent.
    /// </summary>
    /// <param name="p1">
    ///     A System.Windows.Forms.Padding to test.
    /// </param>
    /// <param name="p2">
    ///     A System.Windows.Forms.Padding to test.
    /// </param>
    /// <returns>
    ///     true if the two System.Windows.Forms.Padding objects are equal; otherwise, false.
    /// </returns>
    public static bool operator ==(Padding p1, Padding p2)
    {
        return p1.Left == p2.Left && p1.Top == p2.Top && p1.Right == p2.Right && p1.Bottom == p2.Bottom;
    }

    /// <summary>
    ///     Tests whether two specified System.Windows.Forms.Padding objects are not equivalent.
    /// </summary>
    /// <param name="p1">
    ///     A System.Windows.Forms.Padding to test.
    /// </param>
    /// <param name="p2">
    ///     A System.Windows.Forms.Padding to test.
    /// </param>
    /// <returns>
    ///     true if the two System.Windows.Forms.Padding objects are different; otherwise,
    ///     false.
    /// </returns>
    public static bool operator !=(Padding p1, Padding p2)
    {
        return p1.Left != p2.Left || p1.Top != p2.Top || p1.Right != p2.Right || p1.Bottom != p2.Bottom;
    }
}