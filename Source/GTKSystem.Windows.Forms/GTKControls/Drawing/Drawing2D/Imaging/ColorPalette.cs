using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
    /// <summary>Defines an array of colors that make up a color palette. The colors are 32-bit ARGB colors. Not inheritable.</summary>
    public sealed class ColorPalette
    {
        private int _flags;

        private Color[] _entries;

        /// <summary>Gets a value that specifies how to interpret the color information in the array of colors.</summary>
        /// <returns>The following flag values are valid:
        /// 0x00000001
        /// The color values in the array contain alpha information.
        /// 0x00000002
        /// The colors in the array are grayscale values.
        /// 0x00000004
        /// The colors in the array are halftone values.</returns>
        public int Flags => _flags;

        /// <summary>Gets an array of <see cref="T:System.Drawing.Color" /> structures.</summary>
        /// <returns>The array of <see cref="T:System.Drawing.Color" /> structure that make up this <see cref="T:System.Drawing.Imaging.ColorPalette" />.</returns>
        public Color[] Entries => _entries;

        internal ColorPalette(int count)
        {
            _entries = new Color[count];
        }

        internal ColorPalette()
        {
            _entries = new Color[1];
        }

        internal void ConvertFromMemory(IntPtr memory)
        {
            _flags = Marshal.ReadInt32(memory);
            int num = Marshal.ReadInt32((IntPtr)((long)memory + 4));
            _entries = new Color[num];
            for (int i = 0; i < num; i++)
            {
                int argb = Marshal.ReadInt32((IntPtr)((long)memory + 8 + i * 4));
                _entries[i] = Color.FromArgb(argb);
            }
        }

        internal IntPtr ConvertToMemory()
        {
            int num = _entries.Length;
            IntPtr intPtr;
            checked
            {
                intPtr = Marshal.AllocHGlobal(4 * (2 + num));
                Marshal.WriteInt32(intPtr, 0, _flags);
                Marshal.WriteInt32((IntPtr)((long)intPtr + 4), 0, num);
            }

            for (int i = 0; i < num; i++)
            {
                Marshal.WriteInt32((IntPtr)((long)intPtr + 4 * (i + 2)), 0, _entries[i].ToArgb());
            }

            return intPtr;
        }
    }
}