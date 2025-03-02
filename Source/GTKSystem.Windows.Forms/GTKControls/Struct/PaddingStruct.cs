using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms;
//
// 摘要:
//     Represents padding or margin information associated with a user interface (UI)
//     element.

public struct Padding
{
    //
    // 摘要:
    //     Provides a System.Windows.Forms.Padding object with no padding.
    public static Padding Empty => default;

    //
    // 摘要:
    //     Initializes a new instance of the System.Windows.Forms.Padding class using the
    //     supplied padding size for all edges.
    //
    // 参数:
    //   all:
    //     The number of pixels to be used for padding for all edges.
    public Padding(int all) {
        Left = all;
        Top = all;
        Right = all;
        Bottom = all;
        All = all;
        Size = new Size(0, 0);
    }
    //
    // 摘要:
    //     Initializes a new instance of the System.Windows.Forms.Padding class using a
    //     separate padding size for each edge.
    //
    // 参数:
    //   left:
    //     The padding size, in pixels, for the left edge.
    //
    //   top:
    //     The padding size, in pixels, for the top edge.
    //
    //   right:
    //     The padding size, in pixels, for the right edge.
    //
    //   bottom:
    //     The padding size, in pixels, for the bottom edge.
    public Padding(int left, int top, int right, int bottom) {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;

        All = left;
        Size = new Size(right - left, bottom - top);
    }

    //
    // 摘要:
    //     Gets the combined padding for the right and left edges.
    //
    // 返回结果:
    //     Gets the sum, in pixels, of the System.Windows.Forms.Padding.Left and System.Windows.Forms.Padding.Right
    //     padding values.
    [Browsable(false)]
    public int Horizontal => Right - Left;

    //
    // 摘要:
    //     Gets or sets the padding value for the top edge.
    //
    // 返回结果:
    //     The padding, in pixels, for the top edge.
    [RefreshProperties(RefreshProperties.All)]
    public int Top { get; set; }
    //
    // 摘要:
    //     Gets or sets the padding value for the right edge.
    //
    // 返回结果:
    //     The padding, in pixels, for the right edge.
    [RefreshProperties(RefreshProperties.All)]
    public int Right { get; set; }
    //
    // 摘要:
    //     Gets or sets the padding value for the left edge.
    //
    // 返回结果:
    //     The padding, in pixels, for the left edge.
    [RefreshProperties(RefreshProperties.All)]
    public int Left { get; set; }
    //
    // 摘要:
    //     Gets or sets the padding value for the bottom edge.
    //
    // 返回结果:
    //     The padding, in pixels, for the bottom edge.
    [RefreshProperties(RefreshProperties.All)]
    public int Bottom { get; set; }
    //
    // 摘要:
    //     Gets or sets the padding value for all the edges.
    //
    // 返回结果:
    //     The padding, in pixels, for all edges if the same; otherwise, -1.
    [RefreshProperties(RefreshProperties.All)]
    public int All { get; set; }
    //
    // 摘要:
    //     Gets the padding information in the form of a System.Drawing.Size.
    //
    // 返回结果:
    //     A System.Drawing.Size containing the padding information.
    [Browsable(false)]
    public Size Size { get; }
    //
    // 摘要:
    //     Gets the combined padding for the top and bottom edges.
    //
    // 返回结果:
    //     Gets the sum, in pixels, of the System.Windows.Forms.Padding.Top and System.Windows.Forms.Padding.Bottom
    //     padding values.
    [Browsable(false)]
    public int Vertical => Bottom - Top;

    //
    // 摘要:
    //     Computes the sum of the two specified System.Windows.Forms.Padding values.
    //
    // 参数:
    //   p1:
    //     A System.Windows.Forms.Padding.
    //
    //   p2:
    //     A System.Windows.Forms.Padding.
    //
    // 返回结果:
    //     A System.Windows.Forms.Padding that contains the sum of the two specified System.Windows.Forms.Padding
    //     values.
    public static Padding Add(Padding p1, Padding p2) {
        return new Padding(p1.Left + p2.Left, p1.Top + p2.Top, p1.Right + p2.Right, p1.Bottom + p2.Bottom);
    }
    //
    // 摘要:
    //     Subtracts one specified System.Windows.Forms.Padding value from another.
    //
    // 参数:
    //   p1:
    //     A System.Windows.Forms.Padding.
    //
    //   p2:
    //     A System.Windows.Forms.Padding.
    //
    // 返回结果:
    //     A System.Windows.Forms.Padding that contains the result of the subtraction of
    //     one specified System.Windows.Forms.Padding value from another.
    public static Padding Subtract(Padding p1, Padding p2) {
        return new Padding(p1.Left - p2.Left, p1.Top - p2.Top, p1.Right - p2.Right, p1.Bottom - p2.Bottom);
    }
    //
    // 摘要:
    //     Determines whether the value of the specified object is equivalent to the current
    //     System.Windows.Forms.Padding.
    //
    // 参数:
    //   other:
    //     The object to compare to the current System.Windows.Forms.Padding.
    //
    // 返回结果:
    //     true if the System.Windows.Forms.Padding objects are equivalent; otherwise, false.
    public override bool Equals(object? other) {
        return false;
    }
    //
    // 摘要:
    //     Generates a hash code for the current System.Windows.Forms.Padding.
    //
    // 返回结果:
    //     A 32-bit signed integer hash code.
    public override int GetHashCode() {
        return Left.GetHashCode() + Top.GetHashCode() + Right.GetHashCode() + Bottom.GetHashCode();
    }
    //
    // 摘要:
    //     Returns a string that represents the current System.Windows.Forms.Padding.
    //
    // 返回结果:
    //     A System.String that represents the current System.Windows.Forms.Padding.
    public override string ToString() {
        return GetType().Name;
    }

    //
    // 摘要:
    //     Performs vector addition on the two specified System.Windows.Forms.Padding objects,
    //     resulting in a new System.Windows.Forms.Padding.
    //
    // 参数:
    //   p1:
    //     The first System.Windows.Forms.Padding to add.
    //
    //   p2:
    //     The second System.Windows.Forms.Padding to add.
    //
    // 返回结果:
    //     A new System.Windows.Forms.Padding that results from adding p1 and p2.
    public static Padding operator +(Padding p1, Padding p2) {
        return new Padding(p1.Left + p2.Left, p1.Top + p2.Top, p1.Right + p2.Right, p1.Bottom + p2.Bottom);
    }
    //
    // 摘要:
    //     Performs vector subtraction on the two specified System.Windows.Forms.Padding
    //     objects, resulting in a new System.Windows.Forms.Padding.
    //
    // 参数:
    //   p1:
    //     The System.Windows.Forms.Padding to subtract from (the minuend).
    //
    //   p2:
    //     The System.Windows.Forms.Padding to subtract from (the subtrahend).
    //
    // 返回结果:
    //     The System.Windows.Forms.Padding result of subtracting p2 from p1.
    public static Padding operator -(Padding p1, Padding p2) {
        return new Padding(p1.Left - p2.Left, p1.Top - p2.Top, p1.Right - p2.Right, p1.Bottom - p2.Bottom);
    }
    //
    // 摘要:
    //     Tests whether two specified System.Windows.Forms.Padding objects are equivalent.
    //
    // 参数:
    //   p1:
    //     A System.Windows.Forms.Padding to test.
    //
    //   p2:
    //     A System.Windows.Forms.Padding to test.
    //
    // 返回结果:
    //     true if the two System.Windows.Forms.Padding objects are equal; otherwise, false.
    public static bool operator ==(Padding p1, Padding p2) { return p1.Left == p2.Left && p1.Top == p2.Top && p1.Right == p2.Right && p1.Bottom == p2.Bottom; }
    //
    // 摘要:
    //     Tests whether two specified System.Windows.Forms.Padding objects are not equivalent.
    //
    // 参数:
    //   p1:
    //     A System.Windows.Forms.Padding to test.
    //
    //   p2:
    //     A System.Windows.Forms.Padding to test.
    //
    // 返回结果:
    //     true if the two System.Windows.Forms.Padding objects are different; otherwise,
    //     false.
    public static bool operator !=(Padding p1, Padding p2) {
        return p1.Left != p2.Left || p1.Top != p2.Top || p1.Right != p2.Right || p1.Bottom != p2.Bottom;
    }
}