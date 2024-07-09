// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Drawing.Printing
{
    /// <summary>
    ///  Specifies a printer resolution.
    /// </summary>
    public enum PrinterResolutionKind
    {
        /// <summary>
        ///  High resolution.
        /// </summary>
        High = 1,

        /// <summary>
        ///  Medium resolution.
        /// </summary>
        Medium = 2,

        /// <summary>
        ///  Low resolution.
        /// </summary>
        Low = 3,

        /// <summary>
        ///  Draft-quality resolution.
        /// </summary>
        Draft = 4,

        /// <summary>
        ///  Custom resolution.
        /// </summary>
        Custom = 0,
    }
}