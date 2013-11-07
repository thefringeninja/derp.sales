using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Derp.Sales.Messaging
{
    public class QueuedHandler<T> : Handles<T> where T : Message
    {
        private readonly Handles<T> handler;
        private readonly ConcurrentQueue<T> queue;
        private readonly Thread workerThread;
        private volatile bool stop;

        public int Count
        {
            get { return queue.Count; }
        }

        private void HandleNextMessage(object _)
        {
            while (false == stop)
            {
                T message;
                if (queue.TryDequeue(out message))
                {
                    handler.Handle(message).Wait();
                    continue;
                }
                Thread.Sleep(1);
            }
        }

        public QueuedHandler(Handles<T> handler)
        {
            this.handler = handler;
            queue = new ConcurrentQueue<T>();
            workerThread = new Thread(HandleNextMessage);
        }

        public Task Handle(T message)
        {
            queue.Enqueue(message);
            return Task.FromResult(true);
        }

        public void Start()
        {
            workerThread.Start();
        }

        public void Stop()
        {
            stop = true;
        }

        public override string ToString()
        {
            return handler + " has " + Count + " items left";
        }
    }
}