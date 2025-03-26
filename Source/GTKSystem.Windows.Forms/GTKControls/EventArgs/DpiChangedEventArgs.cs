// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms;

/// <summary>
///  Provides information about a DpiChanged event.
/// </summary>
public sealed class DpiChangedEventArgs : CancelEventArgs
{
    /// <devdov>
    ///  Parameter units are pixels(dots) per inch.
    /// </summary>
    internal DpiChangedEventArgs(int old, Message m)
    {

    }

    public int DeviceDpiOld { get; internal set; }

    public int DeviceDpiNew { get; internal set; }

    public Rectangle SuggestedRectangle { get; internal set; }

    public override string ToString() => $"was: {DeviceDpiOld}, now: {DeviceDpiNew}";
}