namespace System.Drawing
{
    /// <summary>Specifies a range of character positions within a string.</summary>
    public struct CharacterRange
    {
        private int _dummyPrimitive;

        /// <summary>Gets or sets the position in the string of the first character of this <see cref="T:System.Drawing.CharacterRange" />.</summary>
        /// <returns>The first position of this <see cref="T:System.Drawing.CharacterRange" />.</returns>
        public int First
        {
            get { throw null; }
            set { }
        }

        /// <summary>Gets or sets the number of positions in this <see cref="T:System.Drawing.CharacterRange" />.</summary>
        /// <returns>The number of positions in this <see cref="T:System.Drawing.CharacterRange" />.</returns>
        public int Length
        {
            get { throw null; }
            set { }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.CharacterRange" /> structure, specifying a range of character positions within a string.</summary>
        /// <param name="First">The position of the first character in the range. For example, if <paramref name="First" /> is set to 0, the first position of the range is position 0 in the string.</param>
        /// <param name="Length">The number of positions in the range.</param>
        public CharacterRange(int First, int Length)
        {
            throw null;
        }

        /// <summary>Gets a value indicating whether this object is equivalent to the specified object.</summary>
        /// <param name="obj">The object to compare to for equality.</param>
        /// <returns>
        ///   <see langword="true" /> to indicate the specified object is an instance with the same <see cref="P:System.Drawing.CharacterRange.First" /> and <see cref="P:System.Drawing.CharacterRange.Length" /> value as this instance; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            throw null;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            throw null;
        }

        /// <summary>Compares two <see cref="T:System.Drawing.CharacterRange" /> objects. Gets a value indicating whether the <see cref="P:System.Drawing.CharacterRange.First" /> and <see cref="P:System.Drawing.CharacterRange.Length" /> values of the two <see cref="T:System.Drawing.CharacterRange" /> objects are equal.</summary>
        /// <param name="cr1">A <see cref="T:System.Drawing.CharacterRange" /> to compare for equality.</param>
        /// <param name="cr2">A <see cref="T:System.Drawing.CharacterRange" /> to compare for equality.</param>
        /// <returns>
        ///   <see langword="true" /> to indicate the two <see cref="T:System.Drawing.CharacterRange" /> objects have the same <see cref="P:System.Drawing.CharacterRange.First" /> and <see cref="P:System.Drawing.CharacterRange.Length" /> values; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(CharacterRange cr1, CharacterRange cr2)
        {
            throw null;
        }

        /// <summary>Compares two <see cref="T:System.Drawing.CharacterRange" /> objects. Gets a value indicating whether the <see cref="P:System.Drawing.CharacterRange.First" /> or <see cref="P:System.Drawing.CharacterRange.Length" /> values of the two <see cref="T:System.Drawing.CharacterRange" /> objects are not equal.</summary>
        /// <param name="cr1">A <see cref="T:System.Drawing.CharacterRange" /> to compare for inequality.</param>
        /// <param name="cr2">A <see cref="T:System.Drawing.CharacterRange" /> to compare for inequality.</param>
        /// <returns>
        ///   <see langword="true" /> to indicate the either the <see cref="P:System.Drawing.CharacterRange.First" /> or <see cref="P:System.Drawing.CharacterRange.Length" /> values of the two <see cref="T:System.Drawing.CharacterRange" /> objects differ; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(CharacterRange cr1, CharacterRange cr2)
        {
            throw null;
        }
    }
}