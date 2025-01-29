
using Gdk;
using System.Drawing.Text;

namespace System.Drawing
{
    /// <summary>
    ///     Defines a group of type faces having a similar basic design and certain variations
    ///     in styles. This class cannot be inherited.
    /// </summary>
    public sealed class FontFamily : MarshalByRefObject, IDisposable
    {
        /// <summary>
        ///     Initializes a new System.Drawing.FontFamily from the specified generic font family.
        /// </summary>
        /// <param name="genericFamily">
        ///     The System.Drawing.Text.GenericFontFamilies from which to create the new System.Drawing.FontFamily.
        /// </param>
        public FontFamily(GenericFontFamilies genericFamily) : this(genericFamily.ToString(), null)
        {
        }
        /// <summary>
        ///     Initializes a new System.Drawing.FontFamily with the specified name.
        /// </summary>
        /// <param name="name">
        ///     The name of the new System.Drawing.FontFamily.
        ///</param>
        /// <exception cref="T:System.ArgumentException">
        ///     name is an empty string (""). -or- name specifies a font that is not installed
        ///     on the computer running the application. -or- name specifies a font that is not
        ///     a TrueType font.
        /// </exception>
        public FontFamily(string name) : this(name, null) { }
        /// <summary>
        ///     Initializes a new System.Drawing.FontFamily in the specified System.Drawing.Text.FontCollection
        ///     with the specified name.
        /// </summary>
        /// <param name="name">
        ///     A System.String that represents the name of the new System.Drawing.FontFamily.
        /// </param>
        /// <param name="fontCollection">
        ///     The System.Drawing.Text.FontCollection that contains this System.Drawing.FontFamily.
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        ///     name is an empty string (""). -or- name specifies a font that is not installed
        ///     on the computer running the application. -or- name specifies a font that is not
        ///     a TrueType font.
        /// </exception>
        public FontFamily(string name, FontCollection fontCollection) {
            this.Name = name;
        }


      /// <summary>
        ///     Gets a generic sans serif System.Drawing.FontFamily object.
        ///</summary>
        /// <returns>
        ///     A System.Drawing.FontFamily object that represents a generic sans serif font.
        /// </returns>
        public static FontFamily GenericSansSerif { get => new FontFamily(GenericFontFamilies.SansSerif); }
        /// <summary>
        ///     Gets a generic monospace System.Drawing.FontFamily.
        ///</summary>
        /// <returns>
        ///     A System.Drawing.FontFamily that represents a generic monospace font.
        /// </returns>
        public static FontFamily GenericMonospace { get => new FontFamily(GenericFontFamilies.Monospace); }

        /// <summary>
        ///     Returns an array that contains all the System.Drawing.FontFamily objects associated
        ///     with the current graphics context.
        ///</summary>
        /// <returns>
        ///     An array of System.Drawing.FontFamily objects associated with the current graphics
        ///     context.
        /// </returns>
        public static FontFamily[] Families { get => Array.ConvertAll(PangoHelper.ContextGet().Families, o => new FontFamily(o.Name)); }

        /// <summary>
        ///     Gets a generic serif System.Drawing.FontFamily.
        ///</summary>
        /// <returns>
        ///     A System.Drawing.FontFamily that represents a generic serif font.
        /// </returns>
        public static FontFamily GenericSerif { get => new FontFamily(GenericFontFamilies.Serif); }
        /// <summary>
        ///     Gets the name of this System.Drawing.FontFamily.
        ///</summary>
        /// <returns>
        ///     A System.String that represents the name of this System.Drawing.FontFamily.
        /// </returns>
        public string Name { get; set; }

        /// <summary>
        ///     Returns an array that contains all the System.Drawing.FontFamily objects available
        ///     for the specified graphics context.
        ///</summary>
        /// <param name="graphics">
        ///     The System.Drawing.Graphics object from which to return System.Drawing.FontFamily
        ///     objects.
        ///</param>
        /// <returns>
        ///     An array of System.Drawing.FontFamily objects available for the specified System.Drawing.Graphics
        ///     object.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     graphics is null.
        /// </exception>
        [Obsolete("Do not use method GetFamilies, use property Families instead")]
        public static FontFamily[] GetFamilies(Graphics graphics) { return new FontFamily[0]; }
        /// <summary>
        ///     Releases all resources used by this System.Drawing.FontFamily.
        ///</summary>
        public void Dispose() { }
        /// <summary>
        ///     Indicates whether the specified object is a System.Drawing.FontFamily and is
        ///     identical to this System.Drawing.FontFamily.
        ///</summary>
        /// <param name="obj">
        ///     The object to test.
        ///</param>
        /// <returns>
        ///     true if obj is a System.Drawing.FontFamily and is identical to this System.Drawing.FontFamily;
        ///     otherwise, false.
        /// </returns>
        public override bool Equals(object obj) { return Equals(obj as FontFamily); }
        /// <summary>
        ///     Returns the cell ascent, in design units, of the System.Drawing.FontFamily of
        ///     the specified style.
        ///</summary>
        /// <param name="style">
        ///     A System.Drawing.FontStyle that contains style information for the font.
        ///</param>
        /// <returns>
        ///     The cell ascent for this System.Drawing.FontFamily that uses the specified System.Drawing.FontStyle.
        /// </returns>
        public int GetCellAscent(FontStyle style) { return 0; }
        /// <summary>
        ///     Returns the cell descent, in design units, of the System.Drawing.FontFamily of
        ///     the specified style.
        ///</summary>
        /// <param name="style">
        ///     A System.Drawing.FontStyle that contains style information for the font.
        ///</param>
        /// <returns>
        ///     The cell descent metric for this System.Drawing.FontFamily that uses the specified
        ///     System.Drawing.FontStyle.
        /// </returns>
        public int GetCellDescent(FontStyle style) { return 0; }
        /// <summary>
        ///     Gets the height, in font design units, of the em square for the specified style.
        ///</summary>
        /// <param name="style">
        ///     The System.Drawing.FontStyle for which to get the em height.
        ///</param>
        /// <returns>
        ///     The height of the em square.
        /// </returns>
        public int GetEmHeight(FontStyle style) { return 0; }
        /// <summary>
        ///     Gets a hash code for this System.Drawing.FontFamily.
        ///</summary>
        /// <returns>
        ///     The hash code for this System.Drawing.FontFamily.
        /// </returns>
        public override int GetHashCode() { return Name.GetHashCode(); }
        /// <summary>
        ///     Returns the line spacing, in design units, of the System.Drawing.FontFamily of
        ///     the specified style. The line spacing is the vertical distance between the base
        ///     lines of two consecutive lines of text.
        ///</summary>
        /// <param name="style">
        ///     The System.Drawing.FontStyle to apply.
        ///</param>
        /// <returns>
        ///     The distance between two consecutive lines of text.
        /// </returns>
        public int GetLineSpacing(FontStyle style) { return 0; }
        /// <summary>
        ///     Returns the name, in the specified language, of this System.Drawing.FontFamily.
        ///</summary>
        /// <param name="language">
        ///     The language in which the name is returned.
        ///</param>
        /// <returns>
        ///     A System.String that represents the name, in the specified language, of this
        ///     System.Drawing.FontFamily.
        /// </returns>
        public string GetName(int language) { return Name; }
        /// <summary>
        ///     Indicates whether the specified System.Drawing.FontStyle enumeration is available.
        /// </summary>
        /// <param name="style">
        ///     The System.Drawing.FontStyle to test.
        ///</param>
        /// <returns>
        ///     true if the specified System.Drawing.FontStyle is available; otherwise, false.
        /// </returns>
        public bool IsStyleAvailable(FontStyle style) { return false; }
        /// <summary>
        ///     Converts this System.Drawing.FontFamily to a human-readable string representation.
        /// </summary>
        /// <returns>
        ///     The string that represents this System.Drawing.FontFamily.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}