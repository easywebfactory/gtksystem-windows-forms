using System.Threading;

namespace System.Windows.Forms
{
    public class AsyncResult : IAsyncResult
    {
        public object AsyncState => throw new NotImplementedException();

        public WaitHandle AsyncWaitHandle => throw new NotImplementedException();

        public bool CompletedSynchronously => throw new NotImplementedException();

        public bool IsCompleted => throw new NotImplementedException();
    }
}