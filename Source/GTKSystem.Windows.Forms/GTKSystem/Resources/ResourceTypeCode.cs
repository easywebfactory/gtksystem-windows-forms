namespace GTKSystem.Resources
{
    internal enum ResourceTypeCode
    {
        Null = 0,
        String = 1,
        Boolean = 2,
        Char = 3,
        Byte = 4,
        SByte = 5,
        Int16 = 6,
        UInt16 = 7,
        Int32 = 8,
        UInt32 = 9,
        Int64 = 10,
        UInt64 = 11,
        Single = 12,
        Double = 13,
        Decimal = 14,
        DateTime = 0xF,
        TimeSpan = 0x10,
        LastPrimitive = 0x10,
        ByteArray = 0x20,
        Stream = 33,
        StartOfUserTypes = 0x40
    }
}