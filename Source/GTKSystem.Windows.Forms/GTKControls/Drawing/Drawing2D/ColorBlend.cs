#pragma warning disable CS8597 // Thrown value may be null.
namespace System.Drawing.Drawing2D;

/// <summary>Defines arrays of colors and positions used for interpolating color blending in a multicolor gradient. This class cannot be inherited.</summary>
public sealed class ColorBlend
{
    public int Count { get; }

    /// <summary>Gets or sets an array of colors that represents the colors to use at corresponding positions along a gradient.</summary>
    /// <returns>An array of <see cref="T:System.Drawing.Color" /> structures that represents the colors to use at corresponding positions along a gradient.</returns>
    public Color[] Colors => throw new NotImplementedException();

    /// <summary>Gets or sets the positions along a gradient line.</summary>
    /// <returns>An array of values that specify percentages of distance along the gradient line.</returns>
    public float[] Positions => throw new NotImplementedException();

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.ColorBlend" /> class.</summary>
    public ColorBlend()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.ColorBlend" /> class with the specified number of colors and positions.</summary>
    /// <param name="count">The number of colors and positions in this <see cref="T:System.Drawing.Drawing2D.ColorBlend" />.</param>
    public ColorBlend(int count)
    {
        Count = count;
    }
}