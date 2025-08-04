// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    /// <summary>
    ///  Translates between WinForms text-based <see cref="Clipboard"/>
    ///  formats and Win32 32-bit signed integer-based clipboard
    ///  formats. Provides <see langword="static"/> methods to create new
    /// <see cref="Clipboard"/> formats and add them to the Windows Registry.
    /// </summary>
    public static partial class DataFormats
    {
        internal const string TextConstant = "Text";
        internal const string UnicodeTextConstant = "UnicodeText";
        internal const string DibConstant = "DeviceIndependentBitmap";
        internal const string BitmapConstant = "Bitmap";
        internal const string EmfConstant = "EnhancedMetafile";
        internal const string WmfConstant = "MetaFilePict";
        internal const string SymbolicLinkConstant = "SymbolicLink";
        internal const string DifConstant = "DataInterchangeFormat";
        internal const string TiffConstant = "TaggedImageFileFormat";
        internal const string OemTextConstant = "OEMText";
        internal const string PaletteConstant = "Palette";
        internal const string PenDataConstant = "PenData";
        internal const string RiffConstant = "RiffAudio";
        internal const string WaveAudioConstant = "WaveAudio";
        internal const string FileDropConstant = "FileDrop";
        internal const string LocaleConstant = "Locale";
        internal const string HtmlConstant = "HTML Format";
        internal const string RtfConstant = "Rich Text Format";
        internal const string CsvConstant = "Csv";
        internal const string StringConstant = "System.String";
        internal const string SerializableConstant = "WindowsForms10PersistentObject";

        /// <summary>
        ///  Specifies the standard ANSI text format.
        /// </summary>
        public static readonly string Text = TextConstant;

        /// <summary>
        ///  Specifies the standard Windows Unicode text format.
        /// </summary>
        public static readonly string UnicodeText = UnicodeTextConstant;

        /// <summary>
        ///  Specifies the Windows Device Independent Bitmap (DIB) format.
        /// </summary>
        public static readonly string Dib = DibConstant;

        /// <summary>
        ///  Specifies a Windows bitmap format.
        /// </summary>
        public static readonly string Bitmap = BitmapConstant;

        /// <summary>
        ///  Specifies the Windows enhanced metafile format.
        /// </summary>
        public static readonly string EnhancedMetafile = EmfConstant;

        /// <summary>
        ///  Specifies the Windows metafile format, which WinForms does not directly use.
        /// </summary>
        public static readonly string MetafilePict = WmfConstant;

        /// <summary>
        ///  Specifies the Windows symbolic link format, which WinForms does not directly use.
        /// </summary>
        public static readonly string SymbolicLink = SymbolicLinkConstant;

        /// <summary>
        ///  Specifies the Windows data interchange format, which WinForms does not directly use.
        /// </summary>
        public static readonly string Dif = DifConstant;

        /// <summary>
        ///  Specifies the Tagged Image File Format (TIFF), which WinForms does not directly use.
        /// </summary>
        public static readonly string Tiff = TiffConstant;

        /// <summary>
        ///  Specifies the standard Windows original equipment manufacturer (OEM) text format.
        /// </summary>
        public static readonly string OemText = OemTextConstant;

        /// <summary>
        ///  Specifies the Windows palette format.
        /// </summary>
        public static readonly string Palette = PaletteConstant;

        /// <summary>
        ///  Specifies the Windows pen data format, which consists of pen strokes for handwriting
        ///  software; WinForms does not use this format.
        /// </summary>
        public static readonly string PenData = PenDataConstant;

        /// <summary>
        ///  Specifies the Resource Interchange File Format (RIFF) audio format, which WinForms does not directly use.
        /// </summary>
        public static readonly string Riff = RiffConstant;

        /// <summary>
        ///  Specifies the wave audio format, which Win Forms does not directly use.
        /// </summary>
        public static readonly string WaveAudio = WaveAudioConstant;

        /// <summary>
        ///  Specifies the Windows file drop format, which WinForms does not directly use.
        /// </summary>
        public static readonly string FileDrop = FileDropConstant;

        /// <summary>
        ///  Specifies the Windows culture format, which WinForms does not directly use.
        /// </summary>
        public static readonly string Locale = LocaleConstant;

        /// <summary>
        ///  Specifies text consisting of HTML data.
        /// </summary>
        public static readonly string Html = HtmlConstant;

        /// <summary>
        ///  Specifies text consisting of Rich Text Format (RTF) data.
        /// </summary>
        public static readonly string Rtf = RtfConstant;

        /// <summary>
        ///  Specifies a comma-separated value (CSV) format, which is a common interchange format
        ///  used by spreadsheets. This format is not used directly by WinForms.
        /// </summary>
        public static readonly string CommaSeparatedValue = CsvConstant;

        /// <summary>
        ///  Specifies the WinForms string class format, which WinForms uses to store string objects.
        /// </summary>
        public static readonly string StringFormat = StringConstant;

        /// <summary>
        ///  Specifies a format that encapsulates any type of WinForms object.
        /// </summary>
        public static readonly string Serializable = SerializableConstant;
    }
}
