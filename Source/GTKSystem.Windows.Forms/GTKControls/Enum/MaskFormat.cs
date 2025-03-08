using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Forms
{
    public enum MaskFormat
    {
        //
        // 摘要:
        //     Return only text input by the user.
        ExcludePromptAndLiterals = 0,
        //
        // 摘要:
        //     Return text input by the user as well as any instances of the prompt character.
        IncludePrompt = 1,
        //
        // 摘要:
        //     Return text input by the user as well as any literal characters defined in the
        //     mask.
        IncludeLiterals = 2,
        //
        // 摘要:
        //     Return text input by the user as well as any literal characters defined in the
        //     mask and any instances of the prompt character.
        IncludePromptAndLiterals = 3
    }
}
