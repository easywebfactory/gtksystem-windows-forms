// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


namespace System.Drawing.Printing;

/// <summary>
///  Specifies the option buttons in the print dialog box that designate the part of the document to print.
/// </summary>
public enum PrintRange
{
    /// <summary>
    ///  All pages are printed.
    /// </summary>
    AllPages = 0,

    /// <summary>
    ///  The pages between <see cref='PrinterSettings.FromPage'/> and <see cref='PrinterSettings.ToPage'/> are printed.
    /// </summary>
    SomePages = 1,

    /// <summary>
    ///  The selected pages are printed.
    /// </summary>
    Selection = 2,

    /// <summary>
    ///  The current page is printed. The print dialog box requires Windows 2000 or later for this setting; if used
    ///  with an earlier operating system, all pages will be printed.
    /// </summary>
    CurrentPage = 3,
}