using System;
using System.Threading.Tasks;

namespace Derp.Sales.Messaging
{
    public class AdHocHandler<T> : Handles<T> where T : Message
    {
        private readonly Func<T, Task> handler;

        public AdHocHandler(Func<T, Task> handler)
        {
            this.handler = handler;
        }

        public AdHocHandler(Action<T> handler)
        {
            this.handler = message =>
            {
                var tcs = new TaskCompletionSource<object>();
                try
                {
                    handler(message);
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
                return tcs.Task;
            };
        }

        #region Handles<T> Members

        public Task Handle(T message)
        {
            return handler(message);
        }

        #endregion
    }
}
