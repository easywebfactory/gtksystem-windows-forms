using System.Drawing.Text;

namespace System.Drawing
{
    /// <summary>Encapsulates text layout information (such as alignment, orientation and tab stops) display manipulations (such as ellipsis insertion and national digit substitution) and OpenType features. This class cannot be inherited.</summary>
    public sealed class StringFormat : MarshalByRefObject, ICloneable, IDisposable
    {
        /// <summary>Gets or sets horizontal alignment of the string.</summary>
        /// <returns>A <see cref="T:System.Drawing.StringAlignment" /> enumeration that specifies the horizontal  alignment of the string.</returns>
        public StringAlignment Alignment
        {
            get { throw null; }
            set { }
        }

        /// <summary>Gets the language that is used when local digits are substituted for western digits.</summary>
        /// <returns>A National Language Support (NLS) language identifier that identifies the language that will be used when local digits are substituted for western digits. You can pass the <see cref="P:System.Globalization.CultureInfo.LCID" /> property of a <see cref="T:System.Globalization.CultureInfo" /> object as the NLS language identifier. For example, suppose you create a <see cref="T:System.Globalization.CultureInfo" /> object by passing the string "ar-EG" to a <see cref="T:System.Globalization.CultureInfo" /> constructor. If you pass the <see cref="P:System.Globalization.CultureInfo.LCID" /> property of that <see cref="T:System.Globalization.CultureInfo" /> object along with <see cref="F:System.Drawing.StringDigitSubstitute.Traditional" /> to the <see cref="M:System.Drawing.StringFormat.SetDigitSubstitution(System.Int32,System.Drawing.StringDigitSubstitute)" /> method, then Arabic-Indic digits will be substituted for western digits at display time.</returns>
        public int DigitSubstitutionLanguage
        {
            get { throw null; }
        }

        /// <summary>Gets the method to be used for digit substitution.</summary>
        /// <returns>A <see cref="T:System.Drawing.StringDigitSubstitute" /> enumeration value that specifies how to substitute characters in a string that cannot be displayed because they are not supported by the current font.</returns>
        public StringDigitSubstitute DigitSubstitutionMethod
        {
            get { throw null; }
        }

        /// <summary>Gets or sets a <see cref="T:System.Drawing.StringFormatFlags" /> enumeration that contains formatting information.</summary>
        /// <returns>A <see cref="T:System.Drawing.StringFormatFlags" /> enumeration that contains formatting information.</returns>
        public StringFormatFlags FormatFlags
        {
            get { throw null; }
            set { }
        }

        /// <summary>Gets a generic default <see cref="T:System.Drawing.StringFormat" /> object.</summary>
        /// <returns>The generic default <see cref="T:System.Drawing.StringFormat" /> object.</returns>
        public static StringFormat GenericDefault
        {
            get { throw null; }
        }

        /// <summary>Gets a generic typographic <see cref="T:System.Drawing.StringFormat" /> object.</summary>
        /// <returns>A generic typographic <see cref="T:System.Drawing.StringFormat" /> object.</returns>
        public static StringFormat GenericTypographic
        {
            get { throw null; }
        }

        /// <summary>Gets or sets the <see cref="T:System.Drawing.Text.HotkeyPrefix" /> object for this <see cref="T:System.Drawing.StringFormat" /> object.</summary>
        /// <returns>The <see cref="T:System.Drawing.Text.HotkeyPrefix" /> object for this <see cref="T:System.Drawing.StringFormat" /> object, the default is <see cref="F:System.Drawing.Text.HotkeyPrefix.None" />.</returns>
        public HotkeyPrefix HotkeyPrefix
        {
            get { throw null; }
            set { }
        }

        /// <summary>Gets or sets the vertical alignment of the string.</summary>
        /// <returns>A <see cref="T:System.Drawing.StringAlignment" /> enumeration that represents the vertical line alignment.</returns>
        public StringAlignment LineAlignment
        {
            get { throw null; }
            set { }
        }

        /// <summary>Gets or sets the <see cref="T:System.Drawing.StringTrimming" /> enumeration for this <see cref="T:System.Drawing.StringFormat" /> object.</summary>
        /// <returns>A <see cref="T:System.Drawing.StringTrimming" /> enumeration that indicates how text drawn with this <see cref="T:System.Drawing.StringFormat" /> object is trimmed when it exceeds the edges of the layout rectangle.</returns>
        public StringTrimming Trimming
        {
            get { throw null; }
            set { }
        }

        /// <summary>Initializes a new <see cref="T:System.Drawing.StringFormat" /> object.</summary>
        public StringFormat()
        {
        }

        /// <summary>Initializes a new <see cref="T:System.Drawing.StringFormat" /> object from the specified existing <see cref="T:System.Drawing.StringFormat" /> object.</summary>
        /// <param name="format">The <see cref="T:System.Drawing.StringFormat" /> object from which to initialize the new <see cref="T:System.Drawing.StringFormat" /> object.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="format" /> is <see langword="null" />.</exception>
        public StringFormat(StringFormat format)
        {
        }

        /// <summary>Initializes a new <see cref="T:System.Drawing.StringFormat" /> object with the specified <see cref="T:System.Drawing.StringFormatFlags" /> enumeration.</summary>
        /// <param name="options">The <see cref="T:System.Drawing.StringFormatFlags" /> enumeration for the new <see cref="T:System.Drawing.StringFormat" /> object.</param>
        public StringFormat(StringFormatFlags options)
        {
        }

        /// <summary>Initializes a new <see cref="T:System.Drawing.StringFormat" /> object with the specified <see cref="T:System.Drawing.StringFormatFlags" /> enumeration and language.</summary>
        /// <param name="options">The <see cref="T:System.Drawing.StringFormatFlags" /> enumeration for the new <see cref="T:System.Drawing.StringFormat" /> object.</param>
        /// <param name="language">A value that indicates the language of the text.</param>
        public StringFormat(StringFormatFlags options, int language)
        {
        }

        /// <summary>Creates an exact copy of this <see cref="T:System.Drawing.StringFormat" /> object.</summary>
        /// <returns>The <see cref="T:System.Drawing.StringFormat" /> object this method creates.</returns>
        public object Clone()
        {
            throw null;
        }

        /// <summary>Releases all resources used by this <see cref="T:System.Drawing.StringFormat" /> object.</summary>
        public void Dispose()
        {
        }

        /// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
        ~StringFormat()
        {
        }

        /// <summary>Gets the tab stops for this <see cref="T:System.Drawing.StringFormat" /> object.</summary>
        /// <param name="firstTabOffset">The number of spaces between the beginning of a text line and the first tab stop.</param>
        /// <returns>An array of distances (in number of spaces) between tab stops.</returns>
        public float[] GetTabStops(out float firstTabOffset)
        {
            throw null;
        }

        /// <summary>Specifies the language and method to be used when local digits are substituted for western digits.</summary>
        /// <param name="language">A National Language Support (NLS) language identifier that identifies the language that will be used when local digits are substituted for western digits. You can pass the <see cref="P:System.Globalization.CultureInfo.LCID" /> property of a <see cref="T:System.Globalization.CultureInfo" /> object as the NLS language identifier. For example, suppose you create a <see cref="T:System.Globalization.CultureInfo" /> object by passing the string "ar-EG" to a <see cref="T:System.Globalization.CultureInfo" /> constructor. If you pass the <see cref="P:System.Globalization.CultureInfo.LCID" /> property of that <see cref="T:System.Globalization.CultureInfo" /> object along with <see cref="F:System.Drawing.StringDigitSubstitute.Traditional" /> to the <see cref="M:System.Drawing.StringFormat.SetDigitSubstitution(System.Int32,System.Drawing.StringDigitSubstitute)" /> method, then Arabic-Indic digits will be substituted for western digits at display time.</param>
        /// <param name="substitute">An element of the <see cref="T:System.Drawing.StringDigitSubstitute" /> enumeration that specifies how digits are displayed.</param>
        public void SetDigitSubstitution(int language, StringDigitSubstitute substitute)
        {
        }

        /// <summary>Specifies an array of <see cref="T:System.Drawing.CharacterRange" /> structures that represent the ranges of characters measured by a call to the <see cref="M:System.Drawing.Graphics.MeasureCharacterRanges(System.String,System.Drawing.Font,System.Drawing.RectangleF,System.Drawing.StringFormat)" /> method.</summary>
        /// <param name="ranges">An array of <see cref="T:System.Drawing.CharacterRange" /> structures that specifies the ranges of characters measured by a call to the <see cref="M:System.Drawing.Graphics.MeasureCharacterRanges(System.String,System.Drawing.Font,System.Drawing.RectangleF,System.Drawing.StringFormat)" /> method.</param>
        /// <exception cref="T:System.OverflowException">More than 32 character ranges are set.</exception>
        public void SetMeasurableCharacterRanges(CharacterRange[] ranges)
        {
        }

        /// <summary>Sets tab stops for this <see cref="T:System.Drawing.StringFormat" /> object.</summary>
        /// <param name="firstTabOffset">The number of spaces between the beginning of a line of text and the first tab stop.</param>
        /// <param name="tabStops">An array of distances between tab stops in the units specified by the <see cref="P:System.Drawing.Graphics.PageUnit" /> property.</param>
        public void SetTabStops(float firstTabOffset, float[] tabStops)
        {
        }

        /// <summary>Converts this <see cref="T:System.Drawing.StringFormat" /> object to a human-readable string.</summary>
        /// <returns>A string representation of this <see cref="T:System.Drawing.StringFormat" /> object.</returns>
        public override string ToString()
        {
            throw null;
        }
    }
}