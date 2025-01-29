using System;

namespace GTKSystem.Numerics.Hashing
{
    internal static class HashHelpers
    {
        public static readonly int RandomSeed = new Random().Next(int.MinValue, int.MaxValue);

        public static int Combine(int h1, int h2)
        {
            uint num = (uint)(h1 << 5) | ((uint)h1 >> 27);
            return ((int)num + h1) ^ h2;
        }
    }
}