namespace System.Windows.Forms.Resources.IO;

internal static class BinaryReaderExtensions
{
    public static int Read7BitEncodedInt(this BinaryReader? reader)
    {
        var num = 0;
        var num2 = 0;
        byte b=default;
        do
        {
            if (num2 == 35)
            {
                throw new FormatException(Messages.FormatBad7BitInt32);
            }

            if (reader != null)
            {
                b = reader.ReadByte();
            }

            num |= (b & 0x7F) << num2;
            num2 += 7;
        }
        while ((b & 0x80u) != 0);
        return num;
    }
}