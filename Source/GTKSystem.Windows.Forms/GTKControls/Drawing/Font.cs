using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Drawing
{
    //
    // 摘要:
    //     Defines a particular format for text, including font face, size, and style attributes.
    //     This class cannot be inherited.
    [TypeConverter("System.Drawing.FontConverter, System.Windows.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51")]
    public sealed class Font : MarshalByRefObject, ICloneable, IDisposable, ISerializable
    {
        //
        // 摘要:
        //     Initializes a new System.Drawing.Font that uses the specified existing System.Drawing.Font
        //     and System.Drawing.FontStyle enumeration.
        //
        // 参数:
        //   prototype:
        //     The existing System.Drawing.Font from which to create the new System.Drawing.Font.
        //
        //   newStyle:
        //     The System.Drawing.FontStyle to apply to the new System.Drawing.Font. Multiple
        //     values of the System.Drawing.FontStyle enumeration can be combined with the OR
        //     operator.
        public Font(Font prototype, FontStyle newStyle) : this(prototype.FontFamily, prototype.Size, newStyle, prototype.Unit, prototype.GdiCharSet, prototype.GdiVerticalFont)
        {

        }
        //
        // 摘要:
        //     Initializes a new System.Drawing.Font using a specified size.
        //
        // 参数:
        //   family:
        //     The System.Drawing.FontFamily of the new System.Drawing.Font.
        //
        //   emSize:
        //     The em-size, in points, of the new font.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     emSize is less than or equal to 0, evaluates to infinity, or is not a valid number.
        public Font(FontFamily family, float emSize) : this(family, emSize, FontStyle.Regular, GraphicsUnit.Point, 1, false)
        {

        }
        //
        // 摘要:
        //     Initializes a new System.Drawing.Font using a specified size.
        //
        // 参数:
        //   familyName:
        //     A string representation of the System.Drawing.FontFamily for the new System.Drawing.Font.
        //
        //   emSize:
        //     The em-size, in points, of the new font.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     emSize is less than or equal to 0, evaluates to infinity or is not a valid number.
        public Font(string familyName, float emSize) : this(new FontFamily(familyName), emSize, FontStyle.Regular, GraphicsUnit.Point, 1, false) { }
        //
        // 摘要:
        //     Initializes a new System.Drawing.Font using a specified size and style.
        //
        // 参数:
        //   family:
        //     The System.Drawing.FontFamily of the new System.Drawing.Font.
        //
        //   emSize:
        //     The em-size, in points, of the new font.
        //
        //   style:
        //     The System.Drawing.FontStyle of the new font.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     emSize is less than or equal to 0, evaluates to infinity, or is not a valid number.
        //
        //   T:System.ArgumentNullException:
        //     family is null.
        public Font(FontFamily family, float emSize, FontStyle style) : this(family, emSize, style, GraphicsUnit.Point, 1, false) { }
        //
        // 摘要:
        //     Initializes a new System.Drawing.Font using a specified size and unit. The style
        //     is set to System.Drawing.FontStyle.Regular.
        //
        // 参数:
        //   familyName:
        //     A string representation of the System.Drawing.FontFamily for the new System.Drawing.Font.
        //
        //   emSize:
        //     The em-size of the new font in the units specified by the unit parameter.
        //
        //   unit:
        //     The System.Drawing.GraphicsUnit of the new font.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     emSize is less than or equal to 0, evaluates to infinity, or is not a valid number.
        public Font(string familyName, float emSize, GraphicsUnit unit) : this(new FontFamily(familyName), emSize, FontStyle.Regular, unit, 1, false)
        {

        
        }
        //
        // 摘要:
        //     Initializes a new System.Drawing.Font using a specified size and style.
        //
        // 参数:
        //   familyName:
        //     A string representation of the System.Drawing.FontFamily for the new System.Drawing.Font.
        //
        //   emSize:
        //     The em-size, in points, of the new font.
        //
        //   style:
        //     The System.Drawing.FontStyle of the new font.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     emSize is less than or equal to 0, evaluates to infinity, or is not a valid number.
        public Font(string familyName, float emSize, FontStyle style) : this(new FontFamily(familyName), emSize, style, GraphicsUnit.Point, 1, false)
        {
            this.FontFamily = new FontFamily(familyName);
            this.Size = emSize;
            this.Style = style;
        }
        //
        // 摘要:
        //     Initializes a new System.Drawing.Font using a specified size and unit. Sets the
        //     style to System.Drawing.FontStyle.Regular.
        //
        // 参数:
        //   family:
        //     The System.Drawing.FontFamily of the new System.Drawing.Font.
        //
        //   emSize:
        //     The em-size of the new font in the units specified by the unit parameter.
        //
        //   unit:
        //     The System.Drawing.GraphicsUnit of the new font.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     family is null.
        //
        //   T:System.ArgumentException:
        //     emSize is less than or equal to 0, evaluates to infinity, or is not a valid number.
        public Font(FontFamily family, float emSize, GraphicsUnit unit) : this(family, emSize, FontStyle.Regular, unit, 1, false)
        {

        }
        //
        // 摘要:
        //     Initializes a new System.Drawing.Font using a specified size, style, and unit.
        //
        // 参数:
        //   familyName:
        //     A string representation of the System.Drawing.FontFamily for the new System.Drawing.Font.
        //
        //   emSize:
        //     The em-size of the new font in the units specified by the unit parameter.
        //
        //   style:
        //     The System.Drawing.FontStyle of the new font.
        //
        //   unit:
        //     The System.Drawing.GraphicsUnit of the new font.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     emSize is less than or equal to 0, evaluates to infinity or is not a valid number.
        public Font(string familyName, float emSize, FontStyle style, GraphicsUnit unit) : this(new FontFamily(familyName), emSize, style, unit, 1, false)
        {
        }
        //
        // 摘要:
        //     Initializes a new System.Drawing.Font using a specified size, style, and unit.
        //
        // 参数:
        //   family:
        //     The System.Drawing.FontFamily of the new System.Drawing.Font.
        //
        //   emSize:
        //     The em-size of the new font in the units specified by the unit parameter.
        //
        //   style:
        //     The System.Drawing.FontStyle of the new font.
        //
        //   unit:
        //     The System.Drawing.GraphicsUnit of the new font.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     emSize is less than or equal to 0, evaluates to infinity, or is not a valid number.
        //
        //   T:System.ArgumentNullException:
        //     family is null.
        public Font(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit) : this(family, emSize, style, unit, 1, false) { }
        //
        // 摘要:
        //     Initializes a new System.Drawing.Font using a specified size, style, unit, and
        //     character set.
        //
        // 参数:
        //   family:
        //     The System.Drawing.FontFamily of the new System.Drawing.Font.
        //
        //   emSize:
        //     The em-size of the new font in the units specified by the unit parameter.
        //
        //   style:
        //     The System.Drawing.FontStyle of the new font.
        //
        //   unit:
        //     The System.Drawing.GraphicsUnit of the new font.
        //
        //   gdiCharSet:
        //     A System.Byte that specifies a GDI character set to use for the new font.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     emSize is less than or equal to 0, evaluates to infinity, or is not a valid number.
        //
        //   T:System.ArgumentNullException:
        //     family is null.
        public Font(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet) : this(family, emSize, style, unit, gdiCharSet, false) { }
        //
        // 摘要:
        //     Initializes a new System.Drawing.Font using a specified size, style, unit, and
        //     character set.
        //
        // 参数:
        //   familyName:
        //     A string representation of the System.Drawing.FontFamily for the new System.Drawing.Font.
        //
        //   emSize:
        //     The em-size of the new font in the units specified by the unit parameter.
        //
        //   style:
        //     The System.Drawing.FontStyle of the new font.
        //
        //   unit:
        //     The System.Drawing.GraphicsUnit of the new font.
        //
        //   gdiCharSet:
        //     A System.Byte that specifies a GDI character set to use for this font.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     emSize is less than or equal to 0, evaluates to infinity, or is not a valid number.
        public Font(string familyName, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet) : this(new FontFamily(familyName), emSize, style, unit, gdiCharSet, false) { }
        //
        // 摘要:
        //     Initializes a new System.Drawing.Font using the specified size, style, unit,
        //     and character set.
        //
        // 参数:
        //   familyName:
        //     A string representation of the System.Drawing.FontFamily for the new System.Drawing.Font.
        //
        //   emSize:
        //     The em-size of the new font in the units specified by the unit parameter.
        //
        //   style:
        //     The System.Drawing.FontStyle of the new font.
        //
        //   unit:
        //     The System.Drawing.GraphicsUnit of the new font.
        //
        //   gdiCharSet:
        //     A System.Byte that specifies a GDI character set to use for this font.
        //
        //   gdiVerticalFont:
        //     A Boolean value indicating whether the new System.Drawing.Font is derived from
        //     a GDI vertical font.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     emSize is less than or equal to 0, evaluates to infinity, or is not a valid number.
        public Font(string familyName, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont) :this(new FontFamily(familyName), emSize, style, unit, gdiCharSet, gdiVerticalFont) { }
        //
        // 摘要:
        //     Initializes a new System.Drawing.Font using a specified size, style, unit, and
        //     character set.
        //
        // 参数:
        //   family:
        //     The System.Drawing.FontFamily of the new System.Drawing.Font.
        //
        //   emSize:
        //     The em-size of the new font in the units specified by the unit parameter.
        //
        //   style:
        //     The System.Drawing.FontStyle of the new font.
        //
        //   unit:
        //     The System.Drawing.GraphicsUnit of the new font.
        //
        //   gdiCharSet:
        //     A System.Byte that specifies a GDI character set to use for this font.
        //
        //   gdiVerticalFont:
        //     A Boolean value indicating whether the new font is derived from a GDI vertical
        //     font.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     emSize is less than or equal to 0, evaluates to infinity, or is not a valid number.
        //
        //   T:System.ArgumentNullException:
        //     family is null
        public Font(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont) {
            this.Name = family?.Name;
            this.FontFamily = family;
            this.Size = emSize;
            this.Style = style;
            this.Unit = unit;
            this.GdiCharSet = gdiCharSet;
            this.GdiVerticalFont = gdiVerticalFont;
        }

        //
        // 摘要:
        //     Gets a value that indicates whether this font has the italic style applied.
        //
        // 返回结果:
        //     true to indicate this font has the italic style applied; otherwise, false.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Italic { get => Style.HasFlag(FontStyle.Italic); }
        //
        // 摘要:
        //     Gets a byte value that specifies the GDI character set that this System.Drawing.Font
        //     uses.
        //
        // 返回结果:
        //     A byte value that specifies the GDI character set that this System.Drawing.Font
        //     uses. The default is 1.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public byte GdiCharSet { get; }
        //
        // 摘要:
        //     Gets a Boolean value that indicates whether this System.Drawing.Font is derived
        //     from a GDI vertical font.
        //
        // 返回结果:
        //     true if this System.Drawing.Font is derived from a GDI vertical font; otherwise,
        //     false.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool GdiVerticalFont { get; }
        //
        // 摘要:
        //     Gets the line spacing of this font.
        //
        // 返回结果:
        //     The line spacing, in pixels, of this font.
        [Browsable(false)]
        public int Height { get; }
        //
        // 摘要:
        //     Gets a value indicating whether the font is a member of System.Drawing.SystemFonts.
        //
        // 返回结果:
        //     true if the font is a member of System.Drawing.SystemFonts; otherwise, false.
        //     The default is false.
        [Browsable(false)]
        public bool IsSystemFont { get; }
        //
        // 摘要:
        //     Gets the face name of this System.Drawing.Font.
        //
        // 返回结果:
        //     A string representation of the face name of this System.Drawing.Font.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Name { get; }
        //
        // 摘要:
        //     Gets the name of the system font if the System.Drawing.Font.IsSystemFont property
        //     returns true.
        //
        // 返回结果:
        //     The name of the system font, if System.Drawing.Font.IsSystemFont returns true;
        //     otherwise, an empty string ("").
        [Browsable(false)]
        public string SystemFontName { get; }
        //
        // 摘要:
        //     Gets the em-size of this System.Drawing.Font measured in the units specified
        //     by the System.Drawing.Font.Unit property.
        //
        // 返回结果:
        //     The em-size of this System.Drawing.Font.
        public float Size { get; }
        //
        // 摘要:
        //     Gets the em-size, in points, of this System.Drawing.Font.
        //
        // 返回结果:
        //     The em-size, in points, of this System.Drawing.Font.
        [Browsable(false)]
        public float SizeInPoints { get; }
        //
        // 摘要:
        //     Gets a value that indicates whether this System.Drawing.Font specifies a horizontal
        //     line through the font.
        //
        // 返回结果:
        //     true if this System.Drawing.Font has a horizontal line through it; otherwise,
        //     false.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Strikeout { get; }
        //
        // 摘要:
        //     Gets style information for this System.Drawing.Font.
        //
        // 返回结果:
        //     A System.Drawing.FontStyle enumeration that contains style information for this
        //     System.Drawing.Font.
        [Browsable(false)]
        public FontStyle Style { get; }
        //
        // 摘要:
        //     Gets the System.Drawing.FontFamily associated with this System.Drawing.Font.
        //
        // 返回结果:
        //     The System.Drawing.FontFamily associated with this System.Drawing.Font.
        [Browsable(false)]
        public FontFamily FontFamily { get; }
        //
        // 摘要:
        //     Gets the name of the font originally specified.
        //
        // 返回结果:
        //     The string representing the name of the font originally specified.
        [Browsable(false)]
        public string OriginalFontName { get; }
        //
        // 摘要:
        //     Gets a value that indicates whether this System.Drawing.Font is bold.
        //
        // 返回结果:
        //     true if this System.Drawing.Font is bold; otherwise, false.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Bold { get=> Style.HasFlag(FontStyle.Bold); }
        //
        // 摘要:
        //     Gets the unit of measure for this System.Drawing.Font.
        //
        // 返回结果:
        //     A System.Drawing.GraphicsUnit that represents the unit of measure for this System.Drawing.Font.
        public GraphicsUnit Unit { get; }
        //
        // 摘要:
        //     Gets a value that indicates whether this System.Drawing.Font is underlined.
        //
        // 返回结果:
        //     true if this System.Drawing.Font is underlined; otherwise, false.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Underline { get => Style.HasFlag(FontStyle.Underline); }

        //
        // 摘要:
        //     Creates a System.Drawing.Font from the specified Windows handle to a device context.
        //
        // 参数:
        //   hdc:
        //     A handle to a device context.
        //
        // 返回结果:
        //     The System.Drawing.Font this method creates.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     The font for the specified device context is not a TrueType font.
        public static Font FromHdc(IntPtr hdc) { return new Font("Arial", 12); }
        //
        // 摘要:
        //     Creates a System.Drawing.Font from the specified Windows handle.
        //
        // 参数:
        //   hfont:
        //     A Windows handle to a GDI font.
        //
        // 返回结果:
        //     The System.Drawing.Font this method creates.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     hfont points to an object that is not a TrueType font.
        public static Font FromHfont(IntPtr hfont) { return new Font("Arial", 12); }
        //
        // 摘要:
        //     Creates a System.Drawing.Font from the specified GDI logical font (LOGFONT) structure.
        //
        // 参数:
        //   lf:
        //     An System.Object that represents the GDI LOGFONT structure from which to create
        //     the System.Drawing.Font.
        //
        //   hdc:
        //     A handle to a device context that contains additional information about the lf
        //     structure.
        //
        // 返回结果:
        //     The System.Drawing.Font that this method creates.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     The font is not a TrueType font.
        public static Font FromLogFont(object lf, IntPtr hdc) { return new Font("Arial", 12); }
        //
        // 摘要:
        //     Creates a System.Drawing.Font from the specified GDI logical font (LOGFONT) structure.
        //
        // 参数:
        //   lf:
        //     An System.Object that represents the GDI LOGFONT structure from which to create
        //     the System.Drawing.Font.
        //
        // 返回结果:
        //     The System.Drawing.Font that this method creates.
        public static Font FromLogFont(object lf) { return new Font("Arial", 12); }
        //
        // 摘要:
        //     Creates an exact copy of this System.Drawing.Font.
        //
        // 返回结果:
        //     The System.Drawing.Font this method creates, cast as an System.Object.
        public object Clone() { return null; }
        //
        // 摘要:
        //     Releases all resources used by this System.Drawing.Font.
        public void Dispose() { }
        //
        // 摘要:
        //     Indicates whether the specified object is a System.Drawing.Font and has the same
        //     System.Drawing.Font.FontFamily, System.Drawing.Font.GdiVerticalFont, System.Drawing.Font.GdiCharSet,
        //     System.Drawing.Font.Style, System.Drawing.Font.Size, and System.Drawing.Font.Unit
        //     property values as this System.Drawing.Font.
        //
        // 参数:
        //   obj:
        //     The object to test.
        //
        // 返回结果:
        //     true if the obj parameter is a System.Drawing.Font and has the same System.Drawing.Font.FontFamily,
        //     System.Drawing.Font.GdiVerticalFont, System.Drawing.Font.GdiCharSet, System.Drawing.Font.Style,
        //     System.Drawing.Font.Size, and System.Drawing.Font.Unit property values as this
        //     System.Drawing.Font; otherwise, false.
        public override bool Equals(object obj) { return Equals(obj as Font); }
        //
        // 摘要:
        //     Gets the hash code for this System.Drawing.Font.
        //
        // 返回结果:
        //     The hash code for this System.Drawing.Font.
        public override int GetHashCode() { return base.GetHashCode(); }
        //
        // 摘要:
        //     Returns the height, in pixels, of this System.Drawing.Font when drawn to a device
        //     with the specified vertical resolution.
        //
        // 参数:
        //   dpi:
        //     The vertical resolution, in dots per inch, used to calculate the height of the
        //     font.
        //
        // 返回结果:
        //     The height, in pixels, of this System.Drawing.Font.
        public float GetHeight(float dpi) { return Height; }
        //
        // 摘要:
        //     Returns the line spacing, in the current unit of a specified System.Drawing.Graphics,
        //     of this font.
        //
        // 参数:
        //   graphics:
        //     A System.Drawing.Graphics that holds the vertical resolution, in dots per inch,
        //     of the display device as well as settings for page unit and page scale.
        //
        // 返回结果:
        //     The line spacing, in pixels, of this font.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     graphics is null.
        public float GetHeight(Graphics graphics) { return Height; }
        //
        // 摘要:
        //     Returns the line spacing, in pixels, of this font.
        //
        // 返回结果:
        //     The line spacing, in pixels, of this font.
        public float GetHeight() { return Height; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
           
        }

        //
        // 摘要:
        //     Returns a handle to this System.Drawing.Font.
        //
        // 返回结果:
        //     A Windows handle to this System.Drawing.Font.
        //
        // 异常:
        //   T:System.ComponentModel.Win32Exception:
        //     The operation was unsuccessful.
        public IntPtr ToHfont() { return IntPtr.Zero; }
        //
        // 摘要:
        //     Creates a GDI logical font (LOGFONT) structure from this System.Drawing.Font.
        //
        // 参数:
        //   logFont:
        //     An System.Object to represent the LOGFONT structure that this method creates.
        //
        //   graphics:
        //     A System.Drawing.Graphics that provides additional information for the LOGFONT
        //     structure.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     graphics is null.
        public void ToLogFont(object logFont, Graphics graphics) { }
        //
        // 摘要:
        //     Creates a GDI logical font (LOGFONT) structure from this System.Drawing.Font.
        //
        // 参数:
        //   logFont:
        //     An System.Object to represent the LOGFONT structure that this method creates.
        public void ToLogFont(object logFont) { }
        //
        // 摘要:
        //     Returns a human-readable string representation of this System.Drawing.Font.
        //
        // 返回结果:
        //     A string that represents this System.Drawing.Font.
        public override string ToString() { return "Font"; }
    }
}