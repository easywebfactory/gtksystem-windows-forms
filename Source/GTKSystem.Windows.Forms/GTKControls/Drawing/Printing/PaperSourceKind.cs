// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Drawing.Printing
{
    /// <summary>
    ///  Standard paper sources.
    /// </summary>
    public enum PaperSourceKind
    {
        /// <summary>
        ///  The upper bin of a printer (or, if the printer only has one bin, the only bin).
        /// </summary>
        Upper = 0,

        /// <summary>
        ///  The lower bin of a printer.
        /// </summary>
        Lower = 1,

        /// <summary>
        ///  The middle bin of a printer.
        /// </summary>
        Middle = 2,

        /// <summary>
        ///  Manually-fed paper.
        /// </summary>
        Manual = 3,

        /// <summary>
        ///  An envelope.
        /// </summary>
        Envelope = 4,

        /// <summary>
        ///  A manually-fed envelope.
        /// </summary>
        ManualFeed = 5,

        /// <summary>
        ///  Automatic-fed paper.
        /// </summary>
        AutomaticFeed = 6,

        /// <summary>
        ///  A tractor feed.
        /// </summary>
        TractorFeed = 7,

        /// <summary>
        ///  Small-format paper.
        /// </summary>
        SmallFormat = 8,

        /// <summary>
        ///  Large-format paper.
        /// </summary>
        LargeFormat = 9,

        /// <summary>
        ///  A large-capacity bin printer.
        /// </summary>
        LargeCapacity = 10,

        /// <summary>
        ///  A paper cassette.
        /// </summary>
        Cassette = 11,

        FormSource = 12,

        /// <summary>
        ///  A printer-specific paper source.
        /// </summary>
        Custom = 13,
    }
}