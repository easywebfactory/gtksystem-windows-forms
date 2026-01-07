
using Gdk;
using System.Drawing.Text;
using System.Linq;

namespace System.Drawing
{
    //
    // 摘要:
    //     Defines a group of type faces having a similar basic design and certain variations
    //     in styles. This class cannot be inherited.
    public sealed class FontFamily : MarshalByRefObject, IDisposable
    {
        private static Dictionary<string, string> fontLanguages = new Dictionary<string, string>();
        static FontFamily()
        {
            if (fontLanguages.Count == 0)
            {
                fontLanguages.Add("宋体", "SimSun");
                fontLanguages.Add("黑体", "SimHei");
                fontLanguages.Add("微软雅黑", "Microsoft Yahei");
                fontLanguages.Add("微软正黑", "Microsoft JhengHei");
                fontLanguages.Add("微軟正黑體", "Microsoft JhengHei");
                fontLanguages.Add("楷体", "KaiTi");
                fontLanguages.Add("新宋体", "NSimSun");
                fontLanguages.Add("仿宋", "FangSong");
                fontLanguages.Add("標楷體", "BiauKai");
                fontLanguages.Add("新細明體", "PMingLiU");
                fontLanguages.Add("細明體", "MingLiU");
                //macos
                fontLanguages.Add("苹方", "PingFang SC");
                fontLanguages.Add("华文黑体", "STHeiti");
                fontLanguages.Add("华文楷体", "STKaiti");
                fontLanguages.Add("华文宋体", "STSong");
                fontLanguages.Add("华文仿宋", "STFangsong");
                fontLanguages.Add("华文中宋", "STZhongsong");
                fontLanguages.Add("华文琥珀", "STHupo");
                fontLanguages.Add("华文新魏", "STXinwei");
                fontLanguages.Add("华文隶书", "STLiti");
                fontLanguages.Add("华文行楷", "STXingkai");
                //open
                fontLanguages.Add("思源黑体", "Source Han Sans CN");
                fontLanguages.Add("思源宋体", "Source Han Serif SC");
                fontLanguages.Add("文泉驿微米黑", "WenQuanYi Micro Hei");
            }
        }
        //
        // 摘要:
        //     Initializes a new System.Drawing.FontFamily from the specified generic font family.
        //
        // 参数:
        //   genericFamily:
        //     The System.Drawing.Text.GenericFontFamilies from which to create the new System.Drawing.FontFamily.
        public FontFamily(GenericFontFamilies genericFamily) : this(genericFamily.ToString(), null)
        {
        }
        //
        // 摘要:
        //     Initializes a new System.Drawing.FontFamily with the specified name.
        //
        // 参数:
        //   name:
        //     The name of the new System.Drawing.FontFamily.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     name is an empty string (""). -or- name specifies a font that is not installed
        //     on the computer running the application. -or- name specifies a font that is not
        //     a TrueType font.
        public FontFamily(string name) : this(name, null) { }
        //
        // 摘要:
        //     Initializes a new System.Drawing.FontFamily in the specified System.Drawing.Text.FontCollection
        //     with the specified name.
        //
        // 参数:
        //   name:
        //     A System.String that represents the name of the new System.Drawing.FontFamily.
        //
        //   fontCollection:
        //     The System.Drawing.Text.FontCollection that contains this System.Drawing.FontFamily.
        //
        // 异常:
        //   T:System.ArgumentException:
        //     name is an empty string (""). -or- name specifies a font that is not installed
        //     on the computer running the application. -or- name specifies a font that is not
        //     a TrueType font.
        public FontFamily(string name, FontCollection fontCollection) {
            if (fontLanguages.TryGetValue(name, out string enname))
                this.Name = enname;
            else
                this.Name = name;
        }


        //
        // 摘要:
        //     Gets a generic sans serif System.Drawing.FontFamily object.
        //
        // 返回结果:
        //     A System.Drawing.FontFamily object that represents a generic sans serif font.
        public static FontFamily GenericSansSerif { get => new FontFamily(GenericFontFamilies.SansSerif); }
        //
        // 摘要:
        //     Gets a generic monospace System.Drawing.FontFamily.
        //
        // 返回结果:
        //     A System.Drawing.FontFamily that represents a generic monospace font.
        public static FontFamily GenericMonospace { get => new FontFamily(GenericFontFamilies.Monospace); }
        //
        // 摘要:
        //     Returns an array that contains all the System.Drawing.FontFamily objects associated
        //     with the current graphics context.
        //
        // 返回结果:
        //     An array of System.Drawing.FontFamily objects associated with the current graphics
        //     context.
        public static FontFamily[] Families { get => Array.ConvertAll(PangoHelper.ContextGet().Families, o => new FontFamily(o.Name)); }

        //
        // 摘要:
        //     Gets a generic serif System.Drawing.FontFamily.
        //
        // 返回结果:
        //     A System.Drawing.FontFamily that represents a generic serif font.
        public static FontFamily GenericSerif { get => new FontFamily(GenericFontFamilies.Serif); }
        //
        // 摘要:
        //     Gets the name of this System.Drawing.FontFamily.
        //
        // 返回结果:
        //     A System.String that represents the name of this System.Drawing.FontFamily.
        public string Name { get; set; }

        //
        // 摘要:
        //     Returns an array that contains all the System.Drawing.FontFamily objects available
        //     for the specified graphics context.
        //
        // 参数:
        //   graphics:
        //     The System.Drawing.Graphics object from which to return System.Drawing.FontFamily
        //     objects.
        //
        // 返回结果:
        //     An array of System.Drawing.FontFamily objects available for the specified System.Drawing.Graphics
        //     object.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     graphics is null.
        [Obsolete("Do not use method GetFamilies, use property Families instead")]
        public static FontFamily[] GetFamilies(Graphics graphics) { return new FontFamily[0]; }
        //
        // 摘要:
        //     Releases all resources used by this System.Drawing.FontFamily.
        public void Dispose() { }
        //
        // 摘要:
        //     Indicates whether the specified object is a System.Drawing.FontFamily and is
        //     identical to this System.Drawing.FontFamily.
        //
        // 参数:
        //   obj:
        //     The object to test.
        //
        // 返回结果:
        //     true if obj is a System.Drawing.FontFamily and is identical to this System.Drawing.FontFamily;
        //     otherwise, false.
        public override bool Equals(object obj) { return Equals(obj as FontFamily); }
        //
        // 摘要:
        //     Returns the cell ascent, in design units, of the System.Drawing.FontFamily of
        //     the specified style.
        //
        // 参数:
        //   style:
        //     A System.Drawing.FontStyle that contains style information for the font.
        //
        // 返回结果:
        //     The cell ascent for this System.Drawing.FontFamily that uses the specified System.Drawing.FontStyle.
        public int GetCellAscent(FontStyle style) { return 0; }
        //
        // 摘要:
        //     Returns the cell descent, in design units, of the System.Drawing.FontFamily of
        //     the specified style.
        //
        // 参数:
        //   style:
        //     A System.Drawing.FontStyle that contains style information for the font.
        //
        // 返回结果:
        //     The cell descent metric for this System.Drawing.FontFamily that uses the specified
        //     System.Drawing.FontStyle.
        public int GetCellDescent(FontStyle style) { return 0; }
        //
        // 摘要:
        //     Gets the height, in font design units, of the em square for the specified style.
        //
        // 参数:
        //   style:
        //     The System.Drawing.FontStyle for which to get the em height.
        //
        // 返回结果:
        //     The height of the em square.
        public int GetEmHeight(FontStyle style) { return 0; }
        //
        // 摘要:
        //     Gets a hash code for this System.Drawing.FontFamily.
        //
        // 返回结果:
        //     The hash code for this System.Drawing.FontFamily.
        public override int GetHashCode() { return Name.GetHashCode(); }
        //
        // 摘要:
        //     Returns the line spacing, in design units, of the System.Drawing.FontFamily of
        //     the specified style. The line spacing is the vertical distance between the base
        //     lines of two consecutive lines of text.
        //
        // 参数:
        //   style:
        //     The System.Drawing.FontStyle to apply.
        //
        // 返回结果:
        //     The distance between two consecutive lines of text.
        public int GetLineSpacing(FontStyle style) { return 0; }
        //
        // 摘要:
        //     Returns the name, in the specified language, of this System.Drawing.FontFamily.
        //
        // 参数:
        //   language:
        //     The language in which the name is returned.
        //
        // 返回结果:
        //     A System.String that represents the name, in the specified language, of this
        //     System.Drawing.FontFamily.
        public string GetName(int language) { return Name; }
        //
        // 摘要:
        //     Indicates whether the specified System.Drawing.FontStyle enumeration is available.
        //
        // 参数:
        //   style:
        //     The System.Drawing.FontStyle to test.
        //
        // 返回结果:
        //     true if the specified System.Drawing.FontStyle is available; otherwise, false.
        public bool IsStyleAvailable(FontStyle style) { return true; }
        //
        // 摘要:
        //     Converts this System.Drawing.FontFamily to a human-readable string representation.
        //
        // 返回结果:
        //     The string that represents this System.Drawing.FontFamily.
        public override string ToString()
        {
            return Name;
        }
    }
}