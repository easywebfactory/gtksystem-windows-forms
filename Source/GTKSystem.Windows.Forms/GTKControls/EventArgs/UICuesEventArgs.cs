// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Windows.Forms;

/// <summary>
///  Provides data for the <see cref='Control.ChangeUiCues'/> event.
/// </summary>
public class UiCuesEventArgs : EventArgs
{
    private readonly UiCues _uicues;

    public UiCuesEventArgs(UiCues uicues)
    {
        _uicues = uicues;
    }

    /// <summary>
    ///  Focus rectangles are shown after the change.
    /// </summary>
    public bool ShowFocus => (_uicues & UiCues.ShowFocus) != 0;

    /// <summary>
    ///  Keyboard cues are underlined after the change.
    /// </summary>
    public bool ShowKeyboard => (_uicues & UiCues.ShowKeyboard) != 0;

    /// <summary>
    ///  The state of the focus cues has changed.
    /// </summary>
    public bool ChangeFocus => (_uicues & UiCues.ChangeFocus) != 0;

    /// <summary>
    ///  The state of the keyboard cues has changed.
    /// </summary>
    public bool ChangeKeyboard => (_uicues & UiCues.ChangeKeyboard) != 0;

    public UiCues Changed => _uicues & UiCues.Changed;
}