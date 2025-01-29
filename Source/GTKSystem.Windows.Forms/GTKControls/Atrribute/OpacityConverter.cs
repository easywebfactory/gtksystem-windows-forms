using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
    /// <summary>
    ///     Provides a type converter to convert opacity values to and from a string.
    /// </summary>
    public class OpacityConverter : TypeConverter
    {
        /// <summary>
        ///     Initializes an instance of the System.Windows.Forms.OpacityConverter class.
        /// </summary>
        public OpacityConverter()
        {

        }

        /// <summary>
        ///     Returns a value indicating whether this converter can convert an object of the
        ///     specified source type to the native type of the converter that uses the specified
        ///     context.
        /// </summary>
        /// <param name="context">
        ///     A System.ComponentModel.ITypeDescriptorContext that provides information about
        ///     the context of a type converter.
        /// </param>
        /// <param name="sourceType">
        ///     The System.Type that represents the type you want to convert from.
        /// </param>
        /// <returns>
        ///     true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        /// <summary>
        ///     Converts the specified object to the converter's native type.
        /// </summary>
        /// <param name="context">
        ///     A System.ComponentModel.ITypeDescriptorContext that provides information about
        ///     the context of a type converter.
        /// </param>
        /// <param name="culture">
        ///     The locale information for the conversion.
        /// </param>
        /// <param name="value">
        ///     The object to convert.
        ///</param>
        /// <returns>
        ///     A System.Object that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.Exception">
        ///     The object was not a supported type for the conversion.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///     value could not be properly converted to type System.Double. -or- The resulting
        ///     converted value was less than zero percent or greater than one hundred percent.
        /// </exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) { return value; }

        /// <summary>
        ///     Converts from the converter's native type to a value of the destination type.
        /// </summary>
        /// <param name="context">
        ///     A System.ComponentModel.ITypeDescriptorContext that provides information about
        ///     the context of a type converter.
        /// </param>
        /// <param name="culture">
        ///     The locale information for the conversion.
        /// </param>
        /// <param name="value">
        ///     The value to convert.
        /// </param>
        /// <param name="destinationType">
        ///     The type to convert the object to.
        /// </param>
        /// <returns>
        ///     A System.Object that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     destinationType is null.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     value cannot be converted to the destinationType.
        /// </exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) { return value; }
    }
}

