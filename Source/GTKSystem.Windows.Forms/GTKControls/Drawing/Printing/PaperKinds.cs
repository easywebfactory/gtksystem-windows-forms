// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Drawing.Printing
{
    /// <summary>
    ///  Specifies the standard paper sizes.
    /// </summary>
    public enum PaperKind
    {
        /// <summary>
        ///  The paper size is defined by the user.
        /// </summary>
        Custom = 0,

        /// <summary>
        ///  Letter paper (8.5 in. by 11 in.).
        /// </summary>
        Letter = 1,

        /// <summary>
        ///  Legal paper (8.5 in. by 14 in.).
        /// </summary>
        Legal = 2,

        /// <summary>
        ///  A4 paper (210 mm by 297 mm).
        /// </summary>
        A4 = 3,

        /// <summary>
        ///  C paper (17 in. by 22 in.).
        /// </summary>
        CSheet = 4,

        /// <summary>
        ///  D paper (22 in. by 34 in.).
        /// </summary>
        DSheet = 5,

        /// <summary>
        ///  E paper (34 in. by 44 in.).
        /// </summary>
        ESheet = 6,

        /// <summary>
        ///  Letter small paper (8.5 in. by 11 in.).
        /// </summary>
        LetterSmall = 7,

        /// <summary>
        ///  Tabloid paper (11 in. by 17 in.).
        /// </summary>
        Tabloid = 8,

        /// <summary>
        ///  Ledger paper (17 in. by 11 in.).
        /// </summary>
        Ledger = 9,

        /// <summary>
        ///  Statement paper (5.5 in. by 8.5 in.).
        /// </summary>
        Statement = 10,

        /// <summary>
        ///  Executive paper (7.25 in. by 10.5 in.).
        /// </summary>
        Executive = 11,

        /// <summary>
        ///  A3 paper (297 mm by 420 mm).
        /// </summary>
        A3 = 12,

        /// <summary>
        ///  A4 small paper (210 mm by 297 mm).
        /// </summary>
        A4Small = 13,

        /// <summary>
        ///  A5 paper (148 mm by 210 mm).
        /// </summary>
        A5 = 14,

        /// <summary>
        ///  B4 paper (250 mm by 353 mm).
        /// </summary>
        B4 = 15,

        /// <summary>
        ///  B5 paper (176 mm by 250 mm).
        /// </summary>
        B5 = 16,

        /// <summary>
        ///  Folio paper (8.5 in. by 13 in.).
        /// </summary>
        Folio = 17,

        /// <summary>
        ///  Quarto paper (215 mm by 275 mm).
        /// </summary>
        Quarto = 18,

        /// <summary>
        ///  10-by-14-inch paper.
        /// </summary>
        Standard10x14 = 19,

        /// <summary>
        ///  11-by-17-inch paper.
        /// </summary>
        Standard11x17 = 20,

        /// <summary>
        ///  Note paper (8.5 in. by 11 in.).
        /// </summary>
        Note = 21,

        /// <summary>
        ///  #9 envelope (3.875 in. by 8.875 in.).
        /// </summary>
        Number9Envelope = 22,

        /// <summary>
        ///  #10 envelope (4.125 in. by 9.5 in.).
        /// </summary>
        Number10Envelope = 23,

        /// <summary>
        ///  #11 envelope (4.5 in. by 10.375 in.).
        /// </summary>
        Number11Envelope = 24,

        /// <summary>
        ///  #12 envelope (4.75 in. by 11 in.).
        /// </summary>
        Number12Envelope = 25,

        /// <summary>
        ///  #14 envelope (5 in. by 11.5 in.).
        /// </summary>
        Number14Envelope = 26,

        /// <summary>
        ///  DL envelope (110 mm by 220 mm).
        /// </summary>
        DLEnvelope = 27,

        /// <summary>
        ///  C5 envelope (162 mm by 229 mm).
        /// </summary>
        C5Envelope = 28,

        /// <summary>
        ///  C3 envelope (324 mm by 458 mm).
        /// </summary>
        C3Envelope = 29,

        /// <summary>
        ///  C4 envelope (229 mm by 324 mm).
        /// </summary>
        C4Envelope = 30,

        /// <summary>
        ///  C6 envelope (114 mm by 162 mm).
        /// </summary>
        C6Envelope = 31,

        /// <summary>
        ///  C65 envelope (114 mm by 229 mm).
        /// </summary>
        C65Envelope = 32,

        /// <summary>
        ///  B4 envelope (250 mm by 353 mm).
        /// </summary>
        B4Envelope = 33,

        /// <summary>
        ///  B5 envelope (176 mm by 250 mm).
        /// </summary>
        B5Envelope = 34,

        /// <summary>
        ///  B6 envelope (176 mm by 125 mm).
        /// </summary>
        B6Envelope = 35,

        /// <summary>
        ///  Italy envelope (110 mm by 230 mm).
        /// </summary>
        ItalyEnvelope = 36,

        /// <summary>
        ///  Monarch envelope (3.875 in. by 7.5 in.).
        /// </summary>
        MonarchEnvelope = 37,

        /// <summary>
        ///  6 3/4 envelope (3.625 in. by 6.5 in.).
        /// </summary>
        PersonalEnvelope = 38,

        /// <summary>
        ///  US standard fanfold (14.875 in. by 11 in.).
        /// </summary>
        USStandardFanfold = 39,

        /// <summary>
        ///  German standard fanfold (8.5 in. by 12 in.).
        /// </summary>
        GermanStandardFanfold = 40,

        /// <summary>
        ///  German legal fanfold (8.5 in. by 13 in.).
        /// </summary>
        GermanLegalFanfold = 41,

        /// <summary>
        ///  ISO B4 (250 mm by 353 mm).
        /// </summary>
        IsoB4 = 42,

        /// <summary>
        ///  Japanese postcard (100 mm by 148 mm).
        /// </summary>
        JapanesePostcard = 43,

        /// <summary>
        ///  9-by-11-inch paper.
        /// </summary>
        Standard9x11 = 44,

        /// <summary>
        ///  10-by-11-inch paper.
        /// </summary>
        Standard10x11 = 45,

        /// <summary>
        ///  15-by-11-inch paper.
        /// </summary>
        Standard15x11 = 46,

        /// <summary>
        ///  Invite envelope (220 mm by 220 mm).
        /// </summary>
        InviteEnvelope = 47,

        /// <summary>
        ///  Letter extra paper (9.275 in. by 12 in.).
        ///  This value is specific to the PostScript driver and is used only by Linotronic printers in order to conserve paper.
        /// </summary>
        LetterExtra = 48,

        /// <summary>
        ///  Legal extra paper (9.275 in. by 15 in.).
        ///  This value is specific to the PostScript driver and is used only by Linotronic printers in order to conserve paper.
        /// </summary>
        LegalExtra = 49,

        /// <summary>
        ///  Tabloid extra paper (11.69 in. by 18 in.).
        ///  This value is specific to the PostScript driver and is used only by Linotronic printers in order to conserve paper.
        /// </summary>
        TabloidExtra = 50,

        /// <summary>
        ///  A4 extra paper (236 mm by 322 mm).
        ///  This value is specific to the PostScript driver and is used only by Linotronic printers in order to conserve paper.
        /// </summary>
        A4Extra = 51,

        /// <summary>
        ///  Letter transverse paper (8.275 in. by 11 in.).
        /// </summary>
        LetterTransverse = 52,

        /// <summary>
        ///  A4 transverse paper (210 mm by 297 mm).
        /// </summary>
        A4Transverse = 53,

        /// <summary>
        ///  Letter extra transverse paper (9.275 in. by 12 in.).
        /// </summary>
        LetterExtraTransverse = 54,

        /// <summary>
        ///  SuperA/SuperA/A4 paper (227 mm by 356 mm).
        /// </summary>
        APlus = 55,

        /// <summary>
        ///  SuperB/SuperB/A3 paper (305 mm by 487 mm).
        /// </summary>
        BPlus = 56,

        /// <summary>
        ///  Letter plus paper (8.5 in. by 12.69 in.).
        /// </summary>
        LetterPlus = 57,

        /// <summary>
        ///  A4 plus paper (210 mm by 330 mm).
        /// </summary>
        A4Plus = 58,

        /// <summary>
        ///  A5 transverse paper (148 mm by 210 mm).
        /// </summary>
        A5Transverse = 59,

        /// <summary>
        ///  JIS B5 transverse paper (182 mm by 257 mm).
        /// </summary>
        B5Transverse = 60,

        /// <summary>
        ///  A3 extra paper (322 mm by 445 mm).
        /// </summary>
        A3Extra = 61,

        /// <summary>
        ///  A5 extra paper (174 mm by 235 mm).
        /// </summary>
        A5Extra = 62,

        /// <summary>
        ///  ISO B5 extra paper (201 mm by 276 mm).
        /// </summary>
        B5Extra = 63,

        /// <summary>
        ///  A2 paper (420 mm by 594 mm).
        /// </summary>
        A2 = 64,

        /// <summary>
        ///  A3 transverse paper (297 mm by 420 mm).
        /// </summary>
        A3Transverse = 65,

        /// <summary>
        ///  A3 extra transverse paper (322 mm by 445 mm).
        /// </summary>
        A3ExtraTransverse = 66,

        /// <summary>
        ///  Japanese double postcard (200 mm by 148mm).
        /// </summary>
        JapaneseDoublePostcard = 67,

        /// <summary>
        ///  A6 paper (105 mm by 148 mm).
        /// </summary>
        A6 = 68,

        /// <summary>
        ///  Japanese Kaku #2 envelope.
        /// </summary>
        JapaneseEnvelopeKakuNumber2 = 69,

        /// <summary>
        ///  Japanese Kaku #3 envelope.
        /// </summary>
        JapaneseEnvelopeKakuNumber3 = 70,

        /// <summary>
        ///  Japanese Chou #3 envelope.
        /// </summary>
        JapaneseEnvelopeChouNumber3 = 71,

        /// <summary>
        ///  Japanese Chou #4 envelope.
        /// </summary>
        JapaneseEnvelopeChouNumber4 = 72,

        /// <summary>
        ///  Letter rotated paper (11 in. by 8.5 in.).
        /// </summary>
        LetterRotated = 73,

        /// <summary>
        ///  A3 rotated paper (420mm by 297 mm).
        /// </summary>
        A3Rotated = 74,

        /// <summary>
        ///  A4 rotated paper (297 mm by 210 mm).
        /// </summary>
        A4Rotated = 75,

        /// <summary>
        ///  A5 rotated paper (210 mm by 148 mm).
        /// </summary>
        A5Rotated = 76,

        /// <summary>
        ///  JIS B4 rotated paper (364 mm by 257 mm).
        /// </summary>
        B4JisRotated = 77,

        /// <summary>
        ///  JIS B5 rotated paper (257 mm by 182 mm).
        /// </summary>
        B5JisRotated = 78,

        /// <summary>
        ///  Japanese rotated postcard (148 mm by 100 mm).
        /// </summary>
        JapanesePostcardRotated = 79,

        /// <summary>
        ///  Japanese rotated double postcard (148 mm by 200 mm).
        /// </summary>
        JapaneseDoublePostcardRotated = 80,

        /// <summary>
        ///  A6 rotated paper (148 mm by 105 mm).
        /// </summary>
        A6Rotated = 81,

        /// <summary>
        ///  Japanese rotated Kaku #2 envelope.
        /// </summary>
        JapaneseEnvelopeKakuNumber2Rotated = 82,

        /// <summary>
        ///  Japanese rotated Kaku #3 envelope.
        /// </summary>
        JapaneseEnvelopeKakuNumber3Rotated = 83,

        /// <summary>
        ///  Japanese rotated Chou #3 envelope.
        /// </summary>
        JapaneseEnvelopeChouNumber3Rotated = 84,

        /// <summary>
        ///  Japanese rotated Chou #4 envelope.
        /// </summary>
        JapaneseEnvelopeChouNumber4Rotated = 85,

        /// <summary>
        ///  JIS B6 paper (128 mm by 182 mm).
        /// </summary>
        B6Jis = 86,

        /// <summary>
        ///  JIS B6 rotated paper (182 mm by 128 mm).
        /// </summary>
        B6JisRotated = 87,

        /// <summary>
        ///  12-by-11-inch paper.
        /// </summary>
        Standard12x11 = 88,

        /// <summary>
        ///  Japanese You #4 envelope.
        /// </summary>
        JapaneseEnvelopeYouNumber4 = 89,

        /// <summary>
        ///  Japanese You #4 rotated envelope.
        /// </summary>
        JapaneseEnvelopeYouNumber4Rotated = 90,

        /// <summary>
        ///  PRC 16K paper (146 mm by 215 mm).
        /// </summary>
        Prc16K = 91,

        /// <summary>
        ///  PRC 32K paper (97 mm by 151 mm).
        /// </summary>
        Prc32K = 92,

        /// <summary>
        ///  PRC 32K big paper (97 mm by 151 mm).
        /// </summary>
        Prc32KBig = 93,

        /// <summary>
        ///  PRC #1 envelope (102 mm by 165 mm).
        /// </summary>
        PrcEnvelopeNumber1 = 94,

        /// <summary>
        ///  PRC #2 envelope (102 mm by 176 mm).
        /// </summary>
        PrcEnvelopeNumber2 = 95,

        /// <summary>
        ///  PRC #3 envelope (125 mm by 176 mm).
        /// </summary>
        PrcEnvelopeNumber3 = 96,

        /// <summary>
        ///  PRC #4 envelope (110 mm by 208 mm).
        /// </summary>
        PrcEnvelopeNumber4 = 97,

        /// <summary>
        ///  PRC #5 envelope (110 mm by 220 mm).
        /// </summary>
        PrcEnvelopeNumber5 = 98,

        /// <summary>
        ///  PRC #6 envelope (120 mm by 230 mm).
        /// </summary>
        PrcEnvelopeNumber6 = 99,

        /// <summary>
        ///  PRC #7 envelope (160 mm by 230 mm).
        /// </summary>
        PrcEnvelopeNumber7 = 100,

        /// <summary>
        ///  PRC #8 envelope (120 mm by 309 mm).
        /// </summary>
        PrcEnvelopeNumber8 = 101,

        /// <summary>
        ///  PRC #9 envelope (229 mm by 324 mm).
        /// </summary>
        PrcEnvelopeNumber9 = 102,

        /// <summary>
        ///  PRC #10 envelope (324 mm by 458 mm).
        /// </summary>
        PrcEnvelopeNumber10 = 103,

        /// <summary>
        ///  PRC 16K rotated paper (146 mm by 215 mm).
        /// </summary>
        Prc16KRotated = 104,

        /// <summary>
        ///  PRC 32K rotated paper (97 mm by 151 mm).
        /// </summary>
        Prc32KRotated = 105,

        /// <summary>
        ///  PRC 32K big rotated paper (97 mm by 151 mm).
        /// </summary>
        Prc32KBigRotated = 106,

        /// <summary>
        ///  PRC #1 rotated envelope (165 mm by 102 mm).
        /// </summary>
        PrcEnvelopeNumber1Rotated = 107,

        /// <summary>
        ///  PRC #2 rotated envelope (176 mm by 102 mm).
        /// </summary>
        PrcEnvelopeNumber2Rotated = 108,

        /// <summary>
        ///  PRC #3 rotated envelope (176 mm by 125 mm).
        /// </summary>
        PrcEnvelopeNumber3Rotated = 109,

        /// <summary>
        ///  PRC #4 rotated envelope (208 mm by 110 mm).
        /// </summary>
        PrcEnvelopeNumber4Rotated = 110,

        /// <summary>
        ///  PRC #5 rotated envelope (220 mm by 110 mm).
        /// </summary>
        PrcEnvelopeNumber5Rotated = 111,

        /// <summary>
        ///  PRC #6 rotated envelope (230 mm by 120 mm).
        /// </summary>
        PrcEnvelopeNumber6Rotated = 112,

        /// <summary>
        ///  PRC #7 rotated envelope (230 mm by 160 mm).
        /// </summary>
        PrcEnvelopeNumber7Rotated = 113,

        /// <summary>
        ///  PRC #8 rotated envelope (309 mm by 120 mm).
        /// </summary>
        PrcEnvelopeNumber8Rotated = 114,

        /// <summary>
        ///  PRC #9 rotated envelope (324 mm by 229 mm).
        /// </summary>
        PrcEnvelopeNumber9Rotated = 115,

        /// <summary>
        ///  PRC #10 rotated envelope (458 mm by 324 mm).
        /// </summary>
        PrcEnvelopeNumber10Rotated = 116,
    }
}