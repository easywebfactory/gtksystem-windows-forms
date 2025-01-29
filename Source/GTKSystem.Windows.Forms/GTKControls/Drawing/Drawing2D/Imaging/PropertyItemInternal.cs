namespace System.Drawing.Imaging
{
    internal struct PropertyItemInternal
    {
        public int id;

        public int len;

        public short type;

        //public unsafe byte* value;

        //public unsafe Span<byte> Value => new Span<byte>(value, len);
    }
}