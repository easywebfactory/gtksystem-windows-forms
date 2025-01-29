using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GTKSystem.Resources
{
    internal sealed class FastResourceComparer : IComparer, IEqualityComparer, IComparer<string>,
        IEqualityComparer<string>
    {
        internal static readonly FastResourceComparer Default = new FastResourceComparer();

        public int GetHashCode(object key)
        {
            string key2 = (string)key;
            return HashFunction(key2);
        }

        public int GetHashCode([DisallowNull] string key)
        {
            return HashFunction(key);
        }

        internal static int HashFunction(string key)
        {
            uint num = 5381u;
            for (int i = 0; i < key.Length; i++)
            {
                num = ((num << 5) + num) ^ key[i];
            }

            return (int)num;
        }

        public int Compare(object a, object b)
        {
            if (a == b)
            {
                return 0;
            }

            string strA = (string)a;
            string strB = (string)b;
            return string.CompareOrdinal(strA, strB);
        }

        public int Compare(string a, string b)
        {
            return string.CompareOrdinal(a, b);
        }

        public bool Equals(string a, string b)
        {
            return string.Equals(a, b);
        }

        public new bool Equals(object a, object b)
        {
            if (a == b)
            {
                return true;
            }

            string a2 = (string)a;
            string b2 = (string)b;
            return string.Equals(a2, b2);
        }

        public unsafe static int CompareOrdinal(string a, byte[] bytes, int bCharLength)
        {
            int num = 0;
            int num2 = 0;
            int num3 = a.Length;
            if (num3 > bCharLength)
            {
                num3 = bCharLength;
            }

            if (bCharLength == 0)
            {
                if (a.Length != 0)
                {
                    return -1;
                }

                return 0;
            }

            fixed (byte* ptr = bytes)
            {
                byte* ptr2 = ptr;
                while (num < num3 && num2 == 0)
                {
                    int num4 = *ptr2 | (ptr2[1] << 8);
                    num2 = a[num++] - num4;
                    ptr2 += 2;
                }
            }

            if (num2 != 0)
            {
                return num2;
            }

            return a.Length - bCharLength;
        }

        public static int CompareOrdinal(byte[] bytes, int aCharLength, string b)
        {
            return -CompareOrdinal(b, bytes, aCharLength);
        }

        internal unsafe static int CompareOrdinal(byte* a, int byteLen, string b)
        {
            int num = 0;
            int num2 = 0;
            int num3 = byteLen >> 1;
            if (num3 > b.Length)
            {
                num3 = b.Length;
            }

            while (num2 < num3 && num == 0)
            {
                char c = (char)(*(a++) | (*(a++) << 8));
                num = c - b[num2++];
            }

            if (num != 0)
            {
                return num;
            }

            return byteLen - b.Length * 2;
        }
    }
}