using System;
using System.IO;

namespace GTKSystem.IO
{
    internal static class BinaryReaderExtensions
    {
        public static int Read7BitEncodedInt(this BinaryReader reader)
        {
            int num = 0;
            int num2 = 0;
            byte b;
            do
            {
                if (num2 == 35)
                {
                    throw new FormatException(SR.Format_Bad7BitInt32);
                }
                b = reader.ReadByte();
                num |= (b & 0x7F) << num2;
                num2 += 7;
            }
            while ((b & 0x80u) != 0);
            return num;
        }
    }
}
