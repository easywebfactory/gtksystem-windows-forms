namespace System.Drawing.Imaging;

internal struct EncoderParameterPrivate
{
    public Guid parameterGuid;

    public int numberOfValues;

    public EncoderParameterValueType parameterValueType;

    public IntPtr parameterValue;
}