//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

// This class needs to function even if it was built retail.  That is, a debug caller calling against a retail
// build of this assembly should still have asserts fire.  To achieve that, we need to define DEBUG here.
// We do not do the registry override in retail because that would require shipping a test hook.  We
// do not generally ship test hooks today.
#if DEBUG
#define DEBUG_FOR_REALS
#else
#define DEBUG
#endif

namespace System.Runtime;

using System;
using System.Diagnostics;
using System.Security;
using System.Runtime.Versioning;


static class AssertHelper
{
    [ResourceConsumption(ResourceScope.Process)]
    internal static void FireAssert(string message)
    {
        try
        {
#if DEBUG_FOR_REALS
            InternalFireAssert(ref message);
#endif
        }
        finally
        {
            Debug.Assert(false, message);
        }
    }

#if DEBUG_FOR_REALS
    [SecuritySafeCritical]
    static void InternalFireAssert(ref string message)
    {
        try
        {
            string debugMessage = "Assert fired! --> " + message + "\r\n";
            if (Debugger.IsAttached)
            {
                Debugger.Log(0, Debugger.DefaultCategory, debugMessage);
                Debugger.Break();
            }
            Environment.FailFast(message);
        }
        catch (Exception exception)
        {
            string newMessage = "Exception during FireAssert!";
            try
            {
                newMessage = string.Concat(newMessage, " [", exception.GetType().Name, ": ", exception.Message, "] --> ", message);
            }
            finally
            {
                message = newMessage;
            }
            throw;
        }
    }
#endif
}