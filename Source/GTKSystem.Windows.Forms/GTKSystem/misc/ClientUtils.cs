// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Security;

namespace System;

internal static class ClientUtils
{
    public static bool IsCriticalException(Exception ex)
        => ex is NullReferenceException
           || ex is StackOverflowException
           || ex is OutOfMemoryException
           || ex is ThreadAbortException
           || ex is IndexOutOfRangeException
           || ex is AccessViolationException;

    private enum CharType
    {
        None,
        Word,
        NonWord
    }

    /// <summary>
    ///  Imitates the backwards word selection logic of the native SHAutoComplete Ctrl+Backspace handler.
    ///  The selection will consist of any run of word characters and any run of non-word characters at the end of that word.
    ///  If the selection reaches the second character in the input, and the first character is non-word, it is also selected.
    ///  Here, word characters are equivalent to the "\w" regex class but with UnicodeCategory.ConnectorPunctuation excluded.
    /// </summary>
    public static int GetWordBoundaryStart(string text, int endIndex)
    {
        var seenWord = false;
        var lastSeen = CharType.None;
        var index = endIndex - 1;
        for (; index >= 0; index--)
        {
            var character = text[index];
            if (char.IsSurrogate(character))
            {
                break;
            }

            var isWord = char.IsLetterOrDigit(character) ||
                         CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark;
            if ((isWord && lastSeen == CharType.NonWord && seenWord) ||
                (!isWord && lastSeen == CharType.Word && index != 0))
            {
                break;
            }

            seenWord |= isWord;
            lastSeen = isWord ? CharType.Word : CharType.NonWord;
        }

        return index + 1;
    }

    public static bool IsSecurityOrCriticalException(Exception ex)
    {
        if (ex is SecurityException)
        {
            return true;
        }
        return IsCriticalException(ex);
    }
}