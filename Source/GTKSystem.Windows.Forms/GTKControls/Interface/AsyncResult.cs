// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;

namespace System.Runtime.Remoting.Messaging
//namespace System
{
    public class AsyncResult : IAsyncResult
    {
        public object AsyncState { get; set; }

        public WaitHandle AsyncWaitHandle => new EventWaitHandle(false, EventResetMode.AutoReset);

        public bool CompletedSynchronously { get; set; }

        public bool IsCompleted { get; set; }
    }
}
