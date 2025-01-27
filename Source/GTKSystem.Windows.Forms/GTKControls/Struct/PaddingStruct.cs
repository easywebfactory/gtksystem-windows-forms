using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    //
    // summary:
    //     Represents padding or margin information associated with a user interface (UI)
    //     element.

    public struct Padding
    {
        //
        // summary:
        //     Provides a System.Windows.Forms.Padding object with no padding.
        public static readonly Padding Empty;

        //
        // summary:
        //     Initializes a new instance of the System.Windows.Forms.Padding class using the
        //     supplied padding size for all edges.
        //
        // parameter:
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
        // summary:
        //     Initializes a new instance of the System.Windows.Forms.Padding class using a
        //     separate padding size for each edge.
        //
        // parameter:
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
        // summary:
        //     Gets the combined padding for the right and left edges.
        //
        // Return results:
        //     Gets the sum, in pixels, of the System.Windows.Forms.Padding.Left and System.Windows.Forms.Padding.Right
        //     padding values.
        [Browsable(false)]
        public int Horizontal { get { return Right - Left; } }
        //
        // summary:
        //     Gets or sets the padding value for the top edge.
        //
        // Return results:
        //     The padding, in pixels, for the top edge.
        [RefreshProperties(RefreshProperties.All)]
        public int Top { get; set; }
        //
        // summary:
        //     Gets or sets the padding value for the right edge.
        //
        // Return results:
        //     The padding, in pixels, for the right edge.
        [RefreshProperties(RefreshProperties.All)]
        public int Right { get; set; }
        //
        // summary:
        //     Gets or sets the padding value for the left edge.
        //
        // Return results:
        //     The padding, in pixels, for the left edge.
        [RefreshProperties(RefreshProperties.All)]
        public int Left { get; set; }
        //
        // summary:
        //     Gets or sets the padding value for the bottom edge.
        //
        // Return results:
        //     The padding, in pixels, for the bottom edge.
        [RefreshProperties(RefreshProperties.All)]
        public int Bottom { get; set; }
        //
        // summary:
        //     Gets or sets the padding value for all the edges.
        //
        // Return results:
        //     The padding, in pixels, for all edges if the same; otherwise, -1.
        [RefreshProperties(RefreshProperties.All)]
        public int All { get; set; }
        //
        // summary:
        //     Gets the padding information in the form of a System.Drawing.Size.
        //
        // Return results:
        //     A System.Drawing.Size containing the padding information.
        [Browsable(false)]
        public Size Size { get; }
        //
        // summary:
        //     Gets the combined padding for the top and bottom edges.
        //
        // Return results:
        //     Gets the sum, in pixels, of the System.Windows.Forms.Padding.Top and System.Windows.Forms.Padding.Bottom
        //     padding values.
        [Browsable(false)]
        public int Vertical { get { return Bottom - Top; } }

        //
        // summary:
        //     Computes the sum of the two specified System.Windows.Forms.Padding values.
        //
        // parameter:
        //   p1:
        //     A System.Windows.Forms.Padding.
        //
        //   p2:
        //     A System.Windows.Forms.Padding.
        //
        // Return results:
        //     A System.Windows.Forms.Padding that contains the sum of the two specified System.Windows.Forms.Padding
        //     values.
        public static Padding Add(Padding p1, Padding p2) {
            return new Padding(p1.Left + p2.Left, p1.Top + p2.Top, p1.Right + p2.Right, p1.Bottom + p2.Bottom);
        }
        //
        // summary:
        //     Subtracts one specified System.Windows.Forms.Padding value from another.
        //
        // parameter:
        //   p1:
        //     A System.Windows.Forms.Padding.
        //
        //   p2:
        //     A System.Windows.Forms.Padding.
        //
        // Return results:
        //     A System.Windows.Forms.Padding that contains the result of the subtraction of
        //     one specified System.Windows.Forms.Padding value from another.
        public static Padding Subtract(Padding p1, Padding p2) {
            return new Padding(p1.Left - p2.Left, p1.Top - p2.Top, p1.Right - p2.Right, p1.Bottom - p2.Bottom);
        }
        //
        // summary:
        //     Determines whether the value of the specified object is equivalent to the current
        //     System.Windows.Forms.Padding.
        //
        // parameter:
        //   other:
        //     The object to compare to the current System.Windows.Forms.Padding.
        //
        // Return results:
        //     true if the System.Windows.Forms.Padding objects are equivalent; otherwise, false.
        public override bool Equals(object other) {
            return false;
        }
        //
        // summary:
        //     Generates a hash code for the current System.Windows.Forms.Padding.
        //
        // Return results:
        //     A 32-bit signed integer hash code.
        public override int GetHashCode() {
            return Left.GetHashCode() + Top.GetHashCode() + Right.GetHashCode() + Bottom.GetHashCode();
        }
        //
        // summary:
        //     Returns a string that represents the current System.Windows.Forms.Padding.
        //
        // Return results:
        //     A System.String that represents the current System.Windows.Forms.Padding.
        public override string ToString() {
            return this.GetType().Name;
        }

        //
        // summary:
        //     Performs vector addition on the two specified System.Windows.Forms.Padding objects,
        //     resulting in a new System.Windows.Forms.Padding.
        //
        // parameter:
        //   p1:
        //     The first System.Windows.Forms.Padding to add.
        //
        //   p2:
        //     The second System.Windows.Forms.Padding to add.
        //
        // Return results:
        //     A new System.Windows.Forms.Padding that results from adding p1 and p2.
        public static Padding operator +(Padding p1, Padding p2) {
            return new Padding(p1.Left + p2.Left, p1.Top + p2.Top, p1.Right + p2.Right, p1.Bottom + p2.Bottom);
        }
        //
        // summary:
        //     Performs vector subtraction on the two specified System.Windows.Forms.Padding
        //     objects, resulting in a new System.Windows.Forms.Padding.
        //
        // parameter:
        //   p1:
        //     The System.Windows.Forms.Padding to subtract from (the minuend).
        //
        //   p2:
        //     The System.Windows.Forms.Padding to subtract from (the subtrahend).
        //
        // Return results:
        //     The System.Windows.Forms.Padding result of subtracting p2 from p1.
        public static Padding operator -(Padding p1, Padding p2) {
            return new Padding(p1.Left - p2.Left, p1.Top - p2.Top, p1.Right - p2.Right, p1.Bottom - p2.Bottom);
        }
        //
        // summary:
        //     Tests whether two specified System.Windows.Forms.Padding objects are equivalent.
        //
        // parameter:
        //   p1:
        //     A System.Windows.Forms.Padding to test.
        //
        //   p2:
        //     A System.Windows.Forms.Padding to test.
        //
        // Return results:
        //     true if the two System.Windows.Forms.Padding objects are equal; otherwise, false.
        public static bool operator ==(Padding p1, Padding p2) { return p1.Left == p2.Left && p1.Top == p2.Top && p1.Right == p2.Right && p1.Bottom == p2.Bottom; }
        //
        // summary:
        //     Tests whether two specified System.Windows.Forms.Padding objects are not equivalent.
        //
        // parameter:
        //   p1:
        //     A System.Windows.Forms.Padding to test.
        //
        //   p2:
        //     A System.Windows.Forms.Padding to test.
        //
        // Return results:
        //     true if the two System.Windows.Forms.Padding objects are different; otherwise,
        //     false.
        public static bool operator !=(Padding p1, Padding p2) {
            return p1.Left != p2.Left || p1.Top != p2.Top || p1.Right != p2.Right || p1.Bottom != p2.Bottom;
        }
    }
}
