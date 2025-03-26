namespace System.Resources;

internal enum SerializationFormat
{
    BinaryFormatter = 1,
    TypeConverterByteArray,
    TypeConverterString,
    ActivatorStream
}