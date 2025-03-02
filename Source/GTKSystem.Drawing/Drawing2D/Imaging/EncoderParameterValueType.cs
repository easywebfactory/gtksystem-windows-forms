namespace System.Drawing.Imaging;

/// <summary>Used to specify the data type of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> used with the <see cref="Overload:System.Drawing.Image.Save" /> or <see cref="Overload:System.Drawing.Image.SaveAdd" /> method of an image.</summary>
public enum EncoderParameterValueType
{
    /// <summary>Specifies that each value in the array is an 8-bit unsigned integer.</summary>
    ValueTypeByte = 1,
    /// <summary>Specifies that the array of values is a null-terminated ASCII character string. Note that the <see langword="NumberOfValues" /> data member of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object indicates the length of the character string including the NULL terminator.</summary>
    ValueTypeAscii,
    /// <summary>Specifies that each value in the array is a 16-bit, unsigned integer.</summary>
    ValueTypeShort,
    /// <summary>Specifies that each value in the array is a 32-bit unsigned integer.</summary>
    ValueTypeLong,
    /// <summary>Specifies that each value in the array is a pair of 32-bit unsigned integers. Each pair represents a fraction, the first integer being the numerator and the second integer being the denominator.</summary>
    ValueTypeRational,
    /// <summary>Specifies that each value in the array is a pair of 32-bit unsigned integers. Each pair represents a range of numbers.</summary>
    ValueTypeLongRange,
    /// <summary>Specifies that the array of values is an array of bytes that has no data type defined.</summary>
    ValueTypeUndefined,
    /// <summary>Specifies that each value in the array is a set of four, 32-bit unsigned integers. The first two integers represent one fraction, and the second two integers represent a second fraction. The two fractions represent a range of rational numbers. The first fraction is the smallest rational number in the range, and the second fraction is the largest rational number in the range.</summary>
    ValueTypeRationalRange,
    ValueTypePointer
}