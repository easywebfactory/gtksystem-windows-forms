namespace System.Drawing.Imaging
{
    internal struct EncoderParameterPrivate
    {
        public Guid ParameterGuid;

        public int NumberOfValues;

        public EncoderParameterValueType ParameterValueType;

        public IntPtr ParameterValue;
    }
}