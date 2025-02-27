using System.Runtime.InteropServices;

namespace System.Drawing.Imaging;

/// <summary>Used to pass a value, or an array of values, to an image encoder.</summary>
[StructLayout(LayoutKind.Sequential)]
public sealed class EncoderParameter : IDisposable
{
    private readonly int numberOfValues;
    public bool Undefined { get; }
    public Encoder? ConstructorEncoder { get; }
    public int[]? Numerators1 { get; }
    public int[]? Denominators1 { get; }
    public int[]? Numerators2 { get; }
    public int[]? Denominators2 { get; }
    public long[]? Rangebegins { get; }
    public long[]? Rangeends { get; }
    public int[]? Numerators { get; }
    public int[]? Denominators { get; }
    public int Numerator1 { get; }
    public int Demoninator1 { get; }
    public int Numerator2 { get; }
    public int Demoninator2 { get; }
    public long Rangebegin { get; }
    public long Rangeend { get; }
    public int Numerator { get; }
    public int Denominator { get; }
    public long Value { get; }
    private Guid _parameterGuid;

    private int _numberOfValues;

    private EncoderParameterValueType _parameterValueType;

    private IntPtr _parameterValue;

    /// <summary>Gets or sets the <see cref="T:System.Drawing.Imaging.Encoder" /> object associated with this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. The <see cref="T:System.Drawing.Imaging.Encoder" /> object encapsulates the globally unique identifier (GUID) that specifies the category (for example <see cref="F:System.Drawing.Imaging.Encoder.Quality" />, <see cref="F:System.Drawing.Imaging.Encoder.ColorDepth" />, or <see cref="F:System.Drawing.Imaging.Encoder.Compression" />) of the parameter stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</summary>
    /// <returns>An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the GUID that specifies the category of the parameter stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</returns>
    public Encoder Encoder
    {
        get => new(_parameterGuid);
        set => _parameterGuid = value.Guid;
    }

    /// <summary>Gets the data type of the values stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</summary>
    /// <returns>A member of the <see cref="T:System.Drawing.Imaging.EncoderParameterValueType" /> enumeration that indicates the data type of the values stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</returns>
    public EncoderParameterValueType Type
    {
        get => _parameterValueType;
        set => _parameterValueType=value;
    }

    /// <summary>Gets the data type of the values stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</summary>
    /// <returns>A member of the <see cref="T:System.Drawing.Imaging.EncoderParameterValueType" /> enumeration that indicates the data type of the values stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</returns>
    public EncoderParameterValueType ValueType => _parameterValueType;

    /// <summary>Gets the number of elements in the array of values stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</summary>
    /// <returns>An integer that indicates the number of elements in the array of values stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</returns>
    public int NumberOfValues
    {
        get => _numberOfValues;
        set => _numberOfValues = value;
    }

    /// <summary>Allows an <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object to attempt to free resources and perform other cleanup operations before the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object is reclaimed by garbage collection.</summary>
    ~EncoderParameter()
    {
        Dispose(disposing: false);
    }

    /// <summary>Releases all resources used by this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.KeepAlive(this);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }
        if (_parameterValue != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(_parameterValue);
        }
        _parameterValue = IntPtr.Zero;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and one unsigned 8-bit integer. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeByte" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to 1.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="value">An 8-bit unsigned integer that specifies the value stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</param>
    public EncoderParameter(Encoder? encoder, byte value)
    {
        ConstructorEncoder = encoder;
        Value = value;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and one 8-bit value. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeUndefined" /> or <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeByte" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to 1.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="value">A byte that specifies the value stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</param>
    /// <param name="undefined">If <see langword="true" />, the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property is set to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeUndefined" />; otherwise, the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property is set to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeByte" />.</param>
    public  EncoderParameter(Encoder? encoder, byte value, bool undefined)
    {
        Undefined = undefined;
        ConstructorEncoder = encoder;
        Value = value;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and one, 16-bit integer. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeShort" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to 1.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="value">A 16-bit integer that specifies the value stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. Must be nonnegative.</param>
    public  EncoderParameter(Encoder? encoder, short value)
    {
        ConstructorEncoder = encoder;
        Value = value;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and one 64-bit integer. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeLong" /> (32 bits), and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to 1.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="value">A 64-bit integer that specifies the value stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. Must be nonnegative. This parameter is converted to a 32-bit integer before it is stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</param>
    public  EncoderParameter(Encoder? encoder, long value)
    {
        ConstructorEncoder = encoder;
        Value = value;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and a pair of 32-bit integers. The pair of integers represents a fraction, the first integer being the numerator, and the second integer being the denominator. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeRational" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to 1.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="numerator">A 32-bit integer that represents the numerator of a fraction. Must be nonnegative.</param>
    /// <param name="denominator">A 32-bit integer that represents the denominator of a fraction. Must be nonnegative.</param>
    public  EncoderParameter(Encoder? encoder, int numerator, int denominator)
    {
        ConstructorEncoder = encoder;
        Numerator = numerator;
        Denominator = denominator;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and a pair of 64-bit integers. The pair of integers represents a range of integers, the first integer being the smallest number in the range, and the second integer being the largest number in the range. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeLongRange" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to 1.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="rangebegin">A 64-bit integer that represents the smallest number in a range of integers. Must be nonnegative. This parameter is converted to a 32-bit integer before it is stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</param>
    /// <param name="rangeend">A 64-bit integer that represents the largest number in a range of integers. Must be nonnegative. This parameter is converted to a 32-bit integer before it is stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</param>
    public  EncoderParameter(Encoder? encoder, long rangebegin, long rangeend)
    {
        ConstructorEncoder = encoder;
        Rangebegin = rangebegin;
        Rangeend = rangeend;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and four, 32-bit integers. The four integers represent a range of fractions. The first two integers represent the smallest fraction in the range, and the remaining two integers represent the largest fraction in the range. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeRationalRange" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to 1.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="numerator1">A 32-bit integer that represents the numerator of the smallest fraction in the range. Must be nonnegative.</param>
    /// <param name="demoninator1">A 32-bit integer that represents the denominator of the smallest fraction in the range. Must be nonnegative.</param>
    /// <param name="numerator2">A 32-bit integer that represents the denominator of the smallest fraction in the range. Must be nonnegative.</param>
    /// <param name="demoninator2">A 32-bit integer that represents the numerator of the largest fraction in the range. Must be nonnegative.</param>
    public  EncoderParameter(Encoder? encoder, int numerator1, int demoninator1, int numerator2, int demoninator2)
    {
        ConstructorEncoder = encoder;
        Numerator1 = numerator1;
        Demoninator1 = demoninator1;
        Numerator2 = numerator2;
        Demoninator2 = demoninator2;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and a character string. The string is converted to a null-terminated ASCII string before it is stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeAscii" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the length of the ASCII string including the NULL terminator.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="value">A <see cref="T:System.String" /> that specifies the value stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</param>
    public EncoderParameter(Encoder encoder, string value)
    {
        _parameterGuid = encoder.Guid;
        _parameterValueType = EncoderParameterValueType.ValueTypeAscii;
        _numberOfValues = value.Length;
        _parameterValue = Marshal.StringToHGlobalAnsi(value);
        GC.KeepAlive(this);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and an array of unsigned 8-bit integers. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeByte" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the number of elements in the array.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="value">An array of 8-bit unsigned integers that specifies the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</param>
    public EncoderParameter(Encoder encoder, byte[] value)
    {
        _parameterGuid = encoder.Guid;
        _parameterValueType = EncoderParameterValueType.ValueTypeByte;
        _numberOfValues = value.Length;
        _parameterValue = Marshal.AllocHGlobal(_numberOfValues);
        Marshal.Copy(value, 0, _parameterValue, _numberOfValues);
        GC.KeepAlive(this);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and an array of bytes. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeUndefined" /> or <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeByte" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the number of elements in the array.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="value">An array of bytes that specifies the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</param>
    /// <param name="undefined">If <see langword="true" />, the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property is set to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeUndefined" />; otherwise, the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property is set to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeByte" />.</param>
    public EncoderParameter(Encoder encoder, byte[] value, bool undefined)
    {
        _parameterGuid = encoder.Guid;
        _parameterValueType = !undefined ? EncoderParameterValueType.ValueTypeByte : EncoderParameterValueType.ValueTypeUndefined;
        _numberOfValues = value.Length;
        _parameterValue = Marshal.AllocHGlobal(_numberOfValues);
        Marshal.Copy(value, 0, _parameterValue, _numberOfValues);
        GC.KeepAlive(this);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and an array of 16-bit integers. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeShort" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the number of elements in the array.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="value">An array of 16-bit integers that specifies the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. The integers in the array must be nonnegative.</param>
    public EncoderParameter(Encoder encoder, short[] value)
    {
        _parameterGuid = encoder.Guid;
        _parameterValueType = EncoderParameterValueType.ValueTypeShort;
        _numberOfValues = value.Length;
        _parameterValue = Marshal.AllocHGlobal(checked(_numberOfValues * 2));
        Marshal.Copy(value, 0, _parameterValue, _numberOfValues);
        GC.KeepAlive(this);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and an array of 64-bit integers. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeLong" /> (32-bit), and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the number of elements in the array.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="value">An array of 64-bit integers that specifies the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. The integers in the array must be nonnegative. The 64-bit integers are converted to 32-bit integers before they are stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</param>
    public EncoderParameter(Encoder encoder, long[]? value)
    {
        ConstructorEncoder = encoder;
        Values = value;
    }

    public long[]? Values { get; set; }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and two arrays of 32-bit integers. The two arrays represent an array of fractions. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeRational" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the number of elements in the <paramref name="numerator" /> array, which must be the same as the number of elements in the <paramref name="denominator" /> array.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="numerators">An array of 32-bit integers that specifies the numerators of the fractions. The integers in the array must be nonnegative.</param>
    /// <param name="denominators">An array of 32-bit integers that specifies the denominators of the fractions. The integers in the array must be nonnegative. A denominator of a given index is paired with the numerator of the same index.</param>
    public  EncoderParameter(Encoder encoder, int[]? numerators, int[]? denominators)
    {
        ConstructorEncoder = encoder;
        Numerators = numerators;
        Denominators = denominators;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and two arrays of 64-bit integers. The two arrays represent an array integer ranges. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeLongRange" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the number of elements in the <paramref name="rangebegin" /> array, which must be the same as the number of elements in the <paramref name="rangeend" /> array.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="rangebegins">An array of 64-bit integers that specifies the minimum values for the integer ranges. The integers in the array must be nonnegative. The 64-bit integers are converted to 32-bit integers before they are stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</param>
    /// <param name="rangeends">An array of 64-bit integers that specifies the maximum values for the integer ranges. The integers in the array must be nonnegative. The 64-bit integers are converted to 32-bit integers before they are stored in the <see cref="T:System.Drawing.Imaging.EncoderParameters" /> object. A maximum value of a given index is paired with the minimum value of the same index.</param>
    public  EncoderParameter(Encoder encoder, long[]? rangebegins, long[]? rangeends)
    {
        ConstructorEncoder = encoder;
        Rangebegins = rangebegins;
        Rangeends = rangeends;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and four arrays of 32-bit integers. The four arrays represent an array rational ranges. A rational range is the set of all fractions from a minimum fractional value through a maximum fractional value. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeRationalRange" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the number of elements in the <paramref name="numerator1" /> array, which must be the same as the number of elements in the other three arrays.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="numerators1">An array of 32-bit integers that specifies the numerators of the minimum values for the ranges. The integers in the array must be nonnegative.</param>
    /// <param name="denominators1">An array of 32-bit integers that specifies the denominators of the minimum values for the ranges. The integers in the array must be nonnegative.</param>
    /// <param name="numerators2">An array of 32-bit integers that specifies the numerators of the maximum values for the ranges. The integers in the array must be nonnegative.</param>
    /// <param name="denominators2">An array of 32-bit integers that specifies the denominators of the maximum values for the ranges. The integers in the array must be nonnegative.</param>
    public  EncoderParameter(Encoder encoder, int[]? numerators1, int[]? denominators1, int[]? numerators2, int[]? denominators2)
    {
        ConstructorEncoder = encoder;
        Numerators1 = numerators1;
        Denominators1 = denominators1;
        Numerators2 = numerators2;
        Denominators2 = denominators2;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and three integers that specify the number of values, the data type of the values, and a pointer to the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="numberOfValues">An integer that specifies the number of values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. The <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property is set to this value.</param>
    /// <param name="type">A member of the <see cref="T:System.Drawing.Imaging.EncoderParameterValueType" /> enumeration that specifies the data type of the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. The <see cref="T:System.Type" /> and <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> properties are set to this value.</param>
    /// <param name="value">A pointer to an array of values of the type specified by the <paramref name="type" /> parameter.</param>
    /// <exception cref="T:System.InvalidOperationException">Type is not a valid <see cref="T:System.Drawing.Imaging.EncoderParameterValueType" />.</exception>
    [Obsolete("This constructor has been deprecated. Use EncoderParameter(Encoder encoder, int numberValues, EncoderParameterValueType type, IntPtr value) instead.")]
    public  EncoderParameter(Encoder encoder, int numberOfValues, int type, int value)
    {
        NumberOfValues = numberOfValues;
        ConstructorEncoder = encoder;
        Type = (EncoderParameterValueType)type;
        Value = value;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object, number of values, data type of the values, and a pointer to the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</summary>
    /// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category.</param>
    /// <param name="numberValues">An integer that specifies the number of values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. The <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property is set to this value.</param>
    /// <param name="type">A member of the <see cref="T:System.Drawing.Imaging.EncoderParameterValueType" /> enumeration that specifies the data type of the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. The <see cref="T:System.Type" /> and <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> properties are set to this value.</param>
    /// <param name="value">A pointer to an array of values of the type specified by the <paramref name="Type" /> parameter.</param>
    public  EncoderParameter(Encoder encoder, int numberValues, EncoderParameterValueType type, IntPtr value)
    {
        ConstructorEncoder = encoder;
        NumberOfValues = numberValues;
        Type = type;
        Value = (long)(object)value;
    }
}