using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
    //
    // 摘要:
    //     Provides a type converter to convert opacity values to and from a string.
    public class OpacityConverter : TypeConverter
    {
        //
        // 摘要:
        //     Initializes an instance of the System.Windows.Forms.OpacityConverter class.
        public OpacityConverter()
        {

        }

        //
        // 摘要:
        //     Returns a value indicating whether this converter can convert an object of the
        //     specified source type to the native type of the converter that uses the specified
        //     context.
        //
        // 参数:
        //   context:
        //     A System.ComponentModel.ITypeDescriptorContext that provides information about
        //     the context of a type converter.
        //
        //   sourceType:
        //     The System.Type that represents the type you want to convert from.
        //
        // 返回结果:
        //     true if this converter can perform the conversion; otherwise, false.
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }
        //
        // 摘要:
        //     Converts the specified object to the converter's native type.
        //
        // 参数:
        //   context:
        //     A System.ComponentModel.ITypeDescriptorContext that provides information about
        //     the context of a type converter.
        //
        //   culture:
        //     The locale information for the conversion.
        //
        //   value:
        //     The object to convert.
        //
        // 返回结果:
        //     An System.Object that represents the converted value.
        //
        // 异常:
        //   T:System.Exception:
        //     The object was not a supported type for the conversion.
        //
        //   T:System.FormatException:
        //     value could not be properly converted to type System.Double. -or- The resulting
        //     converted value was less than zero percent or greater than one hundred percent.
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) { return value; }
        //
        // 摘要:
        //     Converts from the converter's native type to a value of the destination type.
        //
        // 参数:
        //   context:
        //     A System.ComponentModel.ITypeDescriptorContext that provides information about
        //     the context of a type converter.
        //
        //   culture:
        //     The locale information for the conversion.
        //
        //   value:
        //     The value to convert.
        //
        //   destinationType:
        //     The type to convert the object to.
        //
        // 返回结果:
        //     An System.Object that represents the converted value.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     destinationType is null.
        //
        //   T:System.NotSupportedException:
        //     value cannot be converted to the destinationType.
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) { return value; }
    }
}

